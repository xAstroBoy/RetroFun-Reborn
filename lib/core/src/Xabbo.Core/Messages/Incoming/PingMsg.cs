using System.Net.NetworkInformation;
using Xabbo.Messages;
using Xabbo.Messages.Nitro;


namespace Xabbo.Core.Messages.Incoming;

/// <summary>
/// Received periodically to check if the connection is alive.
/// <para/>
/// Supported clients: <see cref="ClientType.Nitro"/>
/// <para/>
/// Identifiers:
/// <list type="bullet">
/// <item>Flash: <see cref="Ping"/></item>
/// <item>Shockwave: <see cref="In.PING"/></item>
/// </list>
/// </summary>
public sealed record PingMsg : IMessage<PingMsg>
{
    static Identifier IMessage<PingMsg>.Identifier => In.Ping;
    static PingMsg IParser<PingMsg>.Parse(in PacketReader p) => new();
    void IComposer.Compose(in PacketWriter p) { }
}
