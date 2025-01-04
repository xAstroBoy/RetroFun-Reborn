using Xabbo.Configuration;
using Xabbo.Extension;
using Xabbo.Messages.Nitro;
using Xabbo.Services.Abstractions;

namespace Xabbo.Components;

public partial class AntiTypingComponent(IExtension extension, IConfigProvider<AppConfig> config) : Component(extension)
{
    private readonly IConfigProvider<AppConfig> _config = config;

    [InterceptOut(nameof(Out.Typing))]
    private void OnUserStartTyping(Intercept e)
    {
        if (_config.Value.General.AntiTyping) e.Block();
    }
}
