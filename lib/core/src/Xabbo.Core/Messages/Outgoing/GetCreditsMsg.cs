﻿using Xabbo.Messages;

using Xabbo.Core.Messages.Incoming;
using Xabbo.Messages.Nitro;

namespace Xabbo.Core.Messages.Outgoing;

/// <summary>
/// Sent when requesting the user's credit balance.
/// <para/>
/// Request for <see cref="CreditBalanceMsg"/>. Returns an <see cref="int"/> indicating the user's current balance.
/// <para/>
/// Supported clients: <see cref="ClientType.Nitro"/>
/// <para/>
/// Identifiers:
/// <list type="bullet">
/// <item>Flash: <see cref="Out.GetCreditsInfo"/></item>
/// <item>Shockwave: <see cref="Out.GET_CREDITS"/></item>
/// </list>
/// </summary>
public sealed record GetCreditBalanceMsg : IRequestMessage<GetCreditBalanceMsg, CreditBalanceMsg, int>
{
    static Identifier IMessage<GetCreditBalanceMsg>.Identifier => Out.User_Currency;
    int IResponseData<CreditBalanceMsg, int>.GetData(CreditBalanceMsg msg) => msg.Credits;
    static GetCreditBalanceMsg IParser<GetCreditBalanceMsg>.Parse(in PacketReader p) => new();
    void IComposer.Compose(in PacketWriter p) { }
}
