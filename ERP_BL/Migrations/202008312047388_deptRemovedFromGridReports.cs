namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deptRemovedFromGridReports : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.tabDepartment", new[] { "GridReport_Id", "GridReport_userId", "GridReport_settingkey" }, "dbo.GridReports");
            DropIndex("dbo.tabDepartment", new[] { "GridReport_Id", "GridReport_userId", "GridReport_settingkey" });
            DropColumn("dbo.tabDepartment", "GridReport_Id");
            DropColumn("dbo.tabDepartment", "GridReport_userId");
            DropColumn("dbo.tabDepartment", "GridReport_settingkey");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tabDepartment", "GridReport_settingkey", c => c.String(maxLength: 128, unicode: false));
            AddColumn("dbo.tabDepartment", "GridReport_userId", c => c.Int());
            AddColumn("dbo.tabDepartment", "GridReport_Id", c => c.Int());
            CreateIndex("dbo.tabDepartment", new[] { "GridReport_Id", "GridReport_userId", "GridReport_settingkey" });
            AddForeignKey("dbo.tabDepartment", new[] { "GridReport_Id", "GridReport_userId", "GridReport_settingkey" }, "dbo.GridReports", new[] { "Id", "userId", "settingkey" });
        }
    }
}
