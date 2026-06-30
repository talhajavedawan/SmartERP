namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LoansAdvancesToSaleOrderRelationAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LoansAdvances", "saleOrderId", c => c.Int());
            CreateIndex("dbo.LoansAdvances", "saleOrderId");
            AddForeignKey("dbo.LoansAdvances", "saleOrderId", "dbo.SaleOrders", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LoansAdvances", "saleOrderId", "dbo.SaleOrders");
            DropIndex("dbo.LoansAdvances", new[] { "saleOrderId" });
            DropColumn("dbo.LoansAdvances", "saleOrderId");
        }
    }
}
