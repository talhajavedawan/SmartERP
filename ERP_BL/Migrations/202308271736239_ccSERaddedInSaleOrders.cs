namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ccSERaddedInSaleOrders : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SaleOrders", "ccSER", c => c.Double(nullable: false));
            AddColumn("dbo.SaleOrders", "ccMER", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SaleOrders", "ccMER");
            DropColumn("dbo.SaleOrders", "ccSER");
        }
    }
}
