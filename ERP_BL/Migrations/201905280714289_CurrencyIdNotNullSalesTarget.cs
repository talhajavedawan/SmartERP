namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CurrencyIdNotNullSalesTarget : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SalesTargets", "CurrencyId", "dbo.Currencies");
            DropIndex("dbo.SalesTargets", new[] { "CurrencyId" });
            AlterColumn("dbo.SalesTargets", "CurrencyId", c => c.Int(nullable: false));
            CreateIndex("dbo.SalesTargets", "CurrencyId");
            AddForeignKey("dbo.SalesTargets", "CurrencyId", "dbo.Currencies", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SalesTargets", "CurrencyId", "dbo.Currencies");
            DropIndex("dbo.SalesTargets", new[] { "CurrencyId" });
            AlterColumn("dbo.SalesTargets", "CurrencyId", c => c.Int());
            CreateIndex("dbo.SalesTargets", "CurrencyId");
            AddForeignKey("dbo.SalesTargets", "CurrencyId", "dbo.Currencies", "Id");
        }
    }
}
