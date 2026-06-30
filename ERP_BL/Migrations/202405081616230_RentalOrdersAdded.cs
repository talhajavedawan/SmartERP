namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RentalOrdersAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RentalOrders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreationDate = c.DateTime(),
                        companyId = c.Int(nullable: false),
                        deptId = c.Int(),
                        transactionGroupId = c.Int(nullable: false),
                        SystemRef = c.String(),
                        assetRentalId = c.Int(),
                        TenantRentalId = c.Int(),
                        creatorId = c.Int(),
                        statusId = c.Int(),
                        currencyId = c.Int(),
                        fromDate = c.DateTime(nullable: false),
                        toDate = c.DateTime(nullable: false),
                        RentAmount = c.Double(nullable: false),
                        MER = c.Double(nullable: false),
                        NumberOfMonths = c.Double(nullable: false),
                        TotalRentAmount = c.Double(nullable: false),
                        rentalBasis = c.Int(nullable: false),
                        isActive = c.Boolean(nullable: false),
                        stage = c.String(),
                        isVoid = c.Boolean(nullable: false),
                        isReviewed = c.Boolean(),
                        needReview = c.Boolean(),
                        PendingForClosing = c.Boolean(),
                        PendingForReApproval = c.Boolean(),
                        isApproved = c.Boolean(),
                        ApprovedDate = c.DateTime(),
                        isReApproved = c.Boolean(),
                        ReApprovalDate = c.DateTime(),
                        ClosingDate = c.DateTime(),
                        LastStatusChangeDate = c.DateTime(),
                        RentalContract_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AssetRentals", t => t.assetRentalId)
                .ForeignKey("dbo.tabCompany", t => t.companyId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.creatorId)
                .ForeignKey("dbo.Currencies", t => t.currencyId)
                .ForeignKey("dbo.tabDepartment", t => t.deptId)
                .ForeignKey("dbo.RentalOrderStatus", t => t.statusId)
                .ForeignKey("dbo.TenantRentals", t => t.TenantRentalId)
                .ForeignKey("dbo.RentalContracts", t => t.RentalContract_Id)
                .Index(t => t.companyId)
                .Index(t => t.deptId)
                .Index(t => t.assetRentalId)
                .Index(t => t.TenantRentalId)
                .Index(t => t.creatorId)
                .Index(t => t.statusId)
                .Index(t => t.currencyId)
                .Index(t => t.RentalContract_Id);
            
            CreateTable(
                "dbo.RentalInvoices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreationDate = c.DateTime(),
                        companyId = c.Int(nullable: false),
                        deptId = c.Int(),
                        transactionGroupId = c.Int(nullable: false),
                        SystemRef = c.String(),
                        assetRentalId = c.Int(),
                        TenantRentalId = c.Int(),
                        creatorId = c.Int(),
                        statusId = c.Int(),
                        currencyId = c.Int(),
                        fromDate = c.DateTime(nullable: false),
                        toDate = c.DateTime(nullable: false),
                        RentAmount = c.Double(nullable: false),
                        MER = c.Double(nullable: false),
                        NumberOfMonths = c.Double(nullable: false),
                        TotalRentAmount = c.Double(nullable: false),
                        rentalBasis = c.Int(nullable: false),
                        isActive = c.Boolean(nullable: false),
                        stage = c.String(),
                        isVoid = c.Boolean(nullable: false),
                        isReviewed = c.Boolean(),
                        needReview = c.Boolean(),
                        PendingForClosing = c.Boolean(),
                        PendingForReApproval = c.Boolean(),
                        isApproved = c.Boolean(),
                        ApprovedDate = c.DateTime(),
                        isReApproved = c.Boolean(),
                        ReApprovalDate = c.DateTime(),
                        ClosingDate = c.DateTime(),
                        LastStatusChangeDate = c.DateTime(),
                        RentalOrder_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AssetRentals", t => t.assetRentalId)
                .ForeignKey("dbo.tabCompany", t => t.companyId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.creatorId)
                .ForeignKey("dbo.Currencies", t => t.currencyId)
                .ForeignKey("dbo.tabDepartment", t => t.deptId)
                .ForeignKey("dbo.RentalInvoiceStatus", t => t.statusId)
                .ForeignKey("dbo.TenantRentals", t => t.TenantRentalId)
                .ForeignKey("dbo.RentalOrders", t => t.RentalOrder_Id)
                .Index(t => t.companyId)
                .Index(t => t.deptId)
                .Index(t => t.assetRentalId)
                .Index(t => t.TenantRentalId)
                .Index(t => t.creatorId)
                .Index(t => t.statusId)
                .Index(t => t.currencyId)
                .Index(t => t.RentalOrder_Id);
            
            CreateTable(
                "dbo.RentalInvoiceStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.String(),
                        isApproved = c.Boolean(nullable: false),
                        isActive = c.Boolean(),
                        backcolor = c.String(),
                        forecolor = c.String(),
                        HierarchicalIndex = c.Int(nullable: false),
                        isDisable = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RentalOrderStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.String(),
                        isApproved = c.Boolean(nullable: false),
                        isActive = c.Boolean(),
                        backcolor = c.String(),
                        forecolor = c.String(),
                        HierarchicalIndex = c.Int(nullable: false),
                        isDisable = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SecurityDeposits",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        mainBankId = c.Int(),
                        bankId = c.Int(),
                        accountId = c.Int(),
                        Amount = c.Double(nullable: false),
                        collectionMethodId = c.Int(),
                        RefNo = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Accounts", t => t.accountId)
                .ForeignKey("dbo.Banks", t => t.bankId)
                .ForeignKey("dbo.CollectionMethods", t => t.collectionMethodId)
                .ForeignKey("dbo.MainBanks", t => t.mainBankId)
                .Index(t => t.mainBankId)
                .Index(t => t.bankId)
                .Index(t => t.accountId)
                .Index(t => t.collectionMethodId);
            
            CreateTable(
                "dbo.RentalInvoiceStatusStatusClasses",
                c => new
                    {
                        RentalInvoiceStatus_Id = c.Int(nullable: false),
                        StatusClass_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.RentalInvoiceStatus_Id, t.StatusClass_Id })
                .ForeignKey("dbo.RentalInvoiceStatus", t => t.RentalInvoiceStatus_Id, cascadeDelete: true)
                .ForeignKey("dbo.StatusClasses", t => t.StatusClass_Id, cascadeDelete: true)
                .Index(t => t.RentalInvoiceStatus_Id)
                .Index(t => t.StatusClass_Id);
            
            CreateTable(
                "dbo.RentalOrderStatusStatusClasses",
                c => new
                    {
                        RentalOrderStatus_Id = c.Int(nullable: false),
                        StatusClass_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.RentalOrderStatus_Id, t.StatusClass_Id })
                .ForeignKey("dbo.RentalOrderStatus", t => t.RentalOrderStatus_Id, cascadeDelete: true)
                .ForeignKey("dbo.StatusClasses", t => t.StatusClass_Id, cascadeDelete: true)
                .Index(t => t.RentalOrderStatus_Id)
                .Index(t => t.StatusClass_Id);
            
            AddColumn("dbo.RentalContracts", "currencyId", c => c.Int());
            AddColumn("dbo.RentalContracts", "MER", c => c.Double(nullable: false));
            CreateIndex("dbo.RentalContracts", "currencyId");
            AddForeignKey("dbo.RentalContracts", "currencyId", "dbo.Currencies", "Id");
            DropColumn("dbo.RentalContracts", "SecurityDeposit");
            DropColumn("dbo.RentalContracts", "isActive");
        }
        
        public override void Down()
        {
            AddColumn("dbo.RentalContracts", "isActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.RentalContracts", "SecurityDeposit", c => c.Double(nullable: false));
            DropForeignKey("dbo.SecurityDeposits", "mainBankId", "dbo.MainBanks");
            DropForeignKey("dbo.SecurityDeposits", "collectionMethodId", "dbo.CollectionMethods");
            DropForeignKey("dbo.SecurityDeposits", "bankId", "dbo.Banks");
            DropForeignKey("dbo.SecurityDeposits", "accountId", "dbo.Accounts");
            DropForeignKey("dbo.RentalOrders", "RentalContract_Id", "dbo.RentalContracts");
            DropForeignKey("dbo.RentalOrders", "TenantRentalId", "dbo.TenantRentals");
            DropForeignKey("dbo.RentalOrderStatusStatusClasses", "StatusClass_Id", "dbo.StatusClasses");
            DropForeignKey("dbo.RentalOrderStatusStatusClasses", "RentalOrderStatus_Id", "dbo.RentalOrderStatus");
            DropForeignKey("dbo.RentalOrders", "statusId", "dbo.RentalOrderStatus");
            DropForeignKey("dbo.RentalInvoices", "RentalOrder_Id", "dbo.RentalOrders");
            DropForeignKey("dbo.RentalInvoices", "TenantRentalId", "dbo.TenantRentals");
            DropForeignKey("dbo.RentalInvoiceStatusStatusClasses", "StatusClass_Id", "dbo.StatusClasses");
            DropForeignKey("dbo.RentalInvoiceStatusStatusClasses", "RentalInvoiceStatus_Id", "dbo.RentalInvoiceStatus");
            DropForeignKey("dbo.RentalInvoices", "statusId", "dbo.RentalInvoiceStatus");
            DropForeignKey("dbo.RentalInvoices", "deptId", "dbo.tabDepartment");
            DropForeignKey("dbo.RentalInvoices", "currencyId", "dbo.Currencies");
            DropForeignKey("dbo.RentalInvoices", "creatorId", "dbo.Users");
            DropForeignKey("dbo.RentalInvoices", "companyId", "dbo.tabCompany");
            DropForeignKey("dbo.RentalInvoices", "assetRentalId", "dbo.AssetRentals");
            DropForeignKey("dbo.RentalOrders", "deptId", "dbo.tabDepartment");
            DropForeignKey("dbo.RentalOrders", "currencyId", "dbo.Currencies");
            DropForeignKey("dbo.RentalOrders", "creatorId", "dbo.Users");
            DropForeignKey("dbo.RentalOrders", "companyId", "dbo.tabCompany");
            DropForeignKey("dbo.RentalOrders", "assetRentalId", "dbo.AssetRentals");
            DropForeignKey("dbo.RentalContracts", "currencyId", "dbo.Currencies");
            DropIndex("dbo.RentalOrderStatusStatusClasses", new[] { "StatusClass_Id" });
            DropIndex("dbo.RentalOrderStatusStatusClasses", new[] { "RentalOrderStatus_Id" });
            DropIndex("dbo.RentalInvoiceStatusStatusClasses", new[] { "StatusClass_Id" });
            DropIndex("dbo.RentalInvoiceStatusStatusClasses", new[] { "RentalInvoiceStatus_Id" });
            DropIndex("dbo.SecurityDeposits", new[] { "collectionMethodId" });
            DropIndex("dbo.SecurityDeposits", new[] { "accountId" });
            DropIndex("dbo.SecurityDeposits", new[] { "bankId" });
            DropIndex("dbo.SecurityDeposits", new[] { "mainBankId" });
            DropIndex("dbo.RentalInvoices", new[] { "RentalOrder_Id" });
            DropIndex("dbo.RentalInvoices", new[] { "currencyId" });
            DropIndex("dbo.RentalInvoices", new[] { "statusId" });
            DropIndex("dbo.RentalInvoices", new[] { "creatorId" });
            DropIndex("dbo.RentalInvoices", new[] { "TenantRentalId" });
            DropIndex("dbo.RentalInvoices", new[] { "assetRentalId" });
            DropIndex("dbo.RentalInvoices", new[] { "deptId" });
            DropIndex("dbo.RentalInvoices", new[] { "companyId" });
            DropIndex("dbo.RentalOrders", new[] { "RentalContract_Id" });
            DropIndex("dbo.RentalOrders", new[] { "currencyId" });
            DropIndex("dbo.RentalOrders", new[] { "statusId" });
            DropIndex("dbo.RentalOrders", new[] { "creatorId" });
            DropIndex("dbo.RentalOrders", new[] { "TenantRentalId" });
            DropIndex("dbo.RentalOrders", new[] { "assetRentalId" });
            DropIndex("dbo.RentalOrders", new[] { "deptId" });
            DropIndex("dbo.RentalOrders", new[] { "companyId" });
            DropIndex("dbo.RentalContracts", new[] { "currencyId" });
            DropColumn("dbo.RentalContracts", "MER");
            DropColumn("dbo.RentalContracts", "currencyId");
            DropTable("dbo.RentalOrderStatusStatusClasses");
            DropTable("dbo.RentalInvoiceStatusStatusClasses");
            DropTable("dbo.SecurityDeposits");
            DropTable("dbo.RentalOrderStatus");
            DropTable("dbo.RentalInvoiceStatus");
            DropTable("dbo.RentalInvoices");
            DropTable("dbo.RentalOrders");
        }
    }
}
