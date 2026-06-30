namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class itineraryStatusAddedInVisitingCountries : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VisitingCountries", "itineraryStatus", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.VisitingCountries", "itineraryStatus");
        }
    }
}
