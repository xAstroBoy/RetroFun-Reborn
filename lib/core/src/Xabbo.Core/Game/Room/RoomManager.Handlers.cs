﻿using System.Security.Cryptography;
using Microsoft.Extensions.Logging;


using Xabbo.Core.Events;
using Xabbo.Core.Messages.Incoming;
using Xabbo.Messages.Nitro;

namespace Xabbo.Core.Game;

partial class RoomManager
{
    #region - Room packet handlers -
    [InterceptIn(nameof(In.Get_Guest_Room_Result))]
    private void HandleGetGuestRoomResult(Intercept e)
    {
        using var scope = Log.MethodScope();

        var roomData = e.Packet.Read<RoomData>();

        if (!_roomDataCache.ContainsKey(roomData.Id))
        {
            Log.LogDebug(
                "Storing room data in cache. (id:{Id}, name:'{Name}', owner:'{Owner}')",
                roomData.Id, roomData.Name, roomData.OwnerName
            );
        }
        else
        {
            Log.LogDebug("Updating room data cache. (id:{Id}, name:'{Name}', owner:'{Owner}')",
                roomData.Id, roomData.Name, roomData.OwnerName
            );
        }

        _roomDataCache[roomData.Id] = roomData;

        UpdateRoomData(roomData);
    }


    [InterceptIn(nameof(In.Room_Queue_Status))]
    private void HandleRoomQueueStatus(Intercept e)
    {
        int roomId = e.Packet.Read<int>();
        if (roomId != _currentRoom?.Id) return;

        if (e.Packet.Read<Length>() == 2 &&
            e.Packet.Read<string>() == "visitors" &&
            e.Packet.Read<int>() == 2 &&
            e.Packet.Read<Length>() == 1 &&
            e.Packet.Read<string>() == "visitors")
        {
            bool enteredQueue = !IsInQueue;

            IsInQueue = true;
            QueuePosition = e.Packet.Read<int>() + 1;

            if (enteredQueue)
            {
                Log.LogDebug("Entered queue. (pos:{QueuePosition})", QueuePosition);
                EnteredQueue?.Invoke();
            }
            else
            {
                Log.LogDebug("Queue position updated. (pos:{QueuePosition})", QueuePosition);
                QueuePositionUpdated?.Invoke();
            }
        }
    }

    [InterceptIn(nameof(In.Room_Spectator))]
    private void HandleYouAreSpectator(Intercept e)
    {
        if (!IsLoadingRoom)
        {
            Log.LogDebug("Not loading room.");
            return;
        }

        if (_currentRoom is null)
        {
            Log.LogWarning("Current room is null.");
            return;
        }

        int roomId = e.Packet.Read<int>();
        if (roomId != _currentRoom.Id)
        {
            Log.LogError("Room ID mismatch. (expected: {Expected}, actual: {Actual})",
                _currentRoom.Id, roomId);
            return;
        }

        IsSpectating = true;
    }

    [InterceptIn(nameof(In.Room_Model_Name))]
    private void HandleRoomReady(Intercept e)
    {
        using var scope = Log.MethodScope();

        if (IsInQueue)
        {
            IsInQueue = false;
            QueuePosition = 0;
        }

        int roomId;;
        string room_model = "";
        room_model = e.Packet.Read<string>();
        roomId = e.Packet.Read<int>();

        EnteringRoom(roomId, room_model);
    }

    [InterceptIn(nameof(In.RoomProperty))]
    private void HandleRoomProperty(Intercept e)
    {
        if (!IsLoadingRoom)
        {
            Log.LogWarning("Not loading room.");
            return;
        }

        if (_currentRoom is null)
        {
            Log.LogWarning("Current room is null.");
            return;
        }

        string key, value;
        (key, value) = e.Packet.Read<string, string>();

        switch (key)
        {
            case "floor": _currentRoom.FloorPattern = value; break;
            case "wallpaper": _currentRoom.Wallpaper = value; break;
            case "landscape": _currentRoom.Landscape = value; break;
            default: Log.LogWarning("Unknown paint type: {Type}.", key); break;
        }

        if (IsInRoom)
        {
            // TODO PropertyUpdated
        }
    }

