namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class transactionTypeAddedInGridReportGroups : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GridReportGroups", "transactionType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GridReportGroups", "transactionType");
        }
    }
}
