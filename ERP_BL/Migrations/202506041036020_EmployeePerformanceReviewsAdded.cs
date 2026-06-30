namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EmployeePerformanceReviewsAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EmployeePerformanceReviews",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreationDate = c.DateTime(),
                        PerformanceYear = c.DateTime(),
                        EmployeeId = c.Int(),
                        DeptId = c.Int(),
                        SupervisorId = c.Int(),
                        SupervisorReviewDate = c.DateTime(),
                        AdminId = c.Int(),
                        ManagementId = c.Int(),
                        AdminReviewDate = c.DateTime(),
                        MemoId = c.Int(),
                        PreviousYearGoals = c.String(),
                        NextYearGoals = c.String(),
                        performanceReviewerStage = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.AdminId)
                .ForeignKey("dbo.tabDepartment", t => t.DeptId)
                .ForeignKey("dbo.Users", t => t.EmployeeId)
                .ForeignKey("dbo.Users", t => t.ManagementId)
                .ForeignKey("dbo.Memos", t => t.MemoId)
                .ForeignKey("dbo.Users", t => t.SupervisorId)
                .Index(t => t.EmployeeId)
                .Index(t => t.DeptId)
                .Index(t => t.SupervisorId)
                .Index(t => t.AdminId)
                .Index(t => t.ManagementId)
                .Index(t => t.MemoId);
            
            CreateTable(
                "dbo.PerformanceIndicatorRatings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ReviewId = c.Int(nullable: false),
                        IndicatorDefinitionId = c.Int(nullable: false),
                        SelfRating = c.Int(nullable: false),
                        SupervisorRating = c.Int(nullable: false),
                        AdminRating = c.Int(nullable: false),
                        ManagementRating = c.Int(nullable: false),
                        Comment = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PerformanceIndicatorDefinitions", t => t.IndicatorDefinitionId, cascadeDelete: true)
                .ForeignKey("dbo.EmployeePerformanceReviews", t => t.ReviewId, cascadeDelete: true)
                .Index(t => t.ReviewId)
                .Index(t => t.IndicatorDefinitionId);
            
            CreateTable(
                "dbo.PerformanceIndicatorDefinitions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        DisplayOrder = c.Int(nullable: false),
                        Weightage = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EmployeePerformanceReviews", "SupervisorId", "dbo.Users");
            DropForeignKey("dbo.PerformanceIndicatorRatings", "ReviewId", "dbo.EmployeePerformanceReviews");
            DropForeignKey("dbo.PerformanceIndicatorRatings", "IndicatorDefinitionId", "dbo.PerformanceIndicatorDefinitions");
            DropForeignKey("dbo.EmployeePerformanceReviews", "MemoId", "dbo.Memos");
            DropForeignKey("dbo.EmployeePerformanceReviews", "ManagementId", "dbo.Users");
            DropForeignKey("dbo.EmployeePerformanceReviews", "EmployeeId", "dbo.Users");
            DropForeignKey("dbo.EmployeePerformanceReviews", "DeptId", "dbo.tabDepartment");
            DropForeignKey("dbo.EmployeePerformanceReviews", "AdminId", "dbo.Users");
            DropIndex("dbo.PerformanceIndicatorRatings", new[] { "IndicatorDefinitionId" });
            DropIndex("dbo.PerformanceIndicatorRatings", new[] { "ReviewId" });
            DropIndex("dbo.EmployeePerformanceReviews", new[] { "MemoId" });
            DropIndex("dbo.EmployeePerformanceReviews", new[] { "ManagementId" });
            DropIndex("dbo.EmployeePerformanceReviews", new[] { "AdminId" });
            DropIndex("dbo.EmployeePerformanceReviews", new[] { "SupervisorId" });
            DropIndex("dbo.EmployeePerformanceReviews", new[] { "DeptId" });
            DropIndex("dbo.EmployeePerformanceReviews", new[] { "EmployeeId" });
            DropTable("dbo.PerformanceIndicatorDefinitions");
            DropTable("dbo.PerformanceIndicatorRatings");
            DropTable("dbo.EmployeePerformanceReviews");
        }
    }
}
