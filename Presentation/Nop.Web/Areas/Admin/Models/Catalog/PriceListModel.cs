using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;
namespace Nop.Web.Areas.Admin.Models.Catalog
{
    public partial record PriceListModel: BaseNopEntityModel
    {
        [NopResourceDisplayName("Admin.Catalog.PriceLists.Fields.Name")]
        public string Name {  get; set; }=string.Empty;
        [NopResourceDisplayName("Admin.Catalog.PriceLists.Fields.Currency")]
        public int CurrencyId { get; set; } = 0;
        public string CurrencyName { get; set; } = string.Empty;
        public PriceListItemSearchModel PriceListItemSearchModel { get; set; } = new();
    }
}
