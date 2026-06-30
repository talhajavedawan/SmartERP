namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changesInPaymentsAndPI : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Payments", "PaymentRefNoId", c => c.Int());
            AddColumn("dbo.PurchaseInvoices", "vendorPaymentId", c => c.Int());
            CreateIndex("dbo.Payments", "PaymentRefNoId");
            CreateIndex("dbo.PurchaseInvoices", "vendorPaymentId");
            AddForeignKey("dbo.Payments", "PaymentRefNoId", "dbo.BillRefNumbers", "Id");
            AddForeignKey("dbo.PurchaseInvoices", "vendorPaymentId", "dbo.VendorPaymentStatus", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PurchaseInvoices", "vendorPaymentId", "dbo.VendorPaymentStatus");
            DropForeignKey("dbo.Payments", "PaymentRefNoId", "dbo.BillRefNumbers");
            DropIndex("dbo.PurchaseInvoices", new[] { "vendorPaymentId" });
            DropIndex("dbo.Payments", new[] { "PaymentRefNoId" });
            DropColumn("dbo.PurchaseInvoices", "vendorPaymentId");
            DropColumn("dbo.Payments", "PaymentRefNoId");
        }
    }
}
