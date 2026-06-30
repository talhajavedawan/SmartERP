namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CompanyEmployeeAdded : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.DepartmentPayees", newName: "PayeeDepartments");
            DropPrimaryKey("dbo.PayeeDepartments");
            CreateTable(
                "dbo.CompanyEmployees",
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
            
            AddPrimaryKey("dbo.PayeeDepartments", new[] { "Payee_Id", "Department_Id" });
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CompanyEmployees", "Employee_EmpId", "dbo.Employees");
            DropForeignKey("dbo.CompanyEmployees", "Company_Id", "dbo.tabCompany");
            DropIndex("dbo.CompanyEmployees", new[] { "Employee_EmpId" });
            DropIndex("dbo.CompanyEmployees", new[] { "Company_Id" });
            DropPrimaryKey("dbo.PayeeDepartments");
            DropTable("dbo.CompanyEmployees");
            AddPrimaryKey("dbo.PayeeDepartments", new[] { "Department_Id", "Payee_Id" });
            RenameTable(name: "dbo.PayeeDepartments", newName: "DepartmentPayees");
        }
    }
}
