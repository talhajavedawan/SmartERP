namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DepartmentLoanApplicantsNCompanyLoanApplicantsAdded : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.LoanApplicants", "companyId", "dbo.tabCompany");
            DropForeignKey("dbo.LoanApplicants", "deptId", "dbo.tabDepartment");
            DropIndex("dbo.LoanApplicants", new[] { "companyId" });
            DropIndex("dbo.LoanApplicants", new[] { "deptId" });
            CreateTable(
                "dbo.UniqueNumbers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UniqueName = c.String(),
                        isActive = c.Boolean(nullable: false),
                        From = c.DateTime(nullable: false),
                        To = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DepartmentLoanApplicants",
                c => new
                    {
                        Department_Id = c.Int(nullable: false),
                        LoanApplicant_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Department_Id, t.LoanApplicant_Id })
                .ForeignKey("dbo.tabDepartment", t => t.Department_Id, cascadeDelete: true)
                .ForeignKey("dbo.LoanApplicants", t => t.LoanApplicant_Id, cascadeDelete: true)
                .Index(t => t.Department_Id)
                .Index(t => t.LoanApplicant_Id);
            
            CreateTable(
                "dbo.CompanyLoanApplicants",
                c => new
                    {
                        Company_Id = c.Int(nullable: false),
                        LoanApplicant_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Company_Id, t.LoanApplicant_Id })
                .ForeignKey("dbo.tabCompany", t => t.Company_Id, cascadeDelete: true)
                .ForeignKey("dbo.LoanApplicants", t => t.LoanApplicant_Id, cascadeDelete: true)
                .Index(t => t.Company_Id)
                .Index(t => t.LoanApplicant_Id);
            
            DropColumn("dbo.LoanApplicants", "companyId");
            DropColumn("dbo.LoanApplicants", "deptId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.LoanApplicants", "deptId", c => c.Int());
            AddColumn("dbo.LoanApplicants", "companyId", c => c.Int());
            DropForeignKey("dbo.CompanyLoanApplicants", "LoanApplicant_Id", "dbo.LoanApplicants");
            DropForeignKey("dbo.CompanyLoanApplicants", "Company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.DepartmentLoanApplicants", "LoanApplicant_Id", "dbo.LoanApplicants");
            DropForeignKey("dbo.DepartmentLoanApplicants", "Department_Id", "dbo.tabDepartment");
            DropIndex("dbo.CompanyLoanApplicants", new[] { "LoanApplicant_Id" });
            DropIndex("dbo.CompanyLoanApplicants", new[] { "Company_Id" });
            DropIndex("dbo.DepartmentLoanApplicants", new[] { "LoanApplicant_Id" });
            DropIndex("dbo.DepartmentLoanApplicants", new[] { "Department_Id" });
            DropTable("dbo.CompanyLoanApplicants");
            DropTable("dbo.DepartmentLoanApplicants");
            DropTable("dbo.UniqueNumbers");
            CreateIndex("dbo.LoanApplicants", "deptId");
            CreateIndex("dbo.LoanApplicants", "companyId");
            AddForeignKey("dbo.LoanApplicants", "deptId", "dbo.tabDepartment", "Id");
            AddForeignKey("dbo.LoanApplicants", "companyId", "dbo.tabCompany", "Id");
        }
    }
}
