namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TravelersAndTravelingRecordsAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Travelers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        residentCountryId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Countries", t => t.residentCountryId)
                .Index(t => t.residentCountryId);
            
            CreateTable(
                "dbo.TravelingRecords",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreationDate = c.DateTime(),
                        travelerNameId = c.Int(),
                        transactionGroupId = c.Int(nullable: false),
                        SystemRefNo = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Travelers", t => t.travelerNameId)
                .Index(t => t.travelerNameId);
            
            CreateTable(
                "dbo.VisitingCountries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TravelingRecordsId = c.Int(),
                        departingCountryId = c.Int(),
                        visitingCountryId = c.Int(),
                        DepartureDate = c.DateTime(),
                        ArrivalDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Countries", t => t.departingCountryId)
                .ForeignKey("dbo.TravelingRecords", t => t.TravelingRecordsId)
                .ForeignKey("dbo.Countries", t => t.visitingCountryId)
                .Index(t => t.TravelingRecordsId)
                .Index(t => t.departingCountryId)
                .Index(t => t.visitingCountryId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VisitingCountries", "visitingCountryId", "dbo.Countries");
            DropForeignKey("dbo.VisitingCountries", "TravelingRecordsId", "dbo.TravelingRecords");
            DropForeignKey("dbo.VisitingCountries", "departingCountryId", "dbo.Countries");
            DropForeignKey("dbo.TravelingRecords", "travelerNameId", "dbo.Travelers");
            DropForeignKey("dbo.Travelers", "residentCountryId", "dbo.Countries");
            DropIndex("dbo.VisitingCountries", new[] { "visitingCountryId" });
            DropIndex("dbo.VisitingCountries", new[] { "departingCountryId" });
            DropIndex("dbo.VisitingCountries", new[] { "TravelingRecordsId" });
            DropIndex("dbo.TravelingRecords", new[] { "travelerNameId" });
            DropIndex("dbo.Travelers", new[] { "residentCountryId" });
            DropTable("dbo.VisitingCountries");
            DropTable("dbo.TravelingRecords");
            DropTable("dbo.Travelers");
        }
    }
}
