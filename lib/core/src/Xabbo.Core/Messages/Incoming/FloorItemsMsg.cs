using System.Collections.Generic;
using System.Linq;

using Xabbo.Messages;
using Xabbo.Messages.Nitro;


namespace Xabbo.Core.Messages.Incoming;

/// <summary>
/// Represents a list of floor items.
/// <para/>
/// Received when floor items in the room are loaded.
/// <para/>
/// Supported clients: <see cref="ClientType.Nitro"/>
/// <para/>
/// Identifiers:
/// <list type="bullet">
/// <item>Flash: <see cref="In.Objects"/></item>
/// <item>Shockwave: <see cref="In.ACTIVEOBJECTS"/></item>
/// </list>
/// </summary>
public sealed class FloorItemsMsg : List<FloorItem>, IMessage<FloorItemsMsg>
{
    public FloorItemsMsg() { }
    public FloorItemsMsg(int capacity) : base(capacity) { }

    public static Identifier Identifier => In.Floor_Furni_List;

    static FloorItemsMsg IParser<FloorItemsMsg>.Parse(in PacketReader p)
    {
        int n;
        var ownerDictionary = new Dictionary<int, string>();

        n = p.ReadLength();
        for (int i = 0; i < n; i++)
            ownerDictionary.Add(p.ReadInt(), p.ReadString());

        n = p.ReadLength();
        var items = new FloorItemsMsg(n);
        for (int i = 0; i < n; i++)
        {
            var item = p.Parse<FloorItem>();
            if (ownerDictionary.TryGetValue(item.OwnerId, out string? ownerName))
                item.OwnerName = ownerName;
            items.Add(item);
        }

        return items;
    }

    void IComposer.Compose(in PacketWriter p)
    {
        var ownerIds = new HashSet<int>();
        var ownerDictionary = this
            .Where(x => ownerIds.Add(x.OwnerId))
            .ToDictionary(
                key => key.OwnerId,
                val => val.OwnerName
            );

        p.WriteLength((Length)ownerDictionary.Count);
        foreach (var pair in ownerDictionary)
        {
            p.WriteInt(pair.Key);
            p.WriteString(pair.Value);
        }
        p.WriteLength((Length)Count);
        foreach (FloorItem item in this)
            p.Compose(item);
    }
}
