namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changesInBudgetCostSheets : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BudgetCostSheets", "From", c => c.DateTime(nullable: false));
            AddColumn("dbo.BudgetCostSheets", "To", c => c.DateTime(nullable: false));
            AddColumn("dbo.BudgetCostSheets", "LastStatusChangeDate", c => c.DateTime());
            AddColumn("dbo.BudgetCostSheets", "ClosingDate", c => c.DateTime());
            AddColumn("dbo.BudgetCostSheets", "TotalIncome", c => c.Double(nullable: false));
            AddColumn("dbo.BudgetCostSheets", "TotalCGS", c => c.Double(nullable: false));
            AddColumn("dbo.BudgetCostSheets", "TotalExpense", c => c.Double(nullable: false));
            AddColumn("dbo.BudgetCostSheets", "TotalGrossProfit", c => c.Double(nullable: false));
            AddColumn("dbo.BudgetCostSheets", "CreationDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.BudgetCostSheets", "Year");
            DropColumn("dbo.BudgetCostSheets", "Month");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BudgetCostSheets", "Month", c => c.Int(nullable: false));
            AddColumn("dbo.BudgetCostSheets", "Year", c => c.Int(nullable: false));
            DropColumn("dbo.BudgetCostSheets", "CreationDate");
            DropColumn("dbo.BudgetCostSheets", "TotalGrossProfit");
            DropColumn("dbo.BudgetCostSheets", "TotalExpense");
            DropColumn("dbo.BudgetCostSheets", "TotalCGS");
            DropColumn("dbo.BudgetCostSheets", "TotalIncome");
            DropColumn("dbo.BudgetCostSheets", "ClosingDate");
            DropColumn("dbo.BudgetCostSheets", "LastStatusChangeDate");
            DropColumn("dbo.BudgetCostSheets", "To");
            DropColumn("dbo.BudgetCostSheets", "From");
        }
    }
}
