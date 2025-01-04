namespace Xabbo.ViewModels;

public abstract class ChatLogEntryViewModel : ViewModelBase
{
    public int EntryId { get; set; }
    public DateTime Timestamp { get; } = DateTime.Now;

    public virtual bool IsSelectable => true;
}
