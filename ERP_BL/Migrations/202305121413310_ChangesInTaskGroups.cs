namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesInTaskGroups : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TaskGroups", "targetGroup_Id", c => c.Int());
            CreateIndex("dbo.TaskGroups", "targetGroup_Id");
            AddForeignKey("dbo.TaskGroups", "targetGroup_Id", "dbo.TargetGroups", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TaskGroups", "targetGroup_Id", "dbo.TargetGroups");
            DropIndex("dbo.TaskGroups", new[] { "targetGroup_Id" });
            DropColumn("dbo.TaskGroups", "targetGroup_Id");
        }
    }
}
