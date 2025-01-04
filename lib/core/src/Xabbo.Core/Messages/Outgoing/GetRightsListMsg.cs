using Xabbo.Messages;

using Xabbo.Core.Messages.Incoming;
using Xabbo.Messages.Nitro;

namespace Xabbo.Core.Messages.Outgoing;

/// <summary>
/// Sent when requesting the rights list of a room.
/// <para/>
/// Request for <see cref="RightsListMsg"/>.
/// <para/>
/// Supported clients: <see cref="ClientType.Modern"/>
/// <para/>
/// Identifiers:
/// <list type="bullet">
/// <item>Flash: <see cref="Out.GetFlatControllers"/></item>
/// </list>
/// </summary>
/// <param name="RoomId">The ID of the room to request the rights list for.</param>
public sealed record GetRightsListMsg(int RoomId) : IRequestMessage<GetRightsListMsg, RightsListMsg>
{
    static ClientType IMessage<GetRightsListMsg>.SupportedClients => ClientType.Nitro;
    static Identifier IMessage<GetRightsListMsg>.Identifier => Out.Room_Rights_List;
    bool IRequestFor<RightsListMsg>.MatchResponse(RightsListMsg msg) => msg.RoomId == RoomId;
    static GetRightsListMsg IParser<GetRightsListMsg>.Parse(in PacketReader p) => new(p.ReadInt());
    void IComposer.Compose(in PacketWriter p) => p.WriteInt(RoomId);
}
