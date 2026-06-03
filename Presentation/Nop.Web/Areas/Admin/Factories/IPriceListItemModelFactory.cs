using Nop.Core.Domain.Catalog;
using Nop.Web.Areas.Admin.Models.Catalog;

namespace Nop.Web.Areas.Admin.Factories
{
    public partial interface IPriceListItemModelFactory
    {
        Task<PriceListItemSearchModel> PreparePriceListItemSearchModelAsync(PriceListItemSearchModel searchModel, PriceList priceList);
        Task<PriceListItemListModel> PreparePriceListItemListModelAsync(PriceListItemSearchModel searchModel);
    }
}
