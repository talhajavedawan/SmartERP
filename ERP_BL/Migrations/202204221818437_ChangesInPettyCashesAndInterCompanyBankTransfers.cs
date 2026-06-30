namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesInPettyCashesAndInterCompanyBankTransfers : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BillRefNumbers", "isActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.InterBankTransfers", "isPettyCashAmountOC", c => c.Boolean());
            AddColumn("dbo.PettyCashes", "InterCompanyId", c => c.Int());
            AddColumn("dbo.PettyCashes", "InterCompanyBankTransfer_Id", c => c.Int());
            AddColumn("dbo.PettyCashes", "InterCompanyBankTransfer_Id1", c => c.Int());
            AddColumn("dbo.InterCompanyBankTransfers", "isDepositFrom", c => c.Boolean());
            AddColumn("dbo.InterCompanyBankTransfers", "isDepositTo", c => c.Boolean());
            AddColumn("dbo.InterCompanyBankTransfers", "isAmountOCfrom", c => c.Boolean());
            AddColumn("dbo.InterCompanyBankTransfers", "isAmountOCto", c => c.Boolean());
            AddColumn("dbo.VendorBillReferences", "isActive", c => c.Boolean(nullable: false));
            CreateIndex("dbo.PettyCashes", "InterCompanyId");
            CreateIndex("dbo.PettyCashes", "InterCompanyBankTransfer_Id");
            CreateIndex("dbo.PettyCashes", "InterCompanyBankTransfer_Id1");
            AddForeignKey("dbo.PettyCashes", "InterCompanyBankTransfer_Id", "dbo.InterCompanyBankTransfers", "Id");
            AddForeignKey("dbo.PettyCashes", "InterCompanyBankTransfer_Id1", "dbo.InterCompanyBankTransfers", "Id");
            AddForeignKey("dbo.PettyCashes", "InterCompanyId", "dbo.InterCompanyBankTransfers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PettyCashes", "InterCompanyId", "dbo.InterCompanyBankTransfers");
            DropForeignKey("dbo.PettyCashes", "InterCompanyBankTransfer_Id1", "dbo.InterCompanyBankTransfers");
            DropForeignKey("dbo.PettyCashes", "InterCompanyBankTransfer_Id", "dbo.InterCompanyBankTransfers");
            DropIndex("dbo.PettyCashes", new[] { "InterCompanyBankTransfer_Id1" });
            DropIndex("dbo.PettyCashes", new[] { "InterCompanyBankTransfer_Id" });
            DropIndex("dbo.PettyCashes", new[] { "InterCompanyId" });
            DropColumn("dbo.VendorBillReferences", "isActive");
            DropColumn("dbo.InterCompanyBankTransfers", "isAmountOCto");
            DropColumn("dbo.InterCompanyBankTransfers", "isAmountOCfrom");
            DropColumn("dbo.InterCompanyBankTransfers", "isDepositTo");
            DropColumn("dbo.InterCompanyBankTransfers", "isDepositFrom");
            DropColumn("dbo.PettyCashes", "InterCompanyBankTransfer_Id1");
            DropColumn("dbo.PettyCashes", "InterCompanyBankTransfer_Id");
            DropColumn("dbo.PettyCashes", "InterCompanyId");
            DropColumn("dbo.InterBankTransfers", "isPettyCashAmountOC");
            DropColumn("dbo.BillRefNumbers", "isActive");
        }
    }
}
