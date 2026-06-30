namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changesInCoaJvPO : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Reconcilations", "chartofAccountId", "dbo.ChartofAccounts");
            DropIndex("dbo.Reconcilations", new[] { "chartofAccountId" });
            AddColumn("dbo.ChartofAccounts", "reconcilationDate", c => c.DateTime());
            AddColumn("dbo.PurchaseOrders", "stlRefNo", c => c.String());
            AddColumn("dbo.JournalTransactions", "isReconciled", c => c.Boolean(nullable: false));
            AddColumn("dbo.JournalVouchers", "purchaseOrder_Id", c => c.Int());
            CreateIndex("dbo.JournalVouchers", "purchaseOrder_Id");
            AddForeignKey("dbo.JournalVouchers", "purchaseOrder_Id", "dbo.PurchaseOrders", "Id");
            DropColumn("dbo.Reconcilations", "chartofAccountId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Reconcilations", "chartofAccountId", c => c.Int());
            DropForeignKey("dbo.JournalVouchers", "purchaseOrder_Id", "dbo.PurchaseOrders");
            DropIndex("dbo.JournalVouchers", new[] { "purchaseOrder_Id" });
            DropColumn("dbo.JournalVouchers", "purchaseOrder_Id");
            DropColumn("dbo.JournalTransactions", "isReconciled");
            DropColumn("dbo.PurchaseOrders", "stlRefNo");
            DropColumn("dbo.ChartofAccounts", "reconcilationDate");
            CreateIndex("dbo.Reconcilations", "chartofAccountId");
            AddForeignKey("dbo.Reconcilations", "chartofAccountId", "dbo.ChartofAccounts", "Id");
        }
    }
}
