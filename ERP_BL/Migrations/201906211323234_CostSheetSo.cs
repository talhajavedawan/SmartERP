namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CostSheetSo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CostSheets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Timestamp = c.DateTime(nullable: false),
                        Info = c.String(),
                        TransactionType = c.Int(nullable: false),
                        TransactionId = c.Int(nullable: false),
                        TotalBudgetedMargin = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalActualMargin = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.FieldValues",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FieldId = c.Int(nullable: false),
                        Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Type = c.Int(nullable: false),
                        CostSheet_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CostSheetFields", t => t.FieldId, cascadeDelete: true)
                .ForeignKey("dbo.CostSheets", t => t.CostSheet_Id)
                .Index(t => t.FieldId)
                .Index(t => t.CostSheet_Id);
            
            CreateTable(
                "dbo.CostSheetFields",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AddedbyUserId = c.Int(nullable: false),
                        Timestamp = c.DateTime(nullable: false),
                        Title = c.String(),
                        Type = c.Int(nullable: false),
                        isActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.AddedbyUserId, cascadeDelete: true)
                .Index(t => t.AddedbyUserId);
            
            CreateTable(
                "dbo.ViewInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        Timestamp = c.DateTime(nullable: false),
                        Info = c.String(),
                        TransactionType = c.Int(nullable: false),
                        TransactionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            AddColumn("dbo.tabPerson", "Photo", c => c.Binary());
            AddColumn("dbo.SaleOrders", "CostSheet_Id", c => c.Int());
            AddColumn("dbo.Inquiries", "isReviewed", c => c.Boolean());
            AddColumn("dbo.Inquiries", "needReview", c => c.Boolean());
            CreateIndex("dbo.SaleOrders", "CostSheet_Id");
            AddForeignKey("dbo.SaleOrders", "CostSheet_Id", "dbo.CostSheets", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ViewInfoes", "UserId", "dbo.Users");
            DropForeignKey("dbo.SaleOrders", "CostSheet_Id", "dbo.CostSheets");
            DropForeignKey("dbo.FieldValues", "CostSheet_Id", "dbo.CostSheets");
            DropForeignKey("dbo.FieldValues", "FieldId", "dbo.CostSheetFields");
            DropForeignKey("dbo.CostSheetFields", "AddedbyUserId", "dbo.Users");
            DropIndex("dbo.ViewInfoes", new[] { "UserId" });
            DropIndex("dbo.CostSheetFields", new[] { "AddedbyUserId" });
            DropIndex("dbo.FieldValues", new[] { "CostSheet_Id" });
            DropIndex("dbo.FieldValues", new[] { "FieldId" });
            DropIndex("dbo.SaleOrders", new[] { "CostSheet_Id" });
            DropColumn("dbo.Inquiries", "needReview");
            DropColumn("dbo.Inquiries", "isReviewed");
            DropColumn("dbo.SaleOrders", "CostSheet_Id");
            DropColumn("dbo.tabPerson", "Photo");
            DropTable("dbo.ViewInfoes");
            DropTable("dbo.CostSheetFields");
            DropTable("dbo.FieldValues");
            DropTable("dbo.CostSheets");
        }
    }
}
