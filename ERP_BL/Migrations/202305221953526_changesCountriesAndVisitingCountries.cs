namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changesCountriesAndVisitingCountries : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Countries", "ResidentDays", c => c.Int(nullable: false));
            AddColumn("dbo.VisitingCountries", "TicketNo", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.VisitingCountries", "TicketNo");
            DropColumn("dbo.Countries", "ResidentDays");
        }
    }
}
