namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changesInPIandNotifications : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JournalTransactions", "PurchaseInvoiceId", c => c.Int());
            AddColumn("dbo.PurchaseInvoices", "POCER", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.PurchaseInvoices", "PIAmuontSOC", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.tabPopupNotifications", "Title", c => c.String());
            AddColumn("dbo.tabPopupNotifications", "TitleColorCode", c => c.String());
            AddColumn("dbo.tabPopupNotifications", "TextColorCode", c => c.String());
            CreateIndex("dbo.JournalTransactions", "PurchaseInvoiceId");
            AddForeignKey("dbo.JournalTransactions", "PurchaseInvoiceId", "dbo.PurchaseInvoices", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.JournalTransactions", "PurchaseInvoiceId", "dbo.PurchaseInvoices");
            DropIndex("dbo.JournalTransactions", new[] { "PurchaseInvoiceId" });
            DropColumn("dbo.tabPopupNotifications", "TextColorCode");
            DropColumn("dbo.tabPopupNotifications", "TitleColorCode");
            DropColumn("dbo.tabPopupNotifications", "Title");
            DropColumn("dbo.PurchaseInvoices", "PIAmuontSOC");
            DropColumn("dbo.PurchaseInvoices", "POCER");
            DropColumn("dbo.JournalTransactions", "PurchaseInvoiceId");
        }
    }
}
