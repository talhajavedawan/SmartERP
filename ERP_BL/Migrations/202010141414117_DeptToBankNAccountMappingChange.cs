namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeptToBankNAccountMappingChange : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.tabDepartment", "Account_Id", "dbo.Accounts");
            DropForeignKey("dbo.Bills", "incoterm_Id", "dbo.Incoterms");
            DropForeignKey("dbo.Bills", "POPaymentterm_Id", "dbo.PaymentTerms");
            DropForeignKey("dbo.Bills", "SOCurrency_Id", "dbo.Currencies");
            DropForeignKey("dbo.Bills", "SoPaymentterm_Id", "dbo.PaymentTerms");
            DropIndex("dbo.tabDepartment", new[] { "Account_Id" });
            DropIndex("dbo.Bills", new[] { "SoPaymentterm_Id" });
            DropIndex("dbo.Bills", new[] { "POPaymentterm_Id" });
            DropIndex("dbo.Bills", new[] { "SOCurrency_Id" });
            DropIndex("dbo.Bills", new[] { "incoterm_Id" });
            CreateTable(
                "dbo.DepartmentAccounts",
                c => new
                    {
                        Department_Id = c.Int(nullable: false),
                        Account_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Department_Id, t.Account_Id })
                .ForeignKey("dbo.tabDepartment", t => t.Department_Id, cascadeDelete: true)
                .ForeignKey("dbo.Accounts", t => t.Account_Id, cascadeDelete: true)
                .Index(t => t.Department_Id)
                .Index(t => t.Account_Id);
            
            AddColumn("dbo.Accounts", "IBAN", c => c.String());
            AddColumn("dbo.Banks", "SwiftCode", c => c.String());
            AddColumn("dbo.Assets", "isVoid", c => c.Boolean(nullable: false));
            AddColumn("dbo.Assets", "isReviewed", c => c.Boolean());
            AddColumn("dbo.Assets", "needReview", c => c.Boolean());
            AddColumn("dbo.Assets", "PendingForClosing", c => c.Boolean());
            AddColumn("dbo.Assets", "isApproved", c => c.Boolean());
            AddColumn("dbo.Assets", "ApprovedDate", c => c.DateTime());
            AlterColumn("dbo.Bills", "CreditDays", c => c.Int());
            AlterColumn("dbo.Bills", "TargetYear", c => c.Int());
            AlterColumn("dbo.Bills", "TargetMonth", c => c.Int());
            AlterColumn("dbo.Bills", "ExchangeRate", c => c.Single());
            AlterColumn("dbo.Bills", "SOC_ER", c => c.Double());
            AlterColumn("dbo.Bills", "SoAmountSOC_ER", c => c.Double());
            AlterColumn("dbo.Bills", "marginExchangeRate", c => c.Double());
            AlterColumn("dbo.Bills", "POAmountSER", c => c.Double());
            AlterColumn("dbo.Bills", "isPercentTax", c => c.Boolean());
            AlterColumn("dbo.Bills", "salesTax", c => c.Double());
            AlterColumn("dbo.Bills", "SoPaymentterm_Id", c => c.Int());
            AlterColumn("dbo.Bills", "POPaymentterm_Id", c => c.Int());
            AlterColumn("dbo.Bills", "SOCurrency_Id", c => c.Int());
            AlterColumn("dbo.Bills", "incoterm_Id", c => c.Int());
            CreateIndex("dbo.Bills", "SoPaymentterm_Id");
            CreateIndex("dbo.Bills", "POPaymentterm_Id");
            CreateIndex("dbo.Bills", "SOCurrency_Id");
            CreateIndex("dbo.Bills", "incoterm_Id");
            AddForeignKey("dbo.Bills", "incoterm_Id", "dbo.Incoterms", "Id");
            AddForeignKey("dbo.Bills", "POPaymentterm_Id", "dbo.PaymentTerms", "Id");
            AddForeignKey("dbo.Bills", "SOCurrency_Id", "dbo.Currencies", "Id");
            AddForeignKey("dbo.Bills", "SoPaymentterm_Id", "dbo.PaymentTerms", "Id");
            DropColumn("dbo.tabDepartment", "Account_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tabDepartment", "Account_Id", c => c.Int());
            DropForeignKey("dbo.Bills", "SoPaymentterm_Id", "dbo.PaymentTerms");
            DropForeignKey("dbo.Bills", "SOCurrency_Id", "dbo.Currencies");
            DropForeignKey("dbo.Bills", "POPaymentterm_Id", "dbo.PaymentTerms");
            DropForeignKey("dbo.Bills", "incoterm_Id", "dbo.Incoterms");
            DropForeignKey("dbo.DepartmentAccounts", "Account_Id", "dbo.Accounts");
            DropForeignKey("dbo.DepartmentAccounts", "Department_Id", "dbo.tabDepartment");
            DropIndex("dbo.DepartmentAccounts", new[] { "Account_Id" });
            DropIndex("dbo.DepartmentAccounts", new[] { "Department_Id" });
            DropIndex("dbo.Bills", new[] { "incoterm_Id" });
            DropIndex("dbo.Bills", new[] { "SOCurrency_Id" });
            DropIndex("dbo.Bills", new[] { "POPaymentterm_Id" });
            DropIndex("dbo.Bills", new[] { "SoPaymentterm_Id" });
            AlterColumn("dbo.Bills", "incoterm_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.Bills", "SOCurrency_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.Bills", "POPaymentterm_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.Bills", "SoPaymentterm_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.Bills", "salesTax", c => c.Double(nullable: false));
            AlterColumn("dbo.Bills", "isPercentTax", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Bills", "POAmountSER", c => c.Double(nullable: false));
            AlterColumn("dbo.Bills", "marginExchangeRate", c => c.Double(nullable: false));
            AlterColumn("dbo.Bills", "SoAmountSOC_ER", c => c.Double(nullable: false));
            AlterColumn("dbo.Bills", "SOC_ER", c => c.Double(nullable: false));
            AlterColumn("dbo.Bills", "ExchangeRate", c => c.Single(nullable: false));
            AlterColumn("dbo.Bills", "TargetMonth", c => c.Int(nullable: false));
            AlterColumn("dbo.Bills", "TargetYear", c => c.Int(nullable: false));
            AlterColumn("dbo.Bills", "CreditDays", c => c.Int(nullable: false));
            DropColumn("dbo.Assets", "ApprovedDate");
            DropColumn("dbo.Assets", "isApproved");
            DropColumn("dbo.Assets", "PendingForClosing");
            DropColumn("dbo.Assets", "needReview");
            DropColumn("dbo.Assets", "isReviewed");
            DropColumn("dbo.Assets", "isVoid");
            DropColumn("dbo.Banks", "SwiftCode");
            DropColumn("dbo.Accounts", "IBAN");
            DropTable("dbo.DepartmentAccounts");
            CreateIndex("dbo.Bills", "incoterm_Id");
            CreateIndex("dbo.Bills", "SOCurrency_Id");
            CreateIndex("dbo.Bills", "POPaymentterm_Id");
            CreateIndex("dbo.Bills", "SoPaymentterm_Id");
            CreateIndex("dbo.tabDepartment", "Account_Id");
            AddForeignKey("dbo.Bills", "SoPaymentterm_Id", "dbo.PaymentTerms", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Bills", "SOCurrency_Id", "dbo.Currencies", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Bills", "POPaymentterm_Id", "dbo.PaymentTerms", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Bills", "incoterm_Id", "dbo.Incoterms", "Id", cascadeDelete: true);
            AddForeignKey("dbo.tabDepartment", "Account_Id", "dbo.Accounts", "Id");
        }
    }
}
