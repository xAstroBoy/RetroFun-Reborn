using Xabbo.Messages;
using Xabbo.Messages.Nitro;


namespace Xabbo.Core.Messages.Incoming;

/// <summary>
/// Received when an item is removed from the user's inventory.
/// <para/>
/// Supported clients: <see cref="ClientType.Nitro"/>
/// <para/>
/// Identifiers:
/// <list type="bullet">
/// <item>Flash: <see cref="In.FurniListRemove"/></item>
/// <item>Shockwave: <see cref="In.REMOVESTRIPITEM"/></item>
/// </list>
/// </summary>
/// <param name="ItemId">The ID of the item that was removed.</param>
public sealed record InventoryItemRemovedMsg(int ItemId) : IMessage<InventoryItemRemovedMsg>
{
    static Identifier IMessage<InventoryItemRemovedMsg>.Identifier => In.User_Furniture_Remove;
    static InventoryItemRemovedMsg IParser<InventoryItemRemovedMsg>.Parse(in PacketReader p) => new(p.ReadInt());
    void IComposer.Compose(in PacketWriter p) => p.WriteInt(ItemId);
}
