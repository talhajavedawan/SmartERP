namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesInTargetGroups : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TargetGroups", "forStep", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TargetGroups", "forStep");
        }
    }
}
