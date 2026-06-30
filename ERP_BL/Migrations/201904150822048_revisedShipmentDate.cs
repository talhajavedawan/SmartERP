namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class revisedShipmentDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PurchaseOrders", "revisedShipmentDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PurchaseOrders", "revisedShipmentDate");
        }
    }
}
