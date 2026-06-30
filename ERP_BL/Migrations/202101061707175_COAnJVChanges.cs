namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class COAnJVChanges : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.JournalVouchers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        entryNo = c.String(),
                        voucherRefno = c.String(),
                        postingDate = c.DateTime(nullable: false),
                        userId = c.Int(),
                        isVoid = c.Boolean(nullable: false),
                        coaTransactionsType = c.Int(nullable: false),
                        statusId = c.Int(),
                        LastStatusChangeDate = c.DateTime(),
                        isReviewed = c.Boolean(),
                        needReview = c.Boolean(),
                        PendingForClosing = c.Boolean(),
                        PendingForReApproval = c.Boolean(),
                        isApproved = c.Boolean(),
                        ApprovedDate = c.DateTime(),
                        ReApprovalDate = c.DateTime(),
                        isReApproved = c.Boolean(),
                        stage = c.String(),
                        ClosingDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.JournalVoucherStatus", t => t.statusId)
                .ForeignKey("dbo.Users", t => t.userId)
                .Index(t => t.userId)
                .Index(t => t.statusId);
            
            CreateTable(
                "dbo.JournalVoucherStatus",
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
            
            AddColumn("dbo.PurchaseOrders", "proforma", c => c.String());
            AddColumn("dbo.ChartofAccounts", "isOpeningBalance", c => c.Boolean(nullable: false));
            AddColumn("dbo.ChartofAccounts", "companyId", c => c.Int());
            AddColumn("dbo.ChartofAccounts", "currencyId", c => c.Int());
            AddColumn("dbo.ChartofAccounts", "stage", c => c.String());
            AddColumn("dbo.ChartofAccounts", "isReApproved", c => c.Boolean(nullable: false));
            AddColumn("dbo.ChartofAccounts", "ReApprovalDate", c => c.DateTime());
            AddColumn("dbo.JournalTransactions", "journalVoucher_Id", c => c.Int());
            CreateIndex("dbo.ChartofAccounts", "companyId");
            CreateIndex("dbo.ChartofAccounts", "currencyId");
            CreateIndex("dbo.JournalTransactions", "journalVoucher_Id");
            AddForeignKey("dbo.ChartofAccounts", "companyId", "dbo.tabCompany", "Id");
            AddForeignKey("dbo.ChartofAccounts", "currencyId", "dbo.Currencies", "Id");
            AddForeignKey("dbo.JournalTransactions", "journalVoucher_Id", "dbo.JournalVouchers", "Id");
            DropColumn("dbo.ChartofAccounts", "openingBalanceTotalDate");
            DropColumn("dbo.JournalTransactions", "entryNumber");
            DropColumn("dbo.JournalTransactions", "postingDate");
            DropColumn("dbo.JournalTransactions", "isVoid");
        }
        
        public override void Down()
        {
            AddColumn("dbo.JournalTransactions", "isVoid", c => c.Boolean(nullable: false));
            AddColumn("dbo.JournalTransactions", "postingDate", c => c.DateTime());
            AddColumn("dbo.JournalTransactions", "entryNumber", c => c.String());
            AddColumn("dbo.ChartofAccounts", "openingBalanceTotalDate", c => c.DateTime());
            DropForeignKey("dbo.JournalVouchers", "userId", "dbo.Users");
            DropForeignKey("dbo.JournalVouchers", "statusId", "dbo.JournalVoucherStatus");
            DropForeignKey("dbo.JournalTransactions", "journalVoucher_Id", "dbo.JournalVouchers");
            DropForeignKey("dbo.ChartofAccounts", "currencyId", "dbo.Currencies");
            DropForeignKey("dbo.ChartofAccounts", "companyId", "dbo.tabCompany");
            DropIndex("dbo.JournalVouchers", new[] { "statusId" });
            DropIndex("dbo.JournalVouchers", new[] { "userId" });
            DropIndex("dbo.JournalTransactions", new[] { "journalVoucher_Id" });
            DropIndex("dbo.ChartofAccounts", new[] { "currencyId" });
            DropIndex("dbo.ChartofAccounts", new[] { "companyId" });
            DropColumn("dbo.JournalTransactions", "journalVoucher_Id");
            DropColumn("dbo.ChartofAccounts", "ReApprovalDate");
            DropColumn("dbo.ChartofAccounts", "isReApproved");
            DropColumn("dbo.ChartofAccounts", "stage");
            DropColumn("dbo.ChartofAccounts", "currencyId");
            DropColumn("dbo.ChartofAccounts", "companyId");
            DropColumn("dbo.ChartofAccounts", "isOpeningBalance");
            DropColumn("dbo.PurchaseOrders", "proforma");
            DropTable("dbo.JournalVoucherStatus");
            DropTable("dbo.JournalVouchers");
        }
    }
}
