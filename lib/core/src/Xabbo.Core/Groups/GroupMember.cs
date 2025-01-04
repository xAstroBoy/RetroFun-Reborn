using System;

using Xabbo.Messages;

namespace Xabbo.Core;

/// <inheritdoc cref="IGroupMember"/>
public sealed class GroupMember : IGroupMember, IParserComposer<GroupMember>
{
    public GroupMemberType Type { get; set; }
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Figure { get; set; } = "";
    public DateTime Joined { get; set; }

    public GroupMember() { }

    private GroupMember(in PacketReader p)
    {
        Type = (GroupMemberType)p.ReadInt();
        Id = p.ReadInt();
        Name = p.ReadString();
        Figure = p.ReadString();
        Joined = DateTime.Parse(p.ReadString());
    }

    void IComposer.Compose(in PacketWriter p)
    {
        p.WriteInt((int)Type);
        p.WriteInt(Id);
        p.WriteString(Name);
        p.WriteString(Figure);
        p.WriteString(Joined.ToString()); // TODO Check string format.
    }

    static GroupMember IParser<GroupMember>.Parse(in PacketReader p) => new(in p);
}
