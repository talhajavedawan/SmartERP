namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedSystemRefatPOBILl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bills", "SyetmReferenceNo", c => c.String());
            AddColumn("dbo.PurchaseOrders", "SyetmReferenceNo", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PurchaseOrders", "SyetmReferenceNo");
            DropColumn("dbo.Bills", "SyetmReferenceNo");
        }
    }
}
