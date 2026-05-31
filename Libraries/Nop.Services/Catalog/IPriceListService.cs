using Nop.Core.Domain.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Services.Catalog
{
    public partial interface IPriceListService
    {
        Task InsertPriceListAsync(PriceList priceList);
        Task UpdatePriceListAsync(PriceList priceList);
        Task<PriceList> GetPriceListByIdAsync(int priceListId);
        Task<IList<PriceListItem>> GetPriceListItemsByPriceListIdAsync(int priceListId);
        Task<decimal?> GetPriceByProductAndPriceListAsync(int productId, int priceListId);
    }
}
