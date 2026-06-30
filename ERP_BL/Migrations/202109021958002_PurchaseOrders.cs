namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PurchaseOrders : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PurchaseOrders", "CostSheetFieldId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PurchaseOrders", "CostSheetFieldId");
        }
    }
}
