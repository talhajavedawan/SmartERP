namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class isPendingInNotifications : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notifications", "isPending", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Notifications", "isPending");
        }
    }
}
