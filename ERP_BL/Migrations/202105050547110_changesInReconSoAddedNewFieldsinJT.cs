namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changesInReconSoAddedNewFieldsinJT : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JournalTransactions", "reconcilationDate", c => c.DateTime());
            AddColumn("dbo.SaleOrders", "deliveryDateFinal", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SaleOrders", "deliveryDateFinal");
            DropColumn("dbo.JournalTransactions", "reconcilationDate");
        }
    }
}
