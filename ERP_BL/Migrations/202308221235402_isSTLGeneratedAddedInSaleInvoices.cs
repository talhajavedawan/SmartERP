namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class isSTLGeneratedAddedInSaleInvoices : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SaleInvoices", "isSTLGenerated", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SaleInvoices", "isSTLGenerated");
        }
    }
}
