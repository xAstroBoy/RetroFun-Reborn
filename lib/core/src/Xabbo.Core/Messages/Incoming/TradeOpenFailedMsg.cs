using Xabbo.Messages;
using Xabbo.Messages.Nitro;


namespace Xabbo.Core.Messages.Incoming;

/// <summary>
/// Received when a trade fails to open.
/// <para/>
/// Supported clients: <see cref="ClientType.Modern"/>
/// <para/>
/// Identifiers:
/// <list type="bullet">
/// <item>Flash: <see cref="In.TradeOpenFailed"/></item>
/// </list>
/// </summary>
/// <param name="Reason">Indicates the reason that the trade failed to open.</param>
/// <param name="UserName">The name of the user that the trade failed to open with.</param>
public sealed record TradeOpenFailedMsg(int Reason, string UserName) : IMessage<TradeOpenFailedMsg>
{
    static ClientType IMessage<TradeOpenFailedMsg>.SupportedClients => ClientType.Nitro;
    static Identifier IMessage<TradeOpenFailedMsg>.Identifier => In.Trade_Open_Failed;
    static TradeOpenFailedMsg IParser<TradeOpenFailedMsg>.Parse(in PacketReader p) => new(p.ReadInt(), p.ReadString());
    void IComposer.Compose(in PacketWriter p)
    {
        p.WriteInt(Reason);
        p.WriteString(UserName);
    }
}
