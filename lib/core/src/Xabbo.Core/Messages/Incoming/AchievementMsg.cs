﻿
using Xabbo.Messages;
using Xabbo.Messages.Nitro;


namespace Xabbo.Core.Messages.Incoming;

/// <summary>
/// Received when an achievement is updated.
/// <para/>
/// Supported clients: <see cref="ClientType.Modern"/>
/// <para/>
/// Identifiers: <see cref="Core.Achievement"/>
/// </summary>
public sealed record AchievementMsg(Achievement Achievement) : IMessage<AchievementMsg>
{
    static ClientType IMessage<AchievementMsg>.SupportedClients => ClientType.None;
    static Identifier IMessage<AchievementMsg>.Identifier => Identifier.Unknown;

    static AchievementMsg IParser<AchievementMsg>.Parse(in PacketReader p) => new(p.Parse<Achievement>());
    void IComposer.Compose(in PacketWriter p) => p.Compose(Achievement);
}
