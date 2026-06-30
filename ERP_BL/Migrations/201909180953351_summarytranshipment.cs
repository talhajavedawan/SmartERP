namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class summarytranshipment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SaleOrders", "deliveryTerm", c => c.String());
            AddColumn("dbo.CommissionSummarySheets", "SOCommission", c => c.Double(nullable: false));
            //AddColumn("dbo.CommissionSummarySheets", "Offerpaymentterm", c => c.String());
            //AddColumn("dbo.CommissionSummarySheets", "Offertranshipment", c => c.Boolean());
            //DropColumn("dbo.CommissionSummarySheets", "paymenttermWithSupplier");
        }
        
        public override void Down()
        {
            //AddColumn("dbo.CommissionSummarySheets", "paymenttermWithSupplier", c => c.String());
            //DropColumn("dbo.CommissionSummarySheets", "Offertranshipment");
            //DropColumn("dbo.CommissionSummarySheets", "Offerpaymentterm");
            DropColumn("dbo.CommissionSummarySheets", "SOCommission");
            DropColumn("dbo.SaleOrders", "deliveryTerm");
        }
    }
}
