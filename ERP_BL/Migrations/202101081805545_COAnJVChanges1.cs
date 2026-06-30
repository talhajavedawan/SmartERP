namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class COAnJVChanges1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Accounts", "COA_accountId", c => c.Int());
            AddColumn("dbo.Accounts", "isPersonal", c => c.Boolean(nullable: false));
            AddColumn("dbo.ChartofAccounts", "asOfDate", c => c.DateTime());
            AddColumn("dbo.JournalTransactions", "InterBankId", c => c.Int());
            AddColumn("dbo.JournalVouchers", "currencyId", c => c.Int());
            CreateIndex("dbo.Accounts", "COA_accountId");
            CreateIndex("dbo.JournalTransactions", "InterBankId");
            CreateIndex("dbo.JournalVouchers", "currencyId");
            AddForeignKey("dbo.Accounts", "COA_accountId", "dbo.ChartofAccounts", "Id");
            AddForeignKey("dbo.JournalTransactions", "InterBankId", "dbo.InterBankTransfers", "Id");
            AddForeignKey("dbo.JournalVouchers", "currencyId", "dbo.Currencies", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.JournalVouchers", "currencyId", "dbo.Currencies");
            DropForeignKey("dbo.JournalTransactions", "InterBankId", "dbo.InterBankTransfers");
            DropForeignKey("dbo.Accounts", "COA_accountId", "dbo.ChartofAccounts");
            DropIndex("dbo.JournalVouchers", new[] { "currencyId" });
            DropIndex("dbo.JournalTransactions", new[] { "InterBankId" });
            DropIndex("dbo.Accounts", new[] { "COA_accountId" });
            DropColumn("dbo.JournalVouchers", "currencyId");
            DropColumn("dbo.JournalTransactions", "InterBankId");
            DropColumn("dbo.ChartofAccounts", "asOfDate");
            DropColumn("dbo.Accounts", "isPersonal");
            DropColumn("dbo.Accounts", "COA_accountId");
        }
    }
}
