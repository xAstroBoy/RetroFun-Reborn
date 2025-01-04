using System;

using Xabbo.Messages;
using Xabbo.Messages.Nitro;


namespace Xabbo.Core.Messages.Outgoing;

/// <summary>
/// Sent when giving rights to a user in the room.
/// <para/>
/// Supported clients: <see cref="ClientType.Nitro"/>
/// <para/>
/// Identifiers:
/// <list type="bullet">
/// <item>Flash: <see cref="Out.AssignRights"/></item>
/// <item>Shockwave: <see cref="Xabbo.Messages.Shockwave.Out.ASSIGNRIGHTS"/></item>
/// </list>
/// </summary>
/// <param name="Id">The ID of the user to give rights to. Applies to <see cref="ClientType.Modern"/> clients.</param>
/// <param name="Name">The name of the user to give rights to. Applies to the <see cref="ClientType.Origins"/> client.</param>
public sealed record GiveRightsMsg(int? Id = null, string? Name = null) : IMessage<GiveRightsMsg>
{
    static Identifier IMessage<GiveRightsMsg>.Identifier => Out.Room_Rights_Give;

    static GiveRightsMsg IParser<GiveRightsMsg>.Parse(in PacketReader p) => p.Client switch
    {
        _ => new GiveRightsMsg { Id = p.ReadInt() }
    };

    void IComposer.Compose(in PacketWriter p)
    {
        if (Id is not { } id)
            throw new Exception($"{nameof(GiveRightsMsg)} requires the user's ID on {p.Client}.");
        p.WriteInt(id);
    }
}
