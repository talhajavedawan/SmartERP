namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class forTaskFieldAddedInToDoTaskStatus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ToDoTaskStatus", "forTask", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ToDoTaskStatus", "forTask");
        }
    }
}
