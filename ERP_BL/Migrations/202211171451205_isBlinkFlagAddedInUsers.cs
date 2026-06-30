namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class isBlinkFlagAddedInUsers : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "isBlink", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "isBlink");
        }
    }
}
