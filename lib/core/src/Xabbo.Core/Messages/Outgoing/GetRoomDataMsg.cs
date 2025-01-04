using Xabbo.Messages;

using Xabbo.Core.Messages.Incoming;
using Xabbo.Messages.Nitro;

namespace Xabbo.Core.Messages.Outgoing;

/// <summary>
/// Sent when requesting room data for the specified room ID.
/// <para/>
/// Request for <see cref="RoomDataMsg"/>. Returns <see cref="RoomData"/>.
/// <para/>
/// Supported clients: <see cref="ClientType.Nitro"/>
/// <para/>
/// Identifiers:
/// <list type="bullet">
/// <item>Flash: <see cref="Out.GetGuestRoom"/></item>
/// <item>Shockwave: <see cref="Out.GETFLATINFO"/></item>
/// </list>
/// </summary>
/// <param name="Id">The room ID to request data for.</param>
/// <param name="Entering">Whether the user is entering the room. Does not apply to Origins.</param>
/// <param name="Forward">Whether to forward the user to the room in-client. Does not apply to Origins.</param>
public sealed record GetRoomDataMsg(int Id, bool Entering = false, bool Forward = false)
    : IRequestMessage<GetRoomDataMsg, RoomDataMsg, RoomData>
{
    static Identifier IMessage<GetRoomDataMsg>.Identifier => Out.Get_Guest_Room;
    bool IRequestFor<RoomDataMsg>.MatchResponse(RoomDataMsg msg) => msg.Data.Id == Id;
    RoomData IResponseData<RoomDataMsg, RoomData>.GetData(RoomDataMsg msg) => msg.Data;

    static GetRoomDataMsg IParser<GetRoomDataMsg>.Parse(in PacketReader p) => p.Client switch
    {
        _ => new(
            Id: p.ReadInt(),
            Entering: p.ReadInt() != 0,
            Forward: p.ReadInt() != 0
        )
    };

    void IComposer.Compose(in PacketWriter p)
    {
        p.WriteInt(Id);
        p.WriteInt(Entering ? 1 : 0);
        p.WriteInt(Forward ? 1 : 0);
    }
}
