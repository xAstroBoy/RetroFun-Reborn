using Xabbo.Messages;
using Xabbo.Messages.Nitro;


namespace Xabbo.Core.Messages.Incoming;

/// <summary>
/// Received after searching the list of a group's members.
/// <para/>
/// Response for <see cref="Outgoing.SearchGroupMembersMsg"/>.
/// <para/>
/// Supported clients: <see cref="ClientType.Modern"/>
/// <para/>
/// Identifiers:
/// <list type="bullet">
/// <item>Flash: <see cref="In.GuildMembers"/></item>
/// </list>
/// </summary>
/// <param name="Members">The group members.</param>
public sealed record GroupMembersMsg(GroupMembers Members) : IMessage<GroupMembersMsg>
{
    static ClientType IMessage<GroupMembersMsg>.SupportedClients => ClientType.Nitro;
    static Identifier IMessage<GroupMembersMsg>.Identifier => In.Group_Members;
    static GroupMembersMsg IParser<GroupMembersMsg>.Parse(in PacketReader p) => new(p.Parse<GroupMembers>());
    void IComposer.Compose(in PacketWriter p) => p.Compose(Members);
}
