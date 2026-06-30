namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class soNumberAddedInPerformanceSheets : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PerformanceSheets", "soNumber", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PerformanceSheets", "soNumber", c => c.Int(nullable: false));
        }
    }
}
