namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AssetRentalsTablesAdded : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.SplitPERs", name: "SaleOrder_Id", newName: "saleOrderId");
            RenameIndex(table: "dbo.SplitPERs", name: "IX_SaleOrder_Id", newName: "IX_saleOrderId");
            CreateTable(
                "dbo.AssetRentals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreationDate = c.DateTime(),
                        AssetName = c.String(),
                        companyId = c.Int(nullable: false),
                        deptId = c.Int(),
                        isRentable = c.Boolean(nullable: false),
                        isSubsidary = c.Boolean(nullable: false),
                        parentId = c.Int(),
                        adress_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tabAddress", t => t.adress_Id)
                .ForeignKey("dbo.tabCompany", t => t.companyId, cascadeDelete: true)
                .ForeignKey("dbo.tabDepartment", t => t.deptId)
                .ForeignKey("dbo.AssetRentals", t => t.parentId)
                .Index(t => t.companyId)
                .Index(t => t.deptId)
                .Index(t => t.parentId)
                .Index(t => t.adress_Id);
            
            CreateTable(
                "dbo.RentalAssetNatures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NatureName = c.String(),
                        isSubsdary = c.Boolean(nullable: false),
                        parentId = c.Int(),
                        isActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RentalAssetNatures", t => t.parentId)
                .Index(t => t.parentId);
            
            CreateTable(
                "dbo.RentalContracts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        companyId = c.Int(nullable: false),
                        deptId = c.Int(),
                        assetRentalId = c.Int(),
                        isRentable = c.Boolean(nullable: false),
                        isSubsidary = c.Boolean(nullable: false),
                        parentId = c.Int(),
                        TenantRentalId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AssetRentals", t => t.assetRentalId)
                .ForeignKey("dbo.tabCompany", t => t.companyId, cascadeDelete: true)
                .ForeignKey("dbo.tabDepartment", t => t.deptId)
                .ForeignKey("dbo.AssetRentals", t => t.parentId)
                .ForeignKey("dbo.AssetRentals", t => t.TenantRentalId)
                .Index(t => t.companyId)
                .Index(t => t.deptId)
                .Index(t => t.assetRentalId)
                .Index(t => t.parentId)
                .Index(t => t.TenantRentalId);
            
            CreateTable(
                "dbo.RentalPeriodDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        fromDate = c.DateTime(nullable: false),
                        toDate = c.DateTime(nullable: false),
                        RentAmount = c.Double(nullable: false),
                        rentalBasis = c.Int(nullable: false),
                        SecurityDeposit = c.Double(nullable: false),
                        rentalContractId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RentalContracts", t => t.rentalContractId)
                .Index(t => t.rentalContractId);
            
            CreateTable(
                "dbo.TenantRentals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TenantName = c.String(),
                        CreationDate = c.DateTime(),
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
            DropForeignKey("dbo.TenantRentals", "person_Id", "dbo.tabPerson");
            DropForeignKey("dbo.TenantRentals", "contact_Id", "dbo.tabContact");
            DropForeignKey("dbo.TenantRentals", "address_Id", "dbo.tabAddress");
            DropForeignKey("dbo.RentalContracts", "TenantRentalId", "dbo.AssetRentals");
            DropForeignKey("dbo.RentalPeriodDetails", "rentalContractId", "dbo.RentalContracts");
            DropForeignKey("dbo.RentalContracts", "parentId", "dbo.AssetRentals");
            DropForeignKey("dbo.RentalContracts", "deptId", "dbo.tabDepartment");
            DropForeignKey("dbo.RentalContracts", "companyId", "dbo.tabCompany");
            DropForeignKey("dbo.RentalContracts", "assetRentalId", "dbo.AssetRentals");
            DropForeignKey("dbo.RentalAssetNatures", "parentId", "dbo.RentalAssetNatures");
            DropForeignKey("dbo.AssetRentals", "parentId", "dbo.AssetRentals");
            DropForeignKey("dbo.AssetRentals", "deptId", "dbo.tabDepartment");
            DropForeignKey("dbo.AssetRentals", "companyId", "dbo.tabCompany");
            DropForeignKey("dbo.AssetRentals", "adress_Id", "dbo.tabAddress");
            DropIndex("dbo.TenantRentals", new[] { "person_Id" });
            DropIndex("dbo.TenantRentals", new[] { "contact_Id" });
            DropIndex("dbo.TenantRentals", new[] { "address_Id" });
            DropIndex("dbo.RentalPeriodDetails", new[] { "rentalContractId" });
            DropIndex("dbo.RentalContracts", new[] { "TenantRentalId" });
            DropIndex("dbo.RentalContracts", new[] { "parentId" });
            DropIndex("dbo.RentalContracts", new[] { "assetRentalId" });
            DropIndex("dbo.RentalContracts", new[] { "deptId" });
            DropIndex("dbo.RentalContracts", new[] { "companyId" });
            DropIndex("dbo.RentalAssetNatures", new[] { "parentId" });
            DropIndex("dbo.AssetRentals", new[] { "adress_Id" });
            DropIndex("dbo.AssetRentals", new[] { "parentId" });
            DropIndex("dbo.AssetRentals", new[] { "deptId" });
            DropIndex("dbo.AssetRentals", new[] { "companyId" });
            DropTable("dbo.TenantRentals");
            DropTable("dbo.RentalPeriodDetails");
            DropTable("dbo.RentalContracts");
            DropTable("dbo.RentalAssetNatures");
            DropTable("dbo.AssetRentals");
            RenameIndex(table: "dbo.SplitPERs", name: "IX_saleOrderId", newName: "IX_SaleOrder_Id");
            RenameColumn(table: "dbo.SplitPERs", name: "saleOrderId", newName: "SaleOrder_Id");
        }
    }
}
