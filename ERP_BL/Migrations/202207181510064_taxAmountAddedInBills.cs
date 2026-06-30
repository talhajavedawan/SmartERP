namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class taxAmountAddedInBills : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bills", "taxAmount", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Bills", "taxAmount");
        }
    }
}
