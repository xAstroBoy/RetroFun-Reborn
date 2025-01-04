using System;

namespace Xabbo.Messages;

/// <summary>
/// Represents a client type, direction, message name, and an optional header number.
/// </summary>
/// <remarks>
/// Used to associate a message name with a <see cref="HeaderID"/> that is resolved at runtime.
/// If a header is provided, runtime resolution can be skipped.
/// </remarks>
public readonly record struct Identifier
{
    public ClientType Client { get; }
    public Direction Direction { get; }
    public string Name { get; }
    public short? HeaderID { get; }

    public static Header GetHeader(Identifier id) => new(id.Direction, id.HeaderID.GetValueOrDefault(0));

    public static readonly Identifier Unknown = new(ClientType.None, Direction.None, "");

    /// <summary>
    /// Initializes a new instance of the <see cref="Identifier"/> struct.
    /// </summary>
    /// <param name="client">The client type.</param>
    /// <param name="direction">The message direction.</param>
    /// <param name="name">The message name.</param>
    /// <param name="headerId">The optional header number.</param>
    public Identifier(ClientType client, Direction direction, string name, short? headerId = null)
    {
        Client = client;
        Direction = direction;
        Name = name ?? throw new ArgumentNullException(nameof(name));
        HeaderID = headerId;
    }

    public override int GetHashCode()
    {
        // Include Header in the hash code if it's set
        return HeaderID.HasValue
            ? HashCode.Combine(Client, Direction, Name.ToUpperInvariant(), HeaderID.Value)
            : HashCode.Combine(Client, Direction, Name.ToUpperInvariant());
    }

    public bool Equals(Identifier other) =>
        Client == other.Client &&
        Direction == other.Direction &&
        string.Equals(Name, other.Name, StringComparison.InvariantCultureIgnoreCase) &&
        HeaderID == other.HeaderID;

    /// <summary>
    /// Returns a string that represents the current identifier, optionally including direction and header.
    /// </summary>
    /// <param name="includeDirection">Whether to include the direction in the string.</param>
    /// <param name="includeHeader">Whether to include the header in the string.</param>
    /// <returns>A string representation of the identifier.</returns>
    public string ToString(bool includeDirection, bool includeHeader = false)
    {
        string result = "";
        if (includeDirection && Direction != Direction.None)
            result += Direction.ToShortString() + ":";
        if (Client != ClientType.None)
            result += Client.ToShortString() + ":";
        result += Name;
        if (includeHeader && HeaderID.HasValue)
            result += $" (Header: {HeaderID.Value})";
        return result;
    }

    public override string ToString() => ToString(false);

    /// <summary>
    /// Creates an <see cref="Identifier"/> from a direction and name without a client or header.
    /// </summary>
    public static Identifier FromDirectionAndName(Direction direction, string name) =>
        new(ClientType.None, direction, name);

    /// <summary>
    /// Creates an <see cref="Identifier"/> from a client, direction, and name without a header.
    /// </summary>
    public static Identifier FromClientAndDirection(ClientType client, Direction direction, string name) =>
        new(client, direction, name);

    /// <summary>
    /// Creates an <see cref="Identifier"/> from a client, direction, name, and header.
    /// </summary>
    public static Identifier CreateWithHeader(ClientType client, Direction direction, string name, short header) =>
        new(client, direction, name, header);
}

// extensions   

public static class IdentifierUtils
{
    public static Header ToHeader(this Identifier id) => new(id.Direction, id.HeaderID.GetValueOrDefault(0));

    public static bool Is(this Identifier id, Identifier other) =>
        id.Client == other.Client &&
        id.Direction == other.Direction &&
        string.Equals(id.Name, other.Name, StringComparison.InvariantCultureIgnoreCase) &&
        id.HeaderID == other.HeaderID;

    public static bool Is(this Identifier id, Header header) =>
    id.Direction == header.Direction &&
        id.HeaderID == header.Value;

    public static bool Is(this Header header, Identifier id) =>
        id.Direction == header.Direction &&
        id.HeaderID == header.Value;
} 
