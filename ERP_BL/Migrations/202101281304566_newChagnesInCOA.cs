namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newChagnesInCOA : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TaxNames",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        taxTypeId = c.Int(),
                        percentage = c.Double(nullable: false),
                        COA_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ChartofAccounts", t => t.COA_Id)
                .ForeignKey("dbo.TaxTypes", t => t.taxTypeId)
                .Index(t => t.taxTypeId)
                .Index(t => t.COA_Id);
            
            CreateTable(
                "dbo.TaxTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TypeName = c.String(),
                        isActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Products", "productType", c => c.Int(nullable: false));
            AddColumn("dbo.Products", "accountReceivable_id", c => c.Int());
            AddColumn("dbo.Products", "incomeAccount_id", c => c.Int());
            AddColumn("dbo.JournalTransactions", "SaleInvoiceId", c => c.Int());
            AddColumn("dbo.JournalTransactions", "prodId", c => c.Int());
            AddColumn("dbo.JournalTransactions", "MER", c => c.Double());
            AddColumn("dbo.JournalTransactions", "AmountMER", c => c.Double());
            AddColumn("dbo.InterBankTransfers", "AmountOCPaid", c => c.Double(nullable: false));
            AddColumn("dbo.InterBankTransfers", "AmountMERPaid", c => c.Double(nullable: false));
            AddColumn("dbo.InterBankTransfers", "taxFlag", c => c.Int(nullable: false));
            AddColumn("dbo.InterBankTransfers", "taxTypeId", c => c.Int());
            AddColumn("dbo.InterBankTransfers", "taxNameId", c => c.Int());
            AddColumn("dbo.InterBankTransfers", "TaxAmountOC", c => c.Double(nullable: false));
            AddColumn("dbo.InterBankTransfers", "TaxAmountMER", c => c.Double(nullable: false));
            AddColumn("dbo.InterBankTransfers", "industryTypeId", c => c.Int());
            AddColumn("dbo.InterBankTransfers", "vendor_Id", c => c.Int());
            CreateIndex("dbo.Products", "accountReceivable_id");
            CreateIndex("dbo.Products", "incomeAccount_id");
            CreateIndex("dbo.JournalTransactions", "SaleInvoiceId");
            CreateIndex("dbo.JournalTransactions", "prodId");
            CreateIndex("dbo.InterBankTransfers", "taxTypeId");
            CreateIndex("dbo.InterBankTransfers", "taxNameId");
            CreateIndex("dbo.InterBankTransfers", "industryTypeId");
            CreateIndex("dbo.InterBankTransfers", "vendor_Id");
            AddForeignKey("dbo.InterBankTransfers", "industryTypeId", "dbo.IndustryTypes", "Id");
            AddForeignKey("dbo.InterBankTransfers", "taxNameId", "dbo.TaxNames", "Id");
            AddForeignKey("dbo.InterBankTransfers", "taxTypeId", "dbo.TaxTypes", "Id");
            AddForeignKey("dbo.JournalTransactions", "SaleInvoiceId", "dbo.SaleInvoices", "Id");
            AddForeignKey("dbo.InterBankTransfers", "vendor_Id", "dbo.tabVendor", "Id");
            AddForeignKey("dbo.JournalTransactions", "prodId", "dbo.Products", "Id");
            AddForeignKey("dbo.Products", "accountReceivable_id", "dbo.ChartofAccounts", "Id");
            AddForeignKey("dbo.Products", "incomeAccount_id", "dbo.ChartofAccounts", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "incomeAccount_id", "dbo.ChartofAccounts");
            DropForeignKey("dbo.Products", "accountReceivable_id", "dbo.ChartofAccounts");
            DropForeignKey("dbo.JournalTransactions", "prodId", "dbo.Products");
            DropForeignKey("dbo.InterBankTransfers", "vendor_Id", "dbo.tabVendor");
            DropForeignKey("dbo.JournalTransactions", "SaleInvoiceId", "dbo.SaleInvoices");
            DropForeignKey("dbo.InterBankTransfers", "taxTypeId", "dbo.TaxTypes");
            DropForeignKey("dbo.InterBankTransfers", "taxNameId", "dbo.TaxNames");
            DropForeignKey("dbo.TaxNames", "taxTypeId", "dbo.TaxTypes");
            DropForeignKey("dbo.TaxNames", "COA_Id", "dbo.ChartofAccounts");
            DropForeignKey("dbo.InterBankTransfers", "industryTypeId", "dbo.IndustryTypes");
            DropIndex("dbo.TaxNames", new[] { "COA_Id" });
            DropIndex("dbo.TaxNames", new[] { "taxTypeId" });
            DropIndex("dbo.InterBankTransfers", new[] { "vendor_Id" });
            DropIndex("dbo.InterBankTransfers", new[] { "industryTypeId" });
            DropIndex("dbo.InterBankTransfers", new[] { "taxNameId" });
            DropIndex("dbo.InterBankTransfers", new[] { "taxTypeId" });
            DropIndex("dbo.JournalTransactions", new[] { "prodId" });
            DropIndex("dbo.JournalTransactions", new[] { "SaleInvoiceId" });
            DropIndex("dbo.Products", new[] { "incomeAccount_id" });
            DropIndex("dbo.Products", new[] { "accountReceivable_id" });
            DropColumn("dbo.InterBankTransfers", "vendor_Id");
            DropColumn("dbo.InterBankTransfers", "industryTypeId");
            DropColumn("dbo.InterBankTransfers", "TaxAmountMER");
            DropColumn("dbo.InterBankTransfers", "TaxAmountOC");
            DropColumn("dbo.InterBankTransfers", "taxNameId");
            DropColumn("dbo.InterBankTransfers", "taxTypeId");
            DropColumn("dbo.InterBankTransfers", "taxFlag");
            DropColumn("dbo.InterBankTransfers", "AmountMERPaid");
            DropColumn("dbo.InterBankTransfers", "AmountOCPaid");
            DropColumn("dbo.JournalTransactions", "AmountMER");
            DropColumn("dbo.JournalTransactions", "MER");
            DropColumn("dbo.JournalTransactions", "prodId");
            DropColumn("dbo.JournalTransactions", "SaleInvoiceId");
            DropColumn("dbo.Products", "incomeAccount_id");
            DropColumn("dbo.Products", "accountReceivable_id");
            DropColumn("dbo.Products", "productType");
            DropTable("dbo.TaxTypes");
            DropTable("dbo.TaxNames");
        }
    }
}
