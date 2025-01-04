using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace Xabbo.Core;

public abstract class KeyValueDictionary : IReadOnlyDictionary<string, string>
{
    private readonly ImmutableDictionary<string, string> _entries;

    public IEnumerable<string> Keys => _entries.Keys;
    public IEnumerable<string> Values => _entries.Values;
    public int Count => _entries.Count;

    public string this[string key] => _entries[key];

    protected KeyValueDictionary(string filePath)
    {
        Dictionary<string, string> entries = new();

        foreach (string rawLine in File.ReadLines(filePath))
        {
            string line = rawLine.Trim();

            // Skip empty lines
            if (string.IsNullOrWhiteSpace(line))
                continue;

            // Determine if line uses '=' or ':'
            int eqIndex = line.IndexOf('=');
            int colonIndex = line.IndexOf(':');

            if (eqIndex < 0 && colonIndex < 0)
            {
                // No delimiter found
                Debug.Log($"Invalid line in {GetType().Name}: '{line}'.");
                continue;
            }

            string key;
            string value;

            if (eqIndex >= 0 && (colonIndex < 0 || eqIndex < colonIndex))
            {
                // Parse using '='
                key = line[..eqIndex].Trim();
                value = line[(eqIndex + 1)..].Trim();
            }
            else
            {
                // Parse using ':'
                key = line[..colonIndex].Trim().Trim('"');
                value = line[(colonIndex + 1)..].Trim().Trim('"', ',');
            }

            if (string.IsNullOrEmpty(key))
            {
                Debug.Log($"Invalid key in {GetType().Name}: '{line}'.");
                continue;
            }

            // Attempt to add the entry to the dictionary
            if (!entries.TryAdd(key, value))
            {
                Debug.Log($"Duplicate key in {GetType().Name}: '{key}'.");
            }
        }

        _entries = entries.ToImmutableDictionary();
    }


    public bool ContainsKey(string key) => _entries.ContainsKey(key);
    public bool TryGetValue(string key, [NotNullWhen(true)] out string? value) => _entries.TryGetValue(key, out value);
    public IEnumerator<KeyValuePair<string, string>> GetEnumerator() => _entries.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
