using Nop.Web.Framework.Models;

namespace Nop.Web.Areas.Admin.Models.Catalog
{
    public partial record PriceListItemModel : BaseNopEntityModel
    {
        public int PriceListId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } 
        public decimal Price { get; set; }
    }
}
