namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MultiVendorsOfferSoInquirydate : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Offers", "vendor_Id", "dbo.tabVendor");
            DropForeignKey("dbo.SaleOrders", "vendor_Id", "dbo.tabVendor");
            DropIndex("dbo.SaleOrders", new[] { "vendor_Id" });
            DropIndex("dbo.Offers", new[] { "vendor_Id" });
            CreateTable(
                "dbo.OfferVendors",
                c => new
                    {
                        Offer_Id = c.Int(nullable: false),
                        Vendor_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Offer_Id, t.Vendor_Id })
                .ForeignKey("dbo.Offers", t => t.Offer_Id, cascadeDelete: true)
                .ForeignKey("dbo.tabVendor", t => t.Vendor_Id, cascadeDelete: true)
                .Index(t => t.Offer_Id)
                .Index(t => t.Vendor_Id);
            
            CreateTable(
                "dbo.SaleOrderVendors",
                c => new
                    {
                        SaleOrder_Id = c.Int(nullable: false),
                        Vendor_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.SaleOrder_Id, t.Vendor_Id })
                .ForeignKey("dbo.SaleOrders", t => t.SaleOrder_Id, cascadeDelete: true)
                .ForeignKey("dbo.tabVendor", t => t.Vendor_Id, cascadeDelete: true)
                .Index(t => t.SaleOrder_Id)
                .Index(t => t.Vendor_Id);
            
            AddColumn("dbo.Inquiries", "SalesReferenceNo", c => c.String());
            AddColumn("dbo.Inquiries", "CreationDate", c => c.DateTime());
            DropColumn("dbo.SaleOrders", "vendor_Id");
            DropColumn("dbo.Offers", "vendor_Id");
            DropColumn("dbo.Inquiries", "fileNo");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Inquiries", "fileNo", c => c.String());
            AddColumn("dbo.Offers", "vendor_Id", c => c.Int(nullable: false));
            AddColumn("dbo.SaleOrders", "vendor_Id", c => c.Int(nullable: false));
            DropForeignKey("dbo.SaleOrderVendors", "Vendor_Id", "dbo.tabVendor");
            DropForeignKey("dbo.SaleOrderVendors", "SaleOrder_Id", "dbo.SaleOrders");
            DropForeignKey("dbo.OfferVendors", "Vendor_Id", "dbo.tabVendor");
            DropForeignKey("dbo.OfferVendors", "Offer_Id", "dbo.Offers");
            DropIndex("dbo.SaleOrderVendors", new[] { "Vendor_Id" });
            DropIndex("dbo.SaleOrderVendors", new[] { "SaleOrder_Id" });
            DropIndex("dbo.OfferVendors", new[] { "Vendor_Id" });
            DropIndex("dbo.OfferVendors", new[] { "Offer_Id" });
            DropColumn("dbo.Inquiries", "CreationDate");
            DropColumn("dbo.Inquiries", "SalesReferenceNo");
            DropTable("dbo.SaleOrderVendors");
            DropTable("dbo.OfferVendors");
            CreateIndex("dbo.Offers", "vendor_Id");
            CreateIndex("dbo.SaleOrders", "vendor_Id");
            AddForeignKey("dbo.SaleOrders", "vendor_Id", "dbo.tabVendor", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Offers", "vendor_Id", "dbo.tabVendor", "Id", cascadeDelete: true);
        }
    }
}
