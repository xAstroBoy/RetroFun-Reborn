using Xabbo.Core;
using Xabbo.Core.GameData;

namespace Xabbo.ViewModels;

public class FurniInfoViewModel(FurniInfo info)
{
    public FurniInfo Info { get; } = info;

    public string Identifier => Info.Name.IsNotNullOrEmptyOrWhiteSpace() ? Info.Name : Info.ClassName;
    public ItemType Type => Info.Type;
    public long TypeID => Info.TypeID;
    public string TypeKind => $"{Type}/{TypeID}";
    public string Description => Info.Description;

    public string ClassName => Info.ClassName;
    

}
