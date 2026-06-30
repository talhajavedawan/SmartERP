namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PollsAndPollUserAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Polls",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreationDate = c.DateTime(),
                        pollingType = c.Int(nullable: false),
                        Title = c.String(),
                        taskGroupId = c.Int(),
                        initiatedById = c.Int(),
                        ValidUntil = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.initiatedById)
                .ForeignKey("dbo.TaskGroups", t => t.taskGroupId)
                .Index(t => t.taskGroupId)
                .Index(t => t.initiatedById);
            
            CreateTable(
                "dbo.PollUsers",
                c => new
                    {
                        Poll_Id = c.Int(nullable: false),
                        User_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Poll_Id, t.User_id })
                .ForeignKey("dbo.Polls", t => t.Poll_Id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_id, cascadeDelete: true)
                .Index(t => t.Poll_Id)
                .Index(t => t.User_id);
            
            CreateTable(
                "dbo.PollUser1",
                c => new
                    {
                        Poll_Id = c.Int(nullable: false),
                        User_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Poll_Id, t.User_id })
                .ForeignKey("dbo.Polls", t => t.Poll_Id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_id, cascadeDelete: true)
                .Index(t => t.Poll_Id)
                .Index(t => t.User_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PollUser1", "User_id", "dbo.Users");
            DropForeignKey("dbo.PollUser1", "Poll_Id", "dbo.Polls");
            DropForeignKey("dbo.PollUsers", "User_id", "dbo.Users");
            DropForeignKey("dbo.PollUsers", "Poll_Id", "dbo.Polls");
            DropForeignKey("dbo.Polls", "taskGroupId", "dbo.TaskGroups");
            DropForeignKey("dbo.Polls", "initiatedById", "dbo.Users");
            DropIndex("dbo.PollUser1", new[] { "User_id" });
            DropIndex("dbo.PollUser1", new[] { "Poll_Id" });
            DropIndex("dbo.PollUsers", new[] { "User_id" });
            DropIndex("dbo.PollUsers", new[] { "Poll_Id" });
            DropIndex("dbo.Polls", new[] { "initiatedById" });
            DropIndex("dbo.Polls", new[] { "taskGroupId" });
            DropTable("dbo.PollUser1");
            DropTable("dbo.PollUsers");
            DropTable("dbo.Polls");
        }
    }
}
