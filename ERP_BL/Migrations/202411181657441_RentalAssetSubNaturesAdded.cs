namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RentalAssetSubNaturesAdded : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.RentalAssetNatures", "parentId", "dbo.RentalAssetNatures");
            DropIndex("dbo.RentalAssetNatures", new[] { "parentId" });
            CreateTable(
                "dbo.RentalAssetSubNatures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NatureName = c.String(),
                        assetNatureId = c.Int(),
                        isActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RentalAssetNatures", t => t.assetNatureId)
                .Index(t => t.assetNatureId);
            
            AddColumn("dbo.AssetRentals", "assetSubNatureId", c => c.Int());
            AddColumn("dbo.AssetBrands", "assetSubNatureId", c => c.Int());
            AddColumn("dbo.AssetModels", "assetSubNatureId", c => c.Int());
            CreateIndex("dbo.AssetRentals", "assetSubNatureId");
            CreateIndex("dbo.AssetBrands", "assetSubNatureId");
            CreateIndex("dbo.AssetModels", "assetSubNatureId");
            AddForeignKey("dbo.AssetBrands", "assetSubNatureId", "dbo.RentalAssetSubNatures", "Id");
            AddForeignKey("dbo.AssetModels", "assetSubNatureId", "dbo.RentalAssetSubNatures", "Id");
            AddForeignKey("dbo.AssetRentals", "assetSubNatureId", "dbo.RentalAssetSubNatures", "Id");
            DropColumn("dbo.RentalAssetNatures", "isSubsdary");
            DropColumn("dbo.RentalAssetNatures", "parentId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.RentalAssetNatures", "parentId", c => c.Int());
            AddColumn("dbo.RentalAssetNatures", "isSubsdary", c => c.Boolean(nullable: false));
            DropForeignKey("dbo.AssetRentals", "assetSubNatureId", "dbo.RentalAssetSubNatures");
            DropForeignKey("dbo.AssetModels", "assetSubNatureId", "dbo.RentalAssetSubNatures");
            DropForeignKey("dbo.AssetBrands", "assetSubNatureId", "dbo.RentalAssetSubNatures");
            DropForeignKey("dbo.RentalAssetSubNatures", "assetNatureId", "dbo.RentalAssetNatures");
            DropIndex("dbo.AssetModels", new[] { "assetSubNatureId" });
            DropIndex("dbo.RentalAssetSubNatures", new[] { "assetNatureId" });
            DropIndex("dbo.AssetBrands", new[] { "assetSubNatureId" });
            DropIndex("dbo.AssetRentals", new[] { "assetSubNatureId" });
            DropColumn("dbo.AssetModels", "assetSubNatureId");
            DropColumn("dbo.AssetBrands", "assetSubNatureId");
            DropColumn("dbo.AssetRentals", "assetSubNatureId");
            DropTable("dbo.RentalAssetSubNatures");
            CreateIndex("dbo.RentalAssetNatures", "parentId");
            AddForeignKey("dbo.RentalAssetNatures", "parentId", "dbo.RentalAssetNatures", "Id");
        }
    }
}
