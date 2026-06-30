namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class allowedNULL4CommisionNMargin : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Offers", "commision", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.Offers", "margin", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.PurchaseOrders", "commision", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.PurchaseOrders", "margin", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PurchaseOrders", "margin", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.PurchaseOrders", "commision", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Offers", "margin", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Offers", "commision", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
