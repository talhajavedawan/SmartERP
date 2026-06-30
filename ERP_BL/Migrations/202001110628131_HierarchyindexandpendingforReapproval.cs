namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HierarchyindexandpendingforReapproval : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TargetTypes", "HierarchicalIndex", c => c.Int(nullable: false));
            AddColumn("dbo.SaleOrders", "PendingForReApproval", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SaleOrders", "PendingForReApproval");
            DropColumn("dbo.TargetTypes", "HierarchicalIndex");
        }
    }
}
