using Xabbo.Messages;
using Xabbo.Messages.Nitro;


namespace Xabbo.Core.Messages.Incoming;

/// <summary>
/// Received when a floor item is added to the room.
/// <para/>
/// Supported clients: <see cref="ClientType.Nitro"/>
/// <para/>
/// Identifiers:
/// <list type="bullet">
/// <item>Flash: <see cref="In.ObjectAdd"/></item>
/// <item>Shockwave: <see cref="In.ACTIVEOBJECT_ADD"/></item>
/// </list>
/// </summary>
/// <param name="Item">The floor item that was added.</param>
public sealed record FloorItemAddedMsg(FloorItem Item) : IMessage<FloorItemAddedMsg>
{
    static Identifier IMessage<FloorItemAddedMsg>.Identifier => In.Floor_Add;
    static FloorItemAddedMsg IParser<FloorItemAddedMsg>.Parse(in PacketReader p)
    {
        var item = p.Parse<FloorItem>();
        item.OwnerName = p.ReadString();
        return new(item);
    }
    void IComposer.Compose(in PacketWriter p)
    {
        p.Compose(Item);
        p.WriteString(Item.OwnerName);
    }
}
