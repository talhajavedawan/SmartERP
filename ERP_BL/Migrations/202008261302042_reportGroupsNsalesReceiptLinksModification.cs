namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class reportGroupsNsalesReceiptLinksModification : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Roles", "GridReportGroup_Id", "dbo.GridReportGroups");
            DropIndex("dbo.Roles", new[] { "GridReportGroup_Id" });
            DropIndex("dbo.GridReportGroups", new[] { "parentId" });
            AddColumn("dbo.GridReportGroups", "userId", c => c.Int());
            AddColumn("dbo.SalesReceipts", "transactionGroupId", c => c.Int(nullable: false));
            AddColumn("dbo.SalesReceipts", "saleInvoice_Id", c => c.Int());
            AlterColumn("dbo.GridReportGroups", "parentId", c => c.Int());
            CreateIndex("dbo.SalesReceipts", "saleInvoice_Id");
            CreateIndex("dbo.GridReportGroups", "parentId");
            CreateIndex("dbo.GridReportGroups", "userId");
            AddForeignKey("dbo.SalesReceipts", "saleInvoice_Id", "dbo.SaleInvoices", "Id");
            AddForeignKey("dbo.GridReportGroups", "userId", "dbo.Users", "id");
            DropColumn("dbo.Roles", "GridReportGroup_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Roles", "GridReportGroup_Id", c => c.Int());
            DropForeignKey("dbo.GridReportGroups", "userId", "dbo.Users");
            DropForeignKey("dbo.SalesReceipts", "saleInvoice_Id", "dbo.SaleInvoices");
            DropIndex("dbo.GridReportGroups", new[] { "userId" });
            DropIndex("dbo.GridReportGroups", new[] { "parentId" });
            DropIndex("dbo.SalesReceipts", new[] { "saleInvoice_Id" });
            AlterColumn("dbo.GridReportGroups", "parentId", c => c.Int(nullable: false));
            DropColumn("dbo.SalesReceipts", "saleInvoice_Id");
            DropColumn("dbo.SalesReceipts", "transactionGroupId");
            DropColumn("dbo.GridReportGroups", "userId");
            CreateIndex("dbo.GridReportGroups", "parentId");
            CreateIndex("dbo.Roles", "GridReportGroup_Id");
            AddForeignKey("dbo.Roles", "GridReportGroup_Id", "dbo.GridReportGroups", "Id");
        }
    }
}
