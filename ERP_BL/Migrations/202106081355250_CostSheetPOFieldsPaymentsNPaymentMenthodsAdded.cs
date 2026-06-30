namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CostSheetPOFieldsPaymentsNPaymentMenthodsAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CostSheetPOFields",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FieldId = c.Int(nullable: false),
                        CostSheetId = c.Int(nullable: false),
                        PO_Id = c.Int(nullable: false),
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
                "dbo.PaymentMethods",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MethodName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Payments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        paymentType = c.Int(nullable: false),
                        CreationDate = c.DateTime(),
                        TransactionType_Id = c.Int(nullable: false),
                        company_Id = c.Int(),
                        dept_Id = c.Int(),
                        vendor_Id = c.Int(),
                        DebitedDate = c.DateTime(),
                        PaymentDate = c.DateTime(),
                        SystemRefNo = c.String(),
                        PaymentRefNo = c.String(),
                        currency_Id = c.Int(),
                        PaymentAmount = c.Double(nullable: false),
                        statusId = c.Int(),
                        paymentMethodId = c.Int(),
                        bankId = c.Int(),
                        accountId = c.Int(),
                        isCreditCard = c.Boolean(nullable: false),
                        primaryCardNoId = c.Int(),
                        InstrumentNo = c.String(),
                        InstrumentDate = c.DateTime(),
                        BillCreationDate = c.DateTime(),
                        BillFinanceRefNo = c.String(),
                        BillDueDate = c.DateTime(),
                        BillNumber = c.String(),
                        BillAmount = c.Double(nullable: false),
                        AmountDue = c.Double(nullable: false),
                        AmountToPay = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Accounts", t => t.accountId)
                .ForeignKey("dbo.Banks", t => t.bankId)
                .ForeignKey("dbo.tabCompany", t => t.company_Id)
                .ForeignKey("dbo.Currencies", t => t.currency_Id)
                .ForeignKey("dbo.tabDepartment", t => t.dept_Id)
                .ForeignKey("dbo.PaymentMethods", t => t.paymentMethodId)
                .ForeignKey("dbo.CreditCards", t => t.primaryCardNoId)
                .ForeignKey("dbo.PaymentStatus", t => t.statusId)
                .ForeignKey("dbo.tabVendor", t => t.vendor_Id)
                .Index(t => t.company_Id)
                .Index(t => t.dept_Id)
                .Index(t => t.vendor_Id)
                .Index(t => t.currency_Id)
                .Index(t => t.statusId)
                .Index(t => t.paymentMethodId)
                .Index(t => t.bankId)
                .Index(t => t.accountId)
                .Index(t => t.primaryCardNoId);
            
            CreateTable(
                "dbo.PaymentStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.String(),
                        isApproved = c.Boolean(nullable: false),
                        isActive = c.Boolean(),
                        backcolor = c.String(),
                        forecolor = c.String(),
                        HierarchicalIndex = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Payments", "vendor_Id", "dbo.tabVendor");
            DropForeignKey("dbo.Payments", "statusId", "dbo.PaymentStatus");
            DropForeignKey("dbo.Payments", "primaryCardNoId", "dbo.CreditCards");
            DropForeignKey("dbo.Payments", "paymentMethodId", "dbo.PaymentMethods");
            DropForeignKey("dbo.Payments", "dept_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.Payments", "currency_Id", "dbo.Currencies");
            DropForeignKey("dbo.Payments", "company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.Payments", "bankId", "dbo.Banks");
            DropForeignKey("dbo.Payments", "accountId", "dbo.Accounts");
            DropForeignKey("dbo.CostSheetPOFields", "FieldId", "dbo.CostSheetFields");
            DropForeignKey("dbo.CostSheetPOFields", "CostSheetId", "dbo.CostSheets");
            DropIndex("dbo.Payments", new[] { "primaryCardNoId" });
            DropIndex("dbo.Payments", new[] { "accountId" });
            DropIndex("dbo.Payments", new[] { "bankId" });
            DropIndex("dbo.Payments", new[] { "paymentMethodId" });
            DropIndex("dbo.Payments", new[] { "statusId" });
            DropIndex("dbo.Payments", new[] { "currency_Id" });
            DropIndex("dbo.Payments", new[] { "vendor_Id" });
            DropIndex("dbo.Payments", new[] { "dept_Id" });
            DropIndex("dbo.Payments", new[] { "company_Id" });
            DropIndex("dbo.CostSheetPOFields", new[] { "CostSheetId" });
            DropIndex("dbo.CostSheetPOFields", new[] { "FieldId" });
            DropTable("dbo.PaymentStatus");
            DropTable("dbo.Payments");
            DropTable("dbo.PaymentMethods");
            DropTable("dbo.CostSheetPOFields");
        }
    }
}
