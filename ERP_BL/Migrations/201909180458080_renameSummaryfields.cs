namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class renameSummaryfields : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.CommissionSummarySheets", "Offerpaymentterm", c => c.String());
            //AddColumn("dbo.CommissionSummarySheets", "Offertranshipment", c => c.Boolean(nullable: false));
            //DropColumn("dbo.CommissionSummarySheets", "paymenttermWithSupplier");
        }
        
        public override void Down()
        {
            //    AddColumn("dbo.CommissionSummarySheets", "paymenttermWithSupplier", c => c.String());
            //    DropColumn("dbo.CommissionSummarySheets", "Offertranshipment");
            //    DropColumn("dbo.CommissionSummarySheets", "Offerpaymentterm");
        }
    }
}
