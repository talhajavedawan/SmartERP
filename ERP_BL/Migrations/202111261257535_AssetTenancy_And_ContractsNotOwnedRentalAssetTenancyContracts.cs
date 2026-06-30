namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AssetTenancy_And_ContractsNotOwnedRentalAssetTenancyContracts : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.RentalAssets", "unitId", "dbo.Assets");
            DropIndex("dbo.RentalAssets", new[] { "unitId" });
            DropIndex("dbo.NotOwnedRentalAssets", new[] { "ParentId" });
            RenameColumn(table: "dbo.Assets", name: "rentalAssetId", newName: "RentalAssets_Id");
            RenameIndex(table: "dbo.Assets", name: "IX_rentalAssetId", newName: "IX_RentalAssets_Id");
            CreateTable(
                "dbo.NotOwnedRentalAssetTenancyContracts",
                c => new
                    {
                        NotOwnedRentalAsset_Id = c.Int(nullable: false),
                        TenancyContract_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.NotOwnedRentalAsset_Id, t.TenancyContract_Id })
                .ForeignKey("dbo.NotOwnedRentalAssets", t => t.NotOwnedRentalAsset_Id, cascadeDelete: true)
                .ForeignKey("dbo.TenancyContracts", t => t.TenancyContract_Id, cascadeDelete: true)
                .Index(t => t.NotOwnedRentalAsset_Id)
                .Index(t => t.TenancyContract_Id);
            
            CreateTable(
                "dbo.AssetTenancyContracts",
                c => new
                    {
                        Asset_Id = c.Int(nullable: false),
                        TenancyContract_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Asset_Id, t.TenancyContract_Id })
                .ForeignKey("dbo.Assets", t => t.Asset_Id, cascadeDelete: true)
                .ForeignKey("dbo.TenancyContracts", t => t.TenancyContract_Id, cascadeDelete: true)
                .Index(t => t.Asset_Id)
                .Index(t => t.TenancyContract_Id);
            
            AddColumn("dbo.Assets", "isRented", c => c.Boolean(nullable: false));
            AddColumn("dbo.Bills", "hasSummary", c => c.Boolean(nullable: false));
            AddColumn("dbo.Bills", "managementSummary_Id", c => c.Int());
            AddColumn("dbo.Bills", "SummaryMemo", c => c.String());
            AddColumn("dbo.JournalTransactions", "companyId", c => c.Int());
            AddColumn("dbo.JournalTransactions", "currencyId", c => c.Int());
            AddColumn("dbo.RentalAssets", "UnitNames", c => c.String());
            AddColumn("dbo.RentalAssets", "NotOwnedUnitNames", c => c.String());
            AddColumn("dbo.NotOwnedRentalAssets", "isRented", c => c.Boolean(nullable: false));
            AddColumn("dbo.NotOwnedRentalAssets", "RentalAssets_Id", c => c.Int());
            AddColumn("dbo.TenancyContracts", "UnitNames", c => c.String());
            AddColumn("dbo.TenancyContracts", "NotOwnedUnitNames", c => c.String());
            CreateIndex("dbo.Bills", "managementSummary_Id");
            CreateIndex("dbo.JournalTransactions", "companyId");
            CreateIndex("dbo.JournalTransactions", "currencyId");
            CreateIndex("dbo.NotOwnedRentalAssets", "parentId");
            CreateIndex("dbo.NotOwnedRentalAssets", "RentalAssets_Id");
            AddForeignKey("dbo.JournalTransactions", "companyId", "dbo.tabCompany", "Id");
            AddForeignKey("dbo.JournalTransactions", "currencyId", "dbo.Currencies", "Id");
            AddForeignKey("dbo.Bills", "managementSummary_Id", "dbo.ManagementSummaries", "Id");
            AddForeignKey("dbo.NotOwnedRentalAssets", "RentalAssets_Id", "dbo.RentalAssets", "Id");
            DropColumn("dbo.RentalAssets", "isRented");
            DropColumn("dbo.RentalAssets", "unitId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.RentalAssets", "unitId", c => c.Int());
            AddColumn("dbo.RentalAssets", "isRented", c => c.Boolean(nullable: false));
            DropForeignKey("dbo.AssetTenancyContracts", "TenancyContract_Id", "dbo.TenancyContracts");
            DropForeignKey("dbo.AssetTenancyContracts", "Asset_Id", "dbo.Assets");
            DropForeignKey("dbo.NotOwnedRentalAssets", "RentalAssets_Id", "dbo.RentalAssets");
            DropForeignKey("dbo.NotOwnedRentalAssetTenancyContracts", "TenancyContract_Id", "dbo.TenancyContracts");
            DropForeignKey("dbo.NotOwnedRentalAssetTenancyContracts", "NotOwnedRentalAsset_Id", "dbo.NotOwnedRentalAssets");
            DropForeignKey("dbo.Bills", "managementSummary_Id", "dbo.ManagementSummaries");
            DropForeignKey("dbo.JournalTransactions", "currencyId", "dbo.Currencies");
            DropForeignKey("dbo.JournalTransactions", "companyId", "dbo.tabCompany");
            DropIndex("dbo.AssetTenancyContracts", new[] { "TenancyContract_Id" });
            DropIndex("dbo.AssetTenancyContracts", new[] { "Asset_Id" });
            DropIndex("dbo.NotOwnedRentalAssetTenancyContracts", new[] { "TenancyContract_Id" });
            DropIndex("dbo.NotOwnedRentalAssetTenancyContracts", new[] { "NotOwnedRentalAsset_Id" });
            DropIndex("dbo.NotOwnedRentalAssets", new[] { "RentalAssets_Id" });
            DropIndex("dbo.NotOwnedRentalAssets", new[] { "parentId" });
            DropIndex("dbo.JournalTransactions", new[] { "currencyId" });
            DropIndex("dbo.JournalTransactions", new[] { "companyId" });
            DropIndex("dbo.Bills", new[] { "managementSummary_Id" });
            DropColumn("dbo.TenancyContracts", "NotOwnedUnitNames");
            DropColumn("dbo.TenancyContracts", "UnitNames");
            DropColumn("dbo.NotOwnedRentalAssets", "RentalAssets_Id");
            DropColumn("dbo.NotOwnedRentalAssets", "isRented");
            DropColumn("dbo.RentalAssets", "NotOwnedUnitNames");
            DropColumn("dbo.RentalAssets", "UnitNames");
            DropColumn("dbo.JournalTransactions", "currencyId");
            DropColumn("dbo.JournalTransactions", "companyId");
            DropColumn("dbo.Bills", "SummaryMemo");
            DropColumn("dbo.Bills", "managementSummary_Id");
            DropColumn("dbo.Bills", "hasSummary");
            DropColumn("dbo.Assets", "isRented");
            DropTable("dbo.AssetTenancyContracts");
            DropTable("dbo.NotOwnedRentalAssetTenancyContracts");
            RenameIndex(table: "dbo.Assets", name: "IX_RentalAssets_Id", newName: "IX_rentalAssetId");
            RenameColumn(table: "dbo.Assets", name: "RentalAssets_Id", newName: "rentalAssetId");
            CreateIndex("dbo.NotOwnedRentalAssets", "ParentId");
            CreateIndex("dbo.RentalAssets", "unitId");
            AddForeignKey("dbo.RentalAssets", "unitId", "dbo.Assets", "Id");
        }
    }
}
