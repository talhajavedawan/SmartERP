namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class lotNoAddedInTasksAndSalesInvoices : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tasks", "LotNo", c => c.String());
            AddColumn("dbo.SaleInvoices", "lotNo", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SaleInvoices", "lotNo");
            DropColumn("dbo.Tasks", "LotNo");
        }
    }
}
