namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changesinPayementsNotificationsandCommentLog : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Payments", "dept_Id", "dbo.tabDepartment");
            DropIndex("dbo.Payments", new[] { "dept_Id" });
            CreateTable(
                "dbo.DepartmentPayments",
                c => new
                    {
                        Department_Id = c.Int(nullable: false),
                        Payment_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Department_Id, t.Payment_Id })
                .ForeignKey("dbo.tabDepartment", t => t.Department_Id, cascadeDelete: true)
                .ForeignKey("dbo.Payments", t => t.Payment_Id, cascadeDelete: true)
                .Index(t => t.Department_Id)
                .Index(t => t.Payment_Id);
            
            AddColumn("dbo.CommentLogs", "billSystemRef", c => c.String());
            AddColumn("dbo.CommentLogs", "poSystemRef", c => c.String());
            AddColumn("dbo.Notifications", "BillReferenceNo", c => c.String());
            AddColumn("dbo.Notifications", "PoReferenceNo", c => c.String());
            DropColumn("dbo.Payments", "dept_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Payments", "dept_Id", c => c.Int());
            DropForeignKey("dbo.DepartmentPayments", "Payment_Id", "dbo.Payments");
            DropForeignKey("dbo.DepartmentPayments", "Department_Id", "dbo.tabDepartment");
            DropIndex("dbo.DepartmentPayments", new[] { "Payment_Id" });
            DropIndex("dbo.DepartmentPayments", new[] { "Department_Id" });
            DropColumn("dbo.Notifications", "PoReferenceNo");
            DropColumn("dbo.Notifications", "BillReferenceNo");
            DropColumn("dbo.CommentLogs", "poSystemRef");
            DropColumn("dbo.CommentLogs", "billSystemRef");
            DropTable("dbo.DepartmentPayments");
            CreateIndex("dbo.Payments", "dept_Id");
            AddForeignKey("dbo.Payments", "dept_Id", "dbo.tabDepartment", "Id");
        }
    }
}
