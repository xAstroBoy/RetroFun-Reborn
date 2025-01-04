using System.Collections.Generic;

using Xabbo.Messages;
using Xabbo.Messages.Nitro;


namespace Xabbo.Core.Messages.Incoming;

/// <summary>
/// Represents a friend list fragment.
/// <para/>
/// Received after requesting the user's list of friends.
/// <para/>
/// Supported clients: <see cref="ClientType.Nitro"/>
/// <para/>
/// Identifiers:
/// <list type="bullet">
/// <item>Flash: <see cref="In.FriendListFragment"/></item>
/// <item>Shockwave: <see cref="In.BUDDYLIST"/></item>
/// </list>
/// </summary>
public sealed class FriendListMsg : List<Friend>, IMessage<FriendListMsg>
{
    public FriendListMsg() { }
    public FriendListMsg(int capacity) : base(capacity) { }
    public FriendListMsg(IEnumerable<Friend> collection) : base(collection) { }

    public int FragmentIndex { get; set; }
    public int FragmentCount { get; set; }

    public static Identifier Identifier => In.Messenger_Friends;

    static FriendListMsg IParser<FriendListMsg>.Parse(in PacketReader p)
    {
        int fragmentIndex = 0, fragmentCount = 1;

        fragmentCount = p.ReadInt();
        fragmentIndex = p.ReadInt();

        return new(p.ParseArray<Friend>())
        {
            FragmentIndex = fragmentIndex,
            FragmentCount = fragmentCount,
        };
    }

    void IComposer.Compose(in PacketWriter p)
    {
        p.WriteInt(FragmentCount);
        p.WriteInt(FragmentIndex);
        p.ComposeArray(this);
    }
}
