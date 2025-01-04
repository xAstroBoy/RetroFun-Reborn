using Xabbo.Messages;
using Xabbo.Messages.Nitro;


namespace Xabbo.Core.Messages.Incoming;

/// <summary>
/// Received when activity points are loaded.
/// <para/>
/// Supported clients: <see cref="ClientType.Modern"/>
/// <para/>
/// Identifiers: <see cref="In.ActivityPoints"/>
/// </summary>
public sealed record ActivityPointsMsg(ActivityPoints ActivityPoints) : IMessage<ActivityPointsMsg>
{
    static ClientType IMessage<ActivityPointsMsg>.SupportedClients => ClientType.None;
    static Identifier IMessage<ActivityPointsMsg>.Identifier => Identifier.Unknown;

    static ActivityPointsMsg IParser<ActivityPointsMsg>.Parse(in PacketReader p) => new(p.Parse<ActivityPoints>());
    void IComposer.Compose(in PacketWriter p) => p.Compose(ActivityPoints);
}
