namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PrincipalExchangeRateValue : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SaleOrders", "PERValue", c => c.Double(nullable: false));
            DropColumn("dbo.SaleOrders", "SoAmountCfrPER");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SaleOrders", "SoAmountCfrPER", c => c.Double(nullable: false));
            DropColumn("dbo.SaleOrders", "PERValue");
        }
    }
}
