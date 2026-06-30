namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class purchaseInvoiceClasses : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Payments", name: "primaryCardNoId", newName: "PrimaryCreditCardNoId");
            RenameIndex(table: "dbo.Payments", name: "IX_primaryCardNoId", newName: "IX_PrimaryCreditCardNoId");
            CreateTable(
                "dbo.PurchaseInvoices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PIReferenceNo = c.String(),
                        SOReferenceNo = c.String(),
                        SalesReferenceNo = c.String(),
                        FinanceRefrenceNo = c.String(),
                        VendorName = c.String(),
                        OfferReferenceNo = c.String(),
                        CreationDate = c.DateTime(),
                        PurchaseInvoicetype = c.Int(nullable: false),
                        isVoid = c.Boolean(nullable: false),
                        isReviewed = c.Boolean(),
                        needReview = c.Boolean(),
                        PendingForClosing = c.Boolean(),
                        PendingForReApproval = c.Boolean(),
                        isApproved = c.Boolean(),
                        ApprovedDate = c.DateTime(),
                        ReApprovalDate = c.DateTime(),
                        isReApproved = c.Boolean(),
                        currency_Id = c.Int(nullable: false),
                        purchaseOrder_Id = c.Int(),
                        dept_Id = c.Int(nullable: false),
                        company_Id = c.Int(),
                        InterDepartment_Id = c.Int(),
                        InterCompany_Id = c.Int(),
                        isInterCompany = c.Boolean(),
                        customerCompany_Id = c.Int(nullable: false),
                        user_Id = c.Int(),
                        allocation_Id = c.Int(nullable: false),
                        TotalWeight = c.Decimal(precision: 18, scale: 2),
                        TotalQuantity = c.Decimal(precision: 18, scale: 2),
                        vendor_Id = c.Int(),
                        PurchaseInvoiceStatus_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.allocation_Id, cascadeDelete: true)
                .ForeignKey("dbo.Currencies", t => t.currency_Id, cascadeDelete: true)
                .ForeignKey("dbo.CustomerCompanies", t => t.customerCompany_Id, cascadeDelete: true)
                .ForeignKey("dbo.PurchaseInvoiceStatus", t => t.PurchaseInvoiceStatus_Id)
                .ForeignKey("dbo.PurchaseOrders", t => t.purchaseOrder_Id)
                .ForeignKey("dbo.Users", t => t.user_Id)
                .ForeignKey("dbo.tabVendor", t => t.vendor_Id)
                .ForeignKey("dbo.tabDepartment", t => t.dept_Id, cascadeDelete: true)
                .ForeignKey("dbo.tabDepartment", t => t.InterDepartment_Id)
                .ForeignKey("dbo.tabCompany", t => t.company_Id)
                .ForeignKey("dbo.tabCompany", t => t.InterCompany_Id)
                .Index(t => t.currency_Id)
                .Index(t => t.purchaseOrder_Id)
                .Index(t => t.dept_Id)
                .Index(t => t.company_Id)
                .Index(t => t.InterDepartment_Id)
                .Index(t => t.InterCompany_Id)
                .Index(t => t.customerCompany_Id)
                .Index(t => t.user_Id)
                .Index(t => t.allocation_Id)
                .Index(t => t.vendor_Id)
                .Index(t => t.PurchaseInvoiceStatus_Id);
            
            CreateTable(
                "dbo.PurchaseInvoiceStatus",
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
            
            AddColumn("dbo.Payments", "creditCardBankId", c => c.Int());
            AddColumn("dbo.Payments", "BillingMonth", c => c.DateTime());
            AddColumn("dbo.Payments", "DebitedAmount", c => c.Double(nullable: false));
            AddColumn("dbo.Payments", "Deductions", c => c.Double(nullable: false));
            AddColumn("dbo.PaymentMethods", "isActive", c => c.Boolean(nullable: false));
            CreateIndex("dbo.Payments", "creditCardBankId");
            AddForeignKey("dbo.Payments", "creditCardBankId", "dbo.Banks", "Id");
            DropColumn("dbo.Payments", "isCreditCard");
            DropColumn("dbo.Payments", "AmountToPay");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Payments", "AmountToPay", c => c.Double(nullable: false));
            AddColumn("dbo.Payments", "isCreditCard", c => c.Boolean(nullable: false));
            DropForeignKey("dbo.PurchaseInvoices", "InterCompany_Id", "dbo.tabCompany");
            DropForeignKey("dbo.PurchaseInvoices", "company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.PurchaseInvoices", "InterDepartment_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.PurchaseInvoices", "dept_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.PurchaseInvoices", "vendor_Id", "dbo.tabVendor");
            DropForeignKey("dbo.PurchaseInvoices", "user_Id", "dbo.Users");
            DropForeignKey("dbo.PurchaseInvoices", "purchaseOrder_Id", "dbo.PurchaseOrders");
            DropForeignKey("dbo.PurchaseInvoices", "PurchaseInvoiceStatus_Id", "dbo.PurchaseInvoiceStatus");
            DropForeignKey("dbo.PurchaseInvoices", "customerCompany_Id", "dbo.CustomerCompanies");
            DropForeignKey("dbo.PurchaseInvoices", "currency_Id", "dbo.Currencies");
            DropForeignKey("dbo.PurchaseInvoices", "allocation_Id", "dbo.Employees");
            DropForeignKey("dbo.Payments", "creditCardBankId", "dbo.Banks");
            DropIndex("dbo.PurchaseInvoices", new[] { "PurchaseInvoiceStatus_Id" });
            DropIndex("dbo.PurchaseInvoices", new[] { "vendor_Id" });
            DropIndex("dbo.PurchaseInvoices", new[] { "allocation_Id" });
            DropIndex("dbo.PurchaseInvoices", new[] { "user_Id" });
            DropIndex("dbo.PurchaseInvoices", new[] { "customerCompany_Id" });
            DropIndex("dbo.PurchaseInvoices", new[] { "InterCompany_Id" });
            DropIndex("dbo.PurchaseInvoices", new[] { "InterDepartment_Id" });
            DropIndex("dbo.PurchaseInvoices", new[] { "company_Id" });
            DropIndex("dbo.PurchaseInvoices", new[] { "dept_Id" });
            DropIndex("dbo.PurchaseInvoices", new[] { "purchaseOrder_Id" });
            DropIndex("dbo.PurchaseInvoices", new[] { "currency_Id" });
            DropIndex("dbo.Payments", new[] { "creditCardBankId" });
            DropColumn("dbo.PaymentMethods", "isActive");
            DropColumn("dbo.Payments", "Deductions");
            DropColumn("dbo.Payments", "DebitedAmount");
            DropColumn("dbo.Payments", "BillingMonth");
            DropColumn("dbo.Payments", "creditCardBankId");
            DropTable("dbo.PurchaseInvoiceStatus");
            DropTable("dbo.PurchaseInvoices");
            RenameIndex(table: "dbo.Payments", name: "IX_PrimaryCreditCardNoId", newName: "IX_primaryCardNoId");
            RenameColumn(table: "dbo.Payments", name: "PrimaryCreditCardNoId", newName: "primaryCardNoId");
        }
    }
}
