namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AssetBrandsAndAssetModelsAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AssetBrands",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BrandName = c.String(),
                        assetNatureId = c.Int(),
                        isActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RentalAssetNatures", t => t.assetNatureId)
                .Index(t => t.assetNatureId);
            
            CreateTable(
                "dbo.AssetModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ModelNumber = c.String(),
                        assetNatureId = c.Int(),
                        assetBrandId = c.Int(),
                        isActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AssetBrands", t => t.assetBrandId)
                .ForeignKey("dbo.RentalAssetNatures", t => t.assetNatureId)
                .Index(t => t.assetNatureId)
                .Index(t => t.assetBrandId);
            
            CreateTable(
                "dbo.AssetNumbers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Number = c.String(),
                        isActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.AssetRentals", "assetBrandId", c => c.Int());
            AddColumn("dbo.AssetRentals", "assetModelId", c => c.Int());
            AddColumn("dbo.AssetRentals", "assetNumberId", c => c.Int());
            AddColumn("dbo.AssetRentals", "SerialNumber", c => c.String());
            AddColumn("dbo.AssetRentals", "PurchasingCostManual", c => c.Double(nullable: false));
            AddColumn("dbo.AssetRentals", "ProgressiveCostManual", c => c.Double());
            AddColumn("dbo.AssetRentals", "TotalCostManual", c => c.Double());
            AddColumn("dbo.AssetRentals", "TotalCost", c => c.Double());
            AddColumn("dbo.AssetRentals", "AssetHolder", c => c.String());
            CreateIndex("dbo.AssetRentals", "assetBrandId");
            CreateIndex("dbo.AssetRentals", "assetModelId");
            CreateIndex("dbo.AssetRentals", "assetNumberId");
            AddForeignKey("dbo.AssetRentals", "assetBrandId", "dbo.AssetBrands", "Id");
            AddForeignKey("dbo.AssetRentals", "assetModelId", "dbo.AssetModels", "Id");
            AddForeignKey("dbo.AssetRentals", "assetNumberId", "dbo.AssetNumbers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AssetRentals", "assetNumberId", "dbo.AssetNumbers");
            DropForeignKey("dbo.AssetRentals", "assetModelId", "dbo.AssetModels");
            DropForeignKey("dbo.AssetModels", "assetNatureId", "dbo.RentalAssetNatures");
            DropForeignKey("dbo.AssetModels", "assetBrandId", "dbo.AssetBrands");
            DropForeignKey("dbo.AssetRentals", "assetBrandId", "dbo.AssetBrands");
            DropForeignKey("dbo.AssetBrands", "assetNatureId", "dbo.RentalAssetNatures");
            DropIndex("dbo.AssetModels", new[] { "assetBrandId" });
            DropIndex("dbo.AssetModels", new[] { "assetNatureId" });
            DropIndex("dbo.AssetBrands", new[] { "assetNatureId" });
            DropIndex("dbo.AssetRentals", new[] { "assetNumberId" });
            DropIndex("dbo.AssetRentals", new[] { "assetModelId" });
            DropIndex("dbo.AssetRentals", new[] { "assetBrandId" });
            DropColumn("dbo.AssetRentals", "AssetHolder");
            DropColumn("dbo.AssetRentals", "TotalCost");
            DropColumn("dbo.AssetRentals", "TotalCostManual");
            DropColumn("dbo.AssetRentals", "ProgressiveCostManual");
            DropColumn("dbo.AssetRentals", "PurchasingCostManual");
            DropColumn("dbo.AssetRentals", "SerialNumber");
            DropColumn("dbo.AssetRentals", "assetNumberId");
            DropColumn("dbo.AssetRentals", "assetModelId");
            DropColumn("dbo.AssetRentals", "assetBrandId");
            DropTable("dbo.AssetNumbers");
            DropTable("dbo.AssetModels");
            DropTable("dbo.AssetBrands");
        }
    }
}
