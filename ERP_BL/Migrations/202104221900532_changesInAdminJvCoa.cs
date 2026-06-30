namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changesInAdminJvCoa : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ChartofAccounts", "reconcilationId", "dbo.Reconcilations");
            DropIndex("dbo.ChartofAccounts", new[] { "reconcilationId" });
            AddColumn("dbo.Employees", "isAdminBillType", c => c.Boolean(nullable: false));
            AddColumn("dbo.tabAdminBill", "EmployeeForEveryBill", c => c.Boolean(nullable: false));
            AddColumn("dbo.tabAdminBill", "employeeForBill_Id", c => c.Int());
            AddColumn("dbo.JournalVouchers", "bill_Id", c => c.Int());
            AddColumn("dbo.Reconcilations", "chartofAccountId", c => c.Int());
            CreateIndex("dbo.tabAdminBill", "employeeForBill_Id");
            CreateIndex("dbo.JournalVouchers", "bill_Id");
            CreateIndex("dbo.Reconcilations", "chartofAccountId");
            AddForeignKey("dbo.tabAdminBill", "employeeForBill_Id", "dbo.Employees", "EmpId");
            AddForeignKey("dbo.Reconcilations", "chartofAccountId", "dbo.ChartofAccounts", "Id");
            AddForeignKey("dbo.JournalVouchers", "bill_Id", "dbo.Bills", "Id");
            DropColumn("dbo.ChartofAccounts", "reconcilationId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ChartofAccounts", "reconcilationId", c => c.Int());
            DropForeignKey("dbo.JournalVouchers", "bill_Id", "dbo.Bills");
            DropForeignKey("dbo.Reconcilations", "chartofAccountId", "dbo.ChartofAccounts");
            DropForeignKey("dbo.tabAdminBill", "employeeForBill_Id", "dbo.Employees");
            DropIndex("dbo.Reconcilations", new[] { "chartofAccountId" });
            DropIndex("dbo.JournalVouchers", new[] { "bill_Id" });
            DropIndex("dbo.tabAdminBill", new[] { "employeeForBill_Id" });
            DropColumn("dbo.Reconcilations", "chartofAccountId");
            DropColumn("dbo.JournalVouchers", "bill_Id");
            DropColumn("dbo.tabAdminBill", "employeeForBill_Id");
            DropColumn("dbo.tabAdminBill", "EmployeeForEveryBill");
            DropColumn("dbo.Employees", "isAdminBillType");
            CreateIndex("dbo.ChartofAccounts", "reconcilationId");
            AddForeignKey("dbo.ChartofAccounts", "reconcilationId", "dbo.Reconcilations", "Id");
        }
    }
}
