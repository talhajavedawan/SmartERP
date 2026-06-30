namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeInCommentLogAndNotification : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CommentLogs", "FlagId", c => c.Int());
            AddColumn("dbo.Notifications", "commentLogId", c => c.Int());
            CreateIndex("dbo.CommentLogs", "FlagId");
            AddForeignKey("dbo.CommentLogs", "FlagId", "dbo.NotificationFlags", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CommentLogs", "FlagId", "dbo.NotificationFlags");
            DropIndex("dbo.CommentLogs", new[] { "FlagId" });
            DropColumn("dbo.Notifications", "commentLogId");
            DropColumn("dbo.CommentLogs", "FlagId");
        }
    }
}
