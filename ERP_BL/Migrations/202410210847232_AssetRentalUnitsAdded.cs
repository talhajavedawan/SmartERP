namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AssetRentalUnitsAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AssetRentalUnits",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UnitNo = c.String(),
                        cityId = c.Int(),
                        countryId = c.Int(),
                        assetRentalLocationId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AssetRentalLocations", t => t.assetRentalLocationId)
                .ForeignKey("dbo.Cities", t => t.cityId)
                .ForeignKey("dbo.Countries", t => t.countryId)
                .Index(t => t.cityId)
                .Index(t => t.countryId)
                .Index(t => t.assetRentalLocationId);
            
            AddColumn("dbo.AssetRentals", "assetRentalUnitId", c => c.Int());
            AddColumn("dbo.ModuleContracts", "contractValue", c => c.Double(nullable: false));
            AddColumn("dbo.ModuleContracts", "budgetCost", c => c.Double(nullable: false));
            AddColumn("dbo.ModuleContracts", "budgetMargin", c => c.Double(nullable: false));
            CreateIndex("dbo.AssetRentals", "assetRentalUnitId");
            AddForeignKey("dbo.AssetRentals", "assetRentalUnitId", "dbo.AssetRentalUnits", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AssetRentals", "assetRentalUnitId", "dbo.AssetRentalUnits");
            DropForeignKey("dbo.AssetRentalUnits", "countryId", "dbo.Countries");
            DropForeignKey("dbo.AssetRentalUnits", "cityId", "dbo.Cities");
            DropForeignKey("dbo.AssetRentalUnits", "assetRentalLocationId", "dbo.AssetRentalLocations");
            DropIndex("dbo.AssetRentalUnits", new[] { "assetRentalLocationId" });
            DropIndex("dbo.AssetRentalUnits", new[] { "countryId" });
            DropIndex("dbo.AssetRentalUnits", new[] { "cityId" });
            DropIndex("dbo.AssetRentals", new[] { "assetRentalUnitId" });
            DropColumn("dbo.ModuleContracts", "budgetMargin");
            DropColumn("dbo.ModuleContracts", "budgetCost");
            DropColumn("dbo.ModuleContracts", "contractValue");
            DropColumn("dbo.AssetRentals", "assetRentalUnitId");
            DropTable("dbo.AssetRentalUnits");
        }
    }
}
