using Xabbo.Messages;

using Xabbo.Core.Messages.Incoming;
using Xabbo.Messages.Nitro;

namespace Xabbo.Core.Messages.Outgoing;

/// <summary>
/// Sent when requesting the user's achievements.
/// <para/>
/// Request for <see cref="AchievementsMsg"/>. Returns <see cref="Achievements"/>.
/// <para/>
/// Supported clients: <see cref="ClientType.Modern"/>
/// <para/>
/// Identifiers:
/// <list type="bullet">
/// <item>Flash: <see cref="Out.GetAchievements"/></item>
/// </list>
/// </summary>
public sealed record class GetAchievementsMsg() : IRequestMessage<GetAchievementsMsg, AchievementsMsg, Achievements>
{
    static ClientType IMessage<GetAchievementsMsg>.SupportedClients => ClientType.Nitro;
    static Identifier IMessage<GetAchievementsMsg>.Identifier => Out.Get_Achievement_List;
    Achievements IResponseData<AchievementsMsg, Achievements>.GetData(AchievementsMsg msg) => msg.Achievements;
    static GetAchievementsMsg IParser<GetAchievementsMsg>.Parse(in PacketReader p) => new();
    void IComposer.Compose(in PacketWriter p) { }
}
