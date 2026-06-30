namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class reportGroupIdClassAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GridReportGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        groupName = c.String(),
                        isActive = c.Boolean(nullable: false),
                        gridReportType = c.Int(nullable: false),
                        parentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GridReportGroups", t => t.parentId)
                .Index(t => t.parentId);
            
            AddColumn("dbo.Roles", "GridReportGroup_Id", c => c.Int());
            AddColumn("dbo.tabDepartment", "GridReport_Id", c => c.Int());
            AddColumn("dbo.tabDepartment", "GridReport_userId", c => c.Int());
            AddColumn("dbo.tabDepartment", "GridReport_settingkey", c => c.String(maxLength: 128, unicode: false));
            AddColumn("dbo.GridReports", "group_Id", c => c.Int());
            CreateIndex("dbo.Roles", "GridReportGroup_Id");
            CreateIndex("dbo.tabDepartment", new[] { "GridReport_Id", "GridReport_userId", "GridReport_settingkey" });
            CreateIndex("dbo.GridReports", "group_Id");
            AddForeignKey("dbo.tabDepartment", new[] { "GridReport_Id", "GridReport_userId", "GridReport_settingkey" }, "dbo.GridReports", new[] { "Id", "userId", "settingkey" });
            AddForeignKey("dbo.Roles", "GridReportGroup_Id", "dbo.GridReportGroups", "Id");
            AddForeignKey("dbo.GridReports", "group_Id", "dbo.GridReportGroups", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GridReports", "group_Id", "dbo.GridReportGroups");
            DropForeignKey("dbo.Roles", "GridReportGroup_Id", "dbo.GridReportGroups");
            DropForeignKey("dbo.GridReportGroups", "parentId", "dbo.GridReportGroups");
            DropForeignKey("dbo.tabDepartment", new[] { "GridReport_Id", "GridReport_userId", "GridReport_settingkey" }, "dbo.GridReports");
            DropIndex("dbo.GridReportGroups", new[] { "parentId" });
            DropIndex("dbo.GridReports", new[] { "group_Id" });
            DropIndex("dbo.tabDepartment", new[] { "GridReport_Id", "GridReport_userId", "GridReport_settingkey" });
            DropIndex("dbo.Roles", new[] { "GridReportGroup_Id" });
            DropColumn("dbo.GridReports", "group_Id");
            DropColumn("dbo.tabDepartment", "GridReport_settingkey");
            DropColumn("dbo.tabDepartment", "GridReport_userId");
            DropColumn("dbo.tabDepartment", "GridReport_Id");
            DropColumn("dbo.Roles", "GridReportGroup_Id");
            DropTable("dbo.GridReportGroups");
        }
    }
}
