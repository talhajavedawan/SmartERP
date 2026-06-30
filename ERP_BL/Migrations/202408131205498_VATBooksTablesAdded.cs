namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VATBooksTablesAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.VATBookRefNumbers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        companyId = c.Int(),
                        VATBookReferenceNo = c.String(),
                        isActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tabCompany", t => t.companyId)
                .Index(t => t.companyId);
            
            CreateTable(
                "dbo.VATBooks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FinanceRefNo = c.String(),
                        SystemRefNo = c.String(),
                        VATBookRef = c.String(),
                        debit = c.Double(nullable: false),
                        credit = c.Double(nullable: false),
                        MER = c.Double(nullable: false),
                        total = c.Double(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        GLPostingDate = c.DateTime(nullable: false),
                        TransactionType = c.Int(nullable: false),
                        companyId = c.Int(),
                        customerId = c.Int(),
                        vendorId = c.Int(),
                        deptId = c.Int(),
                        Description = c.String(),
                        currencyId = c.Int(),
                        saleInvoiceId = c.Int(),
                        purchaseInvoiceId = c.Int(),
                        vendorBillId = c.Int(),
                        saleReceiptId = c.Int(),
                        interBankTransferId = c.Int(),
                        paymentId = c.Int(),
                        adminBillId = c.Int(),
                        interCompanyId = c.Int(),
                        loansAdvanceId = c.Int(),
                        VATBookRefNumberRefId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tabAdminBill", t => t.adminBillId)
                .ForeignKey("dbo.tabCompany", t => t.companyId)
                .ForeignKey("dbo.CustomerCompanies", t => t.customerId)
                .ForeignKey("dbo.tabDepartment", t => t.deptId)
                .ForeignKey("dbo.InterBankTransfers", t => t.interBankTransferId)
                .ForeignKey("dbo.InterCompanyBankTransfers", t => t.interCompanyId)
                .ForeignKey("dbo.PurchaseInvoices", t => t.purchaseInvoiceId)
                .ForeignKey("dbo.SalesReceipts", t => t.saleReceiptId)
                .ForeignKey("dbo.SaleInvoices", t => t.saleInvoiceId)
                .ForeignKey("dbo.Payments", t => t.paymentId)
                .ForeignKey("dbo.LoansAdvances", t => t.loansAdvanceId)
                .ForeignKey("dbo.Currencies", t => t.currencyId)
                .ForeignKey("dbo.VATBookRefNumbers", t => t.VATBookRefNumberRefId)
                .ForeignKey("dbo.tabVendor", t => t.vendorId)
                .ForeignKey("dbo.Bills", t => t.vendorBillId)
                .Index(t => t.companyId)
                .Index(t => t.customerId)
                .Index(t => t.vendorId)
                .Index(t => t.deptId)
                .Index(t => t.currencyId)
                .Index(t => t.saleInvoiceId)
                .Index(t => t.purchaseInvoiceId)
                .Index(t => t.vendorBillId)
                .Index(t => t.saleReceiptId)
                .Index(t => t.interBankTransferId)
                .Index(t => t.paymentId)
                .Index(t => t.adminBillId)
                .Index(t => t.interCompanyId)
                .Index(t => t.loansAdvanceId)
                .Index(t => t.VATBookRefNumberRefId);
            
            AddColumn("dbo.Bills", "VATBookRefId", c => c.Int());
            AddColumn("dbo.tabAdminBill", "VATBookRefId", c => c.Int());
            AddColumn("dbo.InterBankTransfers", "VATBookRefId", c => c.Int());
            AddColumn("dbo.InterCompanyBankTransfers", "VATBookRefId", c => c.Int());
            AddColumn("dbo.LoansAdvances", "VATBookRefId", c => c.Int());
            AddColumn("dbo.Payments", "VATBookRefId", c => c.Int());
            AddColumn("dbo.PurchaseInvoices", "VATBookRefId", c => c.Int());
            AddColumn("dbo.SaleInvoices", "insuranceAppliedBy_Id", c => c.Int());
            AddColumn("dbo.SaleInvoices", "VATBookRefId", c => c.Int());
            AddColumn("dbo.SalesReceipts", "VATBookRefId", c => c.Int());
            CreateIndex("dbo.Bills", "VATBookRefId");
            CreateIndex("dbo.tabAdminBill", "VATBookRefId");
            CreateIndex("dbo.InterBankTransfers", "VATBookRefId");
            CreateIndex("dbo.InterCompanyBankTransfers", "VATBookRefId");
            CreateIndex("dbo.LoansAdvances", "VATBookRefId");
            CreateIndex("dbo.Payments", "VATBookRefId");
            CreateIndex("dbo.PurchaseInvoices", "VATBookRefId");
            CreateIndex("dbo.SaleInvoices", "insuranceAppliedBy_Id");
            CreateIndex("dbo.SaleInvoices", "VATBookRefId");
            CreateIndex("dbo.SalesReceipts", "VATBookRefId");
            AddForeignKey("dbo.InterCompanyBankTransfers", "VATBookRefId", "dbo.VATBookRefNumbers", "Id");
            AddForeignKey("dbo.PurchaseInvoices", "VATBookRefId", "dbo.VATBookRefNumbers", "Id");
            AddForeignKey("dbo.SaleInvoices", "insuranceAppliedBy_Id", "dbo.Employees", "EmpId");
            AddForeignKey("dbo.SalesReceipts", "VATBookRefId", "dbo.VATBookRefNumbers", "Id");
            AddForeignKey("dbo.SaleInvoices", "VATBookRefId", "dbo.VATBookRefNumbers", "Id");
            AddForeignKey("dbo.Payments", "VATBookRefId", "dbo.VATBookRefNumbers", "Id");
            AddForeignKey("dbo.LoansAdvances", "VATBookRefId", "dbo.VATBookRefNumbers", "Id");
            AddForeignKey("dbo.InterBankTransfers", "VATBookRefId", "dbo.VATBookRefNumbers", "Id");
            AddForeignKey("dbo.tabAdminBill", "VATBookRefId", "dbo.VATBookRefNumbers", "Id");
            AddForeignKey("dbo.Bills", "VATBookRefId", "dbo.VATBookRefNumbers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Bills", "VATBookRefId", "dbo.VATBookRefNumbers");
            DropForeignKey("dbo.tabAdminBill", "VATBookRefId", "dbo.VATBookRefNumbers");
            DropForeignKey("dbo.InterBankTransfers", "VATBookRefId", "dbo.VATBookRefNumbers");
            DropForeignKey("dbo.VATBooks", "vendorBillId", "dbo.Bills");
            DropForeignKey("dbo.VATBooks", "vendorId", "dbo.tabVendor");
            DropForeignKey("dbo.VATBooks", "VATBookRefNumberRefId", "dbo.VATBookRefNumbers");
            DropForeignKey("dbo.VATBooks", "currencyId", "dbo.Currencies");
            DropForeignKey("dbo.VATBooks", "loansAdvanceId", "dbo.LoansAdvances");
            DropForeignKey("dbo.LoansAdvances", "VATBookRefId", "dbo.VATBookRefNumbers");
            DropForeignKey("dbo.VATBooks", "paymentId", "dbo.Payments");
            DropForeignKey("dbo.Payments", "VATBookRefId", "dbo.VATBookRefNumbers");
            DropForeignKey("dbo.VATBooks", "saleInvoiceId", "dbo.SaleInvoices");
            DropForeignKey("dbo.SaleInvoices", "VATBookRefId", "dbo.VATBookRefNumbers");
            DropForeignKey("dbo.VATBooks", "saleReceiptId", "dbo.SalesReceipts");
            DropForeignKey("dbo.SalesReceipts", "VATBookRefId", "dbo.VATBookRefNumbers");
            DropForeignKey("dbo.SaleInvoices", "insuranceAppliedBy_Id", "dbo.Employees");
            DropForeignKey("dbo.VATBooks", "purchaseInvoiceId", "dbo.PurchaseInvoices");
            DropForeignKey("dbo.PurchaseInvoices", "VATBookRefId", "dbo.VATBookRefNumbers");
            DropForeignKey("dbo.VATBooks", "interCompanyId", "dbo.InterCompanyBankTransfers");
            DropForeignKey("dbo.VATBooks", "interBankTransferId", "dbo.InterBankTransfers");
            DropForeignKey("dbo.VATBooks", "deptId", "dbo.tabDepartment");
            DropForeignKey("dbo.VATBooks", "customerId", "dbo.CustomerCompanies");
            DropForeignKey("dbo.VATBooks", "companyId", "dbo.tabCompany");
            DropForeignKey("dbo.VATBooks", "adminBillId", "dbo.tabAdminBill");
            DropForeignKey("dbo.InterCompanyBankTransfers", "VATBookRefId", "dbo.VATBookRefNumbers");
            DropForeignKey("dbo.VATBookRefNumbers", "companyId", "dbo.tabCompany");
            DropIndex("dbo.SalesReceipts", new[] { "VATBookRefId" });
            DropIndex("dbo.SaleInvoices", new[] { "VATBookRefId" });
            DropIndex("dbo.SaleInvoices", new[] { "insuranceAppliedBy_Id" });
            DropIndex("dbo.PurchaseInvoices", new[] { "VATBookRefId" });
            DropIndex("dbo.Payments", new[] { "VATBookRefId" });
            DropIndex("dbo.LoansAdvances", new[] { "VATBookRefId" });
            DropIndex("dbo.VATBooks", new[] { "VATBookRefNumberRefId" });
            DropIndex("dbo.VATBooks", new[] { "loansAdvanceId" });
            DropIndex("dbo.VATBooks", new[] { "interCompanyId" });
            DropIndex("dbo.VATBooks", new[] { "adminBillId" });
            DropIndex("dbo.VATBooks", new[] { "paymentId" });
            DropIndex("dbo.VATBooks", new[] { "interBankTransferId" });
            DropIndex("dbo.VATBooks", new[] { "saleReceiptId" });
            DropIndex("dbo.VATBooks", new[] { "vendorBillId" });
            DropIndex("dbo.VATBooks", new[] { "purchaseInvoiceId" });
            DropIndex("dbo.VATBooks", new[] { "saleInvoiceId" });
            DropIndex("dbo.VATBooks", new[] { "currencyId" });
            DropIndex("dbo.VATBooks", new[] { "deptId" });
            DropIndex("dbo.VATBooks", new[] { "vendorId" });
            DropIndex("dbo.VATBooks", new[] { "customerId" });
            DropIndex("dbo.VATBooks", new[] { "companyId" });
            DropIndex("dbo.VATBookRefNumbers", new[] { "companyId" });
            DropIndex("dbo.InterCompanyBankTransfers", new[] { "VATBookRefId" });
            DropIndex("dbo.InterBankTransfers", new[] { "VATBookRefId" });
            DropIndex("dbo.tabAdminBill", new[] { "VATBookRefId" });
            DropIndex("dbo.Bills", new[] { "VATBookRefId" });
            DropColumn("dbo.SalesReceipts", "VATBookRefId");
            DropColumn("dbo.SaleInvoices", "VATBookRefId");
            DropColumn("dbo.SaleInvoices", "insuranceAppliedBy_Id");
            DropColumn("dbo.PurchaseInvoices", "VATBookRefId");
            DropColumn("dbo.Payments", "VATBookRefId");
            DropColumn("dbo.LoansAdvances", "VATBookRefId");
            DropColumn("dbo.InterCompanyBankTransfers", "VATBookRefId");
            DropColumn("dbo.InterBankTransfers", "VATBookRefId");
            DropColumn("dbo.tabAdminBill", "VATBookRefId");
            DropColumn("dbo.Bills", "VATBookRefId");
            DropTable("dbo.VATBooks");
            DropTable("dbo.VATBookRefNumbers");
        }
    }
}
