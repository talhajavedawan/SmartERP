namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class paymentGroupIdAddedInJV : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JournalVouchers", "paymentGroupId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.JournalVouchers", "paymentGroupId");
        }
    }
}
