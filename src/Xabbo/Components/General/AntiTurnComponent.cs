
using Xabbo.Extension;
using Xabbo.Core.Messages.Outgoing;
using Xabbo.Services.Abstractions;
using Xabbo.Configuration;
using Xabbo.Messages.Nitro;

namespace Xabbo.Components;

[Intercept]
public partial class AntiTurnComponent(
    IExtension extension,
    IConfigProvider<AppConfig> config
    ) : Component(extension)
{
    private int _lastSelectedUser = -1;
    private int _lastLookAtX, _lastLookAtY;
    private DateTime _lastSelection = DateTime.MinValue;

    private readonly IConfigProvider<AppConfig> _config = config;
    private AppConfig Config => _config.Value;

    private bool Enabled => Config.Movement.NoTurn;
    private bool TurnOnReselect => Config.Movement.TurnOnReselectUser;
    private double ReselectThreshold => Config.Movement.ReselectThreshold;

    [Intercept]
    private void OnLookTo(Intercept e, LookToMsg look)
    {
        if (!Enabled) return;

        bool block = true;


        if (block) e.Block();

        _lastLookAtX = look.X;
        _lastLookAtY = look.Y;
    }

    [InterceptOut(nameof(Out.User_Badges))]
    private void OnRequestWearingBadges(Intercept e)
    {
        int userId = e.Packet.Read<int>();

        if (Enabled && TurnOnReselect && (DateTime.Now - _lastSelection).TotalSeconds < ReselectThreshold)
        {
            if (userId == _lastSelectedUser)
                Ext.Send(new LookToMsg(_lastLookAtX, _lastLookAtY));
        }

        _lastSelection = DateTime.Now;
        _lastSelectedUser = userId;
    }
}
