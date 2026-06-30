namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesInSalesReceipts1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SalesReceipts", "saleOrderId", "dbo.SaleOrders");
            DropForeignKey("dbo.SalesReceipts", "purchaseOrderId", "dbo.PurchaseOrders");
            DropIndex("dbo.SalesReceipts", new[] { "saleOrderId" });
            DropIndex("dbo.SalesReceipts", new[] { "purchaseOrderId" });
            AddColumn("dbo.Accounts", "isAdjustmentAccount", c => c.Boolean(nullable: false));
            AddColumn("dbo.SalesReceipts", "receiptId", c => c.Int());
            AddColumn("dbo.SalesReceipts", "paymentId", c => c.Int());
            CreateIndex("dbo.SalesReceipts", "receiptId");
            CreateIndex("dbo.SalesReceipts", "paymentId");
            AddForeignKey("dbo.SalesReceipts", "receiptId", "dbo.SalesReceipts", "Id");
            AddForeignKey("dbo.SalesReceipts", "paymentId", "dbo.Payments", "Id");
            DropColumn("dbo.SalesReceipts", "saleOrderId");
            DropColumn("dbo.SalesReceipts", "purchaseOrderId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SalesReceipts", "purchaseOrderId", c => c.Int());
            AddColumn("dbo.SalesReceipts", "saleOrderId", c => c.Int());
            DropForeignKey("dbo.SalesReceipts", "paymentId", "dbo.Payments");
            DropForeignKey("dbo.SalesReceipts", "receiptId", "dbo.SalesReceipts");
            DropIndex("dbo.SalesReceipts", new[] { "paymentId" });
            DropIndex("dbo.SalesReceipts", new[] { "receiptId" });
            DropColumn("dbo.SalesReceipts", "paymentId");
            DropColumn("dbo.SalesReceipts", "receiptId");
            DropColumn("dbo.Accounts", "isAdjustmentAccount");
            CreateIndex("dbo.SalesReceipts", "purchaseOrderId");
            CreateIndex("dbo.SalesReceipts", "saleOrderId");
            AddForeignKey("dbo.SalesReceipts", "purchaseOrderId", "dbo.PurchaseOrders", "Id");
            AddForeignKey("dbo.SalesReceipts", "saleOrderId", "dbo.SaleOrders", "Id");
        }
    }
}
