using System;

using Xabbo.Messages;

namespace Xabbo.Core;

/// <inheritdoc cref="ITradeItem"/>
public class TradeItem : ITradeItem, IParserComposer<TradeItem>
{
    public int ItemId { get; set; }
    public ItemType Type { get; set; }
    public bool IsFloorItem => Type == ItemType.Floor;
    public bool IsWallItem => Type == ItemType.Wall;
    public long TypeID { get; set; }
    public int Kind { get; set; }
    public string Identifier { get; set; } = "";
    public FurniCategory Category { get; set; }
    public bool IsGroupable { get; set; }
    public ItemData Data { get; set; } = new LegacyData();
    IItemData IInventoryItem.Data => Data;
    public int CreationDay { get; set; }
    public int CreationMonth { get; set; }
    public int CreationYear { get; set; }
    public int Extra { get; set; }
    public Point? Size { get; set; }
    public string SlotId { get; set; } = "";

    bool IInventoryItem.IsRecyclable => true;
    bool IInventoryItem.IsTradeable => true;
    bool IInventoryItem.IsSellable => true;
    string IInventoryItem.SlotId => "";
    int IInventoryItem.SecondsToExpiration => -1;
    bool IInventoryItem.HasRentPeriodStarted => false;
    int IInventoryItem.RoomId => -1;
    public int Id => -1;

    public TradeItem() { }

    protected TradeItem(in PacketReader p)
    {
        ParseModern(in p);
    }

    private void ParseModern(in PacketReader p)
    {
        ItemId = p.ReadInt();
        Type = H.ToItemType(p.ReadString());
        TypeID = p.ReadInt();
        Kind = p.ReadInt();
        Category = (FurniCategory)p.ReadInt();
        IsGroupable = p.ReadBool();
        Data = p.Parse<ItemData>();
        CreationDay = p.ReadInt();
        CreationMonth = p.ReadInt();
        CreationYear = p.ReadInt();

        if (Type == ItemType.Floor)
            Extra = p.ReadInt();
        else
            Extra = -1;
    }


    void IComposer.Compose(in PacketWriter p)
    {
        ComposeModern(in p);
    }

    void ComposeModern(in PacketWriter p)
    {

        p.WriteInt(ItemId);
        p.WriteString(Type.GetClientIdentifier());
        p.WriteInt((int)TypeID);
        p.WriteInt(Kind);
        p.WriteInt((int)Category);
        p.WriteBool(IsGroupable);
        p.Compose(Data);
        p.WriteInt(CreationDay);
        p.WriteInt(CreationMonth);
        p.WriteInt(CreationYear);

        if (Type == ItemType.Floor)
            p.WriteInt(Extra);
    }

    public override string ToString() => $"{nameof(TradeItem)}#{ItemId}/{Type}:{Kind}";

    static TradeItem IParser<TradeItem>.Parse(in PacketReader p) => new(in p);
}
