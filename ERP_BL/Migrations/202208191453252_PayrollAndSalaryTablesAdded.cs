namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PayrollAndSalaryTablesAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Allowances",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Bonus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.EmploymentSalaries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreationDate = c.DateTime(),
                        transactionGroupId = c.Int(nullable: false),
                        SystemRef = c.String(),
                        creatorId = c.Int(),
                        companyId = c.Int(),
                        departmentId = c.Int(),
                        employeeId = c.Int(),
                        BasicSalary = c.Double(nullable: false),
                        FromDate = c.DateTime(),
                        ToDate = c.DateTime(),
                        isActive = c.Boolean(nullable: false),
                        Description = c.String(),
                        isApproved = c.Boolean(),
                        stage = c.String(),
                        ApprovedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tabCompany", t => t.companyId)
                .ForeignKey("dbo.Users", t => t.creatorId)
                .ForeignKey("dbo.tabDepartment", t => t.departmentId)
                .ForeignKey("dbo.Employees", t => t.employeeId)
                .Index(t => t.creatorId)
                .Index(t => t.companyId)
                .Index(t => t.departmentId)
                .Index(t => t.employeeId);
            
            CreateTable(
                "dbo.Payrolls",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreationDate = c.DateTime(),
                        SalaryMonth = c.DateTime(),
                        creatorId = c.Int(),
                        companyId = c.Int(),
                        departmentId = c.Int(),
                        employeeId = c.Int(),
                        employmentSalaryId = c.Int(),
                        ProvidentFund = c.Double(nullable: false),
                        ReturnedLoan = c.Double(nullable: false),
                        loansAdvanceId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tabCompany", t => t.companyId)
                .ForeignKey("dbo.Users", t => t.creatorId)
                .ForeignKey("dbo.tabDepartment", t => t.departmentId)
                .ForeignKey("dbo.Employees", t => t.employeeId)
                .ForeignKey("dbo.LoansAdvances", t => t.loansAdvanceId)
                .ForeignKey("dbo.EmploymentSalaries", t => t.employmentSalaryId)
                .Index(t => t.creatorId)
                .Index(t => t.companyId)
                .Index(t => t.departmentId)
                .Index(t => t.employeeId)
                .Index(t => t.employmentSalaryId)
                .Index(t => t.loansAdvanceId);
            
            CreateTable(
                "dbo.SalaryAllowances",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        payrollId = c.Int(),
                        allowanceId = c.Int(),
                        Amount = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Allowances", t => t.allowanceId)
                .ForeignKey("dbo.Payrolls", t => t.payrollId)
                .Index(t => t.payrollId)
                .Index(t => t.allowanceId);
            
            CreateTable(
                "dbo.SalaryBonus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        payrollId = c.Int(),
                        bonusId = c.Int(),
                        Amount = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Bonus", t => t.bonusId)
                .ForeignKey("dbo.Payrolls", t => t.payrollId)
                .Index(t => t.payrollId)
                .Index(t => t.bonusId);
            
            CreateTable(
                "dbo.SalaryDeductions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        payrollId = c.Int(),
                        deductionId = c.Int(),
                        Amount = c.Double(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SalDeductions", t => t.deductionId)
                .ForeignKey("dbo.Payrolls", t => t.payrollId)
                .Index(t => t.payrollId)
                .Index(t => t.deductionId);
            
            CreateTable(
                "dbo.SalDeductions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.TaskGroups", "isBackground", c => c.Boolean(nullable: false));
            AddColumn("dbo.LoansAdvances", "transactionGroupId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Payrolls", "employmentSalaryId", "dbo.EmploymentSalaries");
            DropForeignKey("dbo.SalaryDeductions", "payrollId", "dbo.Payrolls");
            DropForeignKey("dbo.SalaryDeductions", "deductionId", "dbo.SalDeductions");
            DropForeignKey("dbo.SalaryBonus", "payrollId", "dbo.Payrolls");
            DropForeignKey("dbo.SalaryBonus", "bonusId", "dbo.Bonus");
            DropForeignKey("dbo.SalaryAllowances", "payrollId", "dbo.Payrolls");
            DropForeignKey("dbo.SalaryAllowances", "allowanceId", "dbo.Allowances");
            DropForeignKey("dbo.Payrolls", "loansAdvanceId", "dbo.LoansAdvances");
            DropForeignKey("dbo.Payrolls", "employeeId", "dbo.Employees");
            DropForeignKey("dbo.Payrolls", "departmentId", "dbo.tabDepartment");
            DropForeignKey("dbo.Payrolls", "creatorId", "dbo.Users");
            DropForeignKey("dbo.Payrolls", "companyId", "dbo.tabCompany");
            DropForeignKey("dbo.EmploymentSalaries", "employeeId", "dbo.Employees");
            DropForeignKey("dbo.EmploymentSalaries", "departmentId", "dbo.tabDepartment");
            DropForeignKey("dbo.EmploymentSalaries", "creatorId", "dbo.Users");
            DropForeignKey("dbo.EmploymentSalaries", "companyId", "dbo.tabCompany");
            DropIndex("dbo.SalaryDeductions", new[] { "deductionId" });
            DropIndex("dbo.SalaryDeductions", new[] { "payrollId" });
            DropIndex("dbo.SalaryBonus", new[] { "bonusId" });
            DropIndex("dbo.SalaryBonus", new[] { "payrollId" });
            DropIndex("dbo.SalaryAllowances", new[] { "allowanceId" });
            DropIndex("dbo.SalaryAllowances", new[] { "payrollId" });
            DropIndex("dbo.Payrolls", new[] { "loansAdvanceId" });
            DropIndex("dbo.Payrolls", new[] { "employmentSalaryId" });
            DropIndex("dbo.Payrolls", new[] { "employeeId" });
            DropIndex("dbo.Payrolls", new[] { "departmentId" });
            DropIndex("dbo.Payrolls", new[] { "companyId" });
            DropIndex("dbo.Payrolls", new[] { "creatorId" });
            DropIndex("dbo.EmploymentSalaries", new[] { "employeeId" });
            DropIndex("dbo.EmploymentSalaries", new[] { "departmentId" });
            DropIndex("dbo.EmploymentSalaries", new[] { "companyId" });
            DropIndex("dbo.EmploymentSalaries", new[] { "creatorId" });
            DropColumn("dbo.LoansAdvances", "transactionGroupId");
            DropColumn("dbo.TaskGroups", "isBackground");
            DropTable("dbo.SalDeductions");
            DropTable("dbo.SalaryDeductions");
            DropTable("dbo.SalaryBonus");
            DropTable("dbo.SalaryAllowances");
            DropTable("dbo.Payrolls");
            DropTable("dbo.EmploymentSalaries");
            DropTable("dbo.Bonus");
            DropTable("dbo.Allowances");
        }
    }
}
