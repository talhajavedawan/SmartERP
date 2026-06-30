namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesInVisitingCountries : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VisitingCountries", "LeavingDate", c => c.DateTime());
            AddColumn("dbo.VisitingCountries", "End", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.VisitingCountries", "End");
            DropColumn("dbo.VisitingCountries", "LeavingDate");
        }
    }
}
