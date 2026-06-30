namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class vendorBillIdInInterbankAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InterBankTransfers", "vendorBillId", c => c.Int());
            AddColumn("dbo.InterBankTransfers", "adminBillGroupId", c => c.Int(nullable: false));
            CreateIndex("dbo.InterBankTransfers", "vendorBillId");
            AddForeignKey("dbo.InterBankTransfers", "vendorBillId", "dbo.Bills", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.InterBankTransfers", "vendorBillId", "dbo.Bills");
            DropIndex("dbo.InterBankTransfers", new[] { "vendorBillId" });
            DropColumn("dbo.InterBankTransfers", "adminBillGroupId");
            DropColumn("dbo.InterBankTransfers", "vendorBillId");
        }
    }
}
