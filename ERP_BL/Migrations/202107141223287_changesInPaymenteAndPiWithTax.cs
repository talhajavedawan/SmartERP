namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changesInPaymenteAndPiWithTax : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Payments", "paymentVendorBillTemplate", c => c.Int());
            AddColumn("dbo.PurchaseInvoices", "tax_Id", c => c.Int());
            CreateIndex("dbo.PurchaseInvoices", "tax_Id");
            AddForeignKey("dbo.PurchaseInvoices", "tax_Id", "dbo.TaxNames", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PurchaseInvoices", "tax_Id", "dbo.TaxNames");
            DropIndex("dbo.PurchaseInvoices", new[] { "tax_Id" });
            DropColumn("dbo.PurchaseInvoices", "tax_Id");
            DropColumn("dbo.Payments", "paymentVendorBillTemplate");
        }
    }
}
