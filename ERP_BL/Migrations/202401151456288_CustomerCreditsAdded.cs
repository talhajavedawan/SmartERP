namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CustomerCreditsAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CustomerCredits",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SaleInvoiceId = c.Int(),
                        CustomerCompanyId = c.Int(),
                        creditAmount = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CustomerCompanies", t => t.CustomerCompanyId)
                .ForeignKey("dbo.SaleInvoices", t => t.SaleInvoiceId)
                .Index(t => t.SaleInvoiceId)
                .Index(t => t.CustomerCompanyId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CustomerCredits", "SaleInvoiceId", "dbo.SaleInvoices");
            DropForeignKey("dbo.CustomerCredits", "CustomerCompanyId", "dbo.CustomerCompanies");
            DropIndex("dbo.CustomerCredits", new[] { "CustomerCompanyId" });
            DropIndex("dbo.CustomerCredits", new[] { "SaleInvoiceId" });
            DropTable("dbo.CustomerCredits");
        }
    }
}
