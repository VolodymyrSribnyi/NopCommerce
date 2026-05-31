using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Core.Domain.Catalog
{
    public partial class PriceListItem: BaseEntity
    {
        
        public int PriceListId { get; set; }
        public int ProductId { get; set; }
        public decimal Price { get; set; }
    }
}
