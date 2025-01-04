﻿using System.Collections.Generic;
using Xabbo.Messages;
using Xabbo.Messages.Nitro;


namespace Xabbo.Core.Messages.Incoming;

/// <summary>
/// Received after requesting the list of banned users from a room.
/// <para/>
/// Response for <see cref="Outgoing.GetBannedUsersMsg"/>.
/// <para/>
/// Supported clients: <see cref="ClientType.Modern"/>
/// <para/>
/// Identifiers:
/// <list type="bullet">
/// <item>Flash: <see cref="In.BannedUsersFromRoom"/></item>
/// </list>
/// </summary>
/// <param name="RoomId">The ID of the room.</param>
/// <param name="Users">The list of banned users.</param>
public sealed record BannedUsersMsg(int RoomId, List<IdName> Users) : IMessage<BannedUsersMsg>
{
    static ClientType IMessage<BannedUsersMsg>.SupportedClients => ClientType.Nitro;
    static Identifier IMessage<BannedUsersMsg>.Identifier => In.Room_Ban_List;
    static BannedUsersMsg IParser<BannedUsersMsg>.Parse(in PacketReader p) => new(
        RoomId: p.ReadInt(),
        Users: [.. p.ParseArray<IdName>()]
    );
    void IComposer.Compose(in PacketWriter p)
    {
        p.WriteInt(RoomId);
        p.ComposeArray(Users);
    }
}
