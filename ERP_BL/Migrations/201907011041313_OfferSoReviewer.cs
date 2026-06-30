namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OfferSoReviewer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SaleOrders", "isReviewed", c => c.Boolean());
            AddColumn("dbo.SaleOrders", "needReview", c => c.Boolean());
            AddColumn("dbo.SaleOrders", "stage", c => c.String());
            AddColumn("dbo.Offers", "isReviewed", c => c.Boolean());
            AddColumn("dbo.Offers", "needReview", c => c.Boolean());
            AddColumn("dbo.Offers", "stage", c => c.String());
            AddColumn("dbo.Inquiries", "stage", c => c.String());
            AddColumn("dbo.ViewInfoes", "Comment", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ViewInfoes", "Comment");
            DropColumn("dbo.Inquiries", "stage");
            DropColumn("dbo.Offers", "stage");
            DropColumn("dbo.Offers", "needReview");
            DropColumn("dbo.Offers", "isReviewed");
            DropColumn("dbo.SaleOrders", "stage");
            DropColumn("dbo.SaleOrders", "needReview");
            DropColumn("dbo.SaleOrders", "isReviewed");
        }
    }
}
