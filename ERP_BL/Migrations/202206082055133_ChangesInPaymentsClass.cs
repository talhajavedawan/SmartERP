namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesInPaymentsClass : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Payments", "DeductionExchangeRate", c => c.Double(nullable: false));
            AddColumn("dbo.Payments", "DeductionSOC", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Payments", "DeductionSOC");
            DropColumn("dbo.Payments", "DeductionExchangeRate");
        }
    }
}
