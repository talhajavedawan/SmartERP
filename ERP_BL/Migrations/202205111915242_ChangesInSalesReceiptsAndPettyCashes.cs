namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesInSalesReceiptsAndPettyCashes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SalesReceipts", "isDeposit", c => c.Boolean());
            AddColumn("dbo.SalesReceipts", "isAmountOC", c => c.Boolean());
            AddColumn("dbo.SalesReceipts", "PettyCashRefId", c => c.Int());
            AddColumn("dbo.PettyCashes", "SaleReceiptId", c => c.Int());
            CreateIndex("dbo.SalesReceipts", "PettyCashRefId");
            CreateIndex("dbo.PettyCashes", "SaleReceiptId");
            AddForeignKey("dbo.PettyCashes", "SaleReceiptId", "dbo.SalesReceipts", "Id");
            AddForeignKey("dbo.SalesReceipts", "PettyCashRefId", "dbo.BillRefNumbers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SalesReceipts", "PettyCashRefId", "dbo.BillRefNumbers");
            DropForeignKey("dbo.PettyCashes", "SaleReceiptId", "dbo.SalesReceipts");
            DropIndex("dbo.PettyCashes", new[] { "SaleReceiptId" });
            DropIndex("dbo.SalesReceipts", new[] { "PettyCashRefId" });
            DropColumn("dbo.PettyCashes", "SaleReceiptId");
            DropColumn("dbo.SalesReceipts", "PettyCashRefId");
            DropColumn("dbo.SalesReceipts", "isAmountOC");
            DropColumn("dbo.SalesReceipts", "isDeposit");
        }
    }
}
