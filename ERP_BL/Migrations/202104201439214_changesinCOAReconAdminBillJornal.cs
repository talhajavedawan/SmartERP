namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changesinCOAReconAdminBillJornal : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Reconcilations", "chartofAccountId", "dbo.ChartofAccounts");
            DropIndex("dbo.Reconcilations", new[] { "chartofAccountId" });
            RenameColumn(table: "dbo.JournalTransactions", name: "Reconcilation_Id", newName: "ReconcilationId");
            RenameIndex(table: "dbo.JournalTransactions", name: "IX_Reconcilation_Id", newName: "IX_ReconcilationId");
            AddColumn("dbo.ChartofAccounts", "reconcilationId", c => c.Int());
            AddColumn("dbo.tabDepartment", "IsManagerial", c => c.Boolean(nullable: false));
            AddColumn("dbo.tabAdminBill", "creatorId", c => c.Int());
            CreateIndex("dbo.ChartofAccounts", "reconcilationId");
            CreateIndex("dbo.tabAdminBill", "creatorId");
            AddForeignKey("dbo.tabAdminBill", "creatorId", "dbo.Employees", "EmpId");
            AddForeignKey("dbo.ChartofAccounts", "reconcilationId", "dbo.Reconcilations", "Id");
            DropColumn("dbo.Reconcilations", "chartofAccountId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Reconcilations", "chartofAccountId", c => c.Int());
            DropForeignKey("dbo.ChartofAccounts", "reconcilationId", "dbo.Reconcilations");
            DropForeignKey("dbo.tabAdminBill", "creatorId", "dbo.Employees");
            DropIndex("dbo.tabAdminBill", new[] { "creatorId" });
            DropIndex("dbo.ChartofAccounts", new[] { "reconcilationId" });
            DropColumn("dbo.tabAdminBill", "creatorId");
            DropColumn("dbo.tabDepartment", "IsManagerial");
            DropColumn("dbo.ChartofAccounts", "reconcilationId");
            RenameIndex(table: "dbo.JournalTransactions", name: "IX_ReconcilationId", newName: "IX_Reconcilation_Id");
            RenameColumn(table: "dbo.JournalTransactions", name: "ReconcilationId", newName: "Reconcilation_Id");
            CreateIndex("dbo.Reconcilations", "chartofAccountId");
            AddForeignKey("dbo.Reconcilations", "chartofAccountId", "dbo.ChartofAccounts", "Id");
        }
    }
}
