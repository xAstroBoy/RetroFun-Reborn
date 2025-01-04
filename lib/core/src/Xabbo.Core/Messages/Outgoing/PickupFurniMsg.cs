using System;

using Xabbo.Messages;
using Xabbo.Messages.Nitro;


namespace Xabbo.Core.Messages.Outgoing;

/// <summary>
/// Sent when picking up or ejecting a furni from a room.
/// <para/>
/// Supported clients: <see cref="ClientType.Nitro"/>
/// <para/>
/// Identifiers:
/// <list type="bullet">
/// <item>Flash: <see cref="Out.PickupObject"/></item>
/// <item>Shockwave: <see cref="Out.ADDSTRIPITEM"/></item>
/// </list>
/// </summary>
/// <param name="Type">The type of the furni to pick up.</param>
/// <param name="Id">The ID of the furni to pick up.</param>
public record PickupFurniMsg(ItemType Type, int Id) : IMessage<PickupFurniMsg>
{
    const int ExpectedFieldCount = 3;
    const string ExpectedField0 = "new";

    public static Identifier Identifier => Out.Furniture_Pickup;

    /// <summary>
    /// Constructs a new <see cref="PickupFurniMsg"/> with the specified furni.
    /// </summary>
    /// <param name="furni">The furni to pick up.</param>
    public PickupFurniMsg(IFurni furni) : this(furni.Type, furni.Id) { }

    static PickupFurniMsg IParser<PickupFurniMsg>.Parse(in PacketReader p)
    {
        ItemType itemType = (ItemType)(-1);
        int id = -1;

        int intType = p.ReadInt();
        itemType = intType switch
        {
            20 => ItemType.Wall,
            10 => ItemType.Floor,
            _ => throw new Exception($"Unknown item type when parsing PickupItemMsg: {intType}.")
        };
        id = p.ReadInt();

        return itemType switch
        {
            ItemType.Wall => new PickupFurniMsg(ItemType.Wall, id),
            ItemType.Floor => new PickupFurniMsg(ItemType.Floor, id),
            _ => throw new Exception("Unknown item type when parsing PickupItemMsg.")
        };
    }

    void IComposer.Compose(in PacketWriter p)
    {
        p.WriteInt(Type switch
        {
            ItemType.Wall => 1,
            ItemType.Floor => 2,
            _ => throw new Exception("Unknown item type when composing PickupItemMsg.")
        });
        p.WriteInt(Id);
    }
}
