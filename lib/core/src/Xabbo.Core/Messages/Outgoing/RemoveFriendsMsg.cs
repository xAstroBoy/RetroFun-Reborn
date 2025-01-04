using System.Collections.Generic;

using Xabbo.Messages;
using Xabbo.Messages.Nitro;


namespace Xabbo.Core.Messages.Outgoing;

/// <summary>
/// Sent when removing friends from the user's friends list.
/// <para/>
/// Supported clients: <see cref="ClientType.Nitro"/>
/// <para/>
/// Identifiers:
/// <list type="bullet">
/// <item>Flash: <see cref="Out.RemoveFriend"/></item>
/// <item>Shockwave: <see cref="Out.FRIENDLIST_REMOVEFRIEND"/></item>
/// </list>
/// </summary>
public sealed class RemoveFriendsMsg : List<int>, IMessage<RemoveFriendsMsg>
{
    public RemoveFriendsMsg() { }
    public RemoveFriendsMsg(int capacity) : base(capacity) { }
    public RemoveFriendsMsg(IEnumerable<int> ids) : base(ids) { }

    static Identifier IMessage<RemoveFriendsMsg>.Identifier => Out.Remove_Friend;
    static RemoveFriendsMsg IParser<RemoveFriendsMsg>.Parse(in PacketReader p) => [.. p.ReadIntArray()];
    void IComposer.Compose(in PacketWriter p) => p.WriteIntArray(this);
}
