using System.Text.Json.Serialization;

namespace Xabbo.Core
{
    public class PhotoInfo
    {
        [JsonPropertyName("t")] public long Time { get; set; } = 0;

        [JsonPropertyName("w")] public string Url { get; set; } = string.Empty;

        [JsonPropertyName("s")] public string? S { get; set; } = "0"; 

        [JsonPropertyName("u")]
        public string? User { get; set; } = string.Empty; 

        [JsonPropertyName("n")] public string? N { get; set; } = "0"; 

        [JsonPropertyName("Data")] public object? Data { get; set; } = null;

    }
}
