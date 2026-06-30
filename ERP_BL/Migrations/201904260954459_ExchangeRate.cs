namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ExchangeRate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ExchangeRates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AddedOn = c.DateTime(nullable: false),
                        effectivesince = c.DateTime(nullable: false),
                        currency_Id = c.Int(),
                        Basecurrency_Id = c.Int(),
                        Addedbyuser_Id = c.Int(),
                        Editedbyuser_Id = c.Int(),
                        company_Id = c.Int(),
                        LastUpdated = c.DateTime(),
                        isApproved = c.Boolean(),
                        exchangerate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        maxVariationPercent = c.Decimal(precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Addedbyuser_Id)
                .ForeignKey("dbo.Currencies", t => t.Basecurrency_Id)
                .ForeignKey("dbo.tabCompany", t => t.company_Id)
                .ForeignKey("dbo.Currencies", t => t.currency_Id)
                .ForeignKey("dbo.Users", t => t.Editedbyuser_Id)
                .Index(t => t.currency_Id)
                .Index(t => t.Basecurrency_Id)
                .Index(t => t.Addedbyuser_Id)
                .Index(t => t.Editedbyuser_Id)
                .Index(t => t.company_Id);
            
            CreateTable(
                "dbo.SalesExchangeRates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AddedOn = c.DateTime(nullable: false),
                        targetYear = c.Int(nullable: false),
                        currency_Id = c.Int(),
                        Basecurrency_Id = c.Int(),
                        Addedbyuser_Id = c.Int(),
                        Editedbyuser_Id = c.Int(),
                        company_Id = c.Int(),
                        LastUpdated = c.DateTime(),
                        isApproved = c.Boolean(),
                        exchangerate = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Addedbyuser_Id)
                .ForeignKey("dbo.Currencies", t => t.Basecurrency_Id)
                .ForeignKey("dbo.tabCompany", t => t.company_Id)
                .ForeignKey("dbo.Currencies", t => t.currency_Id)
                .ForeignKey("dbo.Users", t => t.Editedbyuser_Id)
                .Index(t => t.currency_Id)
                .Index(t => t.Basecurrency_Id)
                .Index(t => t.Addedbyuser_Id)
                .Index(t => t.Editedbyuser_Id)
                .Index(t => t.company_Id);
            
            AddColumn("dbo.Offers", "principal_Id", c => c.Int());
            CreateIndex("dbo.Offers", "principal_Id");
            AddForeignKey("dbo.Offers", "principal_Id", "dbo.Principals", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SalesExchangeRates", "Editedbyuser_Id", "dbo.Users");
            DropForeignKey("dbo.SalesExchangeRates", "currency_Id", "dbo.Currencies");
            DropForeignKey("dbo.SalesExchangeRates", "company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.SalesExchangeRates", "Basecurrency_Id", "dbo.Currencies");
            DropForeignKey("dbo.SalesExchangeRates", "Addedbyuser_Id", "dbo.Users");
            DropForeignKey("dbo.Offers", "principal_Id", "dbo.Principals");
            DropForeignKey("dbo.ExchangeRates", "Editedbyuser_Id", "dbo.Users");
            DropForeignKey("dbo.ExchangeRates", "currency_Id", "dbo.Currencies");
            DropForeignKey("dbo.ExchangeRates", "company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.ExchangeRates", "Basecurrency_Id", "dbo.Currencies");
            DropForeignKey("dbo.ExchangeRates", "Addedbyuser_Id", "dbo.Users");
            DropIndex("dbo.SalesExchangeRates", new[] { "company_Id" });
            DropIndex("dbo.SalesExchangeRates", new[] { "Editedbyuser_Id" });
            DropIndex("dbo.SalesExchangeRates", new[] { "Addedbyuser_Id" });
            DropIndex("dbo.SalesExchangeRates", new[] { "Basecurrency_Id" });
            DropIndex("dbo.SalesExchangeRates", new[] { "currency_Id" });
            DropIndex("dbo.Offers", new[] { "principal_Id" });
            DropIndex("dbo.ExchangeRates", new[] { "company_Id" });
            DropIndex("dbo.ExchangeRates", new[] { "Editedbyuser_Id" });
            DropIndex("dbo.ExchangeRates", new[] { "Addedbyuser_Id" });
            DropIndex("dbo.ExchangeRates", new[] { "Basecurrency_Id" });
            DropIndex("dbo.ExchangeRates", new[] { "currency_Id" });
            DropColumn("dbo.Offers", "principal_Id");
            DropTable("dbo.SalesExchangeRates");
            DropTable("dbo.ExchangeRates");
        }
    }
}
