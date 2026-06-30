namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class vendorPaymentStatus : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.VendorPaymentStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.String(),
                        isApproved = c.Boolean(nullable: false),
                        isActive = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.PurchaseOrders", "offerReferenceNo", c => c.String());
            AddColumn("dbo.PurchaseOrders", "vendorPaymentId", c => c.Int());
            CreateIndex("dbo.PurchaseOrders", "vendorPaymentId");
            AddForeignKey("dbo.PurchaseOrders", "vendorPaymentId", "dbo.VendorPaymentStatus", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PurchaseOrders", "vendorPaymentId", "dbo.VendorPaymentStatus");
            DropIndex("dbo.PurchaseOrders", new[] { "vendorPaymentId" });
            DropColumn("dbo.PurchaseOrders", "vendorPaymentId");
            DropColumn("dbo.PurchaseOrders", "offerReferenceNo");
            DropTable("dbo.VendorPaymentStatus");
        }
    }
}
