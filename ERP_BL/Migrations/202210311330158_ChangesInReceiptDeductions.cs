namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesInReceiptDeductions : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.ReceiptDeductions", name: "SalesReceipt_Id", newName: "SalesReceiptForDeductionId");
            RenameIndex(table: "dbo.ReceiptDeductions", name: "IX_SalesReceipt_Id", newName: "IX_SalesReceiptForDeductionId");
            AddColumn("dbo.ReceiptDeductions", "SalesReceiptForBankChargesId", c => c.Int());
            CreateIndex("dbo.ReceiptDeductions", "SalesReceiptForBankChargesId");
            AddForeignKey("dbo.ReceiptDeductions", "SalesReceiptForBankChargesId", "dbo.SalesReceipts", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ReceiptDeductions", "SalesReceiptForBankChargesId", "dbo.SalesReceipts");
            DropIndex("dbo.ReceiptDeductions", new[] { "SalesReceiptForBankChargesId" });
            DropColumn("dbo.ReceiptDeductions", "SalesReceiptForBankChargesId");
            RenameIndex(table: "dbo.ReceiptDeductions", name: "IX_SalesReceiptForDeductionId", newName: "IX_SalesReceipt_Id");
            RenameColumn(table: "dbo.ReceiptDeductions", name: "SalesReceiptForDeductionId", newName: "SalesReceipt_Id");
        }
    }
}
