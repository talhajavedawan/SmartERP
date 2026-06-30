namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class temp : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Products", "accountReceivable_id", "dbo.ChartofAccounts");
            DropIndex("dbo.Products", new[] { "accountReceivable_id" });
            AddColumn("dbo.Accounts", "COA_Type", c => c.Int());
            AddColumn("dbo.JournalTransactions", "AdminBillId", c => c.Int());
            CreateIndex("dbo.JournalTransactions", "AdminBillId");
            AddForeignKey("dbo.JournalTransactions", "AdminBillId", "dbo.tabAdminBill", "Id");
            DropColumn("dbo.Products", "accountReceivable_id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "accountReceivable_id", c => c.Int());
            DropForeignKey("dbo.JournalTransactions", "AdminBillId", "dbo.tabAdminBill");
            DropIndex("dbo.JournalTransactions", new[] { "AdminBillId" });
            DropColumn("dbo.JournalTransactions", "AdminBillId");
            DropColumn("dbo.Accounts", "COA_Type");
            CreateIndex("dbo.Products", "accountReceivable_id");
            AddForeignKey("dbo.Products", "accountReceivable_id", "dbo.ChartofAccounts", "Id");
        }
    }
}