    [InterceptIn(nameof(In.Room_Rights_Owner))]
    private void HandleYouAreOwner(Intercept e)
    {
        IsOwner = true;
        UpdateRightsLevel(RightsLevel.Owner);
    }

    [InterceptIn(nameof(In.Room_Rights))]
    private void HandleYouAreController(Intercept e)
    {
        if (_currentRoom is null)
        {
            Log.LogTrace("Current room is null.");
            return;
        }

        RightsLevel rightsLevel;
        rightsLevel = RightsLevel.Standard;
        IsOwner = RightsLevel == RightsLevel.Owner;
        UpdateRightsLevel(rightsLevel);
    }


    //[InterceptIn(nameof(In.RoomEntryTile))]
    //private void HandleRoomEntryTile(Intercept e)
    //{
    //    using var scope = Log.MethodScope();

    //    if (!IsLoadingRoom)
    //    {
    //        Log.LogWarning("Not loading room.");
    //        return;
    //    }

    //    if (_currentRoom is null)
    //    {
    //        Log.LogWarning("Current room is null.");
    //        return;
    //    }

    //    int x = e.Packet.Read<int>();
    //    int y = e.Packet.Read<int>();
    //    int dir = e.Packet.Read<int>();

    //    _currentRoom.Entry = new Tile(x, y, 0);
    //    _currentRoom.EntryDirection = dir;

    //    Log.LogDebug("Received room entry tile. (x:{X}, y:{Y}, dir: {Dir})", x, y, dir);
    //}

    [InterceptIn( nameof(In.Room_Height_Map))]
    private void HandleHeightMap(Intercept e)
    {
        using var scope = Log.MethodScope();

        if (!IsLoadingRoom)
        {
            Log.LogWarning("Not loading room.");
            return;
        }

        if (_currentRoom is null)
        {
            Log.LogWarning("Current room is null.");
            return;
        }

        Heightmap heightmap = e.Packet.Read<Heightmap>();
        _currentRoom.Heightmap = heightmap;

        Log.LogDebug("Received stacking heightmap. (size:{Width}x{Length})", heightmap.Size.X, heightmap.Size.Y);

        // convert HeightMap to FloorPlan
        Log.LogDebug("Converting heightmap to floor plan.");
        _currentRoom.FloorPlan = FloorPlan.ConvertHeightmapToFloorPlan(heightmap);
    }

    [InterceptIn(nameof(In.Room_Height_Map_Update))]
    private void HandleHeightMapUpdate(Intercept e)
    {
        if (!IsInRoom) return;

        if (_currentRoom is null)
        {
            Log.LogDebug("Current room is null.");
            return;
        }

        if (_currentRoom.Heightmap is null)
        {
            Log.LogWarning("Heightmap is null.");
            return;
        }

        int n = e.Packet.Read<byte>();
        for (int i = 0; i < n; i++)
        {
            int x = e.Packet.Read<byte>();
            int y = e.Packet.Read<byte>();
            _currentRoom.Heightmap[x, y].Update(e.Packet.Read<short>());
        }

        Log.LogTrace("Received stacking heightmap diff. ({Count} change(s))", n);
    }

    [InterceptIn(nameof(In.Room_Visualization_Settings))]
    private void HandleRoomVisualizationSettings(Intercept e)
    {
        if (_currentRoom is null) return;

        _currentRoom.FloorThickness = (Thickness)e.Packet.Read<int>();
        _currentRoom.WallThickness = (Thickness)e.Packet.Read<int>();
        _currentRoom.HideWalls = e.Packet.Read<bool>();

    }

    [InterceptIn(nameof(In.Room_Settings_Chat))]
    private void HandleRoomChatSettings(Intercept e)
    {
        UpdateChatSettings(e.Packet.Read<ChatSettings>());
    }

    [InterceptIn(nameof(In.Room_Info))]
    private void HandleRoomEntryInfo(Intercept e)
    {
        using var scope = Log.MethodScope();

        if (!IsLoadingRoom)
        {
            Log.LogWarning("Not loading room.");
            return;
        }

        int roomId = e.Packet.Read<int>();
        IsOwner = e.Packet.Read<bool>();
        EnterRoom();
    }

