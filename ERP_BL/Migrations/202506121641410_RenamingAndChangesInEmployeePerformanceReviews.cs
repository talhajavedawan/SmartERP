namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenamingAndChangesInEmployeePerformanceReviews : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.EmployeePerformanceReviews", name: "SupervisorId", newName: "SupervisorLevelOneId");
            RenameIndex(table: "dbo.EmployeePerformanceReviews", name: "IX_SupervisorId", newName: "IX_SupervisorLevelOneId");
            AddColumn("dbo.EmployeePerformanceReviews", "SupervisorLevelOneReviewDate", c => c.DateTime());
            AddColumn("dbo.EmployeePerformanceReviews", "SupervisorLevelTwoId", c => c.Int());
            AddColumn("dbo.EmployeePerformanceReviews", "SupervisorLevelTwoReviewDate", c => c.DateTime());
            AddColumn("dbo.EmployeePerformanceReviews", "performanceReviewType", c => c.Int(nullable: false));
            AddColumn("dbo.PerformanceIndicatorRatings", "SupervisorLevelOneRating", c => c.Int(nullable: false));
            AddColumn("dbo.PerformanceIndicatorRatings", "SupervisorLevelTwoRating", c => c.Int(nullable: false));
            AddColumn("dbo.PerformanceIndicatorDefinitions", "IsAdminType", c => c.Boolean(nullable: false));
            CreateIndex("dbo.EmployeePerformanceReviews", "SupervisorLevelTwoId");
            AddForeignKey("dbo.EmployeePerformanceReviews", "SupervisorLevelTwoId", "dbo.Users", "id");
            DropColumn("dbo.EmployeePerformanceReviews", "SupervisorReviewDate");
            DropColumn("dbo.PerformanceIndicatorRatings", "SupervisorRating");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PerformanceIndicatorRatings", "SupervisorRating", c => c.Int(nullable: false));
            AddColumn("dbo.EmployeePerformanceReviews", "SupervisorReviewDate", c => c.DateTime());
            DropForeignKey("dbo.EmployeePerformanceReviews", "SupervisorLevelTwoId", "dbo.Users");
            DropIndex("dbo.EmployeePerformanceReviews", new[] { "SupervisorLevelTwoId" });
            DropColumn("dbo.PerformanceIndicatorDefinitions", "IsAdminType");
            DropColumn("dbo.PerformanceIndicatorRatings", "SupervisorLevelTwoRating");
            DropColumn("dbo.PerformanceIndicatorRatings", "SupervisorLevelOneRating");
            DropColumn("dbo.EmployeePerformanceReviews", "performanceReviewType");
            DropColumn("dbo.EmployeePerformanceReviews", "SupervisorLevelTwoReviewDate");
            DropColumn("dbo.EmployeePerformanceReviews", "SupervisorLevelTwoId");
            DropColumn("dbo.EmployeePerformanceReviews", "SupervisorLevelOneReviewDate");
            RenameIndex(table: "dbo.EmployeePerformanceReviews", name: "IX_SupervisorLevelOneId", newName: "IX_SupervisorId");
            RenameColumn(table: "dbo.EmployeePerformanceReviews", name: "SupervisorLevelOneId", newName: "SupervisorId");
        }
    }
}
