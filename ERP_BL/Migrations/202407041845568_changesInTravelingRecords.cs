namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changesInTravelingRecords : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TravelingRecords", "residentYear", c => c.DateTime());
            AddColumn("dbo.TravelingRecords", "DaysInResidentCountry", c => c.Double(nullable: false));
            AddColumn("dbo.TravelingRecords", "DaysInOtherCountries", c => c.Double(nullable: false));
            AddColumn("dbo.TravelingRecords", "TotalDays", c => c.Double(nullable: false));
            AddColumn("dbo.TravelingRecords", "RequiredResidentDays", c => c.Double(nullable: false));
            AddColumn("dbo.VisitingCountries", "residentYear", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.VisitingCountries", "residentYear");
            DropColumn("dbo.TravelingRecords", "RequiredResidentDays");
            DropColumn("dbo.TravelingRecords", "TotalDays");
            DropColumn("dbo.TravelingRecords", "DaysInOtherCountries");
            DropColumn("dbo.TravelingRecords", "DaysInResidentCountry");
            DropColumn("dbo.TravelingRecords", "residentYear");
        }
    }
}
