namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BMgrossProfitSEAddedInTodoTasks : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ToDoTasks", "BMgrossProfitSE", c => c.Double(nullable: false));
            AddColumn("dbo.ToDoTasks", "BMgrossProfitME", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ToDoTasks", "BMgrossProfitME");
            DropColumn("dbo.ToDoTasks", "BMgrossProfitSE");
        }
    }
}
