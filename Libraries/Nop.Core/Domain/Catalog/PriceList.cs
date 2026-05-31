using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Core.Domain.Catalog
{
    public partial class PriceList: BaseEntity
    {
        
        public string Name { get; set; }
        public int CurrencyId { get; set; }
        
    }
}
