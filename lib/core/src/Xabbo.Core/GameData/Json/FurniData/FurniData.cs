using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Xabbo.Core.GameData.Json.FurniData;

public class FurniDataLoader
{
    public static FurniData Load(string path)
    {
        if (System.IO.File.Exists(path))
        {
            var json = System.IO.File.ReadAllText(path);
            return JsonSerializer.Deserialize<FurniData>(json);

        }

        throw new Exception("Furni data file not found.");
    }


}

public class FurniData
{
    [JsonPropertyName("roomitemtypes")]
    public FurniInfoContainer FloorItemTypes { get; set; }

    [JsonPropertyName("wallitemtypes")]
    public FurniInfoContainer WallItemTypes { get; set; }
}

public class FurniInfoContainer
{
    [JsonPropertyName("furnitype")]
    public List<FurniInfo> FurniType { get; set; } = [];
}

public class FurniInfo
{
    [JsonPropertyName("id")]
    public long TypeID { get; set; }

    [JsonPropertyName("classname")]
    public string ClassName { get; set; }

    [JsonPropertyName("revision")]
    public int Revision { get; set; }

    [JsonPropertyName("category")]
    public string Category { get; set; }

    [JsonPropertyName("defaultdir")]
    public int DefaultDir { get; set; }

    [JsonPropertyName("xdim")]
    public int XDim { get; set; }

    [JsonPropertyName("ydim")]
    public int YDim { get; set; }

    [JsonPropertyName("partcolors")]
    public PartColorContainer PartColors { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("adurl")]
    public string AdUrl { get; set; }

    [JsonPropertyName("offerid")]
    public int OfferId { get; set; }

    [JsonPropertyName("buyout")]
    public bool Buyout { get; set; }

    [JsonPropertyName("rentofferid")]
    public int RentOfferId { get; set; }

    [JsonPropertyName("rentbuyout")]
    public bool RentBuyout { get; set; }

    [JsonPropertyName("bc")]
    public bool Bc { get; set; }

    [JsonPropertyName("excludeddynamic")]
    public bool ExcludedDynamic { get; set; }

    [JsonPropertyName("customparams")]
    public string CustomParams { get; set; }

    [JsonPropertyName("specialtype")]
    public int SpecialType { get; set; }

    [JsonPropertyName("canstandon")]
    public bool CanStandOn { get; set; }

    [JsonPropertyName("cansiton")]
    public bool CanSitOn { get; set; }

    [JsonPropertyName("canlayon")]
    public bool CanLayOn { get; set; }

    [JsonPropertyName("furniline")]
    public string FurniLine { get; set; }

    [JsonPropertyName("environment")]
    public string Environment { get; set; }

    [JsonPropertyName("rare")]
    public bool Rare { get; set; }
}

public class PartColorContainer
{
    [JsonPropertyName("colors")] 
    public string[] Colors { get; set; } = [];
}
