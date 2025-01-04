using System.Collections.Immutable;
using System.Linq;

namespace Xabbo.Core.GameData;

/// <summary>
/// Defines a set of <see cref="FigurePart"/>.
/// </summary>
/// <param name="Id">The ID of the part set.</param>
/// <param name="Gender">The gender of the part set.</param>
/// <param name="Parts">The parts in this part set.</param>
/// <param name="RequiredClubLevel">The club level required to wear this part set.</param>
/// <param name="IsColorable">Whether the part set is colorable.</param>
/// <param name="IsSelectable">Whether the part set is selectable in client.</param>
/// <param name="IsPreSelectable"></param>
/// <param name="IsSellable">Whether the part set has an associated clothing furniture.</param>
public sealed record FigurePartSet(
    int Id,
    Gender Gender,
    ImmutableArray<FigurePart> Parts,
    int RequiredClubLevel = 0,
    bool IsColorable = false,
    bool IsSelectable = false,
    bool IsPreSelectable = false,
    bool IsSellable = false
)
{
    public bool IsClubRequired => RequiredClubLevel > 0;

    internal FigurePartSet(Gender gender, Xabbo.Core.GameData.Json.FigureData.Set partSet) : this(
        Id: partSet.Id,
        Gender: gender,
        Parts: partSet.Parts
            .Select(part => new FigurePart(H.GetFigurePartType(part.Type), part.Id))
            .ToImmutableArray()
    )
    {
    }
}
