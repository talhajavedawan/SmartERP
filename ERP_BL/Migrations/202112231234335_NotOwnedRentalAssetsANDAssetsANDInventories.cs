namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NotOwnedRentalAssetsANDAssetsANDInventories : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.NotOwnedRentalAssets", "AssetNatureId", "dbo.AssetNatures");
            DropForeignKey("dbo.NotOwnedRentalAssets", "OwnerId", "dbo.AssetOwners");
            DropForeignKey("dbo.NotOwnedRentalAssets", "CoOwnerId", "dbo.AssetOwners");
            DropForeignKey("dbo.NotOwnedRentalAssets", "companyId", "dbo.tabCompany");
            DropForeignKey("dbo.NotOwnedRentalAssets", "deptId", "dbo.tabDepartment");
            DropForeignKey("dbo.NotOwnedRentalAssets", "parentId", "dbo.NotOwnedRentalAssets");
            DropForeignKey("dbo.NotOwnedRentalAssetTenancyContracts", "NotOwnedRentalAsset_Id", "dbo.NotOwnedRentalAssets");
            DropForeignKey("dbo.NotOwnedRentalAssetTenancyContracts", "TenancyContract_Id", "dbo.TenancyContracts");
            DropForeignKey("dbo.RentalAssets", "notOwnedAssetId", "dbo.NotOwnedRentalAssets");
            DropForeignKey("dbo.NotOwnedRentalAssets", "RentalAssets_Id", "dbo.RentalAssets");
            DropForeignKey("dbo.RentalReceiveAmounts", "notOwnedAssetId", "dbo.NotOwnedRentalAssets");
            DropIndex("dbo.RentalAssets", new[] { "notOwnedAssetId" });
            DropIndex("dbo.NotOwnedRentalAssets", new[] { "companyId" });
            DropIndex("dbo.NotOwnedRentalAssets", new[] { "deptId" });
            DropIndex("dbo.NotOwnedRentalAssets", new[] { "OwnerId" });
            DropIndex("dbo.NotOwnedRentalAssets", new[] { "CoOwnerId" });
            DropIndex("dbo.NotOwnedRentalAssets", new[] { "parentId" });
            DropIndex("dbo.NotOwnedRentalAssets", new[] { "AssetNatureId" });
            DropIndex("dbo.NotOwnedRentalAssets", new[] { "RentalAssets_Id" });
            DropIndex("dbo.RentalReceiveAmounts", new[] { "notOwnedAssetId" });
            DropIndex("dbo.NotOwnedRentalAssetTenancyContracts", new[] { "NotOwnedRentalAsset_Id" });
            DropIndex("dbo.NotOwnedRentalAssetTenancyContracts", new[] { "TenancyContract_Id" });
            RenameColumn(table: "dbo.Assets", name: "AssetNature_Id", newName: "AssetNatureId");
            RenameColumn(table: "dbo.Assets", name: "managingDept_Id", newName: "deptId");
            RenameIndex(table: "dbo.Assets", name: "IX_AssetNature_Id", newName: "IX_AssetNatureId");
            RenameIndex(table: "dbo.Assets", name: "IX_managingDept_Id", newName: "IX_deptId");
            AddColumn("dbo.Assets", "isOwned", c => c.Boolean(nullable: false));
            AddColumn("dbo.Assets", "CreationDate", c => c.DateTime());
            AddColumn("dbo.Assets", "OwnerId", c => c.Int());
            AddColumn("dbo.Assets", "CoOwnerAssetId", c => c.Int());
            AddColumn("dbo.Inventories", "AverageCost", c => c.Double(nullable: false));
            AddColumn("dbo.Inventories", "AmountOC", c => c.Double(nullable: false));
            AddColumn("dbo.Inventories", "AmountMER", c => c.Double(nullable: false));
            AddColumn("dbo.tabLoan", "creatorId", c => c.Int());
            CreateIndex("dbo.Assets", "OwnerId");
            CreateIndex("dbo.Assets", "CoOwnerAssetId");
            CreateIndex("dbo.tabLoan", "creatorId");
            AddForeignKey("dbo.Assets", "OwnerId", "dbo.AssetOwners", "Id");
            AddForeignKey("dbo.Assets", "CoOwnerAssetId", "dbo.AssetOwners", "Id");
            AddForeignKey("dbo.tabLoan", "creatorId", "dbo.Employees", "EmpId");
            DropColumn("dbo.TenancyContracts", "NotOwnedUnitNames");
            DropColumn("dbo.RentalAssets", "notOwnedAssetId");
            DropColumn("dbo.RentalReceiveAmounts", "notOwnedAssetId");
            DropTable("dbo.NotOwnedRentalAssets");
            DropTable("dbo.NotOwnedRentalAssetTenancyContracts");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.NotOwnedRentalAssetTenancyContracts",
                c => new
                    {
                        NotOwnedRentalAsset_Id = c.Int(nullable: false),
                        TenancyContract_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.NotOwnedRentalAsset_Id, t.TenancyContract_Id });
            
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
                        parentId = c.Int(),
                        AssetNatureId = c.Int(),
                        isRented = c.Boolean(nullable: false),
                        RentalAssets_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.RentalReceiveAmounts", "notOwnedAssetId", c => c.Int());
            AddColumn("dbo.RentalAssets", "notOwnedAssetId", c => c.Int());
            AddColumn("dbo.TenancyContracts", "NotOwnedUnitNames", c => c.String());
            DropForeignKey("dbo.tabLoan", "creatorId", "dbo.Employees");
            DropForeignKey("dbo.Assets", "CoOwnerAssetId", "dbo.AssetOwners");
            DropForeignKey("dbo.Assets", "OwnerId", "dbo.AssetOwners");
            DropIndex("dbo.tabLoan", new[] { "creatorId" });
            DropIndex("dbo.Assets", new[] { "CoOwnerAssetId" });
            DropIndex("dbo.Assets", new[] { "OwnerId" });
            DropColumn("dbo.tabLoan", "creatorId");
            DropColumn("dbo.Inventories", "AmountMER");
            DropColumn("dbo.Inventories", "AmountOC");
            DropColumn("dbo.Inventories", "AverageCost");
            DropColumn("dbo.Assets", "CoOwnerAssetId");
            DropColumn("dbo.Assets", "OwnerId");
            DropColumn("dbo.Assets", "CreationDate");
            DropColumn("dbo.Assets", "isOwned");
            RenameIndex(table: "dbo.Assets", name: "IX_deptId", newName: "IX_managingDept_Id");
            RenameIndex(table: "dbo.Assets", name: "IX_AssetNatureId", newName: "IX_AssetNature_Id");
            RenameColumn(table: "dbo.Assets", name: "deptId", newName: "managingDept_Id");
            RenameColumn(table: "dbo.Assets", name: "AssetNatureId", newName: "AssetNature_Id");
            CreateIndex("dbo.NotOwnedRentalAssetTenancyContracts", "TenancyContract_Id");
            CreateIndex("dbo.NotOwnedRentalAssetTenancyContracts", "NotOwnedRentalAsset_Id");
            CreateIndex("dbo.RentalReceiveAmounts", "notOwnedAssetId");
            CreateIndex("dbo.NotOwnedRentalAssets", "RentalAssets_Id");
            CreateIndex("dbo.NotOwnedRentalAssets", "AssetNatureId");
            CreateIndex("dbo.NotOwnedRentalAssets", "parentId");
            CreateIndex("dbo.NotOwnedRentalAssets", "CoOwnerId");
            CreateIndex("dbo.NotOwnedRentalAssets", "OwnerId");
            CreateIndex("dbo.NotOwnedRentalAssets", "deptId");
            CreateIndex("dbo.NotOwnedRentalAssets", "companyId");
            CreateIndex("dbo.RentalAssets", "notOwnedAssetId");
            AddForeignKey("dbo.RentalReceiveAmounts", "notOwnedAssetId", "dbo.NotOwnedRentalAssets", "Id");
            AddForeignKey("dbo.NotOwnedRentalAssets", "RentalAssets_Id", "dbo.RentalAssets", "Id");
            AddForeignKey("dbo.RentalAssets", "notOwnedAssetId", "dbo.NotOwnedRentalAssets", "Id");
            AddForeignKey("dbo.NotOwnedRentalAssetTenancyContracts", "TenancyContract_Id", "dbo.TenancyContracts", "Id", cascadeDelete: true);
            AddForeignKey("dbo.NotOwnedRentalAssetTenancyContracts", "NotOwnedRentalAsset_Id", "dbo.NotOwnedRentalAssets", "Id", cascadeDelete: true);
            AddForeignKey("dbo.NotOwnedRentalAssets", "parentId", "dbo.NotOwnedRentalAssets", "Id");
            AddForeignKey("dbo.NotOwnedRentalAssets", "deptId", "dbo.tabDepartment", "Id");
            AddForeignKey("dbo.NotOwnedRentalAssets", "companyId", "dbo.tabCompany", "Id");
            AddForeignKey("dbo.NotOwnedRentalAssets", "CoOwnerId", "dbo.AssetOwners", "Id");
            AddForeignKey("dbo.NotOwnedRentalAssets", "OwnerId", "dbo.AssetOwners", "Id");
            AddForeignKey("dbo.NotOwnedRentalAssets", "AssetNatureId", "dbo.AssetNatures", "Id");
        }
    }
}
