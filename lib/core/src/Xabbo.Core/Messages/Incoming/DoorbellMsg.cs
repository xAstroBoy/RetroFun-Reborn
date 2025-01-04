using Xabbo.Messages;
using Xabbo.Messages.Nitro;


namespace Xabbo.Core.Messages.Incoming;

/// <summary>
/// Receives when a user rings the doorbell.
/// <para/>
/// Supported clients: <see cref="ClientType.Nitro"/>
/// <para/>
/// Identifiers:
/// <list type="bullet">
/// <item>Flash: <see cref="In.Doorbell"/></item>
/// <item>Shockwave: <see cref="In.DOORBELL_RINGING"/></item>
/// </list>
/// </summary>
/// <param name="Name">The name of the user who is ringing the doorbell.</param>
public sealed record DoorbellMsg(string Name) : IMessage<DoorbellMsg>
{
    static Identifier IMessage<DoorbellMsg>.Identifier => In.Room_Doorbell;
    static DoorbellMsg IParser<DoorbellMsg>.Parse(in PacketReader p) => new(p.ReadString());
    void IComposer.Compose(in PacketWriter p) => p.WriteString(Name);
}
