namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesinRentalContracts : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RentalContracts", "transactionGroupId", c => c.Int(nullable: false));
            AddColumn("dbo.RentalContracts", "SystemRef", c => c.String());
            AddColumn("dbo.RentalContracts", "creatorId", c => c.Int());
            AddColumn("dbo.RentalContracts", "stage", c => c.String());
            AddColumn("dbo.RentalContracts", "isVoid", c => c.Boolean(nullable: false));
            AddColumn("dbo.RentalContracts", "isReviewed", c => c.Boolean());
            AddColumn("dbo.RentalContracts", "needReview", c => c.Boolean());
            AddColumn("dbo.RentalContracts", "PendingForClosing", c => c.Boolean());
            AddColumn("dbo.RentalContracts", "PendingForReApproval", c => c.Boolean());
            AddColumn("dbo.RentalContracts", "isApproved", c => c.Boolean());
            AddColumn("dbo.RentalContracts", "ApprovedDate", c => c.DateTime());
            AddColumn("dbo.RentalContracts", "isReApproved", c => c.Boolean());
            AddColumn("dbo.RentalContracts", "ReApprovalDate", c => c.DateTime());
            AddColumn("dbo.RentalContracts", "ClosingDate", c => c.DateTime());
            AddColumn("dbo.RentalContracts", "LastStatusChangeDate", c => c.DateTime());
            CreateIndex("dbo.RentalContracts", "creatorId");
            AddForeignKey("dbo.RentalContracts", "creatorId", "dbo.Users", "id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RentalContracts", "creatorId", "dbo.Users");
            DropIndex("dbo.RentalContracts", new[] { "creatorId" });
            DropColumn("dbo.RentalContracts", "LastStatusChangeDate");
            DropColumn("dbo.RentalContracts", "ClosingDate");
            DropColumn("dbo.RentalContracts", "ReApprovalDate");
            DropColumn("dbo.RentalContracts", "isReApproved");
            DropColumn("dbo.RentalContracts", "ApprovedDate");
            DropColumn("dbo.RentalContracts", "isApproved");
            DropColumn("dbo.RentalContracts", "PendingForReApproval");
            DropColumn("dbo.RentalContracts", "PendingForClosing");
            DropColumn("dbo.RentalContracts", "needReview");
            DropColumn("dbo.RentalContracts", "isReviewed");
            DropColumn("dbo.RentalContracts", "isVoid");
            DropColumn("dbo.RentalContracts", "stage");
            DropColumn("dbo.RentalContracts", "creatorId");
            DropColumn("dbo.RentalContracts", "SystemRef");
            DropColumn("dbo.RentalContracts", "transactionGroupId");
        }
    }
}
