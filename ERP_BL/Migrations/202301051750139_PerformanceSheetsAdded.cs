namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PerformanceSheetsAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PerformanceSheets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        soNumber = c.Int(nullable: false),
                        customerId = c.Int(),
                        deptId = c.Int(),
                        supervoisedId = c.Int(),
                        staffLevelOneId = c.Int(),
                        staffLevelTwoId = c.Int(),
                        totalPoints = c.Double(nullable: false),
                        totalAveragePoints = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CustomerCompanies", t => t.customerId)
                .ForeignKey("dbo.tabDepartment", t => t.deptId)
                .ForeignKey("dbo.Employees", t => t.staffLevelOneId)
                .ForeignKey("dbo.Employees", t => t.staffLevelTwoId)
                .ForeignKey("dbo.Employees", t => t.supervoisedId)
                .Index(t => t.customerId)
                .Index(t => t.deptId)
                .Index(t => t.supervoisedId)
                .Index(t => t.staffLevelOneId)
                .Index(t => t.staffLevelTwoId);
            
            CreateTable(
                "dbo.PerfomarmanceSheetFields",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Head_Id = c.Int(),
                        point = c.Double(nullable: false),
                        PerformanceSheet_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PerformanceSheetHeads", t => t.Head_Id)
                .ForeignKey("dbo.PerformanceSheets", t => t.PerformanceSheet_Id)
                .Index(t => t.Head_Id)
                .Index(t => t.PerformanceSheet_Id);
            
            CreateTable(
                "dbo.PerformanceSheetHeads",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        HeadName = c.String(),
                        isActive = c.Boolean(nullable: false),
                        SortId = c.Int(nullable: false),
                        creatorId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.creatorId)
                .Index(t => t.creatorId);
            
            AddColumn("dbo.SaleOrders", "PerformanceSheet_Id", c => c.Int());
            AddColumn("dbo.ToDoTasks", "netCommision", c => c.Double(nullable: false));
            AddColumn("dbo.ToDoTasks", "netCommisionSER", c => c.Double(nullable: false));
            AddColumn("dbo.ToDoTasks", "netCommisionMER", c => c.Double(nullable: false));
            CreateIndex("dbo.SaleOrders", "PerformanceSheet_Id");
            AddForeignKey("dbo.SaleOrders", "PerformanceSheet_Id", "dbo.PerformanceSheets", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SaleOrders", "PerformanceSheet_Id", "dbo.PerformanceSheets");
            DropForeignKey("dbo.PerformanceSheets", "supervoisedId", "dbo.Employees");
            DropForeignKey("dbo.PerformanceSheets", "staffLevelTwoId", "dbo.Employees");
            DropForeignKey("dbo.PerformanceSheets", "staffLevelOneId", "dbo.Employees");
            DropForeignKey("dbo.PerfomarmanceSheetFields", "PerformanceSheet_Id", "dbo.PerformanceSheets");
            DropForeignKey("dbo.PerfomarmanceSheetFields", "Head_Id", "dbo.PerformanceSheetHeads");
            DropForeignKey("dbo.PerformanceSheetHeads", "creatorId", "dbo.Users");
            DropForeignKey("dbo.PerformanceSheets", "deptId", "dbo.tabDepartment");
            DropForeignKey("dbo.PerformanceSheets", "customerId", "dbo.CustomerCompanies");
            DropIndex("dbo.PerformanceSheetHeads", new[] { "creatorId" });
            DropIndex("dbo.PerfomarmanceSheetFields", new[] { "PerformanceSheet_Id" });
            DropIndex("dbo.PerfomarmanceSheetFields", new[] { "Head_Id" });
            DropIndex("dbo.PerformanceSheets", new[] { "staffLevelTwoId" });
            DropIndex("dbo.PerformanceSheets", new[] { "staffLevelOneId" });
            DropIndex("dbo.PerformanceSheets", new[] { "supervoisedId" });
            DropIndex("dbo.PerformanceSheets", new[] { "deptId" });
            DropIndex("dbo.PerformanceSheets", new[] { "customerId" });
            DropIndex("dbo.SaleOrders", new[] { "PerformanceSheet_Id" });
            DropColumn("dbo.ToDoTasks", "netCommisionMER");
            DropColumn("dbo.ToDoTasks", "netCommisionSER");
            DropColumn("dbo.ToDoTasks", "netCommision");
            DropColumn("dbo.SaleOrders", "PerformanceSheet_Id");
            DropTable("dbo.PerformanceSheetHeads");
            DropTable("dbo.PerfomarmanceSheetFields");
            DropTable("dbo.PerformanceSheets");
        }
    }
}
