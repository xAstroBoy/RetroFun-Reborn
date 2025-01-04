using System;

using Xabbo.Messages;
using Xabbo.Messages.Nitro;


namespace Xabbo.Core.Messages.Incoming;

/// <summary>
/// Received when a floor item's data is updated.
/// <para/>
/// Supported clients: <see cref="ClientType.Nitro"/>
/// <para/>
/// Identifiers:
/// <list type="bullet">
/// <item>Flash: <see cref="In.ObjectDataUpdate"/></item>
/// <item>Shockwave: <see cref="In.STUFFDATAUPDATE"/></item>
/// </list>
/// </summary>
/// <param name="Id">The ID of the floor item that was updated.</param>
/// <param name="Data">The updated item data.</param>
public sealed record FloorItemDataUpdatedMsg(int Id, ItemData Data) : IMessage<FloorItemDataUpdatedMsg>
{
    static Identifier IMessage<FloorItemDataUpdatedMsg>.Identifier => In.FloorItem_Update_Status;

    static FloorItemDataUpdatedMsg IParser<FloorItemDataUpdatedMsg>.Parse(in PacketReader p) => new(
        p.ReadString() switch
        {
            string s when int.TryParse(s, out int id) => id,
            string s => throw new FormatException($"Failed to parse {nameof(Id)} in {nameof(FloorItemDataUpdatedMsg)}.")
        },
        p.Parse<ItemData>()
    );

    void IComposer.Compose(in PacketWriter p)
    {
        p.WriteString(Id.ToString());
        p.Compose(Data);
    }
}
