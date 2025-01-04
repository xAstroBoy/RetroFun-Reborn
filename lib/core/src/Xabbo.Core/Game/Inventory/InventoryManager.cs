﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

using Xabbo.Extension;


using Xabbo.Core.Events;
using Xabbo.Messages.Nitro;

namespace Xabbo.Core.Game;

/// <summary>
/// Manages the user's inventory.
/// </summary>
[Intercept]
public sealed partial class InventoryManager : GameStateManager
{
    private class InventoryItemIdComparer : IEqualityComparer<InventoryItem>
    {
        public static readonly InventoryItemIdComparer Default = new InventoryItemIdComparer();

        public int GetHashCode([DisallowNull] InventoryItem obj)
        {
            return obj is InventoryItem inventoryItem ? inventoryItem.ItemId.GetHashCode() : obj.GetHashCode();
        }

        public bool Equals(InventoryItem? x, InventoryItem? y)
        {
            return x?.ItemId == y?.ItemId;
        }
    }

    private readonly ILogger _logger;
    private readonly RoomManager _roomManager;
    private readonly TradeManager _tradeManager;

    private readonly SemaphoreSlim _loadSemaphore = new(1, 1);

    private readonly List<InventoryFragment> _fragments = [];
    private int _currentFragmentIndex;
    private int _totalFragments;

    private TaskCompletionSource<IEnumerable<InventoryItem>>? _loadInventoryItemsTcs;
    private Task<IInventory>? _loadInventoryTask;

    private Inventory? _inventory;

    private bool _isLoading;
    public bool IsLoading
    {
        get => _isLoading;
        set => Set(ref _isLoading, value);
    }

    private int _currentProgress;
    public int CurrentProgress
    {
        get => _currentProgress;
        set => Set(ref _currentProgress, value);
    }

    private int _maxProgress = -1;
    public int MaxProgress
    {
        get => _maxProgress;
        set => Set(ref _maxProgress, value);
    }

    public InventoryManager(IExtension extension,
        RoomManager roomManager,
        TradeManager tradeManager,
        ILoggerFactory? loggerFactory = null
    )
        : base(extension)
    {
        _logger = (ILogger?)loggerFactory?.CreateLogger<InventoryManager>() ?? NullLogger.Instance;
        _roomManager = roomManager;
        _tradeManager = tradeManager;

        _roomManager.FloorItemRemoved += OnFloorItemRemoved;
        _roomManager.WallItemRemoved += OnWallItemRemoved;

        _tradeManager.Completed += OnTradeCompleted;
    }

    public IInventory? Inventory => _inventory;

    protected override void OnDisconnected()
    {
        _inventory = null;
        _currentFragmentIndex = 0;
        _totalFragments = 0;
        Cleared?.Invoke();
    }

    /// <summary>
    /// Returns the user's inventory immediately if it is available
    /// and has not been invalidated, otherwise attempts to retrieve it from the server.
    /// </summary>
    /// <remarks>
    /// The user must be in a room to retrieve the inventory from the server.
    /// If the user is not in a room and a request to load the inventory is made, this method will time out.
    /// </remarks>
    public Task<IInventory> LoadInventoryAsync(
        int timeout = XabboConst.DefaultTimeout,
        int scanInterval = XabboConst.DefaultOriginsInventoryScanInterval,
        bool forceReload = false,
        CancellationToken cancellationToken = default)
    {
        if (!forceReload && Inventory is { IsInvalidated: false } inventory)
            return Task.FromResult((IInventory)inventory);

        return LoadInventoryInternalAsync(timeout, scanInterval, forceReload, cancellationToken);
    }

    private async Task<IInventory> LoadInventoryInternalAsync(
        int timeout, int scanInterval, bool forceReload,
        CancellationToken cancellationToken)
    {
        TaskCompletionSource<IInventory> loadInventoryTcs;
        Task<IInventory> loadInventoryTask;
        Task<IEnumerable<InventoryItem>>? loadItemsTask = null;
        CancellationTokenSource? cts = null;

        _loadSemaphore.Wait(cancellationToken);
        try
        {
            if (_loadInventoryTask is not null)
                return await _loadInventoryTask;

            if (!forceReload && _inventory is { IsInvalidated: false } currentInventory)
                return currentInventory;

            _inventory = null;
            _fragments.Clear();
            CurrentProgress = 1;
            MaxProgress = -1;
            IsLoading = true;

            loadInventoryTcs = new TaskCompletionSource<IInventory>(TaskCreationOptions.RunContinuationsAsynchronously);
            _loadInventoryTask = loadInventoryTask = loadInventoryTcs.Task;

            cts = CancellationTokenSource.CreateLinkedTokenSource(Interceptor.DisconnectToken, cancellationToken);
            cts.Token.Register(() => loadInventoryTcs.TrySetCanceled());
            if (timeout > 0)
                cts.CancelAfter(timeout);

            loadItemsTask = LoadInventoryModernAsync(cts.Token);
        }
        finally { _loadSemaphore.Release(); }

        Cleared?.Invoke();

        IEnumerable<InventoryItem>? items = null;
        Inventory? inventory = null;

        try
        {
            items = await loadItemsTask;
        }
        catch (Exception ex)
        {
            loadInventoryTcs.TrySetException(ex);
            throw;
        }
        finally
        {
            _loadSemaphore.Wait(CancellationToken.None);
            try
            {
                if (items is not null)
                {
                    _inventory = inventory = new Inventory();
                    foreach (InventoryItem item in items)
                    {
                        if (!_inventory.TryAdd(item))
                        {
                            _logger.LogWarning("Failed to add inventory item {itemId}!", item.ItemId);
                        }
                    }
                }

                _loadInventoryTask = null;

                IsLoading = false;
                CurrentProgress = 0;
                MaxProgress = -1;
            }
            finally { _loadSemaphore.Release(); }
        }

        if (inventory is null)
            throw new Exception("Failed to load inventory.");

        Loaded?.Invoke();
        return inventory;
    }

    private async Task<IEnumerable<InventoryItem>> LoadInventoryModernAsync(CancellationToken cancellationToken)
    {
        try
        {
            var loadItemsTcs = _loadInventoryItemsTcs
                = new TaskCompletionSource<IEnumerable<InventoryItem>>(TaskCreationOptions.RunContinuationsAsynchronously);
            cancellationToken.Register(() => loadItemsTcs.TrySetCanceled());

            _logger.LogDebug("Requesting inventory");
            Interceptor.Send(Out.User_Request_Furni_Inventory);

            return await loadItemsTcs.Task;
        }
        finally
        {
            _loadInventoryItemsTcs = null;
        }
    }

    

    // On Origins:
    // Assume we have picked up the item if it is removed from the room and we are the room owner.
    // We need to detect special cases such as:
    // - A sticky note being deleted
    // - A gift being opened (not sure how to detect this yet)
    // But fortunately Origins uses furni identifiers so it is easier to detect this
    // without access to the furni data. Unforunately this means it must be hard-coded.

    private void OnFloorItemRemoved(FloorItemEventArgs e) => HandleRemovedFurni(e.Item);
    private void OnWallItemRemoved(WallItemEventArgs e) => HandleRemovedFurni(e.Item);

    private void HandleRemovedFurni(IFurni furni)
    {
 
    }

    private void OnTradeCompleted(TradeCompletedEventArgs e)
    {
        
    }
}
