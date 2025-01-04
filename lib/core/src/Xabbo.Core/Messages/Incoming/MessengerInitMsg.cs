using System.Collections.Generic;
using Xabbo.Messages;
using Xabbo.Messages.Nitro;


namespace Xabbo.Core.Messages.Incoming;

/// <summary>
/// Received when initializing the messenger.
/// <para/>
/// Supported clients: <see cref="ClientType.Nitro"/>
/// <para/>
/// Identifiers:
/// <list type="bullet">
/// <item>Flash: <see cref="In.MessengerInit"/></item>
/// <item>Shockwave: <see cref="In.FRIEND_LIST_INIT"/></item>
/// </list>
/// </summary>
public sealed record MessengerInitMsg : IMessage<MessengerInitMsg>
{
    public static Identifier Identifier => In.Messenger_Init;

    public string PersistentMessage { get; init; } = "";
    public int UserLimit { get; init; }
    public int NormalLimit { get; init; }
    public int ExtendedLimit { get; init; }
    public List<Friend> Friends { get; init; } = [];
    public List<CampaignMessage> CampaignMessages { get; init; } = [];
    public List<(int Id, string Name)> Categories { get; init; } = [];

    static MessengerInitMsg IParser<MessengerInitMsg>.Parse(in PacketReader p)
    {
        MessengerInitMsg msg = new()
        {
            UserLimit = p.ReadInt(), NormalLimit = p.ReadInt(), ExtendedLimit = p.ReadInt(),
        };
        int n = p.ReadLength();
        for (int i = 0; i < n; i++)
            msg.Categories.Add((p.ReadInt(), p.ReadString()));
        return msg;
    }

    void IComposer.Compose(in PacketWriter p)
    {
        p.WriteString(PersistentMessage);
        p.WriteInt(UserLimit);
        p.WriteInt(NormalLimit);
        p.WriteInt(ExtendedLimit);
        p.ComposeArray(Friends);
        p.ComposeArray(CampaignMessages);
    }
}
