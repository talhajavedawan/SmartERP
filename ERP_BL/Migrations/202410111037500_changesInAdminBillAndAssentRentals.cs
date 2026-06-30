namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changesInAdminBillAndAssentRentals : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AssetRentals", "assetHolderId", "dbo.Users");
            DropIndex("dbo.AssetRentals", new[] { "assetHolderId" });
            AddColumn("dbo.tabAdminBill", "billTypes", c => c.Int(nullable: false));
            AddColumn("dbo.tabAdminBill", "isProgressiveCost", c => c.Boolean());
            AddColumn("dbo.AssetRentals", "VendorFromSystem", c => c.Boolean(nullable: false));
            AddColumn("dbo.AssetRentals", "vendorId", c => c.Int());
            AddColumn("dbo.AssetRentals", "assetHolderEmployeeId", c => c.Int());
            CreateIndex("dbo.AssetRentals", "vendorId");
            CreateIndex("dbo.AssetRentals", "assetHolderEmployeeId");
            AddForeignKey("dbo.AssetRentals", "assetHolderEmployeeId", "dbo.Employees", "EmpId");
            AddForeignKey("dbo.AssetRentals", "vendorId", "dbo.tabVendor", "Id");
            DropColumn("dbo.AssetRentals", "assetHolderId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AssetRentals", "assetHolderId", c => c.Int());
            DropForeignKey("dbo.AssetRentals", "vendorId", "dbo.tabVendor");
            DropForeignKey("dbo.AssetRentals", "assetHolderEmployeeId", "dbo.Employees");
            DropIndex("dbo.AssetRentals", new[] { "assetHolderEmployeeId" });
            DropIndex("dbo.AssetRentals", new[] { "vendorId" });
            DropColumn("dbo.AssetRentals", "assetHolderEmployeeId");
            DropColumn("dbo.AssetRentals", "vendorId");
            DropColumn("dbo.AssetRentals", "VendorFromSystem");
            DropColumn("dbo.tabAdminBill", "isProgressiveCost");
            DropColumn("dbo.tabAdminBill", "billTypes");
            CreateIndex("dbo.AssetRentals", "assetHolderId");
            AddForeignKey("dbo.AssetRentals", "assetHolderId", "dbo.Users", "id");
        }
    }
}
