namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RentalReceiveAmountsAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RentalReceiveAmounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        creationDate = c.DateTime(nullable: false),
                        receiveAmount = c.Double(nullable: false),
                        rentalBasis = c.Int(nullable: false),
                        AssetType = c.Int(nullable: false),
                        FinanceRefNo = c.String(),
                        SystemRefNo = c.String(),
                        transactionGroupId = c.Int(nullable: false),
                        company_Id = c.Int(),
                        dept_Id = c.Int(),
                        tenancyContractId = c.Int(),
                        assetId = c.Int(),
                        tenantId = c.Int(),
                        ownerId = c.Int(),
                        notOwnedAssetId = c.Int(),
                        rentalDate = c.DateTime(),
                        rentalAmount = c.Double(nullable: false),
                        isVoid = c.Boolean(nullable: false),
                        isReviewed = c.Boolean(),
                        needReview = c.Boolean(),
                        PendingForClosing = c.Boolean(),
                        PendingForReApproval = c.Boolean(),
                        isApproved = c.Boolean(),
                        ApprovedDate = c.DateTime(),
                        isReApproved = c.Boolean(),
                        ReApprovalDate = c.DateTime(),
                        ClosingDate = c.DateTime(),
                        LastStatusChangeDate = c.DateTime(),
                        rentalAssetStatus_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Assets", t => t.assetId)
                .ForeignKey("dbo.tabCompany", t => t.company_Id)
                .ForeignKey("dbo.tabDepartment", t => t.dept_Id)
                .ForeignKey("dbo.NotOwnedRentalAssets", t => t.notOwnedAssetId)
                .ForeignKey("dbo.AssetOwners", t => t.ownerId)
                .ForeignKey("dbo.RentalAssetStatus", t => t.rentalAssetStatus_Id)
                .ForeignKey("dbo.TenancyContracts", t => t.tenancyContractId)
                .ForeignKey("dbo.Tenants", t => t.tenantId)
                .Index(t => t.company_Id)
                .Index(t => t.dept_Id)
                .Index(t => t.tenancyContractId)
                .Index(t => t.assetId)
                .Index(t => t.tenantId)
                .Index(t => t.ownerId)
                .Index(t => t.notOwnedAssetId)
                .Index(t => t.rentalAssetStatus_Id);
            
            AddColumn("dbo.RentalAssets", "AssetType", c => c.Int(nullable: false));
            AddColumn("dbo.RentalAssets", "notOwnedAssetId", c => c.Int());
            AddColumn("dbo.TenancyContracts", "AssetType", c => c.Int(nullable: false));
            AddColumn("dbo.TenancyContracts", "NotOwnedAssetId", c => c.Int());
            AddColumn("dbo.TenancyContracts", "OwnerId", c => c.Int());
            CreateIndex("dbo.RentalAssets", "notOwnedAssetId");
            CreateIndex("dbo.TenancyContracts", "NotOwnedAssetId");
            CreateIndex("dbo.TenancyContracts", "OwnerId");
            AddForeignKey("dbo.RentalAssets", "notOwnedAssetId", "dbo.NotOwnedRentalAssets", "Id");
            AddForeignKey("dbo.TenancyContracts", "NotOwnedAssetId", "dbo.RentalAssets", "Id");
            AddForeignKey("dbo.TenancyContracts", "OwnerId", "dbo.AssetOwners", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RentalReceiveAmounts", "tenantId", "dbo.Tenants");
            DropForeignKey("dbo.RentalReceiveAmounts", "tenancyContractId", "dbo.TenancyContracts");
            DropForeignKey("dbo.TenancyContracts", "OwnerId", "dbo.AssetOwners");
            DropForeignKey("dbo.TenancyContracts", "NotOwnedAssetId", "dbo.RentalAssets");
            DropForeignKey("dbo.RentalReceiveAmounts", "rentalAssetStatus_Id", "dbo.RentalAssetStatus");
            DropForeignKey("dbo.RentalReceiveAmounts", "ownerId", "dbo.AssetOwners");
            DropForeignKey("dbo.RentalReceiveAmounts", "notOwnedAssetId", "dbo.NotOwnedRentalAssets");
            DropForeignKey("dbo.RentalReceiveAmounts", "dept_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.RentalReceiveAmounts", "company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.RentalReceiveAmounts", "assetId", "dbo.Assets");
            DropForeignKey("dbo.RentalAssets", "notOwnedAssetId", "dbo.NotOwnedRentalAssets");
            DropIndex("dbo.TenancyContracts", new[] { "OwnerId" });
            DropIndex("dbo.TenancyContracts", new[] { "NotOwnedAssetId" });
            DropIndex("dbo.RentalReceiveAmounts", new[] { "rentalAssetStatus_Id" });
            DropIndex("dbo.RentalReceiveAmounts", new[] { "notOwnedAssetId" });
            DropIndex("dbo.RentalReceiveAmounts", new[] { "ownerId" });
            DropIndex("dbo.RentalReceiveAmounts", new[] { "tenantId" });
            DropIndex("dbo.RentalReceiveAmounts", new[] { "assetId" });
            DropIndex("dbo.RentalReceiveAmounts", new[] { "tenancyContractId" });
            DropIndex("dbo.RentalReceiveAmounts", new[] { "dept_Id" });
            DropIndex("dbo.RentalReceiveAmounts", new[] { "company_Id" });
            DropIndex("dbo.RentalAssets", new[] { "notOwnedAssetId" });
            DropColumn("dbo.TenancyContracts", "OwnerId");
            DropColumn("dbo.TenancyContracts", "NotOwnedAssetId");
            DropColumn("dbo.TenancyContracts", "AssetType");
            DropColumn("dbo.RentalAssets", "notOwnedAssetId");
            DropColumn("dbo.RentalAssets", "AssetType");
            DropTable("dbo.RentalReceiveAmounts");
        }
    }
}
