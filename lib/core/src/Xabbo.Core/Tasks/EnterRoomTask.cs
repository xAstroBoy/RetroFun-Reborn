using System;
using Xabbo.Interceptor;
using Xabbo.Interceptor.Tasks;

using Xabbo.Messages.Nitro;

namespace Xabbo.Core.Tasks;

/// <summary>
/// Attempts to enter a specified room by its ID, and optionally a password.
/// On Shockwave, this task can only enter another room if the user is currently in a room.
/// </summary>
/// <param name="interceptor"></param>
/// <param name="roomId"></param>
/// <param name="password"></param>
/// <remarks>
/// On Flash, this task does the following:
/// <list type="bullet">
/// <item>
/// Requests the specified room data by its ID with <see cref="Out.GetGuestRoom"/>.
/// </item>
/// <item>
/// Modifies the <see cref="RoomData.IsEntering"/>, <see cref="RoomData.Forward"/>, and
/// <see cref="RoomInfo.Access"/> fields in the <see cref="In.GetGuestRoomResult"/> response,
/// causing the client to attempt to enter the room.
/// </item>
/// <item>
/// Replaces the password in the <see cref="Out.OpenFlatConnection"/> packet if required.
/// </item>
/// <item>
/// Handles <see cref="In.RoomEntryInfo"/> and <see cref="In.CantConnect"/> to determine whether
/// room entry was successful.
/// </item>
/// </list>
/// <para/>
/// On Shockwave, this task does the following:
/// <list type="bullet">
/// <item>
/// Attempts to initiate room entry by room ID and password with <see cref="ShockOut.TRYFLAT"/>,
/// causing the client to send <see cref="ShockOut.GOTOFLAT"/>.
/// </item>
/// <item>
/// Replaces the room ID in <see cref="ShockOut.GOTOFLAT"/> with the target room ID. This is
/// required because the packet will have the ID of the room the user is currently in.
/// </item>
/// <item>
/// Handles <see cref="ShockIn.ROOM_READY"/>, <see cref="ShockIn.CANTCONNECT"/>, and
/// <see cref="ShockIn.ERROR"/> to determine whether room entry was successful.
/// </item>
/// </list>
/// </remarks>
[Intercept]
public sealed partial class EnterRoomTask(
    IInterceptor interceptor,
    int roomId, string password = ""
)
    : InterceptorTask<EnterRoomTask.Result>(interceptor)
{
    public enum Result { Unknown, Fail, InvalidPassword, Closed, Full, Banned, Success }
    enum Status
    {
        None,
        RequestingRoomData,
        AwaitingFlatOpc,
        ReplacingRoomId,
        AwaitingRoomEntry,
        Complete
    }

    private readonly int _roomId = roomId;
    private readonly string? _password = password;

    private Status _state = Status.None;

    protected override void OnExecute()
    {
        _state = Status.RequestingRoomData;
        Send(Out.Room_Enter, _roomId, _password, 1);
    }

    [InterceptIn(nameof(In.Room_Enter_Error))]
    void HandleCantConnect(Intercept e)
    {
        if (_state is Status.Complete) return;

        RoomEnterError error = (RoomEnterError)e.Packet.Read<int>();

        _state = Status.Complete;
        SetResult(error switch
        {
            RoomEnterError.Full => Result.Full,
            RoomEnterError.Banned => Result.Banned,
            _ => Result.Unknown
        });
    }

    #region Flash

    [InterceptIn(nameof(In.Get_Guest_Room_Result))]
    void HandleGetGuestRoomResult(Intercept e)
    {
        if (_state is not Status.RequestingRoomData) return;

        try
        {
            RoomData roomData = e.Packet.Read<RoomData>();
            if (roomData.Id == _roomId)
            {
                roomData.IsEntering = false;
                roomData.Forward = true;
                roomData.Access = RoomAccess.Open;

                e.Packet.Clear();
                e.Packet.Write(roomData);

                _state = Status.AwaitingFlatOpc;
            }
        }
        catch (Exception ex)
        {
            _state = Status.Complete;
            SetException(ex);
        }
    }

    [InterceptOut(nameof(Out.Room_Enter))]
    void HandleOpenFlatConnection(Intercept e)
    {
        if (_state is not Status.AwaitingFlatOpc) return;

        try
        {
            int roomId = e.Packet.Read<int>();

            if (roomId == _roomId)
            {
                e.Packet.Replace(_password ?? string.Empty);
                _state = Status.AwaitingRoomEntry;
            }
        }
        catch (Exception ex)
        {
            _state = Status.Complete;
            SetException(ex);
        }
    }

    [InterceptIn(nameof(In.Room_Enter))]
    void HandleRoomEntryInfo(Intercept e)
    {
        if (_state is not Status.AwaitingRoomEntry) return;

        _state = Status.Complete;
        SetResult(Result.Success);
    }

    #endregion

}
