namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class costFieldsSortID : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CostSheetFields", "SortId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CostSheetFields", "SortId");
        }
    }
}
