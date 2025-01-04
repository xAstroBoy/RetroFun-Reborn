using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Xabbo.Messages.Nitro;

namespace Xabbo.Messages;

/// <summary>
/// Manages messages between multiple clients using a mapping file.
/// </summary>
public sealed class MessageManager(string? filePath = null, ILoggerFactory? loggerFactory = null) : IMessageManager
{
    private readonly string _mapFilePath = filePath ??
        Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "xabbo", "messages.ini");

    private readonly ILogger Log = (ILogger?)loggerFactory?.CreateLogger<MessageManager>() ?? NullLogger.Instance;

    private readonly SemaphoreSlim _init = new(1);
    private readonly ReaderWriterLockSlim _lock = new();

    private MessageMap? _messageMap = new(); 

    private readonly Dictionary<Identifier, HashSet<MessageNames>> _identifierNames = [];
    private readonly Dictionary<(Direction Direction, MessageNames Names), Header> _headers = [];
    private readonly Dictionary<Header, MessageNames> _headerNames = [];

    /// <summary>
    /// Whether to fetch the message map file from the xabbo/messages
    /// GitHub repo upon initialization if it does not exist locally.
    /// </summary>
    public bool Fetch { get; set; } = true;

    /// <summary>
    /// The maximum age of the message map file after which it is invalidated.
    /// </summary>
    public TimeSpan MaxAge { get; set; } = TimeSpan.FromDays(7);

    public bool Available { get; private set; }
    public event Action? Loaded;

    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        if (!_init.Wait(0, CancellationToken.None))
            throw new InvalidOperationException("InitializeAsync may only be called once.");

        Log.LogInformation("Initializing message manager...");
        Log.LogDebug("Map file path is '{MapFilePath}'.", _mapFilePath);

        bool fetchMapFile = false;
        FileInfo mapFileInfo = new(_mapFilePath);

        if (!mapFileInfo.Exists)
        {
            if (!Fetch)
            {
                Log.LogError("Map file does not exist and Fetch is false.");
                throw new FileNotFoundException($"Message map file not found: '{_mapFilePath}'.");
            }
            Log.LogDebug("Map file does not exist.");
            fetchMapFile = true;
        }
        else if (Fetch && (DateTime.Now - mapFileInfo.LastWriteTime) >= MaxAge)
        {
            Log.LogDebug("Map file has reached max age ({MaxAge}).", MaxAge);
            fetchMapFile = true;
        }
        else if (mapFileInfo.Length == 0)
        {
            Log.LogDebug("Map file is empty.");
            fetchMapFile = Fetch;
        }

        Log.LogDebug("Loading BSS Headers...");

        _lock.EnterWriteLock();
        try
        {
            Log.LogInformation("Loaded {Count} identifiers from message map.", _messageMap.Count);
        }
        catch (Exception ex)
        {
            Log.LogError(ex, "Failed to load message map: {Error}.", ex.Message);
            throw;
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }

    public bool TryResolve(ReadOnlySpan<Identifier> identifiers,
        [NotNullWhen(true)] out Headers? headers,
        [NotNullWhen(false)] out Identifiers? unresolved)
    {
        if (identifiers.Length == 0)
            throw new ArgumentException("At least one identifier must be specified.", nameof(identifiers));

        headers = null;
        unresolved = null;

        foreach (var identifier in identifiers)
        {
            if (TryGetHeader(identifier, out Header header)) (headers ??= []).Add(header);
            else (unresolved ??= []).Add(identifier);
        }

        return unresolved is null;
    }

    public Headers Resolve(ReadOnlySpan<Identifier> identifiers)
    {
        if (TryResolve(identifiers, out Headers? headers, out Identifiers? unresolved))
            return headers;
        else
            throw new UnresolvedIdentifiersException(unresolved);
    }

    public Header Resolve(Identifier identifier)
    {
        if (!TryGetHeader(identifier, out Header header))
            throw new UnresolvedIdentifiersException([identifier]);
        return header;
    }

    public void Clear()
    {
        _lock.EnterWriteLock();
        try
        {
            Reset();
        }
        finally
        {
            Available = false;
            _lock.ExitWriteLock();
        }
    }

    private void Reset()
    {
        _headers.Clear();
        _headerNames.Clear();
        _identifierNames.Clear();

        Log.LogDebug("Message map reset.");
    }

    public void LoadMessages(IEnumerable<ClientMessage> messages)
    {
        _lock.EnterWriteLock();
        try
        {
      
            FillIdentifiers();

        }
        catch (Exception ex)
        {
            Log.LogError(ex, "Failed to load client messages: {Error}.", ex.Message);
            throw;
        }
        finally
        {
            _lock.ExitWriteLock();
        }

        Log.LogInformation("Loaded {Count} message headers.", _headers.Count);
        Available = true;
        Loaded?.Invoke();
    }

    public void FillIdentifiers()
    {
        Reset();

        // Extract the Hardcoded BSS Headers from Xabbo.Core.Nitro.In and Xabbo.Core.Nitro.Out
        // and add them to the MessageManager
        IReadOnlyList<Identifier> allIncomings = In.AllIdentifiers;
        IReadOnlyList<Identifier> allOutgoings = Out.AllIdentifiers;

        foreach (var incoming in allIncomings)
        {
            // Create a new HashSet<MessageNames> if it does not exist for this identifier.
            if (!_identifierNames.TryGetValue(incoming, out var nameSet))
            {
                nameSet = [new MessageNames().WithName(incoming.Client, incoming.Name)];
                _identifierNames[incoming] = nameSet;
            }

            if (nameSet.Count > 1)
                throw new Exception("Multiple message names conflict.");

            // Create a two-way mapping between Header and Direction/MessageNames.
            var names = nameSet.Single();
            _headers[(incoming.Direction, names)] = incoming.ToHeader(); 
            _headerNames[incoming.ToHeader()] = names;

            var generic_identifier = new Identifier(ClientType.None, incoming.Direction, incoming.Name, incoming.HeaderID);
            if (_identifierNames.TryGetValue(generic_identifier, out var existingNameSet))
            {
                existingNameSet.Add(names);
            }
            else
            {
                _identifierNames.Add(generic_identifier, [names]);
            }

        }

        foreach (var outgoing in allOutgoings)
        {

            // Create a new HashSet<MessageNames> if it does not exist for this identifier.
            if (!_identifierNames.TryGetValue(outgoing, out var nameSet))
            {
                nameSet = [new MessageNames().WithName(outgoing.Client, outgoing.Name)];
                _identifierNames[outgoing] = nameSet;
            }

            if (nameSet.Count > 1)
                throw new Exception("Multiple message names conflict.");

            // Create a two-way mapping between Header and Direction/MessageNames.
            var names = nameSet.Single();
            _headers[(outgoing.Direction, names)] = outgoing.ToHeader();
            _headerNames[outgoing.ToHeader()] = names;

            var generic_identifier = new Identifier(ClientType.None, outgoing.Direction, outgoing.Name, outgoing.HeaderID);
            if (_identifierNames.TryGetValue(generic_identifier, out var existingNameSet))
            {
                existingNameSet.Add(names);
            }
            else
            {
                _identifierNames.Add(generic_identifier, [names]);
            }
        }

    }

    public bool TryGetHeader(Identifier identifier, out Header header)
    {
        if (BitOperations.PopCount((uint)identifier.Client) > 1)
            throw new ArgumentException($"Identifier may only specify a single client. ({identifier})",
                nameof(identifier));
        if (identifier.Direction == Direction.None)
        {
            header = Header.Unknown;
            return false;
        }

        IReadOnlyList<Identifier> ids = null;
        if (identifier.Direction == Direction.In)
            ids = In.AllIdentifiers;
        else if (identifier.Direction == Direction.Out)
            ids = Out.AllIdentifiers;
        else
            throw new Exception("Invalid identifier direction.");
        var lookup_identifier = identifier.Name.Replace("n:", string.Empty);

        foreach (var items in ids)
        {
            var purged_name = items.Name.Replace("n:", string.Empty);
            if (purged_name == lookup_identifier)
            {
                header = items.ToHeader();
                return true;
            }
        }

        header = Header.Unknown;
        return false;   
    }

    public bool TryGetNames(Identifier identifier, out MessageNames names)
    {
        if (BitOperations.PopCount((uint)identifier.Client) > 1)
            throw new ArgumentException($"Identifier may only specify a single client. ({identifier})", nameof(identifier));

        _lock.EnterReadLock();
        try
        {
            if (_identifierNames.TryGetValue(identifier, out var set))
            {
                if (set.Count > 1)
                    throw new AmbiguousIdentifierException(identifier, set);
                names = set.Single();
                return true;
            }
            else
            {
                names = default;
                return false;
            }
        }
        finally { _lock.ExitReadLock(); }
    }

    public bool TryGetNames(Header header, out MessageNames identifiers)
    {
        _lock.EnterReadLock();
        try { return _headerNames.TryGetValue(header, out identifiers); }
        finally { _lock.ExitReadLock(); }
    }

    public bool Is(Header header, ReadOnlySpan<Identifier> identifiers)
    {
        foreach (var identifier in identifiers)
        {
            if (TryGetHeader(identifier, out Header h) && h.Equals(header))
                return true;
        }
        return false;
    }
}
