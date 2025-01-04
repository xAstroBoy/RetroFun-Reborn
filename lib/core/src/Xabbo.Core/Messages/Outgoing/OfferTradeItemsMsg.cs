using System.Collections.Generic;
using System.Linq;

using Xabbo.Messages;
using Xabbo.Messages.Nitro;


namespace Xabbo.Core.Messages.Outgoing;

/// <summary>
/// Sent when offering items in a trade.
/// <para/>
/// Supported clients: <see cref="ClientType.Modern"/>.
/// <para/>
/// Identifiers:
/// <list type="bullet">
/// <item>Flash: <see cref="Out.AddItemsToTrade"/></item>
/// </list>
/// </summary>
public sealed class OfferTradeItemsMsg : List<int>, IMessage<OfferTradeItemsMsg>
{
    public OfferTradeItemsMsg() { }
    public OfferTradeItemsMsg(IEnumerable<int> itemIds) : base(itemIds) { }
    public OfferTradeItemsMsg(IEnumerable<IInventoryItem> items) : base((IEnumerable<int>)items.Select(item => item.ItemId)) { }

    static ClientType IMessage<OfferTradeItemsMsg>.SupportedClients => ClientType.Nitro;
    static Identifier IMessage<OfferTradeItemsMsg>.Identifier => Out.Trade_Item;
    static OfferTradeItemsMsg IParser<OfferTradeItemsMsg>.Parse(in PacketReader p) => new(p.ReadIntArray());
    void IComposer.Compose(in PacketWriter p) => p.WriteIntArray(this);
}
