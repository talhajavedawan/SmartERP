namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newFieldsAddedInTasks : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tasks", "SystemId", c => c.Int(nullable: false));
            AddColumn("dbo.Tasks", "TaskRef", c => c.String());
            AddColumn("dbo.Tasks", "isCompleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tasks", "isCompleted");
            DropColumn("dbo.Tasks", "TaskRef");
            DropColumn("dbo.Tasks", "SystemId");
        }
    }
}
