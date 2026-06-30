namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changesInAdminbillsSalesTargetPayeee : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SalesTargets", "CurrencyId", "dbo.Currencies");
            DropIndex("dbo.SalesTargets", new[] { "CurrencyId" });
            CreateTable(
                "dbo.PayeeVendors",
                c => new
                    {
                        Payee_Id = c.Int(nullable: false),
                        Vendor_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Payee_Id, t.Vendor_Id })
                .ForeignKey("dbo.Payees", t => t.Payee_Id, cascadeDelete: true)
                .ForeignKey("dbo.tabVendor", t => t.Vendor_Id, cascadeDelete: true)
                .Index(t => t.Payee_Id)
                .Index(t => t.Vendor_Id);
            
            AddColumn("dbo.Payees", "IndustryTypeId", c => c.Int());
            AddColumn("dbo.tabAdminBill", "Memo", c => c.String());
            AddColumn("dbo.tabAdminBill", "stage", c => c.String());
            AddColumn("dbo.tabAdminBill", "isVoid", c => c.Boolean(nullable: false));
            AddColumn("dbo.tabAdminBill", "isReviewed", c => c.Boolean());
            AddColumn("dbo.tabAdminBill", "needReview", c => c.Boolean());
            AddColumn("dbo.tabAdminBill", "PendingForClosing", c => c.Boolean());
            AddColumn("dbo.tabAdminBill", "PendingForReApproval", c => c.Boolean());
            AddColumn("dbo.tabAdminBill", "isApproved", c => c.Boolean());
            AddColumn("dbo.tabAdminBill", "ApprovedDate", c => c.DateTime());
            AddColumn("dbo.tabAdminBill", "isReApproved", c => c.Boolean());
            AddColumn("dbo.tabAdminBill", "ReApprovalDate", c => c.DateTime());
            AddColumn("dbo.tabAdminBill", "user_Id", c => c.Int());
            AddColumn("dbo.tabAdminBill", "ClosingDate", c => c.DateTime());
            AddColumn("dbo.tabAdminBill", "LastStatusChangeDate", c => c.DateTime());
            AlterColumn("dbo.SalesTargets", "CurrencyId", c => c.Int());
            CreateIndex("dbo.Payees", "IndustryTypeId");
            CreateIndex("dbo.SalesTargets", "CurrencyId");
            CreateIndex("dbo.tabAdminBill", "user_Id");
            AddForeignKey("dbo.Payees", "IndustryTypeId", "dbo.IndustryTypes", "Id");
            AddForeignKey("dbo.tabAdminBill", "user_Id", "dbo.Users", "id");
            AddForeignKey("dbo.SalesTargets", "CurrencyId", "dbo.Currencies", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SalesTargets", "CurrencyId", "dbo.Currencies");
            DropForeignKey("dbo.tabAdminBill", "user_Id", "dbo.Users");
            DropForeignKey("dbo.PayeeVendors", "Vendor_Id", "dbo.tabVendor");
            DropForeignKey("dbo.PayeeVendors", "Payee_Id", "dbo.Payees");
            DropForeignKey("dbo.Payees", "IndustryTypeId", "dbo.IndustryTypes");
            DropIndex("dbo.PayeeVendors", new[] { "Vendor_Id" });
            DropIndex("dbo.PayeeVendors", new[] { "Payee_Id" });
            DropIndex("dbo.tabAdminBill", new[] { "user_Id" });
            DropIndex("dbo.SalesTargets", new[] { "CurrencyId" });
            DropIndex("dbo.Payees", new[] { "IndustryTypeId" });
            AlterColumn("dbo.SalesTargets", "CurrencyId", c => c.Int(nullable: false));
            DropColumn("dbo.tabAdminBill", "LastStatusChangeDate");
            DropColumn("dbo.tabAdminBill", "ClosingDate");
            DropColumn("dbo.tabAdminBill", "user_Id");
            DropColumn("dbo.tabAdminBill", "ReApprovalDate");
            DropColumn("dbo.tabAdminBill", "isReApproved");
            DropColumn("dbo.tabAdminBill", "ApprovedDate");
            DropColumn("dbo.tabAdminBill", "isApproved");
            DropColumn("dbo.tabAdminBill", "PendingForReApproval");
            DropColumn("dbo.tabAdminBill", "PendingForClosing");
            DropColumn("dbo.tabAdminBill", "needReview");
            DropColumn("dbo.tabAdminBill", "isReviewed");
            DropColumn("dbo.tabAdminBill", "isVoid");
            DropColumn("dbo.tabAdminBill", "stage");
            DropColumn("dbo.tabAdminBill", "Memo");
            DropColumn("dbo.Payees", "IndustryTypeId");
            DropTable("dbo.PayeeVendors");
            CreateIndex("dbo.SalesTargets", "CurrencyId");
            AddForeignKey("dbo.SalesTargets", "CurrencyId", "dbo.Currencies", "Id", cascadeDelete: true);
        }
    }
}
