using Xabbo.Messages;

using Xabbo.Core.Messages.Incoming;
using Xabbo.Messages.Nitro;

namespace Xabbo.Core.Messages.Outgoing;

/// <summary>
/// Sent when requesting a user's profile.
/// <para/>
/// Request for <see cref="ProfileMsg"/>.
/// Returns a <see cref="UserProfile"/>.
/// <para/>
/// Supported clients: <see cref="ClientType.Modern"/>
/// <para/>
/// Identifiers:
/// <list type="bullet">
/// <item>Flash: <see cref="Out.GetExtendedProfile"/></item>
/// </list>
/// </summary>
/// <param name="Id">The ID of the user whose profile to request.</param>
/// <param name="Open">Whether the open the profile in-client.</param>
public sealed record GetProfileMsg(int Id, bool Open = false) : IRequestMessage<GetProfileMsg, ProfileMsg, UserProfile>
{
    static ClientType IMessage<GetProfileMsg>.SupportedClients => ClientType.Nitro;
    static Identifier IMessage<GetProfileMsg>.Identifier => Out.User_Profile;
    bool IRequestFor<ProfileMsg>.MatchResponse(ProfileMsg response) => response.Profile.Id == Id;
    UserProfile IResponseData<ProfileMsg, UserProfile>.GetData(ProfileMsg msg) => msg.Profile;
    static GetProfileMsg IParser<GetProfileMsg>.Parse(in PacketReader p) => new(p.ReadInt(), p.ReadBool());
    void IComposer.Compose(in PacketWriter p)
    {
        p.WriteInt(Id);
        p.WriteBool(Open);
    }
}
