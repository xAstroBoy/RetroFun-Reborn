using System;

namespace Xabbo;

/// <summary>
/// Represents a type of game client.
/// </summary>
[Flags]
public enum ClientType
{
    /// <summary>
    /// Represents no particular client.
    /// </summary>
    None,
    /// <summary>
    /// Represents the Nitro client.
    /// </summary>
    Nitro = 1,
}
