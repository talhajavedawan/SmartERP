namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MemoGroupUsersAdded : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.MemoGroups", "GroupCreatorId", "dbo.Users");
            DropForeignKey("dbo.Memos", "memoGroupId", "dbo.MemoGroups");
            DropForeignKey("dbo.MemoGroups", "parentId", "dbo.MemoGroups");
            DropForeignKey("dbo.MemoGroupUsers", "MemoGroup_Id", "dbo.MemoGroups");
            DropForeignKey("dbo.MemoGroupUsers", "User_id", "dbo.Users");
            DropIndex("dbo.Memos", new[] { "memoGroupId" });
            DropIndex("dbo.MemoGroups", new[] { "parentId" });
            DropIndex("dbo.MemoGroups", new[] { "GroupCreatorId" });
            DropIndex("dbo.MemoGroupUsers", new[] { "MemoGroup_Id" });
            DropIndex("dbo.MemoGroupUsers", new[] { "User_id" });
            AddColumn("dbo.Memos", "taskGroupId", c => c.Int());
            CreateIndex("dbo.Memos", "taskGroupId");
            AddForeignKey("dbo.Memos", "taskGroupId", "dbo.TaskGroups", "Id");
            DropColumn("dbo.Memos", "memoGroupId");
            DropTable("dbo.MemoGroups");
            DropTable("dbo.MemoGroupUsers");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.MemoGroupUsers",
                c => new
                    {
                        MemoGroup_Id = c.Int(nullable: false),
                        User_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.MemoGroup_Id, t.User_id });
            
            CreateTable(
                "dbo.MemoGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GroupName = c.String(),
                        parentId = c.Int(),
                        CreationDate = c.DateTime(),
                        GroupCreatorId = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Memos", "memoGroupId", c => c.Int());
            DropForeignKey("dbo.Memos", "taskGroupId", "dbo.TaskGroups");
            DropIndex("dbo.Memos", new[] { "taskGroupId" });
            DropColumn("dbo.Memos", "taskGroupId");
            CreateIndex("dbo.MemoGroupUsers", "User_id");
            CreateIndex("dbo.MemoGroupUsers", "MemoGroup_Id");
            CreateIndex("dbo.MemoGroups", "GroupCreatorId");
            CreateIndex("dbo.MemoGroups", "parentId");
            CreateIndex("dbo.Memos", "memoGroupId");
            AddForeignKey("dbo.MemoGroupUsers", "User_id", "dbo.Users", "id", cascadeDelete: true);
            AddForeignKey("dbo.MemoGroupUsers", "MemoGroup_Id", "dbo.MemoGroups", "Id", cascadeDelete: true);
            AddForeignKey("dbo.MemoGroups", "parentId", "dbo.MemoGroups", "Id");
            AddForeignKey("dbo.Memos", "memoGroupId", "dbo.MemoGroups", "Id");
            AddForeignKey("dbo.MemoGroups", "GroupCreatorId", "dbo.Users", "id");
        }
    }
}