    [InterceptIn(nameof(In.Hotel_View))]
    private void HandleCloseConnection(Intercept e) => LeaveRoom();

    [InterceptIn(nameof(In.Room_Enter_Error))]
    private void HandleErrorReport(Intercept e)
    {
        if (!IsInRoom) return;

        GenericErrorCode errorCode = (GenericErrorCode)e.Packet.Read<int>();
        Log.LogTrace("Received generic error code {Code}.", errorCode);

        if (errorCode == GenericErrorCode.Kicked)
        {
            Log.LogDebug("Kicked from room.");
            Kicked?.Invoke();
        }
    }
    #endregion

    #region - Floor item handlers -
    [Intercept]
    void HandleFloorItems(FloorItemsMsg items)
    {
        using (Log.MethodScope())
            LoadFloorItems(items);
    }

    [Intercept]
    void HandleFloorItemAdded(FloorItemAddedMsg added)
    {
        using var scope = Log.MethodScope();
        AddFloorItem(added.Item);
    }

    [Intercept]
    void HandleFloorItemRemoved(FloorItemRemovedMsg removed)
    {
        using (Log.MethodScope())
            RemoveFloorItem(removed.Id);
    }

    [Intercept]
    void HandleFloorItemUpdated(FloorItemUpdatedMsg updated)
    {
        using (Log.MethodScope())
            UpdateFloorItem(updated.Item);
    }

    [Intercept]
    void HandleFloorItemDataUpdated(FloorItemDataUpdatedMsg update)
    {
        using (Log.MethodScope())
            UpdateFloorItemData([(update.Id, update.Data)]);
    }

    [Intercept]
    void HandleFloorItemsDataUpdated(FloorItemsDataUpdatedMsg msg)
    {
        using (Log.MethodScope())
            UpdateFloorItemData(msg.Updates);
    }

    [Intercept]
    void HandleDiceValue(DiceValueMsg dice)
    {
        using (Log.MethodScope())
            UpdateDiceValue(dice.Id, dice.Value);
    }

    [Intercept]
    void HandleSlideObjectBundle(SlideObjectBundleMsg msg)
    {
        using var scope = Log.MethodScope();

        if (!EnsureInRoom(out Room? room))
            return;

        var update = msg.Bundle;
        foreach (SlideObject objectUpdate in update.SlideObjects)
        {
            if (room.FloorItems.TryGetValue(objectUpdate.Id, out FloorItem? item))
            {
                Tile previousTile = item.Location;
                item.Location = new Tile(update.To, objectUpdate.ToZ);
                OnFloorItemSlide(item, previousTile, update.RollerId);
            }
            else
            {
                Log.LogWarning("Failed to find floor item {Id} to update.", objectUpdate.Id);
            }
        }

        if (update.Avatar is not null &&
            update.AvatarSlideType is AvatarSlideType.WalkingAvatar or AvatarSlideType.StandingAvatar)
        {
            if (room.Avatars.TryGetValue(update.Avatar.Index, out Avatar? avatar))
            {
                var previousTile = avatar.Location;
                avatar.Location = new Tile(update.To, update.Avatar.ToZ);

                Log.LogTrace(
                    "Avatar slide. ({AvatarName} [{AvatarId}:{AvatarIndex}], {From} -> {To})",
                    avatar.Name, avatar.Id, avatar.Index, previousTile, avatar.Location
                );
                AvatarSlide?.Invoke(new AvatarSlideEventArgs(avatar, previousTile));
            }
            else
            {
                Log.LogWarning("Failed to find avatar with index {Index} to update.", update.Avatar.Index);
            }
        }
    }
    #endregion

    #region - Wall item handlers -
    [Intercept]
    void HandleItems(WallItemsMsg items)
    {
        using (Log.MethodScope())
            LoadWallItems(items);
    }

    [Intercept]
    void HandleItemAdd(WallItemAddedMsg added)
    {
        using (Log.MethodScope())
            AddWallItem(added.Item);
    }

    [Intercept]
    void HandleItemUpdate(WallItemUpdatedMsg updated)
    {
        using (Log.MethodScope())
            UpdateWallItem(updated.Item);
    }

