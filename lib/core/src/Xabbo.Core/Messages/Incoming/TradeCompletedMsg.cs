using Xabbo.Messages;
using Xabbo.Messages.Nitro;


namespace Xabbo.Core.Messages.Incoming;

/// <summary>
/// Received when a trade completes successfully.
/// <para/>
/// Supported clients: <see cref="ClientType.Nitro"/>
/// <para/>
/// Identifiers:
/// <list type="bullet">
/// <item>Flash: <see cref="In.TradingCompleted"/></item>
/// <item>Shockwave: <see cref="In.TRADE_COMPLETED_2"/></item>
/// </list>
/// </summary>
public sealed record TradeCompletedMsg : IMessage<TradeCompletedMsg>
{
    static Identifier IMessage<TradeCompletedMsg>.Identifier => In.Trade_Completed;
    static TradeCompletedMsg IParser<TradeCompletedMsg>.Parse(in PacketReader p) => new();
    void IComposer.Compose(in PacketWriter p) { }
}
