using Nop.Web.Framework.Models;

namespace Nop.Web.Areas.Admin.Models.Catalog
{
    public partial record PriceListItemSearchModel : BaseSearchModel
    {
        public int PriceListId { get; set; }
    }
}
