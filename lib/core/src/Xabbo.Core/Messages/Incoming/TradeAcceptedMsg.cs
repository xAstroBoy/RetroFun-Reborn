using System;
using Xabbo.Messages;
using Xabbo.Messages.Nitro;


namespace Xabbo.Core.Messages.Incoming;

/// <summary>
/// Received when a user accepts or unaccepts at trade.
/// <para/>
/// Supported clients: <see cref="ClientType.Nitro"/>
/// <para/>
/// Identifiers:
/// <list type="bullet">
/// <item>Flash: <see cref="In.TradingAccept"/></item>
/// <item>Shockwave: <see cref="In.TRADE_ACCEPT"/></item>
/// </list>
/// </summary>
/// <param name="UserId">The ID of the user who accepted or unaccepted. Used on modern clients.</param>
/// <param name="UserName">The name of the user who accepted or unaccepted. Used on the Shockwave client.</param>
/// <param name="Accepted">Whether the user accepted.</param>
public sealed record TradeAcceptedMsg(int? UserId, string? UserName, bool Accepted) : IMessage<TradeAcceptedMsg>
{
    static Identifier IMessage<TradeAcceptedMsg>.Identifier => In.Trade_Accepted;

    static TradeAcceptedMsg IParser<TradeAcceptedMsg>.Parse(in PacketReader p)
    {
        return new(p.ReadInt(), null, p.ReadInt() != 0);

    }

    void IComposer.Compose(in PacketWriter p)
    {
        if (UserId is not { } userId)
            throw new Exception($"{nameof(UserId)} is required on the {p.Client} client.");
        p.WriteInt(userId);
        p.WriteInt(Accepted ? 1 : 0);
    }
}
