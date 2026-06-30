namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesInSaleInvoicesAndAttachmentCategory : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SaleInvoices", "paymentOnDate", c => c.DateTime());
            AddColumn("dbo.SaleInvoices", "isPaid", c => c.Boolean(nullable: false));
            AddColumn("dbo.SaleInvoices", "isRedInvoice", c => c.Boolean(nullable: false));
            AddColumn("dbo.SaleInvoices", "deliveryDays", c => c.Double(nullable: false));
            AddColumn("dbo.AttachmentCategories", "Document", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AttachmentCategories", "Document");
            DropColumn("dbo.SaleInvoices", "deliveryDays");
            DropColumn("dbo.SaleInvoices", "isRedInvoice");
            DropColumn("dbo.SaleInvoices", "isPaid");
            DropColumn("dbo.SaleInvoices", "paymentOnDate");
        }
    }
}
