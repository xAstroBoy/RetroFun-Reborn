using System;
using System.Collections.Generic;


using Xabbo.Interceptor;
using Xabbo.Interceptor.Tasks;
using Xabbo.Messages.Nitro;

namespace Xabbo.Core.Tasks;

public sealed partial class GetRightsListTask(IInterceptor interceptor, int roomId)
    : InterceptorTask<List<(int Id, string Name)>>(interceptor)
{
    private readonly int _roomId = roomId;

    protected override ClientType SupportedClients => ClientType.Nitro;

    protected override void OnExecute() => Interceptor.Send(Out.Room_Rights_List, _roomId);

    [InterceptIn(nameof(In.Room_Rights_List))]
    private void HandleFlatControllers(Intercept e)
    {
        try
        {
            int roomId = e.Packet.Read<int>();
            if (roomId == _roomId)
            {
                var list = new List<(int, string)>();
                int n = e.Packet.Read<Length>();
                for (int i = 0; i < n; i++)
                    list.Add((e.Packet.Read<int>(), e.Packet.Read<string>()));

                if (SetResult(list))
                    e.Block();
            }
        }
        catch (Exception ex) { SetException(ex); }
    }
}
