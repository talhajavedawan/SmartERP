namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changesInSaleInvoices : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SaleInvoices", "isSTLDiscount", c => c.Boolean(nullable: false));
            AddColumn("dbo.SaleInvoices", "stlDiscountCurrency_Id", c => c.Int());
            AddColumn("dbo.SaleInvoices", "stlDiscountAmount", c => c.Double(nullable: false));
            CreateIndex("dbo.SaleInvoices", "stlDiscountCurrency_Id");
            AddForeignKey("dbo.SaleInvoices", "stlDiscountCurrency_Id", "dbo.Currencies", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SaleInvoices", "stlDiscountCurrency_Id", "dbo.Currencies");
            DropIndex("dbo.SaleInvoices", new[] { "stlDiscountCurrency_Id" });
            DropColumn("dbo.SaleInvoices", "stlDiscountAmount");
            DropColumn("dbo.SaleInvoices", "stlDiscountCurrency_Id");
            DropColumn("dbo.SaleInvoices", "isSTLDiscount");
        }
    }
}
