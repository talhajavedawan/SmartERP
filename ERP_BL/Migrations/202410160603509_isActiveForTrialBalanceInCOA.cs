namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class isActiveForTrialBalanceInCOA : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ChartofAccounts", "isActiveForTrialBalance", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ChartofAccounts", "isActiveForTrialBalance");
        }
    }
}
