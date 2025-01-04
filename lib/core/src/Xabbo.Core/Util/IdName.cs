using Xabbo.Messages;

namespace Xabbo.Core;

/// <summary>
/// Represents a name and ID.
/// </summary>
public sealed record IdName(int Id, string Name) : IParserComposer<IdName>
{
    static IdName IParser<IdName>.Parse(in PacketReader p) => new(p.ReadInt(), p.ReadString());
    void IComposer.Compose(in PacketWriter p)
    {
        p.WriteInt(Id);
        p.WriteString(Name);
    }

    public static implicit operator IdName((int id, string name) idName) => new(idName.id, idName.name);
}
