namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class isDisabledAddedInTasksStatus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TasksStatus", "isDisable", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TasksStatus", "isDisable");
        }
    }
}
