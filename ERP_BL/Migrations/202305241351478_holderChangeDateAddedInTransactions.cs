namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class holderChangeDateAddedInTransactions : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bills", "holderChangeDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Inquiries", "holderChangeDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Offers", "holderChangeDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.PurchaseOrders", "holderChangeDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Payments", "holderChangeDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.PurchaseInvoices", "holderChangeDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.SaleInvoices", "holderChangeDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.SaleOrders", "holderChangeDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.SalesReceipts", "holderChangeDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SalesReceipts", "holderChangeDate");
            DropColumn("dbo.SaleOrders", "holderChangeDate");
            DropColumn("dbo.SaleInvoices", "holderChangeDate");
            DropColumn("dbo.PurchaseInvoices", "holderChangeDate");
            DropColumn("dbo.Payments", "holderChangeDate");
            DropColumn("dbo.PurchaseOrders", "holderChangeDate");
            DropColumn("dbo.Offers", "holderChangeDate");
            DropColumn("dbo.Inquiries", "holderChangeDate");
            DropColumn("dbo.Bills", "holderChangeDate");
        }
    }
}
