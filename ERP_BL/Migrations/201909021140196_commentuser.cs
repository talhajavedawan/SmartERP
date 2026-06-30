namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class commentuser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CommentLogs", "User_id", c => c.Int());
            CreateIndex("dbo.CommentLogs", "User_id");
            AddForeignKey("dbo.CommentLogs", "User_id", "dbo.Users", "id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CommentLogs", "User_id", "dbo.Users");
            DropIndex("dbo.CommentLogs", new[] { "User_id" });
            DropColumn("dbo.CommentLogs", "User_id");
        }
    }
}
