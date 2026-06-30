namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changesAgainInToDoTasks : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ToDoTasks", "supervisedById", c => c.Int());
            AddColumn("dbo.ToDoTasks", "salesHeadId", c => c.Int());
            CreateIndex("dbo.ToDoTasks", "supervisedById");
            CreateIndex("dbo.ToDoTasks", "salesHeadId");
            AddForeignKey("dbo.ToDoTasks", "salesHeadId", "dbo.Users", "id");
            AddForeignKey("dbo.ToDoTasks", "supervisedById", "dbo.Users", "id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ToDoTasks", "supervisedById", "dbo.Users");
            DropForeignKey("dbo.ToDoTasks", "salesHeadId", "dbo.Users");
            DropIndex("dbo.ToDoTasks", new[] { "salesHeadId" });
            DropIndex("dbo.ToDoTasks", new[] { "supervisedById" });
            DropColumn("dbo.ToDoTasks", "salesHeadId");
            DropColumn("dbo.ToDoTasks", "supervisedById");
        }
    }
}
