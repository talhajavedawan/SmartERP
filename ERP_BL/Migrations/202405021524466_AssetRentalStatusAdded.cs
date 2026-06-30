namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AssetRentalStatusAdded : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.RentalPeriodDetails", "rentalContractId", "dbo.RentalContracts");
            DropIndex("dbo.RentalPeriodDetails", new[] { "rentalContractId" });
            CreateTable(
                "dbo.AssetRentalStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.String(),
                        isApproved = c.Boolean(nullable: false),
                        isActive = c.Boolean(),
                        backcolor = c.String(),
                        forecolor = c.String(),
                        HierarchicalIndex = c.Int(nullable: false),
                        isDisable = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AssetRentalStatusStatusClasses",
                c => new
                    {
                        AssetRentalStatus_Id = c.Int(nullable: false),
                        StatusClass_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.AssetRentalStatus_Id, t.StatusClass_Id })
                .ForeignKey("dbo.AssetRentalStatus", t => t.AssetRentalStatus_Id, cascadeDelete: true)
                .ForeignKey("dbo.StatusClasses", t => t.StatusClass_Id, cascadeDelete: true)
                .Index(t => t.AssetRentalStatus_Id)
                .Index(t => t.StatusClass_Id);
            
            AddColumn("dbo.RentalContracts", "fromDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.RentalContracts", "toDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.RentalContracts", "RentAmount", c => c.Double(nullable: false));
            AddColumn("dbo.RentalContracts", "NumberOfMonths", c => c.Double(nullable: false));
            AddColumn("dbo.RentalContracts", "TotalRentAmount", c => c.Double(nullable: false));
            AddColumn("dbo.RentalContracts", "rentalBasis", c => c.Int(nullable: false));
            AddColumn("dbo.RentalContracts", "SecurityDeposit", c => c.Double(nullable: false));
            AddColumn("dbo.RentalContracts", "isActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.AssetRentals", "statusId", c => c.Int());
            AddColumn("dbo.TenantRentals", "companyId", c => c.Int());
            AddColumn("dbo.TenantRentals", "deptId", c => c.Int());
            AddColumn("dbo.TenantRentals", "assetRentalId", c => c.Int());
            CreateIndex("dbo.AssetRentals", "statusId");
            CreateIndex("dbo.TenantRentals", "companyId");
            CreateIndex("dbo.TenantRentals", "deptId");
            CreateIndex("dbo.TenantRentals", "assetRentalId");
            AddForeignKey("dbo.AssetRentals", "statusId", "dbo.AssetRentalStatus", "Id");
            AddForeignKey("dbo.TenantRentals", "assetRentalId", "dbo.AssetRentals", "Id");
            AddForeignKey("dbo.TenantRentals", "companyId", "dbo.tabCompany", "Id");
            AddForeignKey("dbo.TenantRentals", "deptId", "dbo.tabDepartment", "Id");
            DropTable("dbo.RentalPeriodDetails");
        }
        
        public override void Down()
        {
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
                        isActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.TenantRentals", "deptId", "dbo.tabDepartment");
            DropForeignKey("dbo.TenantRentals", "companyId", "dbo.tabCompany");
            DropForeignKey("dbo.TenantRentals", "assetRentalId", "dbo.AssetRentals");
            DropForeignKey("dbo.AssetRentalStatusStatusClasses", "StatusClass_Id", "dbo.StatusClasses");
            DropForeignKey("dbo.AssetRentalStatusStatusClasses", "AssetRentalStatus_Id", "dbo.AssetRentalStatus");
            DropForeignKey("dbo.AssetRentals", "statusId", "dbo.AssetRentalStatus");
            DropIndex("dbo.AssetRentalStatusStatusClasses", new[] { "StatusClass_Id" });
            DropIndex("dbo.AssetRentalStatusStatusClasses", new[] { "AssetRentalStatus_Id" });
            DropIndex("dbo.TenantRentals", new[] { "assetRentalId" });
            DropIndex("dbo.TenantRentals", new[] { "deptId" });
            DropIndex("dbo.TenantRentals", new[] { "companyId" });
            DropIndex("dbo.AssetRentals", new[] { "statusId" });
            DropColumn("dbo.TenantRentals", "assetRentalId");
            DropColumn("dbo.TenantRentals", "deptId");
            DropColumn("dbo.TenantRentals", "companyId");
            DropColumn("dbo.AssetRentals", "statusId");
            DropColumn("dbo.RentalContracts", "isActive");
            DropColumn("dbo.RentalContracts", "SecurityDeposit");
            DropColumn("dbo.RentalContracts", "rentalBasis");
            DropColumn("dbo.RentalContracts", "TotalRentAmount");
            DropColumn("dbo.RentalContracts", "NumberOfMonths");
            DropColumn("dbo.RentalContracts", "RentAmount");
            DropColumn("dbo.RentalContracts", "toDate");
            DropColumn("dbo.RentalContracts", "fromDate");
            DropTable("dbo.AssetRentalStatusStatusClasses");
            DropTable("dbo.AssetRentalStatus");
            CreateIndex("dbo.RentalPeriodDetails", "rentalContractId");
            AddForeignKey("dbo.RentalPeriodDetails", "rentalContractId", "dbo.RentalContracts", "Id");
        }
    }
}
