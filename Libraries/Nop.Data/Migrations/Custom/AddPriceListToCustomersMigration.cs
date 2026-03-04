using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentMigrator;

namespace Nop.Data.Migrations.Custom;

[NopMigration("2026-03-04 12:00:00:0000000", "Add PriceListId to Customer and CustomerRole tables")]
public class AddPriceListToCustomersMigration : AutoReversingMigration
{
    public override void Up()
    {
        Alter.Table("Customer").AddColumn("PriceListId").AsInt32().Nullable();
        Alter.Table("CustomerRole").AddColumn("PriceListId").AsInt32().Nullable();
    }
}
