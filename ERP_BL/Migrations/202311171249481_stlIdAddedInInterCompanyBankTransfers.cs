namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class stlIdAddedInInterCompanyBankTransfers : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InterCompanyBankTransfers", "stlId", c => c.Int());
            AddColumn("dbo.SaleInvoices", "invoiceNo", c => c.String());
            AddColumn("dbo.SaleInvoices", "invoiceDate", c => c.DateTime());
            CreateIndex("dbo.InterCompanyBankTransfers", "stlId");
            AddForeignKey("dbo.InterCompanyBankTransfers", "stlId", "dbo.STLs", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.InterCompanyBankTransfers", "stlId", "dbo.STLs");
            DropIndex("dbo.InterCompanyBankTransfers", new[] { "stlId" });
            DropColumn("dbo.SaleInvoices", "invoiceDate");
            DropColumn("dbo.SaleInvoices", "invoiceNo");
            DropColumn("dbo.InterCompanyBankTransfers", "stlId");
        }
    }
}
