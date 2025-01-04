using Xabbo.Messages;
using Xabbo.Messages.Nitro;


namespace Xabbo.Core.Messages.Outgoing;

/// <summary>
/// Sent when closing a trade.
/// <para/>
/// Supported clients: <see cref="ClientType.Nitro"/>
/// <para/>
/// Identifiers:
/// <list type="bullet">
/// <item>Flash: <see cref="Out.CloseTrading"/></item>
/// <item>Shockwave: <see cref="Out.TRADE_CLOSE"/></item>
/// </list>
/// </summary>
public sealed record CloseTradeMsg : IMessage<CloseTradeMsg>
{
    static Identifier IMessage<CloseTradeMsg>.Identifier => Out.Trade_Close;
    static CloseTradeMsg IParser<CloseTradeMsg>.Parse(in PacketReader p) => new();
    void IComposer.Compose(in PacketWriter p) { }
}
