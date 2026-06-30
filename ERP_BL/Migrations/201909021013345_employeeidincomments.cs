namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class employeeidincomments : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CommentLogs", "UserId", "dbo.Users");
            DropIndex("dbo.CommentLogs", new[] { "UserId" });
            AddColumn("dbo.CommentLogs", "employeeId", c => c.Int());
            CreateIndex("dbo.CommentLogs", "employeeId");
            AddForeignKey("dbo.CommentLogs", "employeeId", "dbo.Employees", "EmpId");
            DropColumn("dbo.CommentLogs", "UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CommentLogs", "UserId", c => c.Int(nullable: false));
            DropForeignKey("dbo.CommentLogs", "employeeId", "dbo.Employees");
            DropIndex("dbo.CommentLogs", new[] { "employeeId" });
            DropColumn("dbo.CommentLogs", "employeeId");
            CreateIndex("dbo.CommentLogs", "UserId");
            AddForeignKey("dbo.CommentLogs", "UserId", "dbo.Users", "id", cascadeDelete: true);
        }
    }
}
