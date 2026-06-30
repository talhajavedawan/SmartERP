namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FacilityNaturesAndtabLoanAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FacilityNatures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NatureName = c.String(),
                        isActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.tabLoan",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreationDate = c.DateTime(nullable: false),
                        CompanyId = c.Int(),
                        deptId = c.Int(),
                        bankId = c.Int(),
                        accountId = c.Int(),
                        currencyId = c.Int(),
                        facilityNatureId = c.Int(),
                        Amount = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Accounts", t => t.accountId)
                .ForeignKey("dbo.Banks", t => t.bankId)
                .ForeignKey("dbo.tabCompany", t => t.CompanyId)
                .ForeignKey("dbo.Currencies", t => t.currencyId)
                .ForeignKey("dbo.tabDepartment", t => t.deptId)
                .ForeignKey("dbo.FacilityNatures", t => t.facilityNatureId)
                .Index(t => t.CompanyId)
                .Index(t => t.deptId)
                .Index(t => t.bankId)
                .Index(t => t.accountId)
                .Index(t => t.currencyId)
                .Index(t => t.facilityNatureId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tabLoan", "facilityNatureId", "dbo.FacilityNatures");
            DropForeignKey("dbo.tabLoan", "deptId", "dbo.tabDepartment");
            DropForeignKey("dbo.tabLoan", "currencyId", "dbo.Currencies");
            DropForeignKey("dbo.tabLoan", "CompanyId", "dbo.tabCompany");
            DropForeignKey("dbo.tabLoan", "bankId", "dbo.Banks");
            DropForeignKey("dbo.tabLoan", "accountId", "dbo.Accounts");
            DropIndex("dbo.tabLoan", new[] { "facilityNatureId" });
            DropIndex("dbo.tabLoan", new[] { "currencyId" });
            DropIndex("dbo.tabLoan", new[] { "accountId" });
            DropIndex("dbo.tabLoan", new[] { "bankId" });
            DropIndex("dbo.tabLoan", new[] { "deptId" });
            DropIndex("dbo.tabLoan", new[] { "CompanyId" });
            DropTable("dbo.tabLoan");
            DropTable("dbo.FacilityNatures");
        }
    }
}
