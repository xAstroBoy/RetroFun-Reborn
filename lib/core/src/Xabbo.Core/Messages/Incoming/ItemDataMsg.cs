using Xabbo.Messages;
using Xabbo.Messages.Nitro;


namespace Xabbo.Core.Messages.Incoming;

/// <summary>
/// Received after requesting the data of a wall item.
/// <para/>
/// Response for <see cref="Outgoing.GetItemDataMsg"/>.
/// <para/>
/// Supported clients: <see cref="ClientType.Nitro"/>
/// <para/>
/// Identifiers:
/// <list type="bullet">
/// <item>Flash: <see cref="In.ItemDataUpdate"/></item>
/// <item>Shockwave: <see cref="In.IDATA"/></item>
/// </list>
/// </summary>
public sealed record ItemDataMsg(int Id, string Data) : IMessage<ItemDataMsg>
{
    static Identifier IMessage<ItemDataMsg>.Identifier => In.Objects_Data_Update;

    static ItemDataMsg IParser<ItemDataMsg>.Parse(in PacketReader p) => new(
        int.Parse(p.ReadString()),
        p.ReadString()
    );

    void IComposer.Compose(in PacketWriter p)
    {
        p.WriteString(Id.ToString());
        p.WriteString(Data);
    }
}
