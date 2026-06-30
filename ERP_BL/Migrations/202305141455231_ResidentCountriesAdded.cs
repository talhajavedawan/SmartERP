namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ResidentCountriesAdded : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Travelers", "residentCountryId", "dbo.Countries");
            DropIndex("dbo.Travelers", new[] { "residentCountryId" });
            CreateTable(
                "dbo.ResidentCountries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        travelerId = c.Int(),
                        countryId = c.Int(),
                        fromDate = c.DateTime(nullable: false),
                        toDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Countries", t => t.countryId)
                .ForeignKey("dbo.Travelers", t => t.travelerId)
                .Index(t => t.travelerId)
                .Index(t => t.countryId);
            
            DropColumn("dbo.Travelers", "residentCountryId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Travelers", "residentCountryId", c => c.Int());
            DropForeignKey("dbo.ResidentCountries", "travelerId", "dbo.Travelers");
            DropForeignKey("dbo.ResidentCountries", "countryId", "dbo.Countries");
            DropIndex("dbo.ResidentCountries", new[] { "countryId" });
            DropIndex("dbo.ResidentCountries", new[] { "travelerId" });
            DropTable("dbo.ResidentCountries");
            CreateIndex("dbo.Travelers", "residentCountryId");
            AddForeignKey("dbo.Travelers", "residentCountryId", "dbo.Countries", "Id");
        }
    }
}
