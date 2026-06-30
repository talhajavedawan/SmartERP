namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesInAssetRentals : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AssetRentals", "transactionGroupId", c => c.Int(nullable: false));
            AddColumn("dbo.AssetRentals", "SystemRef", c => c.String());
            AddColumn("dbo.AssetRentals", "creatorId", c => c.Int());
            AddColumn("dbo.AssetRentals", "stage", c => c.String());
            AddColumn("dbo.AssetRentals", "isVoid", c => c.Boolean(nullable: false));
            AddColumn("dbo.AssetRentals", "isReviewed", c => c.Boolean());
            AddColumn("dbo.AssetRentals", "needReview", c => c.Boolean());
            AddColumn("dbo.AssetRentals", "PendingForClosing", c => c.Boolean());
            AddColumn("dbo.AssetRentals", "PendingForReApproval", c => c.Boolean());
            AddColumn("dbo.AssetRentals", "isApproved", c => c.Boolean());
            AddColumn("dbo.AssetRentals", "ApprovedDate", c => c.DateTime());
            AddColumn("dbo.AssetRentals", "isReApproved", c => c.Boolean());
            AddColumn("dbo.AssetRentals", "ReApprovalDate", c => c.DateTime());
            AddColumn("dbo.AssetRentals", "ClosingDate", c => c.DateTime());
            AddColumn("dbo.AssetRentals", "LastStatusChangeDate", c => c.DateTime());
            CreateIndex("dbo.AssetRentals", "creatorId");
            AddForeignKey("dbo.AssetRentals", "creatorId", "dbo.Users", "id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AssetRentals", "creatorId", "dbo.Users");
            DropIndex("dbo.AssetRentals", new[] { "creatorId" });
            DropColumn("dbo.AssetRentals", "LastStatusChangeDate");
            DropColumn("dbo.AssetRentals", "ClosingDate");
            DropColumn("dbo.AssetRentals", "ReApprovalDate");
            DropColumn("dbo.AssetRentals", "isReApproved");
            DropColumn("dbo.AssetRentals", "ApprovedDate");
            DropColumn("dbo.AssetRentals", "isApproved");
            DropColumn("dbo.AssetRentals", "PendingForReApproval");
            DropColumn("dbo.AssetRentals", "PendingForClosing");
            DropColumn("dbo.AssetRentals", "needReview");
            DropColumn("dbo.AssetRentals", "isReviewed");
            DropColumn("dbo.AssetRentals", "isVoid");
            DropColumn("dbo.AssetRentals", "stage");
            DropColumn("dbo.AssetRentals", "creatorId");
            DropColumn("dbo.AssetRentals", "SystemRef");
            DropColumn("dbo.AssetRentals", "transactionGroupId");
        }
    }
}
