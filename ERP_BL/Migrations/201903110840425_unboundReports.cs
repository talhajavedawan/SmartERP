namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class unboundReports : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UnBoundReports",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        userId = c.Int(nullable: false),
                        reportName = c.String(nullable: false, maxLength: 128, unicode: false),
                        template = c.String(unicode: false),
                        reportType = c.Int(nullable: false),
                        lastModified = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.Id, t.userId, t.reportName })
                .ForeignKey("dbo.Users", t => t.userId, cascadeDelete: true)
                .Index(t => t.userId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UnBoundReports", "userId", "dbo.Users");
            DropIndex("dbo.UnBoundReports", new[] { "userId" });
            DropTable("dbo.UnBoundReports");
        }
    }
}
