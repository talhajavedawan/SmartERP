namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class openingBalanceAddedInCOA : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ChartofAccounts", "openingBalance", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ChartofAccounts", "openingBalance");
        }
    }
}
