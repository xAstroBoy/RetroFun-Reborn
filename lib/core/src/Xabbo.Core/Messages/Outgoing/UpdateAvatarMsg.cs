﻿using Xabbo.Messages;
using Xabbo.Messages.Nitro;


namespace Xabbo.Core.Messages.Outgoing;

/// <summary>
/// Sent when updating the user's avatar looks.
/// <para/>
/// Supported clients:
/// <list type="bullet">
/// <item><see cref="ClientType.Modern"/> (Intercept/Send)</item>
/// <item><see cref="ClientType.Shockwave"/> (Send only: translates to <see cref="UpdateProfileMsg"/></item>
/// </list>
/// <para/>
/// Identifiers:
/// <list type="bullet">
/// <item>Flash: <see cref="Out.UpdateFigureData"/></item>
/// <item>Shockwave: <see cref="Out.UPDATE"/></item>
/// </list>
/// </summary>
public sealed record UpdateAvatarMsg(Gender Gender, string Figure) : IMessage<UpdateAvatarMsg>
{
    static bool IMessage<UpdateAvatarMsg>.UseTargetedIdentifiers => true;
    static Identifier IMessage<UpdateAvatarMsg>.Identifier => Out.User_Figure;

    Identifier IMessage.GetIdentifier(ClientType client) => client switch
    {
        _ => Out.User_Figure
    };

    public UpdateAvatarMsg(Figure figure) : this(figure.Gender, figure.ToString()) { }

    static UpdateAvatarMsg IParser<UpdateAvatarMsg>.Parse(in PacketReader p)
    {
        return new(
            Gender: H.ToGender(p.ReadString()),
            Figure: p.ReadString()
        );
    }

    void IComposer.Compose(in PacketWriter p)
    {
        p.WriteString(Gender.ToClientString());
        p.WriteString(Figure);
    }
}
