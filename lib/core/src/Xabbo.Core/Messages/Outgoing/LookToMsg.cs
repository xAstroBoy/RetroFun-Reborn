using System;

using Xabbo.Messages;
using Xabbo.Messages.Nitro;


namespace Xabbo.Core.Messages.Outgoing;

/// <summary>
/// Sent when looking towards a tile.
/// <para/>
/// Supported clients: <see cref="ClientType.Nitro"/>
/// <para/>
/// Identifiers:
/// <list type="bullet">
/// <item>Flash: <see cref="Out.LookTo"/></item>
/// <item>Shockwave: <see cref="Out.LOOKTO"/></item>
/// </list>
/// </summary>
/// <param name="Point">The point to look towards.</param>
public sealed record LookToMsg(Point Point) : IMessage<LookToMsg>
{
    public static Identifier Identifier => Out.Look;

    public LookToMsg(int X, int Y) : this(new Point(X, Y)) { }

    /// <summary>
    /// Gets the X coordinate.
    /// </summary>
    public int X => Point.X;

    /// <summary>
    /// Gets the Y coordinate.
    /// </summary>
    public int Y => Point.Y;

    static LookToMsg IParser<LookToMsg>.Parse(in PacketReader p)
    {
        int x, y;
        x = p.ReadInt();
        y = p.ReadInt();

        return new LookToMsg((x, y));
    }

    void IComposer.Compose(in PacketWriter p)
    {
        p.WriteInt(Point.X);
        p.WriteInt(Point.Y);
    }
}
