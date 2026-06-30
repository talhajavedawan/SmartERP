namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ExpectedPaymentDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SaleOrders", "ExpectedPayment", c => c.DateTime());
            AddColumn("dbo.SaleOrders", "SoAmountSER", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SaleOrders", "SoAmountSER");
            DropColumn("dbo.SaleOrders", "ExpectedPayment");
        }
    }
}
