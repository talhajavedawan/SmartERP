namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VendorNaturesAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.VendorNatures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        isApproved = c.Boolean(nullable: false),
                        isActive = c.Boolean(nullable: false),
                        user_Id = c.Int(),
                        addedDate = c.DateTime(nullable: false),
                        isVoid = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.user_Id)
                .Index(t => t.user_Id);
            
            AddColumn("dbo.tabVendor", "isBlackList", c => c.Boolean(nullable: false));
            AddColumn("dbo.tabVendor", "vendorNatureId", c => c.Int());
            AddColumn("dbo.LoansAdvances", "purchaseOrderId", c => c.Int());
            CreateIndex("dbo.tabVendor", "vendorNatureId");
            CreateIndex("dbo.LoansAdvances", "purchaseOrderId");
            AddForeignKey("dbo.LoansAdvances", "purchaseOrderId", "dbo.PurchaseOrders", "Id");
            AddForeignKey("dbo.tabVendor", "vendorNatureId", "dbo.VendorNatures", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tabVendor", "vendorNatureId", "dbo.VendorNatures");
            DropForeignKey("dbo.VendorNatures", "user_Id", "dbo.Users");
            DropForeignKey("dbo.LoansAdvances", "purchaseOrderId", "dbo.PurchaseOrders");
            DropIndex("dbo.VendorNatures", new[] { "user_Id" });
            DropIndex("dbo.LoansAdvances", new[] { "purchaseOrderId" });
            DropIndex("dbo.tabVendor", new[] { "vendorNatureId" });
            DropColumn("dbo.LoansAdvances", "purchaseOrderId");
            DropColumn("dbo.tabVendor", "vendorNatureId");
            DropColumn("dbo.tabVendor", "isBlackList");
            DropTable("dbo.VendorNatures");
        }
    }
}
