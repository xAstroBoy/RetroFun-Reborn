using System;

using Xabbo.Messages;
using Xabbo.Messages.Nitro;


namespace Xabbo.Core.Messages.Incoming;

/// <summary>
/// Received when a wall item is removed from the room.
/// <para/>
/// Supported clients: <see cref="ClientType.Nitro"/>
/// <para/>
/// Identifiers:
/// <list type="bullet">
/// <item>Flash: <see cref="In.ItemRemove"/></item>
/// <item>Shockwave: <see cref="In.REMOVEITEM"/></item>
/// </list>
/// </summary>
/// <param name="Id">The ID of the wall item that was removed.</param>
/// <param name="PickerId">
/// The ID of the user who picked up the item.
/// Only available on <see cref="ClientType.Modern"/> clients.
/// </param>
public sealed record WallItemRemovedMsg(int Id, int PickerId = default) : IMessage<WallItemRemovedMsg>
{
    static Identifier IMessage<WallItemRemovedMsg>.Identifier => In.Item_Wall_Remove;

    static WallItemRemovedMsg IParser<WallItemRemovedMsg>.Parse(in PacketReader p)
    {
        int id, pickerId = 0;

        string strId = p.ReadString();
        if (!int.TryParse(strId, out id))
            throw new FormatException($"Failed to parse Id in WallItemRemovedMsg: '{strId}'.");

        pickerId = p.ReadInt();

        return new(id, pickerId);
    }

    void IComposer.Compose(in PacketWriter p)
    {
        p.WriteString(Id.ToString());
        p.WriteInt(PickerId);

    }
}
