using System;

using Xabbo.Messages;

namespace Xabbo.Core.Messages.Outgoing;

/// <summary>
/// Sent when starting, stopping or changing dances in a room.
/// <para/>
/// Supported clients: <see cref="ClientType.Nitro"/>
/// <para/>
/// Identifiers:
/// <list type="bullet">
/// <item>Flash: <see cref="Xabbo.Messages.Nitro.Out.Dance"/></item>
/// <item>
/// Shockwave:
/// <see cref="Xabbo.Messages.Shockwave.Out.DANCE"/>,
/// <see cref="Xabbo.Messages.Shockwave.Out.STOP"/>.
/// </item>
/// </list>
/// </summary>
/// <param name="Dance">The dance to change to.</param>
public sealed record DanceMsg(AvatarDance Dance = AvatarDance.Dance) : IMessage<DanceMsg>
{
    static Identifier IMessage<DanceMsg>.Identifier => default;
    static bool IMessage<DanceMsg>.UseTargetedIdentifiers => true;

    static Identifier[] IMessage<DanceMsg>.Identifiers { get; } = [
        Xabbo.Messages.Nitro.Out.Dance,
    ];

    Identifier IMessage.GetIdentifier(ClientType client) => client switch
    {
        _ => Xabbo.Messages.Nitro.Out.Dance,
    };

    static bool IMessage<DanceMsg>.Match(in PacketReader p)
    {
        if (p.Context is null)
            throw new Exception($"Context is null when attempting to match {nameof(DanceMsg)}.");
        return true;
    }

    static DanceMsg IParser<DanceMsg>.Parse(in PacketReader p)
    {
        AvatarDance dance;

        dance = (AvatarDance)p.ReadInt();

        return new DanceMsg { Dance = dance };
    }

    void IComposer.Compose(in PacketWriter p)
    {
        p.WriteInt((int)Dance);
    }
}
