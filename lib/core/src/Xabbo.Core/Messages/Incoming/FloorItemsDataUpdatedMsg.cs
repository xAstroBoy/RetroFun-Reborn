using System.Collections.Generic;

using Xabbo.Messages;
using Xabbo.Messages.Nitro;


namespace Xabbo.Core.Messages.Incoming;

/// <summary>
/// Received when the item data of multiple floor items are updated.
/// <para/>
/// Supported clients: <see cref="ClientType.Modern"/>
/// <para/>
/// Identifiers:
/// <list type="bullet">
/// <item>Flash: <see cref="In.ObjectsDataUpdate"/></item>
/// </list>
/// </summary>
/// <param name="Updates">The list of floor item data updated.</param>
public sealed record FloorItemsDataUpdatedMsg(List<(int Id, ItemData Data)> Updates) : IMessage<FloorItemsDataUpdatedMsg>
{
    static ClientType IMessage<FloorItemsDataUpdatedMsg>.SupportedClients => ClientType.Nitro;
    static Identifier IMessage<FloorItemsDataUpdatedMsg>.Identifier => In.Objects_Data_Update;

    static FloorItemsDataUpdatedMsg IParser<FloorItemsDataUpdatedMsg>.Parse(in PacketReader p)
    {
        int n = p.ReadLength();
        List<(int, ItemData)> updates = new(n);
        for (int i = 0; i < n; i++)
            updates.Add((p.ReadInt(), p.Parse<ItemData>()));

        return new FloorItemsDataUpdatedMsg(updates);
    }

    void IComposer.Compose(in PacketWriter p)
    {
        p.WriteLength((Length)Updates.Count);
        foreach (var (id, itemData) in Updates)
        {
            p.WriteInt(id);
            p.Compose(itemData);
        }
    }
}
