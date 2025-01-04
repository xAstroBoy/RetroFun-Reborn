using System;

using Xabbo.Messages;
using Xabbo.Messages.Nitro;


namespace Xabbo.Core.Messages.Outgoing;

/// <summary>
/// Sent when banning a user from a room.
/// <para/>
/// Supported clients: <see cref="ClientType.Nitro"/>
/// <para/>
/// Identifiers:
/// <list type="bullet">
/// <item>Flash: <see cref="Out.BanUserWithDuration"/></item>
/// <item>Shockwave: <see cref="Out.ROOMBAN"/></item>
/// </list>
/// </summary>
/// <param name="Id">The ID of the user to ban. Applies to <see cref="ClientType.Modern"/> clients.</param>
/// <param name="Name">The name of the user to ban. Applies to the <see cref="ClientType.Origins"/> client.</param>
/// <param name="RoomId">The ID of the room to ban the user from. Applies to <see cref="ClientType.Modern"/> clients.</param>
/// <param name="Duration">The duration of the ban.</param>
/// <param name="DurationString">A custom ban duration string. Used when <see cref="Duration"/> is -1.</param>
public sealed record BanUserMsg(
    int? Id,
    string? Name,
    int? RoomId,
    BanDuration Duration,
    string? DurationString = null
)
    : IMessage<BanUserMsg>
{
    /// <summary>
    /// Constructs a new <see cref="BanUserMsg"/> with the specified user, room ID and duration.
    /// </summary>
    /// <param name="user">The user to ban.</param>
    /// <param name="roomId"><inheritdoc cref="BanUserMsg" path="/param[@name='RoomId']"/></param>
    /// <param name="duration"><inheritdoc cref="BanUserMsg" path="/param[@name='Duration']"/></param>
    public BanUserMsg(IUser user, int roomId, BanDuration duration = BanDuration.Permanent)
        : this(user.Id, user.Name, roomId, duration) { }

    /// <summary>
    /// Constructs a new <see cref="BanUserMsg"/> with the specified user/ID pair, room ID and duration.
    /// </summary>
    /// <param name="user">The ID and name of the user to ban.</param>
    /// <param name="roomId"><inheritdoc cref="BanUserMsg" path="/param[@name='RoomId']"/></param>
    /// <param name="duration"><inheritdoc cref="BanUserMsg" path="/param[@name='Duration']"/></param>
    public BanUserMsg(IdName user, int roomId, BanDuration duration = BanDuration.Permanent)
        : this(user.Id, user.Name, roomId, duration) { }

    static Identifier IMessage<BanUserMsg>.Identifier => Out.Room_Ban_Give;

    static BanUserMsg IParser<BanUserMsg>.Parse(in PacketReader p)
    {
        int? id = null;
        int? roomId = null;
        string? name = null;
        BanDuration duration;

        id = p.ReadInt();
        roomId = p.ReadInt();

        string durationString = p.ReadString();

        duration = durationString switch
        {
            string s when s.Equals(BanDuration.Hour.ToClientString(p.Client)) => BanDuration.Day,
            string s when s.Equals(BanDuration.Day.ToClientString(p.Client)) => BanDuration.Day,
            string s when s.Equals(BanDuration.Permanent.ToClientString(p.Client)) => BanDuration.Permanent,
            _ => (BanDuration)(-1)
        };

        return new BanUserMsg(id, name, roomId, duration, duration == (BanDuration)(-1) ? durationString : null);
    }

    void IComposer.Compose(in PacketWriter p)
    {
        if (Id is not { } id)
            throw new Exception($"{nameof(Id)} is required when composing {nameof(BanUserMsg)} on {p.Client}.");
        if (RoomId is not { } roomId)
            throw new Exception($"{nameof(RoomId)} is required when composing {nameof(BanUserMsg)} on {p.Client}.");
        p.WriteInt(id);
        p.WriteInt(roomId);

        if (Duration == (BanDuration)(-1))
            p.WriteString(DurationString ?? "");
        else
            p.WriteString(Duration.ToClientString(p.Client));
    }
}
