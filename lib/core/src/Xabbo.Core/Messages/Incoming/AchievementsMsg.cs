using Xabbo.Messages;
using Xabbo.Messages.Nitro;


namespace Xabbo.Core.Messages.Incoming;

/// <summary>
/// Received when achievements are loaded.
/// <para/>
/// Response for <see cref="Outgoing.GetAchievementsMsg"/>.
/// <para/>
/// Supported clients: <see cref="ClientType.Modern"/>
/// <para/>
/// Identifiers: <see cref="Core.Achievements"/>
/// </summary>
public sealed record AchievementsMsg(Achievements Achievements) : IMessage<AchievementsMsg>
{
    static ClientType IMessage<AchievementsMsg>.SupportedClients => ClientType.Nitro;
    static Identifier IMessage<AchievementsMsg>.Identifier => Identifier.Unknown;

    static AchievementsMsg IParser<AchievementsMsg>.Parse(in PacketReader p) => new(p.Parse<Achievements>());
    void IComposer.Compose(in PacketWriter p) => p.Compose(Achievements);
}
