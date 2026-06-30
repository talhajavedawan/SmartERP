namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesInProcurementProductsAndSaleInvoices : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProcurementProducts", "DispatchedQuantity", c => c.Double(nullable: false));
            AddColumn("dbo.ProcurementProducts", "DispatchedWeight", c => c.Double(nullable: false));
            AddColumn("dbo.SaleInvoices", "LastStatusClassChangeDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SaleInvoices", "LastStatusClassChangeDate");
            DropColumn("dbo.ProcurementProducts", "DispatchedWeight");
            DropColumn("dbo.ProcurementProducts", "DispatchedQuantity");
        }
    }
}
