namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesInPayeeNVendor : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.PayeeVendors", newName: "VendorPayees");
            DropForeignKey("dbo.ChartofAccounts", "companyId", "dbo.tabCompany");
            DropForeignKey("dbo.Payees", "IndustryTypeId", "dbo.IndustryTypes");
            DropIndex("dbo.ChartofAccounts", new[] { "companyId" });
            DropIndex("dbo.Payees", new[] { "IndustryTypeId" });
            DropPrimaryKey("dbo.VendorPayees");
            CreateTable(
                "dbo.BillRefNumbers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        companyId = c.Int(),
                        BillReferenceNo = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tabCompany", t => t.companyId)
                .Index(t => t.companyId);
            
            CreateTable(
                "dbo.ChartofAccountCompanies",
                c => new
                    {
                        ChartofAccount_Id = c.Int(nullable: false),
                        Company_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ChartofAccount_Id, t.Company_Id })
                .ForeignKey("dbo.ChartofAccounts", t => t.ChartofAccount_Id, cascadeDelete: true)
                .ForeignKey("dbo.tabCompany", t => t.Company_Id, cascadeDelete: true)
                .Index(t => t.ChartofAccount_Id)
                .Index(t => t.Company_Id);
            
            CreateTable(
                "dbo.PayeeIndustryTypes",
                c => new
                    {
                        Payee_Id = c.Int(nullable: false),
                        IndustryType_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Payee_Id, t.IndustryType_Id })
                .ForeignKey("dbo.Payees", t => t.Payee_Id, cascadeDelete: true)
                .ForeignKey("dbo.IndustryTypes", t => t.IndustryType_Id, cascadeDelete: true)
                .Index(t => t.Payee_Id)
                .Index(t => t.IndustryType_Id);
            
            AddColumn("dbo.tabAdminBill", "BillRefNoId", c => c.Int());
            AddPrimaryKey("dbo.VendorPayees", new[] { "Vendor_Id", "Payee_Id" });
            CreateIndex("dbo.tabAdminBill", "BillRefNoId");
            AddForeignKey("dbo.tabAdminBill", "BillRefNoId", "dbo.BillRefNumbers", "Id");
            DropColumn("dbo.ChartofAccounts", "companyId");
            DropColumn("dbo.Payees", "IndustryTypeId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Payees", "IndustryTypeId", c => c.Int());
            AddColumn("dbo.ChartofAccounts", "companyId", c => c.Int());
            DropForeignKey("dbo.tabAdminBill", "BillRefNoId", "dbo.BillRefNumbers");
            DropForeignKey("dbo.BillRefNumbers", "companyId", "dbo.tabCompany");
            DropForeignKey("dbo.PayeeIndustryTypes", "IndustryType_Id", "dbo.IndustryTypes");
            DropForeignKey("dbo.PayeeIndustryTypes", "Payee_Id", "dbo.Payees");
            DropForeignKey("dbo.ChartofAccountCompanies", "Company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.ChartofAccountCompanies", "ChartofAccount_Id", "dbo.ChartofAccounts");
            DropIndex("dbo.PayeeIndustryTypes", new[] { "IndustryType_Id" });
            DropIndex("dbo.PayeeIndustryTypes", new[] { "Payee_Id" });
            DropIndex("dbo.ChartofAccountCompanies", new[] { "Company_Id" });
            DropIndex("dbo.ChartofAccountCompanies", new[] { "ChartofAccount_Id" });
            DropIndex("dbo.BillRefNumbers", new[] { "companyId" });
            DropIndex("dbo.tabAdminBill", new[] { "BillRefNoId" });
            DropPrimaryKey("dbo.VendorPayees");
            DropColumn("dbo.tabAdminBill", "BillRefNoId");
            DropTable("dbo.PayeeIndustryTypes");
            DropTable("dbo.ChartofAccountCompanies");
            DropTable("dbo.BillRefNumbers");
            AddPrimaryKey("dbo.VendorPayees", new[] { "Payee_Id", "Vendor_Id" });
            CreateIndex("dbo.Payees", "IndustryTypeId");
            CreateIndex("dbo.ChartofAccounts", "companyId");
            AddForeignKey("dbo.Payees", "IndustryTypeId", "dbo.IndustryTypes", "Id");
            AddForeignKey("dbo.ChartofAccounts", "companyId", "dbo.tabCompany", "Id");
            RenameTable(name: "dbo.VendorPayees", newName: "PayeeVendors");
        }
    }
}
