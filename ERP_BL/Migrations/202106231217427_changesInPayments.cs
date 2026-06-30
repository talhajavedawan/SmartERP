namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changesInPayments : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Payments", "stage", c => c.String());
            AddColumn("dbo.Payments", "isVoid", c => c.Boolean(nullable: false));
            AddColumn("dbo.Payments", "isReviewed", c => c.Boolean());
            AddColumn("dbo.Payments", "needReview", c => c.Boolean());
            AddColumn("dbo.Payments", "PendingForClosing", c => c.Boolean());
            AddColumn("dbo.Payments", "PendingForReApproval", c => c.Boolean());
            AddColumn("dbo.Payments", "isApproved", c => c.Boolean());
            AddColumn("dbo.Payments", "ApprovedDate", c => c.DateTime());
            AddColumn("dbo.Payments", "isReApproved", c => c.Boolean());
            AddColumn("dbo.Payments", "ReApprovalDate", c => c.DateTime());
            AddColumn("dbo.Payments", "user_Id", c => c.Int());
            AddColumn("dbo.Payments", "ClosingDate", c => c.DateTime());
            AddColumn("dbo.Payments", "LastStatusChangeDate", c => c.DateTime());
            AddColumn("dbo.ProcurementProducts", "PurchaseInvoice_Id", c => c.Int());
            AlterColumn("dbo.Payments", "paymentAdminBillTemplate", c => c.Int());
            CreateIndex("dbo.Payments", "user_Id");
            CreateIndex("dbo.ProcurementProducts", "PurchaseInvoice_Id");
            AddForeignKey("dbo.Payments", "user_Id", "dbo.Users", "id");
            AddForeignKey("dbo.ProcurementProducts", "PurchaseInvoice_Id", "dbo.PurchaseInvoices", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProcurementProducts", "PurchaseInvoice_Id", "dbo.PurchaseInvoices");
            DropForeignKey("dbo.Payments", "user_Id", "dbo.Users");
            DropIndex("dbo.ProcurementProducts", new[] { "PurchaseInvoice_Id" });
            DropIndex("dbo.Payments", new[] { "user_Id" });
            AlterColumn("dbo.Payments", "paymentAdminBillTemplate", c => c.Int(nullable: false));
            DropColumn("dbo.ProcurementProducts", "PurchaseInvoice_Id");
            DropColumn("dbo.Payments", "LastStatusChangeDate");
            DropColumn("dbo.Payments", "ClosingDate");
            DropColumn("dbo.Payments", "user_Id");
            DropColumn("dbo.Payments", "ReApprovalDate");
            DropColumn("dbo.Payments", "isReApproved");
            DropColumn("dbo.Payments", "ApprovedDate");
            DropColumn("dbo.Payments", "isApproved");
            DropColumn("dbo.Payments", "PendingForReApproval");
            DropColumn("dbo.Payments", "PendingForClosing");
            DropColumn("dbo.Payments", "needReview");
            DropColumn("dbo.Payments", "isReviewed");
            DropColumn("dbo.Payments", "isVoid");
            DropColumn("dbo.Payments", "stage");
        }
    }
}
