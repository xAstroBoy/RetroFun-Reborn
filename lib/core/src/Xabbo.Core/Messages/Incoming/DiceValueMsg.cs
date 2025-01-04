using System;

using Xabbo.Messages;
using Xabbo.Messages.Nitro;


namespace Xabbo.Core.Messages.Incoming;

/// <summary>
/// Received when the value of a dice is updated.
/// <para/>
/// Supported clients: <see cref="ClientType.Nitro"/>
/// <para/>
/// Identifiers:
/// <list type="bullet">
/// <item>Flash: <see cref="In.DiceValue"/></item>
/// <item>Shockwave: <see cref="In.DICE_VALUE"/></item>
/// </list>
/// </summary>
/// <param name="Id">The ID of the dice.</param>
/// <param name="Value">The updated dice value.</param>
public sealed record DiceValueMsg(int Id, int Value) : IMessage<DiceValueMsg>
{
    static Identifier IMessage<DiceValueMsg>.Identifier => In.Furniture_State;

    static DiceValueMsg IParser<DiceValueMsg>.Parse(in PacketReader p)
    {
        int id;
        int value;

        id = p.ReadInt();
        _ = p.ReadInt();
        int.TryParse(p.ReadString(),out value);


        return new(id, value);
    }

    void IComposer.Compose(in PacketWriter p)
    {
        p.WriteInt(Id);
        p.WriteInt(Value);
    }
}
