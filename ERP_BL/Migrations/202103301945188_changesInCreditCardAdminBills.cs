namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changesInCreditCardAdminBills : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.CreditCards", name: "cardHolder_Id", newName: "PrimaryCardHolderId");
            RenameColumn(table: "dbo.CreditCards", name: "ParentID", newName: "SecondaryCardHolderId");
            RenameColumn(table: "dbo.tabAdminBill", name: "CreditCardNoId", newName: "PrimaryCreditCardNoId");
            RenameIndex(table: "dbo.tabAdminBill", name: "IX_CreditCardNoId", newName: "IX_PrimaryCreditCardNoId");
            RenameIndex(table: "dbo.CreditCards", name: "IX_cardHolder_Id", newName: "IX_PrimaryCardHolderId");
            RenameIndex(table: "dbo.CreditCards", name: "IX_ParentID", newName: "IX_SecondaryCardHolderId");
            CreateTable(
                "dbo.CreditCardDepartments",
                c => new
                    {
                        CreditCard_Id = c.Int(nullable: false),
                        Department_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CreditCard_Id, t.Department_Id })
                .ForeignKey("dbo.CreditCards", t => t.CreditCard_Id, cascadeDelete: true)
                .ForeignKey("dbo.tabDepartment", t => t.Department_Id, cascadeDelete: true)
                .Index(t => t.CreditCard_Id)
                .Index(t => t.Department_Id);
            
            AddColumn("dbo.tabAdminBill", "BankId", c => c.Int());
            AddColumn("dbo.CreditCards", "CompanyId", c => c.Int());
            AddColumn("dbo.CreditCards", "PrimaryCardNumber", c => c.String());
            CreateIndex("dbo.tabAdminBill", "BankId");
            CreateIndex("dbo.CreditCards", "CompanyId");
            AddForeignKey("dbo.tabAdminBill", "BankId", "dbo.Banks", "Id");
            AddForeignKey("dbo.CreditCards", "CompanyId", "dbo.tabCompany", "Id");
            DropColumn("dbo.AdminBillTypes", "COA_AccountType");
            DropColumn("dbo.CreditCards", "CardNumber");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CreditCards", "CardNumber", c => c.String());
            AddColumn("dbo.AdminBillTypes", "COA_AccountType", c => c.Int(nullable: false));
            DropForeignKey("dbo.CreditCardDepartments", "Department_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.CreditCardDepartments", "CreditCard_Id", "dbo.CreditCards");
            DropForeignKey("dbo.CreditCards", "CompanyId", "dbo.tabCompany");
            DropForeignKey("dbo.tabAdminBill", "BankId", "dbo.Banks");
            DropIndex("dbo.CreditCardDepartments", new[] { "Department_Id" });
            DropIndex("dbo.CreditCardDepartments", new[] { "CreditCard_Id" });
            DropIndex("dbo.CreditCards", new[] { "CompanyId" });
            DropIndex("dbo.tabAdminBill", new[] { "BankId" });
            DropColumn("dbo.CreditCards", "PrimaryCardNumber");
            DropColumn("dbo.CreditCards", "CompanyId");
            DropColumn("dbo.tabAdminBill", "BankId");
            DropTable("dbo.CreditCardDepartments");
            RenameIndex(table: "dbo.CreditCards", name: "IX_SecondaryCardHolderId", newName: "IX_ParentID");
            RenameIndex(table: "dbo.CreditCards", name: "IX_PrimaryCardHolderId", newName: "IX_cardHolder_Id");
            RenameIndex(table: "dbo.tabAdminBill", name: "IX_PrimaryCreditCardNoId", newName: "IX_CreditCardNoId");
            RenameColumn(table: "dbo.tabAdminBill", name: "PrimaryCreditCardNoId", newName: "CreditCardNoId");
            RenameColumn(table: "dbo.CreditCards", name: "SecondaryCardHolderId", newName: "ParentID");
            RenameColumn(table: "dbo.CreditCards", name: "PrimaryCardHolderId", newName: "cardHolder_Id");
        }
    }
}
