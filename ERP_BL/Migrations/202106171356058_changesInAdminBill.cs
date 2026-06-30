namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changesInAdminBill : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tabAdminBill", "RemainingAmountOC", c => c.Double(nullable: false));
            DropColumn("dbo.Payments", "AmountDue");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Payments", "AmountDue", c => c.Double(nullable: false));
            DropColumn("dbo.tabAdminBill", "RemainingAmountOC");
        }
    }
}
