namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Invoicevendors : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.tabVendor", "SaleInvoice_Id", "dbo.SaleInvoices");
            DropIndex("dbo.tabVendor", new[] { "SaleInvoice_Id" });
            CreateTable(
                "dbo.SaleInvoiceVendors",
                c => new
                    {
                        SaleInvoice_Id = c.Int(nullable: false),
                        Vendor_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.SaleInvoice_Id, t.Vendor_Id })
                .ForeignKey("dbo.SaleInvoices", t => t.SaleInvoice_Id, cascadeDelete: true)
                .ForeignKey("dbo.tabVendor", t => t.Vendor_Id, cascadeDelete: true)
                .Index(t => t.SaleInvoice_Id)
                .Index(t => t.Vendor_Id);
            
            DropColumn("dbo.tabVendor", "SaleInvoice_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tabVendor", "SaleInvoice_Id", c => c.Int());
            DropForeignKey("dbo.SaleInvoiceVendors", "Vendor_Id", "dbo.tabVendor");
            DropForeignKey("dbo.SaleInvoiceVendors", "SaleInvoice_Id", "dbo.SaleInvoices");
            DropIndex("dbo.SaleInvoiceVendors", new[] { "Vendor_Id" });
            DropIndex("dbo.SaleInvoiceVendors", new[] { "SaleInvoice_Id" });
            DropTable("dbo.SaleInvoiceVendors");
            CreateIndex("dbo.tabVendor", "SaleInvoice_Id");
            AddForeignKey("dbo.tabVendor", "SaleInvoice_Id", "dbo.SaleInvoices", "Id");
        }
    }
}
