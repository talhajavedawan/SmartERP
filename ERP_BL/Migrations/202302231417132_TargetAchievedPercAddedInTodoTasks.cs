namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TargetAchievedPercAddedInTodoTasks : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ToDoTasks", "TargetAchievedPercenatage", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ToDoTasks", "TargetAchievedPercenatage");
        }
    }
}
