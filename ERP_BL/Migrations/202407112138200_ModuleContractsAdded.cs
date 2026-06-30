namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModuleContractsAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ModuleContracts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OfferReferenceNo = c.String(),
                        SalesReferenceNo = c.String(),
                        ModuleContractReferenceNo = c.String(),
                        commisionRefrenceNo = c.String(),
                        ModuleContractDate = c.DateTime(),
                        CreationDate = c.DateTime(),
                        LastStatusChangeDate = c.DateTime(),
                        OfferDate = c.DateTime(),
                        ModuleContractValidityDate = c.DateTime(),
                        deliveryDate = c.DateTime(),
                        maker = c.String(),
                        origin = c.String(),
                        OwnDescription = c.String(),
                        isVoid = c.Boolean(nullable: false),
                        responseDate = c.DateTime(),
                        exchngeRate = c.Single(nullable: false),
                        bidOpenDate = c.DateTime(),
                        alertDate = c.DateTime(),
                        closingDate = c.DateTime(),
                        commision = c.Decimal(precision: 18, scale: 2),
                        marginExchangeRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        margin = c.Decimal(precision: 18, scale: 2),
                        comments = c.String(),
                        totalFOBValue = c.Double(nullable: false),
                        totalCFRValue = c.Double(nullable: false),
                        totalBaseFOBValue = c.Double(nullable: false),
                        totalBaseCFRValue = c.Double(nullable: false),
                        isPercentTax = c.Boolean(nullable: false),
                        salesTax = c.Double(nullable: false),
                        TotalWeight = c.Decimal(precision: 18, scale: 2),
                        TotalQuantity = c.Decimal(precision: 18, scale: 2),
                        customerCompany_Id = c.Int(nullable: false),
                        dept_Id = c.Int(nullable: false),
                        user_Id = c.Int(),
                        allocation_Id = c.Int(nullable: false),
                        company_Id = c.Int(),
                        ModuleContracttype = c.Int(nullable: false),
                        principal_Id = c.Int(),
                        offer_Id = c.Int(),
                        isReviewed = c.Boolean(),
                        needReview = c.Boolean(),
                        stage = c.String(),
                        bid_Id = c.Int(),
                        currency_Id = c.Int(),
                        paymentterm_Id = c.Int(nullable: false),
                        incoterm_Id = c.Int(nullable: false),
                        PendingForClosing = c.Boolean(),
                        isApproved = c.Boolean(),
                        ApprovedDate = c.DateTime(),
                        TitleValue1Id = c.Int(),
                        TitleValue2Id = c.Int(),
                        uniqueNumber = c.String(),
                        transactionHolderId = c.Int(),
                        holderChangeDate = c.DateTime(nullable: false),
                        statusClass_Id = c.Int(),
                        InterDepartment_Id = c.Int(),
                        InterCompany_Id = c.Int(),
                        isInterCompany = c.Boolean(),
                        ModuleContractStatus_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Bids", t => t.bid_Id)
                .ForeignKey("dbo.Currencies", t => t.currency_Id)
                .ForeignKey("dbo.CustomerCompanies", t => t.customerCompany_Id, cascadeDelete: true)
                .ForeignKey("dbo.Employees", t => t.allocation_Id, cascadeDelete: true)
                .ForeignKey("dbo.Incoterms", t => t.incoterm_Id, cascadeDelete: true)
                .ForeignKey("dbo.ModuleContractStatus", t => t.ModuleContractStatus_Id)
                .ForeignKey("dbo.Offers", t => t.offer_Id)
                .ForeignKey("dbo.PaymentTerms", t => t.paymentterm_Id, cascadeDelete: true)
                .ForeignKey("dbo.Principals", t => t.principal_Id)
                .ForeignKey("dbo.StatusClasses", t => t.statusClass_Id)
                .ForeignKey("dbo.Incoterms", t => t.TitleValue1Id)
                .ForeignKey("dbo.Incoterms", t => t.TitleValue2Id)
                .ForeignKey("dbo.Employees", t => t.transactionHolderId)
                .ForeignKey("dbo.Users", t => t.user_Id)
                .ForeignKey("dbo.tabDepartment", t => t.dept_Id, cascadeDelete: true)
                .ForeignKey("dbo.tabDepartment", t => t.InterDepartment_Id)
                .ForeignKey("dbo.tabCompany", t => t.company_Id)
                .ForeignKey("dbo.tabCompany", t => t.InterCompany_Id)
                .Index(t => t.customerCompany_Id)
                .Index(t => t.dept_Id)
                .Index(t => t.user_Id)
                .Index(t => t.allocation_Id)
                .Index(t => t.company_Id)
                .Index(t => t.principal_Id)
                .Index(t => t.offer_Id)
                .Index(t => t.bid_Id)
                .Index(t => t.currency_Id)
                .Index(t => t.paymentterm_Id)
                .Index(t => t.incoterm_Id)
                .Index(t => t.TitleValue1Id)
                .Index(t => t.TitleValue2Id)
                .Index(t => t.transactionHolderId)
                .Index(t => t.statusClass_Id)
                .Index(t => t.InterDepartment_Id)
                .Index(t => t.InterCompany_Id)
                .Index(t => t.ModuleContractStatus_Id);
            
            CreateTable(
                "dbo.ModuleContractStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.String(),
                        isApproved = c.Boolean(nullable: false),
                        isActive = c.Boolean(),
                        backcolor = c.String(),
                        forecolor = c.String(),
                        isDisable = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ModuleContractStatusStatusClasses",
                c => new
                    {
                        ModuleContractStatus_Id = c.Int(nullable: false),
                        StatusClass_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ModuleContractStatus_Id, t.StatusClass_Id })
                .ForeignKey("dbo.ModuleContractStatus", t => t.ModuleContractStatus_Id, cascadeDelete: true)
                .ForeignKey("dbo.StatusClasses", t => t.StatusClass_Id, cascadeDelete: true)
                .Index(t => t.ModuleContractStatus_Id)
                .Index(t => t.StatusClass_Id);
            
            CreateTable(
                "dbo.ModuleContractVendors",
                c => new
                    {
                        ModuleContract_Id = c.Int(nullable: false),
                        Vendor_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ModuleContract_Id, t.Vendor_Id })
                .ForeignKey("dbo.ModuleContracts", t => t.ModuleContract_Id, cascadeDelete: true)
                .ForeignKey("dbo.tabVendor", t => t.Vendor_Id, cascadeDelete: true)
                .Index(t => t.ModuleContract_Id)
                .Index(t => t.Vendor_Id);
            
            AddColumn("dbo.tabDepartment", "IsModuleContractType", c => c.Boolean(nullable: false));
            AddColumn("dbo.Tasks", "moduleContractId", c => c.Int());
            AddColumn("dbo.ProcurementProducts", "ModuleContract_Id", c => c.Int());
            AddColumn("dbo.BookerStatementItems", "ModuleContractId", c => c.Int());
            AddColumn("dbo.ComparativeStatements", "ModuleContract_Id", c => c.Int());
            AddColumn("dbo.SaleOrders", "moduleContract_Id", c => c.Int());
            AddColumn("dbo.MemorandumSales", "ModuleContract_Id", c => c.Int());
            AddColumn("dbo.SalesReceipts", "saleOrderId", c => c.Int());
            AddColumn("dbo.SalesReceipts", "purchaseOrderId", c => c.Int());
            AddColumn("dbo.AttachmentCategories", "ModuleContract", c => c.Int());
            CreateIndex("dbo.Tasks", "moduleContractId");
            CreateIndex("dbo.ProcurementProducts", "ModuleContract_Id");
            CreateIndex("dbo.BookerStatementItems", "ModuleContractId");
            CreateIndex("dbo.ComparativeStatements", "ModuleContract_Id");
            CreateIndex("dbo.SaleOrders", "moduleContract_Id");
            CreateIndex("dbo.MemorandumSales", "ModuleContract_Id");
            CreateIndex("dbo.SalesReceipts", "saleOrderId");
            CreateIndex("dbo.SalesReceipts", "purchaseOrderId");
            AddForeignKey("dbo.BookerStatementItems", "ModuleContractId", "dbo.ModuleContracts", "Id");
            AddForeignKey("dbo.SaleOrders", "moduleContract_Id", "dbo.ModuleContracts", "Id");
            AddForeignKey("dbo.SalesReceipts", "saleOrderId", "dbo.SaleOrders", "Id");
            AddForeignKey("dbo.ComparativeStatements", "ModuleContract_Id", "dbo.ModuleContracts", "Id");
            AddForeignKey("dbo.MemorandumSales", "ModuleContract_Id", "dbo.ModuleContracts", "Id");
            AddForeignKey("dbo.ProcurementProducts", "ModuleContract_Id", "dbo.ModuleContracts", "Id");
            AddForeignKey("dbo.SalesReceipts", "purchaseOrderId", "dbo.PurchaseOrders", "Id");
            AddForeignKey("dbo.Tasks", "moduleContractId", "dbo.ModuleContracts", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ModuleContracts", "InterCompany_Id", "dbo.tabCompany");
            DropForeignKey("dbo.ModuleContracts", "company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.ModuleContracts", "InterDepartment_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.ModuleContracts", "dept_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.Tasks", "moduleContractId", "dbo.ModuleContracts");
            DropForeignKey("dbo.SalesReceipts", "purchaseOrderId", "dbo.PurchaseOrders");
            DropForeignKey("dbo.ModuleContractVendors", "Vendor_Id", "dbo.tabVendor");
            DropForeignKey("dbo.ModuleContractVendors", "ModuleContract_Id", "dbo.ModuleContracts");
            DropForeignKey("dbo.ModuleContracts", "user_Id", "dbo.Users");
            DropForeignKey("dbo.ModuleContracts", "transactionHolderId", "dbo.Employees");
            DropForeignKey("dbo.ModuleContracts", "TitleValue2Id", "dbo.Incoterms");
            DropForeignKey("dbo.ModuleContracts", "TitleValue1Id", "dbo.Incoterms");
            DropForeignKey("dbo.ModuleContracts", "statusClass_Id", "dbo.StatusClasses");
            DropForeignKey("dbo.ProcurementProducts", "ModuleContract_Id", "dbo.ModuleContracts");
            DropForeignKey("dbo.ModuleContracts", "principal_Id", "dbo.Principals");
            DropForeignKey("dbo.ModuleContracts", "paymentterm_Id", "dbo.PaymentTerms");
            DropForeignKey("dbo.ModuleContracts", "offer_Id", "dbo.Offers");
            DropForeignKey("dbo.ModuleContractStatusStatusClasses", "StatusClass_Id", "dbo.StatusClasses");
            DropForeignKey("dbo.ModuleContractStatusStatusClasses", "ModuleContractStatus_Id", "dbo.ModuleContractStatus");
            DropForeignKey("dbo.ModuleContracts", "ModuleContractStatus_Id", "dbo.ModuleContractStatus");
            DropForeignKey("dbo.MemorandumSales", "ModuleContract_Id", "dbo.ModuleContracts");
            DropForeignKey("dbo.ModuleContracts", "incoterm_Id", "dbo.Incoterms");
            DropForeignKey("dbo.ModuleContracts", "allocation_Id", "dbo.Employees");
            DropForeignKey("dbo.ModuleContracts", "customerCompany_Id", "dbo.CustomerCompanies");
            DropForeignKey("dbo.ModuleContracts", "currency_Id", "dbo.Currencies");
            DropForeignKey("dbo.ComparativeStatements", "ModuleContract_Id", "dbo.ModuleContracts");
            DropForeignKey("dbo.SalesReceipts", "saleOrderId", "dbo.SaleOrders");
            DropForeignKey("dbo.SaleOrders", "moduleContract_Id", "dbo.ModuleContracts");
            DropForeignKey("dbo.BookerStatementItems", "ModuleContractId", "dbo.ModuleContracts");
            DropForeignKey("dbo.ModuleContracts", "bid_Id", "dbo.Bids");
            DropIndex("dbo.ModuleContractVendors", new[] { "Vendor_Id" });
            DropIndex("dbo.ModuleContractVendors", new[] { "ModuleContract_Id" });
            DropIndex("dbo.ModuleContractStatusStatusClasses", new[] { "StatusClass_Id" });
            DropIndex("dbo.ModuleContractStatusStatusClasses", new[] { "ModuleContractStatus_Id" });
            DropIndex("dbo.SalesReceipts", new[] { "purchaseOrderId" });
            DropIndex("dbo.SalesReceipts", new[] { "saleOrderId" });
            DropIndex("dbo.MemorandumSales", new[] { "ModuleContract_Id" });
            DropIndex("dbo.SaleOrders", new[] { "moduleContract_Id" });
            DropIndex("dbo.ComparativeStatements", new[] { "ModuleContract_Id" });
            DropIndex("dbo.ModuleContracts", new[] { "ModuleContractStatus_Id" });
            DropIndex("dbo.ModuleContracts", new[] { "InterCompany_Id" });
            DropIndex("dbo.ModuleContracts", new[] { "InterDepartment_Id" });
            DropIndex("dbo.ModuleContracts", new[] { "statusClass_Id" });
            DropIndex("dbo.ModuleContracts", new[] { "transactionHolderId" });
            DropIndex("dbo.ModuleContracts", new[] { "TitleValue2Id" });
            DropIndex("dbo.ModuleContracts", new[] { "TitleValue1Id" });
            DropIndex("dbo.ModuleContracts", new[] { "incoterm_Id" });
            DropIndex("dbo.ModuleContracts", new[] { "paymentterm_Id" });
            DropIndex("dbo.ModuleContracts", new[] { "currency_Id" });
            DropIndex("dbo.ModuleContracts", new[] { "bid_Id" });
            DropIndex("dbo.ModuleContracts", new[] { "offer_Id" });
            DropIndex("dbo.ModuleContracts", new[] { "principal_Id" });
            DropIndex("dbo.ModuleContracts", new[] { "company_Id" });
            DropIndex("dbo.ModuleContracts", new[] { "allocation_Id" });
            DropIndex("dbo.ModuleContracts", new[] { "user_Id" });
            DropIndex("dbo.ModuleContracts", new[] { "dept_Id" });
            DropIndex("dbo.ModuleContracts", new[] { "customerCompany_Id" });
            DropIndex("dbo.BookerStatementItems", new[] { "ModuleContractId" });
            DropIndex("dbo.ProcurementProducts", new[] { "ModuleContract_Id" });
            DropIndex("dbo.Tasks", new[] { "moduleContractId" });
            DropColumn("dbo.AttachmentCategories", "ModuleContract");
            DropColumn("dbo.SalesReceipts", "purchaseOrderId");
            DropColumn("dbo.SalesReceipts", "saleOrderId");
            DropColumn("dbo.MemorandumSales", "ModuleContract_Id");
            DropColumn("dbo.SaleOrders", "moduleContract_Id");
            DropColumn("dbo.ComparativeStatements", "ModuleContract_Id");
            DropColumn("dbo.BookerStatementItems", "ModuleContractId");
            DropColumn("dbo.ProcurementProducts", "ModuleContract_Id");
            DropColumn("dbo.Tasks", "moduleContractId");
            DropColumn("dbo.tabDepartment", "IsModuleContractType");
            DropTable("dbo.ModuleContractVendors");
            DropTable("dbo.ModuleContractStatusStatusClasses");
            DropTable("dbo.ModuleContractStatus");
            DropTable("dbo.ModuleContracts");
        }
    }
}
