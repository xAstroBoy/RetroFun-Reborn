using Xabbo.Messages;
using Xabbo.Messages.Nitro;


namespace Xabbo.Core.Messages.Incoming;

/// <summary>
/// Received when a wall item is updated.
/// <para/>
/// Supported clients: <see cref="ClientType.Nitro"/>
/// <para/>
/// Identifiers:
/// <list type="bullet">
/// <item>Flash: <see cref="In.ItemUpdate"/></item>
/// <item>Shockwave: <see cref="In.UPDATEITEM"/></item>
/// </list>
/// </summary>
/// <param name="Item">The updated wall item.</param>
public sealed record WallItemUpdatedMsg(WallItem Item) : IMessage<WallItemUpdatedMsg>
{
    static Identifier IMessage<WallItemUpdatedMsg>.Identifier => In.Item_Wall_Update;
    static WallItemUpdatedMsg IParser<WallItemUpdatedMsg>.Parse(in PacketReader p) => new(p.Parse<WallItem>());
    void IComposer.Compose(in PacketWriter p) => p.Compose(Item);
}
