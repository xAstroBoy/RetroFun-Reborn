using System;

using Xabbo.Messages;

namespace Xabbo.Core.Messages.Outgoing;

/// <summary>
/// Sent when performing an action in a room.
/// <para/>
/// Supported clients: <see cref="ClientType.Nitro"/>
/// <para/>
/// Identifiers:
/// <list type="bullet">
/// <item>Flash: <see cref="Xabbo.Messages.Nitro.Out.AvatarExpression"/></item>
/// <item>Shockwave: <see cref="Xabbo.Messages.Shockwave.Out.WAVE"/></item>
/// </list>
/// </summary>
/// <param name="Action">
/// The action to perform.
/// Only <see cref="AvatarAction.Wave"/> is supported on <see cref="ClientType.Origins"/>.
/// </param>
public sealed record ActionMsg(AvatarAction Action) : IMessage<ActionMsg>
{
    static Identifier IMessage<ActionMsg>.Identifier => default;
    static Identifier[] IMessage<ActionMsg>.Identifiers => [
        Xabbo.Messages.Nitro.Out.Action,
    ];
    static bool IMessage<ActionMsg>.UseTargetedIdentifiers => true;

    Identifier IMessage.GetIdentifier(ClientType client) => client switch
    {
        _ => Xabbo.Messages.Nitro.Out.Action,
    };

    static ActionMsg IParser<ActionMsg>.Parse(in PacketReader p) => new(p.Client switch
    {
        _ => (AvatarAction)p.ReadInt(),
    });

    void IComposer.Compose(in PacketWriter p)
    {
        p.WriteInt((int)Action);
    }
}
