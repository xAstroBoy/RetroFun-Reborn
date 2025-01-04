﻿
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

using Xabbo.Interceptor;

using Xabbo.Core.Events;
using Xabbo.Core.Messages.Incoming;

namespace Xabbo.Core.Game;

/// <summary>
/// Manages information about the current room, the user's permissions in the room, its furni, avatars and chat.
/// </summary>
[Intercept]
public sealed partial class RoomManager(IInterceptor interceptor, ILoggerFactory? loggerFactory = null)
    : GameStateManager(interceptor)
{
    private readonly ILogger Log = (ILogger?)loggerFactory?.CreateLogger<RoomManager>() ?? NullLogger.Instance;
    private readonly Dictionary<int, RoomData> _roomDataCache = [];

    private Room? _currentRoom;

    private bool _gotHeightMap, _gotUsers, _gotObjects, _gotItems;

    private bool _isRingingDoorbell;
    /// <summary>
    /// Gets whether the user is currently ringing the doorbell to a room.
    /// </summary>
    public bool IsRingingDoorbell
    {
        get => _isRingingDoorbell;
        private set => Set(ref _isRingingDoorbell, value);
    }

    private bool _isInQueue;
    /// <summary>
    /// Gets whether the user is currently in a queue to enter a room.
    /// </summary>
    public bool IsInQueue
    {
        get => _isInQueue;
        private set => Set(ref _isInQueue, value);
    }

    private int _queuePosition;
    /// <summary>
    /// Gets the user's current position in the queue.
    /// </summary>
    public int QueuePosition
    {
        get => _queuePosition;
        private set => Set(ref _queuePosition, value);
    }

    private bool _isSpectating;
    /// <summary>
    /// Gets whether the user is currently spectating a room.
    /// </summary>
    public bool IsSpectating
    {
        get => _isSpectating;
        set => Set(ref _isSpectating, value);
    }

    private bool _isLoadingRoom;
    /// <summary>
    /// Gets whether a room is currently being loaded.
    /// </summary>
    public bool IsLoadingRoom
    {
        get => _isLoadingRoom;
        private set => Set(ref _isLoadingRoom, value);
    }

    private bool _isInRoom;
    /// <summary>
    /// Gets whether the user is currently in a room.
    /// </summary>
    public bool IsInRoom
    {
        get => _isInRoom;
        private set => Set(ref _isInRoom, value);
    }

    /// <summary>
    /// Gets the instance of the room that the user is currently in.
    /// Returns <c>null</c> if the user is not in a room.
    /// </summary>
    public IRoom? Room => _currentRoom;

    private RightsLevel _rightsLevel;
    /// <summary>
    /// Gets the user's rights level in the current room.
    /// </summary>
    public RightsLevel RightsLevel
    {
        get => _rightsLevel;
        private set
        {
            if (Set(ref _rightsLevel, value))
            {
                RaisePropertyChanged(nameof(HasRights));
                RefreshPermissions();
            }
        }
    }

    /// <summary>
    /// Gets whether the user has rights in the current room.
    /// </summary>
    public bool HasRights => RightsLevel > RightsLevel.None;

    private bool _isOwner;
    /// <summary>
    /// Gets whether the user is the owner of the current room.
    /// </summary>
    public bool IsOwner
    {
        get => true; //_isOwner;
        private set => Set(ref _isOwner, value);
    }

    /// <summary>
    /// Gets whether the user has permission to mute in the current room.
    /// </summary>
    public bool CanMute => CheckPermission(Room?.Data?.Moderation.Mute);

    /// <summary>
    /// Gets whether the user has permission to kick in the current room.
    /// </summary>
    public bool CanKick => CheckPermission(Room?.Data?.Moderation.Kick);

    /// <summary>
    /// Gets whether the user has permission to ban in the current room.
    /// </summary>
    public bool CanBan => CheckPermission(Room?.Data?.Moderation.Ban);

    /// <summary>
    /// Gets whether the user has permission to trade in the current room.
    /// </summary>
    public bool CanTrade => Room?.Data?.Trading switch
    {
        TradePermissions.Allowed => true,
        TradePermissions.RightsHolders => HasRights,
        TradePermissions.None or TradePermissions.NotAllowed => false,
        _ => false
    };

    /// <summary>
    /// Retrieves the room data from the cache if it is available.
    /// </summary>
    public bool TryGetRoomData(int roomId, [NotNullWhen(true)] out RoomData? data) => _roomDataCache.TryGetValue(roomId, out data);

    protected override void OnDisconnected() => LeaveRoom();

    /// <summary>
    /// Ensures the user is in a room.
    /// If so, this method will return <c>true</c>
    /// and <paramref name="room"/> will contain the current room instance.
    /// </summary>
    public bool EnsureInRoom([NotNullWhen(true)] out IRoom? room)
        => (room = _currentRoom) is not null && IsInRoom;

    private bool EnsureRoomInternal([NotNullWhen(true)] out Room? room)
    {
        room = _currentRoom;
        if (room is null)
            Log.LogDebug("Current room is null.");
        return room is not null;
    }

    private bool EnsureLoadingRoom([NotNullWhen(true)] out Room? room)
    {
        if (!IsLoadingRoom)
        {
            room = null;
            Log.LogDebug("Not loading room.");
            return false;
        }

        return EnsureRoomInternal(out room);
    }

    private bool EnsureInRoom([NotNullWhen(true)] out Room? room)
    {
        if (!IsInRoom)
        {
            room = null;
            Log.LogDebug("Not in room.");
            return false;
        }

        return EnsureRoomInternal(out room);
    }

    private void EnteringRoom(int id, string? model = null)
    {
        if (_currentRoom is { } currentRoom)
        {
            if (currentRoom.Id == id)
            {
                if (!IsLoadingRoom)
                {
                    Log.LogWarning("Entering room: current room is not null and not loading room.");
                }

                if (currentRoom.Model is null)
                {
                    currentRoom.Model = model;
                    RaisePropertyChanged(nameof(Room));
                }
                return;
            }

            LeaveRoom();
        }

        Log.LogDebug("Entering room #{Id}.", id);

        if (_roomDataCache.TryGetValue(id, out RoomData? data))
        {
            Log.LogDebug("Loaded room data from cache.");
        }
        else
        {
            Log.LogWarning("Failed to load room data from cache.");
        }

        _currentRoom = new Room(id, data) { Model = model! };
        RaisePropertyChanged(nameof(Room));

        IsLoadingRoom = true;
        Entering?.Invoke();
    }


    private void EnterRoom()
    {
        if (_currentRoom is null)
        {
            Log.LogWarning("Enter room: current room is null.");
            return;
        }

        IsInQueue = false;
        IsRingingDoorbell = false;
        IsLoadingRoom = false;
        IsInRoom = true;

        Log.LogInformation("Entered room. (id:{RoomId}, name:{Name})", _currentRoom.Id, _currentRoom.Data?.Name ?? "?");
        Entered?.Invoke(new RoomEventArgs(_currentRoom));
    }

    private void UpdateRoomData(RoomData roomData)
    {
        if (_currentRoom is { } room && room.Id == roomData.Id)
        {
            Log.LogDebug("Room data updated. (name:{Name})", roomData.Name);

            room.Data = roomData;
            // Moderation and trading permissions may have changed.
            RefreshPermissions();
            RoomDataUpdated?.Invoke(new RoomDataEventArgs(roomData));
        }
    }

    private void UpdateChatSettings(ChatSettings chatSettings)
    {
        if (_currentRoom is { Data: RoomData roomData })
        {
            roomData.ChatSettings = chatSettings;
            RoomDataUpdated?.Invoke(new RoomDataEventArgs(roomData));
        }
    }

    private void UpdateRightsLevel(RightsLevel rightsLevel)
    {
        RightsLevel = rightsLevel;
        Log.LogDebug("Rights level updated to {Level}.", rightsLevel);
        RightsUpdated?.Invoke();
    }

    private void RefreshPermissions()
    {
        RaisePropertyChanged(nameof(CanMute));
        RaisePropertyChanged(nameof(CanKick));
        RaisePropertyChanged(nameof(CanBan));
        RaisePropertyChanged(nameof(CanTrade));
    }

    #region - Furni -
    void LoadFloorItems(IEnumerable<FloorItem> items)
    {
        if (!EnsureLoadingRoom(out Room? room))
            return;

        List<FloorItem> newItems = [];

        foreach (FloorItem item in items)
        {
            if (room.FloorItems.TryAdd(item.Id, item))
            {
                newItems.Add(item);
            }
            else
            {
                Log.LogWarning("Failed to add floor item #{Id}.", item.Id);
            }
        }

        if (newItems.Count > 0)
        {
            Log.LogDebug("Loaded {Count} floor items.", newItems.Count);
            FloorItemsLoaded?.Invoke(new FloorItemsEventArgs(items));
        }

    }

    void AddFloorItem(FloorItem item)
    {
        if (!EnsureInRoom(out Room? room))
            return;

        if (room.FloorItems.TryAdd(item.Id, item))
        {
            Log.LogDebug("Floor item #{Id} added.", item.Id);
            FloorItemAdded?.Invoke(new FloorItemEventArgs(item));
        }
        else
        {
            Log.LogWarning("Failed to add floor item #{Id}.", item.Id);
        }
    }

    void UpdateFloorItem(FloorItem item)
    {
        if (!EnsureInRoom(out Room? room))
            return;

        if (room.FloorItems.TryGetValue(item.Id, out FloorItem? previousItem))
        {
            item.OwnerName = previousItem.OwnerName;
            item.IsHidden = previousItem.IsHidden;

            if (room.FloorItems.TryUpdate(item.Id, item, previousItem))
            {
                Log.LogTrace("Floor item #{Id} updated.", item.Id);
                FloorItemUpdated?.Invoke(new FloorItemUpdatedEventArgs(previousItem, item));
            }
            else
            {
                Log.LogWarning("Failed to update floor item #{Id}.", item.Id);
            }
        }
        else
        {
            Log.LogWarning("Failed to find floor item #{Id} to update.", item.Id);
        }
    }

    void UpdateFloorItemData(IEnumerable<(int, ItemData)> updates)
    {
        if (!EnsureInRoom(out Room? room))
            return;

        foreach (var (id, data) in updates)
        {
            if (!room.FloorItems.TryGetValue(id, out FloorItem? item))
            {
                Log.LogWarning("Failed to find floor item #{Id} to update.", id);
                continue;
            }

            IItemData previousData = item.Data;
            item.Data = data;

            Log.LogTrace("Floor item #{Id} data updated.", item.Id);
            FloorItemDataUpdated?.Invoke(new FloorItemDataUpdatedEventArgs(item, previousData));
        }
    }

    void UpdateDiceValue(int id, int value)
    {
        if (!EnsureInRoom(out Room? room))
            return;

        if (!room.FloorItems.TryGetValue(id, out FloorItem? dice))
        {
            Log.LogWarning("Failed to find floor item #{Id} to update.", id);
            return;
        }

        int previousValue = dice.Data.State;
        dice.Data.Value = value.ToString();

        Log.LogTrace("Dice #{Id} value updated. ({PreviousValue} -> {CurrentValue})", id, previousValue, value);
        DiceUpdated?.Invoke(new DiceUpdatedEventArgs(dice, previousValue, value));
    }

    void RemoveFloorItem(int id)
    {
        if (!EnsureInRoom(out Room? room))
            return;

        if (room.FloorItems.TryRemove(id, out FloorItem? item))
        {
            Log.LogDebug("Floor item #{Id} removed.", item.Id);
            FloorItemRemoved?.Invoke(new FloorItemEventArgs(item));
        }
        else
        {
            Log.LogWarning("Failed to remove floor item #{Id}.", id);
        }

    }
    #endregion

    #region - Wall items -
    void LoadWallItems(IEnumerable<WallItem> items)
    {
        if (!EnsureLoadingRoom(out Room? room))
            return;

        List<WallItem> newItems = [];

        foreach (var item in items)
        {
            if (room.WallItems.TryAdd(item.Id, item))
            {
                newItems.Add(item);
            }
            else
            {
                Log.LogWarning("Failed to add wall item #{Id}.", item.Id);
            }
        }

        if (newItems.Count > 0)
        {
            Log.LogDebug("Loaded {Count} wall items.", newItems.Count);
            WallItemsLoaded?.Invoke(new WallItemsEventArgs(items));
        }
    }

    void AddWallItem(WallItem item)
    {
        if (!EnsureInRoom(out Room? room))
            return;

        if (room.WallItems.TryAdd(item.Id, item))
        {
            Log.LogDebug("Wall item #{Id} added.", item.Id);
            WallItemAdded?.Invoke(new WallItemEventArgs(item));
        }
        else
        {
            Log.LogWarning("Failed to add wall item #{Id}.", item.Id);
        }
    }

    void UpdateWallItem(WallItem item)
    {
        if (!EnsureInRoom(out Room? room))
            return;

        if (room.WallItems.TryGetValue(item.Id, out WallItem? previousItem))
        {
            item.OwnerName = previousItem.OwnerName;
            item.IsHidden = previousItem.IsHidden;

            if (room.WallItems.TryUpdate(item.Id, item, previousItem))
            {
                Log.LogTrace("Wall item #{Id} updated.", item.Id);
                WallItemUpdated?.Invoke(new WallItemUpdatedEventArgs(previousItem, item));
            }
            else
            {
                Log.LogWarning("Failed to update wall item #{Id}.", item.Id);
            }
        }
        else
        {
            Log.LogWarning("Failed to find wall item #{Id} to update.", item.Id);
        }
    }

    void RemoveWallItem(int id)
    {
        if (!EnsureInRoom(out Room? room))
            return;

        if (room.WallItems.TryRemove(id, out WallItem? item))
        {
            Log.LogDebug("Wall item #{Id} removed.", id);
            WallItemRemoved?.Invoke(new WallItemEventArgs(item));
        }
        else
        {
            Log.LogWarning("Failed to remove wall item #{Id}.", id);
        }
    }
    #endregion

    #region Avatars
    void AddAvatars(IEnumerable<Avatar> avatars)
    {
        if (!IsLoadingRoom && !IsInRoom)
        {
            Log.LogWarning("Not loading or in room.");
            return;
        }

        if (!EnsureRoomInternal(out Room? room))
            return;

        List<Avatar> added = [];

        foreach (Avatar avatar in avatars)
        {
            if (room.Avatars.TryAdd(avatar.Index, avatar))
            {
                added.Add(avatar);
                Log.LogTrace("Avatar @{Index} '{Name}' added.", added[0].Index, added[0].Name);
                AvatarAdded?.Invoke(new AvatarEventArgs(avatar));
            }
            else
            {
                Log.LogWarning("Failed to add avatar @{Index} '{Name}'.", avatar.Index, avatar.Id);
            }
        }

        if (added.Count > 0)
        {
            Log.LogDebug("Added {Count} avatars.", added.Count);
            AvatarsAdded?.Invoke(new AvatarsEventArgs(added));
        }
    }

    void UpdateAvatars(IEnumerable<AvatarStatus> updates)
    {
        if (!EnsureInRoom(out Room? room))
            return;

        List<IAvatar> updated = [];

        foreach (var update in updates)
        {
            if (!room.Avatars.TryGetValue(update.Index, out Avatar? avatar))
            {
                Log.LogWarning("Failed to find avatar @{Index} to update.", update.Index);
                continue;
            }

            avatar.Update(update);
            updated.Add(avatar);

            Log.LogTrace("Avatar @{Index} '{Name}' updated: '{Status}'.",
                avatar.Index, avatar.Name, update.ToString());
            AvatarUpdated?.Invoke(new AvatarEventArgs(avatar));
        }

        AvatarsUpdated?.Invoke(new AvatarsEventArgs(updated));
    }

    void RemoveAvatar(int index)
    {
        if (!EnsureInRoom(out Room? room))
            return;

        if (room.Avatars.TryRemove(index, out Avatar? avatar))
        {
            Log.LogDebug("Avatar @{Index} '{Name}' removed.", avatar.Index, avatar.Name);
            AvatarRemoved?.Invoke(new AvatarEventArgs(avatar));
        }
        else
        {
            Log.LogWarning("Failed to remove avatar @{Index}.", index);
        }
    }
    #endregion

    private void LeaveRoom()
    {
        if (IsInQueue || IsRingingDoorbell || IsLoadingRoom || IsInRoom)
        {
            ResetState();

            Log.LogInformation("Left room.");
            Left?.Invoke();
        }
        else
        {
            Log.LogDebug("LeaveRoom: not in a room.");
        }
    }

    private void ResetState()
    {
        Log.LogDebug("Resetting room state.");

        _gotHeightMap = _gotUsers = _gotObjects = _gotItems = false;

        IsInQueue =
        IsLoadingRoom =
        IsInRoom = false;
        QueuePosition = 0;

        RightsLevel = RightsLevel.None;
        IsOwner = false;

        _currentRoom = null;
        RaisePropertyChanged(nameof(Room));
    }

    private bool CheckPermission(ModerationPermissions? permissions)
    {
        if (!permissions.HasValue || !IsInRoom) return false;

        return permissions switch
        {
            ModerationPermissions.OwnerOnly => IsOwner,
            ModerationPermissions.GroupAdmins => RightsLevel >= RightsLevel.GroupAdmin,
            ModerationPermissions.RightsHolders or
            ModerationPermissions.GroupAdminsAndRightsHolders
                => RightsLevel > RightsLevel.None,
            ModerationPermissions.AllUsers => true,
            _ => false,
        };
    }

    private bool SetFurniHidden(ItemType type, int id, bool hide)
    {
        if (_currentRoom is null) return false;

        Furni furni;

        if (type == ItemType.Floor)
        {
            if (!_currentRoom.FloorItems.TryGetValue(id, out FloorItem? item) || item.IsHidden == hide)
                return false;

            item.IsHidden = hide;
            furni = _currentRoom.FloorItems.AddOrUpdate(
                id,
                item,
                (key, existing) =>
                {
                    existing.IsHidden = hide;
                    return existing;
                }
            );
        }
        else if (type == ItemType.Wall)
        {
            if (!_currentRoom.WallItems.TryGetValue(id, out WallItem? item) || item.IsHidden == hide)
                return false;

            item.IsHidden = hide;
            furni = _currentRoom.WallItems.AddOrUpdate(
                id,
                item,
                (key, existing) =>
                {
                    existing.IsHidden = hide;
                    return existing;
                }
            );
        }
        else
        {
            return false;
        }

        if (hide)
        {
            if (furni is FloorItem floorItem)
            {
                Interceptor.Send(new FloorItemRemovedMsg(floorItem));
            }
            else if (furni is WallItem)
            {
                Interceptor.Send(new WallItemRemovedMsg(furni.Id));
            }
        }
        else
        {
            if (furni is FloorItem floorItem)
                Interceptor.Send(new FloorItemAddedMsg(floorItem));
            else if (furni is WallItem wallItem)
                Interceptor.Send(new WallItemAddedMsg(wallItem));
        }

        FurniVisibilityToggled?.Invoke(new FurniEventArgs(furni));
        return true;
    }

    public bool ShowFurni(ItemType type, int id) => SetFurniHidden(type, id, false);
    public bool HideFurni(ItemType type, int id) => SetFurniHidden(type, id, true);
    public bool ShowFurni(IFurni furni) => SetFurniHidden(furni.Type, furni.Id, false);
    public bool HideFurni(IFurni furni) => SetFurniHidden(furni.Type, furni.Id, true);
}
