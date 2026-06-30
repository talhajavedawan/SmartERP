namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeInReceiptCOAsAndSalesReceipts : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ReceiptCOAs", "COAcredit_Id", "dbo.ChartofAccounts");
            DropForeignKey("dbo.ReceiptCOAs", "COAdebit_Id", "dbo.ChartofAccounts");
            DropForeignKey("dbo.ReceiptCOAs", "SalesReceipt_Id", "dbo.SalesReceipts");
            DropIndex("dbo.ReceiptCOAs", new[] { "COAcredit_Id" });
            DropIndex("dbo.ReceiptCOAs", new[] { "COAdebit_Id" });
            DropIndex("dbo.ReceiptCOAs", new[] { "SalesReceipt_Id" });
            AddColumn("dbo.BudgetSystemCostFields", "Payment_Id", c => c.Int());
            AddColumn("dbo.BudgetSystemCostFields", "SalesReceipt_Id", c => c.Int());
            AddColumn("dbo.SalesReceipts", "COAcredit_Id", c => c.Int());
            AddColumn("dbo.SalesReceipts", "COAdebit_Id", c => c.Int());
            AddColumn("dbo.SalesReceipts", "Description", c => c.String());
            CreateIndex("dbo.BudgetSystemCostFields", "Payment_Id");
            CreateIndex("dbo.BudgetSystemCostFields", "SalesReceipt_Id");
            CreateIndex("dbo.SalesReceipts", "COAcredit_Id");
            CreateIndex("dbo.SalesReceipts", "COAdebit_Id");
            AddForeignKey("dbo.BudgetSystemCostFields", "Payment_Id", "dbo.Payments", "Id");
            AddForeignKey("dbo.BudgetSystemCostFields", "SalesReceipt_Id", "dbo.SalesReceipts", "Id");
            AddForeignKey("dbo.SalesReceipts", "COAcredit_Id", "dbo.ChartofAccounts", "Id");
            AddForeignKey("dbo.SalesReceipts", "COAdebit_Id", "dbo.ChartofAccounts", "Id");
            DropColumn("dbo.ReceiptCOAs", "COAcredit_Id");
            DropColumn("dbo.ReceiptCOAs", "COAdebit_Id");
            DropColumn("dbo.ReceiptCOAs", "SalesReceipt_Id");
            DropColumn("dbo.ReceiptCOAs", "Amount");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ReceiptCOAs", "Amount", c => c.Double(nullable: false));
            AddColumn("dbo.ReceiptCOAs", "SalesReceipt_Id", c => c.Int());
            AddColumn("dbo.ReceiptCOAs", "COAdebit_Id", c => c.Int());
            AddColumn("dbo.ReceiptCOAs", "COAcredit_Id", c => c.Int());
            DropForeignKey("dbo.SalesReceipts", "COAdebit_Id", "dbo.ChartofAccounts");
            DropForeignKey("dbo.SalesReceipts", "COAcredit_Id", "dbo.ChartofAccounts");
            DropForeignKey("dbo.BudgetSystemCostFields", "SalesReceipt_Id", "dbo.SalesReceipts");
            DropForeignKey("dbo.BudgetSystemCostFields", "Payment_Id", "dbo.Payments");
            DropIndex("dbo.SalesReceipts", new[] { "COAdebit_Id" });
            DropIndex("dbo.SalesReceipts", new[] { "COAcredit_Id" });
            DropIndex("dbo.BudgetSystemCostFields", new[] { "SalesReceipt_Id" });
            DropIndex("dbo.BudgetSystemCostFields", new[] { "Payment_Id" });
            DropColumn("dbo.SalesReceipts", "Description");
            DropColumn("dbo.SalesReceipts", "COAdebit_Id");
            DropColumn("dbo.SalesReceipts", "COAcredit_Id");
            DropColumn("dbo.BudgetSystemCostFields", "SalesReceipt_Id");
            DropColumn("dbo.BudgetSystemCostFields", "Payment_Id");
            CreateIndex("dbo.ReceiptCOAs", "SalesReceipt_Id");
            CreateIndex("dbo.ReceiptCOAs", "COAdebit_Id");
            CreateIndex("dbo.ReceiptCOAs", "COAcredit_Id");
            AddForeignKey("dbo.ReceiptCOAs", "SalesReceipt_Id", "dbo.SalesReceipts", "Id");
            AddForeignKey("dbo.ReceiptCOAs", "COAdebit_Id", "dbo.ChartofAccounts", "Id");
            AddForeignKey("dbo.ReceiptCOAs", "COAcredit_Id", "dbo.ChartofAccounts", "Id");
        }
    }
}
