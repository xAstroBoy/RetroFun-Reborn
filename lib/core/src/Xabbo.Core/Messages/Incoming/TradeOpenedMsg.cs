using Xabbo.Messages;
using Xabbo.Messages.Nitro;


namespace Xabbo.Core.Messages.Incoming;

/// <summary>
/// Received when a trade is opened.
/// <para/>
/// Supported clients: <see cref="ClientType.Modern"/>
/// <para/>
/// Identifiers:
/// <list type="bullet">
/// <item>Flash: <see cref="In.TradingOpen"/></item>
/// </list>
/// </summary>
/// <param name="TraderId">The ID of the user who initiated the trade.</param>
/// <param name="TraderCanTrade">Whether the trader can trade.</param>
/// <param name="TradeeId">The ID of the user who received the trade.</param>
/// <param name="TradeeCanTrade">Whether the tradee can trade.</param>
public sealed record TradeOpenedMsg(
    int TraderId, bool TraderCanTrade,
    int TradeeId, bool TradeeCanTrade
)
    : IMessage<TradeOpenedMsg>
{
    static ClientType IMessage<TradeOpenedMsg>.SupportedClients => ClientType.Nitro;
    static Identifier IMessage<TradeOpenedMsg>.Identifier => In.Trade_Open;

    static TradeOpenedMsg IParser<TradeOpenedMsg>.Parse(in PacketReader p) => new(
        p.ReadInt(), p.ReadInt() == 1,
        p.ReadInt(), p.ReadInt() == 1
    );

    void IComposer.Compose(in PacketWriter p)
    {
        p.WriteInt(TraderId);
        p.WriteInt(TraderCanTrade ? 1 : 0);
        p.WriteInt(TradeeId);
        p.WriteInt(TradeeCanTrade ? 1 : 0);
    }
}
