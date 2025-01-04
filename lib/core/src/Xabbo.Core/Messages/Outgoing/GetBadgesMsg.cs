﻿using Xabbo.Messages;
using Xabbo.Messages.Nitro;


namespace Xabbo.Core.Messages.Outgoing;

/// <summary>
/// Sent when requesting the user's badges.
/// <para/>
/// Responds with a sequence of <see cref="Incoming.BadgeFragmentMsg"/>.
/// <para/>
/// Supported clients: <see cref="ClientType.Modern"/>
/// <para/>
/// Identifiers:
/// <list type="bullet">
/// <item>Flash: <see cref="Out.GetBadges"/></item>
/// </list>
/// </summary>
public sealed record GetBadgesMsg : IMessage<GetBadgesMsg>
{
    static ClientType IMessage<GetBadgesMsg>.SupportedClients => ClientType.Nitro;
    static Identifier IMessage<GetBadgesMsg>.Identifier => Out.User_Badges;
    static GetBadgesMsg IParser<GetBadgesMsg>.Parse(in PacketReader p) => new();
    void IComposer.Compose(in PacketWriter p) { }
}
