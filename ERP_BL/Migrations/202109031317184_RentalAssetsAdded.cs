namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RentalAssetsAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RentalAssetMethods",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MethodName = c.String(),
                        isActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RentalAssets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AssetsName = c.String(),
                        isSubsidary = c.Boolean(nullable: false),
                        rentalType = c.Int(nullable: false),
                        AssetId = c.Int(),
                        address_Id = c.Int(),
                        person_Id = c.Int(),
                        RentalAssetStatus_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tabAddress", t => t.address_Id)
                .ForeignKey("dbo.RentalAssets", t => t.AssetId)
                .ForeignKey("dbo.tabPerson", t => t.person_Id)
                .ForeignKey("dbo.RentalAssetStatus", t => t.RentalAssetStatus_Id)
                .Index(t => t.AssetId)
                .Index(t => t.address_Id)
                .Index(t => t.person_Id)
                .Index(t => t.RentalAssetStatus_Id);
            
            CreateTable(
                "dbo.RentalAssetStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.String(),
                        isApproved = c.Boolean(nullable: false),
                        isActive = c.Boolean(),
                        backcolor = c.String(),
                        forecolor = c.String(),
                        HierarchicalIndex = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tenants",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Profession = c.String(),
                        CreationDate = c.DateTime(nullable: false),
                        tenancyType = c.Int(nullable: false),
                        isActive = c.Boolean(nullable: false),
                        address_Id = c.Int(),
                        contact_Id = c.Int(),
                        person_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tabAddress", t => t.address_Id)
                .ForeignKey("dbo.tabContact", t => t.contact_Id)
                .ForeignKey("dbo.tabPerson", t => t.person_Id)
                .Index(t => t.address_Id)
                .Index(t => t.contact_Id)
                .Index(t => t.person_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tenants", "person_Id", "dbo.tabPerson");
            DropForeignKey("dbo.Tenants", "contact_Id", "dbo.tabContact");
            DropForeignKey("dbo.Tenants", "address_Id", "dbo.tabAddress");
            DropForeignKey("dbo.RentalAssets", "RentalAssetStatus_Id", "dbo.RentalAssetStatus");
            DropForeignKey("dbo.RentalAssets", "person_Id", "dbo.tabPerson");
            DropForeignKey("dbo.RentalAssets", "AssetId", "dbo.RentalAssets");
            DropForeignKey("dbo.RentalAssets", "address_Id", "dbo.tabAddress");
            DropIndex("dbo.Tenants", new[] { "person_Id" });
            DropIndex("dbo.Tenants", new[] { "contact_Id" });
            DropIndex("dbo.Tenants", new[] { "address_Id" });
            DropIndex("dbo.RentalAssets", new[] { "RentalAssetStatus_Id" });
            DropIndex("dbo.RentalAssets", new[] { "person_Id" });
            DropIndex("dbo.RentalAssets", new[] { "address_Id" });
            DropIndex("dbo.RentalAssets", new[] { "AssetId" });
            DropTable("dbo.Tenants");
            DropTable("dbo.RentalAssetStatus");
            DropTable("dbo.RentalAssets");
            DropTable("dbo.RentalAssetMethods");
        }
    }
}
