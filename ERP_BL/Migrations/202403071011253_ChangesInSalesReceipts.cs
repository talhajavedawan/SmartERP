namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesInSalesReceipts : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.SalesReceipts", name: "collectionMethod_Id", newName: "collectionMethodId");
            RenameColumn(table: "dbo.SalesReceipts", name: "company_Id", newName: "companyId");
            RenameColumn(table: "dbo.SalesReceipts", name: "Currency_Id", newName: "CurrencyId");
            RenameColumn(table: "dbo.SalesReceipts", name: "Customer_Id", newName: "CustomerId");
            RenameColumn(table: "dbo.SalesReceipts", name: "department_Id", newName: "deptId");
            RenameColumn(table: "dbo.SalesReceipts", name: "saleReceiptStatus_Id", newName: "StatusId");
            RenameIndex(table: "dbo.SalesReceipts", name: "IX_company_Id", newName: "IX_companyId");
            RenameIndex(table: "dbo.SalesReceipts", name: "IX_department_Id", newName: "IX_deptId");
            RenameIndex(table: "dbo.SalesReceipts", name: "IX_Customer_Id", newName: "IX_CustomerId");
            RenameIndex(table: "dbo.SalesReceipts", name: "IX_Currency_Id", newName: "IX_CurrencyId");
            RenameIndex(table: "dbo.SalesReceipts", name: "IX_collectionMethod_Id", newName: "IX_collectionMethodId");
            RenameIndex(table: "dbo.SalesReceipts", name: "IX_saleReceiptStatus_Id", newName: "IX_StatusId");
            AddColumn("dbo.SalesReceipts", "customerCreditId", c => c.Int());
            CreateIndex("dbo.SalesReceipts", "customerCreditId");
            AddForeignKey("dbo.SalesReceipts", "customerCreditId", "dbo.CustomerCredits", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SalesReceipts", "customerCreditId", "dbo.CustomerCredits");
            DropIndex("dbo.SalesReceipts", new[] { "customerCreditId" });
            DropColumn("dbo.SalesReceipts", "customerCreditId");
            RenameIndex(table: "dbo.SalesReceipts", name: "IX_StatusId", newName: "IX_saleReceiptStatus_Id");
            RenameIndex(table: "dbo.SalesReceipts", name: "IX_collectionMethodId", newName: "IX_collectionMethod_Id");
            RenameIndex(table: "dbo.SalesReceipts", name: "IX_CurrencyId", newName: "IX_Currency_Id");
            RenameIndex(table: "dbo.SalesReceipts", name: "IX_CustomerId", newName: "IX_Customer_Id");
            RenameIndex(table: "dbo.SalesReceipts", name: "IX_deptId", newName: "IX_department_Id");
            RenameIndex(table: "dbo.SalesReceipts", name: "IX_companyId", newName: "IX_company_Id");
            RenameColumn(table: "dbo.SalesReceipts", name: "StatusId", newName: "saleReceiptStatus_Id");
            RenameColumn(table: "dbo.SalesReceipts", name: "deptId", newName: "department_Id");
            RenameColumn(table: "dbo.SalesReceipts", name: "CustomerId", newName: "Customer_Id");
            RenameColumn(table: "dbo.SalesReceipts", name: "CurrencyId", newName: "Currency_Id");
            RenameColumn(table: "dbo.SalesReceipts", name: "companyId", newName: "company_Id");
            RenameColumn(table: "dbo.SalesReceipts", name: "collectionMethodId", newName: "collectionMethod_Id");
        }
    }
}
