using System;

using Xabbo.Messages;
using Xabbo.Messages.Nitro;


namespace Xabbo.Core.Messages.Incoming;

/// <summary>
/// Received when a floor item is removed from the room.
/// <para/>
/// Supported clients: <see cref="ClientType.Nitro"/>
/// <para/>
/// Identifiers:
/// <list type="bullet">
/// <item>Flash: <see cref="In.ObjectRemove"/></item>
/// <item>Shockwave: <see cref="In.ACTIVEOBJECT_REMOVE"/></item>
/// </list>
/// </summary>
/// <param name="Id">The ID of the floor item that was removed.</param>
/// <param name="Item">The instance of the floor item that was removed. Only available on <see cref="ClientType.Shockwave"/></param>
/// <param name="Expired">Whether the item expired or not. Applies to <see cref="ClientType.Modern"/> clients.</param>
/// <param name="PickerId">The ID of the user who picked up the item. Applies to <see cref="ClientType.Modern"/> clients.</param>
/// <param name="Delay">The delay in milliseconds after which the item will be removed by the client. Applies to <see cref="ClientType.Modern"/> clients.</param>
public sealed record FloorItemRemovedMsg(
    int Id,
    FloorItem? Item = null,
    bool Expired = false,
    int PickerId = default,
    int Delay = 0
)
    : IMessage<FloorItemRemovedMsg>
{
    public FloorItemRemovedMsg(FloorItem Item, bool Expired = false, int PickerId = default, int Delay = 0)
        : this(Item.Id, Item, Expired, PickerId, Delay) { }

    static Identifier IMessage<FloorItemRemovedMsg>.Identifier => In.Floor_Remove;

    static FloorItemRemovedMsg IParser<FloorItemRemovedMsg>.Parse(in PacketReader p)
    {
        int id;
        FloorItem? item = null;
        bool expired = false;
        int pickerId = 0;
        int delay = 0;

        id = p.ReadInt();
        expired = p.ReadBool();
        pickerId = p.ReadInt();
        delay = p.ReadInt();


        return new(id, item);
    }

    void IComposer.Compose(in PacketWriter p)
    {
        p.WriteString(Id.ToString());
        p.WriteBool(Expired);
        p.WriteInt(PickerId);
        p.WriteInt(Delay);

    }
}
