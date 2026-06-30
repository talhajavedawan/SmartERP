namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TasksStatusTaskTypesAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TasksStatusTaskTypes",
                c => new
                    {
                        TasksStatus_Id = c.Int(nullable: false),
                        TaskType_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.TasksStatus_Id, t.TaskType_Id })
                .ForeignKey("dbo.TasksStatus", t => t.TasksStatus_Id, cascadeDelete: true)
                .ForeignKey("dbo.TaskTypes", t => t.TaskType_Id, cascadeDelete: true)
                .Index(t => t.TasksStatus_Id)
                .Index(t => t.TaskType_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TasksStatusTaskTypes", "TaskType_Id", "dbo.TaskTypes");
            DropForeignKey("dbo.TasksStatusTaskTypes", "TasksStatus_Id", "dbo.TasksStatus");
            DropIndex("dbo.TasksStatusTaskTypes", new[] { "TaskType_Id" });
            DropIndex("dbo.TasksStatusTaskTypes", new[] { "TasksStatus_Id" });
            DropTable("dbo.TasksStatusTaskTypes");
        }
    }
}
