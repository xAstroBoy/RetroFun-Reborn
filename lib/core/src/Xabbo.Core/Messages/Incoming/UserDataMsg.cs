using Xabbo.Messages;
using Xabbo.Messages.Nitro;


namespace Xabbo.Core.Messages.Incoming;

/// <summary>
/// Received after requesting the user's data.
/// <para/>
/// Response for <see cref="Outgoing.GetUserDataMsg"/>.
/// <para/>
/// Supported clients: <see cref="ClientType.Nitro"/>
/// <para/>
/// Identifiers:
/// <list type="bullet">
/// <item>Flash: <see cref="In.UserObject"/></item>
/// <item>Shockwave: <see cref="In.USER_OBJ"/></item>
/// </list>
/// </summary>
/// <param name="UserData">The current user's data.</param>
public sealed record UserDataMsg(UserData UserData) : IMessage<UserDataMsg>
{
    static Identifier IMessage<UserDataMsg>.Identifier => In.User_Profile;
    static UserDataMsg IParser<UserDataMsg>.Parse(in PacketReader p) => new(p.Parse<UserData>());
    void IComposer.Compose(in PacketWriter p) => p.Compose(UserData);
}
