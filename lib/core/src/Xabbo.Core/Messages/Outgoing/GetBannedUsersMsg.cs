using Xabbo.Messages;

using Xabbo.Core.Messages.Incoming;
using Xabbo.Messages.Nitro;

namespace Xabbo.Core.Messages.Outgoing;

/// <summary>
/// Sent when requesting the list of users banned from a room.
/// <para/>
/// Request for <see cref="BannedUsersMsg"/>. Returns the list of banned users as an array of <see cref="IdName"/>.
/// <para/>
/// Supported clients: <see cref="ClientType.Modern"/>
/// <para/>
/// Identifiers:
/// <list type="bullet">
/// <item>Flash: <see cref="Out.GetBannedUsersFromRoom"/></item>
/// </list>
/// </summary>
/// <param name="RoomId">The ID of the room to request banned users for.</param>
public sealed record GetBannedUsersMsg(int RoomId) : IRequestMessage<GetBannedUsersMsg, BannedUsersMsg, IdName[]>
{
    static ClientType IMessage<GetBannedUsersMsg>.SupportedClients => ClientType.Nitro;
    static Identifier IMessage<GetBannedUsersMsg>.Identifier => Out.Room_Ban_List;
    bool IRequestFor<BannedUsersMsg>.MatchResponse(BannedUsersMsg msg) => msg.RoomId == RoomId;
    IdName[] IResponseData<BannedUsersMsg, IdName[]>.GetData(BannedUsersMsg msg) => [.. msg.Users];
    static GetBannedUsersMsg IParser<GetBannedUsersMsg>.Parse(in PacketReader p) => new(p.ReadInt());
    void IComposer.Compose(in PacketWriter p) => p.WriteInt(RoomId);
}
