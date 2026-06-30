namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newfieldsInSaleSeceipt : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SalesReceipts", "stage", c => c.String());
            AddColumn("dbo.SalesReceipts", "PendingForReApproval", c => c.Boolean());
            AddColumn("dbo.SalesReceipts", "isReApproved", c => c.Boolean());
            AddColumn("dbo.SalesReceipts", "ReApprovalDate", c => c.DateTime());
            AddColumn("dbo.SalesReceipts", "user_Id", c => c.Int());
            AddColumn("dbo.SalesReceipts", "ClosingDate", c => c.DateTime());
            AddColumn("dbo.SalesReceipts", "LastStatusChangeDate", c => c.DateTime());
            CreateIndex("dbo.SalesReceipts", "user_Id");
            AddForeignKey("dbo.SalesReceipts", "user_Id", "dbo.Users", "id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SalesReceipts", "user_Id", "dbo.Users");
            DropIndex("dbo.SalesReceipts", new[] { "user_Id" });
            DropColumn("dbo.SalesReceipts", "LastStatusChangeDate");
            DropColumn("dbo.SalesReceipts", "ClosingDate");
            DropColumn("dbo.SalesReceipts", "user_Id");
            DropColumn("dbo.SalesReceipts", "ReApprovalDate");
            DropColumn("dbo.SalesReceipts", "isReApproved");
            DropColumn("dbo.SalesReceipts", "PendingForReApproval");
            DropColumn("dbo.SalesReceipts", "stage");
        }
    }
}
