using System.Text.Json.Serialization;
using Xabbo.Core.GameData.Json;
using Xabbo.Core.GameData.Json.FurniData;
using Xabbo.Core.GameData.Json.ProductData;

namespace Xabbo.Core.Serialization;

[JsonSourceGenerationOptions(
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    PropertyNameCaseInsensitive = true,
    NumberHandling = JsonNumberHandling.AllowReadingFromString,
    Converters = [typeof(StringConverter)]
)]
[JsonSerializable(typeof(FurniData))]
[JsonSerializable(typeof(ProductData))]
internal partial class GameDataJsonContext : JsonSerializerContext;
