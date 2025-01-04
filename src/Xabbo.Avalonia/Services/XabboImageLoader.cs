// File: Xabbo.Avalonia.Services/XabboImageLoader.cs
using System;
using System.Collections.Concurrent;
using System.Net.Http;
using System.Threading.Tasks;
using AsyncImageLoader.Loaders;
using Avalonia.Media.Imaging;
using Xabbo.Core;

namespace Xabbo.Avalonia.Services
{
    public sealed class XabboImageLoader : RamCachedWebImageLoader
    {
        public static XabboImageLoader Instance { get; }

        private readonly ConcurrentDictionary<string, DateTime> _failureCache = new ConcurrentDictionary<string, DateTime>();

        static XabboImageLoader()
        {
            // Initialize the singleton instance using the shared HttpClient
            Instance = new XabboImageLoader(HttpClientFactory.Instance, disposeHttpClient: false);
        }

        // Private constructor to enforce singleton pattern
        private XabboImageLoader(HttpClient httpClient, bool disposeHttpClient)
            : base(httpClient, disposeHttpClient)
        {
        }

        protected override Task<Bitmap?> LoadAsync(string url)
        {
            // We rely on the base implementation from RamCachedWebImageLoader
            return base.LoadAsync(url);
        }

        public override async Task<Bitmap?> ProvideImageAsync(string url)
        {
            try
            {
                if (_failureCache.TryGetValue(url, out DateTime failureTime) &&
                    (DateTime.Now - failureTime).TotalHours < 1)
                {
                    // If this URL recently failed, skip trying again for an hour
                    return null;
                }

                Bitmap? image = await base.ProvideImageAsync(url);
                if (image is null)
                {
                    // If the image could not be provided, mark it as failed
                    _failureCache.AddOrUpdate(url, DateTime.Now, (_, _) => DateTime.Now);
                }

                return image;
            }
            catch
            {
                // On any exception, mark as failed and re-throw
                _failureCache.AddOrUpdate(url, DateTime.Now, (_, _) => DateTime.Now);
                throw;
            }
        }
    }
}
