namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GridReportsnGroupsConnectionModified : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.GridReports", name: "groupId", newName: "group_Id");
            RenameIndex(table: "dbo.GridReports", name: "IX_groupId", newName: "IX_group_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.GridReports", name: "IX_group_Id", newName: "IX_groupId");
            RenameColumn(table: "dbo.GridReports", name: "group_Id", newName: "groupId");
        }
    }
}
