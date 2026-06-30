namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesInVisitingCountries1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VisitingCountries", "currencyId", c => c.Int());
            AddColumn("dbo.VisitingCountries", "TicketCost", c => c.Double(nullable: false));
            AddColumn("dbo.VisitingCountries", "MER", c => c.Double(nullable: false));
            AddColumn("dbo.VisitingCountries", "TicketCostMER", c => c.Double(nullable: false));
            CreateIndex("dbo.VisitingCountries", "currencyId");
            AddForeignKey("dbo.VisitingCountries", "currencyId", "dbo.Currencies", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VisitingCountries", "currencyId", "dbo.Currencies");
            DropIndex("dbo.VisitingCountries", new[] { "currencyId" });
            DropColumn("dbo.VisitingCountries", "TicketCostMER");
            DropColumn("dbo.VisitingCountries", "MER");
            DropColumn("dbo.VisitingCountries", "TicketCost");
            DropColumn("dbo.VisitingCountries", "currencyId");
        }
    }
}
