namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class paymetdueAgeing : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SaleOrders", "PaymentDueStartDate", c => c.DateTime());
            AddColumn("dbo.SaleOrders", "PaymentDueAgeing", c => c.DateTime());
            AddColumn("dbo.SaleOrders", "CreditDays", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SaleOrders", "CreditDays");
            DropColumn("dbo.SaleOrders", "PaymentDueAgeing");
            DropColumn("dbo.SaleOrders", "PaymentDueStartDate");
        }
    }
}
