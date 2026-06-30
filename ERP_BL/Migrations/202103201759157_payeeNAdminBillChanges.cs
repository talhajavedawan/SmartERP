namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class payeeNAdminBillChanges : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PayeeIndustryTypes", "Payee_Id", "dbo.Payees");
            DropForeignKey("dbo.PayeeIndustryTypes", "IndustryType_Id", "dbo.IndustryTypes");
            DropForeignKey("dbo.tabAdminBill", "industryType_Id", "dbo.IndustryTypes");
            DropIndex("dbo.tabAdminBill", new[] { "industryType_Id" });
            DropIndex("dbo.PayeeIndustryTypes", new[] { "Payee_Id" });
            DropIndex("dbo.PayeeIndustryTypes", new[] { "IndustryType_Id" });
            CreateTable(
                "dbo.AdminBillTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        isApproved = c.Boolean(nullable: false),
                        isActive = c.Boolean(nullable: false),
                        user_Id = c.Int(),
                        addedDate = c.DateTime(nullable: false),
                        isIndustryType = c.Boolean(nullable: false),
                        IndustryTypeId = c.Int(),
                        COA_AccountType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.IndustryTypes", t => t.IndustryTypeId)
                .ForeignKey("dbo.Users", t => t.user_Id)
                .Index(t => t.user_Id)
                .Index(t => t.IndustryTypeId);
            
            CreateTable(
                "dbo.AdminBillTypeChartofAccounts",
                c => new
                    {
                        AdminBillType_Id = c.Int(nullable: false),
                        ChartofAccount_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.AdminBillType_Id, t.ChartofAccount_Id })
                .ForeignKey("dbo.AdminBillTypes", t => t.AdminBillType_Id, cascadeDelete: true)
                .ForeignKey("dbo.ChartofAccounts", t => t.ChartofAccount_Id, cascadeDelete: true)
                .Index(t => t.AdminBillType_Id)
                .Index(t => t.ChartofAccount_Id);
            
            CreateTable(
                "dbo.PayeeAdminBillTypes",
                c => new
                    {
                        Payee_Id = c.Int(nullable: false),
                        AdminBillType_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Payee_Id, t.AdminBillType_Id })
                .ForeignKey("dbo.Payees", t => t.Payee_Id, cascadeDelete: true)
                .ForeignKey("dbo.AdminBillTypes", t => t.AdminBillType_Id, cascadeDelete: true)
                .Index(t => t.Payee_Id)
                .Index(t => t.AdminBillType_Id);
            
            CreateTable(
                "dbo.VendorAdminBillTypes",
                c => new
                    {
                        Vendor_Id = c.Int(nullable: false),
                        AdminBillType_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Vendor_Id, t.AdminBillType_Id })
                .ForeignKey("dbo.tabVendor", t => t.Vendor_Id, cascadeDelete: true)
                .ForeignKey("dbo.AdminBillTypes", t => t.AdminBillType_Id, cascadeDelete: true)
                .Index(t => t.Vendor_Id)
                .Index(t => t.AdminBillType_Id);
            
            AddColumn("dbo.tabAdminBill", "CoaCredit_Id", c => c.Int());
            AddColumn("dbo.tabAdminBill", "adminBillType_Id", c => c.Int());
            CreateIndex("dbo.tabAdminBill", "CoaCredit_Id");
            CreateIndex("dbo.tabAdminBill", "adminBillType_Id");
            AddForeignKey("dbo.tabAdminBill", "adminBillType_Id", "dbo.AdminBillTypes", "Id");
            AddForeignKey("dbo.tabAdminBill", "CoaCredit_Id", "dbo.ChartofAccounts", "Id");
            DropColumn("dbo.tabAdminBill", "industryType_Id");
            DropTable("dbo.PayeeIndustryTypes");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.PayeeIndustryTypes",
                c => new
                    {
                        Payee_Id = c.Int(nullable: false),
                        IndustryType_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Payee_Id, t.IndustryType_Id });
            
            AddColumn("dbo.tabAdminBill", "industryType_Id", c => c.Int());
            DropForeignKey("dbo.AdminBillTypes", "user_Id", "dbo.Users");
            DropForeignKey("dbo.tabAdminBill", "CoaCredit_Id", "dbo.ChartofAccounts");
            DropForeignKey("dbo.tabAdminBill", "adminBillType_Id", "dbo.AdminBillTypes");
            DropForeignKey("dbo.VendorAdminBillTypes", "AdminBillType_Id", "dbo.AdminBillTypes");
            DropForeignKey("dbo.VendorAdminBillTypes", "Vendor_Id", "dbo.tabVendor");
            DropForeignKey("dbo.PayeeAdminBillTypes", "AdminBillType_Id", "dbo.AdminBillTypes");
            DropForeignKey("dbo.PayeeAdminBillTypes", "Payee_Id", "dbo.Payees");
            DropForeignKey("dbo.AdminBillTypes", "IndustryTypeId", "dbo.IndustryTypes");
            DropForeignKey("dbo.AdminBillTypeChartofAccounts", "ChartofAccount_Id", "dbo.ChartofAccounts");
            DropForeignKey("dbo.AdminBillTypeChartofAccounts", "AdminBillType_Id", "dbo.AdminBillTypes");
            DropIndex("dbo.VendorAdminBillTypes", new[] { "AdminBillType_Id" });
            DropIndex("dbo.VendorAdminBillTypes", new[] { "Vendor_Id" });
            DropIndex("dbo.PayeeAdminBillTypes", new[] { "AdminBillType_Id" });
            DropIndex("dbo.PayeeAdminBillTypes", new[] { "Payee_Id" });
            DropIndex("dbo.AdminBillTypeChartofAccounts", new[] { "ChartofAccount_Id" });
            DropIndex("dbo.AdminBillTypeChartofAccounts", new[] { "AdminBillType_Id" });
            DropIndex("dbo.tabAdminBill", new[] { "adminBillType_Id" });
            DropIndex("dbo.tabAdminBill", new[] { "CoaCredit_Id" });
            DropIndex("dbo.AdminBillTypes", new[] { "IndustryTypeId" });
            DropIndex("dbo.AdminBillTypes", new[] { "user_Id" });
            DropColumn("dbo.tabAdminBill", "adminBillType_Id");
            DropColumn("dbo.tabAdminBill", "CoaCredit_Id");
            DropTable("dbo.VendorAdminBillTypes");
            DropTable("dbo.PayeeAdminBillTypes");
            DropTable("dbo.AdminBillTypeChartofAccounts");
            DropTable("dbo.AdminBillTypes");
            CreateIndex("dbo.PayeeIndustryTypes", "IndustryType_Id");
            CreateIndex("dbo.PayeeIndustryTypes", "Payee_Id");
            CreateIndex("dbo.tabAdminBill", "industryType_Id");
            AddForeignKey("dbo.tabAdminBill", "industryType_Id", "dbo.IndustryTypes", "Id");
            AddForeignKey("dbo.PayeeIndustryTypes", "IndustryType_Id", "dbo.IndustryTypes", "Id", cascadeDelete: true);
            AddForeignKey("dbo.PayeeIndustryTypes", "Payee_Id", "dbo.Payees", "Id", cascadeDelete: true);
        }
    }
}
