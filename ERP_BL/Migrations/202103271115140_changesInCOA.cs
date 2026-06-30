namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changesInCOA : DbMigration
    {
        // wrong name added by mistake. Changes in COA was told by Talha by migration is all about SO changes.
        public override void Up()
        {
            AddColumn("dbo.SaleOrders", "taxNameId", c => c.Int());
            CreateIndex("dbo.SaleOrders", "taxNameId");
            AddForeignKey("dbo.SaleOrders", "taxNameId", "dbo.TaxNames", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SaleOrders", "taxNameId", "dbo.TaxNames");
            DropIndex("dbo.SaleOrders", new[] { "taxNameId" });
            DropColumn("dbo.SaleOrders", "taxNameId");
        }
    }
}
