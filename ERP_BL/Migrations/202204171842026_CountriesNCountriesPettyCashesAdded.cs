namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CountriesNCountriesPettyCashesAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PettyCashes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreationDate = c.DateTime(nullable: false),
                        TransactionType = c.Int(nullable: false),
                        companyId = c.Int(),
                        deptId = c.Int(),
                        Description = c.String(),
                        currencyId = c.Int(),
                        FinanceRefNo = c.String(),
                        SystemRefNo = c.String(),
                        debit = c.Double(nullable: false),
                        credit = c.Double(nullable: false),
                        interBankTransferId = c.Int(),
                        MER = c.Double(nullable: false),
                        total = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tabCompany", t => t.companyId)
                .ForeignKey("dbo.tabDepartment", t => t.deptId)
                .ForeignKey("dbo.InterBankTransfers", t => t.interBankTransferId)
                .ForeignKey("dbo.Currencies", t => t.currencyId)
                .Index(t => t.companyId)
                .Index(t => t.deptId)
                .Index(t => t.currencyId)
                .Index(t => t.interBankTransferId);
            
            CreateTable(
                "dbo.Cities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CityName = c.String(),
                        Abbriviation = c.String(),
                        PostalCode = c.String(),
                        countryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Countries", t => t.countryId, cascadeDelete: true)
                .Index(t => t.countryId);
            
            CreateTable(
                "dbo.Countries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CountryName = c.String(),
                        Abbriviation = c.String(),
                        CountryCode = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.InterBankTransfers", "isDeposit", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Cities", "countryId", "dbo.Countries");
            DropForeignKey("dbo.PettyCashes", "currencyId", "dbo.Currencies");
            DropForeignKey("dbo.PettyCashes", "interBankTransferId", "dbo.InterBankTransfers");
            DropForeignKey("dbo.PettyCashes", "deptId", "dbo.tabDepartment");
            DropForeignKey("dbo.PettyCashes", "companyId", "dbo.tabCompany");
            DropIndex("dbo.Cities", new[] { "countryId" });
            DropIndex("dbo.PettyCashes", new[] { "interBankTransferId" });
            DropIndex("dbo.PettyCashes", new[] { "currencyId" });
            DropIndex("dbo.PettyCashes", new[] { "deptId" });
            DropIndex("dbo.PettyCashes", new[] { "companyId" });
            DropColumn("dbo.InterBankTransfers", "isDeposit");
            DropTable("dbo.Countries");
            DropTable("dbo.Cities");
            DropTable("dbo.PettyCashes");
        }
    }
}
