namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class principalinpo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PurchaseOrders", "principal_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.PurchaseOrders", "principal_Id");
            AddForeignKey("dbo.PurchaseOrders", "principal_Id", "dbo.Principals", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PurchaseOrders", "principal_Id", "dbo.Principals");
            DropIndex("dbo.PurchaseOrders", new[] { "principal_Id" });
            DropColumn("dbo.PurchaseOrders", "principal_Id");
        }
    }
}
