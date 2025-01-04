using System.Collections.Generic;

using Xabbo.Messages;
using Xabbo.Messages.Nitro;


namespace Xabbo.Core.Messages.Incoming;

/// <summary>
/// Represents a list of wired movement updates.
/// <para/>
/// Received when objects in the room are moved by wired.
/// <para/>
/// Supported clients: <see cref="ClientType.Modern"/>
/// <para/>
/// Identifiers: <see cref="In.WiredMovements"/>
/// </summary>
public sealed class WiredMovementsMsg : List<WiredMovement>, IMessage<WiredMovementsMsg>
{
    static ClientType IMessage<WiredMovementsMsg>.SupportedClients => ClientType.None;
    static Identifier IMessage<WiredMovementsMsg>.Identifier => Identifier.Unknown;

    public WiredMovementsMsg() { }
    public WiredMovementsMsg(int capacity) : base(capacity) { }
    public WiredMovementsMsg(IEnumerable<WiredMovement> collection) : base(collection) { }

    static WiredMovementsMsg IParser<WiredMovementsMsg>.Parse(in PacketReader p) => new(p.ParseArray<WiredMovement>());
    void IComposer.Compose(in PacketWriter p) => p.ComposeArray(this);
}
