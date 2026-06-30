namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesInChartofAccounts : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ChartofAccounts", "manualBalanceOC", c => c.Double(nullable: false));
            AddColumn("dbo.ChartofAccounts", "manualBalancePKR", c => c.Double(nullable: false));
            AddColumn("dbo.STLs", "stlRemarks", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.STLs", "stlRemarks");
            DropColumn("dbo.ChartofAccounts", "manualBalancePKR");
            DropColumn("dbo.ChartofAccounts", "manualBalanceOC");
        }
    }
}
