namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReligionsAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Religions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ReligionName = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.SaleInvoices", "SItaxSubject", c => c.String());
            AddColumn("dbo.SaleInvoices", "CISubject", c => c.String());
            AddColumn("dbo.SaleInvoices", "DNSubject", c => c.String());
            AddColumn("dbo.ContactPersons", "customerCompanyId", c => c.Int());
            AddColumn("dbo.ContactPersons", "ReligionId", c => c.Int());
            CreateIndex("dbo.ContactPersons", "customerCompanyId");
            CreateIndex("dbo.ContactPersons", "ReligionId");
            AddForeignKey("dbo.ContactPersons", "customerCompanyId", "dbo.CustomerCompanies", "Id");
            AddForeignKey("dbo.ContactPersons", "ReligionId", "dbo.Religions", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ContactPersons", "ReligionId", "dbo.Religions");
            DropForeignKey("dbo.ContactPersons", "customerCompanyId", "dbo.CustomerCompanies");
            DropIndex("dbo.ContactPersons", new[] { "ReligionId" });
            DropIndex("dbo.ContactPersons", new[] { "customerCompanyId" });
            DropColumn("dbo.ContactPersons", "ReligionId");
            DropColumn("dbo.ContactPersons", "customerCompanyId");
            DropColumn("dbo.SaleInvoices", "DNSubject");
            DropColumn("dbo.SaleInvoices", "CISubject");
            DropColumn("dbo.SaleInvoices", "SItaxSubject");
            DropTable("dbo.Religions");
        }
    }
}
