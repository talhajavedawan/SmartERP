namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BankAndAccountConnectWithSaleInvoices : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SaleInvoices", "bank_Id", c => c.Int());
            AddColumn("dbo.SaleInvoices", "account_Id", c => c.Int());
            CreateIndex("dbo.SaleInvoices", "bank_Id");
            CreateIndex("dbo.SaleInvoices", "account_Id");
            AddForeignKey("dbo.SaleInvoices", "account_Id", "dbo.Accounts", "Id");
            AddForeignKey("dbo.SaleInvoices", "bank_Id", "dbo.Banks", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SaleInvoices", "bank_Id", "dbo.Banks");
            DropForeignKey("dbo.SaleInvoices", "account_Id", "dbo.Accounts");
            DropIndex("dbo.SaleInvoices", new[] { "account_Id" });
            DropIndex("dbo.SaleInvoices", new[] { "bank_Id" });
            DropColumn("dbo.SaleInvoices", "account_Id");
            DropColumn("dbo.SaleInvoices", "bank_Id");
        }
    }
}
