
using Xabbo.Messages;
using Xabbo.Messages.Nitro;


namespace Xabbo.Core.Messages.Incoming;

/// <summary>
/// Received when a console message is received from a friend.
/// <para/>
/// Supported clients: <see cref="ClientType.Nitro"/>
/// <para/>
/// Identifiers:
/// <list type="bullet">
/// <item>Flash: <see cref="In.NewConsole"/></item>
/// <item>Shockwave: <see cref="In.MESSENGER_MESSAGE"/></item>
/// </list>
/// </summary>
public sealed record ConsoleMessageMsg(ConsoleMessage Message) : IMessage<ConsoleMessageMsg>
{
    static Identifier IMessage<ConsoleMessageMsg>.Identifier => Identifier.Unknown; // TODO : Restore In.Messenger_Chat
    static ConsoleMessageMsg IParser<ConsoleMessageMsg>.Parse(in PacketReader p) => new(p.Parse<ConsoleMessage>());
    void IComposer.Compose(in PacketWriter p) => p.Compose(Message);
}
