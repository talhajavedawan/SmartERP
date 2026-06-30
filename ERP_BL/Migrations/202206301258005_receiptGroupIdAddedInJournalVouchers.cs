namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class receiptGroupIdAddedInJournalVouchers : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JournalVouchers", "receiptGroupId", c => c.Int(nullable: false));
            AddColumn("dbo.InterBankTransfers", "receiptGroupId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.InterBankTransfers", "receiptGroupId");
            DropColumn("dbo.JournalVouchers", "receiptGroupId");
        }
    }
}
