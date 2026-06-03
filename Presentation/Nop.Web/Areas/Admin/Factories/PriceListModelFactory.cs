using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Nop.Core.Domain.Catalog;
using Nop.Services.Catalog;
using Nop.Web.Areas.Admin.Models.Catalog;
using Nop.Web.Framework.Models.Extensions;
namespace Nop.Web.Areas.Admin.Factories
{
    public partial class PriceListModelFactory: IPriceListModelFactory
    {
        private readonly IPriceListService _priceListService;
        
        public PriceListModelFactory(IPriceListService priceListService)
        {
            _priceListService = priceListService;
            
        }
        public async Task<PriceListSearchModel> PreparePriceListSearchModelAsync(PriceListSearchModel priceListSearchModel)
        {
            if (priceListSearchModel==null)
            {
                priceListSearchModel = new PriceListSearchModel();
            }
            priceListSearchModel.SetGridPageSize();
            return priceListSearchModel;
        }
        public async Task<PriceListListModel>PreparePriceListListModelAsync(PriceListSearchModel priceListSearchModel)
        {

                PriceListListModel priceListListModel = new PriceListListModel();
                IList<PriceListModel> tempList = new List<PriceListModel>();
                var priceLists = await _priceListService.GetAllPriceListAsync(priceListSearchModel.PriceListName, priceListSearchModel.Page-1, priceListSearchModel.PageSize);
                priceListListModel.PrepareToGrid(priceListSearchModel, priceLists, () =>
                {
                    return priceLists.Select(priceList => new PriceListModel
                {
                    Id = priceList.Id,
                    Name = priceList.Name,
                    CurrencyId = priceList.CurrencyId
                });
                });

                return priceListListModel;



        }
        

    }
}
