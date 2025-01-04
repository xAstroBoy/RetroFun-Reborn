﻿using Xabbo.Messages;
using Xabbo.Messages.Nitro;


namespace Xabbo.Core.Messages.Outgoing;

/// <summary>
/// Sent when responding to the doorbell.
/// <para/>
/// Supported clients: <see cref="ClientType.Nitro"/>
/// <para/>
/// Identifiers:
/// <list type="bullet">
/// <item>Flash: <see cref="Out.LetUserIn"/></item>
/// <item>Shockwave: <see cref="Out.LETUSERIN"/></item>
/// </list>
/// </summary>
/// <param name="Name">The name of the user.</param>
/// <param name="Accept">Whether to accept the user into the room.</param>
public sealed record AnswerDoorbellMsg(string Name, bool Accept) : IMessage<AnswerDoorbellMsg>
{
    static Identifier IMessage<AnswerDoorbellMsg>.Identifier => Out.Room_Doorbell;
    static AnswerDoorbellMsg IParser<AnswerDoorbellMsg>.Parse(in PacketReader p) => new(p.ReadString(), p.ReadBool());
    void IComposer.Compose(in PacketWriter p)
    {
        p.WriteString(Name);
        p.WriteBool(Accept);
    }
}
