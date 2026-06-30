namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SupplierReferenceNoAddedInBills : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bills", "SupplierReferenceNo", c => c.String());
            AddColumn("dbo.Bills", "SupplyDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Bills", "SupplyDate");
            DropColumn("dbo.Bills", "SupplierReferenceNo");
        }
    }
}
