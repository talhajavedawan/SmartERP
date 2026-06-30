namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SaleInvoiceLinkedToLoansAdvances : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LoansAdvances", "SaleInvoiceId", c => c.Int());
            CreateIndex("dbo.LoansAdvances", "SaleInvoiceId");
            AddForeignKey("dbo.LoansAdvances", "SaleInvoiceId", "dbo.SaleInvoices", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LoansAdvances", "SaleInvoiceId", "dbo.SaleInvoices");
            DropIndex("dbo.LoansAdvances", new[] { "SaleInvoiceId" });
            DropColumn("dbo.LoansAdvances", "SaleInvoiceId");
        }
    }
}
