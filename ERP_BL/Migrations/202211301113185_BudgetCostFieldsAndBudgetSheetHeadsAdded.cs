namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BudgetCostFieldsAndBudgetSheetHeadsAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BudgetCostFields",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Head_Id = c.Int(),
                        BudgetedCost = c.Double(nullable: false),
                        bmPerc = c.Double(nullable: false),
                        RSBC = c.Double(nullable: false),
                        rsbcPerc = c.Double(nullable: false),
                        systemCost = c.Double(nullable: false),
                        scPerc = c.Double(nullable: false),
                        AdjustmentCost = c.Double(nullable: false),
                        siCost = c.Double(nullable: false),
                        BudgetCostSheet_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BudgetSheetHeads", t => t.Head_Id)
                .ForeignKey("dbo.BudgetCostSheets", t => t.BudgetCostSheet_Id)
                .Index(t => t.Head_Id)
                .Index(t => t.BudgetCostSheet_Id);
            
            CreateTable(
                "dbo.BudgetSheetHeads",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        HeadName = c.String(),
                        isActive = c.Boolean(nullable: false),
                        SortId = c.Int(nullable: false),
                        creatorId = c.Int(),
                        isIncome = c.Boolean(nullable: false),
                        isCGS = c.Boolean(nullable: false),
                        isExpense = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.creatorId)
                .Index(t => t.creatorId);
            
            CreateTable(
                "dbo.BudgetCostSheets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        compId = c.Int(),
                        deptId = c.Int(),
                        Year = c.Int(nullable: false),
                        Month = c.Int(nullable: false),
                        PendingForClosing = c.Boolean(),
                        PendingForReApproval = c.Boolean(),
                        isApproved = c.Boolean(),
                        ReApprovalDate = c.DateTime(),
                        isReApproved = c.Boolean(),
                        ApprovedDate = c.DateTime(),
                        isReviewed = c.Boolean(),
                        needReview = c.Boolean(),
                        stage = c.String(),
                        isVoid = c.Boolean(nullable: false),
                        creatorId = c.Int(),
                        budgetCostSheetStatus_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BudgetCostSheetStatus", t => t.budgetCostSheetStatus_Id)
                .ForeignKey("dbo.tabCompany", t => t.deptId)
                .ForeignKey("dbo.Users", t => t.creatorId)
                .ForeignKey("dbo.tabDepartment", t => t.deptId)
                .Index(t => t.deptId)
                .Index(t => t.creatorId)
                .Index(t => t.budgetCostSheetStatus_Id);
            
            CreateTable(
                "dbo.BudgetCostSheetStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.String(),
                        isApproved = c.Boolean(nullable: false),
                        isActive = c.Boolean(),
                        backcolor = c.String(),
                        forecolor = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BudgetCostSheets", "deptId", "dbo.tabDepartment");
            DropForeignKey("dbo.BudgetCostSheets", "creatorId", "dbo.Users");
            DropForeignKey("dbo.BudgetCostSheets", "deptId", "dbo.tabCompany");
            DropForeignKey("dbo.BudgetCostSheets", "budgetCostSheetStatus_Id", "dbo.BudgetCostSheetStatus");
            DropForeignKey("dbo.BudgetCostFields", "BudgetCostSheet_Id", "dbo.BudgetCostSheets");
            DropForeignKey("dbo.BudgetCostFields", "Head_Id", "dbo.BudgetSheetHeads");
            DropForeignKey("dbo.BudgetSheetHeads", "creatorId", "dbo.Users");
            DropIndex("dbo.BudgetCostSheets", new[] { "budgetCostSheetStatus_Id" });
            DropIndex("dbo.BudgetCostSheets", new[] { "creatorId" });
            DropIndex("dbo.BudgetCostSheets", new[] { "deptId" });
            DropIndex("dbo.BudgetSheetHeads", new[] { "creatorId" });
            DropIndex("dbo.BudgetCostFields", new[] { "BudgetCostSheet_Id" });
            DropIndex("dbo.BudgetCostFields", new[] { "Head_Id" });
            DropTable("dbo.BudgetCostSheetStatus");
            DropTable("dbo.BudgetCostSheets");
            DropTable("dbo.BudgetSheetHeads");
            DropTable("dbo.BudgetCostFields");
        }
    }
}
