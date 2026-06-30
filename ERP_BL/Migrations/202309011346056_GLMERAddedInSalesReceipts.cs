namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GLMERAddedInSalesReceipts : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SalesReceipts", "GLMER", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SalesReceipts", "GLMER");
        }
    }
}
