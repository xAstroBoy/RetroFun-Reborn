using System;

using Xabbo.Messages;

namespace Xabbo.Core;

public readonly record struct FriendUpdate(FriendListUpdateType Type, int Id, Friend? Friend = null) : IParserComposer<FriendUpdate>
{
    static FriendUpdate IParser<FriendUpdate>.Parse(in PacketReader p)
    {
        var type = (FriendListUpdateType)p.ReadInt();
        int id = 0;
        Friend? friend = null;

        switch (type)
        {
            case FriendListUpdateType.Update or FriendListUpdateType.Add:
                friend = p.Parse<Friend>();
                id = friend.Id;
                break;
            case FriendListUpdateType.Remove:
                id = p.ReadInt();
                break;
            default:
                throw new Exception($"Unknown friend list update type: {type}.");
        }

        return new(type, id, friend);
    }

    void IComposer.Compose(in PacketWriter p)
    {
        p.WriteInt((int)Type);
        if (Type is FriendListUpdateType.Update or FriendListUpdateType.Add)
        {
            if (Friend is null)
                throw new Exception($"Friend cannot be null when update type is {Type}.");
            p.Compose(Friend);
        }
        else if (Type is FriendListUpdateType.Remove)
        {
            if (Id == default)
                throw new Exception($"Id cannot be zero when update type is {Type}.");
            p.WriteInt(Id);
        }
        else
        {
            throw new Exception($"Unknown friend list update type: {Type}.");
        }
    }
}
