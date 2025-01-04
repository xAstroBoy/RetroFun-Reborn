using Xabbo.Messages;
using Xabbo.Messages.Nitro;


namespace Xabbo.Core.Messages.Outgoing;

/// <summary>
/// Sent when opening a trade with a user.
/// <para/>
/// Supported clients: <see cref="ClientType.Nitro"/>
/// <para/>
/// Identifiers:
/// <list type="bullet">
/// <item>Flash: <see cref="Out.OpenTrading"/></item>
/// <item>Shockwave: <see cref="Out.TRADE_OPEN"/></item>
/// </list>
/// </summary>
/// <param name="UserIndex">The avatar index of the user to trade.</param>
public sealed record TradeUserMsg(int UserIndex) : IMessage<TradeUserMsg>
{
    static Identifier IMessage<TradeUserMsg>.Identifier => Out.Trade;

    static TradeUserMsg IParser<TradeUserMsg>.Parse(in PacketReader p) => new(p.Client switch
    {
        _ => p.ReadInt()
    });

    void IComposer.Compose(in PacketWriter p)
    {
        p.WriteInt(UserIndex);
    }
}
