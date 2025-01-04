using ReactiveUI;

namespace Xabbo.Configuration;

public sealed class TimingConfigs : ReactiveObject
{
    [Reactive] public ModernTimingConfig Modern { get; set; } = new();

    public TimingConfigBase GetTiming(ClientType client) => Modern;
    public TimingConfigBase GetTiming(Session session) => GetTiming(session.Client.Type);
}
