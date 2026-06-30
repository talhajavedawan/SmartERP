namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class gridReportClassAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
              "dbo.GridReports",
              c => new
              {
                  Id = c.Int(nullable: false, identity: true),
                  userId = c.Int(nullable: false),
                  settingkey = c.String(nullable: false, maxLength: 128, unicode: false),
                  groupId = c.Int(),
                  settingValue = c.String(unicode: false),
                  lastModified = c.DateTime(nullable: false),
                  gridReportType = c.Int(nullable: false),
                  reportName = c.String(),
              })
              .PrimaryKey(t => new { t.Id, t.userId, t.settingkey })
              .ForeignKey("dbo.Users", t => t.userId, cascadeDelete: true)
              //.ForeignKey("dbo.GridReportGroup", t => t.groupId)
              .Index(t => t.userId)
              /*.Index(t => t.group_Id)*/;
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tabDepartment", new[] { "GridReport_Id", "GridReport_userId", "GridReport_settingkey" }, "dbo.GridReports");
            DropForeignKey("dbo.GridReports", "userId", "dbo.Users");
            DropIndex("dbo.GridReports", new[] { "groupId" });
            DropIndex("dbo.GridReports", new[] { "userId" });
            DropTable("dbo.GridReports");
        }
    }
}
