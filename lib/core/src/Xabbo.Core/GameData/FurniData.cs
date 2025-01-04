
using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Immutable;
using Xabbo.Core.GameData.Json.FurniData;

namespace Xabbo.Core.GameData;

/// <summary>
/// Defines a map of floor and wall <see cref="FurniInfo"/>.
/// </summary>
public sealed class FurniData : IReadOnlyCollection<FurniInfo>
{
    public static FurniData Load(string json) => new(Json.FurniData.FurniDataLoader.Load(json));

    private readonly ImmutableDictionary<string, FurniInfo> _identifierMap;
    private readonly ImmutableDictionary<long, FurniInfo> _floorItemMap, _wallItemMap;

    /// <summary>
    /// Gets the information of all floor items.
    /// </summary>
    public ImmutableArray<FurniInfo> FloorItems { get; }

    /// <summary>
    /// Gets the information of all wall items.
    /// </summary>
    public ImmutableArray<FurniInfo> WallItems { get; }

    /// <summary>
    /// Gets the total number of <see cref="FurniInfo"/> contained in the furni data.
    /// </summary>
    public int Count => FloorItems.Length + WallItems.Length;

    public IEnumerator<FurniInfo> GetEnumerator() => FloorItems.Concat(WallItems).GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <summary>
    /// Gets the information of the furni with the specified identifier.
    /// </summary>
    public FurniInfo this[string identifier] => GetInfo(identifier);

    internal FurniData(Json.FurniData.FurniData proxy)
    {
        List<FurniInfo> Floor = new();
        List<FurniInfo> Wall = new();

        foreach (var item in proxy.FloorItemTypes.FurniType)
        {
            if (Floor.Any(x => x.TypeID == item.TypeID))
                continue;

            Floor.Add(new FurniInfo(ItemType.Floor, item));
        }
        FloorItems = Floor.ToImmutableArray();
        foreach (var item in proxy.WallItemTypes.FurniType)
        {
            if (Floor.Any(x => x.TypeID == item.TypeID))
                continue;

            Wall.Add(new FurniInfo(ItemType.Wall, item));
        }
        WallItems = Wall.ToImmutableArray();


        var allItems = FloorItems.Concat(WallItems);
        _identifierMap = allItems
            .GroupBy(fi => fi.Identifier, StringComparer.InvariantCultureIgnoreCase)
            .SelectMany(group =>
                group.Select((fi, index) => new
                {
                    Key = index == 0 ? fi.Identifier : $"{fi.Identifier}_dup{index}", Value = fi
                })
            )
            .ToImmutableDictionary(
                x => x.Key,
                x => x.Value,
                StringComparer.InvariantCultureIgnoreCase
            );

        _floorItemMap = FloorItems.ToImmutableDictionary(furniInfo => furniInfo.TypeID);
        _wallItemMap = WallItems.ToImmutableDictionary(wallItem => wallItem.TypeID);
    }
    /// <summary>
    /// Gets whether furni info with the specified type and kind exists or not.
    /// </summary>
    public bool Exists(ItemType type, long TypeId)
    {
        return type switch
        {
            ItemType.Floor => _floorItemMap.ContainsKey(TypeId),
            ItemType.Wall => _wallItemMap.ContainsKey(TypeId),
            _ => false
        };
    }

    /// <summary>
    /// Gets whether furni info for the specified item exists or not.
    /// </summary>
    public bool Exists(IItem item) => item.Identifier is not null ? Exists(item.Identifier) : Exists(item.Type, item.TypeID);

    /// <summary>
    /// Gets whether furni info with the specified identifier exists or not.
    /// </summary>
    public bool Exists(string identifier) => _identifierMap.ContainsKey(identifier);

    /// <summary>
    /// Gets whether floor furni info with the specified kind exists or not.
    /// </summary>
    public bool FloorItemExists(int TypeId) => Exists(ItemType.Floor, TypeId);

    /// <summary>
    /// Gets whether wall furni info with the specified kind exists or not.
    /// </summary>
    public bool WallItemExists(int kind) => Exists(ItemType.Wall, kind);

