namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvoiceProdcutCommissionAmount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProcurementProducts", "TotalInvoicedQuantity", c => c.Double(nullable: false));
            AddColumn("dbo.ProcurementProducts", "TotalInvoicedWeight", c => c.Double(nullable: false));
            AddColumn("dbo.ProcurementProducts", "totalCommission", c => c.Double(nullable: false));
            AddColumn("dbo.ProcurementProducts", "UnInvoicedSoAmount", c => c.Double(nullable: false));
            AddColumn("dbo.ProcurementProducts", "totalInvoicedSoAmount", c => c.Double(nullable: false));
            AddColumn("dbo.ProcurementProducts", "NowAmount", c => c.Double(nullable: false));
            AddColumn("dbo.SaleInvoices", "totalInvoiceAmount", c => c.Double(nullable: false));
            AddColumn("dbo.SaleInvoices", "totalBaseAmount", c => c.Double(nullable: false));
            AddColumn("dbo.SaleInvoices", "RemainingBaseAmount", c => c.Double(nullable: false));
            DropColumn("dbo.SaleInvoices", "totalFOBValue");
            DropColumn("dbo.SaleInvoices", "totalCFRValue");
            DropColumn("dbo.SaleInvoices", "SOFOBValue");
            DropColumn("dbo.SaleInvoices", "totalBaseFOBValue");
            DropColumn("dbo.SaleInvoices", "totalBaseCFRValue");
            DropColumn("dbo.SaleInvoices", "RemainingBaseFOBValue");
            DropColumn("dbo.SaleInvoices", "RemainingBaseCFRValue");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SaleInvoices", "RemainingBaseCFRValue", c => c.Double(nullable: false));
            AddColumn("dbo.SaleInvoices", "RemainingBaseFOBValue", c => c.Double(nullable: false));
            AddColumn("dbo.SaleInvoices", "totalBaseCFRValue", c => c.Double(nullable: false));
            AddColumn("dbo.SaleInvoices", "totalBaseFOBValue", c => c.Double(nullable: false));
            AddColumn("dbo.SaleInvoices", "SOFOBValue", c => c.Double(nullable: false));
            AddColumn("dbo.SaleInvoices", "totalCFRValue", c => c.Double(nullable: false));
            AddColumn("dbo.SaleInvoices", "totalFOBValue", c => c.Double(nullable: false));
            DropColumn("dbo.SaleInvoices", "RemainingBaseAmount");
            DropColumn("dbo.SaleInvoices", "totalBaseAmount");
            DropColumn("dbo.SaleInvoices", "totalInvoiceAmount");
            DropColumn("dbo.ProcurementProducts", "NowAmount");
            DropColumn("dbo.ProcurementProducts", "totalInvoicedSoAmount");
            DropColumn("dbo.ProcurementProducts", "UnInvoicedSoAmount");
            DropColumn("dbo.ProcurementProducts", "totalCommission");
            DropColumn("dbo.ProcurementProducts", "TotalInvoicedWeight");
            DropColumn("dbo.ProcurementProducts", "TotalInvoicedQuantity");
        }
    }
}
