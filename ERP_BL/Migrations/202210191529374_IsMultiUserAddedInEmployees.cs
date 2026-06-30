namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IsMultiUserAddedInEmployees : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employees", "isMultiUser", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Employees", "isMultiUser");
        }
    }
}
