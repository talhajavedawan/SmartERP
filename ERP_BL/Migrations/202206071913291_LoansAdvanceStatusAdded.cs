namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LoansAdvanceStatusAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LoansAdvanceStatus",
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
            
            AddColumn("dbo.LoansAdvances", "statusId", c => c.Int());
            AddColumn("dbo.LoansAdvances", "stage", c => c.String());
            AddColumn("dbo.LoansAdvances", "isVoid", c => c.Boolean(nullable: false));
            AddColumn("dbo.LoansAdvances", "isReviewed", c => c.Boolean());
            AddColumn("dbo.LoansAdvances", "needReview", c => c.Boolean());
            AddColumn("dbo.LoansAdvances", "PendingForClosing", c => c.Boolean());
            AddColumn("dbo.LoansAdvances", "PendingForReApproval", c => c.Boolean());
            AddColumn("dbo.LoansAdvances", "isApproved", c => c.Boolean());
            AddColumn("dbo.LoansAdvances", "ApprovedDate", c => c.DateTime());
            AddColumn("dbo.LoansAdvances", "isReApproved", c => c.Boolean());
            AddColumn("dbo.LoansAdvances", "ReApprovalDate", c => c.DateTime());
            CreateIndex("dbo.LoansAdvances", "statusId");
            AddForeignKey("dbo.LoansAdvances", "statusId", "dbo.LoansAdvanceStatus", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LoansAdvances", "statusId", "dbo.LoansAdvanceStatus");
            DropIndex("dbo.LoansAdvances", new[] { "statusId" });
            DropColumn("dbo.LoansAdvances", "ReApprovalDate");
            DropColumn("dbo.LoansAdvances", "isReApproved");
            DropColumn("dbo.LoansAdvances", "ApprovedDate");
            DropColumn("dbo.LoansAdvances", "isApproved");
            DropColumn("dbo.LoansAdvances", "PendingForReApproval");
            DropColumn("dbo.LoansAdvances", "PendingForClosing");
            DropColumn("dbo.LoansAdvances", "needReview");
            DropColumn("dbo.LoansAdvances", "isReviewed");
            DropColumn("dbo.LoansAdvances", "isVoid");
            DropColumn("dbo.LoansAdvances", "stage");
            DropColumn("dbo.LoansAdvances", "statusId");
            DropTable("dbo.LoansAdvanceStatus");
        }
    }
}
