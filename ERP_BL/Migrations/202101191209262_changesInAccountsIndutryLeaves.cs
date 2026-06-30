namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changesInAccountsIndutryLeaves : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.JournalTransactions", new[] { "journalVoucher_Id" });
            AddColumn("dbo.Accounts", "accountsCategory", c => c.Int(nullable: false));
            AddColumn("dbo.Accounts", "industryTypeId", c => c.Int());
            AddColumn("dbo.Accounts", "vendor_Id", c => c.Int());
            AddColumn("dbo.Leaves", "LeaveDate", c => c.DateTime());
            AddColumn("dbo.ChartofAccounts", "deptId", c => c.Int());
            AddColumn("dbo.ChartofAccounts", "isVoid", c => c.Boolean(nullable: false));
            AddColumn("dbo.JournalVouchers", "company_Id", c => c.Int());
            AddColumn("dbo.JournalVouchers", "dept_Id", c => c.Int());
            AddColumn("dbo.JournalVouchers", "emp_Id", c => c.Int());
            AddColumn("dbo.JournalVouchers", "MER", c => c.Double(nullable: false));
            AddColumn("dbo.IndustryTypes", "isVendorType", c => c.Boolean(nullable: false));
            CreateIndex("dbo.Accounts", "industryTypeId");
            CreateIndex("dbo.Accounts", "vendor_Id");
            CreateIndex("dbo.ChartofAccounts", "deptId");
            CreateIndex("dbo.JournalTransactions", "journalVoucher_id");
            CreateIndex("dbo.JournalVouchers", "company_Id");
            CreateIndex("dbo.JournalVouchers", "dept_Id");
            CreateIndex("dbo.JournalVouchers", "emp_Id");
            AddForeignKey("dbo.ChartofAccounts", "deptId", "dbo.tabDepartment", "Id");
            AddForeignKey("dbo.JournalVouchers", "company_Id", "dbo.tabCompany", "Id");
            AddForeignKey("dbo.JournalVouchers", "dept_Id", "dbo.tabDepartment", "Id");
            AddForeignKey("dbo.JournalVouchers", "emp_Id", "dbo.Employees", "EmpId");
            AddForeignKey("dbo.Accounts", "industryTypeId", "dbo.IndustryTypes", "Id");
            AddForeignKey("dbo.Accounts", "vendor_Id", "dbo.tabVendor", "Id");
            DropColumn("dbo.Accounts", "isPersonal");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Accounts", "isPersonal", c => c.Boolean(nullable: false));
            DropForeignKey("dbo.Accounts", "vendor_Id", "dbo.tabVendor");
            DropForeignKey("dbo.Accounts", "industryTypeId", "dbo.IndustryTypes");
            DropForeignKey("dbo.JournalVouchers", "emp_Id", "dbo.Employees");
            DropForeignKey("dbo.JournalVouchers", "dept_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.JournalVouchers", "company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.ChartofAccounts", "deptId", "dbo.tabDepartment");
            DropIndex("dbo.JournalVouchers", new[] { "emp_Id" });
            DropIndex("dbo.JournalVouchers", new[] { "dept_Id" });
            DropIndex("dbo.JournalVouchers", new[] { "company_Id" });
            DropIndex("dbo.JournalTransactions", new[] { "journalVoucher_id" });
            DropIndex("dbo.ChartofAccounts", new[] { "deptId" });
            DropIndex("dbo.Accounts", new[] { "vendor_Id" });
            DropIndex("dbo.Accounts", new[] { "industryTypeId" });
            DropColumn("dbo.IndustryTypes", "isVendorType");
            DropColumn("dbo.JournalVouchers", "MER");
            DropColumn("dbo.JournalVouchers", "emp_Id");
            DropColumn("dbo.JournalVouchers", "dept_Id");
            DropColumn("dbo.JournalVouchers", "company_Id");
            DropColumn("dbo.ChartofAccounts", "isVoid");
            DropColumn("dbo.ChartofAccounts", "deptId");
            DropColumn("dbo.Leaves", "LeaveDate");
            DropColumn("dbo.Accounts", "vendor_Id");
            DropColumn("dbo.Accounts", "industryTypeId");
            DropColumn("dbo.Accounts", "accountsCategory");
            CreateIndex("dbo.JournalTransactions", "journalVoucher_Id");
        }
    }
}
