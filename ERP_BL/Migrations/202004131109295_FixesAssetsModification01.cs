namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixesAssetsModification01 : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.Assets", "isSubsidary", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            //DropColumn("dbo.Assets", "isSubsidary");
        }
    }
}
