namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChagnesInCashFlows : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CashFlows", "saleOrderSettingKey", c => c.String(unicode: false));
            AddColumn("dbo.CashFlows", "purchaseOrderSettingKey", c => c.String(unicode: false));
            AddColumn("dbo.CashFlows", "saleinvoiceSettingKey", c => c.String(unicode: false));
            AddColumn("dbo.CashFlows", "vendorBillSettingKey", c => c.String(unicode: false));
            AddColumn("dbo.CashFlows", "adminBillSettingKey", c => c.String(unicode: false));
            AddColumn("dbo.CashFlows", "stlSettingKey", c => c.String(unicode: false));
            AddColumn("dbo.CashFlows", "paymentSettingKey", c => c.String(unicode: false));
            AddColumn("dbo.CashFlows", "bankSettingKey", c => c.String(unicode: false));
            AddColumn("dbo.CashFlows", "loanSettingKey", c => c.String(unicode: false));
            AddColumn("dbo.CashFlows", "advanceSettingKey", c => c.String(unicode: false));
            AddColumn("dbo.CashFlows", "gridReportType", c => c.Int(nullable: false));
            AddColumn("dbo.CashFlows", "titleId", c => c.Int());
            CreateIndex("dbo.CashFlows", "titleId");
            AddForeignKey("dbo.CashFlows", "titleId", "dbo.ReportTitles", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CashFlows", "titleId", "dbo.ReportTitles");
            DropIndex("dbo.CashFlows", new[] { "titleId" });
            DropColumn("dbo.CashFlows", "titleId");
            DropColumn("dbo.CashFlows", "gridReportType");
            DropColumn("dbo.CashFlows", "advanceSettingKey");
            DropColumn("dbo.CashFlows", "loanSettingKey");
            DropColumn("dbo.CashFlows", "bankSettingKey");
            DropColumn("dbo.CashFlows", "paymentSettingKey");
            DropColumn("dbo.CashFlows", "stlSettingKey");
            DropColumn("dbo.CashFlows", "adminBillSettingKey");
            DropColumn("dbo.CashFlows", "vendorBillSettingKey");
            DropColumn("dbo.CashFlows", "saleinvoiceSettingKey");
            DropColumn("dbo.CashFlows", "purchaseOrderSettingKey");
            DropColumn("dbo.CashFlows", "saleOrderSettingKey");
        }
    }
}
