namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class isGeneratedBySOLinkAddedInSaleOrders : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SaleOrders", "isGeneratedBySOLink", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SaleOrders", "isGeneratedBySOLink");
        }
    }
}
