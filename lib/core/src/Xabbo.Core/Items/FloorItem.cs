using System.Globalization;
using System.Text.Json.Serialization;
using Xabbo.Messages;
using Xabbo.Messages.Nitro;

namespace Xabbo.Core;

/// <inheritdoc cref="IFloorItem"/>
public class FloorItem : Furni, IFloorItem, IParserComposer<FloorItem>
{
    public override ItemType Type => ItemType.Floor;

    public Tile Location { get; set; }

    private Point? _size;
    public Point? Size
    {
        get
        {
            if (_size is not null)
            {
                return _size;
            }

            if (Extensions.TryGetInfo(this, out var info) &&
                info is { Size: Point size })
            {
                return size;
            }

            return null;
        }

        set => _size = value;
    }

    public Area? Area
    {
        get
        {
            if (Size is { } size)
            {
                var area = new Area(size).At(Location);
                if (Direction == 2 || Direction == 6)
                    area = area.Flip();
                return area;
            }
            return null;
        }
    }

    [JsonIgnore] public int X => Location.X;
    [JsonIgnore] public int Y => Location.Y;
    [JsonIgnore] public Point XY => Location.XY;
    [JsonIgnore] public double Z => Location.Z;

    public int Direction { get; set; }
    public float Height { get; set; }
    public int Extra { get; set; }

    public ItemData Data { get; set; } = new EmptyItemData();
    IItemData IFloorItem.Data => Data;

    public override int State => Data.Value switch
    {
        "FALSE" or "OFF" or "C" => 0,
        "TRUE" or "ON" or "O" => 1,
        _ => double.TryParse(Data.Value, CultureInfo.InvariantCulture, out double state) ? (int)state : -1
    };

    public string Colors { get; set; } = "";
    public string RuntimeData { get; set; } = "";

    /// <summary>
    /// Constructs a new empty floor item.
    /// </summary>
    public FloorItem() { }

    /// <summary>
    /// Constructs a new copy of the specified floor item.
    /// </summary>
    public FloorItem(IFloorItem item)
    {
        Id = item.Id;
        TypeID = item.TypeID;
        Location = item.Location;
        Direction = item.Direction;
        Height = item.Height;
        Extra = item.Extra;
        Data = ItemData.Clone(item.Data);
        SecondsToExpiration = item.SecondsToExpiration;
        Usage = item.Usage;
        OwnerId = item.OwnerId;
        OwnerName = item.OwnerName;
        Identifier = item.Identifier;
    }

    public FloorItem(int id, long typeId, Tile location, int direction, float height, int extra, ItemData data, int secondsToExpiration, FurniUsage usage, int ownerId, string? identifier = null)
    {
        Id = id;
        TypeID = typeId;
        Location = location;
        Direction = direction;
        Height = height;
        Extra = extra;
        Data = data;
        SecondsToExpiration = secondsToExpiration;
        Usage = usage;
        OwnerId = ownerId;
        Identifier = identifier;
    }
    protected FloorItem(in PacketReader p)
    {
        Id = p.ReadInt();
        TypeID = p.ReadInt();
        int x = p.ReadInt();
        int y = p.ReadInt();
        Direction = p.ReadInt();
        float z = p.ReadFloat();
        Location = new Tile(x, y, z);
        if (p.Header.Value == In.Floor_Furni_List.HeaderID)
        {
            Height = (FloatString)p.ReadString();
        }
        else
        {
            var heightval = p.ReadString();
            // check if is a valid string , not ""
            if (heightval != "")
            {
                Height = (FloatString)heightval;
            }
            else
            {
                Height = 0;
            }
        }
        Extra = p.ReadInt();

        Data = p.Parse<ItemData>();

        SecondsToExpiration = p.ReadInt();
        Usage = (FurniUsage)p.ReadInt();
        OwnerId = p.ReadInt();

        if (TypeID < 0)
        {
            Identifier = p.ReadString();
        }
    }



    protected override void Compose(in PacketWriter p)
    {
        p.WriteInt(Id);
        p.WriteInt((int)TypeID);
        p.WriteInt(Location.X);
        p.WriteInt(Location.Y);
        p.WriteInt(Direction);
        p.WriteString((FloatString)Location.Z);
        p.WriteFloat(Height);
        p.WriteInt(Extra);
        p.Compose(Data);
        p.WriteInt(SecondsToExpiration);
        p.WriteInt((int)Usage);
        p.WriteInt(OwnerId);

        if (TypeID < 0)
            p.WriteString(Identifier ?? "");
    }

    public override string ToString() => $"{nameof(FloorItem)}#{Id}/{TypeID}";

    static FloorItem IParser<FloorItem>.Parse(in PacketReader p) => new(in p);
}
