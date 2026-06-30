namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class STLAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.STLs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        paymentGroupId = c.Double(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        company_Id = c.Int(),
                        dept_Id = c.Int(),
                        user_Id = c.Int(),
                        statusId = c.Int(),
                        LastStatusChangeDate = c.DateTime(),
                        stlPaymentDate = c.DateTime(),
                        creditTenure = c.DateTime(),
                        extendedCreditTenure = c.DateTime(),
                        paymentDueDays = c.Double(nullable: false),
                        stlUtilizedDays = c.Double(nullable: false),
                        interestPercent = c.Double(nullable: false),
                        interestAmountCurrency_Id = c.Int(),
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
                        stlPayment_Id = c.Int(nullable: false),
                        stlSettlment_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tabCompany", t => t.company_Id)
                .ForeignKey("dbo.tabDepartment", t => t.dept_Id)
                .ForeignKey("dbo.Currencies", t => t.interestAmountCurrency_Id)
                .ForeignKey("dbo.STLPayments", t => t.stlPayment_Id, cascadeDelete: true)
                .ForeignKey("dbo.STLSettlments", t => t.stlSettlment_Id, cascadeDelete: true)
                .ForeignKey("dbo.STLStatus", t => t.statusId)
                .ForeignKey("dbo.Users", t => t.user_Id)
                .Index(t => t.company_Id)
                .Index(t => t.dept_Id)
                .Index(t => t.user_Id)
                .Index(t => t.statusId)
                .Index(t => t.interestAmountCurrency_Id)
                .Index(t => t.stlPayment_Id)
                .Index(t => t.stlSettlment_Id);
            
            CreateTable(
                "dbo.STLPayments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        bankOne_Id = c.Int(),
                        bankOneCurrency_Id = c.Int(),
                        paymentAmountOC = c.Double(nullable: false),
                        accountOne_Id = c.Int(),
                        bankTwo_Id = c.Int(),
                        bankTwoCurrency_Id = c.Int(),
                        paymentAmountSTL = c.Double(nullable: false),
                        paymentSTLER = c.Double(nullable: false),
                        paymentAmountSTLMER = c.Double(nullable: false),
                        accountTwo_Id = c.Int(),
                        bankThree_Id = c.Int(),
                        cashMarginPercent = c.Double(nullable: false),
                        cashMarginAmount = c.Double(nullable: false),
                        accountThree_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Accounts", t => t.accountOne_Id)
                .ForeignKey("dbo.Accounts", t => t.accountThree_Id)
                .ForeignKey("dbo.Accounts", t => t.accountTwo_Id)
                .ForeignKey("dbo.Banks", t => t.bankOne_Id)
                .ForeignKey("dbo.Currencies", t => t.bankOneCurrency_Id)
                .ForeignKey("dbo.Banks", t => t.bankThree_Id)
                .ForeignKey("dbo.Banks", t => t.bankTwo_Id)
                .ForeignKey("dbo.Currencies", t => t.bankTwoCurrency_Id)
                .Index(t => t.bankOne_Id)
                .Index(t => t.bankOneCurrency_Id)
                .Index(t => t.accountOne_Id)
                .Index(t => t.bankTwo_Id)
                .Index(t => t.bankTwoCurrency_Id)
                .Index(t => t.accountTwo_Id)
                .Index(t => t.bankThree_Id)
                .Index(t => t.accountThree_Id);
            
            CreateTable(
                "dbo.STLSettlments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        settlmentAmount = c.Double(nullable: false),
                        settlmentBalance = c.Double(nullable: false),
                        marginReversal = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.STLStatus",
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.STLs", "user_Id", "dbo.Users");
            DropForeignKey("dbo.STLs", "statusId", "dbo.STLStatus");
            DropForeignKey("dbo.STLs", "stlSettlment_Id", "dbo.STLSettlments");
            DropForeignKey("dbo.STLs", "stlPayment_Id", "dbo.STLPayments");
            DropForeignKey("dbo.STLPayments", "bankTwoCurrency_Id", "dbo.Currencies");
            DropForeignKey("dbo.STLPayments", "bankTwo_Id", "dbo.Banks");
            DropForeignKey("dbo.STLPayments", "bankThree_Id", "dbo.Banks");
            DropForeignKey("dbo.STLPayments", "bankOneCurrency_Id", "dbo.Currencies");
            DropForeignKey("dbo.STLPayments", "bankOne_Id", "dbo.Banks");
            DropForeignKey("dbo.STLPayments", "accountTwo_Id", "dbo.Accounts");
            DropForeignKey("dbo.STLPayments", "accountThree_Id", "dbo.Accounts");
            DropForeignKey("dbo.STLPayments", "accountOne_Id", "dbo.Accounts");
            DropForeignKey("dbo.STLs", "interestAmountCurrency_Id", "dbo.Currencies");
            DropForeignKey("dbo.STLs", "dept_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.STLs", "company_Id", "dbo.tabCompany");
            DropIndex("dbo.STLPayments", new[] { "accountThree_Id" });
            DropIndex("dbo.STLPayments", new[] { "bankThree_Id" });
            DropIndex("dbo.STLPayments", new[] { "accountTwo_Id" });
            DropIndex("dbo.STLPayments", new[] { "bankTwoCurrency_Id" });
            DropIndex("dbo.STLPayments", new[] { "bankTwo_Id" });
            DropIndex("dbo.STLPayments", new[] { "accountOne_Id" });
            DropIndex("dbo.STLPayments", new[] { "bankOneCurrency_Id" });
            DropIndex("dbo.STLPayments", new[] { "bankOne_Id" });
            DropIndex("dbo.STLs", new[] { "stlSettlment_Id" });
            DropIndex("dbo.STLs", new[] { "stlPayment_Id" });
            DropIndex("dbo.STLs", new[] { "interestAmountCurrency_Id" });
            DropIndex("dbo.STLs", new[] { "statusId" });
            DropIndex("dbo.STLs", new[] { "user_Id" });
            DropIndex("dbo.STLs", new[] { "dept_Id" });
            DropIndex("dbo.STLs", new[] { "company_Id" });
            DropTable("dbo.STLStatus");
            DropTable("dbo.STLSettlments");
            DropTable("dbo.STLPayments");
            DropTable("dbo.STLs");
        }
    }
}
