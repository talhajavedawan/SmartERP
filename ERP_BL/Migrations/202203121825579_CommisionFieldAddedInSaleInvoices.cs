namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CommisionFieldAddedInSaleInvoices : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SaleInvoices", "Commission", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SaleInvoices", "Commission");
        }
    }
}
