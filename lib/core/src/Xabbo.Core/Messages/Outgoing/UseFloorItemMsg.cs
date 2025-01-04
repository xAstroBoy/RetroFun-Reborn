using System;

using Xabbo.Messages;

namespace Xabbo.Core.Messages.Outgoing;

/// <summary>
/// Sent when using a floor item.
/// <para/>
/// Supported clients: <see cref="ClientType.Nitro"/>
/// <para/>
/// Identifiers:
/// <list type="bullet">
/// <item>Flash: <see cref="Xabbo.Messages.Nitro.Out.UseFurniture"/></item>
/// <item>Shockwave: <see cref="Xabbo.Messages.Shockwave.Out.SETSTUFFDATA"/></item>
/// </list>
/// </summary>
/// <param name="Id">The ID of the floor item.</param>
/// <param name="State">The state of the floor item. Appears to be unused - items are always toggled between states.</param>
public sealed record UseFloorItemMsg(int Id, int State = 0) : IMessage<UseFloorItemMsg>
{
    static Identifier IMessage<UseFloorItemMsg>.Identifier => default;
    static Identifier[] IMessage<UseFloorItemMsg>.Identifiers { get; } = [
        Xabbo.Messages.Nitro.Out.UseFurniture,
    ];
    static bool IMessage<UseFloorItemMsg>.UseTargetedIdentifiers => true;

    Identifier IMessage.GetIdentifier(ClientType client) => client switch
    {
        _ => Xabbo.Messages.Nitro.Out.UseFurniture
    };

    static UseFloorItemMsg IParser<UseFloorItemMsg>.Parse(in PacketReader p)
    {
        int id; int state;

        id = p.ReadInt();
        state = p.ReadInt();

        return new(id, state);
    }

    void IComposer.Compose(in PacketWriter p)
    {
        p.WriteInt(Id);
        p.WriteInt(State);

    }
}
