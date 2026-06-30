namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class invoiceid : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SalesReceipts", "saleInvoice_Id", "dbo.SaleInvoices");
            DropIndex("dbo.SalesReceipts", new[] { "saleInvoice_Id" });
            RenameColumn(table: "dbo.SalesReceipts", name: "saleInvoice_Id", newName: "saleInvoiceId");
            AlterColumn("dbo.SalesReceipts", "saleInvoiceId", c => c.Int(nullable: false));
            CreateIndex("dbo.SalesReceipts", "saleInvoiceId");
            AddForeignKey("dbo.SalesReceipts", "saleInvoiceId", "dbo.SaleInvoices", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SalesReceipts", "saleInvoiceId", "dbo.SaleInvoices");
            DropIndex("dbo.SalesReceipts", new[] { "saleInvoiceId" });
            AlterColumn("dbo.SalesReceipts", "saleInvoiceId", c => c.Int());
            RenameColumn(table: "dbo.SalesReceipts", name: "saleInvoiceId", newName: "saleInvoice_Id");
            CreateIndex("dbo.SalesReceipts", "saleInvoice_Id");
            AddForeignKey("dbo.SalesReceipts", "saleInvoice_Id", "dbo.SaleInvoices", "Id");
        }
    }
}
