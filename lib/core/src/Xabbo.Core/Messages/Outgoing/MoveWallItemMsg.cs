using Xabbo.Messages;
using Xabbo.Messages.Nitro;


namespace Xabbo.Core.Messages.Outgoing;

/// <summary>
/// Sent when moving a wall item.
/// <para/>
/// Supported clients: <see cref="ClientType.Nitro"/>
/// <para/>
/// Identifiers:
/// <list type="bullet">
/// <item>Flash: <see cref="Out.MoveWallItem"/></item>
/// <item>Shockwave: <see cref="Out.MOVEITEM"/></item>
/// </list>
/// </summary>
/// <param name="Id">The ID of the wall item to move.</param>
/// <param name="Location">The location to move the wall item to.</param>
public sealed record MoveWallItemMsg(int Id, WallLocation Location) : IMessage<MoveWallItemMsg>
{
    static Identifier IMessage<MoveWallItemMsg>.Identifier => Out.Move_WallItem;

    static MoveWallItemMsg IParser<MoveWallItemMsg>.Parse(in PacketReader p) => new(p.ReadInt(), (WallLocation)p.ReadString());

    void IComposer.Compose(in PacketWriter p)
    {
        p.WriteInt(Id);
        p.WriteString(Location.ToString());
    }
}
