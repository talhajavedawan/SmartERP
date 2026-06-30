namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class rentalBasisAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assets", "rentalBasis", c => c.Int(nullable: false));
            AddColumn("dbo.Mortgagees", "isActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.Lesees", "isActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.Manufacturers", "isActive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Manufacturers", "isActive");
            DropColumn("dbo.Lesees", "isActive");
            DropColumn("dbo.Mortgagees", "isActive");
            DropColumn("dbo.Assets", "rentalBasis");
        }
    }
}
