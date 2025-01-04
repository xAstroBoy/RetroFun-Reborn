using Xabbo.Messages;
using Xabbo.Messages.Nitro;


namespace Xabbo.Core.Messages.Incoming;

/// <summary>
/// Received when the user's credit balance changes or is explicitly requested.
/// <para/>
/// Response for <see cref="Outgoing.GetCreditBalanceMsg"/>.
/// <para/>
/// Supported clients: <see cref="ClientType.Nitro"/>
/// <para/>
/// Identifiers:
/// <list type="bullet">
/// <item>Flash: <see cref="In.CreditBalance"/></item>
/// <item>Shockwave: <see cref="In.PURSE"/></item>
/// </list>
/// </summary>
public sealed record CreditBalanceMsg(int Credits) : IMessage<CreditBalanceMsg>
{
    static Identifier IMessage<CreditBalanceMsg>.Identifier => In.User_Credits;

    static CreditBalanceMsg IParser<CreditBalanceMsg>.Parse(in PacketReader p)
        => new((int)(FloatString)p.ReadString());

    void IComposer.Compose(in PacketWriter p) => p.WriteString(Credits.ToString());
}
