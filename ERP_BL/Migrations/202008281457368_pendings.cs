namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pendings : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Bills",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        POReferenceNo = c.String(),
                        SOReferenceNo = c.String(),
                        SalesReferenceNo = c.String(),
                        FinanceRefrenceNo = c.String(),
                        VendorName = c.String(),
                        OfferReferenceNo = c.String(),
                        saleOrderDate = c.DateTime(),
                        BillDate = c.DateTime(),
                        CreationDate = c.DateTime(),
                        ClosingDate = c.DateTime(),
                        LastStatusChangeDate = c.DateTime(),
                        DeliveryDate = c.DateTime(),
                        ShipmentDate = c.DateTime(),
                        OrderConfirmationDate = c.DateTime(),
                        BillOfLaddingDate = c.DateTime(),
                        lCDate = c.DateTime(),
                        MaterialReciptDate = c.DateTime(),
                        RevisedShipmentDate = c.DateTime(),
                        PaymentDueStartDate = c.DateTime(),
                        ExpectedPayment = c.DateTime(),
                        PaymentDueAgeing = c.DateTime(),
                        CreditDays = c.Int(nullable: false),
                        TargetYear = c.Int(nullable: false),
                        TargetMonth = c.Int(nullable: false),
                        LCnumber = c.String(),
                        ExchangeRate = c.Single(nullable: false),
                        NetCommision = c.Double(),
                        Commision = c.Double(),
                        commisioninBase = c.Double(),
                        SOC_ER = c.Double(nullable: false),
                        SoAmountSOC_ER = c.Double(nullable: false),
                        marginExchangeRate = c.Double(nullable: false),
                        margin = c.Double(),
                        BudgetedMargininBase = c.Double(),
                        SalesBudgetedMargin = c.Double(),
                        BudgetedMarginPercent = c.Double(),
                        RevisedMargin = c.Double(),
                        RevisedMargininBase = c.Double(),
                        SalesRevisedMargin = c.Double(),
                        RevisedMarginPercent = c.Double(),
                        ActualMarginPercent = c.Double(),
                        ActualMargin = c.Double(),
                        ActualMargininBase = c.Double(),
                        SalesActualMargin = c.Double(),
                        transshipment = c.Boolean(),
                        packing = c.String(),
                        LCShipmentDate = c.DateTime(),
                        LCExpiryDate = c.DateTime(),
                        deliveryTerm = c.String(),
                        LCAmedmentNo = c.String(),
                        LCShipmentAmendmentDate = c.DateTime(),
                        LCExpiryAmedmentDate = c.DateTime(),
                        maker = c.String(),
                        origin = c.String(),
                        OwnDescription = c.String(),
                        comments = c.String(),
                        deliveryTime = c.String(),
                        totalFOBValue = c.Double(nullable: false),
                        totalCFRValue = c.Double(nullable: false),
                        RemainingFOBValue = c.Double(nullable: false),
                        RemainingCFRValue = c.Double(nullable: false),
                        totalBaseFOBValue = c.Double(nullable: false),
                        totalBaseCFRValue = c.Double(nullable: false),
                        UnInvoicedTotalWeight = c.Decimal(precision: 18, scale: 2),
                        UnInvoicedTotalQuantity = c.Decimal(precision: 18, scale: 2),
                        ReceivedAmount = c.Double(nullable: false),
                        TotalWeight = c.Decimal(precision: 18, scale: 2),
                        TotalQuantity = c.Decimal(precision: 18, scale: 2),
                        POAmountSER = c.Double(nullable: false),
                        isPercentTax = c.Boolean(nullable: false),
                        salesTax = c.Double(nullable: false),
                        stage = c.String(),
                        InvoiceStage = c.String(),
                        billType_Id = c.Int(),
                        isVoid = c.Boolean(nullable: false),
                        isReviewed = c.Boolean(),
                        needReview = c.Boolean(),
                        PendingForClosing = c.Boolean(),
                        PendingForReApproval = c.Boolean(),
                        isApproved = c.Boolean(),
                        ApprovedDate = c.DateTime(),
                        ReApprovalDate = c.DateTime(),
                        isReApproved = c.Boolean(),
                        saleOrder_Id = c.Int(),
                        purchaseOrder_Id = c.Int(),
                        CostSheet_Id = c.Int(),
                        CommissionSummarySheetId = c.Int(),
                        dept_Id = c.Int(nullable: false),
                        company_Id = c.Int(),
                        customerCompany_Id = c.Int(nullable: false),
                        SOWarrantyId = c.Int(),
                        POWarrantyId = c.Int(),
                        user_Id = c.Int(),
                        allocation_Id = c.Int(nullable: false),
                        vendorPaymentId = c.Int(),
                        vendor_Id = c.Int(),
                        SoPaymentterm_Id = c.Int(nullable: false),
                        POPaymentterm_Id = c.Int(nullable: false),
                        bid_Id = c.Int(),
                        currency_Id = c.Int(nullable: false),
                        SOCurrency_Id = c.Int(nullable: false),
                        incoterm_Id = c.Int(nullable: false),
                        TitleValue1Id = c.Int(),
                        TitleValue2Id = c.Int(),
                        BillStatus_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.allocation_Id, cascadeDelete: true)
                .ForeignKey("dbo.Bids", t => t.bid_Id)
                .ForeignKey("dbo.BillStatus", t => t.BillStatus_Id)
                .ForeignKey("dbo.BillTypes", t => t.billType_Id)
                .ForeignKey("dbo.CommissionSummarySheets", t => t.CommissionSummarySheetId)
                .ForeignKey("dbo.tabCompany", t => t.company_Id)
                .ForeignKey("dbo.CostSheets", t => t.CostSheet_Id)
                .ForeignKey("dbo.Currencies", t => t.currency_Id, cascadeDelete: true)
                .ForeignKey("dbo.CustomerCompanies", t => t.customerCompany_Id, cascadeDelete: true)
                .ForeignKey("dbo.tabDepartment", t => t.dept_Id, cascadeDelete: true)
                .ForeignKey("dbo.Incoterms", t => t.incoterm_Id, cascadeDelete: true)
                .ForeignKey("dbo.PaymentTerms", t => t.POPaymentterm_Id, cascadeDelete: true)
                .ForeignKey("dbo.Warranties", t => t.POWarrantyId)
                .ForeignKey("dbo.PurchaseOrders", t => t.purchaseOrder_Id)
                .ForeignKey("dbo.SaleOrders", t => t.saleOrder_Id)
                .ForeignKey("dbo.Currencies", t => t.SOCurrency_Id, cascadeDelete: false)
                .ForeignKey("dbo.PaymentTerms", t => t.SoPaymentterm_Id, cascadeDelete: false)
                .ForeignKey("dbo.Warranties", t => t.SOWarrantyId)
                .ForeignKey("dbo.Incoterms", t => t.TitleValue1Id)
                .ForeignKey("dbo.Incoterms", t => t.TitleValue2Id)
                .ForeignKey("dbo.Users", t => t.user_Id)
                .ForeignKey("dbo.tabVendor", t => t.vendor_Id)
                .ForeignKey("dbo.VendorPaymentStatus", t => t.vendorPaymentId)
                .Index(t => t.billType_Id)
                .Index(t => t.saleOrder_Id)
                .Index(t => t.purchaseOrder_Id)
                .Index(t => t.CostSheet_Id)
                .Index(t => t.CommissionSummarySheetId)
                .Index(t => t.dept_Id)
                .Index(t => t.company_Id)
                .Index(t => t.customerCompany_Id)
                .Index(t => t.SOWarrantyId)
                .Index(t => t.POWarrantyId)
                .Index(t => t.user_Id)
                .Index(t => t.allocation_Id)
                .Index(t => t.vendorPaymentId)
                .Index(t => t.vendor_Id)
                .Index(t => t.SoPaymentterm_Id)
                .Index(t => t.POPaymentterm_Id)
                .Index(t => t.bid_Id)
                .Index(t => t.currency_Id)
                .Index(t => t.SOCurrency_Id)
                .Index(t => t.incoterm_Id)
                .Index(t => t.TitleValue1Id)
                .Index(t => t.TitleValue2Id)
                .Index(t => t.BillStatus_Id);
            
            CreateTable(
                "dbo.BillStatus",
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
            
            CreateTable(
                "dbo.BillTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        billType = c.String(),
                        isActive = c.Boolean(nullable: false),
                        user_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.user_Id)
                .Index(t => t.user_Id);
            
            AddColumn("dbo.ProcurementProducts", "Bill_Id", c => c.Int());
            CreateIndex("dbo.ProcurementProducts", "Bill_Id");
            AddForeignKey("dbo.ProcurementProducts", "Bill_Id", "dbo.Bills", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Bills", "vendorPaymentId", "dbo.VendorPaymentStatus");
            DropForeignKey("dbo.Bills", "vendor_Id", "dbo.tabVendor");
            DropForeignKey("dbo.Bills", "user_Id", "dbo.Users");
            DropForeignKey("dbo.Bills", "TitleValue2Id", "dbo.Incoterms");
            DropForeignKey("dbo.Bills", "TitleValue1Id", "dbo.Incoterms");
            DropForeignKey("dbo.Bills", "SOWarrantyId", "dbo.Warranties");
            DropForeignKey("dbo.Bills", "SoPaymentterm_Id", "dbo.PaymentTerms");
            DropForeignKey("dbo.Bills", "SOCurrency_Id", "dbo.Currencies");
            DropForeignKey("dbo.Bills", "saleOrder_Id", "dbo.SaleOrders");
            DropForeignKey("dbo.Bills", "purchaseOrder_Id", "dbo.PurchaseOrders");
            DropForeignKey("dbo.ProcurementProducts", "Bill_Id", "dbo.Bills");
            DropForeignKey("dbo.Bills", "POWarrantyId", "dbo.Warranties");
            DropForeignKey("dbo.Bills", "POPaymentterm_Id", "dbo.PaymentTerms");
            DropForeignKey("dbo.Bills", "incoterm_Id", "dbo.Incoterms");
            DropForeignKey("dbo.Bills", "dept_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.Bills", "customerCompany_Id", "dbo.CustomerCompanies");
            DropForeignKey("dbo.Bills", "currency_Id", "dbo.Currencies");
            DropForeignKey("dbo.Bills", "CostSheet_Id", "dbo.CostSheets");
            DropForeignKey("dbo.Bills", "company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.Bills", "CommissionSummarySheetId", "dbo.CommissionSummarySheets");
            DropForeignKey("dbo.Bills", "billType_Id", "dbo.BillTypes");
            DropForeignKey("dbo.BillTypes", "user_Id", "dbo.Users");
            DropForeignKey("dbo.Bills", "BillStatus_Id", "dbo.BillStatus");
            DropForeignKey("dbo.Bills", "bid_Id", "dbo.Bids");
            DropForeignKey("dbo.Bills", "allocation_Id", "dbo.Employees");
            DropIndex("dbo.ProcurementProducts", new[] { "Bill_Id" });
            DropIndex("dbo.BillTypes", new[] { "user_Id" });
            DropIndex("dbo.Bills", new[] { "BillStatus_Id" });
            DropIndex("dbo.Bills", new[] { "TitleValue2Id" });
            DropIndex("dbo.Bills", new[] { "TitleValue1Id" });
            DropIndex("dbo.Bills", new[] { "incoterm_Id" });
            DropIndex("dbo.Bills", new[] { "SOCurrency_Id" });
            DropIndex("dbo.Bills", new[] { "currency_Id" });
            DropIndex("dbo.Bills", new[] { "bid_Id" });
            DropIndex("dbo.Bills", new[] { "POPaymentterm_Id" });
            DropIndex("dbo.Bills", new[] { "SoPaymentterm_Id" });
            DropIndex("dbo.Bills", new[] { "vendor_Id" });
            DropIndex("dbo.Bills", new[] { "vendorPaymentId" });
            DropIndex("dbo.Bills", new[] { "allocation_Id" });
            DropIndex("dbo.Bills", new[] { "user_Id" });
            DropIndex("dbo.Bills", new[] { "POWarrantyId" });
            DropIndex("dbo.Bills", new[] { "SOWarrantyId" });
            DropIndex("dbo.Bills", new[] { "customerCompany_Id" });
            DropIndex("dbo.Bills", new[] { "company_Id" });
            DropIndex("dbo.Bills", new[] { "dept_Id" });
            DropIndex("dbo.Bills", new[] { "CommissionSummarySheetId" });
            DropIndex("dbo.Bills", new[] { "CostSheet_Id" });
            DropIndex("dbo.Bills", new[] { "purchaseOrder_Id" });
            DropIndex("dbo.Bills", new[] { "saleOrder_Id" });
            DropIndex("dbo.Bills", new[] { "billType_Id" });
            DropColumn("dbo.ProcurementProducts", "Bill_Id");
            DropTable("dbo.BillTypes");
            DropTable("dbo.BillStatus");
            DropTable("dbo.Bills");
        }
    }
}
