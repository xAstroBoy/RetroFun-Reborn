﻿using ReactiveUI;

using Xabbo.Messages;

using Xabbo.Extension;
using Xabbo.Core;
using Xabbo.Core.Game;
using Xabbo.Core.Messages.Outgoing;
using Xabbo.Services.Abstractions;
using Xabbo.Configuration;
using Xabbo.Messages.Nitro;

namespace Xabbo.Components;

[Intercept]
public partial class AntiIdleComponent : Component
{
    private readonly IConfigProvider<AppConfig> _settingsProvider;
    private AppConfig Settings => _settingsProvider.Value;

    private readonly ProfileManager _profileManager;
    private readonly RoomManager _roomManager;

    private int _pingCount = 0;

    public AntiIdleComponent(IExtension extension,
        IConfigProvider<AppConfig> settingsProvider,
        ProfileManager profileManager,
        RoomManager roomManager)
        : base(extension)
    {
        _profileManager = profileManager;
        _roomManager = roomManager;

        _settingsProvider = settingsProvider;
        _settingsProvider.WhenAnyValue(
            x => x.Value.General.AntiIdle,
            x => x.Value.General.AntiIdleOut)
            .Subscribe(_ => OnActiveChanged());
    }

    protected override void OnConnected(ConnectedEventArgs e)
    {
        base.OnConnected(e);
    }

    protected override void OnDisconnected()
    {
        base.OnDisconnected();

        _pingCount = 0;
    }

    protected void OnActiveChanged() => SendAntiIdlePacket();

    [InterceptOut(nameof(Out.Latency))]
    protected void HandleLatencyPingRequest(Intercept e)
    {
        _pingCount = e.Packet.Read<int>();
        SendAntiIdlePacket();
    }

    private void SendAntiIdlePacket()
    {
        if (_pingCount <= 0) return;

        IMessage? antiIdleMsg = null;

        if (_profileManager.UserData is not null &&
            _roomManager.Room is not null &&
            _roomManager.Room.TryGetUserById(_profileManager.UserData.Id, out IUser? self))
        {
            if (Settings.General.AntiIdle)
            {
                if (self.Dance != 0 && Settings.General.AntiIdle)
                    antiIdleMsg = new WalkMsg(0, 0);
                else if (Settings.General.AntiIdle)
                    antiIdleMsg = new ActionMsg(AvatarAction.None);
            }
            else if (Settings.General.AntiIdleOut)
            {
                if (self.IsIdle)
                    antiIdleMsg = new ActionMsg(AvatarAction.Idle);
            }
        }
        else
        {
            if (Settings.General.AntiIdle)
                antiIdleMsg = new ActionMsg(AvatarAction.None);
        }

        if (antiIdleMsg is not null)
            Ext.Send(antiIdleMsg);
    }
}
