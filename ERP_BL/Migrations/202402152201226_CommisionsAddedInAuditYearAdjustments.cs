namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CommisionsAddedInAuditYearAdjustments : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AuditYearAdjustments", "comissionOC", c => c.Double(nullable: false));
            AddColumn("dbo.AuditYearAdjustments", "netComissionOC", c => c.Double(nullable: false));
            AddColumn("dbo.AuditYearAdjustments", "comissionAudit", c => c.Double(nullable: false));
            AddColumn("dbo.AuditYearAdjustments", "netComissionAudit", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AuditYearAdjustments", "netComissionAudit");
            DropColumn("dbo.AuditYearAdjustments", "comissionAudit");
            DropColumn("dbo.AuditYearAdjustments", "netComissionOC");
            DropColumn("dbo.AuditYearAdjustments", "comissionOC");
        }
    }
}
