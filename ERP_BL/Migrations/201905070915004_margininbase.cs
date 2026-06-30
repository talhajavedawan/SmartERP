namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class margininbase : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SaleOrders", "commisioninBase", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.SaleOrders", "BudgetedMargininBase", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.SaleOrders", "SalesBudgetedMargin", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.SaleOrders", "ActualMargin", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.SaleOrders", "ActualMargininBase", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.SaleOrders", "SalesActualMargin", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SaleOrders", "SalesActualMargin");
            DropColumn("dbo.SaleOrders", "ActualMargininBase");
            DropColumn("dbo.SaleOrders", "ActualMargin");
            DropColumn("dbo.SaleOrders", "SalesBudgetedMargin");
            DropColumn("dbo.SaleOrders", "BudgetedMargininBase");
            DropColumn("dbo.SaleOrders", "commisioninBase");
        }
    }
}
