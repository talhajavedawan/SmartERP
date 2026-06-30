namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ClosingDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Offers", "LastStatusChangeDate", c => c.DateTime());
            AddColumn("dbo.Inquiries", "ClosingDate", c => c.DateTime());
            AddColumn("dbo.Inquiries", "LastStatusChangeDate", c => c.DateTime());
            AddColumn("dbo.SaleOrders", "ClosingDate", c => c.DateTime());
            AddColumn("dbo.SaleOrders", "LastStatusChangeDate", c => c.DateTime());
            AddColumn("dbo.SaleInvoices", "ClosingDate", c => c.DateTime());
            AddColumn("dbo.SaleInvoices", "LastStatusChangeDate", c => c.DateTime());
            AddColumn("dbo.MemorandumSales", "ClosingDate", c => c.DateTime());
            AddColumn("dbo.MemorandumSales", "LastStatusChangeDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.MemorandumSales", "LastStatusChangeDate");
            DropColumn("dbo.MemorandumSales", "ClosingDate");
            DropColumn("dbo.SaleInvoices", "LastStatusChangeDate");
            DropColumn("dbo.SaleInvoices", "ClosingDate");
            DropColumn("dbo.SaleOrders", "LastStatusChangeDate");
            DropColumn("dbo.SaleOrders", "ClosingDate");
            DropColumn("dbo.Inquiries", "LastStatusChangeDate");
            DropColumn("dbo.Inquiries", "ClosingDate");
            DropColumn("dbo.Offers", "LastStatusChangeDate");
        }
    }
}
