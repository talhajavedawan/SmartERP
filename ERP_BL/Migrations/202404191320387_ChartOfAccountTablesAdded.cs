namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChartOfAccountTablesAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ChartofAccountGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        referenceNo = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ChartofAccountGroupChartofAccounts",
                c => new
                    {
                        ChartofAccountGroup_Id = c.Int(nullable: false),
                        ChartofAccount_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ChartofAccountGroup_Id, t.ChartofAccount_Id })
                .ForeignKey("dbo.ChartofAccountGroups", t => t.ChartofAccountGroup_Id, cascadeDelete: true)
                .ForeignKey("dbo.ChartofAccounts", t => t.ChartofAccount_Id, cascadeDelete: true)
                .Index(t => t.ChartofAccountGroup_Id)
                .Index(t => t.ChartofAccount_Id);
            
            CreateTable(
                "dbo.DepartmentChartofAccountGroups",
                c => new
                    {
                        Department_Id = c.Int(nullable: false),
                        ChartofAccountGroup_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Department_Id, t.ChartofAccountGroup_Id })
                .ForeignKey("dbo.tabDepartment", t => t.Department_Id, cascadeDelete: true)
                .ForeignKey("dbo.ChartofAccountGroups", t => t.ChartofAccountGroup_Id, cascadeDelete: true)
                .Index(t => t.Department_Id)
                .Index(t => t.ChartofAccountGroup_Id);
            
            CreateTable(
                "dbo.EmployeeChartofAccountGroups",
                c => new
                    {
                        Employee_EmpId = c.Int(nullable: false),
                        ChartofAccountGroup_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Employee_EmpId, t.ChartofAccountGroup_Id })
                .ForeignKey("dbo.Employees", t => t.Employee_EmpId, cascadeDelete: true)
                .ForeignKey("dbo.ChartofAccountGroups", t => t.ChartofAccountGroup_Id, cascadeDelete: true)
                .Index(t => t.Employee_EmpId)
                .Index(t => t.ChartofAccountGroup_Id);
            
            CreateTable(
                "dbo.CompanyChartofAccountGroups",
                c => new
                    {
                        Company_Id = c.Int(nullable: false),
                        ChartofAccountGroup_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Company_Id, t.ChartofAccountGroup_Id })
                .ForeignKey("dbo.tabCompany", t => t.Company_Id, cascadeDelete: true)
                .ForeignKey("dbo.ChartofAccountGroups", t => t.ChartofAccountGroup_Id, cascadeDelete: true)
                .Index(t => t.Company_Id)
                .Index(t => t.ChartofAccountGroup_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CompanyChartofAccountGroups", "ChartofAccountGroup_Id", "dbo.ChartofAccountGroups");
            DropForeignKey("dbo.CompanyChartofAccountGroups", "Company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.EmployeeChartofAccountGroups", "ChartofAccountGroup_Id", "dbo.ChartofAccountGroups");
            DropForeignKey("dbo.EmployeeChartofAccountGroups", "Employee_EmpId", "dbo.Employees");
            DropForeignKey("dbo.DepartmentChartofAccountGroups", "ChartofAccountGroup_Id", "dbo.ChartofAccountGroups");
            DropForeignKey("dbo.DepartmentChartofAccountGroups", "Department_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.ChartofAccountGroupChartofAccounts", "ChartofAccount_Id", "dbo.ChartofAccounts");
            DropForeignKey("dbo.ChartofAccountGroupChartofAccounts", "ChartofAccountGroup_Id", "dbo.ChartofAccountGroups");
            DropIndex("dbo.CompanyChartofAccountGroups", new[] { "ChartofAccountGroup_Id" });
            DropIndex("dbo.CompanyChartofAccountGroups", new[] { "Company_Id" });
            DropIndex("dbo.EmployeeChartofAccountGroups", new[] { "ChartofAccountGroup_Id" });
            DropIndex("dbo.EmployeeChartofAccountGroups", new[] { "Employee_EmpId" });
            DropIndex("dbo.DepartmentChartofAccountGroups", new[] { "ChartofAccountGroup_Id" });
            DropIndex("dbo.DepartmentChartofAccountGroups", new[] { "Department_Id" });
            DropIndex("dbo.ChartofAccountGroupChartofAccounts", new[] { "ChartofAccount_Id" });
            DropIndex("dbo.ChartofAccountGroupChartofAccounts", new[] { "ChartofAccountGroup_Id" });
            DropTable("dbo.CompanyChartofAccountGroups");
            DropTable("dbo.EmployeeChartofAccountGroups");
            DropTable("dbo.DepartmentChartofAccountGroups");
            DropTable("dbo.ChartofAccountGroupChartofAccounts");
            DropTable("dbo.ChartofAccountGroups");
        }
    }
}
