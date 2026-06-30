namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changesInRentalReceiveAmountsAndRentalAssets : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.RentalReceiveAmounts", "assetId", "dbo.Assets");
            DropIndex("dbo.RentalAssets", new[] { "RentalAssetStatus_Id" });
            DropIndex("dbo.RentalReceiveAmounts", new[] { "assetId" });
            RenameColumn(table: "dbo.RentalReceiveAmounts", name: "RentalAssetStatus_Id", newName: "statusId");
            RenameIndex(table: "dbo.RentalReceiveAmounts", name: "IX_rentalAssetStatus_Id", newName: "IX_statusId");
            AddColumn("dbo.Products", "company_id", c => c.Int());
            AddColumn("dbo.InterCompanyBankTransfers", "COAdebit_Id", c => c.Int());
            AddColumn("dbo.InterCompanyBankTransfers", "COAcredit_Id", c => c.Int());
            AddColumn("dbo.RentalReceiveAmounts", "Subsidary", c => c.String());
            AddColumn("dbo.RentalReceiveAmounts", "TenantName", c => c.String());
            AddColumn("dbo.RentalReceiveAmounts", "DateFrom", c => c.DateTime());
            AddColumn("dbo.RentalReceiveAmounts", "DateTo", c => c.DateTime());
            AddColumn("dbo.RentalReceiveAmounts", "RentalassetId", c => c.Int());
            CreateIndex("dbo.Products", "company_id");
            CreateIndex("dbo.InterCompanyBankTransfers", "COAdebit_Id");
            CreateIndex("dbo.InterCompanyBankTransfers", "COAcredit_Id");
            CreateIndex("dbo.RentalReceiveAmounts", "RentalassetId");
            AddForeignKey("dbo.Products", "company_id", "dbo.tabCompany", "Id");
            AddForeignKey("dbo.InterCompanyBankTransfers", "COAcredit_Id", "dbo.ChartofAccounts", "Id");
            AddForeignKey("dbo.InterCompanyBankTransfers", "COAdebit_Id", "dbo.ChartofAccounts", "Id");
            AddForeignKey("dbo.RentalReceiveAmounts", "RentalassetId", "dbo.RentalAssets", "Id");
            //DropColumn("dbo.RentalAssets", "RentalAssetStatus_Id");
            DropColumn("dbo.RentalReceiveAmounts", "assetId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.RentalReceiveAmounts", "assetId", c => c.Int());
            //AddColumn("dbo.RentalAssets", "RentalAssetStatus_Id", c => c.Int());
            DropForeignKey("dbo.RentalReceiveAmounts", "RentalassetId", "dbo.RentalAssets");
            DropForeignKey("dbo.InterCompanyBankTransfers", "COAdebit_Id", "dbo.ChartofAccounts");
            DropForeignKey("dbo.InterCompanyBankTransfers", "COAcredit_Id", "dbo.ChartofAccounts");
            DropForeignKey("dbo.Products", "company_id", "dbo.tabCompany");
            DropIndex("dbo.RentalReceiveAmounts", new[] { "RentalassetId" });
            DropIndex("dbo.InterCompanyBankTransfers", new[] { "COAcredit_Id" });
            DropIndex("dbo.InterCompanyBankTransfers", new[] { "COAdebit_Id" });
            DropIndex("dbo.Products", new[] { "company_id" });
            DropColumn("dbo.RentalReceiveAmounts", "RentalassetId");
            DropColumn("dbo.RentalReceiveAmounts", "DateTo");
            DropColumn("dbo.RentalReceiveAmounts", "DateFrom");
            DropColumn("dbo.RentalReceiveAmounts", "TenantName");
            DropColumn("dbo.RentalReceiveAmounts", "Subsidary");
            DropColumn("dbo.InterCompanyBankTransfers", "COAcredit_Id");
            DropColumn("dbo.InterCompanyBankTransfers", "COAdebit_Id");
            DropColumn("dbo.Products", "company_id");
            RenameIndex(table: "dbo.RentalReceiveAmounts", name: "IX_statusId", newName: "IX_rentalAssetStatus_Id");
            RenameColumn(table: "dbo.RentalReceiveAmounts", name: "statusId", newName: "RentalAssetStatus_Id");
            CreateIndex("dbo.RentalReceiveAmounts", "assetId");
            CreateIndex("dbo.RentalAssets", "RentalAssetStatus_Id");
            AddForeignKey("dbo.RentalReceiveAmounts", "assetId", "dbo.Assets", "Id");
        }
    }
}
