namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TaskToUserRelationAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Tasks_Id", c => c.Int());
            CreateIndex("dbo.Users", "Tasks_Id");
            AddForeignKey("dbo.Users", "Tasks_Id", "dbo.Tasks", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "Tasks_Id", "dbo.Tasks");
            DropIndex("dbo.Users", new[] { "Tasks_Id" });
            DropColumn("dbo.Users", "Tasks_Id");
        }
    }
}
