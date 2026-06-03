using Nop.Web.Framework.Models;

namespace Nop.Web.Areas.Admin.Models.Catalog
{
    public partial record PriceListSearchModel: BaseSearchModel
    {
        public string PriceListName {  get; set; }
    }
}
