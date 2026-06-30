namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CommentLogUser2Added : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CommentLogUser2",
                c => new
                    {
                        CommentLog_Id = c.Int(nullable: false),
                        User_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CommentLog_Id, t.User_id })
                .ForeignKey("dbo.CommentLogs", t => t.CommentLog_Id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_id, cascadeDelete: true)
                .Index(t => t.CommentLog_Id)
                .Index(t => t.User_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CommentLogUser2", "User_id", "dbo.Users");
            DropForeignKey("dbo.CommentLogUser2", "CommentLog_Id", "dbo.CommentLogs");
            DropIndex("dbo.CommentLogUser2", new[] { "User_id" });
            DropIndex("dbo.CommentLogUser2", new[] { "CommentLog_Id" });
            DropTable("dbo.CommentLogUser2");
        }
    }
}
