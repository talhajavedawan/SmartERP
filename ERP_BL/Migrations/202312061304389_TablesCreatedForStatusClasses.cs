namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TablesCreatedForStatusClasses : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StatusClasses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClassName = c.String(),
                        transactionType = c.Int(nullable: false),
                        isApproved = c.Boolean(nullable: false),
                        isActive = c.Boolean(),
                        backcolor = c.String(),
                        forecolor = c.String(),
                        isDisable = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BillStatusStatusClasses",
                c => new
                    {
                        BillStatus_Id = c.Int(nullable: false),
                        StatusClass_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.BillStatus_Id, t.StatusClass_Id })
                .ForeignKey("dbo.BillStatus", t => t.BillStatus_Id, cascadeDelete: true)
                .ForeignKey("dbo.StatusClasses", t => t.StatusClass_Id, cascadeDelete: true)
                .Index(t => t.BillStatus_Id)
                .Index(t => t.StatusClass_Id);
            
            CreateTable(
                "dbo.InterBankTransferStatusStatusClasses",
                c => new
                    {
                        InterBankTransferStatus_Id = c.Int(nullable: false),
                        StatusClass_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.InterBankTransferStatus_Id, t.StatusClass_Id })
                .ForeignKey("dbo.InterBankTransferStatus", t => t.InterBankTransferStatus_Id, cascadeDelete: true)
                .ForeignKey("dbo.StatusClasses", t => t.StatusClass_Id, cascadeDelete: true)
                .Index(t => t.InterBankTransferStatus_Id)
                .Index(t => t.StatusClass_Id);
            
            CreateTable(
                "dbo.InquiryStatusStatusClasses",
                c => new
                    {
                        InquiryStatus_Id = c.Int(nullable: false),
                        StatusClass_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.InquiryStatus_Id, t.StatusClass_Id })
                .ForeignKey("dbo.InquiryStatus", t => t.InquiryStatus_Id, cascadeDelete: true)
                .ForeignKey("dbo.StatusClasses", t => t.StatusClass_Id, cascadeDelete: true)
                .Index(t => t.InquiryStatus_Id)
                .Index(t => t.StatusClass_Id);
            
            CreateTable(
                "dbo.PurchaseInvoiceStatusStatusClasses",
                c => new
                    {
                        PurchaseInvoiceStatus_Id = c.Int(nullable: false),
                        StatusClass_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.PurchaseInvoiceStatus_Id, t.StatusClass_Id })
                .ForeignKey("dbo.PurchaseInvoiceStatus", t => t.PurchaseInvoiceStatus_Id, cascadeDelete: true)
                .ForeignKey("dbo.StatusClasses", t => t.StatusClass_Id, cascadeDelete: true)
                .Index(t => t.PurchaseInvoiceStatus_Id)
                .Index(t => t.StatusClass_Id);
            
            CreateTable(
                "dbo.SaleInvoiceStatusStatusClasses",
                c => new
                    {
                        SaleInvoiceStatus_Id = c.Int(nullable: false),
                        StatusClass_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.SaleInvoiceStatus_Id, t.StatusClass_Id })
                .ForeignKey("dbo.SaleInvoiceStatus", t => t.SaleInvoiceStatus_Id, cascadeDelete: true)
                .ForeignKey("dbo.StatusClasses", t => t.StatusClass_Id, cascadeDelete: true)
                .Index(t => t.SaleInvoiceStatus_Id)
                .Index(t => t.StatusClass_Id);
            
            CreateTable(
                "dbo.SaleOrderStatusStatusClasses",
                c => new
                    {
                        SaleOrderStatus_Id = c.Int(nullable: false),
                        StatusClass_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.SaleOrderStatus_Id, t.StatusClass_Id })
                .ForeignKey("dbo.SaleOrderStatus", t => t.SaleOrderStatus_Id, cascadeDelete: true)
                .ForeignKey("dbo.StatusClasses", t => t.StatusClass_Id, cascadeDelete: true)
                .Index(t => t.SaleOrderStatus_Id)
                .Index(t => t.StatusClass_Id);
            
            CreateTable(
                "dbo.SalesReceiptStatusStatusClasses",
                c => new
                    {
                        SalesReceiptStatus_Id = c.Int(nullable: false),
                        StatusClass_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.SalesReceiptStatus_Id, t.StatusClass_Id })
                .ForeignKey("dbo.SalesReceiptStatus", t => t.SalesReceiptStatus_Id, cascadeDelete: true)
                .ForeignKey("dbo.StatusClasses", t => t.StatusClass_Id, cascadeDelete: true)
                .Index(t => t.SalesReceiptStatus_Id)
                .Index(t => t.StatusClass_Id);
            
            CreateTable(
                "dbo.OfferStatusStatusClasses",
                c => new
                    {
                        OfferStatus_Id = c.Int(nullable: false),
                        StatusClass_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.OfferStatus_Id, t.StatusClass_Id })
                .ForeignKey("dbo.OfferStatus", t => t.OfferStatus_Id, cascadeDelete: true)
                .ForeignKey("dbo.StatusClasses", t => t.StatusClass_Id, cascadeDelete: true)
                .Index(t => t.OfferStatus_Id)
                .Index(t => t.StatusClass_Id);
            
            CreateTable(
                "dbo.PurchaseOrderStatusStatusClasses",
                c => new
                    {
                        PurchaseOrderStatus_Id = c.Int(nullable: false),
                        StatusClass_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.PurchaseOrderStatus_Id, t.StatusClass_Id })
                .ForeignKey("dbo.PurchaseOrderStatus", t => t.PurchaseOrderStatus_Id, cascadeDelete: true)
                .ForeignKey("dbo.StatusClasses", t => t.StatusClass_Id, cascadeDelete: true)
                .Index(t => t.PurchaseOrderStatus_Id)
                .Index(t => t.StatusClass_Id);
            
            CreateTable(
                "dbo.PaymentStatusStatusClasses",
                c => new
                    {
                        PaymentStatus_Id = c.Int(nullable: false),
                        StatusClass_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.PaymentStatus_Id, t.StatusClass_Id })
                .ForeignKey("dbo.PaymentStatus", t => t.PaymentStatus_Id, cascadeDelete: true)
                .ForeignKey("dbo.StatusClasses", t => t.StatusClass_Id, cascadeDelete: true)
                .Index(t => t.PaymentStatus_Id)
                .Index(t => t.StatusClass_Id);
            
            CreateTable(
                "dbo.LoansAdvanceStatusStatusClasses",
                c => new
                    {
                        LoansAdvanceStatus_Id = c.Int(nullable: false),
                        StatusClass_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.LoansAdvanceStatus_Id, t.StatusClass_Id })
                .ForeignKey("dbo.LoansAdvanceStatus", t => t.LoansAdvanceStatus_Id, cascadeDelete: true)
                .ForeignKey("dbo.StatusClasses", t => t.StatusClass_Id, cascadeDelete: true)
                .Index(t => t.LoansAdvanceStatus_Id)
                .Index(t => t.StatusClass_Id);
            
            CreateTable(
                "dbo.LoansStatusStatusClasses",
                c => new
                    {
                        LoansStatus_Id = c.Int(nullable: false),
                        StatusClass_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.LoansStatus_Id, t.StatusClass_Id })
                .ForeignKey("dbo.LoansStatus", t => t.LoansStatus_Id, cascadeDelete: true)
                .ForeignKey("dbo.StatusClasses", t => t.StatusClass_Id, cascadeDelete: true)
                .Index(t => t.LoansStatus_Id)
                .Index(t => t.StatusClass_Id);
            
            CreateTable(
                "dbo.StatusClassTargetRewardStatus",
                c => new
                    {
                        StatusClass_Id = c.Int(nullable: false),
                        TargetRewardStatus_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.StatusClass_Id, t.TargetRewardStatus_Id })
                .ForeignKey("dbo.StatusClasses", t => t.StatusClass_Id, cascadeDelete: true)
                .ForeignKey("dbo.TargetRewardStatus", t => t.TargetRewardStatus_Id, cascadeDelete: true)
                .Index(t => t.StatusClass_Id)
                .Index(t => t.TargetRewardStatus_Id);
            
            CreateTable(
                "dbo.StatusClassTasksStatus",
                c => new
                    {
                        StatusClass_Id = c.Int(nullable: false),
                        TasksStatus_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.StatusClass_Id, t.TasksStatus_Id })
                .ForeignKey("dbo.StatusClasses", t => t.StatusClass_Id, cascadeDelete: true)
                .ForeignKey("dbo.TasksStatus", t => t.TasksStatus_Id, cascadeDelete: true)
                .Index(t => t.StatusClass_Id)
                .Index(t => t.TasksStatus_Id);
            
            CreateTable(
                "dbo.StatusClassToDoTaskStatus",
                c => new
                    {
                        StatusClass_Id = c.Int(nullable: false),
                        ToDoTaskStatus_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.StatusClass_Id, t.ToDoTaskStatus_Id })
                .ForeignKey("dbo.StatusClasses", t => t.StatusClass_Id, cascadeDelete: true)
                .ForeignKey("dbo.ToDoTaskStatus", t => t.ToDoTaskStatus_Id, cascadeDelete: true)
                .Index(t => t.StatusClass_Id)
                .Index(t => t.ToDoTaskStatus_Id);
            
            CreateTable(
                "dbo.AdminBillStatusStatusClasses",
                c => new
                    {
                        AdminBillStatus_Id = c.Int(nullable: false),
                        StatusClass_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.AdminBillStatus_Id, t.StatusClass_Id })
                .ForeignKey("dbo.AdminBillStatus", t => t.AdminBillStatus_Id, cascadeDelete: true)
                .ForeignKey("dbo.StatusClasses", t => t.StatusClass_Id, cascadeDelete: true)
                .Index(t => t.AdminBillStatus_Id)
                .Index(t => t.StatusClass_Id);
            
            AddColumn("dbo.Bills", "statusClass_Id", c => c.Int());
            AddColumn("dbo.tabAdminBill", "statusClass_Id", c => c.Int());
            AddColumn("dbo.Tasks", "statusClass_Id", c => c.Int());
            AddColumn("dbo.Tasks", "InputTax", c => c.Double(nullable: false));
            AddColumn("dbo.Tasks", "OutputTax", c => c.Double(nullable: false));
            AddColumn("dbo.Tasks", "RefundAmountClaim", c => c.Double(nullable: false));
            AddColumn("dbo.Tasks", "CreditCarriedForward", c => c.Double(nullable: false));
            AddColumn("dbo.Tasks", "AccumulatedCredit", c => c.Double(nullable: false));
            AddColumn("dbo.Tasks", "AccumulatedDebit", c => c.Double(nullable: false));
            AddColumn("dbo.Tasks", "FEDpayable", c => c.Double(nullable: false));
            AddColumn("dbo.Tasks", "PLpayable", c => c.Double(nullable: false));
            AddColumn("dbo.Tasks", "SaleTaxPayable", c => c.Double(nullable: false));
            AddColumn("dbo.Tasks", "TotalAmountPaid", c => c.Double(nullable: false));
            AddColumn("dbo.InterBankTransfers", "statusClass_Id", c => c.Int());
            AddColumn("dbo.InterCompanyBankTransfers", "statusClass_Id", c => c.Int());
            AddColumn("dbo.LoansAdvances", "statusClass_Id", c => c.Int());
            AddColumn("dbo.Payments", "statusClass_Id", c => c.Int());
            AddColumn("dbo.PurchaseOrders", "statusClass_Id", c => c.Int());
            AddColumn("dbo.Offers", "statusClass_Id", c => c.Int());
            AddColumn("dbo.Inquiries", "statusClass_Id", c => c.Int());
            AddColumn("dbo.PurchaseInvoices", "statusClass_Id", c => c.Int());
            AddColumn("dbo.SaleInvoices", "ExpectedDiscountDate", c => c.DateTime());
            AddColumn("dbo.SaleInvoices", "statusClass_Id", c => c.Int());
            AddColumn("dbo.SaleOrders", "statusClass_Id", c => c.Int());
            AddColumn("dbo.SalesReceipts", "statusClass_Id", c => c.Int());
            AddColumn("dbo.TargetRewards", "statusClass_Id", c => c.Int());
            AddColumn("dbo.ToDoTasks", "statusClass_Id", c => c.Int());
            AddColumn("dbo.AttachmentCategories", "ProcurementProducts", c => c.Int());
            AddColumn("dbo.tabLoan", "statusClass_Id", c => c.Int());
            CreateIndex("dbo.Bills", "statusClass_Id");
            CreateIndex("dbo.tabAdminBill", "statusClass_Id");
            CreateIndex("dbo.InterBankTransfers", "statusClass_Id");
            CreateIndex("dbo.Tasks", "statusClass_Id");
            CreateIndex("dbo.InterCompanyBankTransfers", "statusClass_Id");
            CreateIndex("dbo.LoansAdvances", "statusClass_Id");
            CreateIndex("dbo.Payments", "statusClass_Id");
            CreateIndex("dbo.PurchaseOrders", "statusClass_Id");
            CreateIndex("dbo.Offers", "statusClass_Id");
            CreateIndex("dbo.Inquiries", "statusClass_Id");
            CreateIndex("dbo.PurchaseInvoices", "statusClass_Id");
            CreateIndex("dbo.SaleInvoices", "statusClass_Id");
            CreateIndex("dbo.SaleOrders", "statusClass_Id");
            CreateIndex("dbo.SalesReceipts", "statusClass_Id");
            CreateIndex("dbo.TargetRewards", "statusClass_Id");
            CreateIndex("dbo.ToDoTasks", "statusClass_Id");
            CreateIndex("dbo.tabLoan", "statusClass_Id");
            AddForeignKey("dbo.PurchaseInvoices", "statusClass_Id", "dbo.StatusClasses", "Id");
            AddForeignKey("dbo.SaleOrders", "statusClass_Id", "dbo.StatusClasses", "Id");
            AddForeignKey("dbo.SalesReceipts", "statusClass_Id", "dbo.StatusClasses", "Id");
            AddForeignKey("dbo.SaleInvoices", "statusClass_Id", "dbo.StatusClasses", "Id");
            AddForeignKey("dbo.Inquiries", "statusClass_Id", "dbo.StatusClasses", "Id");
            AddForeignKey("dbo.Offers", "statusClass_Id", "dbo.StatusClasses", "Id");
            AddForeignKey("dbo.PurchaseOrders", "statusClass_Id", "dbo.StatusClasses", "Id");
            AddForeignKey("dbo.Payments", "statusClass_Id", "dbo.StatusClasses", "Id");
            AddForeignKey("dbo.TargetRewards", "statusClass_Id", "dbo.StatusClasses", "Id");
            AddForeignKey("dbo.ToDoTasks", "statusClass_Id", "dbo.StatusClasses", "Id");
            AddForeignKey("dbo.LoansAdvances", "statusClass_Id", "dbo.StatusClasses", "Id");
            AddForeignKey("dbo.InterCompanyBankTransfers", "statusClass_Id", "dbo.StatusClasses", "Id");
            AddForeignKey("dbo.Tasks", "statusClass_Id", "dbo.StatusClasses", "Id");
            AddForeignKey("dbo.InterBankTransfers", "statusClass_Id", "dbo.StatusClasses", "Id");
            AddForeignKey("dbo.tabLoan", "statusClass_Id", "dbo.StatusClasses", "Id");
            AddForeignKey("dbo.tabAdminBill", "statusClass_Id", "dbo.StatusClasses", "Id");
            AddForeignKey("dbo.Bills", "statusClass_Id", "dbo.StatusClasses", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Bills", "statusClass_Id", "dbo.StatusClasses");
            DropForeignKey("dbo.tabAdminBill", "statusClass_Id", "dbo.StatusClasses");
            DropForeignKey("dbo.AdminBillStatusStatusClasses", "StatusClass_Id", "dbo.StatusClasses");
            DropForeignKey("dbo.AdminBillStatusStatusClasses", "AdminBillStatus_Id", "dbo.AdminBillStatus");
            DropForeignKey("dbo.StatusClassToDoTaskStatus", "ToDoTaskStatus_Id", "dbo.ToDoTaskStatus");
            DropForeignKey("dbo.StatusClassToDoTaskStatus", "StatusClass_Id", "dbo.StatusClasses");
            DropForeignKey("dbo.StatusClassTasksStatus", "TasksStatus_Id", "dbo.TasksStatus");
            DropForeignKey("dbo.StatusClassTasksStatus", "StatusClass_Id", "dbo.StatusClasses");
            DropForeignKey("dbo.StatusClassTargetRewardStatus", "TargetRewardStatus_Id", "dbo.TargetRewardStatus");
            DropForeignKey("dbo.StatusClassTargetRewardStatus", "StatusClass_Id", "dbo.StatusClasses");
            DropForeignKey("dbo.LoansStatusStatusClasses", "StatusClass_Id", "dbo.StatusClasses");
            DropForeignKey("dbo.LoansStatusStatusClasses", "LoansStatus_Id", "dbo.LoansStatus");
            DropForeignKey("dbo.tabLoan", "statusClass_Id", "dbo.StatusClasses");
            DropForeignKey("dbo.InterBankTransfers", "statusClass_Id", "dbo.StatusClasses");
            DropForeignKey("dbo.Tasks", "statusClass_Id", "dbo.StatusClasses");
            DropForeignKey("dbo.InterCompanyBankTransfers", "statusClass_Id", "dbo.StatusClasses");
            DropForeignKey("dbo.LoansAdvances", "statusClass_Id", "dbo.StatusClasses");
            DropForeignKey("dbo.LoansAdvanceStatusStatusClasses", "StatusClass_Id", "dbo.StatusClasses");
            DropForeignKey("dbo.LoansAdvanceStatusStatusClasses", "LoansAdvanceStatus_Id", "dbo.LoansAdvanceStatus");
            DropForeignKey("dbo.ToDoTasks", "statusClass_Id", "dbo.StatusClasses");
            DropForeignKey("dbo.TargetRewards", "statusClass_Id", "dbo.StatusClasses");
            DropForeignKey("dbo.Payments", "statusClass_Id", "dbo.StatusClasses");
            DropForeignKey("dbo.PaymentStatusStatusClasses", "StatusClass_Id", "dbo.StatusClasses");
            DropForeignKey("dbo.PaymentStatusStatusClasses", "PaymentStatus_Id", "dbo.PaymentStatus");
            DropForeignKey("dbo.PurchaseOrders", "statusClass_Id", "dbo.StatusClasses");
            DropForeignKey("dbo.PurchaseOrderStatusStatusClasses", "StatusClass_Id", "dbo.StatusClasses");
            DropForeignKey("dbo.PurchaseOrderStatusStatusClasses", "PurchaseOrderStatus_Id", "dbo.PurchaseOrderStatus");
            DropForeignKey("dbo.Offers", "statusClass_Id", "dbo.StatusClasses");
            DropForeignKey("dbo.OfferStatusStatusClasses", "StatusClass_Id", "dbo.StatusClasses");
            DropForeignKey("dbo.OfferStatusStatusClasses", "OfferStatus_Id", "dbo.OfferStatus");
            DropForeignKey("dbo.Inquiries", "statusClass_Id", "dbo.StatusClasses");
            DropForeignKey("dbo.SaleInvoices", "statusClass_Id", "dbo.StatusClasses");
            DropForeignKey("dbo.SalesReceipts", "statusClass_Id", "dbo.StatusClasses");
            DropForeignKey("dbo.SalesReceiptStatusStatusClasses", "StatusClass_Id", "dbo.StatusClasses");
            DropForeignKey("dbo.SalesReceiptStatusStatusClasses", "SalesReceiptStatus_Id", "dbo.SalesReceiptStatus");
            DropForeignKey("dbo.SaleOrders", "statusClass_Id", "dbo.StatusClasses");
            DropForeignKey("dbo.SaleOrderStatusStatusClasses", "StatusClass_Id", "dbo.StatusClasses");
            DropForeignKey("dbo.SaleOrderStatusStatusClasses", "SaleOrderStatus_Id", "dbo.SaleOrderStatus");
            DropForeignKey("dbo.SaleInvoiceStatusStatusClasses", "StatusClass_Id", "dbo.StatusClasses");
            DropForeignKey("dbo.SaleInvoiceStatusStatusClasses", "SaleInvoiceStatus_Id", "dbo.SaleInvoiceStatus");
            DropForeignKey("dbo.PurchaseInvoices", "statusClass_Id", "dbo.StatusClasses");
            DropForeignKey("dbo.PurchaseInvoiceStatusStatusClasses", "StatusClass_Id", "dbo.StatusClasses");
            DropForeignKey("dbo.PurchaseInvoiceStatusStatusClasses", "PurchaseInvoiceStatus_Id", "dbo.PurchaseInvoiceStatus");
            DropForeignKey("dbo.InquiryStatusStatusClasses", "StatusClass_Id", "dbo.StatusClasses");
            DropForeignKey("dbo.InquiryStatusStatusClasses", "InquiryStatus_Id", "dbo.InquiryStatus");
            DropForeignKey("dbo.InterBankTransferStatusStatusClasses", "StatusClass_Id", "dbo.StatusClasses");
            DropForeignKey("dbo.InterBankTransferStatusStatusClasses", "InterBankTransferStatus_Id", "dbo.InterBankTransferStatus");
            DropForeignKey("dbo.BillStatusStatusClasses", "StatusClass_Id", "dbo.StatusClasses");
            DropForeignKey("dbo.BillStatusStatusClasses", "BillStatus_Id", "dbo.BillStatus");
            DropIndex("dbo.AdminBillStatusStatusClasses", new[] { "StatusClass_Id" });
            DropIndex("dbo.AdminBillStatusStatusClasses", new[] { "AdminBillStatus_Id" });
            DropIndex("dbo.StatusClassToDoTaskStatus", new[] { "ToDoTaskStatus_Id" });
            DropIndex("dbo.StatusClassToDoTaskStatus", new[] { "StatusClass_Id" });
            DropIndex("dbo.StatusClassTasksStatus", new[] { "TasksStatus_Id" });
            DropIndex("dbo.StatusClassTasksStatus", new[] { "StatusClass_Id" });
            DropIndex("dbo.StatusClassTargetRewardStatus", new[] { "TargetRewardStatus_Id" });
            DropIndex("dbo.StatusClassTargetRewardStatus", new[] { "StatusClass_Id" });
            DropIndex("dbo.LoansStatusStatusClasses", new[] { "StatusClass_Id" });
            DropIndex("dbo.LoansStatusStatusClasses", new[] { "LoansStatus_Id" });
            DropIndex("dbo.LoansAdvanceStatusStatusClasses", new[] { "StatusClass_Id" });
            DropIndex("dbo.LoansAdvanceStatusStatusClasses", new[] { "LoansAdvanceStatus_Id" });
            DropIndex("dbo.PaymentStatusStatusClasses", new[] { "StatusClass_Id" });
            DropIndex("dbo.PaymentStatusStatusClasses", new[] { "PaymentStatus_Id" });
            DropIndex("dbo.PurchaseOrderStatusStatusClasses", new[] { "StatusClass_Id" });
            DropIndex("dbo.PurchaseOrderStatusStatusClasses", new[] { "PurchaseOrderStatus_Id" });
            DropIndex("dbo.OfferStatusStatusClasses", new[] { "StatusClass_Id" });
            DropIndex("dbo.OfferStatusStatusClasses", new[] { "OfferStatus_Id" });
            DropIndex("dbo.SalesReceiptStatusStatusClasses", new[] { "StatusClass_Id" });
            DropIndex("dbo.SalesReceiptStatusStatusClasses", new[] { "SalesReceiptStatus_Id" });
            DropIndex("dbo.SaleOrderStatusStatusClasses", new[] { "StatusClass_Id" });
            DropIndex("dbo.SaleOrderStatusStatusClasses", new[] { "SaleOrderStatus_Id" });
            DropIndex("dbo.SaleInvoiceStatusStatusClasses", new[] { "StatusClass_Id" });
            DropIndex("dbo.SaleInvoiceStatusStatusClasses", new[] { "SaleInvoiceStatus_Id" });
            DropIndex("dbo.PurchaseInvoiceStatusStatusClasses", new[] { "StatusClass_Id" });
            DropIndex("dbo.PurchaseInvoiceStatusStatusClasses", new[] { "PurchaseInvoiceStatus_Id" });
            DropIndex("dbo.InquiryStatusStatusClasses", new[] { "StatusClass_Id" });
            DropIndex("dbo.InquiryStatusStatusClasses", new[] { "InquiryStatus_Id" });
            DropIndex("dbo.InterBankTransferStatusStatusClasses", new[] { "StatusClass_Id" });
            DropIndex("dbo.InterBankTransferStatusStatusClasses", new[] { "InterBankTransferStatus_Id" });
            DropIndex("dbo.BillStatusStatusClasses", new[] { "StatusClass_Id" });
            DropIndex("dbo.BillStatusStatusClasses", new[] { "BillStatus_Id" });
            DropIndex("dbo.tabLoan", new[] { "statusClass_Id" });
            DropIndex("dbo.ToDoTasks", new[] { "statusClass_Id" });
            DropIndex("dbo.TargetRewards", new[] { "statusClass_Id" });
            DropIndex("dbo.SalesReceipts", new[] { "statusClass_Id" });
            DropIndex("dbo.SaleOrders", new[] { "statusClass_Id" });
            DropIndex("dbo.SaleInvoices", new[] { "statusClass_Id" });
            DropIndex("dbo.PurchaseInvoices", new[] { "statusClass_Id" });
            DropIndex("dbo.Inquiries", new[] { "statusClass_Id" });
            DropIndex("dbo.Offers", new[] { "statusClass_Id" });
            DropIndex("dbo.PurchaseOrders", new[] { "statusClass_Id" });
            DropIndex("dbo.Payments", new[] { "statusClass_Id" });
            DropIndex("dbo.LoansAdvances", new[] { "statusClass_Id" });
            DropIndex("dbo.InterCompanyBankTransfers", new[] { "statusClass_Id" });
            DropIndex("dbo.Tasks", new[] { "statusClass_Id" });
            DropIndex("dbo.InterBankTransfers", new[] { "statusClass_Id" });
            DropIndex("dbo.tabAdminBill", new[] { "statusClass_Id" });
            DropIndex("dbo.Bills", new[] { "statusClass_Id" });
            DropColumn("dbo.tabLoan", "statusClass_Id");
            DropColumn("dbo.AttachmentCategories", "ProcurementProducts");
            DropColumn("dbo.ToDoTasks", "statusClass_Id");
            DropColumn("dbo.TargetRewards", "statusClass_Id");
            DropColumn("dbo.SalesReceipts", "statusClass_Id");
            DropColumn("dbo.SaleOrders", "statusClass_Id");
            DropColumn("dbo.SaleInvoices", "statusClass_Id");
            DropColumn("dbo.SaleInvoices", "ExpectedDiscountDate");
            DropColumn("dbo.PurchaseInvoices", "statusClass_Id");
            DropColumn("dbo.Inquiries", "statusClass_Id");
            DropColumn("dbo.Offers", "statusClass_Id");
            DropColumn("dbo.PurchaseOrders", "statusClass_Id");
            DropColumn("dbo.Payments", "statusClass_Id");
            DropColumn("dbo.LoansAdvances", "statusClass_Id");
            DropColumn("dbo.InterCompanyBankTransfers", "statusClass_Id");
            DropColumn("dbo.InterBankTransfers", "statusClass_Id");
            DropColumn("dbo.Tasks", "TotalAmountPaid");
            DropColumn("dbo.Tasks", "SaleTaxPayable");
            DropColumn("dbo.Tasks", "PLpayable");
            DropColumn("dbo.Tasks", "FEDpayable");
            DropColumn("dbo.Tasks", "AccumulatedDebit");
            DropColumn("dbo.Tasks", "AccumulatedCredit");
            DropColumn("dbo.Tasks", "CreditCarriedForward");
            DropColumn("dbo.Tasks", "RefundAmountClaim");
            DropColumn("dbo.Tasks", "OutputTax");
            DropColumn("dbo.Tasks", "InputTax");
            DropColumn("dbo.Tasks", "statusClass_Id");
            DropColumn("dbo.tabAdminBill", "statusClass_Id");
            DropColumn("dbo.Bills", "statusClass_Id");
            DropTable("dbo.AdminBillStatusStatusClasses");
            DropTable("dbo.StatusClassToDoTaskStatus");
            DropTable("dbo.StatusClassTasksStatus");
            DropTable("dbo.StatusClassTargetRewardStatus");
            DropTable("dbo.LoansStatusStatusClasses");
            DropTable("dbo.LoansAdvanceStatusStatusClasses");
            DropTable("dbo.PaymentStatusStatusClasses");
            DropTable("dbo.PurchaseOrderStatusStatusClasses");
            DropTable("dbo.OfferStatusStatusClasses");
            DropTable("dbo.SalesReceiptStatusStatusClasses");
            DropTable("dbo.SaleOrderStatusStatusClasses");
            DropTable("dbo.SaleInvoiceStatusStatusClasses");
            DropTable("dbo.PurchaseInvoiceStatusStatusClasses");
            DropTable("dbo.InquiryStatusStatusClasses");
            DropTable("dbo.InterBankTransferStatusStatusClasses");
            DropTable("dbo.BillStatusStatusClasses");
            DropTable("dbo.StatusClasses");
        }
    }
}
