namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UnitPriceIncoterm : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Offers", "TitleValue1Id", c => c.Int());
            AddColumn("dbo.Offers", "TitleValue2Id", c => c.Int());
            AddColumn("dbo.ProcurementProducts", "unitPrice", c => c.Double(nullable: false));
            AddColumn("dbo.PurchaseOrders", "TitleValue1Id", c => c.Int());
            AddColumn("dbo.PurchaseOrders", "TitleValue2Id", c => c.Int());
            CreateIndex("dbo.Offers", "TitleValue1Id");
            CreateIndex("dbo.Offers", "TitleValue2Id");
            CreateIndex("dbo.PurchaseOrders", "TitleValue1Id");
            CreateIndex("dbo.PurchaseOrders", "TitleValue2Id");
            AddForeignKey("dbo.Offers", "TitleValue1Id", "dbo.Incoterms", "Id");
            AddForeignKey("dbo.Offers", "TitleValue2Id", "dbo.Incoterms", "Id");
            AddForeignKey("dbo.PurchaseOrders", "TitleValue1Id", "dbo.Incoterms", "Id");
            AddForeignKey("dbo.PurchaseOrders", "TitleValue2Id", "dbo.Incoterms", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PurchaseOrders", "TitleValue2Id", "dbo.Incoterms");
            DropForeignKey("dbo.PurchaseOrders", "TitleValue1Id", "dbo.Incoterms");
            DropForeignKey("dbo.Offers", "TitleValue2Id", "dbo.Incoterms");
            DropForeignKey("dbo.Offers", "TitleValue1Id", "dbo.Incoterms");
            DropIndex("dbo.PurchaseOrders", new[] { "TitleValue2Id" });
            DropIndex("dbo.PurchaseOrders", new[] { "TitleValue1Id" });
            DropIndex("dbo.Offers", new[] { "TitleValue2Id" });
            DropIndex("dbo.Offers", new[] { "TitleValue1Id" });
            DropColumn("dbo.PurchaseOrders", "TitleValue2Id");
            DropColumn("dbo.PurchaseOrders", "TitleValue1Id");
            DropColumn("dbo.ProcurementProducts", "unitPrice");
            DropColumn("dbo.Offers", "TitleValue2Id");
            DropColumn("dbo.Offers", "TitleValue1Id");
        }
    }
}
