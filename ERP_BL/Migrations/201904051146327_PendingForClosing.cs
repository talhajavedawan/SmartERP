namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PendingForClosing : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Inquiries", "PendingForClosing", c => c.Boolean());
            AddColumn("dbo.Offers", "PendingForClosing", c => c.Boolean());
            AddColumn("dbo.PurchaseOrders", "PendingForClosing", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PurchaseOrders", "PendingForClosing");
            DropColumn("dbo.Offers", "PendingForClosing");
            DropColumn("dbo.Inquiries", "PendingForClosing");
        }
    }
}
