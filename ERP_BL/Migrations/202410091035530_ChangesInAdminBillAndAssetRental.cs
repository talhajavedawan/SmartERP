namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesInAdminBillAndAssetRental : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AssetRentalLocations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LocationTitle = c.String(),
                        cityId = c.Int(),
                        countryId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cities", t => t.cityId)
                .ForeignKey("dbo.Countries", t => t.countryId)
                .Index(t => t.cityId)
                .Index(t => t.countryId);
            
            AddColumn("dbo.AssetRentals", "isLeased", c => c.Boolean(nullable: false));
            AddColumn("dbo.AssetRentals", "countryId", c => c.Int());
            AddColumn("dbo.AssetRentals", "cityId", c => c.Int());
            AddColumn("dbo.AssetRentals", "assetRentalLocationId", c => c.Int());
            CreateIndex("dbo.AssetRentals", "countryId");
            CreateIndex("dbo.AssetRentals", "cityId");
            CreateIndex("dbo.AssetRentals", "assetRentalLocationId");
            AddForeignKey("dbo.AssetRentals", "assetRentalLocationId", "dbo.AssetRentalLocations", "Id");
            AddForeignKey("dbo.AssetRentals", "cityId", "dbo.Cities", "Id");
            AddForeignKey("dbo.AssetRentals", "countryId", "dbo.Countries", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AssetRentals", "countryId", "dbo.Countries");
            DropForeignKey("dbo.AssetRentals", "cityId", "dbo.Cities");
            DropForeignKey("dbo.AssetRentals", "assetRentalLocationId", "dbo.AssetRentalLocations");
            DropForeignKey("dbo.AssetRentalLocations", "countryId", "dbo.Countries");
            DropForeignKey("dbo.AssetRentalLocations", "cityId", "dbo.Cities");
            DropIndex("dbo.AssetRentalLocations", new[] { "countryId" });
            DropIndex("dbo.AssetRentalLocations", new[] { "cityId" });
            DropIndex("dbo.AssetRentals", new[] { "assetRentalLocationId" });
            DropIndex("dbo.AssetRentals", new[] { "cityId" });
            DropIndex("dbo.AssetRentals", new[] { "countryId" });
            DropColumn("dbo.AssetRentals", "assetRentalLocationId");
            DropColumn("dbo.AssetRentals", "cityId");
            DropColumn("dbo.AssetRentals", "countryId");
            DropColumn("dbo.AssetRentals", "isLeased");
            DropTable("dbo.AssetRentalLocations");
        }
    }
}
