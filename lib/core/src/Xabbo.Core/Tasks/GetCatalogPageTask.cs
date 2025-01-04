using System;

using Xabbo.Interceptor;
using Xabbo.Interceptor.Tasks;

using Xabbo.Messages.Nitro;

namespace Xabbo.Core.Tasks;

/// <summary>
/// Gets a catalog page by its ID.
/// </summary>
[Intercept]
public sealed partial class GetCatalogPageTask : InterceptorTask<ICatalogPage>
{
    private readonly int _pageId;
    private readonly string _catalogType;

    protected override ClientType SupportedClients => ClientType.Nitro;

    public GetCatalogPageTask(IInterceptor interceptor, int pageId, string catalogType)
        : base(interceptor)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(pageId);

        _pageId = pageId;
        _catalogType = catalogType;
    }

    protected override void OnExecute() => Interceptor.Send(Out.Get_Catalog_Page, _pageId, -1, _catalogType);

    [InterceptIn(nameof(In.Catalog_Page))]
    private void HandleCatalogPage(Intercept e)
    {
        try
        {
            var catalogPage = e.Packet.Read<CatalogPage>();
            if (catalogPage.Id == _pageId &&
                catalogPage.CatalogType == _catalogType)
            {
                if (SetResult(catalogPage))
                    e.Block();
            }
        }
        catch (Exception ex) { SetException(ex); }
    }
}
