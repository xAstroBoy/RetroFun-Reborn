using System;

namespace Xabbo;

/// <summary>
/// Thrown when an operation is not supported for the current client.
/// </summary>
public sealed class UnsupportedClientException(ClientType client)
    : Exception($"This operation is not supported for the {client} client.")
{
    /// <summary>
    /// The client that is not supported.
    /// </summary>
    public ClientType Client { get; } = client;

    /// <summary>
    /// Throws if the client is any of the specified clients.
    /// </summary>
    public static void ThrowIf(ClientType client, ClientType clients)
    {
        if ((client & clients) != ClientType.None)
            throw new UnsupportedClientException(client);
    }

    /// <summary>
    /// Throws if the client is unknown or any of the specified clients.
    /// </summary>
    public static void ThrowIfNoneOr(ClientType client, ClientType clients)
    {
        if ((client & ClientType.Nitro) == ClientType.None)
            throw new UnsupportedClientException(client);
        if ((client & clients) != ClientType.None)
            throw new UnsupportedClientException(client);
    }

}
