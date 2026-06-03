using Nop.Core;
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
        Task<IPagedList<PriceList>> GetAllPriceListAsync(string name, int pageIndex, int pageSize);
        Task DeletePriceListAsync(PriceList priceList);
    }
}
