namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changesInPayeeNAdminBill : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.PayeeDepartments", newName: "DepartmentPayees");
            DropForeignKey("dbo.AdminBillTypes", "IndustryTypeId", "dbo.IndustryTypes");
            DropIndex("dbo.AdminBillTypes", new[] { "IndustryTypeId" });
            DropPrimaryKey("dbo.DepartmentPayees");
            CreateTable(
                "dbo.CreditCardTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DepartmentChartofAccounts",
                c => new
                    {
                        Department_Id = c.Int(nullable: false),
                        ChartofAccount_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Department_Id, t.ChartofAccount_Id })
                .ForeignKey("dbo.tabDepartment", t => t.Department_Id, cascadeDelete: true)
                .ForeignKey("dbo.ChartofAccounts", t => t.ChartofAccount_Id, cascadeDelete: true)
                .Index(t => t.Department_Id)
                .Index(t => t.ChartofAccount_Id);
            
            CreateTable(
                "dbo.EmployeeChartofAccounts",
                c => new
                    {
                        Employee_EmpId = c.Int(nullable: false),
                        ChartofAccount_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Employee_EmpId, t.ChartofAccount_Id })
                .ForeignKey("dbo.Employees", t => t.Employee_EmpId, cascadeDelete: true)
                .ForeignKey("dbo.ChartofAccounts", t => t.ChartofAccount_Id, cascadeDelete: true)
                .Index(t => t.Employee_EmpId)
                .Index(t => t.ChartofAccount_Id);
            
            AddColumn("dbo.tabAdminBill", "CardHolderType", c => c.Int());
            AddColumn("dbo.tabAdminBill", "SecondaryCreditCardNoId", c => c.Int());
            AddColumn("dbo.CreditCards", "PrimaryCardNoId", c => c.Int());
            AddColumn("dbo.CreditCards", "CardNumber", c => c.String());
            AddColumn("dbo.CreditCards", "creditCardTypeId", c => c.Int());
            AddColumn("dbo.CreditCards", "currencyId", c => c.Int());
            AddColumn("dbo.CreditCards", "LimitAmount", c => c.Double(nullable: false));
            AddPrimaryKey("dbo.DepartmentPayees", new[] { "Department_Id", "Payee_Id" });
            CreateIndex("dbo.CreditCards", "PrimaryCardNoId");
            CreateIndex("dbo.CreditCards", "creditCardTypeId");
            CreateIndex("dbo.CreditCards", "currencyId");
            CreateIndex("dbo.tabAdminBill", "SecondaryCreditCardNoId");
            AddForeignKey("dbo.CreditCards", "creditCardTypeId", "dbo.CreditCardTypes", "Id");
            AddForeignKey("dbo.CreditCards", "currencyId", "dbo.Currencies", "Id");
            AddForeignKey("dbo.CreditCards", "PrimaryCardNoId", "dbo.CreditCards", "Id");
            AddForeignKey("dbo.tabAdminBill", "SecondaryCreditCardNoId", "dbo.CreditCards", "Id");
            DropColumn("dbo.AdminBillTypes", "isIndustryType");
            DropColumn("dbo.AdminBillTypes", "IndustryTypeId");
            DropColumn("dbo.CreditCards", "PrimaryCardNumber");
            DropColumn("dbo.CreditCards", "SecondaryCardNumber");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CreditCards", "SecondaryCardNumber", c => c.String());
            AddColumn("dbo.CreditCards", "PrimaryCardNumber", c => c.String());
            AddColumn("dbo.AdminBillTypes", "IndustryTypeId", c => c.Int());
            AddColumn("dbo.AdminBillTypes", "isIndustryType", c => c.Boolean(nullable: false));
            DropForeignKey("dbo.EmployeeChartofAccounts", "ChartofAccount_Id", "dbo.ChartofAccounts");
            DropForeignKey("dbo.EmployeeChartofAccounts", "Employee_EmpId", "dbo.Employees");
            DropForeignKey("dbo.tabAdminBill", "SecondaryCreditCardNoId", "dbo.CreditCards");
            DropForeignKey("dbo.CreditCards", "PrimaryCardNoId", "dbo.CreditCards");
            DropForeignKey("dbo.CreditCards", "currencyId", "dbo.Currencies");
            DropForeignKey("dbo.CreditCards", "creditCardTypeId", "dbo.CreditCardTypes");
            DropForeignKey("dbo.DepartmentChartofAccounts", "ChartofAccount_Id", "dbo.ChartofAccounts");
            DropForeignKey("dbo.DepartmentChartofAccounts", "Department_Id", "dbo.tabDepartment");
            DropIndex("dbo.EmployeeChartofAccounts", new[] { "ChartofAccount_Id" });
            DropIndex("dbo.EmployeeChartofAccounts", new[] { "Employee_EmpId" });
            DropIndex("dbo.DepartmentChartofAccounts", new[] { "ChartofAccount_Id" });
            DropIndex("dbo.DepartmentChartofAccounts", new[] { "Department_Id" });
            DropIndex("dbo.tabAdminBill", new[] { "SecondaryCreditCardNoId" });
            DropIndex("dbo.CreditCards", new[] { "currencyId" });
            DropIndex("dbo.CreditCards", new[] { "creditCardTypeId" });
            DropIndex("dbo.CreditCards", new[] { "PrimaryCardNoId" });
            DropPrimaryKey("dbo.DepartmentPayees");
            DropColumn("dbo.CreditCards", "LimitAmount");
            DropColumn("dbo.CreditCards", "currencyId");
            DropColumn("dbo.CreditCards", "creditCardTypeId");
            DropColumn("dbo.CreditCards", "CardNumber");
            DropColumn("dbo.CreditCards", "PrimaryCardNoId");
            DropColumn("dbo.tabAdminBill", "SecondaryCreditCardNoId");
            DropColumn("dbo.tabAdminBill", "CardHolderType");
            DropTable("dbo.EmployeeChartofAccounts");
            DropTable("dbo.DepartmentChartofAccounts");
            DropTable("dbo.CreditCardTypes");
            AddPrimaryKey("dbo.DepartmentPayees", new[] { "Payee_Id", "Department_Id" });
            CreateIndex("dbo.AdminBillTypes", "IndustryTypeId");
            AddForeignKey("dbo.AdminBillTypes", "IndustryTypeId", "dbo.IndustryTypes", "Id");
            RenameTable(name: "dbo.DepartmentPayees", newName: "PayeeDepartments");
        }
    }
}
