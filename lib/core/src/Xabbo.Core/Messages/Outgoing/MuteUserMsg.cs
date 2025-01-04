using Xabbo.Messages;
using Xabbo.Messages.Nitro;


namespace Xabbo.Core.Messages.Outgoing;

/// <summary>
/// Sent when muting a user in a room.
/// <para/>
/// Supported clients: <see cref="ClientType.Modern"/>
/// <para/>
/// Identifiers:
/// <list type="bullet">
/// <item>Flash: <see cref="Out.MuteUser"/></item>
/// </list>
/// </summary>
/// <param name="Id">The ID of the user to mute.</param>
/// <param name="RoomId">The ID of the room to mute the user in.</param>
/// <param name="Minutes">The duration in minutes to mute the user for.</param>
public sealed record MuteUserMsg(int Id, int RoomId, int Minutes) : IMessage<MuteUserMsg>
{
    static ClientType IMessage<MuteUserMsg>.SupportedClients => ClientType.Nitro;
    static Identifier IMessage<MuteUserMsg>.Identifier => Out.Room_Mute_User;

    /// <summary>
    /// Constructs a new <see cref="MuteUserMsg"/> with the specified user, room ID and duration.
    /// </summary>
    /// <param name="user">The user to mute.</param>
    /// <param name="roomId"><inheritdoc cref="MuteUserMsg" path="/param[@name='RoomId']"/></param>
    /// <param name="minutes"><inheritdoc cref="MuteUserMsg" path="/param[@name='Minutes']"/></param>
    public MuteUserMsg(IUser user, int roomId, int minutes) : this(user.Id, roomId, minutes) { }

    static MuteUserMsg IParser<MuteUserMsg>.Parse(in PacketReader p) => new(p.ReadInt(), p.ReadInt(), p.ReadInt());
    void IComposer.Compose(in PacketWriter p)
    {
        p.WriteInt(Id);
        p.WriteInt(RoomId);
        p.WriteInt(Minutes);
    }
}
