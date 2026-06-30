namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class POCommentCategorySentNotification : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.CommentLogUsers", newName: "CommentLogUser1");
            RenameTable(name: "dbo.CustomerCompanyDepartments", newName: "DepartmentCustomerCompanies");
            RenameTable(name: "dbo.PurchaseOrderVendors", newName: "VendorPurchaseOrders");
            DropForeignKey("dbo.PurchaseOrders", "principal_Id", "dbo.Principals");
            DropIndex("dbo.PurchaseOrders", new[] { "principal_Id" });
            //DropIndex("dbo.PurchaseOrders", new[] { "purchaseOrderStatus_Id" });
            RenameColumn(table: "dbo.PurchaseOrders", name: "paymentterm_Id", newName: "POPaymentterm_Id");
            RenameIndex(table: "dbo.PurchaseOrders", name: "IX_paymentterm_Id", newName: "IX_POPaymentterm_Id");
            DropPrimaryKey("dbo.DepartmentCustomerCompanies");
            DropPrimaryKey("dbo.VendorPurchaseOrders");
            CreateTable(
                "dbo.CommentCategories",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    category = c.String(),
                    discription = c.String(),
                    isActive = c.Boolean(nullable: false),
                    user_Id = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.user_Id)
                .Index(t => t.user_Id);

            CreateTable(
                "dbo.TransactionItems",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    TransactionType = c.Int(nullable: true),
                    CommentCategory_Id = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CommentCategories", t => t.CommentCategory_Id)
                .Index(t => t.CommentCategory_Id);

            CreateTable(
                "dbo.CommentLogUsers",
                c => new
                {
                    CommentLog_Id = c.Int(nullable: false),
                    User_id = c.Int(nullable: false),
                })
                .PrimaryKey(t => new { t.CommentLog_Id, t.User_id });
            CreateIndex("dbo.CommentLogUsers", "CommentLog_Id");
            CreateIndex("dbo.CommentLogUsers", "User_id");
            AddForeignKey("dbo.CommentLogUsers", "CommentLog_Id", "CommentLogs", "Id", cascadeDelete: true);
            AddForeignKey("dbo.CommentLogUsers", "User_id", "Users", "id", cascadeDelete: true);

            AddColumn("dbo.PurchaseOrders", "POReferenceNo", c => c.String());
            AddColumn("dbo.PurchaseOrders", "SOReferenceNo", c => c.String());
            AddColumn("dbo.PurchaseOrders", "OfferReferenceNo", c => c.String());
            AddColumn("dbo.PurchaseOrders", "saleOrderDate", c => c.DateTime());
            AddColumn("dbo.PurchaseOrders", "ClosingDate", c => c.DateTime());
            AddColumn("dbo.PurchaseOrders", "LastStatusChangeDate", c => c.DateTime());
            AddColumn("dbo.PurchaseOrders", "BillOfLaddingDate", c => c.DateTime());
            AddColumn("dbo.PurchaseOrders", "PaymentDueStartDate", c => c.DateTime());
            AddColumn("dbo.PurchaseOrders", "ExpectedPayment", c => c.DateTime());
            AddColumn("dbo.PurchaseOrders", "PaymentDueAgeing", c => c.DateTime());
            AddColumn("dbo.PurchaseOrders", "CreditDays", c => c.Int(nullable: false));
            AddColumn("dbo.PurchaseOrders", "NetCommision", c => c.Double());
            AddColumn("dbo.PurchaseOrders", "commisioninBase", c => c.Double());
            AddColumn("dbo.PurchaseOrders", "SOC_ER", c => c.Double(nullable: false));
            AddColumn("dbo.PurchaseOrders", "SoAmountSOC_ER", c => c.Double(nullable: false));
            AddColumn("dbo.PurchaseOrders", "BudgetedMargininBase", c => c.Double());
            AddColumn("dbo.PurchaseOrders", "SalesBudgetedMargin", c => c.Double());
            AddColumn("dbo.PurchaseOrders", "BudgetedMarginPercent", c => c.Double());
            AddColumn("dbo.PurchaseOrders", "RevisedMargin", c => c.Double());
            AddColumn("dbo.PurchaseOrders", "RevisedMargininBase", c => c.Double());
            AddColumn("dbo.PurchaseOrders", "SalesRevisedMargin", c => c.Double());
            AddColumn("dbo.PurchaseOrders", "RevisedMarginPercent", c => c.Double());
            AddColumn("dbo.PurchaseOrders", "ActualMarginPercent", c => c.Double());
            AddColumn("dbo.PurchaseOrders", "ActualMargin", c => c.Double());
            AddColumn("dbo.PurchaseOrders", "ActualMargininBase", c => c.Double());
            AddColumn("dbo.PurchaseOrders", "SalesActualMargin", c => c.Double());
            AddColumn("dbo.PurchaseOrders", "transshipment", c => c.Boolean());
            AddColumn("dbo.PurchaseOrders", "packing", c => c.String());
            AddColumn("dbo.PurchaseOrders", "LCShipmentDate", c => c.DateTime());
            AddColumn("dbo.PurchaseOrders", "LCExpiryDate", c => c.DateTime());
            AddColumn("dbo.PurchaseOrders", "deliveryTerm", c => c.String());
            AddColumn("dbo.PurchaseOrders", "LCAmedmentNo", c => c.String());
            AddColumn("dbo.PurchaseOrders", "LCShipmentAmendmentDate", c => c.DateTime());
            AddColumn("dbo.PurchaseOrders", "LCExpiryAmedmentDate", c => c.DateTime());
            AddColumn("dbo.PurchaseOrders", "RemainingFOBValue", c => c.Double(nullable: false));
            AddColumn("dbo.PurchaseOrders", "RemainingCFRValue", c => c.Double(nullable: false));
            AddColumn("dbo.PurchaseOrders", "UnInvoicedTotalWeight", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.PurchaseOrders", "UnInvoicedTotalQuantity", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.PurchaseOrders", "ReceivedAmount", c => c.Double(nullable: false));
            AddColumn("dbo.PurchaseOrders", "TotalWeight", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.PurchaseOrders", "TotalQuantity", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.PurchaseOrders", "POAmountSER", c => c.Double(nullable: false));
            AddColumn("dbo.PurchaseOrders", "stage", c => c.String());
            AddColumn("dbo.PurchaseOrders", "InvoiceStage", c => c.String());
            AddColumn("dbo.PurchaseOrders", "isVoid", c => c.Boolean(nullable: false));
            AddColumn("dbo.PurchaseOrders", "isReviewed", c => c.Boolean());
            AddColumn("dbo.PurchaseOrders", "needReview", c => c.Boolean());
            AddColumn("dbo.PurchaseOrders", "PendingForReApproval", c => c.Boolean());
            AddColumn("dbo.PurchaseOrders", "ReApprovalDate", c => c.DateTime());
            AddColumn("dbo.PurchaseOrders", "isReApproved", c => c.Boolean());
            AddColumn("dbo.PurchaseOrders", "CostSheet_Id", c => c.Int());
            AddColumn("dbo.PurchaseOrders", "CommissionSummarySheetId", c => c.Int());
            AddColumn("dbo.PurchaseOrders", "SOWarrantyId", c => c.Int());
            AddColumn("dbo.PurchaseOrders", "POWarrantyId", c => c.Int());
            AddColumn("dbo.PurchaseOrders", "SoPaymentterm_Id", c => c.Int(nullable: false));
            AddColumn("dbo.PurchaseOrders", "SOCurrency_Id", c => c.Int(nullable: false));
            //AddColumn("dbo.PurchaseOrderStatus", "HierarchicalIndex", c => c.Int(nullable: false));
            AddColumn("dbo.CommentLogs", "AssigneeId", c => c.Int());
            AddColumn("dbo.CommentLogs", "ReplyCommentId", c => c.Int());
            AddColumn("dbo.CommentLogs", "CategoryId", c => c.Int());
            AddColumn("dbo.CommentLogs", "Subject", c => c.String());
            AddColumn("dbo.CommentLogs", "isReply", c => c.Boolean(nullable: false));
            AddColumn("dbo.Notifications", "SendingUserId", c => c.Int());
            AlterColumn("dbo.PurchaseOrders", "Commision", c => c.Double());
            AlterColumn("dbo.PurchaseOrders", "marginExchangeRate", c => c.Double(nullable: false));
            AlterColumn("dbo.PurchaseOrders", "margin", c => c.Double());
            AddPrimaryKey("dbo.DepartmentCustomerCompanies", new[] { "Department_Id", "CustomerCompany_Id" });
            AddPrimaryKey("dbo.VendorPurchaseOrders", new[] { "Vendor_Id", "PurchaseOrder_Id" });
            CreateIndex("dbo.CommentLogs", "AssigneeId");
            CreateIndex("dbo.CommentLogs", "ReplyCommentId");
            CreateIndex("dbo.CommentLogs", "CategoryId");
            CreateIndex("dbo.PurchaseOrders", "CostSheet_Id");
            CreateIndex("dbo.PurchaseOrders", "CommissionSummarySheetId");
            CreateIndex("dbo.PurchaseOrders", "SOWarrantyId");
            CreateIndex("dbo.PurchaseOrders", "POWarrantyId");
            CreateIndex("dbo.PurchaseOrders", "SoPaymentterm_Id");
            CreateIndex("dbo.PurchaseOrders", "SOCurrency_Id");
            //CreateIndex("dbo.PurchaseOrders", "PurchaseOrderStatus_Id");
            CreateIndex("dbo.Notifications", "SendingUserId");
            AddForeignKey("dbo.CommentLogs", "AssigneeId", "dbo.Employees", "EmpId");
            AddForeignKey("dbo.PurchaseOrders", "CommissionSummarySheetId", "dbo.CommissionSummarySheets", "Id");
            AddForeignKey("dbo.PurchaseOrders", "SoPaymentterm_Id", "dbo.PaymentTerms", "Id", cascadeDelete: false);
            AddForeignKey("dbo.PurchaseOrders", "POWarrantyId", "dbo.Warranties", "Id");
            AddForeignKey("dbo.PurchaseOrders", "SOWarrantyId", "dbo.Warranties", "Id");
            AddForeignKey("dbo.PurchaseOrders", "CostSheet_Id", "dbo.CostSheets", "Id");
            AddForeignKey("dbo.PurchaseOrders", "SOCurrency_Id", "dbo.Currencies", "Id", cascadeDelete: false);
            AddForeignKey("dbo.CommentLogs", "CategoryId", "dbo.CommentCategories", "Id");
            AddForeignKey("dbo.CommentLogs", "ReplyCommentId", "dbo.CommentLogs", "Id");
            AddForeignKey("dbo.Notifications", "SendingUserId", "dbo.Users", "id");
            DropColumn("dbo.PurchaseOrders", "referenceNo");
            DropColumn("dbo.PurchaseOrders", "saleOrderReferenceNo");
            DropColumn("dbo.PurchaseOrders", "billLaddingDate");
            DropColumn("dbo.PurchaseOrders", "principal_Id");
        }

        public override void Down()
        {
            AddColumn("dbo.PurchaseOrders", "principal_Id", c => c.Int(nullable: true));
            AddColumn("dbo.PurchaseOrders", "billLaddingDate", c => c.DateTime());
            AddColumn("dbo.PurchaseOrders", "saleOrderReferenceNo", c => c.String());
            AddColumn("dbo.PurchaseOrders", "referenceNo", c => c.String());
            DropForeignKey("dbo.Notifications", "SendingUserId", "dbo.Users");
            DropForeignKey("dbo.CommentLogs", "ReplyCommentId", "dbo.CommentLogs");
            DropForeignKey("dbo.CommentLogUsers", "User_id", "dbo.Users");
            DropForeignKey("dbo.CommentLogUsers", "CommentLog_Id", "dbo.CommentLogs");
            DropForeignKey("dbo.CommentLogs", "CategoryId", "dbo.CommentCategories");
            DropForeignKey("dbo.CommentCategories", "user_Id", "dbo.Users");
            DropForeignKey("dbo.TransactionItems", "CommentCategory_Id", "dbo.CommentCategories");
            DropForeignKey("dbo.PurchaseOrders", "SOCurrency_Id", "dbo.Currencies");
            DropForeignKey("dbo.PurchaseOrders", "CostSheet_Id", "dbo.CostSheets");
            DropForeignKey("dbo.PurchaseOrders", "SOWarrantyId", "dbo.Warranties");
            DropForeignKey("dbo.PurchaseOrders", "POWarrantyId", "dbo.Warranties");
            DropForeignKey("dbo.PurchaseOrders", "SoPaymentterm_Id", "dbo.PaymentTerms");
            DropForeignKey("dbo.PurchaseOrders", "CommissionSummarySheetId", "dbo.CommissionSummarySheets");
            DropForeignKey("dbo.CommentLogs", "AssigneeId", "dbo.Employees");
            DropIndex("dbo.CommentLogUsers", new[] { "User_idCC" });
            DropIndex("dbo.CommentLogUsers", new[] { "CommentLog_IdCC" });
            DropIndex("dbo.Notifications", new[] { "SendingUserId" });
            DropIndex("dbo.TransactionItems", new[] { "CommentCategory_Id" });
            DropIndex("dbo.CommentCategories", new[] { "user_Id" });
            //DropIndex("dbo.PurchaseOrders", new[] { "PurchaseOrderStatus_Id" });
            DropIndex("dbo.PurchaseOrders", new[] { "SOCurrency_Id" });
            DropIndex("dbo.PurchaseOrders", new[] { "SoPaymentterm_Id" });
            DropIndex("dbo.PurchaseOrders", new[] { "POWarrantyId" });
            DropIndex("dbo.PurchaseOrders", new[] { "SOWarrantyId" });
            DropIndex("dbo.PurchaseOrders", new[] { "CommissionSummarySheetId" });
            DropIndex("dbo.PurchaseOrders", new[] { "CostSheet_Id" });
            DropIndex("dbo.CommentLogs", new[] { "CategoryId" });
            DropIndex("dbo.CommentLogs", new[] { "ReplyCommentId" });
            DropIndex("dbo.CommentLogs", new[] { "AssigneeId" });
            DropPrimaryKey("dbo.VendorPurchaseOrders");
            DropPrimaryKey("dbo.DepartmentCustomerCompanies");
            AlterColumn("dbo.PurchaseOrders", "margin", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.PurchaseOrders", "marginExchangeRate", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.PurchaseOrders", "Commision", c => c.Decimal(precision: 18, scale: 2));
            DropColumn("dbo.Notifications", "SendingUserId");
            DropColumn("dbo.CommentLogs", "isReply");
            DropColumn("dbo.CommentLogs", "Subject");
            DropColumn("dbo.CommentLogs", "CategoryId");
            DropColumn("dbo.CommentLogs", "ReplyCommentId");
            DropColumn("dbo.CommentLogs", "AssigneeId");
            //DropColumn("dbo.PurchaseOrderStatus", "HierarchicalIndex");
            DropColumn("dbo.PurchaseOrders", "SOCurrency_Id");
            DropColumn("dbo.PurchaseOrders", "SoPaymentterm_Id");
            DropColumn("dbo.PurchaseOrders", "POWarrantyId");
            DropColumn("dbo.PurchaseOrders", "SOWarrantyId");
            DropColumn("dbo.PurchaseOrders", "CommissionSummarySheetId");
            DropColumn("dbo.PurchaseOrders", "CostSheet_Id");
            DropColumn("dbo.PurchaseOrders", "isReApproved");
            DropColumn("dbo.PurchaseOrders", "ReApprovalDate");
            DropColumn("dbo.PurchaseOrders", "PendingForReApproval");
            DropColumn("dbo.PurchaseOrders", "needReview");
            DropColumn("dbo.PurchaseOrders", "isReviewed");
            DropColumn("dbo.PurchaseOrders", "isVoid");
            DropColumn("dbo.PurchaseOrders", "InvoiceStage");
            DropColumn("dbo.PurchaseOrders", "stage");
            DropColumn("dbo.PurchaseOrders", "POAmountSER");
            DropColumn("dbo.PurchaseOrders", "TotalQuantity");
            DropColumn("dbo.PurchaseOrders", "TotalWeight");
            DropColumn("dbo.PurchaseOrders", "ReceivedAmount");
            DropColumn("dbo.PurchaseOrders", "UnInvoicedTotalQuantity");
            DropColumn("dbo.PurchaseOrders", "UnInvoicedTotalWeight");
            DropColumn("dbo.PurchaseOrders", "RemainingCFRValue");
            DropColumn("dbo.PurchaseOrders", "RemainingFOBValue");
            DropColumn("dbo.PurchaseOrders", "LCExpiryAmedmentDate");
            DropColumn("dbo.PurchaseOrders", "LCShipmentAmendmentDate");
            DropColumn("dbo.PurchaseOrders", "LCAmedmentNo");
            DropColumn("dbo.PurchaseOrders", "deliveryTerm");
            DropColumn("dbo.PurchaseOrders", "LCExpiryDate");
            DropColumn("dbo.PurchaseOrders", "LCShipmentDate");
            DropColumn("dbo.PurchaseOrders", "packing");
            DropColumn("dbo.PurchaseOrders", "transshipment");
            DropColumn("dbo.PurchaseOrders", "SalesActualMargin");
            DropColumn("dbo.PurchaseOrders", "ActualMargininBase");
            DropColumn("dbo.PurchaseOrders", "ActualMargin");
            DropColumn("dbo.PurchaseOrders", "ActualMarginPercent");
            DropColumn("dbo.PurchaseOrders", "RevisedMarginPercent");
            DropColumn("dbo.PurchaseOrders", "SalesRevisedMargin");
            DropColumn("dbo.PurchaseOrders", "RevisedMargininBase");
            DropColumn("dbo.PurchaseOrders", "RevisedMargin");
            DropColumn("dbo.PurchaseOrders", "BudgetedMarginPercent");
            DropColumn("dbo.PurchaseOrders", "SalesBudgetedMargin");
            DropColumn("dbo.PurchaseOrders", "BudgetedMargininBase");
            DropColumn("dbo.PurchaseOrders", "SoAmountSOC_ER");
            DropColumn("dbo.PurchaseOrders", "SOC_ER");
            DropColumn("dbo.PurchaseOrders", "commisioninBase");
            DropColumn("dbo.PurchaseOrders", "NetCommision");
            DropColumn("dbo.PurchaseOrders", "CreditDays");
            DropColumn("dbo.PurchaseOrders", "PaymentDueAgeing");
            DropColumn("dbo.PurchaseOrders", "ExpectedPayment");
            DropColumn("dbo.PurchaseOrders", "PaymentDueStartDate");
            DropColumn("dbo.PurchaseOrders", "BillOfLaddingDate");
            DropColumn("dbo.PurchaseOrders", "LastStatusChangeDate");
            DropColumn("dbo.PurchaseOrders", "ClosingDate");
            DropColumn("dbo.PurchaseOrders", "saleOrderDate");
            DropColumn("dbo.PurchaseOrders", "OfferReferenceNo");
            DropColumn("dbo.PurchaseOrders", "SOReferenceNo");
            DropColumn("dbo.PurchaseOrders", "POReferenceNo");
            DropTable("dbo.CommentLogUsers");
            DropTable("dbo.TransactionItems");
            DropTable("dbo.CommentCategories");
            AddPrimaryKey("dbo.VendorPurchaseOrders", new[] { "PurchaseOrder_Id", "Vendor_Id" });
            AddPrimaryKey("dbo.DepartmentCustomerCompanies", new[] { "CustomerCompany_Id", "Department_Id" });
            RenameIndex(table: "dbo.PurchaseOrders", name: "IX_POPaymentterm_Id", newName: "IX_paymentterm_Id");
            RenameColumn(table: "dbo.PurchaseOrders", name: "POPaymentterm_Id", newName: "paymentterm_Id");
            //CreateIndex("dbo.PurchaseOrders", "purchaseOrderStatus_Id");
            CreateIndex("dbo.PurchaseOrders", "principal_Id");
            AddForeignKey("dbo.PurchaseOrders", "principal_Id", "dbo.Principals", "Id", cascadeDelete: true);
            RenameTable(name: "dbo.VendorPurchaseOrders", newName: "PurchaseOrderVendors");
            RenameTable(name: "dbo.DepartmentCustomerCompanies", newName: "CustomerCompanyDepartments");
            RenameTable(name: "dbo.CommentLogUser1", newName: "CommentLogUsers");
        }
    }
}
