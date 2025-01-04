using System;

using Xabbo.Messages;
using Xabbo.Messages.Nitro;


namespace Xabbo.Core.Messages.Outgoing;

/// <summary>
/// Sent when clicking a furni.
/// <para/>
/// Supported clients: <see cref="ClientType.Modern"/>
/// <para/>
/// Identifiers:
/// <list type="bullet">
/// <item>Flash: <see cref="Out.ClickFurni"/></item>
/// </list>
/// </summary>
/// <param name="Type">The type of item that was clicked.</param>
/// <param name="Id">The ID of the item that was clicked.</param>
/// <param name="State">Appears to be unused.</param>
public sealed record ClickFurniMsg(ItemType Type, int Id, int State = 0) : IMessage<ClickFurniMsg>
{
    static ClientType IMessage<ClickFurniMsg>.SupportedClients => ClientType.Nitro;
    static Identifier IMessage<ClickFurniMsg>.Identifier => Out.UseFurniture;

    static ClickFurniMsg IParser<ClickFurniMsg>.Parse(in PacketReader p)
    {
        // A negative ID is used to indicate a wall item.
        int id = p.ReadInt();
        ItemType type = id < 0 ? ItemType.Wall : ItemType.Floor;
        if (id < 0)
            id = -id;
        int state = p.ReadInt();

        return new ClickFurniMsg(type, id, state);
    }

    void IComposer.Compose(in PacketWriter p)
    {
        if (Id < 0)
            throw new Exception($"Id cannot be negative when composing ClickFurniMsg: {Id}.");
        p.WriteInt(Id);
        p.WriteInt(State);
    }
}
