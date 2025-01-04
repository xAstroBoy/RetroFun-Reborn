﻿
using System.Collections.Immutable;
using System.Drawing;

namespace Xabbo.Core.GameData;

/// <summary>
/// Defines information about a furniture.
/// </summary>
/// <param name="Type">The furniture type.</param>
/// <param name="TypeID">The furniture kind.</param>
/// <param name="Identifier">The furniture's unique string identifier.</param>
/// <param name="Revision">The revision number.</param>
/// <param name="DefaultDirection">The default direction when placed in a room.</param>
/// <param name="XDimension">The number of tiles this furniture occupies aint the X-axis.</param>
/// <param name="YDimension">The number of tiles this furniture occupies aint the Y-axis.</param>
/// <param name="PartColors"></param>
/// <param name="Name">The name of the furniture.</param>
/// <param name="Description">The description of the furniture.</param>
/// <param name="AdUrl"></param>
/// <param name="OfferId"></param>
/// <param name="BuyOut"></param>
/// <param name="RentOfferId"></param>
/// <param name="RentBuyOut"></param>
/// <param name="IsBuildersClub"></param>
/// <param name="ExcludedDynamic"></param>
/// <param name="CustomParams"></param>
/// <param name="Category"></param>
/// <param name="CategoryName"></param>
/// <param name="CanStandOn"></param>
/// <param name="CanSitOn"></param>
/// <param name="CanLayOn"></param>
/// <param name="Line"></param>
/// <param name="Environment"></param>
/// <param name="IsRare"></param>
/// <remarks>
/// <see cref="TypeID"/> is the numeric identifier of a furniture.
/// This number can be different across hotels.
/// It is only unique for each item type - a floor and a wall item may have the same kind.
/// <para/>
/// <see cref="Identifier"/> is a unique string identifier, also known as its <i>class name</i>.
/// This identifier is unique across both furniture types, and is the same across hotels.
/// </remarks>
public sealed record FurniInfo(

    ItemType Type,
    long TypeID,
    string Identifier,
    int Revision = 0,
    int DefaultDirection = 0,
    int XDimension = 0,
    int YDimension = 0,
    ImmutableArray<string> PartColors = default,
    string Name = "",
    string Description = "",
    string AdUrl = "",
    int OfferId = 0,
    bool BuyOut = false,
    int RentOfferId = 0,
    bool RentBuyOut = false,
    bool IsBuildersClub = false,
    bool ExcludedDynamic = false,
    string CustomParams = "",
    FurniCategory Category = FurniCategory.Unknown,
    string CategoryName = "",
    bool CanStandOn = false,
    bool CanSitOn = false,
    bool CanLayOn = false,
    string Line = "",
    string Environment = "",
    string ClassName = "",

    bool IsRare = false,
    Point Size = default
)
{
    public ImmutableArray<string> PartColors { get; } = PartColors.IsDefault ? [] : PartColors;

    internal FurniInfo(ItemType type, Json.FurniData.FurniInfo proxy) : this(
        Type: type,
        TypeID: proxy.TypeID,
        Identifier: proxy.ClassName ?? "",
        Revision: proxy.Revision,
        DefaultDirection: proxy.DefaultDir,
        XDimension: proxy.XDim,
        YDimension: proxy.YDim,
        PartColors: proxy?.PartColors?.Colors.ToImmutableArray() ?? ImmutableArray<string>.Empty,
        Name: proxy.Name ?? "",
        Description: proxy.Description ?? "",
        AdUrl: proxy.AdUrl ?? "",
        OfferId: proxy.OfferId,
        BuyOut: proxy.Buyout,
        RentOfferId: proxy.RentOfferId,
        RentBuyOut: proxy.RentBuyout,
        IsBuildersClub: proxy.Bc,
        ExcludedDynamic: proxy.ExcludedDynamic,
        CustomParams: proxy.CustomParams ?? "",
        Category: (FurniCategory)proxy.SpecialType,
        CategoryName: proxy.Category ?? "",
        CanStandOn: proxy.CanStandOn,
        CanSitOn: proxy.CanSitOn,
        CanLayOn: proxy.CanLayOn,
        Line: proxy.FurniLine ?? "",
        Environment: proxy.Environment ?? "",
        IsRare: proxy.Rare,
        Size : new Point(proxy.XDim, proxy.YDim),
        ClassName: proxy.ClassName ?? ""
    )
    { }
}
