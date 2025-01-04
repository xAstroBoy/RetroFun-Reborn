using Xabbo.Messages;
using Xabbo.Messages.Nitro;


namespace Xabbo.Core.Messages.Outgoing;

/// <summary>
/// Sent when talking in a room.
/// <para/>
/// Supported clients: <see cref="ClientType.Nitro"/>
/// <para/>
/// Identifiers:
/// <list type="bullet">
/// <item>Flash: <see cref="Out.Chat"/></item>
/// <item>Shockwave: <see cref="Out.CHAT"/></item>
/// </list>
/// </summary>
/// <param name="Message">The chat message content.</param>
/// <param name="BubbleStyle">The chat bubble style. Applies to <see cref="ClientType.Modern"/> clients.</param>
/// <param name="TrackingId">The tracking ID of the message.</param>
public sealed record TalkMsg(string Message, int BubbleStyle = 0, int TrackingId = -1) : IMessage<TalkMsg>
{
    static Identifier IMessage<TalkMsg>.Identifier => Out.Chat;

    static TalkMsg IParser<TalkMsg>.Parse(in PacketReader p) => new(
        p.ReadString(),
        p.ReadInt(), 
        p.ReadInt()
    );

    void IComposer.Compose(in PacketWriter p)
    {
        p.WriteString(Message);
        p.WriteInt(BubbleStyle);
        p.WriteInt(TrackingId);
    }
}
