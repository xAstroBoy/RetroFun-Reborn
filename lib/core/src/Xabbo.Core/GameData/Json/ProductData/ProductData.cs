using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Xabbo.Core.GameData.Json.ProductData;

public static class ProductDataLoader
{
    public static ProductData Load(string path)
    {
        if (System.IO.File.Exists(path))
        {
            var json = System.IO.File.ReadAllText(path);
            return JsonSerializer.Deserialize<ProductData>(json);

        }

        throw new Exception("Product data file not found.");
    }

}

public class ProductData
{
    [JsonPropertyName("productdata")] 
    public ProductInfoContainer ProductDataContainer { get; set; } = new();

    public class ProductInfoContainer
    {
        [JsonPropertyName("product")] 
        public List<ProductInfo> Products { get; set; } = new();
    }
}

public class ProductInfo
{
    [JsonPropertyName("code")] 
    public string Code { get; set; } = "";

    [JsonPropertyName("name")] 
    public string Name { get; set; } = "";

    [JsonPropertyName("description")] 
    public string Description { get; set; } = "";
}

public class Root
{
    [JsonPropertyName("productdata")]
    public ProductData.ProductInfoContainer ProductDataContainer { get; set; } = new();
}
