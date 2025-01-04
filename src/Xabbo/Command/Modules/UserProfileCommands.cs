using Xabbo.Core.Messages.Outgoing;
using Xabbo.Messages.Nitro;


namespace Xabbo.Command.Modules;

[CommandModule]
public sealed class UserProfileCommands : CommandModule
{
    const short FieldMotto = 6;

    [Command("motto")]
    private async Task SetMotto(CommandArgs args)
    {
        string motto = string.Join(" ", args);
        Ext.Send(Out.User_Motto, motto);
    }
}
