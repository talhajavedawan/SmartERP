namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cahngesInJornalPayeeTransactions : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JournalTransactions", "taxFlag", c => c.Boolean(nullable: false));
            AddColumn("dbo.Deductions", "chartofAccountId", c => c.Int());
            AddColumn("dbo.Payees", "company_Id", c => c.Int());
            AddColumn("dbo.Payees", "dept_Id", c => c.Int());
            AddColumn("dbo.Templates", "Coa_AccountType", c => c.Int());
            AlterColumn("dbo.Employees", "HireDate", c => c.DateTime());
            AlterColumn("dbo.tabPerson", "DOB", c => c.DateTime());
            AlterColumn("dbo.SalesTargets", "StartingDate", c => c.DateTime());
            AlterColumn("dbo.SalesTargets", "EndDate", c => c.DateTime());
            CreateIndex("dbo.Deductions", "chartofAccountId");
            CreateIndex("dbo.Payees", "company_Id");
            CreateIndex("dbo.Payees", "dept_Id");
            AddForeignKey("dbo.Deductions", "chartofAccountId", "dbo.ChartofAccounts", "Id");
            AddForeignKey("dbo.Payees", "company_Id", "dbo.tabCompany", "Id");
            AddForeignKey("dbo.Payees", "dept_Id", "dbo.tabDepartment", "Id");
            DropColumn("dbo.JournalTransactions", "MER");
            DropColumn("dbo.JournalTransactions", "AmountMER");
        }
        
        public override void Down()
        {
            AddColumn("dbo.JournalTransactions", "AmountMER", c => c.Double());
            AddColumn("dbo.JournalTransactions", "MER", c => c.Double());
            DropForeignKey("dbo.Payees", "dept_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.Payees", "company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.Deductions", "chartofAccountId", "dbo.ChartofAccounts");
            DropIndex("dbo.Payees", new[] { "dept_Id" });
            DropIndex("dbo.Payees", new[] { "company_Id" });
            DropIndex("dbo.Deductions", new[] { "chartofAccountId" });
            AlterColumn("dbo.SalesTargets", "EndDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.SalesTargets", "StartingDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.tabPerson", "DOB", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Employees", "HireDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.Templates", "Coa_AccountType");
            DropColumn("dbo.Payees", "dept_Id");
            DropColumn("dbo.Payees", "company_Id");
            DropColumn("dbo.Deductions", "chartofAccountId");
            DropColumn("dbo.JournalTransactions", "taxFlag");
        }
    }
}
