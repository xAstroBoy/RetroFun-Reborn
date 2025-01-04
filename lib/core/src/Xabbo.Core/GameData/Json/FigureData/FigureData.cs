using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Xabbo.Core.GameData.Json.FigureData;


public static class FigureDataLoader
{
    public static FigureData Load(string path)
    {
        if (System.IO.File.Exists(path))
        {
            var json = System.IO.File.ReadAllText(path);
            return JsonSerializer.Deserialize<FigureData>(json);

        }

        throw new Exception("Figure data file not found.");
    }

}

public class FigureData
{
    [JsonPropertyName("figuredata")] 
    public Root Data { get; set; } = new();

}

public class Color
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("index")]
    public int Index { get; set; }

    [JsonPropertyName("club")]
    public int Club { get; set; }

    [JsonPropertyName("selectable")]
    public bool Selectable { get; set; }

    [JsonPropertyName("hexCode")]
    public string HexCode { get; set; }
}

public class HiddenLayer
{
    [JsonPropertyName("partType")]
    public string PartType { get; set; }
}

public class Palette
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("colors")]
    public List<Color> Colors { get; set; } = new();
}

public class Part
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("colorable")]
    public bool Colorable { get; set; }

    [JsonPropertyName("index")]
    public int Index { get; set; }

    [JsonPropertyName("colorindex")]
    public int ColorIndex { get; set; }
}

public class Set
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("gender")]
    public string Gender { get; set; }

    [JsonPropertyName("club")]
    public int Club { get; set; }

    [JsonPropertyName("colorable")]
    public bool Colorable { get; set; }

    [JsonPropertyName("selectable")]
    public bool Selectable { get; set; }

    [JsonPropertyName("preselectable")]
    public bool Preselectable { get; set; }

    [JsonPropertyName("sellable")]
    public bool Sellable { get; set; }

    [JsonPropertyName("parts")]
    public List<Part> Parts { get; set; } = new();

    [JsonPropertyName("hiddenLayers")]
    public List<HiddenLayer> HiddenLayers { get; set; } = new();
}

public class SetType
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("paletteId")]
    public int PaletteId { get; set; }

    [JsonPropertyName("mandatory_f_0")]
    public bool MandatoryF0 { get; set; }

    [JsonPropertyName("mandatory_f_1")]
    public bool MandatoryF1 { get; set; }

    [JsonPropertyName("mandatory_m_0")]
    public bool MandatoryM0 { get; set; }

    [JsonPropertyName("mandatory_m_1")]
    public bool MandatoryM1 { get; set; }

    [JsonPropertyName("sets")]
    public List<Set> Sets { get; set; } = new();
}

public class Root
{
    [JsonPropertyName("palettes")]
    public List<Palette> Palettes { get; set; } = new();

    [JsonPropertyName("setTypes")]
    public List<SetType> SetTypes { get; set; } = new();
}
