namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TargetRewardStatusAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TargetRewardStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.String(),
                        isApproved = c.Boolean(nullable: false),
                        isActive = c.Boolean(),
                        backcolor = c.String(),
                        forecolor = c.String(),
                        HierarchicalIndex = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.TargetRewards", "statusId", c => c.Int());
            AddColumn("dbo.TargetRewards", "isApplied", c => c.Boolean(nullable: false));
            AddColumn("dbo.TargetRewards", "AppliedDate", c => c.DateTime());
            AddColumn("dbo.TargetRewards", "stage", c => c.String());
            AddColumn("dbo.TargetRewards", "isReviewed", c => c.Boolean());
            AddColumn("dbo.TargetRewards", "needReview", c => c.Boolean());
            AddColumn("dbo.TargetRewards", "PendingForClosing", c => c.Boolean());
            AddColumn("dbo.TargetRewards", "PendingForReApproval", c => c.Boolean());
            AddColumn("dbo.TargetRewards", "isApproved", c => c.Boolean());
            AddColumn("dbo.TargetRewards", "ApprovedDate", c => c.DateTime());
            AddColumn("dbo.TargetRewards", "isReApproved", c => c.Boolean());
            AddColumn("dbo.TargetRewards", "ReApprovalDate", c => c.DateTime());
            AddColumn("dbo.TargetRewards", "ClosingDate", c => c.DateTime());
            AddColumn("dbo.TargetRewards", "LastStatusChangeDate", c => c.DateTime());
            AddColumn("dbo.TargetRewards", "creator_Id", c => c.Int());
            CreateIndex("dbo.TargetRewards", "statusId");
            CreateIndex("dbo.TargetRewards", "creator_Id");
            AddForeignKey("dbo.TargetRewards", "creator_Id", "dbo.Users", "id");
            AddForeignKey("dbo.TargetRewards", "statusId", "dbo.TargetRewardStatus", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TargetRewards", "statusId", "dbo.TargetRewardStatus");
            DropForeignKey("dbo.TargetRewards", "creator_Id", "dbo.Users");
            DropIndex("dbo.TargetRewards", new[] { "creator_Id" });
            DropIndex("dbo.TargetRewards", new[] { "statusId" });
            DropColumn("dbo.TargetRewards", "creator_Id");
            DropColumn("dbo.TargetRewards", "LastStatusChangeDate");
            DropColumn("dbo.TargetRewards", "ClosingDate");
            DropColumn("dbo.TargetRewards", "ReApprovalDate");
            DropColumn("dbo.TargetRewards", "isReApproved");
            DropColumn("dbo.TargetRewards", "ApprovedDate");
            DropColumn("dbo.TargetRewards", "isApproved");
            DropColumn("dbo.TargetRewards", "PendingForReApproval");
            DropColumn("dbo.TargetRewards", "PendingForClosing");
            DropColumn("dbo.TargetRewards", "needReview");
            DropColumn("dbo.TargetRewards", "isReviewed");
            DropColumn("dbo.TargetRewards", "stage");
            DropColumn("dbo.TargetRewards", "AppliedDate");
            DropColumn("dbo.TargetRewards", "isApplied");
            DropColumn("dbo.TargetRewards", "statusId");
            DropTable("dbo.TargetRewardStatus");
        }
    }
}
