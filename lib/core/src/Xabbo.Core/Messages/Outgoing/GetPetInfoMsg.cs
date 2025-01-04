using Xabbo.Messages;

using Xabbo.Core.Messages.Incoming;
using Xabbo.Messages.Nitro;

namespace Xabbo.Core.Messages.Outgoing;

/// <summary>
/// Sent when requesting a pet's information.
/// <para/>
/// Request for <see cref="PetInfoMsg"/>. Returns a <see cref="PetInfo"/>.
/// <para/>
/// Supported clients: <see cref="ClientType.Modern"/>
/// <para/>
/// Identifiers:
/// <list type="bullet">
/// <item>Flash: <see cref="Out.GetPetInfo"/></item>
/// </list>
/// </summary>
/// <param name="Id">The ID of the pet to retrieve information for.</param>
public sealed record GetPetInfoMsg(int Id) : IRequestMessage<GetPetInfoMsg, PetInfoMsg, PetInfo>
{
    static ClientType IMessage<GetPetInfoMsg>.SupportedClients => ClientType.Nitro;
    static Identifier IMessage<GetPetInfoMsg>.Identifier => Out.Pet_Info;
    PetInfo IResponseData<PetInfoMsg, PetInfo>.GetData(PetInfoMsg msg) => msg.Info;
    static GetPetInfoMsg IParser<GetPetInfoMsg>.Parse(in PacketReader p) => new(p.ReadInt());
    void IComposer.Compose(in PacketWriter p) => p.WriteInt(Id);
}
