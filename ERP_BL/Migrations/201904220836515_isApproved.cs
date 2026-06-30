namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class isApproved : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Inquiries", "ApprovedDate", c => c.DateTime());
            AddColumn("dbo.Inquiries", "isApproved", c => c.Boolean());
            AddColumn("dbo.Offers", "isApproved", c => c.Boolean());
            AddColumn("dbo.Offers", "ApprovedDate", c => c.DateTime());
            AddColumn("dbo.PurchaseOrders", "isApproved", c => c.Boolean());
            AddColumn("dbo.PurchaseOrders", "ApprovedDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PurchaseOrders", "ApprovedDate");
            DropColumn("dbo.PurchaseOrders", "isApproved");
            DropColumn("dbo.Offers", "ApprovedDate");
            DropColumn("dbo.Offers", "isApproved");
            DropColumn("dbo.Inquiries", "isApproved");
            DropColumn("dbo.Inquiries", "ApprovedDate");
        }
    }
}