    [Intercept]
    void HandleItemRemove(WallItemRemovedMsg removed)
    {
        using (Log.MethodScope())
            RemoveWallItem(removed.Id);
    }
    #endregion

    #region - Avatar handlers -
    [Intercept]
    void HandleAvatarsAdded(AvatarsAddedMsg avatars)
    {
        using (Log.MethodScope())
            AddAvatars(avatars);
    }

    [Intercept]
    void HandleAvatarRemoved(AvatarRemovedMsg removed)
    {
        using (Log.MethodScope())
            RemoveAvatar(removed.Index);
    }

    [Intercept]
    void HandlerUserUpdate(AvatarStatusMsg updates)
    {
        using (Log.MethodScope())
            UpdateAvatars(updates);
    }

    [Intercept]
    void HandleWiredMovements(WiredMovementsMsg movements)
    {
        if (!IsInRoom) return;

        if (_currentRoom is null)
        {
            Log.LogDebug("Current room is null.");
            return;
        }

        foreach (var movement in movements)
        {
            switch (movement)
            {
                case AvatarWiredMovement m:
                    {
                        if (_currentRoom.Avatars.TryGetValue(m.AvatarIndex, out Avatar? avatar))
                        {
                            Tile previousLocation = avatar.Location;
                            int previousDirection = avatar.Direction;
                            int previousHeadDirection = avatar.HeadDirection;
                            avatar.Location = m.Destination;
                            avatar.Direction = m.BodyDirection;
                            avatar.HeadDirection = m.HeadDirection;
                            AvatarWiredMovement?.Invoke(new AvatarWiredMovementEventArgs(
                                avatar, previousLocation, previousDirection, previousHeadDirection, m));
                        }
                    }
                    break;
                case AvatarDirectionWiredMovement m:
                    {
                        if (_currentRoom.Avatars.TryGetValue(m.AvatarIndex, out Avatar? avatar))
                        {
                            int previousDirection = avatar.Direction;
                            int previousHeadDirection = avatar.HeadDirection;
                            avatar.Direction = m.BodyDirection;
                            avatar.HeadDirection = m.HeadDirection;
                            AvatarDirectionWiredMovement?.Invoke(new AvatarDirectionWiredMovementEventArgs(
                                avatar, previousDirection, previousHeadDirection, m));
                        }
                    }
                    break;
                case FloorItemWiredMovement m:
                    if (_currentRoom.FloorItems.TryGetValue(m.ItemId, out FloorItem? item))
                    {
                        Tile previousLocation = item.Location;
                        int previousDirection = item.Direction;
                        item.Location = m.Destination;
                        item.Direction = m.Rotation;
                        FloorItemWiredMovement?.Invoke(new FloorItemWiredMovementEventArgs(
                            item, previousLocation, previousDirection, m));
                    }
                    break;
                case WallItemWiredMovement m:
                    if (_currentRoom.WallItems.TryGetValue(m.ItemId, out WallItem? wallItem))
                    {
                        WallLocation previousLocation = wallItem.Location;
                        wallItem.Location = m.Destination;
                        WallItemWiredMovement?.Invoke(new WallItemWiredMovementEventArgs(
                            wallItem, previousLocation, m));
                    }
                    break;
            }
        }

        Log.LogTrace("Received {Count} wired movements.", movements.Count);
        WiredMovements?.Invoke(new WiredMovementsEventArgs(movements));
    }

