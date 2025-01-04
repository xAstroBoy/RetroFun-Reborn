using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Xabbo.Core.GameData.Json.FigureData;

namespace Xabbo.Core.GameData;

/// <summary>
/// Defines a collection of figure part sets of the same <see cref="FigurePartType"/>.
/// </summary>
public sealed record FigurePartSetCollection(
    FigurePartType Type,
    int PaletteId,
    ImmutableDictionary<int, FigurePartSet> PartSets,
    int Mand_m_0 = 0,
    int Mand_f_0 = 0,
    int Mand_m_1 = 0,
    int Mand_f_1 = 0
)
{
    internal FigurePartSetCollection(FigurePartType partSetType,
        Xabbo.Core.GameData.Json.FigureData.FigureData figureData) : this(
        Type: partSetType,
        PaletteId: -1,
        PartSets: ConvertToImmutableDictionary(partSetType, figureData)
    )
    {
    }

    private static System.Collections.Immutable.ImmutableDictionary<int, FigurePartSet> ConvertToImmutableDictionary(
        FigurePartType partSetType, Xabbo.Core.GameData.Json.FigureData.FigureData figureData)
    {
        var setType = FindSetType(partSetType, figureData);

        if (setType == null || setType.Sets == null || setType.Sets.Count == 0)
            return System.Collections.Immutable.ImmutableDictionary<int, FigurePartSet>.Empty;

        var partSets = new Dictionary<int, FigurePartSet>();

        foreach (var set in setType.Sets)
        {
            var gender = set.Gender == "M" ? Gender.Male : Gender.Female;
            var partSet = new FigurePartSet(gender, set);
            partSets[set.Id] = partSet;
        }

        return partSets.ToImmutableDictionary();
    }

    private static SetType? FindSetType(FigurePartType partSetType,
        Xabbo.Core.GameData.Json.FigureData.FigureData figureData)
    {
        foreach (var setType in figureData.Data.SetTypes)
        {
            if (setType.Type == partSetType.ToShortString())
            {
                return setType;
            }
        }

        return null;
    }


}
