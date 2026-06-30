namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changesInAuditYearAdjustments : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AuditYearAdjustments", "BudgetCost", c => c.Double(nullable: false));
            AddColumn("dbo.AuditYearAdjustments", "ActualCost", c => c.Double(nullable: false));
            AddColumn("dbo.AuditYearAdjustments", "SystemCost", c => c.Double(nullable: false));
            AddColumn("dbo.AuditYearAdjustments", "BudgetCostAudit", c => c.Double(nullable: false));
            AddColumn("dbo.AuditYearAdjustments", "ActualCostAudit", c => c.Double(nullable: false));
            AddColumn("dbo.AuditYearAdjustments", "SystemCostAudit", c => c.Double(nullable: false));
            AddColumn("dbo.SaleInvoices", "BatchRefrenceNo", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SaleInvoices", "BatchRefrenceNo");
            DropColumn("dbo.AuditYearAdjustments", "SystemCostAudit");
            DropColumn("dbo.AuditYearAdjustments", "ActualCostAudit");
            DropColumn("dbo.AuditYearAdjustments", "BudgetCostAudit");
            DropColumn("dbo.AuditYearAdjustments", "SystemCost");
            DropColumn("dbo.AuditYearAdjustments", "ActualCost");
            DropColumn("dbo.AuditYearAdjustments", "BudgetCost");
        }
    }
}
