namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CashFlowsAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CashFlows",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        reportName = c.String(),
                        userId = c.Int(),
                        groupId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GridReportGroups", t => t.groupId)
                .ForeignKey("dbo.Users", t => t.userId)
                .Index(t => t.userId)
                .Index(t => t.groupId);
            
            CreateTable(
                "dbo.CashFlowCompanies",
                c => new
                    {
                        CashFlow_Id = c.Int(nullable: false),
                        Company_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CashFlow_Id, t.Company_Id })
                .ForeignKey("dbo.CashFlows", t => t.CashFlow_Id, cascadeDelete: true)
                .ForeignKey("dbo.tabCompany", t => t.Company_Id, cascadeDelete: true)
                .Index(t => t.CashFlow_Id)
                .Index(t => t.Company_Id);
            
            CreateTable(
                "dbo.CashFlowDepartments",
                c => new
                    {
                        CashFlow_Id = c.Int(nullable: false),
                        Department_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CashFlow_Id, t.Department_Id })
                .ForeignKey("dbo.CashFlows", t => t.CashFlow_Id, cascadeDelete: true)
                .ForeignKey("dbo.tabDepartment", t => t.Department_Id, cascadeDelete: true)
                .Index(t => t.CashFlow_Id)
                .Index(t => t.Department_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CashFlows", "userId", "dbo.Users");
            DropForeignKey("dbo.CashFlows", "groupId", "dbo.GridReportGroups");
            DropForeignKey("dbo.CashFlowDepartments", "Department_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.CashFlowDepartments", "CashFlow_Id", "dbo.CashFlows");
            DropForeignKey("dbo.CashFlowCompanies", "Company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.CashFlowCompanies", "CashFlow_Id", "dbo.CashFlows");
            DropIndex("dbo.CashFlowDepartments", new[] { "Department_Id" });
            DropIndex("dbo.CashFlowDepartments", new[] { "CashFlow_Id" });
            DropIndex("dbo.CashFlowCompanies", new[] { "Company_Id" });
            DropIndex("dbo.CashFlowCompanies", new[] { "CashFlow_Id" });
            DropIndex("dbo.CashFlows", new[] { "groupId" });
            DropIndex("dbo.CashFlows", new[] { "userId" });
            DropTable("dbo.CashFlowDepartments");
            DropTable("dbo.CashFlowCompanies");
            DropTable("dbo.CashFlows");
        }
    }
}
