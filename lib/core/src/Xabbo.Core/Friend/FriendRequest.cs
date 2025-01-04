using Xabbo.Messages;

namespace Xabbo.Core;

public class FriendRequest : IParserComposer<FriendRequest>
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Figure { get; set; } = "";

    public FriendRequest() { }

    protected FriendRequest(in PacketReader p)
    {
        Id = p.ReadInt();
        Name = p.ReadString();
        Figure = p.ReadString();

    }

    void IComposer.Compose(in PacketWriter p)
    {
        p.WriteInt(Id);
        p.WriteString(Name);
        p.WriteString(Figure);

    }

    static FriendRequest IParser<FriendRequest>.Parse(in PacketReader p) => new(in p);
}
