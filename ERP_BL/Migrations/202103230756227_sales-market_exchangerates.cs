namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class salesmarket_exchangerates : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ExchangeRates", "Addedbyuser_Id", "dbo.Users");
            DropForeignKey("dbo.ExchangeRates", "Basecurrency_Id", "dbo.Currencies");
            DropForeignKey("dbo.ExchangeRates", "company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.ExchangeRates", "currency_Id", "dbo.Currencies");
            DropForeignKey("dbo.ExchangeRates", "Editedbyuser_Id", "dbo.Users");
            DropIndex("dbo.ExchangeRates", new[] { "currency_Id" });
            DropIndex("dbo.ExchangeRates", new[] { "Basecurrency_Id" });
            DropIndex("dbo.ExchangeRates", new[] { "Addedbyuser_Id" });
            DropIndex("dbo.ExchangeRates", new[] { "Editedbyuser_Id" });
            DropIndex("dbo.ExchangeRates", new[] { "company_Id" });
            RenameColumn(table: "dbo.SalesExchangeRates", name: "Basecurrency_Id", newName: "base_currency_Id");
            RenameColumn(table: "dbo.SalesExchangeRates", name: "currency_Id", newName: "target_currency_Id");
            RenameIndex(table: "dbo.SalesExchangeRates", name: "IX_currency_Id", newName: "IX_target_currency_Id");
            RenameIndex(table: "dbo.SalesExchangeRates", name: "IX_Basecurrency_Id", newName: "IX_base_currency_Id");
            CreateTable(
                "dbo.MarketExchangeRates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AddedOn = c.DateTime(nullable: false),
                        effectiveFrom = c.DateTime(nullable: false),
                        effectiveSince = c.DateTime(nullable: false),
                        target_currency_Id = c.Int(),
                        base_currency_Id = c.Int(),
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
                .ForeignKey("dbo.Currencies", t => t.base_currency_Id)
                .ForeignKey("dbo.Users", t => t.Editedbyuser_Id)
                .ForeignKey("dbo.Currencies", t => t.target_currency_Id)
                .ForeignKey("dbo.tabCompany", t => t.company_Id)
                .Index(t => t.target_currency_Id)
                .Index(t => t.base_currency_Id)
                .Index(t => t.Addedbyuser_Id)
                .Index(t => t.Editedbyuser_Id)
                .Index(t => t.company_Id);
            
            AddColumn("dbo.SalesExchangeRates", "effectiveFrom", c => c.DateTime(nullable: false));
            AddColumn("dbo.SalesExchangeRates", "effectiveSince", c => c.DateTime(nullable: false));
            DropTable("dbo.ExchangeRates");
        }
        
        public override void Down()
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
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.MarketExchangeRates", "company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.MarketExchangeRates", "target_currency_Id", "dbo.Currencies");
            DropForeignKey("dbo.MarketExchangeRates", "Editedbyuser_Id", "dbo.Users");
            DropForeignKey("dbo.MarketExchangeRates", "base_currency_Id", "dbo.Currencies");
            DropForeignKey("dbo.MarketExchangeRates", "Addedbyuser_Id", "dbo.Users");
            DropIndex("dbo.MarketExchangeRates", new[] { "company_Id" });
            DropIndex("dbo.MarketExchangeRates", new[] { "Editedbyuser_Id" });
            DropIndex("dbo.MarketExchangeRates", new[] { "Addedbyuser_Id" });
            DropIndex("dbo.MarketExchangeRates", new[] { "base_currency_Id" });
            DropIndex("dbo.MarketExchangeRates", new[] { "target_currency_Id" });
            DropColumn("dbo.SalesExchangeRates", "effectiveSince");
            DropColumn("dbo.SalesExchangeRates", "effectiveFrom");
            DropTable("dbo.MarketExchangeRates");
            RenameIndex(table: "dbo.SalesExchangeRates", name: "IX_base_currency_Id", newName: "IX_Basecurrency_Id");
            RenameIndex(table: "dbo.SalesExchangeRates", name: "IX_target_currency_Id", newName: "IX_currency_Id");
            RenameColumn(table: "dbo.SalesExchangeRates", name: "target_currency_Id", newName: "currency_Id");
            RenameColumn(table: "dbo.SalesExchangeRates", name: "base_currency_Id", newName: "Basecurrency_Id");
            CreateIndex("dbo.ExchangeRates", "company_Id");
            CreateIndex("dbo.ExchangeRates", "Editedbyuser_Id");
            CreateIndex("dbo.ExchangeRates", "Addedbyuser_Id");
            CreateIndex("dbo.ExchangeRates", "Basecurrency_Id");
            CreateIndex("dbo.ExchangeRates", "currency_Id");
            AddForeignKey("dbo.ExchangeRates", "Editedbyuser_Id", "dbo.Users", "id");
            AddForeignKey("dbo.ExchangeRates", "currency_Id", "dbo.Currencies", "Id");
            AddForeignKey("dbo.ExchangeRates", "company_Id", "dbo.tabCompany", "Id");
            AddForeignKey("dbo.ExchangeRates", "Basecurrency_Id", "dbo.Currencies", "Id");
            AddForeignKey("dbo.ExchangeRates", "Addedbyuser_Id", "dbo.Users", "id");
        }
    }
}
