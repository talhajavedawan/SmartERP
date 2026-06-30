namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sopo1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.tabVendor", "PurchaseOrder_Id", "dbo.PurchaseOrders");
            DropIndex("dbo.tabVendor", new[] { "PurchaseOrder_Id" });
            CreateTable(
                "dbo.PurchaseOrderVendors",
                c => new
                    {
                        PurchaseOrder_Id = c.Int(nullable: false),
                        Vendor_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.PurchaseOrder_Id, t.Vendor_Id })
                .ForeignKey("dbo.PurchaseOrders", t => t.PurchaseOrder_Id, cascadeDelete: true)
                .ForeignKey("dbo.tabVendor", t => t.Vendor_Id, cascadeDelete: true)
                .Index(t => t.PurchaseOrder_Id)
                .Index(t => t.Vendor_Id);
            
            DropColumn("dbo.tabVendor", "PurchaseOrder_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tabVendor", "PurchaseOrder_Id", c => c.Int());
            DropForeignKey("dbo.PurchaseOrderVendors", "Vendor_Id", "dbo.tabVendor");
            DropForeignKey("dbo.PurchaseOrderVendors", "PurchaseOrder_Id", "dbo.PurchaseOrders");
            DropIndex("dbo.PurchaseOrderVendors", new[] { "Vendor_Id" });
            DropIndex("dbo.PurchaseOrderVendors", new[] { "PurchaseOrder_Id" });
            DropTable("dbo.PurchaseOrderVendors");
            CreateIndex("dbo.tabVendor", "PurchaseOrder_Id");
            AddForeignKey("dbo.tabVendor", "PurchaseOrder_Id", "dbo.PurchaseOrders", "Id");
        }
    }
}