    // TODO: check
    [InterceptIn(nameof(In.UserChange))]
    void HandleUserChange(Intercept e)
    {
        if (!IsInRoom) return;

        if (_currentRoom is null)
        {
            Log.LogDebug("Current room is null.");
            return;
        }

        int index = e.Packet.Read<int>();
        if (_currentRoom.Avatars.TryGetValue(index, out Avatar? avatar))
        {
            string previousFigure = avatar.Figure;
            Gender previousGender;
            string previousMotto = avatar.Motto;
            int previousAchievementScore = 0;

            string updatedFigure = e.Packet.Read<string>();
            Gender updatedGender = H.ToGender(e.Packet.Read<string>());
            string updatedMotto = e.Packet.Read<string>();
            int updatedAchievementScore = e.Packet.Read<int>();

            avatar.Figure = updatedFigure;
            avatar.Motto = updatedMotto;

            if (avatar is User user)
            {
                previousGender = user.Gender;
                user.Gender = updatedGender;
                previousAchievementScore = user.AchievementScore;
                user.AchievementScore = updatedAchievementScore;
            }
            else if (avatar is Bot bot)
            {
                previousGender = bot.Gender;
                bot.Gender = updatedGender;
                previousAchievementScore = updatedAchievementScore;
            }
            else
            {
                previousGender = updatedGender;
                previousAchievementScore = updatedAchievementScore;
            }

            Log.LogTrace(
                "Avatar data updated. ({Name} [{Id}:{Index}])",
                avatar.Name, avatar.Id, avatar.Index
            );
            AvatarChanged?.Invoke(new AvatarChangedEventArgs(
                avatar, previousFigure, previousGender,
                previousMotto, previousAchievementScore
            ));
        }
        else
        {
            Log.LogWarning("Failed to find avatar with index {Index} to update.", index);
        }
    }

    [InterceptIn(nameof(In.UserNameChanged))]
    void HandleUserNameChanged(Intercept e)
    {
        if (!IsInRoom) return;

        if (_currentRoom is null)
        {
            Log.LogDebug("Current room is null.");
            return;
        }

        int id = e.Packet.Read<int>();
        int index = e.Packet.Read<int>();
        string newName = e.Packet.Read<string>();

        if (_currentRoom.Avatars.TryGetValue(index, out Avatar? avatar))
        {
            string previousName = avatar.Name;
            avatar.Name = newName;

            Log.LogTrace(
                "Avatar name changed. ({PreviousName} -> {AvatarName} [{AvatarId}:{AvatarIndex}])",
                previousName, avatar.Name, avatar.Id, avatar.Index
            );
            AvatarNameChanged?.Invoke(new AvatarNameChangedEventArgs(avatar, previousName));
        }
        else
        {
            Log.LogWarning("Failed to find avatar with index {index} to update.", index);
        }
    }

    [Intercept]
    void HandleAvatarIdle(AvatarIdleMsg msg)
    {
        if (!IsInRoom) return;

        if (_currentRoom is null)
        {
            Log.LogDebug("Current room is null.");
            return;
        }

        if (_currentRoom.Avatars.TryGetValue(msg.Index, out Avatar? avatar))
        {
            bool wasIdle = avatar.IsIdle;
            avatar.IsIdle = msg.Idle;

            Log.LogTrace(
                "Avatar idle. ({AvatarName} [{AvatarId}:{AvatarIndex}], {WasIdle} -> {IsIdle})",
                avatar.Name, avatar.Id, avatar.Index, wasIdle, avatar.IsIdle
            );
            AvatarIdle?.Invoke(new AvatarIdleEventArgs(avatar, wasIdle));
        }
        else
        {
            Log.LogWarning("Failed to find avatar with index {Index} to update.", msg.Index);
        }
    }

    [Intercept]
    void HandleDance(AvatarDanceMsg msg)
    {
        if (!IsInRoom) return;

        if (_currentRoom is null)
        {
            Log.LogDebug("Current room is null.");
            return;
        }

        if (_currentRoom.Avatars.TryGetValue(msg.Index, out Avatar? avatar))
        {
            AvatarDance previousDance = avatar.Dance;
            avatar.Dance = msg.Dance;

            Log.LogTrace(
                "Avatar dance. ({AvatarName} [{AvatarId}:{AvatarIndex}], {PreviousDance} -> {Dance})",
                avatar.Name, avatar.Id, avatar.Index, previousDance, avatar.Dance
            );
            AvatarDance?.Invoke(new AvatarDanceEventArgs(avatar, previousDance));
        }
        else
        {
            Log.LogWarning("Failed to find avatar with index {Index} to update.", msg.Index);
            return;
        }
    }

