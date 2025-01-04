using System.Web;

namespace Xabbo.Utility;

public static class UrlHelper
{
    public static string? AvatarImageUrl(string? name = null, string? figure = null,
        int direction = 2, int? headDirection = null, bool headOnly = false)
    {
        headDirection ??= direction;

        var query = HttpUtility.ParseQueryString("");
        query.Add("direction", direction.ToString());
        query.Add("head_direction", headDirection.Value.ToString());
        query.Add("size", "l");
        if (headOnly)
            query.Add("headonly", "1");
        if (string.IsNullOrWhiteSpace(figure))
            query.Add("user", name);
        else
            query.Add("figure", figure);

        return $"https://images.bsshotel.it/avatar/image?{query}";
    }

    public static string FurniIconUrl(string identifier)
    {
        identifier = identifier.Replace('*', '_');
        return $"https://images.bsshotel.it/dcr/hof_furni/icons/{identifier}_icon.png";
    }


}
