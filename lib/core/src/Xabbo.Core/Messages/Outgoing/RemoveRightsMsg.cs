using System;
using System.Collections.Generic;

using Xabbo.Messages;
using Xabbo.Messages.Nitro;


namespace Xabbo.Core.Messages.Outgoing;

/// <summary>
/// Sent when removing the rights of a user from the current room.
/// <para/>
/// Supported clients: <see cref="ClientType.Nitro"/>
/// <para/>
/// Identifiers:
/// <list type="bullet">
/// <item>Flash: <see cref="Out.RemoveRights"/></item>
/// <item>Shockwave: <see cref="Out.REMOVERIGHTS"/></item>
/// </list>
/// </summary>
/// <param name="Ids">The list of user IDs to remove rights from. Applies to <see cref="ClientType.Modern"/> clients.</param>
/// <param name="Name">The name of the user to remove rights from. Applies to the <see cref="ClientType.Origins"/> client.</param>
public sealed record RemoveRightsMsg(List<int>? Ids = null, string? Name = null) : IMessage<RemoveRightsMsg>
{
    static Identifier IMessage<RemoveRightsMsg>.Identifier => Out.Room_Rights_Remove;

    static RemoveRightsMsg IParser<RemoveRightsMsg>.Parse(in PacketReader p) => p.Client switch
    {
        _ => new RemoveRightsMsg { Ids = new(p.ReadIntArray()) },
    };

    void IComposer.Compose(in PacketWriter p)
    {
        if (Ids is null)
            throw new Exception($"{nameof(Ids)} is required when composing {nameof(RemoveRightsMsg)} on {p.Client}.");
        p.WriteIntArray(Ids);
    }
}
