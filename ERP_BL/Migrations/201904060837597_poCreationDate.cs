namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class poCreationDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Offers", "OfferDate", c => c.DateTime());
            AddColumn("dbo.PurchaseOrders", "CreationDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PurchaseOrders", "CreationDate");
            DropColumn("dbo.Offers", "OfferDate");
        }
    }
}
