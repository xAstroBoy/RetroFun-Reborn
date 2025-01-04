
using Xabbo.Extension;
using Xabbo.Messages.Nitro;

namespace Xabbo.Components;

public partial class AntiHcGiftNotificationComponent(IExtension extension) : Component(extension)
{
    [Reactive] public bool Enabled { get; set; } = true;

    [InterceptIn(nameof(In.Club_Gift_Notification))]
    protected void HandleClubGiftNotification(Intercept e)
    {
        if (Enabled)
            e.Block();
    }
}
