namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TargetGroupsAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TargetGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GroupName = c.String(),
                        parentId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TargetGroups", t => t.parentId)
                .Index(t => t.parentId);
            
            AddColumn("dbo.ToDoTasks", "targetGroupId", c => c.Int());
            CreateIndex("dbo.ToDoTasks", "targetGroupId");
            AddForeignKey("dbo.ToDoTasks", "targetGroupId", "dbo.TargetGroups", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ToDoTasks", "targetGroupId", "dbo.TargetGroups");
            DropForeignKey("dbo.TargetGroups", "parentId", "dbo.TargetGroups");
            DropIndex("dbo.TargetGroups", new[] { "parentId" });
            DropIndex("dbo.ToDoTasks", new[] { "targetGroupId" });
            DropColumn("dbo.ToDoTasks", "targetGroupId");
            DropTable("dbo.TargetGroups");
        }
    }
}
