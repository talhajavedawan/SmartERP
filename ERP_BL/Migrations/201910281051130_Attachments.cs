namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Attachments : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.UserCommentLogs", newName: "CommentLogUsers");
            DropPrimaryKey("dbo.CommentLogUsers");
            CreateTable(
                "dbo.AttachmentCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ParentId = c.Int(),
                        lastChangedDate = c.DateTime(nullable: false),
                        additionDate = c.DateTime(nullable: false),
                        description = c.String(),
                        thumbnail = c.Binary(),
                        isActive = c.Boolean(nullable: false),
                        userId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AttachmentCategories", t => t.ParentId)
                .ForeignKey("dbo.Users", t => t.userId, cascadeDelete: true)
                .Index(t => t.ParentId)
                .Index(t => t.userId);
            
            CreateTable(
                "dbo.Attachments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        fileServerAdress = c.String(),
                        fileLocalAdress = c.String(),
                        fileName = c.String(),
                        fileType = c.Int(nullable: false),
                        lastOpendate = c.DateTime(nullable: false),
                        additionDate = c.DateTime(nullable: false),
                        comment = c.String(),
                        currentStatus = c.Int(nullable: false),
                        thumbnail = c.Binary(),
                        isactive = c.Boolean(nullable: false),
                        transactionType = c.Int(nullable: false),
                        transactionId = c.Int(nullable: false),
                        userId = c.Int(nullable: false),
                        categoryId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AttachmentCategories", t => t.categoryId)
                .ForeignKey("dbo.Users", t => t.userId, cascadeDelete: true)
                .Index(t => t.userId)
                .Index(t => t.categoryId);
            
            AddPrimaryKey("dbo.CommentLogUsers", new[] { "CommentLog_Id", "User_id" });
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AttachmentCategories", "userId", "dbo.Users");
            DropForeignKey("dbo.AttachmentCategories", "ParentId", "dbo.AttachmentCategories");
            DropForeignKey("dbo.Attachments", "userId", "dbo.Users");
            DropForeignKey("dbo.Attachments", "categoryId", "dbo.AttachmentCategories");
            DropIndex("dbo.Attachments", new[] { "categoryId" });
            DropIndex("dbo.Attachments", new[] { "userId" });
            DropIndex("dbo.AttachmentCategories", new[] { "userId" });
            DropIndex("dbo.AttachmentCategories", new[] { "ParentId" });
            DropPrimaryKey("dbo.CommentLogUsers");
            DropTable("dbo.Attachments");
            DropTable("dbo.AttachmentCategories");
            AddPrimaryKey("dbo.CommentLogUsers", new[] { "User_id", "CommentLog_Id" });
            RenameTable(name: "dbo.CommentLogUsers", newName: "UserCommentLogs");
        }
    }
}
