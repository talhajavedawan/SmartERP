namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changesInPaymentsAndDepartments : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tabDepartment", "accountPayableId", c => c.Int());
            AddColumn("dbo.JournalTransactions", "PaymentId", c => c.Int());
            AddColumn("dbo.Payments", "paymentPurchaseInvoiceTemplate", c => c.Int());
            AddColumn("dbo.Payments", "PInvoice_Id", c => c.Int());
            AddColumn("dbo.Products", "cgsAccount_id", c => c.Int());
            AddColumn("dbo.tabBackground", "PopupText", c => c.String());
            AddColumn("dbo.tabBackground", "isSharedAll", c => c.Boolean(nullable: false));
            CreateIndex("dbo.tabDepartment", "accountPayableId");
            CreateIndex("dbo.JournalTransactions", "PaymentId");
            CreateIndex("dbo.Payments", "PInvoice_Id");
            CreateIndex("dbo.Products", "cgsAccount_id");
            AddForeignKey("dbo.JournalTransactions", "PaymentId", "dbo.Payments", "Id");
            AddForeignKey("dbo.Products", "cgsAccount_id", "dbo.ChartofAccounts", "Id");
            AddForeignKey("dbo.Payments", "PInvoice_Id", "dbo.PurchaseInvoices", "Id");
            AddForeignKey("dbo.tabDepartment", "accountPayableId", "dbo.ChartofAccounts", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tabDepartment", "accountPayableId", "dbo.ChartofAccounts");
            DropForeignKey("dbo.Payments", "PInvoice_Id", "dbo.PurchaseInvoices");
            DropForeignKey("dbo.Products", "cgsAccount_id", "dbo.ChartofAccounts");
            DropForeignKey("dbo.JournalTransactions", "PaymentId", "dbo.Payments");
            DropIndex("dbo.Products", new[] { "cgsAccount_id" });
            DropIndex("dbo.Payments", new[] { "PInvoice_Id" });
            DropIndex("dbo.JournalTransactions", new[] { "PaymentId" });
            DropIndex("dbo.tabDepartment", new[] { "accountPayableId" });
            DropColumn("dbo.tabBackground", "isSharedAll");
            DropColumn("dbo.tabBackground", "PopupText");
            DropColumn("dbo.Products", "cgsAccount_id");
            DropColumn("dbo.Payments", "PInvoice_Id");
            DropColumn("dbo.Payments", "paymentPurchaseInvoiceTemplate");
            DropColumn("dbo.JournalTransactions", "PaymentId");
            DropColumn("dbo.tabDepartment", "accountPayableId");
        }
    }
}
