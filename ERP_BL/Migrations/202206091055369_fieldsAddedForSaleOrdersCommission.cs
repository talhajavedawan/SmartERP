namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fieldsAddedForSaleOrdersCommission : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SaleOrders", "totalComissionSER", c => c.Double(nullable: false));
            AddColumn("dbo.SaleOrders", "totalComissionMER", c => c.Double(nullable: false));
            AddColumn("dbo.SaleOrders", "totalNetComissionSER", c => c.Double(nullable: false));
            AddColumn("dbo.SaleOrders", "totalNetComissionMER", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SaleOrders", "totalNetComissionMER");
            DropColumn("dbo.SaleOrders", "totalNetComissionSER");
            DropColumn("dbo.SaleOrders", "totalComissionMER");
            DropColumn("dbo.SaleOrders", "totalComissionSER");
        }
    }
}
