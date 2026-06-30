namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class isTitleAddedInProducts : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "isTitle", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "isTitle");
        }
    }
}
