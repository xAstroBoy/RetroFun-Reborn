using Xabbo.Messages;
using Xabbo.Messages.Nitro;


namespace Xabbo.Core.Messages.Incoming;

/// <summary>
/// Received when the user's own figure is updated.
/// <para/>
/// Supported clients: <see cref="ClientType.Modern"/>
/// <para/>
/// Identifiers:
/// <list type="bullet">
/// <item>Flash: <see cref="In.FigureUpdate"/></item>
/// </list>
/// </summary>
/// <param name="Figure">The user's updated figure string.</param>
/// <param name="Gender">The user's updated gender.</param>
public sealed record FigureUpdateMsg(string Figure, Gender Gender) : IMessage<FigureUpdateMsg>
{
    static ClientType IMessage<FigureUpdateMsg>.SupportedClients => ClientType.Nitro;
    static Identifier IMessage<FigureUpdateMsg>.Identifier => In.User_Figure;

    static FigureUpdateMsg IParser<FigureUpdateMsg>.Parse(in PacketReader p) => new(
        Figure: p.ReadString(),
        Gender: H.ToGender(p.ReadString())
    );

    void IComposer.Compose(in PacketWriter p)
    {
        p.WriteString(Figure);
        p.WriteString(Gender.ToClientString());
    }
}
