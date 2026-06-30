namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesInSalesReceiptsLoansAdvancesPettyCashes : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SalesReceipts", "saleInvoiceId", "dbo.SaleInvoices");
            DropIndex("dbo.SalesReceipts", new[] { "saleInvoiceId" });
            AddColumn("dbo.Payments", "paymentLoansAdvancesTemplate", c => c.Int());
            AddColumn("dbo.PettyCashes", "LoansAdvanceId", c => c.Int());
            AddColumn("dbo.LoansAdvances", "PettyCashRefId", c => c.Int());
            AddColumn("dbo.LoansAdvances", "isDeposit", c => c.Boolean());
            AlterColumn("dbo.SalesReceipts", "saleInvoiceId", c => c.Int());
            CreateIndex("dbo.PettyCashes", "LoansAdvanceId");
            CreateIndex("dbo.LoansAdvances", "PettyCashRefId");
            CreateIndex("dbo.SalesReceipts", "saleInvoiceId");
            AddForeignKey("dbo.PettyCashes", "LoansAdvanceId", "dbo.LoansAdvances", "Id");
            AddForeignKey("dbo.LoansAdvances", "PettyCashRefId", "dbo.BillRefNumbers", "Id");
            AddForeignKey("dbo.SalesReceipts", "saleInvoiceId", "dbo.SaleInvoices", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SalesReceipts", "saleInvoiceId", "dbo.SaleInvoices");
            DropForeignKey("dbo.LoansAdvances", "PettyCashRefId", "dbo.BillRefNumbers");
            DropForeignKey("dbo.PettyCashes", "LoansAdvanceId", "dbo.LoansAdvances");
            DropIndex("dbo.SalesReceipts", new[] { "saleInvoiceId" });
            DropIndex("dbo.LoansAdvances", new[] { "PettyCashRefId" });
            DropIndex("dbo.PettyCashes", new[] { "LoansAdvanceId" });
            AlterColumn("dbo.SalesReceipts", "saleInvoiceId", c => c.Int(nullable: false));
            DropColumn("dbo.LoansAdvances", "isDeposit");
            DropColumn("dbo.LoansAdvances", "PettyCashRefId");
            DropColumn("dbo.PettyCashes", "LoansAdvanceId");
            DropColumn("dbo.Payments", "paymentLoansAdvancesTemplate");
            CreateIndex("dbo.SalesReceipts", "saleInvoiceId");
            AddForeignKey("dbo.SalesReceipts", "saleInvoiceId", "dbo.SaleInvoices", "Id", cascadeDelete: true);
        }
    }
}
