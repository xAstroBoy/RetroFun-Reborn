using System;
using System.Collections.Immutable;

namespace Xabbo.Messages;

/// <summary>
/// Defines an association of message names between clients.
/// </summary>
public readonly record struct MessageNames(string? Nitro = null)
{
    private static readonly ImmutableArray<ClientType> ClientTypes = [
        ClientType.Nitro
    ];

    public string? GetName(ClientType client) => client switch
    {
        ClientType.Nitro => Nitro,
        _ => throw new Exception($"Unknown client: {client}"),
    };

    public MessageNames WithName(ClientType client, string name) => client switch
    {
        ClientType.Nitro => this with { Nitro = name },
        _ => throw new Exception($"Unknown client: {client}"),
    };

    public override int GetHashCode() => (
        Nitro?.ToUpperInvariant()!
    ).GetHashCode();

    public bool Equals(MessageNames other) =>
        string.Equals(Nitro, other.Nitro, StringComparison.OrdinalIgnoreCase);

    public override string ToString()
    {
        ClientType processed = ClientType.None;

        string result = "";
        for (int i = 0; i < ClientTypes.Length; i++)
        {
            ClientType current = ClientTypes[i];
            if ((processed & current) != 0)
                continue;

            string? name = GetName(ClientTypes[i]);
            if (name is null) continue;

            for (int j = i + 1; j < ClientTypes.Length; j++)
            {
                ClientType other = ClientTypes[j];
                if ((processed & other) != 0)
                    continue;

                if (name.Equals(GetName(other)))
                {
                    processed |= other;
                    current |= other;
                }
            }

            if (result.Length > 0)
                result += " ";
            result += $"{current.ToShortString()}:{name}";
        }

        return result;
    }
}
