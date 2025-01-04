using Xabbo.Core;
using Xabbo.Web.Dto;

using MarketplaceItemStats = Xabbo.Web.Dto.MarketplaceItemStats;

namespace Xabbo.Services.Abstractions;

public interface IHabboApi
{
    Task<string> FetchPhotoDataAsync(string Url, CancellationToken cancellationToken = default);
}
