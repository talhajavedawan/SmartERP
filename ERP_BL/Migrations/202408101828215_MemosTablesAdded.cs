namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MemosTablesAdded : DbMigration
    {
        public override void Up()
        {
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.GroupCreatorId)
                .ForeignKey("dbo.MemoGroups", t => t.parentId)
                .Index(t => t.parentId)
                .Index(t => t.GroupCreatorId);
            
            CreateTable(
                "dbo.Memos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Topic = c.String(),
                        CreationDate = c.DateTime(),
                        createdById = c.Int(),
                        createdForId = c.Int(),
                        memoGroupId = c.Int(),
                        isGroupMemo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.createdById)
                .ForeignKey("dbo.MemoGroups", t => t.memoGroupId)
                .ForeignKey("dbo.Users", t => t.createdForId)
                .Index(t => t.createdById)
                .Index(t => t.createdForId)
                .Index(t => t.memoGroupId);
            
            CreateTable(
                "dbo.MemoGroupUsers",
                c => new
                    {
                        MemoGroup_Id = c.Int(nullable: false),
                        User_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.MemoGroup_Id, t.User_id })
                .ForeignKey("dbo.MemoGroups", t => t.MemoGroup_Id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_id, cascadeDelete: true)
                .Index(t => t.MemoGroup_Id)
                .Index(t => t.User_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Memos", "createdForId", "dbo.Users");
            DropForeignKey("dbo.MemoGroupUsers", "User_id", "dbo.Users");
            DropForeignKey("dbo.MemoGroupUsers", "MemoGroup_Id", "dbo.MemoGroups");
            DropForeignKey("dbo.MemoGroups", "parentId", "dbo.MemoGroups");
            DropForeignKey("dbo.Memos", "memoGroupId", "dbo.MemoGroups");
            DropForeignKey("dbo.Memos", "createdById", "dbo.Users");
            DropForeignKey("dbo.MemoGroups", "GroupCreatorId", "dbo.Users");
            DropIndex("dbo.MemoGroupUsers", new[] { "User_id" });
            DropIndex("dbo.MemoGroupUsers", new[] { "MemoGroup_Id" });
            DropIndex("dbo.Memos", new[] { "memoGroupId" });
            DropIndex("dbo.Memos", new[] { "createdForId" });
            DropIndex("dbo.Memos", new[] { "createdById" });
            DropIndex("dbo.MemoGroups", new[] { "GroupCreatorId" });
            DropIndex("dbo.MemoGroups", new[] { "parentId" });
            DropTable("dbo.MemoGroupUsers");
            DropTable("dbo.Memos");
            DropTable("dbo.MemoGroups");
        }
    }
}
