using System;

using Xabbo.Messages;

namespace Xabbo.Core;

/// <inheritdoc cref="IWallItem"/>
public class WallItem : Furni, IWallItem, IParserComposer<WallItem>
{
    public override ItemType Type => ItemType.Wall;

    public string Data { get; set; } = "";

    public override int State => int.TryParse(Data, out int state) ? state : -1;

    public WallLocation Location { get; set; } = WallLocation.Zero;

    public int WX => Location.Wall.X;
    public int WY => Location.Wall.Y;
    public int LX => Location.Offset.X;
    public int LY => Location.Offset.Y;
    public WallOrientation Orientation => Location.Orientation;

    /// <summary>
    /// Constructs a new empty wall item.
    /// </summary>
    public WallItem() { }

    /// <summary>
    /// Constructs a new copy of the specified wall item.
    /// </summary>
    public WallItem(IWallItem item)
    {
        Id = item.Id;
        TypeID = item.TypeID;
        Location = item.Location;
        Data = item.Data;
        SecondsToExpiration = item.SecondsToExpiration;
        Usage = item.Usage;

        OwnerId = item.OwnerId;
        OwnerName = item.OwnerName;

        IsHidden = item.IsHidden;
    }

    public WallItem(int id, long TypeID, WallLocation location, string data, int secondsToExpiration, FurniUsage usage, int ownerId, string? ownerName = null)
    {
        Id = id;
        this.TypeID = TypeID;
        Location = location;
        Data = data;
        SecondsToExpiration = secondsToExpiration;
        Usage = usage;
        OwnerId = ownerId;
        OwnerName = ownerName;
    }


    protected WallItem(in PacketReader p)
        : this()
    {
        ParseModern(in p);
    }

    private void ParseModern(in PacketReader p)
    {
        Id = int.Parse(p.ReadString());
        TypeID = p.ReadInt();

        Location = WallLocation.ParseString(p.ReadString());
        Data = p.ReadString();
        SecondsToExpiration = p.ReadInt();
        Usage = (FurniUsage)p.ReadInt();
        OwnerId = p.ReadInt();
    }

    protected override void Compose(in PacketWriter p)
    {
        p.WriteString(TypeID.ToString());

        p.WriteInt(Id);
        p.WriteString(Location.ToString());
        p.WriteString(Data);
        p.WriteInt(SecondsToExpiration);
        p.WriteInt((int)Usage);
        p.WriteInt(OwnerId);
    }

    public override string ToString() => $"{nameof(WallItem)}#{Id}/{TypeID}";


    static WallItem IParser<WallItem>.Parse(in PacketReader p)
    {
        return new(in p);
    }

}
