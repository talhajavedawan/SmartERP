namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TasksUsersAdded : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Users", "Tasks_Id", "dbo.Tasks");
            DropIndex("dbo.Users", new[] { "Tasks_Id" });
            CreateTable(
                "dbo.TasksUsers",
                c => new
                    {
                        Tasks_Id = c.Int(nullable: false),
                        User_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Tasks_Id, t.User_id })
                .ForeignKey("dbo.Tasks", t => t.Tasks_Id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_id, cascadeDelete: true)
                .Index(t => t.Tasks_Id)
                .Index(t => t.User_id);
            
            DropColumn("dbo.Users", "Tasks_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "Tasks_Id", c => c.Int());
            DropForeignKey("dbo.TasksUsers", "User_id", "dbo.Users");
            DropForeignKey("dbo.TasksUsers", "Tasks_Id", "dbo.Tasks");
            DropIndex("dbo.TasksUsers", new[] { "User_id" });
            DropIndex("dbo.TasksUsers", new[] { "Tasks_Id" });
            DropTable("dbo.TasksUsers");
            CreateIndex("dbo.Users", "Tasks_Id");
            AddForeignKey("dbo.Users", "Tasks_Id", "dbo.Tasks", "Id");
        }
    }
}
