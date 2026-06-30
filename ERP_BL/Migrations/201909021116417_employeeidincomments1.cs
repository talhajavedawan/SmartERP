namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class employeeidincomments1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Users", "CommentLog_Id", "dbo.CommentLogs");
            DropIndex("dbo.Users", new[] { "CommentLog_Id" });
            CreateTable(
                "dbo.UserCommentLogs",
                c => new
                    {
                        User_id = c.Int(nullable: false),
                        CommentLog_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_id, t.CommentLog_Id })
                .ForeignKey("dbo.Users", t => t.User_id, cascadeDelete: true)
                .ForeignKey("dbo.CommentLogs", t => t.CommentLog_Id, cascadeDelete: true)
                .Index(t => t.User_id)
                .Index(t => t.CommentLog_Id);
            
            DropColumn("dbo.Users", "CommentLog_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "CommentLog_Id", c => c.Int());
            DropForeignKey("dbo.UserCommentLogs", "CommentLog_Id", "dbo.CommentLogs");
            DropForeignKey("dbo.UserCommentLogs", "User_id", "dbo.Users");
            DropIndex("dbo.UserCommentLogs", new[] { "CommentLog_Id" });
            DropIndex("dbo.UserCommentLogs", new[] { "User_id" });
            DropTable("dbo.UserCommentLogs");
            CreateIndex("dbo.Users", "CommentLog_Id");
            AddForeignKey("dbo.Users", "CommentLog_Id", "dbo.CommentLogs", "Id");
        }
    }
}
