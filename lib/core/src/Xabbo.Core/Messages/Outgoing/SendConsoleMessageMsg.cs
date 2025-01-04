using System;
using System.Collections.Generic;

using Xabbo.Messages;
using Xabbo.Messages.Nitro;


namespace Xabbo.Core.Messages.Outgoing;

/// <summary>
/// Sent when sending a message to a friend via the console.
/// <para/>
/// Supported clients: <see cref="ClientType.Nitro"/>
/// <para/>
/// Identifiers:
/// <list type="bullet">
/// <item>Flash: <see cref="Out.SendMsg"/></item>
/// <item>Shockwave: <see cref="Out.MESSENGER_SENDMSG"/></item>
/// </list>
/// </summary>
/// <remarks>
/// Only a single recipient is supported on <see cref="ClientType.Modern"/> clients.
/// On the <see cref="ClientType.Origins"/> client, you can specify multiple recipients.
/// </remarks>
public sealed class SendConsoleMessageMsg : IMessage<SendConsoleMessageMsg>
{
    public static Identifier Identifier => Out.Messenger_Chat;

    /// <summary>
    /// The list of recipient IDs.
    /// Only a single recipient is supported on <see cref="ClientType.Modern"/> clients.
    /// </summary>
    public List<int> Recipients { get; set; } = [];

    /// <summary>
    /// The message content.
    /// </summary>
    public string Message { get; set; } = "";

    public int ConfirmationId { get; set; }

    static SendConsoleMessageMsg IParser<SendConsoleMessageMsg>.Parse(in PacketReader p) => p.Client switch
    {
        _ => new()
        {
            Recipients = [p.ReadInt()],
            Message = p.ReadString(),
            ConfirmationId = p.ReadInt()
        }
    };

    void IComposer.Compose(in PacketWriter p)
    {
        if (Recipients.Count != 1)
            throw new Exception($"Only a single recipient is supported on the {p.Client} client.");
        p.WriteInt(Recipients[0]);
        p.WriteString(Message);
        p.WriteInt(ConfirmationId);

    }
}
