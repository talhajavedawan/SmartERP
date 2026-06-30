namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CostSheetSIFieldsAndPaymentTaxesAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CostSheetSIFields",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        FieldId = c.Int(nullable: false),
                        CostSheetId = c.Int(nullable: false),
                        SI_Id = c.Int(nullable: false),
                        Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FieldType = c.Int(nullable: false),
                        timeStamp = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.CostSheets", t => t.CostSheetId, cascadeDelete: true)
                .ForeignKey("dbo.CostSheetFields", t => t.FieldId, cascadeDelete: true)
                .Index(t => t.FieldId)
                .Index(t => t.CostSheetId);
            
            CreateTable(
                "dbo.PaymentTaxes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        taxNameId = c.Int(),
                        Amount = c.Double(nullable: false),
                        Payment_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TaxNames", t => t.taxNameId)
                .ForeignKey("dbo.Payments", t => t.Payment_Id)
                .Index(t => t.taxNameId)
                .Index(t => t.Payment_Id);
            
            AddColumn("dbo.Payments", "IsAdjusted", c => c.Boolean());
            AddColumn("dbo.Payments", "totalVATamount", c => c.Double(nullable: false));
            AddColumn("dbo.SaleInvoices", "CostSheet_Id", c => c.Int());
            AddColumn("dbo.SaleInvoices", "amountSOC", c => c.Double(nullable: false));
            CreateIndex("dbo.SaleInvoices", "CostSheet_Id");
            AddForeignKey("dbo.SaleInvoices", "CostSheet_Id", "dbo.CostSheets", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PaymentTaxes", "Payment_Id", "dbo.Payments");
            DropForeignKey("dbo.PaymentTaxes", "taxNameId", "dbo.TaxNames");
            DropForeignKey("dbo.SaleInvoices", "CostSheet_Id", "dbo.CostSheets");
            DropForeignKey("dbo.CostSheetSIFields", "FieldId", "dbo.CostSheetFields");
            DropForeignKey("dbo.CostSheetSIFields", "CostSheetId", "dbo.CostSheets");
            DropIndex("dbo.PaymentTaxes", new[] { "Payment_Id" });
            DropIndex("dbo.PaymentTaxes", new[] { "taxNameId" });
            DropIndex("dbo.SaleInvoices", new[] { "CostSheet_Id" });
            DropIndex("dbo.CostSheetSIFields", new[] { "CostSheetId" });
            DropIndex("dbo.CostSheetSIFields", new[] { "FieldId" });
            DropColumn("dbo.SaleInvoices", "amountSOC");
            DropColumn("dbo.SaleInvoices", "CostSheet_Id");
            DropColumn("dbo.Payments", "totalVATamount");
            DropColumn("dbo.Payments", "IsAdjusted");
            DropTable("dbo.PaymentTaxes");
            DropTable("dbo.CostSheetSIFields");
        }
    }
}
