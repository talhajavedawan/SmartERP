namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changesInCostSheet : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CostSheetFields", "Maker", c => c.String());
            AddColumn("dbo.CostSheetFields", "Origin", c => c.String());
            AddColumn("dbo.CostSheetFields", "Packing", c => c.String());
            AddColumn("dbo.FieldValues", "stringValue", c => c.String());
            AddColumn("dbo.FieldValues", "dateValue", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.FieldValues", "dateValue");
            DropColumn("dbo.FieldValues", "stringValue");
            DropColumn("dbo.CostSheetFields", "Packing");
            DropColumn("dbo.CostSheetFields", "Origin");
            DropColumn("dbo.CostSheetFields", "Maker");
        }
    }
}
