using FluentMigrator.Builders.Create.Table;
using Nop.Core.Domain.Catalog;
using Nop.Data.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Data.Mapping.Builders.Catalog
{
    public partial class PriceListItemBuilder: NopEntityBuilder<PriceListItem>
    {
        #region Methods
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(PriceListItem.Id)).AsInt32().NotNullable().PrimaryKey()
                .WithColumn(nameof(PriceListItem.PriceListId)).AsInt32().NotNullable().ForeignKey(nameof(PriceList), "Id").OnDelete(System.Data.Rule.Cascade)
                .WithColumn(nameof(PriceListItem.ProductId)).AsInt32().NotNullable().ForeignKey(nameof(Product), "Id").OnDelete(System.Data.Rule.None)
                .WithColumn(nameof(PriceListItem.Price)).AsDecimal(18,4).NotNullable();
        }
        #endregion
    }
}
