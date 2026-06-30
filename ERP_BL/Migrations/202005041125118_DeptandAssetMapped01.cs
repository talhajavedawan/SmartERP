namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeptandAssetMapped01 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OfficialAuths", "isActive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OfficialAuths", "isActive");
        }
    }
}
