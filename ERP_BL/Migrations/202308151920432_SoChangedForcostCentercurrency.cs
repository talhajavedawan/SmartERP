namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SoChangedForcostCentercurrency : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SaleOrders", "costCentercurrency_Id", c => c.Int());
            AddColumn("dbo.SaleOrders", "costCenterExchangeRate", c => c.Double(nullable: false));
            AddColumn("dbo.SaleOrders", "costCenterAmount", c => c.Double(nullable: false));
            CreateIndex("dbo.SaleOrders", "costCentercurrency_Id");
            AddForeignKey("dbo.SaleOrders", "costCentercurrency_Id", "dbo.Currencies", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SaleOrders", "costCentercurrency_Id", "dbo.Currencies");
            DropIndex("dbo.SaleOrders", new[] { "costCentercurrency_Id" });
            DropColumn("dbo.SaleOrders", "costCenterAmount");
            DropColumn("dbo.SaleOrders", "costCenterExchangeRate");
            DropColumn("dbo.SaleOrders", "costCentercurrency_Id");
        }
    }
}