    /// <summary>
    /// Gets the information of the furni with the specified type and kind.
    /// </summary>
    public FurniInfo GetInfo(ItemType type, long TypeID) => type switch
    {
        ItemType.Floor => GetFloorItem(TypeID),
        ItemType.Wall => GetWallItem(TypeID),
        _ => throw new Exception($"Cannot get furni info for item type: {type}.")
    };

    /// <summary>
    /// Gets the information of the furni with the specified identifier.
    /// </summary>
    public FurniInfo GetInfo(string identifier)
    {
        if (!TryGetInfo(identifier, out FurniInfo? info))
            throw new Exception($"Furni info not found for '{identifier}'.");
        return info;
    }

    /// <summary>
    /// Gets the information of the specified item.
    /// </summary>
    public FurniInfo GetInfo(IItem item) =>
        item.Identifier is not null
        ? GetInfo(item.Identifier)
        : GetInfo(item.Type, item.TypeID);

    public FurniInfo GetInfoFromType(IItem item) => GetInfo(item.Type, item.TypeID);

    /// <summary>
    /// Gets the information of the furni with the specified type and kind.
    /// </summary>
    public bool TryGetInfo(ItemType type, long TypeID, [NotNullWhen(true)] out FurniInfo? info)
    {
        info = null;
        return type switch
        {
            ItemType.Floor => _floorItemMap.TryGetValue(TypeID, out info),
            ItemType.Wall => _wallItemMap.TryGetValue(TypeID, out info),
            _ => false
        };
    }

    /// <summary>
    /// Gets the information of the furni with the specified identifier.
    /// </summary>
    public bool TryGetInfo(string identifier, [NotNullWhen(true)] out FurniInfo? info) => _identifierMap.TryGetValue(identifier, out info);

    /// <summary>
    /// Attempts to get the information of the specified item.
    /// </summary>
    public bool TryGetInfo(IItem item, [NotNullWhen(true)] out FurniInfo? info)
    {
        if (item.Identifier is not null)
            return _identifierMap.TryGetValue(item.Identifier, out info);
        else
            return TryGetInfo(item.Type, item.TypeID, out info);
    }

    /// <summary>
    /// Gets the information of the floor item with the specified kind.
    /// </summary>
    public FurniInfo GetFloorItem(long TypeID)
    {
        if (!_floorItemMap.TryGetValue(TypeID, out FurniInfo info))
            return null;
        return info;
    }

    /// <summary>
    /// Gets the information of the wall item with the specified kind.
    /// </summary>
    public FurniInfo GetWallItem(long TypeID)
    { 
        if (!_wallItemMap.TryGetValue(TypeID, out FurniInfo info))
            return null;
        return info;
    }

    private static IEnumerable<FurniInfo> FindItems(IEnumerable<FurniInfo> infos, string searchText)
    {
        searchText = searchText.ToLower();
        return infos
            .Where(x => x.Name.Contains(searchText, StringComparison.CurrentCultureIgnoreCase))
            .OrderBy(x => Math.Abs(x.Name.Length - searchText.Length));
    }

    /// <summary>
    /// Finds information of all furni containing the specified text in its name.
    /// </summary>
    public IEnumerable<FurniInfo> FindItems(string searchText) => FindItems(this, searchText);

    /// <summary>
    /// Finds information of all floor furni containing the specified text in its name.
    /// </summary>
    public IEnumerable<FurniInfo> FindFloorItems(string searchText) => FindItems(FloorItems, searchText);

    /// <summary>
    /// Finds information of all wall furni containing the specified text in its name.
    /// </summary>
    public IEnumerable<FurniInfo> FindWallItems(string searchText) => FindItems(WallItems, searchText);

    /// <summary>
    /// Finds the first information of a furni containing the specified text in its name,
    /// or <c>null</c> if no matches were found.
    /// </summary>
    public FurniInfo? FindItem(string searchText) => FindItems(this, searchText).FirstOrDefault();

