namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ColumnAddedInSaleInvoices15122021 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SaleInvoices", "BLdeliveryRefNo", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SaleInvoices", "BLdeliveryRefNo");
        }
    }
}
