namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReceiptTaxesAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ReceiptTaxes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        taxNameId = c.Int(),
                        SalesReceiptForDeductionTaxId = c.Int(),
                        SalesReceiptForBankChargesTaxId = c.Int(),
                        Amount = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SalesReceipts", t => t.SalesReceiptForBankChargesTaxId)
                .ForeignKey("dbo.SalesReceipts", t => t.SalesReceiptForDeductionTaxId)
                .ForeignKey("dbo.TaxNames", t => t.taxNameId)
                .Index(t => t.taxNameId)
                .Index(t => t.SalesReceiptForDeductionTaxId)
                .Index(t => t.SalesReceiptForBankChargesTaxId);
            
            AddColumn("dbo.SalesReceipts", "IsAdjustedDedVAT", c => c.Boolean());
            AddColumn("dbo.SalesReceipts", "IsAdjustedBankVAT", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ReceiptTaxes", "taxNameId", "dbo.TaxNames");
            DropForeignKey("dbo.ReceiptTaxes", "SalesReceiptForDeductionTaxId", "dbo.SalesReceipts");
            DropForeignKey("dbo.ReceiptTaxes", "SalesReceiptForBankChargesTaxId", "dbo.SalesReceipts");
            DropIndex("dbo.ReceiptTaxes", new[] { "SalesReceiptForBankChargesTaxId" });
            DropIndex("dbo.ReceiptTaxes", new[] { "SalesReceiptForDeductionTaxId" });
            DropIndex("dbo.ReceiptTaxes", new[] { "taxNameId" });
            DropColumn("dbo.SalesReceipts", "IsAdjustedBankVAT");
            DropColumn("dbo.SalesReceipts", "IsAdjustedDedVAT");
            DropTable("dbo.ReceiptTaxes");
        }
    }
}
