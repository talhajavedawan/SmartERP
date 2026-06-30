namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesInCommentLogs : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CommentLogs", "managerId", c => c.Int());
            AddColumn("dbo.CommentLogs", "salesPersonId", c => c.Int());
            AddColumn("dbo.CommentLogs", "financePersonId", c => c.Int());
            CreateIndex("dbo.CommentLogs", "managerId");
            CreateIndex("dbo.CommentLogs", "salesPersonId");
            CreateIndex("dbo.CommentLogs", "financePersonId");
            AddForeignKey("dbo.CommentLogs", "financePersonId", "dbo.Employees", "EmpId");
            AddForeignKey("dbo.CommentLogs", "managerId", "dbo.Employees", "EmpId");
            AddForeignKey("dbo.CommentLogs", "salesPersonId", "dbo.Employees", "EmpId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CommentLogs", "salesPersonId", "dbo.Employees");
            DropForeignKey("dbo.CommentLogs", "managerId", "dbo.Employees");
            DropForeignKey("dbo.CommentLogs", "financePersonId", "dbo.Employees");
            DropIndex("dbo.CommentLogs", new[] { "financePersonId" });
            DropIndex("dbo.CommentLogs", new[] { "salesPersonId" });
            DropIndex("dbo.CommentLogs", new[] { "managerId" });
            DropColumn("dbo.CommentLogs", "financePersonId");
            DropColumn("dbo.CommentLogs", "salesPersonId");
            DropColumn("dbo.CommentLogs", "managerId");
        }
    }
}
