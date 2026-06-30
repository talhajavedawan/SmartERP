namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CustomerCompanyIndustryTypesAdded : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.tabCompany", "industryTypeId", "dbo.IndustryTypes");
            DropIndex("dbo.tabCompany", new[] { "industryTypeId" });
            CreateTable(
                "dbo.CustomerCompanyIndustryTypes",
                c => new
                    {
                        CustomerCompany_Id = c.Int(nullable: false),
                        IndustryType_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CustomerCompany_Id, t.IndustryType_Id })
                .ForeignKey("dbo.CustomerCompanies", t => t.CustomerCompany_Id, cascadeDelete: true)
                .ForeignKey("dbo.IndustryTypes", t => t.IndustryType_Id, cascadeDelete: true)
                .Index(t => t.CustomerCompany_Id)
                .Index(t => t.IndustryType_Id);
            
            AlterColumn("dbo.tabCompany", "industryTypeId", c => c.Int());
            CreateIndex("dbo.tabCompany", "industryTypeId");
            AddForeignKey("dbo.tabCompany", "industryTypeId", "dbo.IndustryTypes", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tabCompany", "industryTypeId", "dbo.IndustryTypes");
            DropForeignKey("dbo.CustomerCompanyIndustryTypes", "IndustryType_Id", "dbo.IndustryTypes");
            DropForeignKey("dbo.CustomerCompanyIndustryTypes", "CustomerCompany_Id", "dbo.CustomerCompanies");
            DropIndex("dbo.CustomerCompanyIndustryTypes", new[] { "IndustryType_Id" });
            DropIndex("dbo.CustomerCompanyIndustryTypes", new[] { "CustomerCompany_Id" });
            DropIndex("dbo.tabCompany", new[] { "industryTypeId" });
            AlterColumn("dbo.tabCompany", "industryTypeId", c => c.Int(nullable: false));
            DropTable("dbo.CustomerCompanyIndustryTypes");
            CreateIndex("dbo.tabCompany", "industryTypeId");
            AddForeignKey("dbo.tabCompany", "industryTypeId", "dbo.IndustryTypes", "Id", cascadeDelete: true);
        }
    }
}
