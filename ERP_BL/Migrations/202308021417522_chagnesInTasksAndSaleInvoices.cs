namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class chagnesInTasksAndSaleInvoices : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TaskGroups", "isTitle", c => c.Boolean(nullable: false));
            AddColumn("dbo.Tasks", "TaxQuarter", c => c.Int());
            AddColumn("dbo.SaleInvoices", "isAdvancePayment", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SaleInvoices", "isAdvancePayment");
            DropColumn("dbo.Tasks", "TaxQuarter");
            DropColumn("dbo.TaskGroups", "isTitle");
        }
    }
}
