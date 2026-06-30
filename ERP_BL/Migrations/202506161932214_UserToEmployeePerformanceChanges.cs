namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserToEmployeePerformanceChanges : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "EmployeePerformanceReview_Id", c => c.Int());
            CreateIndex("dbo.Users", "EmployeePerformanceReview_Id");
            AddForeignKey("dbo.Users", "EmployeePerformanceReview_Id", "dbo.EmployeePerformanceReviews", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "EmployeePerformanceReview_Id", "dbo.EmployeePerformanceReviews");
            DropIndex("dbo.Users", new[] { "EmployeePerformanceReview_Id" });
            DropColumn("dbo.Users", "EmployeePerformanceReview_Id");
        }
    }
}
