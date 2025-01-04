﻿using Xabbo.Messages;

namespace Xabbo.Core;

/// <inheritdoc cref="IFurni"/>
public abstract class Furni : IFurni, IComposer
{
    public abstract ItemType Type { get; }
    public int Id { get; set; }
    public bool IsFloorItem => Type == ItemType.Floor;
    public bool IsWallItem => Type == ItemType.Wall;
    public long TypeID { get; set; }
    public int OwnerId { get; set; }
    public string OwnerName { get; set; } = "";

    public abstract int State { get; }

    public int SecondsToExpiration { get; set; } = -1;
    public FurniUsage Usage { get; set; } = FurniUsage.None;

    public string? Identifier { get; set; }

    public bool IsHidden { get; set; }

    void IComposer.Compose(in PacketWriter p) => Compose(in p);
    protected abstract void Compose(in PacketWriter p);
}
