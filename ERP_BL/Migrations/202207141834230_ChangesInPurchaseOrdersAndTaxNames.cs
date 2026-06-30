namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesInPurchaseOrdersAndTaxNames : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PurchaseOrders", "isDiscount", c => c.Boolean(nullable: false));
            AddColumn("dbo.PurchaseOrders", "Discount", c => c.Double(nullable: false));
            AddColumn("dbo.PurchaseOrders", "isFreight", c => c.Boolean(nullable: false));
            AddColumn("dbo.PurchaseOrders", "Freight", c => c.Double(nullable: false));
            AddColumn("dbo.PurchaseOrders", "isCOO", c => c.Boolean(nullable: false));
            AddColumn("dbo.PurchaseOrders", "COO", c => c.Double(nullable: false));
            AddColumn("dbo.ProcurementProducts", "priority", c => c.Int(nullable: false));
            AddColumn("dbo.TaxNames", "isManual", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TaxNames", "isManual");
            DropColumn("dbo.ProcurementProducts", "priority");
            DropColumn("dbo.PurchaseOrders", "COO");
            DropColumn("dbo.PurchaseOrders", "isCOO");
            DropColumn("dbo.PurchaseOrders", "Freight");
            DropColumn("dbo.PurchaseOrders", "isFreight");
            DropColumn("dbo.PurchaseOrders", "Discount");
            DropColumn("dbo.PurchaseOrders", "isDiscount");
        }
    }
}
