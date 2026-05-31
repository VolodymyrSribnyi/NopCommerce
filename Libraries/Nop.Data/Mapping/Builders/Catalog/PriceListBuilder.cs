using FluentMigrator.Builders.Create.Table;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Directory;
using Nop.Data.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Data.Mapping.Builders.Catalog
{
    public partial class PriceListBuilder : NopEntityBuilder<PriceList>
    {
        #region Methods
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(PriceList.Id)).AsInt32().NotNullable().PrimaryKey()
                .WithColumn(nameof(PriceList.Name)).AsString(400).NotNullable()
                .WithColumn(nameof(PriceList.CurrencyId)).AsInt32().NotNullable().ForeignKey(nameof(Currency), "Id");
        }
        #endregion
    }
}
