namespace Xabbo.Core.GameData;

/// <summary>
/// The default <see cref="IGameDataUrlProvider"/> implementation.
/// </summary>
public class GameDataUrlProvider : IGameDataUrlProvider
{
    public string? GetHashesUrl(Hotel hotel)
    {

        return null;
    }

    public string? GetGameDataUrl(Hotel hotel, GameDataType type, string? hash = null)
    {
        return null;
    }
}
