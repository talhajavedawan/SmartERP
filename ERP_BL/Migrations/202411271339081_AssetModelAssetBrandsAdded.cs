namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AssetModelAssetBrandsAdded : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AssetBrands", "assetSubNatureId", "dbo.RentalAssetSubNatures");
            DropForeignKey("dbo.AssetModels", "assetBrandId", "dbo.AssetBrands");
            DropForeignKey("dbo.AssetModels", "assetSubNatureId", "dbo.RentalAssetSubNatures");
            DropIndex("dbo.AssetBrands", new[] { "assetSubNatureId" });
            DropIndex("dbo.AssetModels", new[] { "assetSubNatureId" });
            DropIndex("dbo.AssetModels", new[] { "assetBrandId" });
            CreateTable(
                "dbo.AssetModelAssetBrands",
                c => new
                    {
                        AssetModel_Id = c.Int(nullable: false),
                        AssetBrand_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.AssetModel_Id, t.AssetBrand_Id })
                .ForeignKey("dbo.AssetModels", t => t.AssetModel_Id, cascadeDelete: true)
                .ForeignKey("dbo.AssetBrands", t => t.AssetBrand_Id, cascadeDelete: true)
                .Index(t => t.AssetModel_Id)
                .Index(t => t.AssetBrand_Id);
            
            CreateTable(
                "dbo.AssetModelRentalAssetSubNatures",
                c => new
                    {
                        AssetModel_Id = c.Int(nullable: false),
                        RentalAssetSubNature_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.AssetModel_Id, t.RentalAssetSubNature_Id })
                .ForeignKey("dbo.AssetModels", t => t.AssetModel_Id, cascadeDelete: true)
                .ForeignKey("dbo.RentalAssetSubNatures", t => t.RentalAssetSubNature_Id, cascadeDelete: true)
                .Index(t => t.AssetModel_Id)
                .Index(t => t.RentalAssetSubNature_Id);
            
            CreateTable(
                "dbo.AssetBrandRentalAssetSubNatures",
                c => new
                    {
                        AssetBrand_Id = c.Int(nullable: false),
                        RentalAssetSubNature_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.AssetBrand_Id, t.RentalAssetSubNature_Id })
                .ForeignKey("dbo.AssetBrands", t => t.AssetBrand_Id, cascadeDelete: true)
                .ForeignKey("dbo.RentalAssetSubNatures", t => t.RentalAssetSubNature_Id, cascadeDelete: true)
                .Index(t => t.AssetBrand_Id)
                .Index(t => t.RentalAssetSubNature_Id);
            
            DropColumn("dbo.AssetBrands", "assetSubNatureId");
            DropColumn("dbo.AssetModels", "assetSubNatureId");
            DropColumn("dbo.AssetModels", "assetBrandId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AssetModels", "assetBrandId", c => c.Int());
            AddColumn("dbo.AssetModels", "assetSubNatureId", c => c.Int());
            AddColumn("dbo.AssetBrands", "assetSubNatureId", c => c.Int());
            DropForeignKey("dbo.AssetBrandRentalAssetSubNatures", "RentalAssetSubNature_Id", "dbo.RentalAssetSubNatures");
            DropForeignKey("dbo.AssetBrandRentalAssetSubNatures", "AssetBrand_Id", "dbo.AssetBrands");
            DropForeignKey("dbo.AssetModelRentalAssetSubNatures", "RentalAssetSubNature_Id", "dbo.RentalAssetSubNatures");
            DropForeignKey("dbo.AssetModelRentalAssetSubNatures", "AssetModel_Id", "dbo.AssetModels");
            DropForeignKey("dbo.AssetModelAssetBrands", "AssetBrand_Id", "dbo.AssetBrands");
            DropForeignKey("dbo.AssetModelAssetBrands", "AssetModel_Id", "dbo.AssetModels");
            DropIndex("dbo.AssetBrandRentalAssetSubNatures", new[] { "RentalAssetSubNature_Id" });
            DropIndex("dbo.AssetBrandRentalAssetSubNatures", new[] { "AssetBrand_Id" });
            DropIndex("dbo.AssetModelRentalAssetSubNatures", new[] { "RentalAssetSubNature_Id" });
            DropIndex("dbo.AssetModelRentalAssetSubNatures", new[] { "AssetModel_Id" });
            DropIndex("dbo.AssetModelAssetBrands", new[] { "AssetBrand_Id" });
            DropIndex("dbo.AssetModelAssetBrands", new[] { "AssetModel_Id" });
            DropTable("dbo.AssetBrandRentalAssetSubNatures");
            DropTable("dbo.AssetModelRentalAssetSubNatures");
            DropTable("dbo.AssetModelAssetBrands");
            CreateIndex("dbo.AssetModels", "assetBrandId");
            CreateIndex("dbo.AssetModels", "assetSubNatureId");
            CreateIndex("dbo.AssetBrands", "assetSubNatureId");
            AddForeignKey("dbo.AssetModels", "assetSubNatureId", "dbo.RentalAssetSubNatures", "Id");
            AddForeignKey("dbo.AssetModels", "assetBrandId", "dbo.AssetBrands", "Id");
            AddForeignKey("dbo.AssetBrands", "assetSubNatureId", "dbo.RentalAssetSubNatures", "Id");
        }
    }
}
