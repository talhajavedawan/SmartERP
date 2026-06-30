namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixesAssetsClassesAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Areas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Length = c.Double(nullable: false),
                        width = c.Double(nullable: false),
                        measureUnitType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AssetNatures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NatureName = c.String(),
                        isSubsdary = c.Boolean(nullable: false),
                        parentId = c.Int(),
                        isActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AssetNatures", t => t.parentId)
                .Index(t => t.parentId);
            
            CreateTable(
                "dbo.Assets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AssetName = c.String(),
                        isRentable = c.Boolean(nullable: false),
                        parentId = c.Int(),
                        coOwnerID = c.Int(),
                        mustInsured = c.Boolean(nullable: false),
                        isInsured = c.Boolean(nullable: false),
                        addressId = c.Int(nullable: false),
                        purchaseInfoId = c.Int(nullable: false),
                        AssetNature_Id = c.Int(),
                        designation_DesigId = c.Int(),
                        handler_id = c.Int(),
                        owner_EmpId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tabAddress", t => t.addressId, cascadeDelete: true)
                .ForeignKey("dbo.AssetNatures", t => t.AssetNature_Id)
                .ForeignKey("dbo.Employees", t => t.coOwnerID)
                .ForeignKey("dbo.Designations", t => t.designation_DesigId)
                .ForeignKey("dbo.Users", t => t.handler_id)
                .ForeignKey("dbo.Employees", t => t.owner_EmpId)
                .ForeignKey("dbo.Assets", t => t.parentId)
                .ForeignKey("dbo.PurchaseInfoes", t => t.purchaseInfoId, cascadeDelete: true)
                .Index(t => t.parentId)
                .Index(t => t.coOwnerID)
                .Index(t => t.addressId)
                .Index(t => t.purchaseInfoId)
                .Index(t => t.AssetNature_Id)
                .Index(t => t.designation_DesigId)
                .Index(t => t.handler_id)
                .Index(t => t.owner_EmpId);
            
            CreateTable(
                "dbo.PurchaseInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        acquireAt = c.DateTime(nullable: false),
                        currencyId = c.Int(nullable: false),
                        FA_Amount = c.Double(nullable: false),
                        PER = c.Double(nullable: false),
                        FA_Amount_PER = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Currencies", t => t.currencyId, cascadeDelete: true)
                .Index(t => t.currencyId);
            
            CreateTable(
                "dbo.AuthDocs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DocName = c.String(),
                        isMust = c.Boolean(nullable: false),
                        palcedAt = c.String(),
                        isAttached = c.Boolean(nullable: false),
                        uplaodLocation = c.String(),
                        authId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.OfficialAuths", t => t.authId, cascadeDelete: true)
                .Index(t => t.authId);
            
            CreateTable(
                "dbo.OfficialAuths",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AuthName = c.String(),
                        region_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Regions", t => t.region_Id)
                .Index(t => t.region_Id);
            
            CreateTable(
                "dbo.Regions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        City = c.String(),
                        Country = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Buildings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        assetId = c.Int(nullable: false),
                        unitId = c.Int(),
                        Floors = c.Int(nullable: false),
                        isMortgaged = c.Boolean(nullable: false),
                        mortgeeId = c.Int(),
                        mortgagedValue = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Assets", t => t.assetId, cascadeDelete: true)
                .ForeignKey("dbo.Areas", t => t.unitId)
                .ForeignKey("dbo.Mortgagees", t => t.mortgeeId)
                .Index(t => t.assetId)
                .Index(t => t.unitId)
                .Index(t => t.mortgeeId);
            
            CreateTable(
                "dbo.Mortgagees",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BuyingStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        buyingStatus = c.String(),
                        isNew = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ConditionImages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ImageName = c.String(),
                        location = c.String(),
                        uploadDate = c.DateTime(nullable: false),
                        Vehicle_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Vehicles", t => t.Vehicle_Id)
                .Index(t => t.Vehicle_Id);
            
            CreateTable(
                "dbo.Lands",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        assetId = c.Int(nullable: false),
                        unitId = c.Int(),
                        isMortgaged = c.Boolean(nullable: false),
                        mortgeeId = c.Int(),
                        mortgagedValue = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Assets", t => t.assetId, cascadeDelete: true)
                .ForeignKey("dbo.Areas", t => t.unitId)
                .ForeignKey("dbo.Mortgagees", t => t.mortgeeId)
                .Index(t => t.assetId)
                .Index(t => t.unitId)
                .Index(t => t.mortgeeId);
            
            CreateTable(
                "dbo.Lesees",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LesseName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Manufacturers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ManufacturerName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Vehicles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        assetId = c.Int(nullable: false),
                        Model = c.Int(nullable: false),
                        Chesis = c.String(),
                        EngineNo = c.String(),
                        isNew = c.Boolean(nullable: false),
                        EngineReading = c.Double(nullable: false),
                        LifeTimeToken = c.Boolean(nullable: false),
                        tokenValidTill = c.DateTime(nullable: false),
                        isLeased = c.Boolean(nullable: false),
                        lesseId = c.Int(),
                        leasingValue = c.Double(nullable: false),
                        manufacturer_Id = c.Int(),
                        OfficialAuths_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Assets", t => t.assetId, cascadeDelete: true)
                .ForeignKey("dbo.Lesees", t => t.lesseId)
                .ForeignKey("dbo.Manufacturers", t => t.manufacturer_Id)
                .ForeignKey("dbo.OfficialAuths", t => t.OfficialAuths_Id)
                .Index(t => t.assetId)
                .Index(t => t.lesseId)
                .Index(t => t.manufacturer_Id)
                .Index(t => t.OfficialAuths_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Vehicles", "OfficialAuths_Id", "dbo.OfficialAuths");
            DropForeignKey("dbo.Vehicles", "manufacturer_Id", "dbo.Manufacturers");
            DropForeignKey("dbo.Vehicles", "lesseId", "dbo.Lesees");
            DropForeignKey("dbo.ConditionImages", "Vehicle_Id", "dbo.Vehicles");
            DropForeignKey("dbo.Vehicles", "assetId", "dbo.Assets");
            DropForeignKey("dbo.Lands", "mortgeeId", "dbo.Mortgagees");
            DropForeignKey("dbo.Lands", "unitId", "dbo.Areas");
            DropForeignKey("dbo.Lands", "assetId", "dbo.Assets");
            DropForeignKey("dbo.Buildings", "mortgeeId", "dbo.Mortgagees");
            DropForeignKey("dbo.Buildings", "unitId", "dbo.Areas");
            DropForeignKey("dbo.Buildings", "assetId", "dbo.Assets");
            DropForeignKey("dbo.OfficialAuths", "region_Id", "dbo.Regions");
            DropForeignKey("dbo.AuthDocs", "authId", "dbo.OfficialAuths");
            DropForeignKey("dbo.Assets", "purchaseInfoId", "dbo.PurchaseInfoes");
            DropForeignKey("dbo.PurchaseInfoes", "currencyId", "dbo.Currencies");
            DropForeignKey("dbo.Assets", "parentId", "dbo.Assets");
            DropForeignKey("dbo.Assets", "owner_EmpId", "dbo.Employees");
            DropForeignKey("dbo.Assets", "handler_id", "dbo.Users");
            DropForeignKey("dbo.Assets", "designation_DesigId", "dbo.Designations");
            DropForeignKey("dbo.Assets", "coOwnerID", "dbo.Employees");
            DropForeignKey("dbo.Assets", "AssetNature_Id", "dbo.AssetNatures");
            DropForeignKey("dbo.Assets", "addressId", "dbo.tabAddress");
            DropForeignKey("dbo.AssetNatures", "parentId", "dbo.AssetNatures");
            DropIndex("dbo.Vehicles", new[] { "OfficialAuths_Id" });
            DropIndex("dbo.Vehicles", new[] { "manufacturer_Id" });
            DropIndex("dbo.Vehicles", new[] { "lesseId" });
            DropIndex("dbo.Vehicles", new[] { "assetId" });
            DropIndex("dbo.Lands", new[] { "mortgeeId" });
            DropIndex("dbo.Lands", new[] { "unitId" });
            DropIndex("dbo.Lands", new[] { "assetId" });
            DropIndex("dbo.ConditionImages", new[] { "Vehicle_Id" });
            DropIndex("dbo.Buildings", new[] { "mortgeeId" });
            DropIndex("dbo.Buildings", new[] { "unitId" });
            DropIndex("dbo.Buildings", new[] { "assetId" });
            DropIndex("dbo.OfficialAuths", new[] { "region_Id" });
            DropIndex("dbo.AuthDocs", new[] { "authId" });
            DropIndex("dbo.PurchaseInfoes", new[] { "currencyId" });
            DropIndex("dbo.Assets", new[] { "owner_EmpId" });
            DropIndex("dbo.Assets", new[] { "handler_id" });
            DropIndex("dbo.Assets", new[] { "designation_DesigId" });
            DropIndex("dbo.Assets", new[] { "AssetNature_Id" });
            DropIndex("dbo.Assets", new[] { "purchaseInfoId" });
            DropIndex("dbo.Assets", new[] { "addressId" });
            DropIndex("dbo.Assets", new[] { "coOwnerID" });
            DropIndex("dbo.Assets", new[] { "parentId" });
            DropIndex("dbo.AssetNatures", new[] { "parentId" });
            DropTable("dbo.Vehicles");
            DropTable("dbo.Manufacturers");
            DropTable("dbo.Lesees");
            DropTable("dbo.Lands");
            DropTable("dbo.ConditionImages");
            DropTable("dbo.BuyingStatus");
            DropTable("dbo.Mortgagees");
            DropTable("dbo.Buildings");
            DropTable("dbo.Regions");
            DropTable("dbo.OfficialAuths");
            DropTable("dbo.AuthDocs");
            DropTable("dbo.PurchaseInfoes");
            DropTable("dbo.Assets");
            DropTable("dbo.AssetNatures");
            DropTable("dbo.Areas");
        }
    }
}
