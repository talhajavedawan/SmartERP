namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesinPurchaseInvoices : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PurchaseInvoices", "POReferenceNo", c => c.String());
            AddColumn("dbo.PurchaseInvoices", "PODate", c => c.DateTime());
            AddColumn("dbo.PurchaseInvoices", "PODeliveryDate", c => c.DateTime());
            AddColumn("dbo.PurchaseInvoices", "SODate", c => c.DateTime());
            AddColumn("dbo.PurchaseInvoices", "SODeliveryDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PurchaseInvoices", "SODeliveryDate");
            DropColumn("dbo.PurchaseInvoices", "SODate");
            DropColumn("dbo.PurchaseInvoices", "PODeliveryDate");
            DropColumn("dbo.PurchaseInvoices", "PODate");
            DropColumn("dbo.PurchaseInvoices", "POReferenceNo");
        }
    }
}
