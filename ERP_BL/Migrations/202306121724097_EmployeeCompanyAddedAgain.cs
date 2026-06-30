namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EmployeeCompanyAddedAgain : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EmployeeCompany1",
                c => new
                    {
                        Employee_EmpId = c.Int(nullable: false),
                        Company_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Employee_EmpId, t.Company_Id })
                .ForeignKey("dbo.Employees", t => t.Employee_EmpId, cascadeDelete: true)
                .ForeignKey("dbo.tabCompany", t => t.Company_Id, cascadeDelete: true)
                .Index(t => t.Employee_EmpId)
                .Index(t => t.Company_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EmployeeCompany1", "Company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.EmployeeCompany1", "Employee_EmpId", "dbo.Employees");
            DropIndex("dbo.EmployeeCompany1", new[] { "Company_Id" });
            DropIndex("dbo.EmployeeCompany1", new[] { "Employee_EmpId" });
            DropTable("dbo.EmployeeCompany1");
        }
    }
}
