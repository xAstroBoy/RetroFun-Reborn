using System;

namespace Xabbo.Core;

/// <summary>
/// Represents the duration of a room ban.
/// </summary>
public enum BanDuration
{
    Hour,
    Day,
    Permanent
}

public static partial class XabboEnumExtensions
{
    /// <summary>
    /// Gets the client-specific string representation of the specified ban duration.
    /// </summary>
    /// <exception cref="Exception">If an invalid ban duration is specified.</exception>
    public static string ToClientString(this BanDuration banDuration, ClientType client) => client switch
    {
        _ => banDuration switch
        {
            BanDuration.Hour => "RWUAM_BAN_USER_HOUR",
            BanDuration.Day => "RWUAM_BAN_USER_DAY",
            BanDuration.Permanent => "RWUAM_BAN_USER_PERM",
            _ => throw new Exception($"Unknown {client} ban duration: '{banDuration}'.")
        }
    };
}
