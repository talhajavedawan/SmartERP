namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changesInRentalModule09172021 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.RentalAssets", "address_Id", "dbo.tabAddress");
            DropForeignKey("dbo.RentalAssets", "AssetId", "dbo.RentalAssets");
            DropForeignKey("dbo.TenancyContracts", "UnitId", "dbo.RentalAssets");
            DropIndex("dbo.RentalAssets", new[] { "AssetId" });
            DropIndex("dbo.RentalAssets", new[] { "address_Id" });
            DropIndex("dbo.TenancyContracts", new[] { "UnitId" });
            AddColumn("dbo.Assets", "rentalAssetId", c => c.Int());
            AddColumn("dbo.RentalAssets", "CreationDate", c => c.DateTime());
            AddColumn("dbo.RentalAssets", "isRented", c => c.Boolean(nullable: false));
            AddColumn("dbo.RentalAssets", "AssetNatureId", c => c.Int());
            AddColumn("dbo.RentalAssets", "unitId", c => c.Int());
            CreateIndex("dbo.Assets", "rentalAssetId");
            CreateIndex("dbo.RentalAssets", "AssetNatureId");
            CreateIndex("dbo.RentalAssets", "unitId");
            CreateIndex("dbo.RentalAssets", "assetId");
            AddForeignKey("dbo.RentalAssets", "assetId", "dbo.Assets", "Id");
            AddForeignKey("dbo.RentalAssets", "AssetNatureId", "dbo.AssetNatures", "Id");
            AddForeignKey("dbo.RentalAssets", "unitId", "dbo.Assets", "Id");
            AddForeignKey("dbo.Assets", "rentalAssetId", "dbo.RentalAssets", "Id");
            DropColumn("dbo.RentalAssets", "AssetsName");
            DropColumn("dbo.RentalAssets", "isSubsidary");
            DropColumn("dbo.RentalAssets", "rentalType");
            DropColumn("dbo.RentalAssets", "address_Id");
            DropColumn("dbo.TenancyContracts", "UnitId");
            DropColumn("dbo.Tenants", "tenancyType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tenants", "tenancyType", c => c.Int(nullable: false));
            AddColumn("dbo.TenancyContracts", "UnitId", c => c.Int());
            AddColumn("dbo.RentalAssets", "address_Id", c => c.Int());
            AddColumn("dbo.RentalAssets", "rentalType", c => c.Int(nullable: false));
            AddColumn("dbo.RentalAssets", "isSubsidary", c => c.Boolean(nullable: false));
            AddColumn("dbo.RentalAssets", "AssetsName", c => c.String());
            DropForeignKey("dbo.Assets", "rentalAssetId", "dbo.RentalAssets");
            DropForeignKey("dbo.RentalAssets", "unitId", "dbo.Assets");
            DropForeignKey("dbo.RentalAssets", "AssetNatureId", "dbo.AssetNatures");
            DropForeignKey("dbo.RentalAssets", "assetId", "dbo.Assets");
            DropIndex("dbo.RentalAssets", new[] { "assetId" });
            DropIndex("dbo.RentalAssets", new[] { "unitId" });
            DropIndex("dbo.RentalAssets", new[] { "AssetNatureId" });
            DropIndex("dbo.Assets", new[] { "rentalAssetId" });
            DropColumn("dbo.RentalAssets", "unitId");
            DropColumn("dbo.RentalAssets", "AssetNatureId");
            DropColumn("dbo.RentalAssets", "isRented");
            DropColumn("dbo.RentalAssets", "CreationDate");
            DropColumn("dbo.Assets", "rentalAssetId");
            CreateIndex("dbo.TenancyContracts", "UnitId");
            CreateIndex("dbo.RentalAssets", "address_Id");
            CreateIndex("dbo.RentalAssets", "AssetId");
            AddForeignKey("dbo.TenancyContracts", "UnitId", "dbo.RentalAssets", "Id");
            AddForeignKey("dbo.RentalAssets", "AssetId", "dbo.RentalAssets", "Id");
            AddForeignKey("dbo.RentalAssets", "address_Id", "dbo.tabAddress", "Id");
        }
    }
}
