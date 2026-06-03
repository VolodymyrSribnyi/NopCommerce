using Nop.Core.Domain.Catalog;
using Nop.Web.Areas.Admin.Models.Catalog;

namespace Nop.Web.Areas.Admin.Factories
{
    public partial interface IPriceListModelFactory
    {
        Task<PriceListSearchModel> PreparePriceListSearchModelAsync(PriceListSearchModel priceListSearchModel);
        Task<PriceListListModel> PreparePriceListListModelAsync(PriceListSearchModel priceListSearchModel);
        
    }
}
