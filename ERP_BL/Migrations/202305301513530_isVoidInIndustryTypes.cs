namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class isVoidInIndustryTypes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.IndustryTypes", "isVoid", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.IndustryTypes", "isVoid");
        }
    }
}
