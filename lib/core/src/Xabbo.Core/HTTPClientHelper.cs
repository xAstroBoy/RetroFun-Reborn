// File: Xabbo.Core/HttpClientFactory.cs
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using Xabbo.Core;

namespace Xabbo.Core
{
    public static class HttpClientFactory
    {
        private static readonly Lazy<HttpClient> _httpClientLazy = new Lazy<HttpClient>(() =>
        {
            var cookieContainer = new CookieContainer();
            LoadCookiesFromJson(cookieContainer, "Cookies.json"); // Adjust the path as needed

            var handler = new HttpClientHandler
            {
                CookieContainer = cookieContainer,
                UseCookies = true,
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };

            var client = new HttpClient(handler)
            {
                Timeout = TimeSpan.FromSeconds(30) // Adjust timeout as needed
            };

            // Set a valid, commonly recognized User-Agent
            client.DefaultRequestHeaders.Add("User-Agent",
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 " +
                "(KHTML, like Gecko) Chrome/115.0.0.0 Safari/537.36");

            return client;
        });

        /// <summary>
        /// Provides a singleton instance of HttpClient configured with cookies and User-Agent.
        /// </summary>
        public static HttpClient Instance => _httpClientLazy.Value;

        /// <summary>
        /// Loads cookies from a JSON file and adds them to the provided CookieContainer.
        /// </summary>
        /// <param name="container">The CookieContainer to populate.</param>
        /// <param name="filePath">Path to the Cookies.json file.</param>
        private static void LoadCookiesFromJson(CookieContainer container, string filePath)
        {
            if (!File.Exists(filePath))
            {
                // Create an empty Cookies.json if it doesn't exist
                File.WriteAllText(filePath, "[]");
                return;
            }

            string json = File.ReadAllText(filePath);
            var cookies = JsonSerializer.Deserialize<List<JsonCookie>>(json);

            if (cookies == null || cookies.Count == 0)
            {
                // No cookies to add
                return;
            }

            foreach (var c in cookies)
            {
                try
                {
                    DateTime? expires = null;
                    if (c.ExpirationDate.HasValue)
                    {
                        // Convert Unix timestamp to DateTime
                        expires = DateTimeOffset.FromUnixTimeSeconds((long)c.ExpirationDate.Value).UtcDateTime;
                    }

                    var cookie = new Cookie(c.Name, c.Value)
                    {
                        Domain = c.Domain,
                        Path = string.IsNullOrEmpty(c.Path) ? "/" : c.Path,
                        Secure = c.Secure,
                        HttpOnly = c.HttpOnly,
                        Expires = expires ?? DateTime.MinValue
                    };

                    container.Add(cookie);
                }
                catch (Exception ex)
                {
                    // Log the exception or handle it as needed
                    Console.Error.WriteLine($"Failed to add cookie '{c.Name}': {ex.Message}");
                }
            }
        }
    }
}
