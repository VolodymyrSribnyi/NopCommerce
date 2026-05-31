using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Data.Extensions;
using Nop.Core.Domain.Catalog;

namespace Nop.Data.Migrations.Custom
{
    [NopMigration("2026/03/04 13:36:00", "Catalog. Add PriceList and PriceListItem tables")]
    public class AddPriceListTableAndPriceListItemTable: AutoReversingMigration
    {
        public override void Up()
        {
            Create.TableFor<PriceList>();
            Create.TableFor<PriceListItem>();
        }
    }
}
