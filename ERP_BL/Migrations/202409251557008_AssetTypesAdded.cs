namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AssetTypesAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AssetTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TypeName = c.String(),
                        isActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.tabAdminBill", "assetRentalId", c => c.Int());
            AddColumn("dbo.AssetRentals", "AssetNatureId", c => c.Int());
            AddColumn("dbo.AssetRentals", "assetTypeId", c => c.Int());
            AddColumn("dbo.AssetRentals", "Vendor", c => c.String());
            AddColumn("dbo.AssetRentals", "AssetNumber", c => c.String());
            AddColumn("dbo.AssetRentals", "Description", c => c.String());
            AddColumn("dbo.AssetRentals", "PurchasingCost", c => c.Double(nullable: false));
            AddColumn("dbo.AssetRentals", "ProgressiveCost", c => c.String());
            AddColumn("dbo.AssetRentals", "assetHolderId", c => c.Int());
            CreateIndex("dbo.tabAdminBill", "assetRentalId");
            CreateIndex("dbo.AssetRentals", "AssetNatureId");
            CreateIndex("dbo.AssetRentals", "assetTypeId");
            CreateIndex("dbo.AssetRentals", "assetHolderId");
            AddForeignKey("dbo.tabAdminBill", "assetRentalId", "dbo.AssetRentals", "Id");
            AddForeignKey("dbo.AssetRentals", "assetHolderId", "dbo.Users", "id");
            AddForeignKey("dbo.AssetRentals", "AssetNatureId", "dbo.RentalAssetNatures", "Id");
            AddForeignKey("dbo.AssetRentals", "assetTypeId", "dbo.AssetTypes", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AssetRentals", "assetTypeId", "dbo.AssetTypes");
            DropForeignKey("dbo.AssetRentals", "AssetNatureId", "dbo.RentalAssetNatures");
            DropForeignKey("dbo.AssetRentals", "assetHolderId", "dbo.Users");
            DropForeignKey("dbo.tabAdminBill", "assetRentalId", "dbo.AssetRentals");
            DropIndex("dbo.AssetRentals", new[] { "assetHolderId" });
            DropIndex("dbo.AssetRentals", new[] { "assetTypeId" });
            DropIndex("dbo.AssetRentals", new[] { "AssetNatureId" });
            DropIndex("dbo.tabAdminBill", new[] { "assetRentalId" });
            DropColumn("dbo.AssetRentals", "assetHolderId");
            DropColumn("dbo.AssetRentals", "ProgressiveCost");
            DropColumn("dbo.AssetRentals", "PurchasingCost");
            DropColumn("dbo.AssetRentals", "Description");
            DropColumn("dbo.AssetRentals", "AssetNumber");
            DropColumn("dbo.AssetRentals", "Vendor");
            DropColumn("dbo.AssetRentals", "assetTypeId");
            DropColumn("dbo.AssetRentals", "AssetNatureId");
            DropColumn("dbo.tabAdminBill", "assetRentalId");
            DropTable("dbo.AssetTypes");
        }
    }
}
