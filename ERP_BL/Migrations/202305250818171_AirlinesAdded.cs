namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AirlinesAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Airlines",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.AttachmentCategories", "TravelingRecord", c => c.Int());
            AddColumn("dbo.VisitingCountries", "airlineId", c => c.Int());
            CreateIndex("dbo.VisitingCountries", "airlineId");
            AddForeignKey("dbo.VisitingCountries", "airlineId", "dbo.Airlines", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VisitingCountries", "airlineId", "dbo.Airlines");
            DropIndex("dbo.VisitingCountries", new[] { "airlineId" });
            DropColumn("dbo.VisitingCountries", "airlineId");
            DropColumn("dbo.AttachmentCategories", "TravelingRecord");
            DropTable("dbo.Airlines");
        }
    }
}
