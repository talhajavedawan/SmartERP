namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VoidTransactions : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Offers", "isVoid", c => c.Boolean(nullable: false));
            AddColumn("dbo.Inquiries", "isVoid", c => c.Boolean(nullable: false));
            AddColumn("dbo.SaleOrders", "isVoid", c => c.Boolean(nullable: false));
            AddColumn("dbo.SaleInvoices", "isVoid", c => c.Boolean(nullable: false));
            AddColumn("dbo.MemorandumSales", "isVoid", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MemorandumSales", "isVoid");
            DropColumn("dbo.SaleInvoices", "isVoid");
            DropColumn("dbo.SaleOrders", "isVoid");
            DropColumn("dbo.Inquiries", "isVoid");
            DropColumn("dbo.Offers", "isVoid");
        }
    }
}
