namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class isVoidAddedInTargetRewards : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TargetRewards", "isVoid", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TargetRewards", "isVoid");
        }
    }
}
