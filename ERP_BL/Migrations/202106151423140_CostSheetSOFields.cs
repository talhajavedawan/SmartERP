namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CostSheetSOFields : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CostSheetSOFields",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FieldId = c.Int(nullable: false),
                        CostSheetId = c.Int(nullable: false),
                        SO_Id = c.Int(nullable: false),
                        Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FieldType = c.Int(nullable: false),
                        timeStamp = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CostSheets", t => t.CostSheetId, cascadeDelete: true)
                .ForeignKey("dbo.CostSheetFields", t => t.FieldId, cascadeDelete: true)
                .Index(t => t.FieldId)
                .Index(t => t.CostSheetId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CostSheetSOFields", "FieldId", "dbo.CostSheetFields");
            DropForeignKey("dbo.CostSheetSOFields", "CostSheetId", "dbo.CostSheets");
            DropIndex("dbo.CostSheetSOFields", new[] { "CostSheetId" });
            DropIndex("dbo.CostSheetSOFields", new[] { "FieldId" });
            DropTable("dbo.CostSheetSOFields");
        }
    }
}
