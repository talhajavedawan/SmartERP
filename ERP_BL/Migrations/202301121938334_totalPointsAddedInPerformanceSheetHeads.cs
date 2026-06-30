namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class totalPointsAddedInPerformanceSheetHeads : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PerformanceSheetHeads", "totalPoints", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PerformanceSheetHeads", "totalPoints");
        }
    }
}
