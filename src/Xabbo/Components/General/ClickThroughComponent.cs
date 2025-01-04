using Microsoft.Extensions.Configuration;
using ReactiveUI;


using Xabbo.Extension;
using Xabbo.Messages.Nitro;

namespace Xabbo.Components;

public partial class ClickThroughComponent : Component
{
    [Reactive] public bool Enabled { get; set; }

    public ClickThroughComponent(IExtension extension)
        : base(extension)
    {
        this.ObservableForProperty(x => x.Enabled)
            .Subscribe(x => OnIsActiveChanged(x.Value));
    }

    protected override void OnConnected(ConnectedEventArgs e)
    {
        base.OnConnected(e);

        IsAvailable = true;
    }

    


    // [RequiredIn(nameof(In.GameYouArePlayer))]
    protected void OnIsActiveChanged(bool isActive)
    {
        Ext.Send(In.Playing_Game, isActive);
    }

    [InterceptIn(nameof(In.Room_Enter))]
    // [RequiredIn(nameof(In.GameYouArePlayer))]
    private void OnEnterRoom(Intercept e)
    {
        if (Enabled)
            Ext.Send(In.Playing_Game, true);
    }
}
