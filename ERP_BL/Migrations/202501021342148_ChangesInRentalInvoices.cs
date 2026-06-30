namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesInRentalInvoices : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RentalInvoices", "RentMonth", c => c.DateTime());
            AddColumn("dbo.RentalInvoices", "InvoiceAmountMER", c => c.Double(nullable: false));
            DropColumn("dbo.RentalInvoices", "NumberOfMonths");
        }
        
        public override void Down()
        {
            AddColumn("dbo.RentalInvoices", "NumberOfMonths", c => c.Double(nullable: false));
            DropColumn("dbo.RentalInvoices", "InvoiceAmountMER");
            DropColumn("dbo.RentalInvoices", "RentMonth");
        }
    }
}
