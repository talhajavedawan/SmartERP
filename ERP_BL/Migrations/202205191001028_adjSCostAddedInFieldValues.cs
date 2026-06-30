namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class adjSCostAddedInFieldValues : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FieldValues", "adjSCost", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.FieldValues", "adjSCost");
        }
    }
}
