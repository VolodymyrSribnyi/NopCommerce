using Nop.Core;
using Nop.Core.Domain.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Services.Catalog
{
    public partial interface IPriceListItemService
    {
        Task<IPagedList<PriceListItem>> GetPriceListItemsByPriceListIdAsync(int priceListId, int pageIndex, int pageSize);
        Task<PriceListItem> GetPriceListItemByIdAsync(int priceListItemId);
        Task InsertPriceListItemAsync(PriceListItem priceListItem);
        Task UpdatePriceListItemAsync(PriceListItem priceListItem);
        Task DeletePriceListItemAsync(PriceListItem priceListItem);
    }
}
