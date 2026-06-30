namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IsCloedFieldAddedInToDoTasks : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ToDoTasks", "isClosed", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ToDoTasks", "isClosed");
        }
    }
}
