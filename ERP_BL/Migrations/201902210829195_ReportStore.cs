namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReportStore : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Reports",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ReportName = c.String(nullable: false, maxLength: 8000, unicode: false),
                        ReportDesign = c.String(unicode: false),
                        Type = c.String(),
                        lastModified = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.ReportName, unique: true);
            
            CreateTable(
                "dbo.ReportUsers",
                c => new
                    {
                        Report_Id = c.Int(nullable: false),
                        User_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Report_Id, t.User_id })
                .ForeignKey("dbo.Reports", t => t.Report_Id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_id, cascadeDelete: true)
                .Index(t => t.Report_Id)
                .Index(t => t.User_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ReportUsers", "User_id", "dbo.Users");
            DropForeignKey("dbo.ReportUsers", "Report_Id", "dbo.Reports");
            DropIndex("dbo.ReportUsers", new[] { "User_id" });
            DropIndex("dbo.ReportUsers", new[] { "Report_Id" });
            DropIndex("dbo.Reports", new[] { "ReportName" });
            DropTable("dbo.ReportUsers");
            DropTable("dbo.Reports");
        }
    }
}
