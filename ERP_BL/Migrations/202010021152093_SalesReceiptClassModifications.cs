namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SalesReceiptClassModifications : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SalesReceipts", "isVoid", c => c.Boolean(nullable: false));
            AddColumn("dbo.SalesReceipts", "isReviewed", c => c.Boolean());
            AddColumn("dbo.SalesReceipts", "needReview", c => c.Boolean());
            AddColumn("dbo.SalesReceipts", "PendingForClosing", c => c.Boolean());
            AddColumn("dbo.SalesReceipts", "isApproved", c => c.Boolean());
            AddColumn("dbo.SalesReceipts", "ApprovedDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SalesReceipts", "ApprovedDate");
            DropColumn("dbo.SalesReceipts", "isApproved");
            DropColumn("dbo.SalesReceipts", "PendingForClosing");
            DropColumn("dbo.SalesReceipts", "needReview");
            DropColumn("dbo.SalesReceipts", "isReviewed");
            DropColumn("dbo.SalesReceipts", "isVoid");
        }
    }
}
