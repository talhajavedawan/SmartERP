namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LoginTimeAddedInUsers : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "LoginTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "LoginTime");
        }
    }
}
