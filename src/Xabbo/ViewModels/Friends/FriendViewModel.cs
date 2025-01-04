namespace Xabbo.ViewModels;

public sealed partial class FriendViewModel : ViewModelBase
{
    public int Id { get; set; }
    [Reactive] public bool IsOnline { get; set; }
    [Reactive] public string Name { get; set; } = "";
    [Reactive] public string Motto { get; set; } = "";
    [Reactive] public string Figure { get; set; } = "";

    [Reactive] public string? ModernFigure { get; set; }

    public FriendViewModel() { }
}
