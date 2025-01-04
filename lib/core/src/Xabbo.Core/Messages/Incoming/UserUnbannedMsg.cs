using Xabbo.Messages;
using Xabbo.Messages.Nitro;


namespace Xabbo.Core.Messages.Incoming;

/// <summary>
/// Received when a user is unbanned from a room.
/// <para/>
/// Response for <see cref="Outgoing.UnbanUserMsg"/>.
/// <para/>
/// Supported clients: <see cref="ClientType.Modern"/>
/// <para/>
/// Identifiers:
/// <list type="bullet">
/// <item>Flash: <see cref="In.UserUnbannedFromRoom"/></item>
/// </list>
/// </summary>
/// <param name="RoomId">The ID of the room that the user was unbanned from.</param>
/// <param name="UserId">The ID of the user that was unbanned.</param>
public sealed record UserUnbannedMsg(int RoomId, int UserId) : IMessage<UserUnbannedMsg>
{
    static ClientType IMessage<UserUnbannedMsg>.SupportedClients => ClientType.Nitro;
    static Identifier IMessage<UserUnbannedMsg>.Identifier => In.Room_Ban_Remove;
    static UserUnbannedMsg IParser<UserUnbannedMsg>.Parse(in PacketReader p) => new(
        RoomId: p.ReadInt(),
        UserId: p.ReadInt()
    );
    void IComposer.Compose(in PacketWriter p)
    {
        p.WriteInt(RoomId);
        p.WriteInt(UserId);
    }
}
