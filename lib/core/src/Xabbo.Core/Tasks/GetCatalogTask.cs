using System;


using Xabbo.Interceptor;
using Xabbo.Interceptor.Tasks;
using Xabbo.Messages.Nitro;

namespace Xabbo.Core.Tasks;

[Intercept]
public sealed partial class GetCatalogTask(IInterceptor interceptor, string type) : InterceptorTask<ICatalog>(interceptor)
{
    private readonly string _type = type;

    protected override ClientType SupportedClients => ClientType.Nitro;

    protected override void OnExecute() => Interceptor.Send(Out.Get_Catalog_Index, _type);

    [InterceptIn(nameof(In.Catalog_Page))]
    void HandleCatalogIndex(Intercept e)
    {
        try
        {
            var catalog = e.Packet.Read<Catalog>();
            if (catalog.Type == _type)
            {
                if (SetResult(catalog))
                    e.Block();
            }
        }
        catch (Exception ex) { SetException(ex); }
    }
}
