namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NotOwnedRentalAssets_RoleFields_Added : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NotOwnedRentalAssets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        creationDate = c.DateTime(nullable: false),
                        isRentable = c.Boolean(nullable: false),
                        AssetName = c.String(),
                        companyId = c.Int(),
                        deptId = c.Int(),
                        OwnerId = c.Int(),
                        CoOwnerId = c.Int(),
                        ParentId = c.Int(),
                        AssetNatureId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AssetNatures", t => t.AssetNatureId)
                .ForeignKey("dbo.AssetOwners", t => t.OwnerId)
                .ForeignKey("dbo.AssetOwners", t => t.CoOwnerId)
                .ForeignKey("dbo.tabCompany", t => t.companyId)
                .ForeignKey("dbo.tabDepartment", t => t.deptId)
                .ForeignKey("dbo.NotOwnedRentalAssets", t => t.ParentId)
                .Index(t => t.companyId)
                .Index(t => t.deptId)
                .Index(t => t.OwnerId)
                .Index(t => t.CoOwnerId)
                .Index(t => t.ParentId)
                .Index(t => t.AssetNatureId);
            
            CreateTable(
                "dbo.RoleFields",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Header = c.String(),
                        FieldName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.AssetOwners", "gender", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.NotOwnedRentalAssets", "ParentId", "dbo.NotOwnedRentalAssets");
            DropForeignKey("dbo.NotOwnedRentalAssets", "deptId", "dbo.tabDepartment");
            DropForeignKey("dbo.NotOwnedRentalAssets", "companyId", "dbo.tabCompany");
            DropForeignKey("dbo.NotOwnedRentalAssets", "CoOwnerId", "dbo.AssetOwners");
            DropForeignKey("dbo.NotOwnedRentalAssets", "OwnerId", "dbo.AssetOwners");
            DropForeignKey("dbo.NotOwnedRentalAssets", "AssetNatureId", "dbo.AssetNatures");
            DropIndex("dbo.NotOwnedRentalAssets", new[] { "AssetNatureId" });
            DropIndex("dbo.NotOwnedRentalAssets", new[] { "ParentId" });
            DropIndex("dbo.NotOwnedRentalAssets", new[] { "CoOwnerId" });
            DropIndex("dbo.NotOwnedRentalAssets", new[] { "OwnerId" });
            DropIndex("dbo.NotOwnedRentalAssets", new[] { "deptId" });
            DropIndex("dbo.NotOwnedRentalAssets", new[] { "companyId" });
            DropColumn("dbo.AssetOwners", "gender");
            DropTable("dbo.RoleFields");
            DropTable("dbo.NotOwnedRentalAssets");
        }
    }
}
