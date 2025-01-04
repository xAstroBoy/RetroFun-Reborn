using System;

using Xabbo.Messages;
using Xabbo.Messages.Nitro;


namespace Xabbo.Core.Messages.Outgoing;

/// <summary>
/// Sent when moving a floor item.
/// <para/>
/// Supported clients: <see cref="ClientType.Nitro"/>
/// <para/>
/// Identifiers:
/// <list type="bullet">
/// <item>Flash: <see cref="Out.MoveObject"/></item>
/// <item>Shockwave: <see cref="Out.MOVESTUFF"/></item>
/// </list>
/// </summary>
/// <param name="Id">The ID of the floor item to move.</param>
/// <param name="Location">The location to move the floor item to.</param>
/// <param name="Direction">The direction to set the floor item to.</param>
public sealed record MoveFloorItemMsg(int Id, Point Location, int Direction) : IMessage<MoveFloorItemMsg>
{
    const int ExpectedFieldCount = 4;

    static Identifier IMessage<MoveFloorItemMsg>.Identifier => Out.Floor_MoveObject;

    /// <summary>
    /// Gets the X coordinate.
    /// </summary>
    public int X => Location.X;

    /// <summary>
    /// Gets the Y coordinate.
    /// </summary>
    public int Y => Location.Y;

    public MoveFloorItemMsg(int Id, int X, int Y, int Direction)
        : this(Id, (X, Y), Direction)
    { }

    static MoveFloorItemMsg IParser<MoveFloorItemMsg>.Parse(in PacketReader p)
    {
        int id;
        int x, y, direction;

        id = p.ReadInt();
        x = p.ReadInt();
        y = p.ReadInt();
        direction = p.ReadInt();


        return new MoveFloorItemMsg(id, x, y, direction);
    }

    void IComposer.Compose(in PacketWriter p)
    {
        p.WriteInt(Id);
        p.WriteInt(X);
        p.WriteInt(Y);
        p.WriteInt(Direction);
    }
}
