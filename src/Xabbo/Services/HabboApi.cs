using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

using Xabbo.Core;
using Xabbo.Services.Abstractions;
using Xabbo.Web.Serialization;

namespace Xabbo.Services;

public sealed class HabboApi : IHabboApi
{
    private readonly HttpClient _http;

    public HabboApi()
    {
        // Utilize the shared HttpClient from HttpClientFactory
        _http = HttpClientFactory.Instance;
    }


    private async Task<T> GetRequiredDataAsync<T>(string url, CancellationToken cancellationToken = default)
    {

        var typeInfo = JsonWebContext.Default.GetTypeInfo(typeof(T)) as JsonTypeInfo<T>
            ?? throw new Exception($"Failed to get type info for '{typeof(T)}'.");

        var res = await _http.GetAsync(url, cancellationToken);
        res.EnsureSuccessStatusCode();

        return await JsonSerializer.DeserializeAsync<T>(
            res.Content.ReadAsStream(cancellationToken), typeInfo, cancellationToken)
            ?? throw new Exception($"Failed to deserialize {typeInfo.Type.Name}.");
    }

    public Task<string> FetchPhotoDataAsync(string Url, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(Url);
    }
}
