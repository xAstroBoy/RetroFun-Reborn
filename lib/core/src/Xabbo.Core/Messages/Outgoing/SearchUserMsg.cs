﻿using Xabbo.Messages;

using Xabbo.Core.Messages.Incoming;
using Xabbo.Messages.Nitro;

namespace Xabbo.Core.Messages.Outgoing;

/// <summary>
/// Sent when searching for a user in the console.
/// <para/>
/// Request for <see cref="SearchUserResultsMsg"/>. Returns <see cref="UserSearchResults"/>.
/// <para/>
/// Supported clients: <see cref="ClientType.Nitro"/>
/// <para/>
/// Identifiers:
/// <list type="bullet">
/// <item>Flash: <see cref="Out.HabboSearch"/></item>
/// <item>Shockwave: <see cref="Out.MESSENGER_HABBOSEARCH"/></item>
/// </list>
/// </summary><param name="Name">The name of the user to search for.</param>
/// <param name="Type">
/// The type of search to perform.
/// Only used on <see cref="ClientType.Shockwave"/>.
/// Defaults to "MESSENGER".
/// </param>
public sealed record SearchUserMsg(string Name, string Type = "MESSENGER")
    : IRequestMessage<SearchUserMsg, SearchUserResultsMsg, UserSearchResults>
{
    static Identifier IMessage<SearchUserMsg>.Identifier => Out.HabboSearch;

    UserSearchResults IResponseData<SearchUserResultsMsg, UserSearchResults>.GetData(SearchUserResultsMsg msg) => msg.Results;

    static SearchUserMsg IParser<SearchUserMsg>.Parse(in PacketReader p) => p.Client switch
    {
        _ => new(Name: p.ReadString())
    };

    void IComposer.Compose(in PacketWriter p)
    {
        p.WriteString(Name);
    }
}
