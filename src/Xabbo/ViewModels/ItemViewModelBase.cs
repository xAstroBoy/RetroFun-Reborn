using ReactiveUI;
using Xabbo.Abstractions;
using Xabbo.Core;
using Xabbo.Core.GameData;
using Xabbo.Models;

namespace Xabbo.ViewModels;

public abstract class ItemViewModelBase : ViewModelBase
{
    public FurniInfo FurniInfo { get; private set; }

    public IItem Item { get; private set; }

    public int Id => Item.Id;
    public ItemType Type => Item.Type;
    public long TypeID => Item.TypeID;

    public string Name { get; private set; }

    public string Description { get; private set; }

    [Reactive] public bool IsHidden { get; set; }

    public IItemIcon? Icon { get; set; }

    public ItemViewModelBase(IItem item)
    {
        Item = item;

        if (item is IFloorItem or IWallItem)
        {
            MakeIcon(item);
        }
        else
        {
            MakeIconFromType(item);
        }
    }

    public void MakeIcon(IItem item)
    {
        var info = item.GetInfo();
        if (info != null)
        {
            FurniInfo = info;
            Name = FurniInfo.Name.IsNotNullOrEmptyOrWhiteSpace() ? FurniInfo.Name : FurniInfo.ClassName;
            Description = FurniInfo.Description.IsNotNullOrEmptyOrWhiteSpace() ? FurniInfo.Description : "No description available.";
            Icon = new ItemIcon(info.ClassName);
            if (item is IFloorItem { Data.IsLimitedRare: true } ltd)
                Name += $" #{ltd.Data.UniqueSerialNumber}";
        }
        else
        {
            Name = $"Unknown {item.TypeID}";
            Description = "Unknown item type";
            Icon = new ItemIcon(null);
        }

    }


    public void MakeIconFromType(IItem item)
    {
        var info = item.GetByType();
        if (info != null)
        {
            FurniInfo = info;
            Name = FurniInfo.Name.IsNotNullOrEmptyOrWhiteSpace() ? FurniInfo.Name : FurniInfo.ClassName;
            Description = FurniInfo.Description.IsNotNullOrEmptyOrWhiteSpace() ? FurniInfo.Description : "No description available.";
            Icon = new ItemIcon(info.ClassName);
            if (item is IFloorItem { Data.IsLimitedRare: true } ltd)
                Name += $" #{ltd.Data.UniqueSerialNumber}";
        }
        else
        {
            Name = $"Unknown {item.TypeID}";
            Description = "Unknown item type";
            Icon = new ItemIcon(null);
        }
    }
    public void UpdateItem(IItem item)
    {
        Item = item;
        this.RaisePropertyChanged(nameof(Item));
    }
}
