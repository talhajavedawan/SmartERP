namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class backgroundImagesPathnPurchaseInvoiceVendors : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tabBackground",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Path = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PurchaseInvoiceVendors",
                c => new
                    {
                        PurchaseInvoice_Id = c.Int(nullable: false),
                        Vendor_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.PurchaseInvoice_Id, t.Vendor_Id })
                .ForeignKey("dbo.PurchaseInvoices", t => t.PurchaseInvoice_Id, cascadeDelete: true)
                .ForeignKey("dbo.tabVendor", t => t.Vendor_Id, cascadeDelete: true)
                .Index(t => t.PurchaseInvoice_Id)
                .Index(t => t.Vendor_Id);
            
            AddColumn("dbo.PurchaseInvoices", "totalInvoiceAmount", c => c.Double(nullable: false));
            AddColumn("dbo.PurchaseInvoices", "POCFRValue", c => c.Double(nullable: false));
            AddColumn("dbo.PurchaseInvoices", "totalBaseAmount", c => c.Double(nullable: false));
            AddColumn("dbo.PurchaseInvoices", "exchangeRate", c => c.Single(nullable: false));
            AddColumn("dbo.PurchaseInvoices", "marginExchangeRate", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.PurchaseInvoices", "stage", c => c.String());
            AddColumn("dbo.PurchaseInvoices", "ClosingDate", c => c.DateTime());
            AddColumn("dbo.PurchaseInvoices", "LastStatusChangeDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PurchaseInvoiceVendors", "Vendor_Id", "dbo.tabVendor");
            DropForeignKey("dbo.PurchaseInvoiceVendors", "PurchaseInvoice_Id", "dbo.PurchaseInvoices");
            DropIndex("dbo.PurchaseInvoiceVendors", new[] { "Vendor_Id" });
            DropIndex("dbo.PurchaseInvoiceVendors", new[] { "PurchaseInvoice_Id" });
            DropColumn("dbo.PurchaseInvoices", "LastStatusChangeDate");
            DropColumn("dbo.PurchaseInvoices", "ClosingDate");
            DropColumn("dbo.PurchaseInvoices", "stage");
            DropColumn("dbo.PurchaseInvoices", "marginExchangeRate");
            DropColumn("dbo.PurchaseInvoices", "exchangeRate");
            DropColumn("dbo.PurchaseInvoices", "totalBaseAmount");
            DropColumn("dbo.PurchaseInvoices", "POCFRValue");
            DropColumn("dbo.PurchaseInvoices", "totalInvoiceAmount");
            DropTable("dbo.PurchaseInvoiceVendors");
            DropTable("dbo.tabBackground");
        }
    }
}
