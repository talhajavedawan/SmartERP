namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InterBankTransfersAndadminBillIdConnected : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InterBankTransfers", "adminBillId", c => c.Int());
            CreateIndex("dbo.InterBankTransfers", "adminBillId");
            AddForeignKey("dbo.InterBankTransfers", "adminBillId", "dbo.tabAdminBill", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.InterBankTransfers", "adminBillId", "dbo.tabAdminBill");
            DropIndex("dbo.InterBankTransfers", new[] { "adminBillId" });
            DropColumn("dbo.InterBankTransfers", "adminBillId");
        }
    }
}
