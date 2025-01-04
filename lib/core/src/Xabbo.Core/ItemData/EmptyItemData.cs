using Xabbo.Messages;

namespace Xabbo.Core;

/// <inheritdoc cref="IEmptyItemData"/>
public class EmptyItemData : ItemData, IEmptyItemData
{
    public EmptyItemData()
        : base(ItemDataType.Empty)
    { }

    public EmptyItemData(IEmptyItemData data)
        : this()
    { }

    protected override void Initialize(in PacketReader p)
    {
    }

    protected override void WriteData(in PacketWriter p)
    {
    }
}
