using System;
using System.Collections.Generic;


using Xabbo.Interceptor;
using Xabbo.Interceptor.Tasks;
using Xabbo.Core.Messages.Incoming;
using Xabbo.Messages.Nitro;

namespace Xabbo.Core.Tasks;

[Intercept]
public sealed partial class GetFriendsTask(IInterceptor interceptor) : InterceptorTask<List<Friend>>(interceptor)
{
    private int _totalExpected = -1, _currentIndex = 0;
    private readonly List<Friend> _friends = [];

    protected override void OnExecute() => Interceptor.Send(Out.Messenger_Init);

    [InterceptIn(nameof(In.Messenger_Friends))]
    void OnFriends(Intercept e)
    {
        try
        {
            int total = e.Packet.Read<int>();
            int current = e.Packet.Read<int>();

            if (current != _currentIndex) return;
            if (_totalExpected == -1) _totalExpected = total;
            else if (_totalExpected != total) return;
            _currentIndex++;

            e.Block();

            int n = e.Packet.Read<Length>();
            for (int i = 0; i < n; i++)
                _friends.Add(e.Packet.Read<Friend>());

            if (_currentIndex == total)
                SetResult(_friends);
        }
        catch (Exception ex) { SetException(ex); }
    }
}
