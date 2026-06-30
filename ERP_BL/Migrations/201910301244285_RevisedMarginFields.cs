namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RevisedMarginFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SaleOrders", "RevisedMargin", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.SaleOrders", "RevisedMargininBase", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.SaleOrders", "SalesRevisedMargin", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.SaleOrders", "RevisedMarginPercent", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.CostSheets", "TotalRevisedMargin", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CostSheets", "TotalRevisedMargin");
            DropColumn("dbo.SaleOrders", "RevisedMarginPercent");
            DropColumn("dbo.SaleOrders", "SalesRevisedMargin");
            DropColumn("dbo.SaleOrders", "RevisedMargininBase");
            DropColumn("dbo.SaleOrders", "RevisedMargin");
        }
    }
}
