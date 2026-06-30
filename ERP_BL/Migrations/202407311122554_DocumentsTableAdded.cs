namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DocumentsTableAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DocumentStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.String(),
                        isApproved = c.Boolean(nullable: false),
                        isActive = c.Boolean(),
                        backcolor = c.String(),
                        forecolor = c.String(),
                        HierarchicalIndex = c.Int(nullable: false),
                        isDisable = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Documents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        isVoid = c.Boolean(nullable: false),
                        isReviewed = c.Boolean(),
                        needReview = c.Boolean(),
                        PendingForClosing = c.Boolean(),
                        PendingForReApproval = c.Boolean(),
                        isApproved = c.Boolean(),
                        ApprovedDate = c.DateTime(),
                        ReApprovalDate = c.DateTime(),
                        isReApproved = c.Boolean(),
                        dept_Id = c.Int(nullable: false),
                        company_Id = c.Int(),
                        user_Id = c.Int(),
                        statusClass_Id = c.Int(),
                        allocation_Id = c.Int(nullable: false),
                        DocumentStatus_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DocumentStatus", t => t.DocumentStatus_Id)
                .ForeignKey("dbo.Employees", t => t.allocation_Id, cascadeDelete: true)
                .ForeignKey("dbo.StatusClasses", t => t.statusClass_Id)
                .ForeignKey("dbo.Users", t => t.user_Id)
                .ForeignKey("dbo.tabDepartment", t => t.dept_Id, cascadeDelete: true)
                .ForeignKey("dbo.tabCompany", t => t.company_Id)
                .Index(t => t.dept_Id)
                .Index(t => t.company_Id)
                .Index(t => t.user_Id)
                .Index(t => t.statusClass_Id)
                .Index(t => t.allocation_Id)
                .Index(t => t.DocumentStatus_Id);
            
            CreateTable(
                "dbo.StatusClassDocumentStatus",
                c => new
                    {
                        StatusClass_Id = c.Int(nullable: false),
                        DocumentStatus_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.StatusClass_Id, t.DocumentStatus_Id })
                .ForeignKey("dbo.StatusClasses", t => t.StatusClass_Id, cascadeDelete: true)
                .ForeignKey("dbo.DocumentStatus", t => t.DocumentStatus_Id, cascadeDelete: true)
                .Index(t => t.StatusClass_Id)
                .Index(t => t.DocumentStatus_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Documents", "company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.Documents", "dept_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.StatusClassDocumentStatus", "DocumentStatus_Id", "dbo.DocumentStatus");
            DropForeignKey("dbo.StatusClassDocumentStatus", "StatusClass_Id", "dbo.StatusClasses");
            DropForeignKey("dbo.Documents", "user_Id", "dbo.Users");
            DropForeignKey("dbo.Documents", "statusClass_Id", "dbo.StatusClasses");
            DropForeignKey("dbo.Documents", "allocation_Id", "dbo.Employees");
            DropForeignKey("dbo.Documents", "DocumentStatus_Id", "dbo.DocumentStatus");
            DropIndex("dbo.StatusClassDocumentStatus", new[] { "DocumentStatus_Id" });
            DropIndex("dbo.StatusClassDocumentStatus", new[] { "StatusClass_Id" });
            DropIndex("dbo.Documents", new[] { "DocumentStatus_Id" });
            DropIndex("dbo.Documents", new[] { "allocation_Id" });
            DropIndex("dbo.Documents", new[] { "statusClass_Id" });
            DropIndex("dbo.Documents", new[] { "user_Id" });
            DropIndex("dbo.Documents", new[] { "company_Id" });
            DropIndex("dbo.Documents", new[] { "dept_Id" });
            DropTable("dbo.StatusClassDocumentStatus");
            DropTable("dbo.Documents");
            DropTable("dbo.DocumentStatus");
        }
    }
}
