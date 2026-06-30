namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesInInventoriesAndPurchaseOrders : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Inventories", "bookerItemId", c => c.Int());
            AddColumn("dbo.PurchaseOrders", "totalAmount", c => c.Double(nullable: false));
            AddColumn("dbo.PurchaseOrders", "totalAmountGST", c => c.Double(nullable: false));
            AddColumn("dbo.PurchaseOrders", "totalClaimDisount", c => c.Double(nullable: false));
            AddColumn("dbo.PurchaseOrders", "totalPassOn", c => c.Double(nullable: false));
            AddColumn("dbo.PurchaseOrders", "totalFocSampling", c => c.Double(nullable: false));
            AddColumn("dbo.PurchaseOrders", "totalPOAdvance", c => c.Double(nullable: false));
            AddColumn("dbo.PurchaseOrders", "totalPOSattled", c => c.Double(nullable: false));
            AddColumn("dbo.BookerStatementItems", "purchaseOrderId", c => c.Int());
            AddColumn("dbo.BookerStatementItems", "purchaseInvoiceId", c => c.Int());
            CreateIndex("dbo.BookerStatementItems", "purchaseOrderId");
            CreateIndex("dbo.BookerStatementItems", "purchaseInvoiceId");
            CreateIndex("dbo.Inventories", "bookerItemId");
            AddForeignKey("dbo.BookerStatementItems", "purchaseOrderId", "dbo.PurchaseOrders", "Id");
            AddForeignKey("dbo.BookerStatementItems", "purchaseInvoiceId", "dbo.PurchaseInvoices", "Id");
            AddForeignKey("dbo.Inventories", "bookerItemId", "dbo.BookerStatementItems", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Inventories", "bookerItemId", "dbo.BookerStatementItems");
            DropForeignKey("dbo.BookerStatementItems", "purchaseInvoiceId", "dbo.PurchaseInvoices");
            DropForeignKey("dbo.BookerStatementItems", "purchaseOrderId", "dbo.PurchaseOrders");
            DropIndex("dbo.Inventories", new[] { "bookerItemId" });
            DropIndex("dbo.BookerStatementItems", new[] { "purchaseInvoiceId" });
            DropIndex("dbo.BookerStatementItems", new[] { "purchaseOrderId" });
            DropColumn("dbo.BookerStatementItems", "purchaseInvoiceId");
            DropColumn("dbo.BookerStatementItems", "purchaseOrderId");
            DropColumn("dbo.PurchaseOrders", "totalPOSattled");
            DropColumn("dbo.PurchaseOrders", "totalPOAdvance");
            DropColumn("dbo.PurchaseOrders", "totalFocSampling");
            DropColumn("dbo.PurchaseOrders", "totalPassOn");
            DropColumn("dbo.PurchaseOrders", "totalClaimDisount");
            DropColumn("dbo.PurchaseOrders", "totalAmountGST");
            DropColumn("dbo.PurchaseOrders", "totalAmount");
            DropColumn("dbo.Inventories", "bookerItemId");
        }
    }
}
