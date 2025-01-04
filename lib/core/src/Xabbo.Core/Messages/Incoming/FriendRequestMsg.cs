using Xabbo.Messages;
using Xabbo.Messages.Nitro;


namespace Xabbo.Core.Messages.Incoming;

/// <summary>
/// Received when a friend request is received.
/// <para/>
/// Supported clients: <see cref="ClientType.Nitro"/>.
/// <para/>
/// Identifiers:
/// <list type="bullet">
/// <item>Flash: <see cref="In.NewFriendRequest"/></item>
/// <item>Shockwave: <see cref="In.FRIEND_REQUEST"/></item>
/// </list>
/// </summary>
/// <param name="Id">The ID of the user who sent the request.</param>
/// <param name="Name">The name of the user who sent the request.</param>
/// <param name="FigureString">The figure of the user who sent the request.</param>
public sealed record FriendRequestMsg(int Id, string Name, string? FigureString) : IMessage<FriendRequestMsg>
{
    static Identifier IMessage<FriendRequestMsg>.Identifier => Identifier.Unknown;

    static FriendRequestMsg IParser<FriendRequestMsg>.Parse(in PacketReader p) => new(
        Id: p.ReadInt(),
        Name: p.ReadString(),
        FigureString: p.ReadString()
    );

    void IComposer.Compose(in PacketWriter p)
    {
        p.WriteInt(Id);
        p.WriteString(Name);
        p.WriteString(FigureString ?? "");

    }
}
