using System.Text.RegularExpressions;
using Xabbo.Core.GameData;
using Xabbo.Custom;
using Xabbo.Messages.Nitro;

namespace Xabbo.Command.Modules;

[CommandModule]
public sealed partial class EffectCommands : CommandModule
{
    [GeneratedRegex(@"^fx_(\d+)$")]
    private static partial Regex RegexEffect();

    private readonly IGameDataManager _gameDataManager;

    private bool _isReady, _isFaulted;

    private readonly Dictionary<int, EffectInfo> _effectNames = new();

    public EffectCommands(IGameDataManager gameDataManager)
    {
        _gameDataManager = gameDataManager;

        _gameDataManager.Loaded += OnGameDataLoaded;
        _gameDataManager.LoadFailed += OnGameDataLoadFailed;
    }

    protected override void OnInitialize()
    {
        IsAvailable = true;
    }

    private void OnGameDataLoadFailed(Exception ex)
    {
        _isFaulted = true;
    }

    private void OnGameDataLoaded()
    {
        foreach(var effect in EffectLibrary.GetInstance().EffectList)
        {
            _effectNames[effect.Id] = effect;
        }

        _isReady = true;
    }

    private List<(int Id, string Name)> FindEffects(string searchText)
    {
        searchText = searchText.ToLower();

        return _effectNames
            .Where(x => x.Value.Name.ToLower().Contains(searchText))
            .OrderBy(x => Math.Abs(x.Value.Name.Length - searchText.Length))
            .Select(x => (x.Key, x.Value.Name))
            .ToList();
    }

    private void EnableMatchingEffect(CommandArgs args, bool activate)
    {
        if (_isFaulted)
        {
            ShowMessage($"This command is unavailable (external texts failed to load)");
            return;
        }
        else if (!_isReady)
        {
            ShowMessage($"This command is currently unavailable (loading external texts)");
            return;
        }

        string searchText = string.Join(" ", args);

        if (string.IsNullOrWhiteSpace(searchText))
        {
            Ext.Send(Out.Chat, ":eff 0", 0);
            return;
        }

        var matches = FindEffects(searchText);
        if (matches.Count > 0)
        {
            if (activate)
                Ext.Send(Out.Chat, $":eff {matches[0].Id}", 0);
        }
        else
        {
            ShowMessage($"No effects matching '{searchText}' found");
        }
    }

    [Command("fxa")]
    public Task OnActivateEffect(CommandArgs args)
    {
        EnableMatchingEffect(args, true);
        return Task.CompletedTask;
    }

    [Command("fx")]
    public Task OnEnableEffect(CommandArgs args)
    {
        EnableMatchingEffect(args, false);
        return Task.CompletedTask;
    }

    [Command("dropfx")]
    public Task OnDropEffect(CommandArgs args)
    {
        Ext.Send(Out.Chat, ":eff 0", 0);
        return Task.CompletedTask;
    }
}
