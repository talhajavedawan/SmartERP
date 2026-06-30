namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApprovalFlagsAddedInTasks : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tasks", "isReviewed", c => c.Boolean());
            AddColumn("dbo.Tasks", "needReview", c => c.Boolean());
            AddColumn("dbo.Tasks", "PendingForReApproval", c => c.Boolean());
            AddColumn("dbo.Tasks", "isApproved", c => c.Boolean());
            AddColumn("dbo.Tasks", "ApprovedDate", c => c.DateTime());
            AddColumn("dbo.Tasks", "isReApproved", c => c.Boolean());
            AddColumn("dbo.Tasks", "ReApprovalDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tasks", "ReApprovalDate");
            DropColumn("dbo.Tasks", "isReApproved");
            DropColumn("dbo.Tasks", "ApprovedDate");
            DropColumn("dbo.Tasks", "isApproved");
            DropColumn("dbo.Tasks", "PendingForReApproval");
            DropColumn("dbo.Tasks", "needReview");
            DropColumn("dbo.Tasks", "isReviewed");
        }
    }
}
