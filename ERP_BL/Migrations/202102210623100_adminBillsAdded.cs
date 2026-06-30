namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class adminBillsAdded : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.tabNewBills", newName: "tabAdminBill");
            DropForeignKey("dbo.ChartofAccounts", "deptId", "dbo.tabDepartment");
            DropForeignKey("dbo.PayeeCategoryPayees", "PayeeCategory_Id", "dbo.PayeeCategories");
            DropForeignKey("dbo.PayeeCategoryPayees", "Payee_Id", "dbo.Payees");
            DropIndex("dbo.ChartofAccounts", new[] { "deptId" });
            DropIndex("dbo.PayeeCategoryPayees", new[] { "PayeeCategory_Id" });
            DropIndex("dbo.PayeeCategoryPayees", new[] { "Payee_Id" });
            CreateTable(
                "dbo.AdminBillStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.String(),
                        isApproved = c.Boolean(nullable: false),
                        isActive = c.Boolean(),
                        backcolor = c.String(),
                        forecolor = c.String(),
                        HierarchicalIndex = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.tabDepartment", "chartofAccountId", c => c.Int());
            AddColumn("dbo.JournalTransactions", "SaleReceiptId", c => c.Int());
            AddColumn("dbo.JournalTransactions", "deptId", c => c.Int());
            AddColumn("dbo.tabAdminBill", "template_Id", c => c.Int());
            AddColumn("dbo.tabAdminBill", "CreationDate", c => c.DateTime());
            AddColumn("dbo.tabAdminBill", "status_Id", c => c.Int());
            AddColumn("dbo.tabAdminBill", "COA_Id", c => c.Int());
            AddColumn("dbo.tabAdminBill", "industryType_Id", c => c.Int());
            AddColumn("dbo.tabAdminBill", "billNumber", c => c.String());
            AddColumn("dbo.tabAdminBill", "transactionGroupId", c => c.Int(nullable: false));
            CreateIndex("dbo.tabDepartment", "chartofAccountId");
            CreateIndex("dbo.JournalTransactions", "SaleReceiptId");
            CreateIndex("dbo.JournalTransactions", "deptId");
            CreateIndex("dbo.tabAdminBill", "template_Id");
            CreateIndex("dbo.tabAdminBill", "status_Id");
            CreateIndex("dbo.tabAdminBill", "COA_Id");
            CreateIndex("dbo.tabAdminBill", "industryType_Id");
            AddForeignKey("dbo.JournalTransactions", "deptId", "dbo.tabDepartment", "Id");
            AddForeignKey("dbo.JournalTransactions", "SaleReceiptId", "dbo.SalesReceipts", "Id");
            AddForeignKey("dbo.tabDepartment", "chartofAccountId", "dbo.ChartofAccounts", "Id");
            AddForeignKey("dbo.tabAdminBill", "status_Id", "dbo.AdminBillStatus", "Id");
            AddForeignKey("dbo.tabAdminBill", "COA_Id", "dbo.ChartofAccounts", "Id");
            AddForeignKey("dbo.tabAdminBill", "industryType_Id", "dbo.IndustryTypes", "Id");
            AddForeignKey("dbo.tabAdminBill", "template_Id", "dbo.Templates", "Id");
            DropColumn("dbo.ChartofAccounts", "deptId");
            DropColumn("dbo.tabAdminBill", "billTemplate");
            DropColumn("dbo.tabAdminBill", "COA");
            DropTable("dbo.PayeeCategoryPayees");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.PayeeCategoryPayees",
                c => new
                    {
                        PayeeCategory_Id = c.Int(nullable: false),
                        Payee_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.PayeeCategory_Id, t.Payee_Id });
            
            AddColumn("dbo.tabAdminBill", "COA", c => c.String());
            AddColumn("dbo.tabAdminBill", "billTemplate", c => c.Int(nullable: false));
            AddColumn("dbo.ChartofAccounts", "deptId", c => c.Int());
            DropForeignKey("dbo.tabAdminBill", "template_Id", "dbo.Templates");
            DropForeignKey("dbo.tabAdminBill", "industryType_Id", "dbo.IndustryTypes");
            DropForeignKey("dbo.tabAdminBill", "COA_Id", "dbo.ChartofAccounts");
            DropForeignKey("dbo.tabAdminBill", "status_Id", "dbo.AdminBillStatus");
            DropForeignKey("dbo.tabDepartment", "chartofAccountId", "dbo.ChartofAccounts");
            DropForeignKey("dbo.JournalTransactions", "SaleReceiptId", "dbo.SalesReceipts");
            DropForeignKey("dbo.JournalTransactions", "deptId", "dbo.tabDepartment");
            DropIndex("dbo.tabAdminBill", new[] { "industryType_Id" });
            DropIndex("dbo.tabAdminBill", new[] { "COA_Id" });
            DropIndex("dbo.tabAdminBill", new[] { "status_Id" });
            DropIndex("dbo.tabAdminBill", new[] { "template_Id" });
            DropIndex("dbo.JournalTransactions", new[] { "deptId" });
            DropIndex("dbo.JournalTransactions", new[] { "SaleReceiptId" });
            DropIndex("dbo.tabDepartment", new[] { "chartofAccountId" });
            DropColumn("dbo.tabAdminBill", "transactionGroupId");
            DropColumn("dbo.tabAdminBill", "billNumber");
            DropColumn("dbo.tabAdminBill", "industryType_Id");
            DropColumn("dbo.tabAdminBill", "COA_Id");
            DropColumn("dbo.tabAdminBill", "status_Id");
            DropColumn("dbo.tabAdminBill", "CreationDate");
            DropColumn("dbo.tabAdminBill", "template_Id");
            DropColumn("dbo.JournalTransactions", "deptId");
            DropColumn("dbo.JournalTransactions", "SaleReceiptId");
            DropColumn("dbo.tabDepartment", "chartofAccountId");
            DropTable("dbo.AdminBillStatus");
            CreateIndex("dbo.PayeeCategoryPayees", "Payee_Id");
            CreateIndex("dbo.PayeeCategoryPayees", "PayeeCategory_Id");
            CreateIndex("dbo.ChartofAccounts", "deptId");
            AddForeignKey("dbo.PayeeCategoryPayees", "Payee_Id", "dbo.Payees", "Id", cascadeDelete: true);
            AddForeignKey("dbo.PayeeCategoryPayees", "PayeeCategory_Id", "dbo.PayeeCategories", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ChartofAccounts", "deptId", "dbo.tabDepartment", "Id");
            RenameTable(name: "dbo.tabAdminBill", newName: "tabNewBills");
        }
    }
}
