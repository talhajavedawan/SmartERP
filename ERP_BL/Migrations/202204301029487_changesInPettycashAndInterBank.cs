namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changesInPettycashAndInterBank : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tabAdminBill", "isDeposit", c => c.Boolean());
            AddColumn("dbo.tabAdminBill", "isAmountOC", c => c.Boolean());
            AddColumn("dbo.Payments", "isDeposit", c => c.Boolean());
            AddColumn("dbo.InterBankTransfers", "PettyCashRefId", c => c.Int());
            AddColumn("dbo.PettyCashes", "PaymentId", c => c.Int());
            AddColumn("dbo.PettyCashes", "AdminBillId", c => c.Int());
            AddColumn("dbo.PettyCashes", "PettyCashRefId", c => c.Int());
            AddColumn("dbo.InterCompanyBankTransfers", "PettyCashRefFromId", c => c.Int());
            AddColumn("dbo.InterCompanyBankTransfers", "PettyCashRefToId", c => c.Int());
            AlterColumn("dbo.tabPerson", "CNIC", c => c.String());
            CreateIndex("dbo.PettyCashes", "PaymentId");
            CreateIndex("dbo.PettyCashes", "AdminBillId");
            CreateIndex("dbo.PettyCashes", "PettyCashRefId");
            CreateIndex("dbo.InterBankTransfers", "PettyCashRefId");
            CreateIndex("dbo.InterCompanyBankTransfers", "PettyCashRefFromId");
            CreateIndex("dbo.InterCompanyBankTransfers", "PettyCashRefToId");
            AddForeignKey("dbo.PettyCashes", "AdminBillId", "dbo.tabAdminBill", "Id");
            AddForeignKey("dbo.InterBankTransfers", "PettyCashRefId", "dbo.BillRefNumbers", "Id");
            AddForeignKey("dbo.InterCompanyBankTransfers", "PettyCashRefFromId", "dbo.BillRefNumbers", "Id");
            AddForeignKey("dbo.InterCompanyBankTransfers", "PettyCashRefToId", "dbo.BillRefNumbers", "Id");
            AddForeignKey("dbo.PettyCashes", "PaymentId", "dbo.Payments", "Id");
            AddForeignKey("dbo.PettyCashes", "PettyCashRefId", "dbo.BillRefNumbers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PettyCashes", "PettyCashRefId", "dbo.BillRefNumbers");
            DropForeignKey("dbo.PettyCashes", "PaymentId", "dbo.Payments");
            DropForeignKey("dbo.InterCompanyBankTransfers", "PettyCashRefToId", "dbo.BillRefNumbers");
            DropForeignKey("dbo.InterCompanyBankTransfers", "PettyCashRefFromId", "dbo.BillRefNumbers");
            DropForeignKey("dbo.InterBankTransfers", "PettyCashRefId", "dbo.BillRefNumbers");
            DropForeignKey("dbo.PettyCashes", "AdminBillId", "dbo.tabAdminBill");
            DropIndex("dbo.InterCompanyBankTransfers", new[] { "PettyCashRefToId" });
            DropIndex("dbo.InterCompanyBankTransfers", new[] { "PettyCashRefFromId" });
            DropIndex("dbo.InterBankTransfers", new[] { "PettyCashRefId" });
            DropIndex("dbo.PettyCashes", new[] { "PettyCashRefId" });
            DropIndex("dbo.PettyCashes", new[] { "AdminBillId" });
            DropIndex("dbo.PettyCashes", new[] { "PaymentId" });
            AlterColumn("dbo.tabPerson", "CNIC", c => c.String(nullable: false));
            DropColumn("dbo.InterCompanyBankTransfers", "PettyCashRefToId");
            DropColumn("dbo.InterCompanyBankTransfers", "PettyCashRefFromId");
            DropColumn("dbo.PettyCashes", "PettyCashRefId");
            DropColumn("dbo.PettyCashes", "AdminBillId");
            DropColumn("dbo.PettyCashes", "PaymentId");
            DropColumn("dbo.InterBankTransfers", "PettyCashRefId");
            DropColumn("dbo.Payments", "isDeposit");
            DropColumn("dbo.tabAdminBill", "isAmountOC");
            DropColumn("dbo.tabAdminBill", "isDeposit");
        }
    }
}
