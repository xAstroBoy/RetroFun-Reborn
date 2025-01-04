using System;
using System.Collections.Generic;
using System.Linq;

using Xabbo.Messages;
using Xabbo.Messages.Nitro;


namespace Xabbo.Core.Messages.Incoming;

/// <summary>
/// Received when friends are added, updated, or removed.
/// <para/>
/// Supported clients: <see cref="ClientType.Nitro"/>
/// <para/>
/// Identifiers:
/// <list type="bullet">
/// <item>Flash: <see cref="In.FriendListUpdate"/></item>
/// <item>Shockwave: <see cref="In.FRIEND_LIST_UPDATE"/></item>
/// </list>
/// </summary>
/// <remarks>
/// On Origins, this message will only contain friend updates.
/// For friends that are added or removed, see <see cref="FriendAddedMsg"/> or
/// <see cref="FriendsRemovedMsg"/>.
/// </remarks>
public sealed class FriendsUpdatedMsg : IMessage<FriendsUpdatedMsg>
{
    static Identifier IMessage<FriendsUpdatedMsg>.Identifier => In.Messenger_Update;

    /// <summary>
    /// The friend list categories.
    /// </summary>
    /// <remarks>
    /// Used on modern clients.
    /// </remarks>
    public List<FriendCategory> Categories { get; set; } = [];

    /// <summary>
    /// The list of friend updates.
    /// </summary>
    /// <remarks>
    /// Shockwave only supports updates with type <see cref="FriendListUpdateType.Update"/>.
    /// </remarks>
    public List<FriendUpdate> Updates { get; set; } = [];

    static FriendsUpdatedMsg IParser<FriendsUpdatedMsg>.Parse(in PacketReader p)
    {
        return new() { Categories = [.. p.ParseArray<FriendCategory>()], Updates = [.. p.ParseArray<FriendUpdate>()], };
    }

    void IComposer.Compose(in PacketWriter p)
    {
        p.ComposeArray(Categories);
        p.ComposeArray(Updates);
    }
}