    /// <summary>
    /// Finds the first information of a floor furni containing the specified text in its name,
    /// or <c>null</c> if no matches were found.
    /// </summary>
    public FurniInfo? FindFloorItem(string searchText) => FindItems(FloorItems, searchText).FirstOrDefault();

    /// <summary>
    /// Finds the first information of a wall furni containing the specified text in its name,
    /// or <c>null</c> if no matches were found.
    /// </summary>
    public FurniInfo? FindWallItem(string searchText) => FindItems(WallItems, searchText).FirstOrDefault();

    /// <summary>
    /// Determines whether an item specifies a variant (not state) in its data.
    /// </summary>
    public static bool HasVariant(FurniInfo info) => info.Identifier.Equals("poster");

    /// <summary>
    /// Determines whether the item specifies a variant (not state) in its data.
    /// </summary>
    public bool HasVariant(IItem item) => item.Type switch
    {
        ItemType.Wall => HasVariant(GetInfo(item)),
        ItemType.Badge or ItemType.Effect or ItemType.Bot => true,
        _ => false
    };

    private static string? GetVariantInternal(IItem item) => (item switch
    {
        ItemDescriptor x => x.Variant,
        IFloorItem x => x.Data.Value,
        IWallItem x => x.Data,
        IInventoryItem x => x.Data.Value,
        IMarketplaceOffer x => x.Data.Value,
        ICatalogProduct x => x.Variant,
        _ => null
    })?.Trim();

    /// <summary>
    /// Gets the item's variant, or null if the specified item does not have a variant.
    /// </summary>
    public string? GetVariant(IItem item)
    {
        if (HasVariant(item))
        {
            return GetVariantInternal(item) ??
                throw new ArgumentException("Failed to get variant for the specified item.", nameof(item));
        }
        return null;
    }

    /// <summary>
    /// Attempts to get the variant of the specified item.
    /// </summary>
    public bool TryGetVariant(IItem item, [NotNullWhen(true)] out string? variant)
    {
        if (!HasVariant(item))
        {
            variant = null;
            return false;
        }

        variant = GetVariantInternal(item);
        return variant is not null;
    }

    /// <summary>
    /// Gets the <see cref="ItemDescriptor"/> of the specified item.
    /// </summary>
    public ItemDescriptor GetItemDescriptor(IItem item)
        => new(item.Type, item.TypeID, item.Identifier, GetVariant(item));

    /// <summary>
    /// Gets the <see cref="ItemDescriptor"/> of the specified inventory item.
    /// </summary>
    public StackDescriptor GetStackDescriptor(IInventoryItem item)
        => new(item.Type, item.TypeID, item.Identifier, GetVariant(item), item.IsTradeable, item.IsGroupable);

    /// <summary>
    /// Groups items by maximum slots/items in order to offer in a trade.
    /// </summary>
    public IEnumerable<IGrouping<int, IInventoryItem>> GroupItems(IEnumerable<IInventoryItem> items,
        int maxSlots = 9, int maxItems = 1500)
    {
        if (maxSlots < 1 || maxSlots > 9) throw new ArgumentOutOfRangeException(nameof(maxSlots));
        if (maxItems < 1 || maxItems > 1500) throw new ArgumentOutOfRangeException(nameof(maxItems));

        StackDescriptor currentDescriptor = default;
        int currentGroup = 0, currentSlots = 0, currentItems = 0;

        return items
            .OrderBy(item => item.Type)
            .ThenBy(item => item.TypeID)
            .ThenBy(item => item.Data?.Value)
            .GroupBy(item =>
            {
                var descriptor = GetStackDescriptor(item);
                if (!item.IsGroupable || descriptor != currentDescriptor)
                    currentSlots++;

                currentDescriptor = descriptor;
                currentItems++;

                if (currentSlots > maxSlots || currentItems > maxItems)
                {
                    currentDescriptor = default;
                    currentSlots = currentItems = 0;
                    currentGroup++;
                }

                return currentGroup;
            });
    }
}
