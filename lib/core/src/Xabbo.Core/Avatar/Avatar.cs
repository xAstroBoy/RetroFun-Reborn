using System;
using System.Text.Json.Serialization;

using Xabbo.Messages;

namespace Xabbo.Core;

/// <inheritdoc cref="IAvatar"/>
public abstract class Avatar(AvatarType type, int id, int index) : IAvatar, IParserComposer<Avatar>
{
    public bool IsRemoved { get; set; }
    public bool IsHidden { get; set; }

    public AvatarType Type { get; } = type;

    public int Id { get; } = id;
    public int Index { get; } = index;

    public string Name { get; set; } = "";
    public string Motto { get; set; } = "";
    public string Figure { get; set; } = "";

    public Tile Location { get; set; } = default;
    [JsonIgnore] public int X => Location.X;
    [JsonIgnore] public int Y => Location.Y;
    [JsonIgnore] public Point XY => Location.XY;
    [JsonIgnore] public float Z => Location.Z;
    public int Direction { get; set; }
    public Point? Size => (1, 1);
    public Area? Area => new(Location.XY, 1, 1);

    // States
    public int HeadDirection { get; set; }
    public AvatarDance Dance { get; set; }
    public bool IsIdle { get; set; }
    public bool IsTyping { get; set; }
    public int HandItem { get; set; }
    public int Effect { get; set; }

    public AvatarStatus? CurrentUpdate { get; private set; }
    IAvatarStatus? IAvatar.CurrentUpdate => CurrentUpdate;
    public AvatarStatus? PreviousUpdate { get; private set; }
    IAvatarStatus? IAvatar.PreviousUpdate => PreviousUpdate;

    public void Update(AvatarStatus update)
    {
        if (update.Index != Index)
            throw new InvalidOperationException("Avatar update index does not match the index of this avatar.");

        Location = update.Location;
        Direction = update.Direction;
        HeadDirection = update.HeadDirection;

        OnUpdate(update);

        PreviousUpdate = CurrentUpdate;
        CurrentUpdate = update;
    }

    protected virtual void OnUpdate(AvatarStatus update) { }

    public virtual void Compose(in PacketWriter p)
    {
        p.WriteInt(Id);
        p.WriteString(Name);
        p.WriteString(Motto);
        p.WriteString(Figure);
        p.WriteInt(Index);
        p.Compose(Location);
        p.WriteInt(Direction);
        p.WriteInt((int)Type);
    }

    public override string ToString() => Name;

    static Avatar IParser<Avatar>.Parse(in PacketReader p)
    {
        int id;
        string name, motto, figure, gender, poolFigure, badgeCode;
        int index, dir;
        Tile tile;
        AvatarType type;

        id = p.ReadInt();
        name = p.ReadString();
        motto = p.ReadString();
        figure = p.ReadString();
        index = p.ReadInt();
        tile = p.Parse<Tile>();
        dir = p.ReadInt();

        type = (AvatarType)p.ReadInt();
        Avatar avatar = type switch
        {
            AvatarType.User => new User(id, index, in p),
            AvatarType.Pet => new Pet(id, index, in p),
            AvatarType.PublicBot or AvatarType.PrivateBot => new Bot(type, id, index, in p),
            _ => throw new Exception($"Unknown avatar type: {type}"),
        };

        avatar.Name = name;
        avatar.Motto = motto;
        avatar.Figure = figure;
        avatar.Location = tile;
        avatar.Direction = dir;
        avatar.HeadDirection = dir;

        return avatar;
    }
}
