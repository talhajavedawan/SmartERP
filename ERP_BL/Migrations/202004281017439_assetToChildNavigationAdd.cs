namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class assetToChildNavigationAdd : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Assets", "addressId", "dbo.tabAddress");
            DropForeignKey("dbo.Assets", "purchaseInfoId", "dbo.PurchaseInfoes");
            DropForeignKey("dbo.Buildings", "assetId", "dbo.Assets");
            DropForeignKey("dbo.Lands", "assetId", "dbo.Assets");
            DropIndex("dbo.Assets", new[] { "addressId" });
            DropIndex("dbo.Assets", new[] { "purchaseInfoId" });
            DropIndex("dbo.Buildings", new[] { "assetId" });
            DropIndex("dbo.Lands", new[] { "assetId" });
            RenameColumn(table: "dbo.Assets", name: "addressId", newName: "address_Id");
            RenameColumn(table: "dbo.Assets", name: "owner_EmpId", newName: "OwnerID");
            RenameColumn(table: "dbo.Assets", name: "purchaseInfoId", newName: "purchaseInfo_Id");
            RenameColumn(table: "dbo.Buildings", name: "assetId", newName: "asset_Id");
            RenameColumn(table: "dbo.Buildings", name: "unitId", newName: "measureUnit_Id");
            RenameColumn(table: "dbo.Buildings", name: "mortgeeId", newName: "mortgagee_Id");
            RenameColumn(table: "dbo.Lands", name: "assetId", newName: "asset_Id");
            RenameColumn(table: "dbo.Lands", name: "unitId", newName: "measureUnit_Id");
            RenameColumn(table: "dbo.Lands", name: "mortgeeId", newName: "mortgagee_Id");
            RenameIndex(table: "dbo.Assets", name: "IX_owner_EmpId", newName: "IX_OwnerID");
            RenameIndex(table: "dbo.Buildings", name: "IX_unitId", newName: "IX_measureUnit_Id");
            RenameIndex(table: "dbo.Buildings", name: "IX_mortgeeId", newName: "IX_mortgagee_Id");
            RenameIndex(table: "dbo.Lands", name: "IX_unitId", newName: "IX_measureUnit_Id");
            RenameIndex(table: "dbo.Lands", name: "IX_mortgeeId", newName: "IX_mortgagee_Id");
            //AddColumn("dbo.Assets", "isSubsidary", c => c.Boolean(nullable: false));
            AddColumn("dbo.Assets", "companyId", c => c.Int(nullable: false));
            AlterColumn("dbo.Assets", "address_Id", c => c.Int());
            AlterColumn("dbo.Assets", "purchaseInfo_Id", c => c.Int());
            AlterColumn("dbo.Buildings", "asset_Id", c => c.Int());
            AlterColumn("dbo.Lands", "asset_Id", c => c.Int());
            CreateIndex("dbo.Assets", "companyId");
            CreateIndex("dbo.Assets", "address_Id");
            CreateIndex("dbo.Assets", "purchaseInfo_Id");
            CreateIndex("dbo.Buildings", "asset_Id");
            CreateIndex("dbo.Lands", "asset_Id");
            AddForeignKey("dbo.Assets", "companyId", "dbo.tabCompany", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Assets", "address_Id", "dbo.tabAddress", "Id");
            AddForeignKey("dbo.Assets", "purchaseInfo_Id", "dbo.PurchaseInfoes", "Id");
            AddForeignKey("dbo.Buildings", "asset_Id", "dbo.Assets", "Id");
            AddForeignKey("dbo.Lands", "asset_Id", "dbo.Assets", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Lands", "asset_Id", "dbo.Assets");
            DropForeignKey("dbo.Buildings", "asset_Id", "dbo.Assets");
            DropForeignKey("dbo.Assets", "purchaseInfo_Id", "dbo.PurchaseInfoes");
            DropForeignKey("dbo.Assets", "address_Id", "dbo.tabAddress");
            DropForeignKey("dbo.Assets", "companyId", "dbo.tabCompany");
            DropIndex("dbo.Lands", new[] { "asset_Id" });
            DropIndex("dbo.Buildings", new[] { "asset_Id" });
            DropIndex("dbo.Assets", new[] { "purchaseInfo_Id" });
            DropIndex("dbo.Assets", new[] { "address_Id" });
            DropIndex("dbo.Assets", new[] { "companyId" });
            AlterColumn("dbo.Lands", "asset_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.Buildings", "asset_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.Assets", "purchaseInfo_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.Assets", "address_Id", c => c.Int(nullable: false));
            DropColumn("dbo.Assets", "companyId");
            //DropColumn("dbo.Assets", "isSubsidary");
            RenameIndex(table: "dbo.Lands", name: "IX_mortgagee_Id", newName: "IX_mortgeeId");
            RenameIndex(table: "dbo.Lands", name: "IX_measureUnit_Id", newName: "IX_unitId");
            RenameIndex(table: "dbo.Buildings", name: "IX_mortgagee_Id", newName: "IX_mortgeeId");
            RenameIndex(table: "dbo.Buildings", name: "IX_measureUnit_Id", newName: "IX_unitId");
            RenameIndex(table: "dbo.Assets", name: "IX_OwnerID", newName: "IX_owner_EmpId");
            RenameColumn(table: "dbo.Lands", name: "mortgagee_Id", newName: "mortgeeId");
            RenameColumn(table: "dbo.Lands", name: "measureUnit_Id", newName: "unitId");
            RenameColumn(table: "dbo.Lands", name: "asset_Id", newName: "assetId");
            RenameColumn(table: "dbo.Buildings", name: "mortgagee_Id", newName: "mortgeeId");
            RenameColumn(table: "dbo.Buildings", name: "measureUnit_Id", newName: "unitId");
            RenameColumn(table: "dbo.Buildings", name: "asset_Id", newName: "assetId");
            RenameColumn(table: "dbo.Assets", name: "purchaseInfo_Id", newName: "purchaseInfoId");
            RenameColumn(table: "dbo.Assets", name: "OwnerID", newName: "owner_EmpId");
            RenameColumn(table: "dbo.Assets", name: "address_Id", newName: "addressId");
            CreateIndex("dbo.Lands", "assetId");
            CreateIndex("dbo.Buildings", "assetId");
            CreateIndex("dbo.Assets", "purchaseInfoId");
            CreateIndex("dbo.Assets", "addressId");
            AddForeignKey("dbo.Lands", "assetId", "dbo.Assets", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Buildings", "assetId", "dbo.Assets", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Assets", "purchaseInfoId", "dbo.PurchaseInfoes", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Assets", "addressId", "dbo.tabAddress", "Id", cascadeDelete: true);
        }
    }
}
