using System;

using Xabbo.Messages;
using Xabbo.Messages.Nitro;


namespace Xabbo.Core.Messages.Outgoing;

/// <summary>
/// Sent when placing a wall item in a room.
/// <para/>
/// Supported clients: <see cref="ClientType.Flash"/>, <see cref="ClientType.Shockwave"/>
/// <para/>
/// Identifiers:
/// <list type="bullet">
/// <item>Flash: <see cref="Out.PlaceObject"/></item>
/// <item>Shockwave: <see cref="Out.PLACESTUFF"/></item>
/// </list>
/// </summary>
/// <param name="ItemId">The ID of the wall item to place.</param>
/// <param name="Location">The location to place the wall item at.</param>
public sealed record PlaceWallItemMsg(int ItemId, WallLocation Location) : IMessage<PlaceWallItemMsg>
{
    static Identifier IMessage<PlaceWallItemMsg>.Identifier => Out.Furniture_Place;

    static bool IMessage<PlaceWallItemMsg>.Match(in PacketReader p)
    {
        string content = p.ReadString();
        int index = content.IndexOf(' ');
        if (index < 0 || (index + 1) >= content.Length) return false;
        return content[index + 1] == ':';
    }

    static PlaceWallItemMsg IParser<PlaceWallItemMsg>.Parse(in PacketReader p)
    {
        string content = p.ReadString();

        int i = content.IndexOf(' ');
        if (i < 0)
            throw new Exception("No separator in PlaceWallItemMsg.");

        int.TryParse(content[..i], out int itemId);

        return new PlaceWallItemMsg(itemId, (WallLocation)content[(i + 1)..]);
    }

    void IComposer.Compose(in PacketWriter p)
    {
        p.WriteString($"{ItemId} {Location}");
    }
}
