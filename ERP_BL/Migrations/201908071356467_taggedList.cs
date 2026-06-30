namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class taggedList : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "CommentLog_Id", c => c.Int());
            CreateIndex("dbo.Users", "CommentLog_Id");
            AddForeignKey("dbo.Users", "CommentLog_Id", "dbo.CommentLogs", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "CommentLog_Id", "dbo.CommentLogs");
            DropIndex("dbo.Users", new[] { "CommentLog_Id" });
            DropColumn("dbo.Users", "CommentLog_Id");
        }
    }
}
