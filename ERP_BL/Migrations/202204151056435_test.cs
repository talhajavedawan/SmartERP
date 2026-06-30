namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PurchaseOrders", "totaltaxAmount", c => c.Double(nullable: false));
            AddColumn("dbo.PurchaseInvoices", "totaltaxAmount", c => c.Double(nullable: false));
            AddColumn("dbo.SaleInvoices", "totaltaxAmount", c => c.Double(nullable: false));
            AddColumn("dbo.SaleOrders", "totaltaxAmount", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SaleOrders", "totaltaxAmount");
            DropColumn("dbo.SaleInvoices", "totaltaxAmount");
            DropColumn("dbo.PurchaseInvoices", "totaltaxAmount");
            DropColumn("dbo.PurchaseOrders", "totaltaxAmount");
        }
    }
}
