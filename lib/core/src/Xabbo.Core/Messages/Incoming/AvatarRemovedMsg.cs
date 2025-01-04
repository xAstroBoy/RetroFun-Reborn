﻿using System;

using Xabbo.Messages;
using Xabbo.Messages.Nitro;


namespace Xabbo.Core.Messages.Incoming;

/// <summary>
/// Received when an avatar is removed from the room.
/// <para/>
/// Supported clients: <see cref="ClientType.Nitro"/>
/// <para/>
/// Identifiers:
/// <list type="bullet">
/// <item>Flash: <see cref="In.UserRemove"/></item>
/// <item>Shockwave: <see cref="In.LOGOUT"/></item>
/// </list>
/// </summary>
/// <param name="Index">The index of the avatar that was removed.</param>
public sealed record AvatarRemovedMsg(int Index) : IMessage<AvatarRemovedMsg>
{
    static Identifier IMessage<AvatarRemovedMsg>.Identifier => In.UserRemove;

    static AvatarRemovedMsg IParser<AvatarRemovedMsg>.Parse(in PacketReader p)
    {
        string strId = p.ReadString();

        if (!int.TryParse(strId, out int index))
            throw new FormatException($"Failed to parse Index in AvatarRemovedMsg: '{strId}'.");

        return new(index);
    }

    void IComposer.Compose(in PacketWriter p)
    {
        p.WriteString(Index.ToString());
    }
}
