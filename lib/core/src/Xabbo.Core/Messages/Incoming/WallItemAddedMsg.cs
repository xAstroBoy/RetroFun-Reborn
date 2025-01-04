using Xabbo.Messages;
using Xabbo.Messages.Nitro;


namespace Xabbo.Core.Messages.Incoming;

/// <summary>
/// Received when a wall item is added to the room.
/// <para/>
/// Supported clients: <see cref="ClientType.Nitro"/>
/// <para/>
/// Identifiers:
/// <list type="bullet">
/// <item>Flash: <see cref="In.ItemAdd"/></item>
/// <item>Shockwave: <see cref="In.ITEMS_2"/></item>
/// </list>
/// </summary>
/// <param name="Item">The wall item that was added.</param>
public sealed record WallItemAddedMsg(WallItem Item) : IMessage<WallItemAddedMsg>
{
    static Identifier IMessage<WallItemAddedMsg>.Identifier => In.Item_Wall_Add;
    static WallItemAddedMsg IParser<WallItemAddedMsg>.Parse(in PacketReader p)
    {
        var item = p.Parse<WallItem>();
        item.OwnerName = p.ReadString();
        return new(item);
    }
    void IComposer.Compose(in PacketWriter p)
    {
        p.Compose(Item);
        p.WriteString(Item.OwnerName);
    }
}
