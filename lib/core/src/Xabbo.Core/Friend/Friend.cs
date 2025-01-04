﻿using Xabbo.Messages;

namespace Xabbo.Core;

/// <inheritdoc cref="IFriend"/>
public class Friend : IFriend, IParserComposer<Friend>
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public Gender Gender { get; set; }
    public bool IsOnline { get; set; }
    public bool CanFollow { get; set; }
    public string Figure { get; set; } = "";
    public int CategoryId { get; set; }
    public string Motto { get; set; } = "";
    public string RealName { get; set; } = "";
    public string FacebookId { get; set; } = "";
    public bool IsAcceptingOfflineMessages { get; set; }
    public bool IsVipMember { get; set; }
    public bool IsPocketHabboUser { get; set; }
    public Relation Relation { get; set; }

    /// <summary>
    /// The current location of the friend.
    /// </summary>
    /// <remarks>
    /// Only available on Shockwave.
    /// </remarks>
    public string Location { get; set; } = "";

    /// <summary>
    /// The last access time of the friend.
    /// </summary>
    /// <remarks>
    /// Only available on Shockwave.
    /// </remarks>
    public string LastAccess { get; set; } = "";

    public Friend() { }

    protected Friend(in PacketReader p)
    {
        Id = p.ReadInt();
        Name = p.ReadString();
        Gender = H.ToGender(p.ReadInt());
        IsOnline = p.ReadBool();
        CanFollow = p.ReadBool();
        Figure = p.ReadString();
        CategoryId = p.ReadInt();
        Motto = p.ReadString();

        RealName = p.ReadString();
        FacebookId = p.ReadString();

        IsAcceptingOfflineMessages = p.ReadBool();
        IsVipMember = p.ReadBool();
        IsPocketHabboUser = p.ReadBool();
        Relation = (Relation)p.ReadShort();
    }

    void IComposer.Compose(in PacketWriter p)
    {
        p.WriteInt(Id);
        p.WriteString(Name);
        p.WriteInt(Gender.ToClientValue());
        p.WriteBool(IsOnline);
        p.WriteBool(CanFollow);
        p.WriteString(Figure);
        p.WriteInt(CategoryId);
        p.WriteString(Motto);
        p.WriteString(RealName);
        p.WriteString(FacebookId);
        p.WriteBool(IsAcceptingOfflineMessages);
        p.WriteBool(IsVipMember);
        p.WriteBool(IsPocketHabboUser);
        p.WriteShort((short)Relation);
    }

    public override string ToString() => Name;

    static Friend IParser<Friend>.Parse(in PacketReader p) => new(in p);
}
