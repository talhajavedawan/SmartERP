namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReApproval_SO : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SaleOrders", "ReApprovalDate", c => c.DateTime());
            AddColumn("dbo.SaleOrders", "isReApproved", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SaleOrders", "isReApproved");
            DropColumn("dbo.SaleOrders", "ReApprovalDate");
        }
    }
}
