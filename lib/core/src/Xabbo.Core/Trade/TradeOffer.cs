using System;
using System.Collections.Generic;

using Xabbo.Messages;

namespace Xabbo.Core;

/// <inheritdoc cref="ITradeOffer"/>
public sealed class TradeOffer : List<TradeItem>, ITradeOffer, IParserComposer<TradeOffer>
{
    public int? UserId { get; set; }
    public string? UserName { get; set; }
    /// <remarks>
    /// This appears to be an unused field in the Origins packet structure as users will
    /// automatically unaccept the trade whenever a trade offer updates.
    /// </remarks>
    public bool Accepted { get; set; }
    public int FurniCount { get; set; }
    public int CreditCount { get; set; }

    ITradeItem IReadOnlyList<ITradeItem>.this[int index] => this[index];
    IEnumerator<ITradeItem> IEnumerable<ITradeItem>.GetEnumerator() => GetEnumerator();

    public TradeOffer() { }

    private TradeOffer(in PacketReader p) : this()
    {
        UserId = p.ReadInt();

        AddRange(p.ParseArray<TradeItem>());

        FurniCount = p.ReadInt();
        CreditCount = p.ReadInt();
    }

    void IComposer.Compose(in PacketWriter p)
    {
        p.WriteInt(UserId ?? throw new Exception($"{nameof(UserId)} is required on {p.Client}."));

        p.ComposeArray<TradeItem>(this);

        p.WriteInt(FurniCount);
        p.WriteInt(CreditCount);
    }

    static TradeOffer IParser<TradeOffer>.Parse(in PacketReader p) => new(in p);
}
