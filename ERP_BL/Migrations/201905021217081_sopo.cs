namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sopo : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PurchaseOrders", "offer_Id", "dbo.Offers");
            DropForeignKey("dbo.PurchaseOrders", "vendor_Id", "dbo.tabVendor");
            DropIndex("dbo.PurchaseOrders", new[] { "vendor_Id" });
            DropIndex("dbo.PurchaseOrders", new[] { "offer_Id" });
            CreateTable(
                "dbo.SaleOrders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        referenceNo = c.String(),
                        SalesReferenceNo = c.String(),
                        FinanceRefrenceNo = c.String(),
                        commisionRefrenceNo = c.String(),
                        offerReferenceNo = c.String(),
                        saleOrderDate = c.DateTime(),
                        CreationDate = c.DateTime(),
                        deliveryDate = c.DateTime(),
                        shipmentDate = c.DateTime(),
                        orderConfirmationDate = c.DateTime(),
                        billLaddingDate = c.DateTime(),
                        lCDate = c.DateTime(),
                        materialReciptDate = c.DateTime(),
                        revisedShipmentDate = c.DateTime(),
                        targetYear = c.Int(nullable: false),
                        targetMonth = c.Int(nullable: false),
                        lCnumber = c.String(),
                        exchangeRate = c.Single(nullable: false),
                        commision = c.Decimal(precision: 18, scale: 2),
                        marginExchangeRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        margin = c.Decimal(precision: 18, scale: 2),
                        maker = c.String(),
                        origin = c.String(),
                        OwnDescription = c.String(),
                        comments = c.String(),
                        deliveryTime = c.String(),
                        totalFOBValue = c.Double(nullable: false),
                        totalCFRValue = c.Double(nullable: false),
                        totalBaseFOBValue = c.Double(nullable: false),
                        totalBaseCFRValue = c.Double(nullable: false),
                        isPercentTax = c.Boolean(nullable: false),
                        salesTax = c.Double(nullable: false),
                        customerCompany_Id = c.Int(nullable: false),
                        dept_Id = c.Int(nullable: false),
                        user_Id = c.Int(),
                        allocation_Id = c.Int(nullable: false),
                        vendor_Id = c.Int(nullable: false),
                        principal_Id = c.Int(nullable: false),
                        company_Id = c.Int(),
                        saleOrdertype = c.Int(nullable: false),
                        vendorPaymentId = c.Int(),
                        offer_Id = c.Int(),
                        bid_Id = c.Int(),
                        currency_Id = c.Int(nullable: false),
                        paymentterm_Id = c.Int(nullable: false),
                        incoterm_Id = c.Int(nullable: false),
                        PendingForClosing = c.Boolean(),
                        isApproved = c.Boolean(),
                        ApprovedDate = c.DateTime(),
                        TitleValue1Id = c.Int(),
                        TitleValue2Id = c.Int(),
                        saleOrderStatus_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Bids", t => t.bid_Id)
                .ForeignKey("dbo.tabCompany", t => t.company_Id)
                .ForeignKey("dbo.Currencies", t => t.currency_Id, cascadeDelete: true)
                .ForeignKey("dbo.CustomerCompanies", t => t.customerCompany_Id, cascadeDelete: true)
                .ForeignKey("dbo.tabDepartment", t => t.dept_Id, cascadeDelete: true)
                .ForeignKey("dbo.Employees", t => t.allocation_Id, cascadeDelete: true)
                .ForeignKey("dbo.Incoterms", t => t.incoterm_Id, cascadeDelete: true)
                .ForeignKey("dbo.Offers", t => t.offer_Id)
                .ForeignKey("dbo.PaymentTerms", t => t.paymentterm_Id, cascadeDelete: true)
                .ForeignKey("dbo.Principals", t => t.principal_Id, cascadeDelete: true)
                .ForeignKey("dbo.SaleOrderStatus", t => t.saleOrderStatus_Id)
                .ForeignKey("dbo.Incoterms", t => t.TitleValue1Id)
                .ForeignKey("dbo.Incoterms", t => t.TitleValue2Id)
                .ForeignKey("dbo.Users", t => t.user_Id)
                .ForeignKey("dbo.tabVendor", t => t.vendor_Id, cascadeDelete: true)
                .ForeignKey("dbo.VendorPaymentStatus", t => t.vendorPaymentId)
                .Index(t => t.customerCompany_Id)
                .Index(t => t.dept_Id)
                .Index(t => t.user_Id)
                .Index(t => t.allocation_Id)
                .Index(t => t.vendor_Id)
                .Index(t => t.principal_Id)
                .Index(t => t.company_Id)
                .Index(t => t.vendorPaymentId)
                .Index(t => t.offer_Id)
                .Index(t => t.bid_Id)
                .Index(t => t.currency_Id)
                .Index(t => t.paymentterm_Id)
                .Index(t => t.incoterm_Id)
                .Index(t => t.TitleValue1Id)
                .Index(t => t.TitleValue2Id)
                .Index(t => t.saleOrderStatus_Id);
            
            //CreateTable(
            //    "dbo.SaleOrderStatus",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Status = c.String(),
            //            isApproved = c.Boolean(nullable: false),
            //            isActive = c.Boolean(),
            //            backcolor = c.String(),
            //            forecolor = c.String(),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.tabVendor", "PurchaseOrder_Id", c => c.Int());
            AddColumn("dbo.ProcurementProducts", "SaleOrder_Id", c => c.Int());
            AddColumn("dbo.PurchaseOrders", "saleOrderReferenceNo", c => c.String());
            AddColumn("dbo.PurchaseOrders", "saleOrder_Id", c => c.Int());
            CreateIndex("dbo.tabVendor", "PurchaseOrder_Id");
            CreateIndex("dbo.ProcurementProducts", "SaleOrder_Id");
            CreateIndex("dbo.PurchaseOrders", "saleOrder_Id");
            AddForeignKey("dbo.ProcurementProducts", "SaleOrder_Id", "dbo.SaleOrders", "Id");
            AddForeignKey("dbo.PurchaseOrders", "saleOrder_Id", "dbo.SaleOrders", "Id");
            AddForeignKey("dbo.tabVendor", "PurchaseOrder_Id", "dbo.PurchaseOrders", "Id");
            DropColumn("dbo.PurchaseOrders", "offerReferenceNo");
            DropColumn("dbo.PurchaseOrders", "vendor_Id");
            DropColumn("dbo.PurchaseOrders", "offer_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PurchaseOrders", "offer_Id", c => c.Int());
            AddColumn("dbo.PurchaseOrders", "vendor_Id", c => c.Int(nullable: false));
            AddColumn("dbo.PurchaseOrders", "offerReferenceNo", c => c.String());
            DropForeignKey("dbo.tabVendor", "PurchaseOrder_Id", "dbo.PurchaseOrders");
            DropForeignKey("dbo.PurchaseOrders", "saleOrder_Id", "dbo.SaleOrders");
            DropForeignKey("dbo.SaleOrders", "vendorPaymentId", "dbo.VendorPaymentStatus");
            DropForeignKey("dbo.SaleOrders", "vendor_Id", "dbo.tabVendor");
            DropForeignKey("dbo.SaleOrders", "user_Id", "dbo.Users");
            DropForeignKey("dbo.SaleOrders", "TitleValue2Id", "dbo.Incoterms");
            DropForeignKey("dbo.SaleOrders", "TitleValue1Id", "dbo.Incoterms");
            DropForeignKey("dbo.SaleOrders", "saleOrderStatus_Id", "dbo.SaleOrderStatus");
            DropForeignKey("dbo.ProcurementProducts", "SaleOrder_Id", "dbo.SaleOrders");
            DropForeignKey("dbo.SaleOrders", "principal_Id", "dbo.Principals");
            DropForeignKey("dbo.SaleOrders", "paymentterm_Id", "dbo.PaymentTerms");
            DropForeignKey("dbo.SaleOrders", "offer_Id", "dbo.Offers");
            DropForeignKey("dbo.SaleOrders", "incoterm_Id", "dbo.Incoterms");
            DropForeignKey("dbo.SaleOrders", "allocation_Id", "dbo.Employees");
            DropForeignKey("dbo.SaleOrders", "dept_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.SaleOrders", "customerCompany_Id", "dbo.CustomerCompanies");
            DropForeignKey("dbo.SaleOrders", "currency_Id", "dbo.Currencies");
            DropForeignKey("dbo.SaleOrders", "company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.SaleOrders", "bid_Id", "dbo.Bids");
            DropIndex("dbo.SaleOrders", new[] { "saleOrderStatus_Id" });
            DropIndex("dbo.SaleOrders", new[] { "TitleValue2Id" });
            DropIndex("dbo.SaleOrders", new[] { "TitleValue1Id" });
            DropIndex("dbo.SaleOrders", new[] { "incoterm_Id" });
            DropIndex("dbo.SaleOrders", new[] { "paymentterm_Id" });
            DropIndex("dbo.SaleOrders", new[] { "currency_Id" });
            DropIndex("dbo.SaleOrders", new[] { "bid_Id" });
            DropIndex("dbo.SaleOrders", new[] { "offer_Id" });
            DropIndex("dbo.SaleOrders", new[] { "vendorPaymentId" });
            DropIndex("dbo.SaleOrders", new[] { "company_Id" });
            DropIndex("dbo.SaleOrders", new[] { "principal_Id" });
            DropIndex("dbo.SaleOrders", new[] { "vendor_Id" });
            DropIndex("dbo.SaleOrders", new[] { "allocation_Id" });
            DropIndex("dbo.SaleOrders", new[] { "user_Id" });
            DropIndex("dbo.SaleOrders", new[] { "dept_Id" });
            DropIndex("dbo.SaleOrders", new[] { "customerCompany_Id" });
            DropIndex("dbo.PurchaseOrders", new[] { "saleOrder_Id" });
            DropIndex("dbo.ProcurementProducts", new[] { "SaleOrder_Id" });
            DropIndex("dbo.tabVendor", new[] { "PurchaseOrder_Id" });
            DropColumn("dbo.PurchaseOrders", "saleOrder_Id");
            DropColumn("dbo.PurchaseOrders", "saleOrderReferenceNo");
            DropColumn("dbo.ProcurementProducts", "SaleOrder_Id");
            DropColumn("dbo.tabVendor", "PurchaseOrder_Id");
            DropTable("dbo.SaleOrderStatus");
            DropTable("dbo.SaleOrders");
            CreateIndex("dbo.PurchaseOrders", "offer_Id");
            CreateIndex("dbo.PurchaseOrders", "vendor_Id");
            AddForeignKey("dbo.PurchaseOrders", "vendor_Id", "dbo.tabVendor", "Id", cascadeDelete: true);
            AddForeignKey("dbo.PurchaseOrders", "offer_Id", "dbo.Offers", "Id");
        }
    }
}
