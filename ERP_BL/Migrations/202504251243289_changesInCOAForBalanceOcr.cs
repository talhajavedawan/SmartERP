namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changesInCOAForBalanceOcr : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ChartofAccounts", "BalanceOC", c => c.Double(nullable: false));
            AddColumn("dbo.ChartofAccounts", "BalancePKR", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ChartofAccounts", "BalancePKR");
            DropColumn("dbo.ChartofAccounts", "BalanceOC");
        }
    }
}
