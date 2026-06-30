namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MemoUsersAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MemoUsers",
                c => new
                    {
                        Memo_Id = c.Int(nullable: false),
                        User_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Memo_Id, t.User_id })
                .ForeignKey("dbo.Memos", t => t.Memo_Id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_id, cascadeDelete: true)
                .Index(t => t.Memo_Id)
                .Index(t => t.User_id);
            
            AddColumn("dbo.Memos", "memoType", c => c.Int(nullable: false));
            AddColumn("dbo.Memos", "Subject", c => c.String());
            AddColumn("dbo.Memos", "isVoid", c => c.Boolean(nullable: false));
            AddColumn("dbo.AttachmentCategories", "Memo", c => c.Int());
            DropColumn("dbo.Memos", "Topic");
            DropColumn("dbo.Memos", "isGroupMemo");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Memos", "isGroupMemo", c => c.Boolean(nullable: false));
            AddColumn("dbo.Memos", "Topic", c => c.String());
            DropForeignKey("dbo.MemoUsers", "User_id", "dbo.Users");
            DropForeignKey("dbo.MemoUsers", "Memo_Id", "dbo.Memos");
            DropIndex("dbo.MemoUsers", new[] { "User_id" });
            DropIndex("dbo.MemoUsers", new[] { "Memo_Id" });
            DropColumn("dbo.AttachmentCategories", "Memo");
            DropColumn("dbo.Memos", "isVoid");
            DropColumn("dbo.Memos", "Subject");
            DropColumn("dbo.Memos", "memoType");
            DropTable("dbo.MemoUsers");
        }
    }
}
