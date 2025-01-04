
using System.Text.Json.Serialization;

namespace Xabbo.Core
{
    public class JsonCookie
    {
        [JsonPropertyName("domain")] public string Domain { get; set; } = string.Empty;

        [JsonPropertyName("expirationDate")] public double? ExpirationDate { get; set; }

        [JsonPropertyName("httpOnly")] public bool HttpOnly { get; set; }

        [JsonPropertyName("name")] public string Name { get; set; } = string.Empty;

        [JsonPropertyName("path")] public string Path { get; set; } = "/";

        [JsonPropertyName("secure")] public bool Secure { get; set; }

        [JsonPropertyName("value")] public string Value { get; set; } = string.Empty;
    }
}
