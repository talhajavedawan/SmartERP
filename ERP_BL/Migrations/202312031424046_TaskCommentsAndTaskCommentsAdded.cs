namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TaskCommentsAndTaskCommentsAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TaskComments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        goodReceiveNoteId = c.Int(),
                        userId = c.Int(),
                        taskId = c.Int(),
                        Comment = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GoodReceiveNotes", t => t.goodReceiveNoteId)
                .ForeignKey("dbo.Tasks", t => t.taskId)
                .ForeignKey("dbo.Users", t => t.userId)
                .Index(t => t.goodReceiveNoteId)
                .Index(t => t.userId)
                .Index(t => t.taskId);
            
            CreateTable(
                "dbo.GoodReceiveNotes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TaskComments", "userId", "dbo.Users");
            DropForeignKey("dbo.TaskComments", "taskId", "dbo.Tasks");
            DropForeignKey("dbo.TaskComments", "goodReceiveNoteId", "dbo.GoodReceiveNotes");
            DropIndex("dbo.TaskComments", new[] { "taskId" });
            DropIndex("dbo.TaskComments", new[] { "userId" });
            DropIndex("dbo.TaskComments", new[] { "goodReceiveNoteId" });
            DropTable("dbo.GoodReceiveNotes");
            DropTable("dbo.TaskComments");
        }
    }
}
