namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SummarySheetFeilds : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SummaryFieldValues",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FieldId = c.Int(nullable: false),
                        Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Type = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SummarySheetFields", t => t.FieldId, cascadeDelete: true)
                .Index(t => t.FieldId);
            
            CreateTable(
                "dbo.SummarySheetFields",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AddedbyUserId = c.Int(nullable: false),
                        Timestamp = c.DateTime(nullable: false),
                        Title = c.String(),
                        SortId = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                        isActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.AddedbyUserId, cascadeDelete: true)
                .Index(t => t.AddedbyUserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SummaryFieldValues", "FieldId", "dbo.SummarySheetFields");
            DropForeignKey("dbo.SummarySheetFields", "AddedbyUserId", "dbo.Users");
            DropIndex("dbo.SummarySheetFields", new[] { "AddedbyUserId" });
            DropIndex("dbo.SummaryFieldValues", new[] { "FieldId" });
            DropTable("dbo.SummarySheetFields");
            DropTable("dbo.SummaryFieldValues");
        }
    }
}
