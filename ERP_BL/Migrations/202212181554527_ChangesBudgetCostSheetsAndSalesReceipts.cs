namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesBudgetCostSheetsAndSalesReceipts : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.BudgetCostSheets", "creatorId", "dbo.Users");
            DropForeignKey("dbo.BudgetCostSheets", "deptId", "dbo.tabCompany");
            DropIndex("dbo.BudgetCostSheets", new[] { "creatorId" });
            AddColumn("dbo.SalesReceipts", "DeductionExchangeRate", c => c.Double(nullable: false));
            AddColumn("dbo.SalesReceipts", "DeductionSOC", c => c.Double(nullable: false));
            AddColumn("dbo.BudgetCostSheets", "companyId", c => c.Int());
            AddColumn("dbo.BudgetCostSheets", "empId", c => c.Int());
            AddColumn("dbo.BudgetCostSheets", "TotalNetProfit", c => c.Double(nullable: false));
            AddColumn("dbo.BudgetCostSheets", "currencyId", c => c.Int());
            AddColumn("dbo.BudgetCostSheets", "refNo", c => c.String());
            AlterColumn("dbo.BudgetCostSheets", "CreationDate", c => c.DateTime());
            CreateIndex("dbo.BudgetCostSheets", "companyId");
            CreateIndex("dbo.BudgetCostSheets", "empId");
            CreateIndex("dbo.BudgetCostSheets", "currencyId");
            AddForeignKey("dbo.BudgetCostSheets", "currencyId", "dbo.Currencies", "Id");
            AddForeignKey("dbo.BudgetCostSheets", "empId", "dbo.Employees", "EmpId");
            AddForeignKey("dbo.BudgetCostSheets", "companyId", "dbo.tabCompany", "Id");
            DropColumn("dbo.BudgetCostSheets", "compId");
            DropColumn("dbo.BudgetCostSheets", "creatorId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BudgetCostSheets", "creatorId", c => c.Int());
            AddColumn("dbo.BudgetCostSheets", "compId", c => c.Int());
            DropForeignKey("dbo.BudgetCostSheets", "companyId", "dbo.tabCompany");
            DropForeignKey("dbo.BudgetCostSheets", "empId", "dbo.Employees");
            DropForeignKey("dbo.BudgetCostSheets", "currencyId", "dbo.Currencies");
            DropIndex("dbo.BudgetCostSheets", new[] { "currencyId" });
            DropIndex("dbo.BudgetCostSheets", new[] { "empId" });
            DropIndex("dbo.BudgetCostSheets", new[] { "companyId" });
            AlterColumn("dbo.BudgetCostSheets", "CreationDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.BudgetCostSheets", "refNo");
            DropColumn("dbo.BudgetCostSheets", "currencyId");
            DropColumn("dbo.BudgetCostSheets", "TotalNetProfit");
            DropColumn("dbo.BudgetCostSheets", "empId");
            DropColumn("dbo.BudgetCostSheets", "companyId");
            DropColumn("dbo.SalesReceipts", "DeductionSOC");
            DropColumn("dbo.SalesReceipts", "DeductionExchangeRate");
            CreateIndex("dbo.BudgetCostSheets", "creatorId");
            AddForeignKey("dbo.BudgetCostSheets", "deptId", "dbo.tabCompany", "Id");
            AddForeignKey("dbo.BudgetCostSheets", "creatorId", "dbo.Users", "id");
        }
    }
}
