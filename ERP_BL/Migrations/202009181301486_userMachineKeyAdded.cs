namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userMachineKeyAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "isKeyApproved", c => c.Boolean(nullable: false));
            AddColumn("dbo.Users", "machineKey", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "machineKey");
            DropColumn("dbo.Users", "isKeyApproved");
        }
    }
}
