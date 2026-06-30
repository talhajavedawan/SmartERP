namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesInLeavesClass : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Leaves", "isApproved", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Leaves", "isApproved");
        }
    }
}
