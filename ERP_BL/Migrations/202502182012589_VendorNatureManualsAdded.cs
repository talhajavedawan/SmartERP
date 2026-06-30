namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VendorNatureManualsAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.VendorNatureManuals",
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
            
            AddColumn("dbo.tabVendor", "Rating", c => c.Int(nullable: false));
            AddColumn("dbo.tabVendor", "vendorNatureManualId", c => c.Int());
            CreateIndex("dbo.tabVendor", "vendorNatureManualId");
            AddForeignKey("dbo.tabVendor", "vendorNatureManualId", "dbo.VendorNatureManuals", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tabVendor", "vendorNatureManualId", "dbo.VendorNatureManuals");
            DropForeignKey("dbo.VendorNatureManuals", "user_Id", "dbo.Users");
            DropIndex("dbo.VendorNatureManuals", new[] { "user_Id" });
            DropIndex("dbo.tabVendor", new[] { "vendorNatureManualId" });
            DropColumn("dbo.tabVendor", "vendorNatureManualId");
            DropColumn("dbo.tabVendor", "Rating");
            DropTable("dbo.VendorNatureManuals");
        }
    }
}
