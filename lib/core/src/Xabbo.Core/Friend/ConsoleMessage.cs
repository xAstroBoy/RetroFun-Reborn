using Xabbo.Messages;

namespace Xabbo.Core;

public class ConsoleMessage : IParserComposer<ConsoleMessage>
{
    public int ChatId { get; set; }
    public string Content { get; set; } = "";
    public int SecondsSinceSent { get; set; }
    public string? Time { get; set; }
    public string MessageId { get; set; } = "";
    public int ConfirmationId { get; set; }
    public int SenderId { get; set; }
    public string? SenderName { get; set; }
    public string SenderFigure { get; set; } = "";

    static ConsoleMessage IParser<ConsoleMessage>.Parse(in PacketReader p) => p.Client switch
    {
        _ => new()
        {
            ChatId = p.ReadInt(),
            Content = p.ReadString(),
            SecondsSinceSent = p.ReadInt(),
            MessageId = p.ReadString(),
            ConfirmationId = p.ReadInt(),
            SenderId = p.ReadInt(),
            SenderName = p.ReadString(),
            SenderFigure = p.ReadString(),
        }
    };

    void IComposer.Compose(in PacketWriter p)
    {
        p.WriteInt(ChatId);
        p.WriteString(Content);
        p.WriteInt(SecondsSinceSent);
        p.WriteString(MessageId);
        p.WriteInt(ConfirmationId);
        p.WriteInt(SenderId);
        p.WriteString(SenderName ?? "");
        p.WriteString(SenderFigure);
    }
}
