namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class chagnesInSaleInvoices : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SaleInvoices", "insuranceRequired", c => c.Boolean(nullable: false));
            AddColumn("dbo.SaleInvoices", "insuranceApplied", c => c.Boolean(nullable: false));
            AddColumn("dbo.SaleInvoices", "insuranceNotApplicable", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SaleInvoices", "insuranceNotApplicable");
            DropColumn("dbo.SaleInvoices", "insuranceApplied");
            DropColumn("dbo.SaleInvoices", "insuranceRequired");
        }
    }
}
