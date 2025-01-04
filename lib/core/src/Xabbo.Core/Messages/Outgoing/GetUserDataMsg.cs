using Xabbo.Messages;

using Xabbo.Core.Messages.Incoming;
using Xabbo.Messages.Nitro;

namespace Xabbo.Core.Messages.Outgoing;

/// <summary>
/// Sent when requesting the user's data.
/// <para/>
/// Request for <see cref="UserDataMsg"/>.
/// <para/>
/// Supported clients: <see cref="ClientType.Nitro"/>
/// <para/>
/// Identifiers:
/// <list type="bullet">
/// <item>Flash: <see cref="Out.InfoRetrieve"/></item>
/// <item>Shockwave: <see cref="Out.INFORETRIEVE"/></item>
/// </list>
/// </summary>
public sealed record GetUserDataMsg : IRequestMessage<GetUserDataMsg, UserDataMsg, UserData>
{
    static Identifier IMessage<GetUserDataMsg>.Identifier => Out.User_Info;
    UserData IResponseData<UserDataMsg, UserData>.GetData(UserDataMsg msg) => msg.UserData;
    static GetUserDataMsg IParser<GetUserDataMsg>.Parse(in PacketReader p) => new();
    void IComposer.Compose(in PacketWriter p) { }
}
