﻿using System;
using System.Threading.Tasks;


using Xabbo.Interceptor;
using Xabbo.Interceptor.Tasks;
using Xabbo.Messages.Nitro;

namespace Xabbo.Core.Tasks;

public class SearchNavigatorTask : InterceptorTask<NavigatorSearchResults>
{
    private readonly string _category;
    private readonly string _filter;

    public SearchNavigatorTask(IInterceptor interceptor, string category, string filter)
        : base(interceptor)
    {
        _category = category;
        _filter = filter;
    }

    protected override void OnExecute() => Interceptor.Send(Out.Navigator_Search, _category, _filter);

    [InterceptIn(nameof(In.Navigator_Searches))]
    protected void OnNavigatorSearchResults(Intercept e)
    {
        try
        {
            var results = e.Packet.Read<NavigatorSearchResults>();
            if (results.Category == _category &&
                results.Filter == _filter)
            {
                if (SetResult(results))
                    e.Block();
            }
        }
        catch (Exception ex) { SetException(ex); }
    }
}
