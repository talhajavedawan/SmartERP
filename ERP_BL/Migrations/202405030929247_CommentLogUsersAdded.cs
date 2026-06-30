namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CommentLogUsersAdded : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.CommentLogUser2", newName: "CommentLogUser3");
            RenameTable(name: "dbo.CommentLogUser1", newName: "CommentLogUser2");
            RenameTable(name: "dbo.CommentLogUsers", newName: "CommentLogUser1");
            CreateTable(
                "dbo.CommentLogUsers",
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
            DropForeignKey("dbo.CommentLogUsers", "User_id", "dbo.Users");
            DropForeignKey("dbo.CommentLogUsers", "CommentLog_Id", "dbo.CommentLogs");
            DropIndex("dbo.CommentLogUsers", new[] { "User_id" });
            DropIndex("dbo.CommentLogUsers", new[] { "CommentLog_Id" });
            DropTable("dbo.CommentLogUsers");
            RenameTable(name: "dbo.CommentLogUser1", newName: "CommentLogUsers");
            RenameTable(name: "dbo.CommentLogUser2", newName: "CommentLogUser1");
            RenameTable(name: "dbo.CommentLogUser3", newName: "CommentLogUser2");
        }
    }
}
