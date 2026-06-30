namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModificationsInBusiTransactions : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PurchaseOrders", "Budget_Id", c => c.Int());
            AddColumn("dbo.SaleInvoices", "isInterCompanyReceivable", c => c.Boolean(nullable: false));
            AddColumn("dbo.BudgetSystemCostFields", "PurchaseInvoice_Id", c => c.Int());
            AddColumn("dbo.BudgetSystemCostFields", "Bill_Id", c => c.Int());
            AddColumn("dbo.PerformanceSheets", "totalPointsPerc", c => c.Double(nullable: false));
            AddColumn("dbo.PerfomarmanceSheetFields", "revisedPoint", c => c.Double(nullable: false));
            CreateIndex("dbo.PurchaseOrders", "Budget_Id");
            CreateIndex("dbo.BudgetSystemCostFields", "PurchaseInvoice_Id");
            CreateIndex("dbo.BudgetSystemCostFields", "Bill_Id");
            AddForeignKey("dbo.PurchaseOrders", "Budget_Id", "dbo.BudgetCostSheets", "Id");
            AddForeignKey("dbo.BudgetSystemCostFields", "PurchaseInvoice_Id", "dbo.PurchaseInvoices", "Id");
            AddForeignKey("dbo.BudgetSystemCostFields", "Bill_Id", "dbo.Bills", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BudgetSystemCostFields", "Bill_Id", "dbo.Bills");
            DropForeignKey("dbo.BudgetSystemCostFields", "PurchaseInvoice_Id", "dbo.PurchaseInvoices");
            DropForeignKey("dbo.PurchaseOrders", "Budget_Id", "dbo.BudgetCostSheets");
            DropIndex("dbo.BudgetSystemCostFields", new[] { "Bill_Id" });
            DropIndex("dbo.BudgetSystemCostFields", new[] { "PurchaseInvoice_Id" });
            DropIndex("dbo.PurchaseOrders", new[] { "Budget_Id" });
            DropColumn("dbo.PerfomarmanceSheetFields", "revisedPoint");
            DropColumn("dbo.PerformanceSheets", "totalPointsPerc");
            DropColumn("dbo.BudgetSystemCostFields", "Bill_Id");
            DropColumn("dbo.BudgetSystemCostFields", "PurchaseInvoice_Id");
            DropColumn("dbo.SaleInvoices", "isInterCompanyReceivable");
            DropColumn("dbo.PurchaseOrders", "Budget_Id");
        }
    }
}
