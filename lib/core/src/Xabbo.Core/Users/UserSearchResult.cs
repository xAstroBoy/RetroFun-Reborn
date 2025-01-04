using Xabbo.Messages;

namespace Xabbo.Core;

/// <summary>
/// Represents a user search result.
/// </summary>
public sealed class UserSearchResult : IParserComposer<UserSearchResult>
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Motto { get; set; } = "";
    public bool Online { get; set; }
    public bool CanFollow { get; set; }
    public string LastAccess { get; set; } = "";
    public Gender Gender { get; set; } = Gender.None;
    public string Figure { get; set; } = "";
    public string RealName { get; set; } = "";

    /// <summary>
    /// The current location of the user.
    /// </summary>
    /// <remarks>
    /// Only available on Shockwave.
    /// </remarks>
    public string Location { get; set; } = "";

    public UserSearchResult() { }

    private UserSearchResult(in PacketReader p)
    {
        Id = p.ReadInt();
        Name = p.ReadString();
        Motto = p.ReadString();
        Online = p.ReadBool();
        CanFollow = p.ReadBool();
        LastAccess = p.ReadString();
        Gender = H.ToGender(p.ReadInt());
        Figure = p.ReadString();
        RealName = p.ReadString();

    }

    void IComposer.Compose(in PacketWriter p)
    {
        p.WriteInt(Id);
        p.WriteString(Name);
        p.WriteString(Motto);
        p.WriteBool(Online);
        p.WriteBool(CanFollow);
        p.WriteString(LastAccess);
        p.WriteInt((int)Gender);
        p.WriteString(Figure);
        p.WriteString(RealName);
    }

    static UserSearchResult IParser<UserSearchResult>.Parse(in PacketReader p) => new(in p);
}
