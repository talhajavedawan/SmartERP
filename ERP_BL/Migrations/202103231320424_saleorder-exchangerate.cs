namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class saleorderexchangerate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SaleOrders", "saleExchangerateId", c => c.Int());
            AddColumn("dbo.SaleOrders", "marketExchangerateId", c => c.Int());
            AddColumn("dbo.MarketExchangeRates", "effectiveTo", c => c.DateTime(nullable: false));
            AddColumn("dbo.SalesExchangeRates", "effectiveTo", c => c.DateTime(nullable: false));
            CreateIndex("dbo.SaleOrders", "saleExchangerateId");
            CreateIndex("dbo.SaleOrders", "marketExchangerateId");
            AddForeignKey("dbo.SaleOrders", "marketExchangerateId", "dbo.MarketExchangeRates", "Id");
            AddForeignKey("dbo.SaleOrders", "saleExchangerateId", "dbo.SalesExchangeRates", "Id");
            DropColumn("dbo.MarketExchangeRates", "effectiveSince");
            DropColumn("dbo.SalesExchangeRates", "effectiveSince");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SalesExchangeRates", "effectiveSince", c => c.DateTime(nullable: false));
            AddColumn("dbo.MarketExchangeRates", "effectiveSince", c => c.DateTime(nullable: false));
            DropForeignKey("dbo.SaleOrders", "saleExchangerateId", "dbo.SalesExchangeRates");
            DropForeignKey("dbo.SaleOrders", "marketExchangerateId", "dbo.MarketExchangeRates");
            DropIndex("dbo.SaleOrders", new[] { "marketExchangerateId" });
            DropIndex("dbo.SaleOrders", new[] { "saleExchangerateId" });
            DropColumn("dbo.SalesExchangeRates", "effectiveTo");
            DropColumn("dbo.MarketExchangeRates", "effectiveTo");
            DropColumn("dbo.SaleOrders", "marketExchangerateId");
            DropColumn("dbo.SaleOrders", "saleExchangerateId");
        }
    }
}
