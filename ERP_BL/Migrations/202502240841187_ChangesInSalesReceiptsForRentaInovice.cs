namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesInSalesReceiptsForRentaInovice : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SalesReceipts", "rentalInvoiceId", c => c.Int());
            CreateIndex("dbo.SalesReceipts", "rentalInvoiceId");
            AddForeignKey("dbo.SalesReceipts", "rentalInvoiceId", "dbo.RentalInvoices", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SalesReceipts", "rentalInvoiceId", "dbo.RentalInvoices");
            DropIndex("dbo.SalesReceipts", new[] { "rentalInvoiceId" });
            DropColumn("dbo.SalesReceipts", "rentalInvoiceId");
        }
    }
}
