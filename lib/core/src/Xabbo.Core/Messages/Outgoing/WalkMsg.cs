using Xabbo.Messages;
using Xabbo.Messages.Nitro;


namespace Xabbo.Core.Messages.Outgoing;

/// <summary>
/// Sent when clicking a tile in a room to walk to.
/// <para/>
/// Supported clients: <see cref="ClientType.Nitro"/>
/// <para/>
/// Identifiers:
/// <list type="bullet">
/// <item>Flash: <see cref="Out.MoveAvatar"/></item>
/// <item>Shockwave: <see cref="Out.MOVE"/></item>
/// </list>
/// </summary>
/// <param name="Point">The coordinates of the tile.</param>
public sealed record WalkMsg(Point Point) : IMessage<WalkMsg>
{
    static Identifier IMessage<WalkMsg>.Identifier => Out.Walk;

    public WalkMsg(int x, int y) : this(new Point(x, y)) { }

    public int X => Point.X;
    public int Y => Point.Y;

    static WalkMsg IParser<WalkMsg>.Parse(in PacketReader p) => p.Client switch
    {
        _ => new(p.ReadInt(), p.ReadInt()),
    };

    void IComposer.Compose(in PacketWriter p)
    {
        p.WriteInt(Point.X);
        p.WriteInt(Point.Y);
    }
}
