namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SaleInvoice : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Warranties",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        isApproved = c.Boolean(nullable: false),
                        isActive = c.Boolean(nullable: false),
                        user_Id = c.Int(),
                        addedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.user_Id)
                .Index(t => t.user_Id);
            
            CreateTable(
                "dbo.SaleInvoices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        referenceNo = c.String(),
                        SalesReferenceNo = c.String(),
                        FinanceRefrenceNo = c.String(),
                        commisionRefrenceNo = c.String(),
                        offerReferenceNo = c.String(),
                        saleInvoiceDate = c.DateTime(),
                        CreationDate = c.DateTime(),
                        deliveryDate = c.DateTime(),
                        ETDDate = c.DateTime(),
                        ETADate = c.DateTime(),
                        BLAWBDate = c.DateTime(),
                        lCDate = c.DateTime(),
                        materialReciptDate = c.DateTime(),
                        PaymentDueStartDate = c.DateTime(),
                        ExpectedPayment = c.DateTime(),
                        PaymentDueAgeing = c.DateTime(),
                        CreditDays = c.Int(nullable: false),
                        targetYear = c.Int(nullable: false),
                        targetMonth = c.Int(nullable: false),
                        lCnumber = c.String(),
                        exchangeRate = c.Single(nullable: false),
                        commision = c.Decimal(precision: 18, scale: 2),
                        commisioninBase = c.Decimal(precision: 18, scale: 2),
                        marginExchangeRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        margin = c.Decimal(precision: 18, scale: 2),
                        BudgetedMargininBase = c.Decimal(precision: 18, scale: 2),
                        SalesBudgetedMargin = c.Decimal(precision: 18, scale: 2),
                        ActualMargin = c.Decimal(precision: 18, scale: 2),
                        ActualMargininBase = c.Decimal(precision: 18, scale: 2),
                        SalesActualMargin = c.Decimal(precision: 18, scale: 2),
                        maker = c.String(),
                        origin = c.String(),
                        OwnDescription = c.String(),
                        comments = c.String(),
                        deliveryTime = c.String(),
                        totalFOBValue = c.Double(nullable: false),
                        totalCFRValue = c.Double(nullable: false),
                        SOFOBValue = c.Double(nullable: false),
                        SOCFRValue = c.Double(nullable: false),
                        totalBaseFOBValue = c.Double(nullable: false),
                        totalBaseCFRValue = c.Double(nullable: false),
                        RemainingBaseFOBValue = c.Double(nullable: false),
                        RemainingBaseCFRValue = c.Double(nullable: false),
                        UnInvoicedTotalWeight = c.Decimal(precision: 18, scale: 2),
                        UnInvoicedTotalQuantity = c.Decimal(precision: 18, scale: 2),
                        ReceivedAmount = c.Double(nullable: false),
                        TotalWeight = c.Decimal(precision: 18, scale: 2),
                        TotalQuantity = c.Decimal(precision: 18, scale: 2),
                        SoAmountSER = c.Double(nullable: false),
                        isPercentTax = c.Boolean(nullable: false),
                        salesTax = c.Double(nullable: false),
                        customerCompany_Id = c.Int(nullable: false),
                        isReviewed = c.Boolean(),
                        needReview = c.Boolean(),
                        stage = c.String(),
                        dept_Id = c.Int(nullable: false),
                        CostSheet_Id = c.Int(),
                        CommissionSummarySheetId = c.Int(),
                        user_Id = c.Int(),
                        allocation_Id = c.Int(nullable: false),
                        principal_Id = c.Int(nullable: false),
                        company_Id = c.Int(),
                        saleInvoicetype = c.Int(nullable: false),
                        SaleOrderId = c.Int(),
                        currency_Id = c.Int(nullable: false),
                        paymentterm_Id = c.Int(nullable: false),
                        incoterm_Id = c.Int(nullable: false),
                        PendingForClosing = c.Boolean(),
                        isApproved = c.Boolean(),
                        ApprovedDate = c.DateTime(),
                        TitleValue1Id = c.Int(),
                        TitleValue2Id = c.Int(),
                        saleInvoiceStatus_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CommissionSummarySheets", t => t.CommissionSummarySheetId)
                .ForeignKey("dbo.tabCompany", t => t.company_Id)
                .ForeignKey("dbo.CostSheets", t => t.CostSheet_Id)
                .ForeignKey("dbo.Currencies", t => t.currency_Id, cascadeDelete: true)
                .ForeignKey("dbo.CustomerCompanies", t => t.customerCompany_Id, cascadeDelete: true)
                .ForeignKey("dbo.tabDepartment", t => t.dept_Id, cascadeDelete: true)
                .ForeignKey("dbo.Employees", t => t.allocation_Id, cascadeDelete: true)
                .ForeignKey("dbo.Incoterms", t => t.incoterm_Id, cascadeDelete: true)
                .ForeignKey("dbo.PaymentTerms", t => t.paymentterm_Id, cascadeDelete: true)
                .ForeignKey("dbo.Principals", t => t.principal_Id, cascadeDelete: true)
                .ForeignKey("dbo.SaleInvoiceStatus", t => t.saleInvoiceStatus_Id)
                .ForeignKey("dbo.SaleOrders", t => t.SaleOrderId)
                .ForeignKey("dbo.Incoterms", t => t.TitleValue1Id)
                .ForeignKey("dbo.Incoterms", t => t.TitleValue2Id)
                .ForeignKey("dbo.Users", t => t.user_Id)
                .Index(t => t.customerCompany_Id)
                .Index(t => t.dept_Id)
                .Index(t => t.CostSheet_Id)
                .Index(t => t.CommissionSummarySheetId)
                .Index(t => t.user_Id)
                .Index(t => t.allocation_Id)
                .Index(t => t.principal_Id)
                .Index(t => t.company_Id)
                .Index(t => t.SaleOrderId)
                .Index(t => t.currency_Id)
                .Index(t => t.paymentterm_Id)
                .Index(t => t.incoterm_Id)
                .Index(t => t.TitleValue1Id)
                .Index(t => t.TitleValue2Id)
                .Index(t => t.saleInvoiceStatus_Id);
            
            CreateTable(
                "dbo.SaleInvoiceStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.String(),
                        isApproved = c.Boolean(nullable: false),
                        isActive = c.Boolean(),
                        backcolor = c.String(),
                        forecolor = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.tabVendor", "SaleInvoice_Id", c => c.Int());
            AddColumn("dbo.ProcurementProducts", "UnInvoicedQuantity", c => c.Double(nullable: false));
            AddColumn("dbo.ProcurementProducts", "UnInvoicedWeight", c => c.Double(nullable: false));
            AddColumn("dbo.ProcurementProducts", "InvoicedQuantity", c => c.Double(nullable: false));
            AddColumn("dbo.ProcurementProducts", "InvoicedWeight", c => c.Double(nullable: false));
            AddColumn("dbo.ProcurementProducts", "SaleInvoice_Id", c => c.Int());
            AddColumn("dbo.SaleOrders", "BudgetedMarginPercent", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.SaleOrders", "ActualMarginPercent", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.SaleOrders", "RemainingFOBValue", c => c.Double(nullable: false));
            AddColumn("dbo.SaleOrders", "RemainingCFRValue", c => c.Double(nullable: false));
            AddColumn("dbo.SaleOrders", "UnInvoicedTotalWeight", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.SaleOrders", "UnInvoicedTotalQuantity", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.SaleOrders", "WarrantyId", c => c.Int());
            AddColumn("dbo.CostSheets", "deliveryTime", c => c.String());
            AddColumn("dbo.CostSheets", "maker", c => c.String());
            AddColumn("dbo.CostSheets", "origin", c => c.String());
            AddColumn("dbo.CostSheets", "SupplierWarrantyId", c => c.Int());
            AddColumn("dbo.CostSheets", "packing", c => c.String());
            AddColumn("dbo.CostSheets", "paymentterm_Id", c => c.Int());
            AddColumn("dbo.CostSheets", "incoterm_Id", c => c.Int());
            CreateIndex("dbo.tabVendor", "SaleInvoice_Id");
            CreateIndex("dbo.ProcurementProducts", "SaleInvoice_Id");
            CreateIndex("dbo.SaleOrders", "WarrantyId");
            CreateIndex("dbo.CostSheets", "SupplierWarrantyId");
            CreateIndex("dbo.CostSheets", "paymentterm_Id");
            CreateIndex("dbo.CostSheets", "incoterm_Id");
            AddForeignKey("dbo.CostSheets", "incoterm_Id", "dbo.Incoterms", "Id");
            AddForeignKey("dbo.CostSheets", "paymentterm_Id", "dbo.PaymentTerms", "Id");
            AddForeignKey("dbo.CostSheets", "SupplierWarrantyId", "dbo.Warranties", "Id");
            AddForeignKey("dbo.ProcurementProducts", "SaleInvoice_Id", "dbo.SaleInvoices", "Id");
            AddForeignKey("dbo.tabVendor", "SaleInvoice_Id", "dbo.SaleInvoices", "Id");
            AddForeignKey("dbo.SaleOrders", "WarrantyId", "dbo.Warranties", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SaleOrders", "WarrantyId", "dbo.Warranties");
            DropForeignKey("dbo.tabVendor", "SaleInvoice_Id", "dbo.SaleInvoices");
            DropForeignKey("dbo.SaleInvoices", "user_Id", "dbo.Users");
            DropForeignKey("dbo.SaleInvoices", "TitleValue2Id", "dbo.Incoterms");
            DropForeignKey("dbo.SaleInvoices", "TitleValue1Id", "dbo.Incoterms");
            DropForeignKey("dbo.SaleInvoices", "SaleOrderId", "dbo.SaleOrders");
            DropForeignKey("dbo.SaleInvoices", "saleInvoiceStatus_Id", "dbo.SaleInvoiceStatus");
            DropForeignKey("dbo.ProcurementProducts", "SaleInvoice_Id", "dbo.SaleInvoices");
            DropForeignKey("dbo.SaleInvoices", "principal_Id", "dbo.Principals");
            DropForeignKey("dbo.SaleInvoices", "paymentterm_Id", "dbo.PaymentTerms");
            DropForeignKey("dbo.SaleInvoices", "incoterm_Id", "dbo.Incoterms");
            DropForeignKey("dbo.SaleInvoices", "allocation_Id", "dbo.Employees");
            DropForeignKey("dbo.SaleInvoices", "dept_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.SaleInvoices", "customerCompany_Id", "dbo.CustomerCompanies");
            DropForeignKey("dbo.SaleInvoices", "currency_Id", "dbo.Currencies");
            DropForeignKey("dbo.SaleInvoices", "CostSheet_Id", "dbo.CostSheets");
            DropForeignKey("dbo.SaleInvoices", "company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.SaleInvoices", "CommissionSummarySheetId", "dbo.CommissionSummarySheets");
            DropForeignKey("dbo.CostSheets", "SupplierWarrantyId", "dbo.Warranties");
            DropForeignKey("dbo.Warranties", "user_Id", "dbo.Users");
            DropForeignKey("dbo.CostSheets", "paymentterm_Id", "dbo.PaymentTerms");
            DropForeignKey("dbo.CostSheets", "incoterm_Id", "dbo.Incoterms");
            DropIndex("dbo.SaleInvoices", new[] { "saleInvoiceStatus_Id" });
            DropIndex("dbo.SaleInvoices", new[] { "TitleValue2Id" });
            DropIndex("dbo.SaleInvoices", new[] { "TitleValue1Id" });
            DropIndex("dbo.SaleInvoices", new[] { "incoterm_Id" });
            DropIndex("dbo.SaleInvoices", new[] { "paymentterm_Id" });
            DropIndex("dbo.SaleInvoices", new[] { "currency_Id" });
            DropIndex("dbo.SaleInvoices", new[] { "SaleOrderId" });
            DropIndex("dbo.SaleInvoices", new[] { "company_Id" });
            DropIndex("dbo.SaleInvoices", new[] { "principal_Id" });
            DropIndex("dbo.SaleInvoices", new[] { "allocation_Id" });
            DropIndex("dbo.SaleInvoices", new[] { "user_Id" });
            DropIndex("dbo.SaleInvoices", new[] { "CommissionSummarySheetId" });
            DropIndex("dbo.SaleInvoices", new[] { "CostSheet_Id" });
            DropIndex("dbo.SaleInvoices", new[] { "dept_Id" });
            DropIndex("dbo.SaleInvoices", new[] { "customerCompany_Id" });
            DropIndex("dbo.Warranties", new[] { "user_Id" });
            DropIndex("dbo.CostSheets", new[] { "incoterm_Id" });
            DropIndex("dbo.CostSheets", new[] { "paymentterm_Id" });
            DropIndex("dbo.CostSheets", new[] { "SupplierWarrantyId" });
            DropIndex("dbo.SaleOrders", new[] { "WarrantyId" });
            DropIndex("dbo.ProcurementProducts", new[] { "SaleInvoice_Id" });
            DropIndex("dbo.tabVendor", new[] { "SaleInvoice_Id" });
            DropColumn("dbo.CostSheets", "incoterm_Id");
            DropColumn("dbo.CostSheets", "paymentterm_Id");
            DropColumn("dbo.CostSheets", "packing");
            DropColumn("dbo.CostSheets", "SupplierWarrantyId");
            DropColumn("dbo.CostSheets", "origin");
            DropColumn("dbo.CostSheets", "maker");
            DropColumn("dbo.CostSheets", "deliveryTime");
            DropColumn("dbo.SaleOrders", "WarrantyId");
            DropColumn("dbo.SaleOrders", "UnInvoicedTotalQuantity");
            DropColumn("dbo.SaleOrders", "UnInvoicedTotalWeight");
            DropColumn("dbo.SaleOrders", "RemainingCFRValue");
            DropColumn("dbo.SaleOrders", "RemainingFOBValue");
            DropColumn("dbo.SaleOrders", "ActualMarginPercent");
            DropColumn("dbo.SaleOrders", "BudgetedMarginPercent");
            DropColumn("dbo.ProcurementProducts", "SaleInvoice_Id");
            DropColumn("dbo.ProcurementProducts", "InvoicedWeight");
            DropColumn("dbo.ProcurementProducts", "InvoicedQuantity");
            DropColumn("dbo.ProcurementProducts", "UnInvoicedWeight");
            DropColumn("dbo.ProcurementProducts", "UnInvoicedQuantity");
            DropColumn("dbo.tabVendor", "SaleInvoice_Id");
            DropTable("dbo.SaleInvoiceStatus");
            DropTable("dbo.SaleInvoices");
            DropTable("dbo.Warranties");
        }
    }
}