    [Intercept]
    void HandleExpression(AvatarActionMsg msg)
    {
        if (!IsInRoom) return;

        if (_currentRoom is null)
        {
            Log.LogDebug("Current room is null.");
            return;
        }

        if (_currentRoom.Avatars.TryGetValue(msg.Index, out Avatar? avatar))
        {
            Log.LogTrace(
                "Avatar action. ({AvatarName} [{AvatarId}:{AvatarIndex}], action:{Action})",
                avatar.Name, avatar.Id, avatar.Index, msg.Action
            );
            AvatarAction?.Invoke(new AvatarActionEventArgs(avatar, msg.Action));
        }
        else
        {
            Log.LogError("Failed to find avatar with index {Index} to update.", msg.Index);
        }
    }

    [Intercept]
    void HandleCarryObject(AvatarHandItemMsg msg)
    {
        if (!IsInRoom) return;

        if (_currentRoom is null)
        {
            Log.LogDebug("Current room is null.");
            return;
        }

        if (_currentRoom.Avatars.TryGetValue(msg.Index, out Avatar? avatar))
        {
            int previousItem = avatar.HandItem;
            avatar.HandItem = msg.Item;

            Log.LogTrace(
                "Avatar hand item. ({AvatarName} [{AvatarId}:{AvatarIndex}], {PreviousItemId} -> {ItemId})",
                avatar.Name, avatar.Id, avatar.Index, previousItem, avatar.HandItem
            );
            AvatarHandItem?.Invoke(new AvatarHandItemEventArgs(avatar, previousItem));
        }
        else
        {
            Log.LogWarning("Failed to find avatar with index {Index} to update.", msg.Index);
        }
    }

    [Intercept]
    void HandleAvatarEffect(AvatarEffectMsg msg)
    {
        if (!IsInRoom) return;

        if (_currentRoom is null)
        {
            Log.LogDebug("Current room is null.");
            return;
        }

        if (_currentRoom.Avatars.TryGetValue(msg.Index, out Avatar? avatar))
        {
            int previousEffect = avatar.Effect;
            avatar.Effect = msg.Effect;

            Log.LogTrace(
                "Avatar effect. ({AvatarName} [{AvatarId}:{AvatarIndex}], {PreviousEffect} -> {Effect})",
                avatar.Name, avatar.Id, avatar.Index, previousEffect, avatar.Effect
            );
            AvatarEffect?.Invoke(new AvatarEffectEventArgs(avatar, previousEffect));
        }
        else
        {
            Log.LogWarning("Failed to find avatar with index {Index} to update.", msg.Index);
        }
    }

    [Intercept]
    void HandleUserTyping(AvatarTypingMsg msg)
    {
        if (!IsInRoom) return;

        if (_currentRoom is null)
        {
            Log.LogDebug("Current room is null.");
            return;
        }

        if (_currentRoom.Avatars.TryGetValue(msg.Index, out Avatar? avatar))
        {
            bool wasTyping = avatar.IsTyping;
            avatar.IsTyping = msg.Typing;

            Log.LogTrace(
                "Avatar typing. ({AvatarName} [{AvatarId}:{AvatarIndex}], {WasTyping} -> {IsTyping})",
                avatar.Name, avatar.Id, avatar.Index, wasTyping, avatar.IsTyping
            );
            AvatarTyping?.Invoke(new AvatarTypingEventArgs(avatar, wasTyping));
        }
        else
        {
            Log.LogWarning("Failed to find avatar with index {Index} to update.", msg.Index);
        }
    }

    [Intercept]
    void HandleChat(Intercept<AvatarChatMsg> e)
    {
        if (!IsInRoom) return;

        if (_currentRoom is null)
        {
            Log.LogDebug("Current room is null.");
            return;
        }

        var chat = e.Msg;

        if (!_currentRoom.Avatars.TryGetValue(chat.AvatarIndex, out Avatar? avatar))
        {
            Log.LogWarning("Failed to find avatar with index {Index}.", chat.AvatarIndex);
            return;
        }

        Log.LogTrace(
            "{Type}({Bubble}) {Avatar} {Message}",
            chat.Type, chat.BubbleStyle, avatar, chat.Message
        );

        AvatarChatEventArgs chatEventArgs = new(avatar, chat.Type, chat.Message, chat.BubbleStyle);
        AvatarChat?.Invoke(chatEventArgs);

        if (chatEventArgs.IsBlocked || avatar.IsHidden) e.Block();
    }
    #endregion

}
