namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class venodrNamePOandAttachmentWithTransactionType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TransactionItems", "AttachmentCategory_Id", c => c.Int());
            AddColumn("dbo.PurchaseOrders", "VendorName", c => c.String());
            CreateIndex("dbo.TransactionItems", "AttachmentCategory_Id");
            AddForeignKey("dbo.TransactionItems", "AttachmentCategory_Id", "dbo.AttachmentCategories", "Id");
            DropColumn("dbo.PurchaseOrders", "CommisionRefrenceNo");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PurchaseOrders", "CommisionRefrenceNo", c => c.String());
            DropForeignKey("dbo.TransactionItems", "AttachmentCategory_Id", "dbo.AttachmentCategories");
            DropIndex("dbo.TransactionItems", new[] { "AttachmentCategory_Id" });
            DropColumn("dbo.PurchaseOrders", "VendorName");
            DropColumn("dbo.TransactionItems", "AttachmentCategory_Id");
        }
    }
}
