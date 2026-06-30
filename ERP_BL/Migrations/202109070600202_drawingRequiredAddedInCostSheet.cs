namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class drawingRequiredAddedInCostSheet : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FieldValues", "drawingRequired", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.FieldValues", "drawingRequired");
        }
    }
}
