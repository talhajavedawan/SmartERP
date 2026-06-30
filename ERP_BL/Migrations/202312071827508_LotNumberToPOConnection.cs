namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LotNumberToPOConnection : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PurchaseOrders", "lotNo", c => c.String());
            AddColumn("dbo.PurchaseOrders", "lotNumberId", c => c.Int());
            CreateIndex("dbo.PurchaseOrders", "lotNumberId");
            AddForeignKey("dbo.PurchaseOrders", "lotNumberId", "dbo.LotNumbers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PurchaseOrders", "lotNumberId", "dbo.LotNumbers");
            DropIndex("dbo.PurchaseOrders", new[] { "lotNumberId" });
            DropColumn("dbo.PurchaseOrders", "lotNumberId");
            DropColumn("dbo.PurchaseOrders", "lotNo");
        }
    }
}
