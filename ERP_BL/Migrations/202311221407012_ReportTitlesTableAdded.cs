namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReportTitlesTableAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ReportTitles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        titleName = c.String(),
                        isActive = c.Boolean(nullable: false),
                        gridReportType = c.Int(nullable: false),
                        userId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.userId)
                .Index(t => t.userId);
            
            AddColumn("dbo.GridReports", "titleId", c => c.Int());
            CreateIndex("dbo.GridReports", "titleId");
            AddForeignKey("dbo.GridReports", "titleId", "dbo.ReportTitles", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GridReports", "titleId", "dbo.ReportTitles");
            DropForeignKey("dbo.ReportTitles", "userId", "dbo.Users");
            DropIndex("dbo.ReportTitles", new[] { "userId" });
            DropIndex("dbo.GridReports", new[] { "titleId" });
            DropColumn("dbo.GridReports", "titleId");
            DropTable("dbo.ReportTitles");
        }
    }
}
