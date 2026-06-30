namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class vendorDepartmenTandRenaming : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProductDepartments",
                c => new
                    {
                        Product_Id = c.Int(nullable: false),
                        Department_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Product_Id, t.Department_Id })
                .ForeignKey("dbo.Products", t => t.Product_Id, cascadeDelete: true)
                .ForeignKey("dbo.tabDepartment", t => t.Department_Id, cascadeDelete: true)
                .Index(t => t.Product_Id)
                .Index(t => t.Department_Id);
            
            CreateTable(
                "dbo.VendorDepartments",
                c => new
                    {
                        Vendor_Id = c.Int(nullable: false),
                        Department_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Vendor_Id, t.Department_Id })
                .ForeignKey("dbo.tabVendor", t => t.Vendor_Id, cascadeDelete: true)
                .ForeignKey("dbo.tabDepartment", t => t.Department_Id, cascadeDelete: true)
                .Index(t => t.Vendor_Id)
                .Index(t => t.Department_Id);
            
            AddColumn("dbo.Inquiries", "DeliveryDueDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Inquiries", "lastSubmissionDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Inquiries", "OwnDescription", c => c.String());
            AddColumn("dbo.Inquiries", "Comments", c => c.String());
            AddColumn("dbo.Offers", "InquiryReferenceNo", c => c.String());
            AddColumn("dbo.Offers", "SalesReferenceNo", c => c.String());
            AddColumn("dbo.Offers", "offerReferenceNo", c => c.String());
            AddColumn("dbo.Offers", "commisionRefrenceNo", c => c.String());
            //AddColumn("dbo.Offers", "OfferDate", c => c.DateTime());
            AddColumn("dbo.Offers", "InquiryDate", c => c.DateTime());
            AddColumn("dbo.Offers", "OwnDescription", c => c.String());
            AddColumn("dbo.Offers", "marginExchangeRate", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Offers", "margin", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Offers", "totalBaseFOBValue", c => c.Double(nullable: false));
            AddColumn("dbo.Offers", "totalBaseCFRValue", c => c.Double(nullable: false));
            AddColumn("dbo.Principals", "targetAmount", c => c.Double(nullable: false));
            AddColumn("dbo.Principals", "marginTargetAmount", c => c.Double(nullable: false));
            AddColumn("dbo.PurchaseOrders", "SalesReferenceNo", c => c.String());
            AddColumn("dbo.PurchaseOrders", "FinanceRefrenceNo", c => c.String());
            AddColumn("dbo.PurchaseOrders", "commisionRefrenceNo", c => c.String());
            AddColumn("dbo.PurchaseOrders", "targetMonth", c => c.Int(nullable: false));
            AddColumn("dbo.PurchaseOrders", "exchangeRate", c => c.Single(nullable: false));
            AddColumn("dbo.PurchaseOrders", "marginExchangeRate", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.PurchaseOrders", "margin", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.PurchaseOrders", "OwnDescription", c => c.String());
            AddColumn("dbo.PurchaseOrders", "totalBaseFOBValue", c => c.Double(nullable: false));
            AddColumn("dbo.PurchaseOrders", "totalBaseCFRValue", c => c.Double(nullable: false));
            AlterColumn("dbo.InquiryStatus", "isActive", c => c.Boolean());
            AlterColumn("dbo.Offers", "offerValidityDate", c => c.DateTime());
            AlterColumn("dbo.Offers", "deliveryDate", c => c.DateTime());
            AlterColumn("dbo.Offers", "responseDate", c => c.DateTime());
            AlterColumn("dbo.Offers", "bidOpenDate", c => c.DateTime());
            AlterColumn("dbo.Offers", "alertDate", c => c.DateTime());
            AlterColumn("dbo.Offers", "closingDate", c => c.DateTime());
            //AlterColumn("dbo.Offers", "commision", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.Offers", "commision");
            AddColumn("dbo.Offers", "commision", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.OfferStatus", "isActive", c => c.Boolean());
            AlterColumn("dbo.PurchaseOrders", "purchaseOrderDate", c => c.DateTime());
            AlterColumn("dbo.PurchaseOrders", "deliveryDate", c => c.DateTime());
            AlterColumn("dbo.PurchaseOrders", "shipmentDate", c => c.DateTime());
            AlterColumn("dbo.PurchaseOrders", "orderConfirmationDate", c => c.DateTime());
            AlterColumn("dbo.PurchaseOrders", "billLaddingDate", c => c.DateTime());
            AlterColumn("dbo.PurchaseOrders", "lCDate", c => c.DateTime());
            AlterColumn("dbo.PurchaseOrders", "materialReciptDate", c => c.DateTime());
           // AlterColumn("dbo.PurchaseOrders", "commision", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.PurchaseOrders", "commision");
            AddColumn("dbo.PurchaseOrders", "commision", c => c.Decimal(nullable: false, precision: 18, scale: 2));

            AlterColumn("dbo.SaleOrderStatus", "isActive", c => c.Boolean());
            DropColumn("dbo.Inquiries", "responseDate");
            DropColumn("dbo.Inquiries", "closingDate");
            DropColumn("dbo.Offers", "referenceNo");
            DropColumn("dbo.Offers", "fileNo");
            DropColumn("dbo.Offers", "ourreferenceNo");
            DropColumn("dbo.Offers", "offerDate");
            DropColumn("dbo.Offers", "deliveryTime");
            DropColumn("dbo.PurchaseOrders", "fileNo");
            DropColumn("dbo.PurchaseOrders", "exchngeRate");
            DropStoredProcedure("dbo.Address_Insert");
            DropStoredProcedure("dbo.Address_Update");
            DropStoredProcedure("dbo.Address_Delete");
            DropStoredProcedure("dbo.Contact_Insert");
            DropStoredProcedure("dbo.Contact_Update");
            DropStoredProcedure("dbo.Contact_Delete");
            DropStoredProcedure("dbo.Currency_Insert");
            DropStoredProcedure("dbo.Currency_Update");
            DropStoredProcedure("dbo.Currency_Delete");
            DropStoredProcedure("dbo.Department_Insert");
            DropStoredProcedure("dbo.Department_Update");
            DropStoredProcedure("dbo.Department_Delete");
            DropStoredProcedure("dbo.CustomerCompany_Insert");
            DropStoredProcedure("dbo.CustomerCompany_Update");
            DropStoredProcedure("dbo.CustomerCompany_Delete");
            DropStoredProcedure("dbo.Employee_Insert");
            DropStoredProcedure("dbo.Employee_Update");
            DropStoredProcedure("dbo.Employee_Delete");
            DropStoredProcedure("dbo.User_Insert");
            DropStoredProcedure("dbo.User_Update");
            DropStoredProcedure("dbo.User_Delete");
            DropStoredProcedure("dbo.Inquiry_Insert");
            DropStoredProcedure("dbo.Inquiry_Update");
            DropStoredProcedure("dbo.Inquiry_Delete");
            DropStoredProcedure("dbo.InquiryStatus_Insert");
            DropStoredProcedure("dbo.InquiryStatus_Update");
            DropStoredProcedure("dbo.InquiryStatus_Delete");
            DropStoredProcedure("dbo.InquiryProduct_Insert");
            DropStoredProcedure("dbo.InquiryProduct_Update");
            DropStoredProcedure("dbo.InquiryProduct_Delete");
            DropStoredProcedure("dbo.Product_Insert");
            DropStoredProcedure("dbo.Product_Update");
            DropStoredProcedure("dbo.Product_Delete");
            DropStoredProcedure("dbo.Offer_Insert");
            DropStoredProcedure("dbo.Offer_Update");
            DropStoredProcedure("dbo.Offer_Delete");
            DropStoredProcedure("dbo.OfferStatus_Insert");
            DropStoredProcedure("dbo.OfferStatus_Update");
            DropStoredProcedure("dbo.OfferStatus_Delete");
            DropStoredProcedure("dbo.ProcurementProduct_Insert");
            DropStoredProcedure("dbo.ProcurementProduct_Update");
            DropStoredProcedure("dbo.ProcurementProduct_Delete");
            DropStoredProcedure("dbo.Vendor_Insert");
            DropStoredProcedure("dbo.Vendor_Update");
            DropStoredProcedure("dbo.Vendor_Delete");
            DropStoredProcedure("dbo.PurchaseOrder_Insert");
            DropStoredProcedure("dbo.PurchaseOrder_Update");
            DropStoredProcedure("dbo.PurchaseOrder_Delete");
            DropStoredProcedure("dbo.PurchaseOrderStatus_Insert");
            DropStoredProcedure("dbo.PurchaseOrderStatus_Update");
            DropStoredProcedure("dbo.PurchaseOrderStatus_Delete");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PurchaseOrders", "exchngeRate", c => c.Single(nullable: false));
            AddColumn("dbo.PurchaseOrders", "fileNo", c => c.String());
            AddColumn("dbo.Offers", "deliveryTime", c => c.String());
            AddColumn("dbo.Offers", "offerDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Offers", "ourreferenceNo", c => c.String());
            AddColumn("dbo.Offers", "fileNo", c => c.String());
            AddColumn("dbo.Offers", "referenceNo", c => c.String());
            AddColumn("dbo.Inquiries", "closingDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Inquiries", "responseDate", c => c.DateTime(nullable: false));
            DropForeignKey("dbo.VendorDepartments", "Department_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.VendorDepartments", "Vendor_Id", "dbo.tabVendor");
            DropForeignKey("dbo.ProductDepartments", "Department_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.ProductDepartments", "Product_Id", "dbo.Products");
            DropIndex("dbo.VendorDepartments", new[] { "Department_Id" });
            DropIndex("dbo.VendorDepartments", new[] { "Vendor_Id" });
            DropIndex("dbo.ProductDepartments", new[] { "Department_Id" });
            DropIndex("dbo.ProductDepartments", new[] { "Product_Id" });
            AlterColumn("dbo.SaleOrderStatus", "isActive", c => c.Boolean(nullable: false));
            AlterColumn("dbo.PurchaseOrders", "commision", c => c.String());
            AlterColumn("dbo.PurchaseOrders", "materialReciptDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.PurchaseOrders", "lCDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.PurchaseOrders", "billLaddingDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.PurchaseOrders", "orderConfirmationDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.PurchaseOrders", "shipmentDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.PurchaseOrders", "deliveryDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.PurchaseOrders", "purchaseOrderDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.OfferStatus", "isActive", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Offers", "commision", c => c.String());
            AlterColumn("dbo.Offers", "closingDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Offers", "alertDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Offers", "bidOpenDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Offers", "responseDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Offers", "deliveryDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Offers", "offerValidityDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.InquiryStatus", "isActive", c => c.Boolean(nullable: false));
            DropColumn("dbo.PurchaseOrders", "totalBaseCFRValue");
            DropColumn("dbo.PurchaseOrders", "totalBaseFOBValue");
            DropColumn("dbo.PurchaseOrders", "OwnDescription");
            DropColumn("dbo.PurchaseOrders", "margin");
            DropColumn("dbo.PurchaseOrders", "marginExchangeRate");
            DropColumn("dbo.PurchaseOrders", "exchangeRate");
            DropColumn("dbo.PurchaseOrders", "targetMonth");
            DropColumn("dbo.PurchaseOrders", "commisionRefrenceNo");
            DropColumn("dbo.PurchaseOrders", "FinanceRefrenceNo");
            DropColumn("dbo.PurchaseOrders", "SalesReferenceNo");
            DropColumn("dbo.Principals", "marginTargetAmount");
            DropColumn("dbo.Principals", "targetAmount");
            DropColumn("dbo.Offers", "totalBaseCFRValue");
            DropColumn("dbo.Offers", "totalBaseFOBValue");
            DropColumn("dbo.Offers", "margin");
            DropColumn("dbo.Offers", "marginExchangeRate");
            DropColumn("dbo.Offers", "OwnDescription");
            DropColumn("dbo.Offers", "InquiryDate");
            DropColumn("dbo.Offers", "OfferDate");
            DropColumn("dbo.Offers", "commisionRefrenceNo");
            DropColumn("dbo.Offers", "offerReferenceNo");
            DropColumn("dbo.Offers", "SalesReferenceNo");
            DropColumn("dbo.Offers", "InquiryReferenceNo");
            DropColumn("dbo.Inquiries", "Comments");
            DropColumn("dbo.Inquiries", "OwnDescription");
            DropColumn("dbo.Inquiries", "lastSubmissionDate");
            DropColumn("dbo.Inquiries", "DeliveryDueDate");
            DropTable("dbo.VendorDepartments");
            DropTable("dbo.ProductDepartments");
            throw new NotSupportedException("Scaffolding create or alter procedure operations is not supported in down methods.");
        }
    }
}
