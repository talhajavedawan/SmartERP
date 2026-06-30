namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BudgetSystemCostFieldsAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BudgetSystemCostFields",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Head_Id = c.Int(),
                    BudgetedCost = c.Double(nullable: false),
                    RSBC = c.Double(nullable: false),
                    addedSystemCost = c.Double(nullable: false),
                    SaleInvoice_Id = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BudgetSheetHeads", t => t.Head_Id)
                .ForeignKey("dbo.SaleInvoices", t => t.SaleInvoice_Id)
                .Index(t => t.Head_Id)
                .Index(t => t.SaleInvoice_Id);

            AddColumn("dbo.SaleOrders", "revisedBudgetAmount", c => c.Double(nullable: false));
            AddColumn("dbo.SaleOrders", "Budget_Id", c => c.Int());
            AddColumn("dbo.SaleOrders", "auditYear", c => c.DateTime());
            AddColumn("dbo.BudgetCostFields", "addedRSBC", c => c.Double(nullable: false));
            AddColumn("dbo.BudgetCostSheets", "TotalRSBCIncome", c => c.Double(nullable: false));
            AddColumn("dbo.BudgetCostSheets", "TotalRSBCCGS", c => c.Double(nullable: false));
            AddColumn("dbo.BudgetCostSheets", "TotalRSBCExpense", c => c.Double(nullable: false));
            AddColumn("dbo.BudgetCostSheets", "TotalRSBCGrossProfit", c => c.Double(nullable: false));
            AddColumn("dbo.BudgetCostSheets", "TotalRSBCNetProfit", c => c.Double(nullable: false));
            AddColumn("dbo.BudgetCostSheets", "BudgetMonth", c => c.DateTime());
            CreateIndex("dbo.SaleOrders", "Budget_Id");
            AddForeignKey("dbo.SaleOrders", "Budget_Id", "dbo.BudgetCostSheets", "Id");
            DropColumn("dbo.BudgetCostFields", "systemCost");
            DropColumn("dbo.BudgetCostFields", "scPerc");
            DropColumn("dbo.BudgetCostFields", "siCost");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BudgetCostFields", "siCost", c => c.Double(nullable: false));
            AddColumn("dbo.BudgetCostFields", "scPerc", c => c.Double(nullable: false));
            AddColumn("dbo.BudgetCostFields", "systemCost", c => c.Double(nullable: false));
            DropForeignKey("dbo.SaleOrders", "Budget_Id", "dbo.BudgetCostSheets");
            DropForeignKey("dbo.BudgetSystemCostFields", "SaleInvoice_Id", "dbo.SaleInvoices");
            DropForeignKey("dbo.BudgetSystemCostFields", "Head_Id", "dbo.BudgetSheetHeads");
            DropIndex("dbo.SaleOrders", new[] { "Budget_Id" });
            DropIndex("dbo.BudgetSystemCostFields", new[] { "SaleInvoice_Id" });
            DropIndex("dbo.BudgetSystemCostFields", new[] { "Head_Id" });
            DropColumn("dbo.BudgetCostSheets", "BudgetMonth");
            DropColumn("dbo.BudgetCostSheets", "TotalRSBCNetProfit");
            DropColumn("dbo.BudgetCostSheets", "TotalRSBCGrossProfit");
            DropColumn("dbo.BudgetCostSheets", "TotalRSBCExpense");
            DropColumn("dbo.BudgetCostSheets", "TotalRSBCCGS");
            DropColumn("dbo.BudgetCostSheets", "TotalRSBCIncome");
            DropColumn("dbo.BudgetCostFields", "addedRSBC");
            DropColumn("dbo.SaleOrders", "auditYear");
            DropColumn("dbo.SaleOrders", "Budget_Id");
            DropColumn("dbo.SaleOrders", "revisedBudgetAmount");
            DropTable("dbo.BudgetSystemCostFields");
        }
    }
}
