using Xabbo.Messages;
using Xabbo.Messages.Nitro;


namespace Xabbo.Core.Messages.Outgoing;

/// <summary>
/// Sent when updating the user's motto.
/// <para/>
/// Supported clients:
/// <list type="bullet">
/// <item><see cref="ClientType.Modern"/></item>
/// <item><see cref="ClientType.Origins"/> (Send only: translates to <see cref="UpdateProfileMsg"/></item>
/// </list>
/// <para/>
/// Identifiers:
/// <list type="bullet">
/// <item>Flash: <see cref="Out.ChangeMotto"/></item>
/// <item>Shockwave: <see cref="Out.UPDATE"/></item>
/// </list>
/// </summary>
public sealed record UpdateMottoMsg(string Motto) : IMessage<UpdateMottoMsg>
{
    static bool IMessage<UpdateMottoMsg>.UseTargetedIdentifiers => true;
    static Identifier IMessage<UpdateMottoMsg>.Identifier => Out.User_Motto;
    Identifier IMessage.GetIdentifier(Xabbo.ClientType client) => client switch
    {
        _ => Out.User_Motto

    };

    static UpdateMottoMsg IParser<UpdateMottoMsg>.Parse(in PacketReader p)
    {
        return new(p.ReadString());
    }

    void IComposer.Compose(in PacketWriter p)
    {
        p.WriteString(Motto);
    }
}
