namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class STLSettlmentsAndSTLPaymentsAdded : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.STLPayments", "accountOne_Id", "dbo.Accounts");
            DropForeignKey("dbo.STLPayments", "accountThree_Id", "dbo.Accounts");
            DropForeignKey("dbo.STLPayments", "accountTwo_Id", "dbo.Accounts");
            DropForeignKey("dbo.STLPayments", "bankOne_Id", "dbo.Banks");
            DropForeignKey("dbo.STLPayments", "bankOneCurrency_Id", "dbo.Currencies");
            DropForeignKey("dbo.STLPayments", "bankThree_Id", "dbo.Banks");
            DropForeignKey("dbo.STLPayments", "bankTwo_Id", "dbo.Banks");
            DropForeignKey("dbo.STLPayments", "bankTwoCurrency_Id", "dbo.Currencies");
            DropForeignKey("dbo.STLs", "stlPayment_Id", "dbo.STLPayments");
            DropForeignKey("dbo.STLs", "stlSettlment_Id", "dbo.STLSettlments");
            DropIndex("dbo.STLs", new[] { "stlPayment_Id" });
            DropIndex("dbo.STLs", new[] { "stlSettlment_Id" });
            DropIndex("dbo.STLPayments", new[] { "bankOne_Id" });
            DropIndex("dbo.STLPayments", new[] { "bankOneCurrency_Id" });
            DropIndex("dbo.STLPayments", new[] { "accountOne_Id" });
            DropIndex("dbo.STLPayments", new[] { "bankTwo_Id" });
            DropIndex("dbo.STLPayments", new[] { "bankTwoCurrency_Id" });
            DropIndex("dbo.STLPayments", new[] { "accountTwo_Id" });
            DropIndex("dbo.STLPayments", new[] { "bankThree_Id" });
            DropIndex("dbo.STLPayments", new[] { "accountThree_Id" });
            AddColumn("dbo.STLs", "creditTenureNo", c => c.Double(nullable: false));
            AddColumn("dbo.STLs", "extendedCreditTenureNo", c => c.Double(nullable: false));
            AddColumn("dbo.STLs", "paymentAmountOC", c => c.Double(nullable: false));
            AddColumn("dbo.STLs", "paymentBank_Id", c => c.Int());
            AddColumn("dbo.STLs", "paymentCurrency_Id", c => c.Int());
            AddColumn("dbo.STLs", "paymentAccount_Id", c => c.Int());
            AddColumn("dbo.STLs", "settlmentAmount", c => c.Double(nullable: false));
            AddColumn("dbo.STLs", "settlmentBalance", c => c.Double(nullable: false));
            AddColumn("dbo.STLs", "marginReversal", c => c.Double(nullable: false));
            AddColumn("dbo.STLs", "stlBank_Id", c => c.Int());
            AddColumn("dbo.STLs", "stlCurrency_Id", c => c.Int());
            AddColumn("dbo.STLs", "stlPaymentAmountOC", c => c.Double(nullable: false));
            AddColumn("dbo.STLs", "stlAccount_Id", c => c.Int());
            AddColumn("dbo.STLs", "cashMarginBank_Id", c => c.Int());
            AddColumn("dbo.STLs", "cashMarginCurrency_Id", c => c.Int());
            AddColumn("dbo.STLs", "paymentAmountSTL", c => c.Double(nullable: false));
            AddColumn("dbo.STLs", "paymentSTLER", c => c.Double(nullable: false));
            AddColumn("dbo.STLs", "paymentAmountSTLMER", c => c.Double(nullable: false));
            AddColumn("dbo.STLs", "cashMarginAccount_Id", c => c.Int());
            AddColumn("dbo.STLs", "cashMarginPercent", c => c.Double(nullable: false));
            AddColumn("dbo.STLs", "cashMarginAmount", c => c.Double(nullable: false));
            AlterColumn("dbo.STLs", "CreationDate", c => c.DateTime());
            CreateIndex("dbo.STLs", "paymentBank_Id");
            CreateIndex("dbo.STLs", "paymentCurrency_Id");
            CreateIndex("dbo.STLs", "paymentAccount_Id");
            CreateIndex("dbo.STLs", "stlBank_Id");
            CreateIndex("dbo.STLs", "stlCurrency_Id");
            CreateIndex("dbo.STLs", "stlAccount_Id");
            CreateIndex("dbo.STLs", "cashMarginBank_Id");
            CreateIndex("dbo.STLs", "cashMarginCurrency_Id");
            CreateIndex("dbo.STLs", "cashMarginAccount_Id");
            AddForeignKey("dbo.STLs", "cashMarginAccount_Id", "dbo.Accounts", "Id");
            AddForeignKey("dbo.STLs", "cashMarginBank_Id", "dbo.Banks", "Id");
            AddForeignKey("dbo.STLs", "cashMarginCurrency_Id", "dbo.Currencies", "Id");
            AddForeignKey("dbo.STLs", "paymentAccount_Id", "dbo.Accounts", "Id");
            AddForeignKey("dbo.STLs", "paymentBank_Id", "dbo.Banks", "Id");
            AddForeignKey("dbo.STLs", "paymentCurrency_Id", "dbo.Currencies", "Id");
            AddForeignKey("dbo.STLs", "stlAccount_Id", "dbo.Accounts", "Id");
            AddForeignKey("dbo.STLs", "stlBank_Id", "dbo.Banks", "Id");
            AddForeignKey("dbo.STLs", "stlCurrency_Id", "dbo.Currencies", "Id");
            DropColumn("dbo.STLs", "stlPayment_Id");
            DropColumn("dbo.STLs", "stlSettlment_Id");
            DropTable("dbo.STLPayments");
            DropTable("dbo.STLSettlments");
        }
        
        public override void Down()
        {
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
                .PrimaryKey(t => t.Id);

            AddColumn("dbo.STLs", "stlSettlment_Id", c => c.Int(nullable: false));
            AddColumn("dbo.STLs", "stlPayment_Id", c => c.Int(nullable: false));
            DropForeignKey("dbo.STLs", "stlCurrency_Id", "dbo.Currencies");
            DropForeignKey("dbo.STLs", "stlBank_Id", "dbo.Banks");
            DropForeignKey("dbo.STLs", "stlAccount_Id", "dbo.Accounts");
            DropForeignKey("dbo.STLs", "paymentCurrency_Id", "dbo.Currencies");
            DropForeignKey("dbo.STLs", "paymentBank_Id", "dbo.Banks");
            DropForeignKey("dbo.STLs", "paymentAccount_Id", "dbo.Accounts");
            DropForeignKey("dbo.STLs", "cashMarginCurrency_Id", "dbo.Currencies");
            DropForeignKey("dbo.STLs", "cashMarginBank_Id", "dbo.Banks");
            DropForeignKey("dbo.STLs", "cashMarginAccount_Id", "dbo.Accounts");
            DropIndex("dbo.STLs", new[] { "cashMarginAccount_Id" });
            DropIndex("dbo.STLs", new[] { "cashMarginCurrency_Id" });
            DropIndex("dbo.STLs", new[] { "cashMarginBank_Id" });
            DropIndex("dbo.STLs", new[] { "stlAccount_Id" });
            DropIndex("dbo.STLs", new[] { "stlCurrency_Id" });
            DropIndex("dbo.STLs", new[] { "stlBank_Id" });
            DropIndex("dbo.STLs", new[] { "paymentAccount_Id" });
            DropIndex("dbo.STLs", new[] { "paymentCurrency_Id" });
            DropIndex("dbo.STLs", new[] { "paymentBank_Id" });
            AlterColumn("dbo.STLs", "CreationDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.STLs", "cashMarginAmount");
            DropColumn("dbo.STLs", "cashMarginPercent");
            DropColumn("dbo.STLs", "cashMarginAccount_Id");
            DropColumn("dbo.STLs", "paymentAmountSTLMER");
            DropColumn("dbo.STLs", "paymentSTLER");
            DropColumn("dbo.STLs", "paymentAmountSTL");
            DropColumn("dbo.STLs", "cashMarginCurrency_Id");
            DropColumn("dbo.STLs", "cashMarginBank_Id");
            DropColumn("dbo.STLs", "stlAccount_Id");
            DropColumn("dbo.STLs", "stlPaymentAmountOC");
            DropColumn("dbo.STLs", "stlCurrency_Id");
            DropColumn("dbo.STLs", "stlBank_Id");
            DropColumn("dbo.STLs", "marginReversal");
            DropColumn("dbo.STLs", "settlmentBalance");
            DropColumn("dbo.STLs", "settlmentAmount");
            DropColumn("dbo.STLs", "paymentAccount_Id");
            DropColumn("dbo.STLs", "paymentCurrency_Id");
            DropColumn("dbo.STLs", "paymentBank_Id");
            DropColumn("dbo.STLs", "paymentAmountOC");
            DropColumn("dbo.STLs", "extendedCreditTenureNo");
            DropColumn("dbo.STLs", "creditTenureNo");
            CreateIndex("dbo.STLPayments", "accountThree_Id");
            CreateIndex("dbo.STLPayments", "bankThree_Id");
            CreateIndex("dbo.STLPayments", "accountTwo_Id");
            CreateIndex("dbo.STLPayments", "bankTwoCurrency_Id");
            CreateIndex("dbo.STLPayments", "bankTwo_Id");
            CreateIndex("dbo.STLPayments", "accountOne_Id");
            CreateIndex("dbo.STLPayments", "bankOneCurrency_Id");
            CreateIndex("dbo.STLPayments", "bankOne_Id");
            CreateIndex("dbo.STLs", "stlSettlment_Id");
            CreateIndex("dbo.STLs", "stlPayment_Id");
            AddForeignKey("dbo.STLs", "stlSettlment_Id", "dbo.STLSettlments", "Id", cascadeDelete: true);
            AddForeignKey("dbo.STLs", "stlPayment_Id", "dbo.STLPayments", "Id", cascadeDelete: true);
            AddForeignKey("dbo.STLPayments", "bankTwoCurrency_Id", "dbo.Currencies", "Id");
            AddForeignKey("dbo.STLPayments", "bankTwo_Id", "dbo.Banks", "Id");
            AddForeignKey("dbo.STLPayments", "bankThree_Id", "dbo.Banks", "Id");
            AddForeignKey("dbo.STLPayments", "bankOneCurrency_Id", "dbo.Currencies", "Id");
            AddForeignKey("dbo.STLPayments", "bankOne_Id", "dbo.Banks", "Id");
            AddForeignKey("dbo.STLPayments", "accountTwo_Id", "dbo.Accounts", "Id");
            AddForeignKey("dbo.STLPayments", "accountThree_Id", "dbo.Accounts", "Id");
            AddForeignKey("dbo.STLPayments", "accountOne_Id", "dbo.Accounts", "Id");
        }
    }
}
