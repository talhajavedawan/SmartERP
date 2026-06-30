namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class costsheetdeliverydate : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SaleInvoices", "CostSheet_Id", "dbo.CostSheets");
            DropIndex("dbo.SaleInvoices", new[] { "CostSheet_Id" });
            AddColumn("dbo.SaleOrders", "InvoiceStage", c => c.String());
            AddColumn("dbo.CostSheets", "PODeliveryDate", c => c.DateTime());
            DropColumn("dbo.CostSheets", "deliveryTime");
            DropColumn("dbo.SaleInvoices", "commision");
            DropColumn("dbo.SaleInvoices", "commisioninBase");
            DropColumn("dbo.SaleInvoices", "margin");
            DropColumn("dbo.SaleInvoices", "BudgetedMargininBase");
            DropColumn("dbo.SaleInvoices", "SalesBudgetedMargin");
            DropColumn("dbo.SaleInvoices", "ActualMargin");
            DropColumn("dbo.SaleInvoices", "ActualMargininBase");
            DropColumn("dbo.SaleInvoices", "SalesActualMargin");
            DropColumn("dbo.SaleInvoices", "CostSheet_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SaleInvoices", "CostSheet_Id", c => c.Int());
            AddColumn("dbo.SaleInvoices", "SalesActualMargin", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.SaleInvoices", "ActualMargininBase", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.SaleInvoices", "ActualMargin", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.SaleInvoices", "SalesBudgetedMargin", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.SaleInvoices", "BudgetedMargininBase", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.SaleInvoices", "margin", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.SaleInvoices", "commisioninBase", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.SaleInvoices", "commision", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.CostSheets", "deliveryTime", c => c.String());
            DropColumn("dbo.CostSheets", "PODeliveryDate");
            DropColumn("dbo.SaleOrders", "InvoiceStage");
            CreateIndex("dbo.SaleInvoices", "CostSheet_Id");
            AddForeignKey("dbo.SaleInvoices", "CostSheet_Id", "dbo.CostSheets", "Id");
        }
    }
}
