namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VehicleRegnoAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Vehicles", "RegNo", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Vehicles", "RegNo");
        }
    }
}
