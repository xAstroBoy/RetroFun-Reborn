using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Xabbo.Core.GameData
{
    /// <summary>
    /// Manages the game data for a specified hotel.
    /// </summary>
    public class GameDataManager : IGameDataManager
    {
        public static GameDataManager Instance { get; private set; }
        private readonly ILogger _logger;
        private readonly IGameDataLoader _loader;

        private readonly SemaphoreSlim _loadSemaphore = new(1, 1);
        private TaskCompletionSource? _tcsLoad;
        private Task _loadTask;

        public const string Path_Figure = "FigureData.json";
        public const string Path_Furni = "FurnitureData.json";
        public const string Path_Texts = "ExternalTexts.json";
        public const string Path_Variables = "ExternalVariables.json";
        public const string Path_Products = "ProductData.json";

        public const string BSS_URL_Figure = "https://images.bsshotel.it/gamedata/nitro/FigureData.json";
        public const string BSS_URL_Furni = "https://images.bsshotel.it/gamedata/nitro/FurnitureData.json";
        public const string BSS_URL_Texts = "https://images.bsshotel.it/gamedata/nitro/ExternalTexts.json";
        public const string BSS_URL_Variables = ""; // If available, set here
        public const string BSS_URL_Products = "https://images.bsshotel.it/gamedata/nitro/ProductData.json";

        public FigureData? Figure { get; private set; }
        public FurniData? Furni { get; private set; }
        public ProductData? Products { get; private set; }
        public ExternalTexts? Texts { get; private set; }
        public ExternalVariables? Variables { get; private set; }

        public event Action? Loading;
        public event Action? Loaded;
        public event Action<Exception>? LoadFailed;

        public bool AutoInitCoreExtensions { get; set; } = true;

        public GameDataManager(
            IGameDataLoader? loader = null,
            ILoggerFactory? loggerFactory = null)
        {
            _loader = loader ?? new GameDataLoader(loggerFactory: loggerFactory);
            _logger = (ILogger?)loggerFactory?.CreateLogger<GameDataManager>() ?? NullLogger.Instance;

            _tcsLoad = new(TaskCreationOptions.RunContinuationsAsynchronously);
            _loadTask = _tcsLoad.Task;
            Instance = this;
        }

        private void Reset()
        {
            Figure = null;
            Furni = null;
            Products = null;
            Texts = null;
            Variables = null;
        }

        /// <summary>
        /// Loads the specified game data types.
        /// If no types are specified, then all types are loaded.
        /// </summary>
        /// <param name="hotel">The hotel to load game data for.</param>
        /// <param name="typesToLoad">The game data types to load.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <exception cref="InvalidOperationException">If game data is currently being loaded.</exception>
        public async Task LoadAsync(Hotel hotel, GameDataType[]? typesToLoad = null, CancellationToken cancellationToken = default)
        {
            if (!_loadSemaphore.Wait(0, cancellationToken))
                throw new InvalidOperationException("Game data is currently being loaded.");

            try
            {
                _logger.LogInformation("Loading game data for {Hotel}.", hotel.Name);

                if (_tcsLoad is null)
                {
                    _tcsLoad = new(TaskCreationOptions.RunContinuationsAsynchronously);
                    _loadTask = _tcsLoad.Task;
                }

                Reset();
                Loading?.Invoke();

                _logger.LogDebug("Downloading JSON files from remote endpoints...");
                await DownloadAllGameDataJsonAsync(cancellationToken);

                _logger.LogDebug("Loading Files from JSON...");

                Figure = FigureData.Load(Path_Figure);
                Furni = FurniData.Load(Path_Furni);
                Texts = ExternalTexts.Load(Path_Texts);
                // If ExternalVariables is supported by your code, handle similarly:
                // Variables = ExternalVariables.Load(Path_Variables);
                Variables = null;
                Products = ProductData.Load(Path_Products);

                if (AutoInitCoreExtensions)
                {
                    _logger.LogInformation("Initializing core extensions with {Hotel} hotel game data.", hotel.Name);
                    Extensions.Initialize(Furni);
                }

                _logger.LogInformation("Game data loaded.");
                Loaded?.Invoke();
                _tcsLoad.TrySetResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load game data: {Message}", ex.Message);

                Reset();
                LoadFailed?.Invoke(ex);
                _tcsLoad?.TrySetException(ex);
            }
            finally
            {
                _tcsLoad = null;
                _loadSemaphore.Release();
            }
        }

        /// <summary>
        /// Downloads all required JSON files and saves them locally, pretty-printed.
        /// </summary>
        private async Task DownloadAllGameDataJsonAsync(CancellationToken cancellationToken)
        {
            // Setup HttpClient with cookies and user-agent
            using var httpClient = CreateHttpClientWithCookies();

            // Download each file if the URL is set
            if (!string.IsNullOrWhiteSpace(BSS_URL_Figure))
                await DownloadAndSaveJsonAsync(httpClient, BSS_URL_Figure, Path_Figure, cancellationToken);

            if (!string.IsNullOrWhiteSpace(BSS_URL_Furni))
                await DownloadAndSaveJsonAsync(httpClient, BSS_URL_Furni, Path_Furni, cancellationToken);

            if (!string.IsNullOrWhiteSpace(BSS_URL_Texts))
                await DownloadAndSaveJsonAsync(httpClient, BSS_URL_Texts, Path_Texts, cancellationToken);

            if (!string.IsNullOrWhiteSpace(BSS_URL_Products))
                await DownloadAndSaveJsonAsync(httpClient, BSS_URL_Products, Path_Products, cancellationToken);

            // If BSS_URL_Variables is needed and available:
            if (!string.IsNullOrWhiteSpace(BSS_URL_Variables))
                await DownloadAndSaveJsonAsync(httpClient, BSS_URL_Variables, Path_Variables, cancellationToken);
        }

        private async Task DownloadAndSaveJsonAsync(HttpClient httpClient, string url, string filePath, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Downloading {Url}...", url);

            using var response = await httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Failed to download {Url}. Status code: {StatusCode}", url, response.StatusCode);
                return;
            }

            string json = await response.Content.ReadAsStringAsync(cancellationToken);

            // Attempt to parse & pretty-print the JSON
            try
            {
                using var doc = JsonDocument.Parse(json);
                var prettyJson = JsonSerializer.Serialize(doc.RootElement, new JsonSerializerOptions { WriteIndented = true });

                await File.WriteAllTextAsync(filePath, prettyJson, cancellationToken);
                _logger.LogDebug("Saved {FilePath} with pretty-printed JSON.", filePath);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to pretty-print JSON from {Url}. Saving as-is.", url);
                // Save as-is if pretty-print fails
                await File.WriteAllTextAsync(filePath, json, cancellationToken);
            }
        }

        /// <summary>
        /// Waits for the game data to load.
        /// </summary>
        public async Task WaitForLoadAsync(CancellationToken cancellationToken = default)
        {
            await await Task.WhenAny(_loadTask, Task.Delay(-1, cancellationToken));
        }

        /// <summary>
        /// Creates an HttpClient with cookies and a user-agent.
        /// Loads cookies from Cookies.json if available.
        /// </summary>
        private static HttpClient CreateHttpClientWithCookies()
        {
            var cookieContainer = new CookieContainer();
            if (!File.Exists("Cookies.json"))
            {
                // make it and open it with code
                File.WriteAllText("Cookies.json", "[]");
            }

            LoadCookiesFromJson(cookieContainer, "Cookies.json");

            var handler = new HttpClientHandler
            {
                CookieContainer = cookieContainer,
                UseCookies = true,
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };

            var client = new HttpClient(handler)
            {
                Timeout = TimeSpan.FromSeconds(30)
            };

            // Set a valid User-Agent
            client.DefaultRequestHeaders.Add("User-Agent",
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/115.0.0.0 Safari/537.36");

            return client;
        }

        private static void LoadCookiesFromJson(CookieContainer container, string filePath)
        {
            if (!File.Exists(filePath))
                return;

            string json = File.ReadAllText(filePath);
            var cookies = JsonSerializer.Deserialize<JsonCookie[]>(json);

            if (cookies == null || cookies.Length == 0)
                return;

            foreach (var c in cookies)
            {
                try
                {
                    DateTime? expires = null;
                    if (c.expirationDate.HasValue)
                    {
                        expires = DateTimeOffset.FromUnixTimeSeconds((long)c.expirationDate.Value).UtcDateTime;
                    }

                    var cookie = new Cookie(c.name, c.value)
                    {
                        Domain = c.domain,
                        Path = string.IsNullOrEmpty(c.path) ? "/" : c.path,
                        Secure = c.secure,
                        HttpOnly = c.httpOnly
                    };

                    if (expires.HasValue)
                        cookie.Expires = expires.Value;

                    container.Add(cookie);
                }
                catch
                {
                    // If any cookie fails to load, skip it
                }
            }
        }

        private class JsonCookie
        {
            public string domain { get; set; } = "";
            public double? expirationDate { get; set; }
            public bool httpOnly { get; set; }
            public string name { get; set; } = "";
            public string path { get; set; } = "/";
            public bool secure { get; set; }
            public string value { get; set; } = "";
        }
    }
}
