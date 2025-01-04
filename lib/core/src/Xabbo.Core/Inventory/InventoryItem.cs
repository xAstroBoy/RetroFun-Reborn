using System;

using Xabbo.Messages;

namespace Xabbo.Core;

/// <inheritdoc cref="IInventoryItem"/>
public sealed class InventoryItem : IInventoryItem, IParserComposer<InventoryItem>
{
    public int ItemId { get; set; }
    public ItemType Type { get; set; }
    public int Id { get; set; }
    public long TypeID { get; set; }
    public string? Identifier { get; set; }
    public FurniCategory Category { get; set; }
    public ItemData Data { get; set; }
    IItemData IInventoryItem.Data => Data;
    public bool IsRecyclable { get; set; }
    public bool IsTradeable { get; set; }
    public bool IsGroupable { get; set; }
    public bool IsSellable { get; set; }
    public int SecondsToExpiration { get; set; }
    public bool HasRentPeriodStarted { get; set; }
    public int RoomId { get; set; }
    public string SlotId { get; set; } = "";
    public int Extra { get; set; }
    public bool IsFloorItem => Type is ItemType.Floor;
    public bool IsWallItem => Type is ItemType.Wall;

    public Point? Size { get; set; }

    public InventoryItem()
    {
        Data = new LegacyData();
    }

    public InventoryItem(IInventoryItem item)
    {
        Type = item.Type;
        TypeID = item.TypeID;

        Id = item.Id;
        ItemId = item.ItemId;
        Category = item.Category;
        Data = ItemData.Clone(item.Data);
    }

    private InventoryItem(in PacketReader p) : this()
    {
        ParseModern(in p);
    }

    private void ParseModern(in PacketReader p)
    {
        ItemId = p.ReadInt();

        Type = H.ToItemType(p.ReadString());

        Id = p.ReadInt();
        TypeID = p.ReadInt();
        Category = (FurniCategory)p.ReadInt();
        Data = p.Parse<ItemData>();
        IsRecyclable = p.ReadBool();
        IsTradeable = p.ReadBool();
        IsGroupable = p.ReadBool();
        IsSellable = p.ReadBool();
        SecondsToExpiration = p.ReadInt();
        HasRentPeriodStarted = p.ReadBool();
        RoomId = p.ReadInt();


        if (Type is ItemType.Floor)
        {
            SlotId = p.ReadString();
            Extra = p.ReadInt();

        }
    }

    

    void IComposer.Compose(in PacketWriter p)
    {
        ComposeModern(in p);
    }

    private void ComposeModern(in PacketWriter p)
    {
        p.WriteInt(ItemId);

        p.WriteString(Type.GetClientIdentifier().ToUpper());


        p.WriteInt(Id);
        p.WriteInt((int)TypeID);
        p.WriteInt((int)Category);
        p.Compose(Data);
        p.WriteBool(IsRecyclable);
        p.WriteBool(IsTradeable);
        p.WriteBool(IsGroupable);
        p.WriteBool(IsSellable);
        p.WriteInt(SecondsToExpiration);
        p.WriteBool(HasRentPeriodStarted);
        p.WriteInt(RoomId);


        if (Type is ItemType.Floor)
        {
            p.WriteString(SlotId);
            p.WriteInt((int)Extra);

        }

    }

    static InventoryItem IParser<InventoryItem>.Parse(in PacketReader p) => new(in p);

    public override string ToString() => $"{nameof(InventoryItem)}#{ItemId}";
}
