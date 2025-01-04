using Xabbo.Core;
using Xabbo.Core.Messages.Outgoing;

namespace Xabbo.Command.Modules;

[CommandModule]
public sealed class InfoCommands : CommandModule
{
    public InfoCommands() { }

    [Command("profile", "p")]
    public async Task ShowProfileAsync(CommandArgs args)
    {
        string name = string.Join(' ', args);

        UserProfile profile;
        if (name.StartsWith("id:"))
        {
            name = name[3..].Trim();

            if (int.TryParse(name, out int id))
            {
                profile = await Ext.RequestAsync(new GetProfileMsg(id, true), block: false, timeout: 3000);
            }
            else
            {
                ShowMessage($"Invalid ID: '{name}'.");
                return;
            }
        }
        else
        {
            profile = await Ext.RequestAsync(new GetProfileByNameMsg(name), block: false, timeout: 3000);
        }

        if (!profile.DisplayInClient)
        {
            ShowMessage($"{profile.Name}'s profile is not visible.");
        }
    }

    [Command("group", "g")]
    public async Task ShowGroupInfoAsync(CommandArgs args)
    {
        if (args.Length < 1)
            throw new InvalidArgsException();

        if (args.Length < 1 || !int.TryParse(args[0], out int id))
        {
            ShowMessage($"Invalid ID: '{args[0]}'.");
            return;
        }

        await Ext.RequestAsync(new GetGroupDataMsg(id, true), block: false, timeout: 3000);
    }
}
