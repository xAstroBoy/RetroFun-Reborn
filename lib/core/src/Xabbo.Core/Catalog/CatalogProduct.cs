using System;

using Xabbo.Messages;

namespace Xabbo.Core;

/// <inheritdoc cref="ICatalogProduct"/>
public sealed class CatalogProduct : ICatalogProduct, IParserComposer<CatalogProduct>
{
    public ItemType Type { get; set; }
    public int Kind { get; set; }
    public string Variant { get; set; }
    public int Count { get; set; }
    public bool IsLimited { get; set; }
    public int LimitedTotal { get; set; }
    public int LimitedRemaining { get; set; }

    public bool IsFloorItem => Type == ItemType.Floor;
    public bool IsWallItem => Type == ItemType.Wall;
    long IItem.TypeID => -1;

    string? IItem.Identifier => null;
    public int Id { get; set; }

    public CatalogProduct()
    {
        Variant = "";
    }

    private CatalogProduct(in PacketReader p)
    {
        Type = H.ToItemType(p.ReadString());

        if (Type == ItemType.Badge)
        {
            Variant = p.ReadString();
            Count = 1;
        }
        else
        {
            Kind = p.ReadInt();
            Variant = p.ReadString();
            Count = p.ReadInt();
            IsLimited = p.ReadBool();
            if (IsLimited)
            {
                LimitedTotal = p.ReadInt();
                LimitedRemaining = p.ReadInt();
            }
        }
    }

    public override string ToString()
    {
        if (string.IsNullOrWhiteSpace(Variant))
        {
            return $"{nameof(CatalogProduct)}/{Type}:{Kind}";
        }
        else
        {
            return $"{nameof(CatalogProduct)}/{Type}:{Kind}:{Variant}";
        }
    }

    void IComposer.Compose(in PacketWriter p)
    {
        p.WriteString(Type.GetClientIdentifier());

        if (Type == ItemType.Badge)
        {
            p.WriteString(Variant);
        }
        else
        {
            p.WriteInt(Kind);
            p.WriteString(Variant);
            p.WriteInt(Count);
            p.WriteBool(IsLimited);

            if (IsLimited)
            {
                p.WriteInt(LimitedTotal);
                p.WriteInt(LimitedRemaining);
            }
        }
    }

    static CatalogProduct IParser<CatalogProduct>.Parse(in PacketReader p) => new(in p);
}
