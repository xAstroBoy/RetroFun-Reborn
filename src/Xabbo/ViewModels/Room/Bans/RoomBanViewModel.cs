namespace Xabbo.ViewModels;

public class RoomBanViewModel(int id, string name) : ViewModelBase
{
    public int Id { get; } = id;
    public string Name { get; } = name;
}
