﻿using Xabbo.Messages;
using Xabbo.Messages.Nitro;


namespace Xabbo.Core.Messages.Incoming;

/// <summary>
/// Received after searching for a user in the console.
/// <para/>
/// Response for <see cref="Outgoing.SearchUserMsg"/>.
/// <para/>
/// Supported clients: <see cref="ClientType.Nitro"/>
/// <para/>
/// Identifiers:
/// <list type="bullet">
/// <item>Flash: <see cref="In.HabboSearchResult"/></item>
/// <item>Shockwave: <see cref="In.HABBO_SEARCH_RESULT"/></item>
/// </list>
/// </summary>
/// <param name="Results">The search results.</param>
public sealed record SearchUserResultsMsg(UserSearchResults Results) : IMessage<SearchUserResultsMsg>
{
    static Identifier IMessage<SearchUserResultsMsg>.Identifier => In.Messenger_Search;
    static SearchUserResultsMsg IParser<SearchUserResultsMsg>.Parse(in PacketReader p) => new(p.Parse<UserSearchResults>());
    void IComposer.Compose(in PacketWriter p) => p.Compose(Results);
}
