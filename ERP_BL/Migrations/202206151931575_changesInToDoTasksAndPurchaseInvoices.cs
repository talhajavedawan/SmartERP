namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changesInToDoTasksAndPurchaseInvoices : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ToDoTasks", "margin", c => c.Double(nullable: false));
            AddColumn("dbo.ToDoTasks", "BudgetedMargininBase", c => c.Double(nullable: false));
            AddColumn("dbo.ToDoTasks", "SalesBudgetedMargin", c => c.Double(nullable: false));
            AddColumn("dbo.ToDoTasks", "RevisedMargin", c => c.Double(nullable: false));
            AddColumn("dbo.ToDoTasks", "RevisedMargininBase", c => c.Double(nullable: false));
            AddColumn("dbo.ToDoTasks", "SalesRevisedMargin", c => c.Double(nullable: false));
            AddColumn("dbo.ToDoTasks", "ActualMargin", c => c.Double(nullable: false));
            AddColumn("dbo.ToDoTasks", "ActualMargininBase", c => c.Double(nullable: false));
            AddColumn("dbo.ToDoTasks", "SalesActualMargin", c => c.Double(nullable: false));
            AddColumn("dbo.ToDoTasks", "totalFOBValue", c => c.Double(nullable: false));
            AddColumn("dbo.ToDoTasks", "totalCFRValue", c => c.Double(nullable: false));
            AddColumn("dbo.ToDoTasks", "totalBaseCFRValue", c => c.Double(nullable: false));
            AddColumn("dbo.ToDoTasks", "SoAmountSER", c => c.Double(nullable: false));
            AddColumn("dbo.ToDoTasks", "SoAmountPER", c => c.Double(nullable: false));
            AddColumn("dbo.ToDoTasks", "commisioninBase", c => c.Double(nullable: false));
            AddColumn("dbo.ToDoTasks", "commision", c => c.Double(nullable: false));
            AddColumn("dbo.ToDoTaskStatus", "forStep", c => c.Boolean(nullable: false));
            AddColumn("dbo.PurchaseOrders", "isAdjustedTax", c => c.Boolean(nullable: false));
            AddColumn("dbo.PurchaseInvoices", "isAdjustedTax", c => c.Boolean(nullable: false));
            AddColumn("dbo.TaxNames", "isAdjusted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TaxNames", "isAdjusted");
            DropColumn("dbo.PurchaseInvoices", "isAdjustedTax");
            DropColumn("dbo.PurchaseOrders", "isAdjustedTax");
            DropColumn("dbo.ToDoTaskStatus", "forStep");
            DropColumn("dbo.ToDoTasks", "commision");
            DropColumn("dbo.ToDoTasks", "commisioninBase");
            DropColumn("dbo.ToDoTasks", "SoAmountPER");
            DropColumn("dbo.ToDoTasks", "SoAmountSER");
            DropColumn("dbo.ToDoTasks", "totalBaseCFRValue");
            DropColumn("dbo.ToDoTasks", "totalCFRValue");
            DropColumn("dbo.ToDoTasks", "totalFOBValue");
            DropColumn("dbo.ToDoTasks", "SalesActualMargin");
            DropColumn("dbo.ToDoTasks", "ActualMargininBase");
            DropColumn("dbo.ToDoTasks", "ActualMargin");
            DropColumn("dbo.ToDoTasks", "SalesRevisedMargin");
            DropColumn("dbo.ToDoTasks", "RevisedMargininBase");
            DropColumn("dbo.ToDoTasks", "RevisedMargin");
            DropColumn("dbo.ToDoTasks", "SalesBudgetedMargin");
            DropColumn("dbo.ToDoTasks", "BudgetedMargininBase");
            DropColumn("dbo.ToDoTasks", "margin");
        }
    }
}
