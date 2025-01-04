using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Xabbo.Core.Serialization;

namespace Xabbo.Core.GameData;

public class GameDataLoader(
    string? cachePath = null,
    IGameDataUrlProvider? urlProvider = null,
    ILoggerFactory? loggerFactory = null
)
    : IGameDataLoader
{
    private readonly string _cachePath = cachePath
        ?? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "xabbo", "gamedata");
    private readonly IGameDataUrlProvider _urlProvider = urlProvider ?? new GameDataUrlProvider();
    private readonly ILogger _logger = (ILogger?)loggerFactory?.CreateLogger<GameDataLoader>() ?? NullLogger.Instance;
    private readonly HttpClient _http = new(new HttpClientHandler { AllowAutoRedirect = false })
    {
        DefaultRequestHeaders = {
            { "User-Agent", "xabbo" }
        }
    };

    public bool CacheOnly { get; set; }
    public TimeSpan MaxAge { get; set; } = TimeSpan.FromHours(4);

    private static GameDataType? GetGameDataTypeFromName(string name) => name switch
    {
        "figurepartlist" => GameDataType.FigureData,
        "furnidata" => GameDataType.FurniData,
        "productdata" => GameDataType.ProductData,
        "external_texts" => GameDataType.ExternalTexts,
        "external_variables" => GameDataType.ExternalVariables,
        _ => null
    };

}
