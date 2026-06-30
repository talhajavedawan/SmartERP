namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CostSheetSaleReceiptFields : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CostSheetSaleReceiptFields",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FieldId = c.Int(nullable: false),
                        CostSheetId = c.Int(nullable: false),
                        Receipt_Id = c.Int(nullable: false),
                        Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FieldType = c.Int(nullable: false),
                        timeStamp = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CostSheets", t => t.CostSheetId, cascadeDelete: true)
                .ForeignKey("dbo.CostSheetFields", t => t.FieldId, cascadeDelete: true)
                .Index(t => t.FieldId)
                .Index(t => t.CostSheetId);
            
            AddColumn("dbo.SalesReceipts", "CostSheet_Id", c => c.Int());
            AddColumn("dbo.SalesReceipts", "costSheetFieldId", c => c.Int());
            CreateIndex("dbo.SalesReceipts", "CostSheet_Id");
            AddForeignKey("dbo.SalesReceipts", "CostSheet_Id", "dbo.CostSheets", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SalesReceipts", "CostSheet_Id", "dbo.CostSheets");
            DropForeignKey("dbo.CostSheetSaleReceiptFields", "FieldId", "dbo.CostSheetFields");
            DropForeignKey("dbo.CostSheetSaleReceiptFields", "CostSheetId", "dbo.CostSheets");
            DropIndex("dbo.SalesReceipts", new[] { "CostSheet_Id" });
            DropIndex("dbo.CostSheetSaleReceiptFields", new[] { "CostSheetId" });
            DropIndex("dbo.CostSheetSaleReceiptFields", new[] { "FieldId" });
            DropColumn("dbo.SalesReceipts", "costSheetFieldId");
            DropColumn("dbo.SalesReceipts", "CostSheet_Id");
            DropTable("dbo.CostSheetSaleReceiptFields");
        }
    }
}
