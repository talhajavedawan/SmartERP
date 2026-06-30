namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CostSheetPaymentFieldsAndPaymentDeductionsAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CostSheetPaymentFields",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FieldId = c.Int(nullable: false),
                        CostSheetId = c.Int(nullable: false),
                        Payment_Id = c.Int(nullable: false),
                        Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FieldType = c.Int(nullable: false),
                        timeStamp = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CostSheets", t => t.CostSheetId, cascadeDelete: true)
                .ForeignKey("dbo.CostSheetFields", t => t.FieldId, cascadeDelete: true)
                .Index(t => t.FieldId)
                .Index(t => t.CostSheetId);
            
            CreateTable(
                "dbo.PaymentDeductions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        deduction_id = c.Int(),
                        Amount = c.Double(nullable: false),
                        Payment_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Deductions", t => t.deduction_id)
                .ForeignKey("dbo.Payments", t => t.Payment_Id)
                .Index(t => t.deduction_id)
                .Index(t => t.Payment_Id);
            
            AddColumn("dbo.Bills", "billVendor_Id", c => c.Int());
            AddColumn("dbo.Bills", "billVendorName", c => c.String());
            AddColumn("dbo.Bills", "POVendor_Id", c => c.Int());
            AddColumn("dbo.Bills", "POVendorName", c => c.String());
            AddColumn("dbo.Payments", "CostSheetId", c => c.Int());
            AddColumn("dbo.Deductions", "paymentAmount", c => c.Double(nullable: false));
            CreateIndex("dbo.Bills", "billVendor_Id");
            CreateIndex("dbo.Bills", "POVendor_Id");
            CreateIndex("dbo.Payments", "CostSheetId");
            AddForeignKey("dbo.Bills", "billVendor_Id", "dbo.tabVendor", "Id");
            AddForeignKey("dbo.Payments", "CostSheetId", "dbo.CostSheets", "Id");
            AddForeignKey("dbo.Bills", "POVendor_Id", "dbo.tabVendor", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Bills", "POVendor_Id", "dbo.tabVendor");
            DropForeignKey("dbo.PaymentDeductions", "Payment_Id", "dbo.Payments");
            DropForeignKey("dbo.PaymentDeductions", "deduction_id", "dbo.Deductions");
            DropForeignKey("dbo.Payments", "CostSheetId", "dbo.CostSheets");
            DropForeignKey("dbo.CostSheetPaymentFields", "FieldId", "dbo.CostSheetFields");
            DropForeignKey("dbo.CostSheetPaymentFields", "CostSheetId", "dbo.CostSheets");
            DropForeignKey("dbo.Bills", "billVendor_Id", "dbo.tabVendor");
            DropIndex("dbo.PaymentDeductions", new[] { "Payment_Id" });
            DropIndex("dbo.PaymentDeductions", new[] { "deduction_id" });
            DropIndex("dbo.Payments", new[] { "CostSheetId" });
            DropIndex("dbo.CostSheetPaymentFields", new[] { "CostSheetId" });
            DropIndex("dbo.CostSheetPaymentFields", new[] { "FieldId" });
            DropIndex("dbo.Bills", new[] { "POVendor_Id" });
            DropIndex("dbo.Bills", new[] { "billVendor_Id" });
            DropColumn("dbo.Deductions", "paymentAmount");
            DropColumn("dbo.Payments", "CostSheetId");
            DropColumn("dbo.Bills", "POVendorName");
            DropColumn("dbo.Bills", "POVendor_Id");
            DropColumn("dbo.Bills", "billVendorName");
            DropColumn("dbo.Bills", "billVendor_Id");
            DropTable("dbo.PaymentDeductions");
            DropTable("dbo.CostSheetPaymentFields");
        }
    }
}
