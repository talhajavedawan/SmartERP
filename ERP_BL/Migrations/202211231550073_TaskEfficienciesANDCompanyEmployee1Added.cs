namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TaskEfficienciesANDCompanyEmployee1Added : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TaskEfficiencies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        efficiencyPoints = c.Int(nullable: false),
                        tasksId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tasks", t => t.tasksId)
                .Index(t => t.tasksId);
            
            CreateTable(
                "dbo.CompanyEmployee1",
                c => new
                    {
                        Company_Id = c.Int(nullable: false),
                        Employee_EmpId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Company_Id, t.Employee_EmpId })
                .ForeignKey("dbo.tabCompany", t => t.Company_Id, cascadeDelete: true)
                .ForeignKey("dbo.Employees", t => t.Employee_EmpId, cascadeDelete: true)
                .Index(t => t.Company_Id)
                .Index(t => t.Employee_EmpId);
            
            AddColumn("dbo.Offers", "uniqueNumber", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CompanyEmployee1", "Employee_EmpId", "dbo.Employees");
            DropForeignKey("dbo.CompanyEmployee1", "Company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.TaskEfficiencies", "tasksId", "dbo.Tasks");
            DropIndex("dbo.CompanyEmployee1", new[] { "Employee_EmpId" });
            DropIndex("dbo.CompanyEmployee1", new[] { "Company_Id" });
            DropIndex("dbo.TaskEfficiencies", new[] { "tasksId" });
            DropColumn("dbo.Offers", "uniqueNumber");
            DropTable("dbo.CompanyEmployee1");
            DropTable("dbo.TaskEfficiencies");
        }
    }
}
