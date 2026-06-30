namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tempChanges : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PurchaseOrders", "SoPaymentterm_Id", "dbo.PaymentTerms");
            DropIndex("dbo.PurchaseOrders", new[] { "SoPaymentterm_Id" });
            CreateTable(
                "dbo.CostSheetBillFields",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FieldId = c.Int(nullable: false),
                        CostSheetId = c.Int(nullable: false),
                        Bill_Id = c.Int(nullable: false),
                        Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FieldType = c.Int(nullable: false),
                        timeStamp = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CostSheets", t => t.CostSheetId, cascadeDelete: true)
                .ForeignKey("dbo.CostSheetFields", t => t.FieldId, cascadeDelete: true)
                .Index(t => t.FieldId)
                .Index(t => t.CostSheetId);
            
            AddColumn("dbo.Currencies", "isVoid", c => c.Boolean(nullable: false));
            AddColumn("dbo.PurchaseOrders", "hasTax", c => c.Boolean());
            AddColumn("dbo.PurchaseOrders", "tax_Id", c => c.Int());
            AddColumn("dbo.PurchaseOrders", "billWithTax", c => c.Double());
            AddColumn("dbo.PurchaseOrders", "hasWHT", c => c.Boolean());
            AddColumn("dbo.PurchaseOrders", "WHT_Id", c => c.Int());
            AddColumn("dbo.PurchaseOrders", "billAfterTax", c => c.Double());
            AddColumn("dbo.JournalTransactions", "reconcilationType", c => c.Int(nullable: false));
            AlterColumn("dbo.PurchaseOrders", "SoPaymentterm_Id", c => c.Int());
            CreateIndex("dbo.PurchaseOrders", "tax_Id");
            CreateIndex("dbo.PurchaseOrders", "WHT_Id");
            CreateIndex("dbo.PurchaseOrders", "SoPaymentterm_Id");
            AddForeignKey("dbo.PurchaseOrders", "tax_Id", "dbo.TaxNames", "Id");
            AddForeignKey("dbo.PurchaseOrders", "WHT_Id", "dbo.TaxNames", "Id");
            //AddForeignKey("dbo.PurchaseOrders", "SoPaymentterm_Id", "dbo.PaymentTerms", "Id");
        }
        
        public override void Down()
        {
            //DropForeignKey("dbo.PurchaseOrders", "SoPaymentterm_Id", "dbo.PaymentTerms");
            DropForeignKey("dbo.PurchaseOrders", "WHT_Id", "dbo.TaxNames");
            DropForeignKey("dbo.PurchaseOrders", "tax_Id", "dbo.TaxNames");
            DropForeignKey("dbo.CostSheetBillFields", "FieldId", "dbo.CostSheetFields");
            DropForeignKey("dbo.CostSheetBillFields", "CostSheetId", "dbo.CostSheets");
            //DropIndex("dbo.PurchaseOrders", new[] { "SoPaymentterm_Id" });
            DropIndex("dbo.PurchaseOrders", new[] { "WHT_Id" });
            DropIndex("dbo.PurchaseOrders", new[] { "tax_Id" });
            DropIndex("dbo.CostSheetBillFields", new[] { "CostSheetId" });
            DropIndex("dbo.CostSheetBillFields", new[] { "FieldId" });
            //AlterColumn("dbo.PurchaseOrders", "SoPaymentterm_Id", c => c.Int(nullable: false));
            DropColumn("dbo.JournalTransactions", "reconcilationType");
            DropColumn("dbo.PurchaseOrders", "billAfterTax");
            DropColumn("dbo.PurchaseOrders", "WHT_Id");
            DropColumn("dbo.PurchaseOrders", "hasWHT");
            DropColumn("dbo.PurchaseOrders", "billWithTax");
            DropColumn("dbo.PurchaseOrders", "tax_Id");
            DropColumn("dbo.PurchaseOrders", "hasTax");
            DropColumn("dbo.Currencies", "isVoid");
            DropTable("dbo.CostSheetBillFields");
            //CreateIndex("dbo.PurchaseOrders", "SoPaymentterm_Id");
            //AddForeignKey("dbo.PurchaseOrders", "SoPaymentterm_Id", "dbo.PaymentTerms", "Id", cascadeDelete: true);
        }
    }
}
