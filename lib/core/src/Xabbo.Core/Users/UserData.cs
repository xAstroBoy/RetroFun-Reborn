using System;

using Xabbo.Messages;

namespace Xabbo.Core;

/// <inheritdoc cref="IUserData"/>
public sealed class UserData : IUserData, IParserComposer<UserData>
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Figure { get; set; } = "";
    public string Motto { get; set; } = "";

    public UserData() { }

    private UserData(in PacketReader p)
    {
        ParseModern(in p);
    }

    private void ParseModern(in PacketReader p)
    {
        Id = p.ReadInt();
        Name = p.ReadString();
        Figure = p.ReadString();
        Motto = p.ReadString();
    }
    void IComposer.Compose(in PacketWriter p)
    {
        p.WriteInt(Id);
        p.WriteString(Name);
        p.WriteString(Figure);
        p.WriteString(Motto);
    }

    static UserData IParser<UserData>.Parse(in PacketReader p) => new(in p);
}
