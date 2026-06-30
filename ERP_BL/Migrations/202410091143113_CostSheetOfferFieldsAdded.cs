namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CostSheetOfferFieldsAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CostSheetOfferFields",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FieldId = c.Int(nullable: false),
                        CostSheetId = c.Int(nullable: false),
                        Offer_Id = c.Int(nullable: false),
                        Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FieldType = c.Int(nullable: false),
                        timeStamp = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CostSheets", t => t.CostSheetId, cascadeDelete: true)
                .ForeignKey("dbo.CostSheetFields", t => t.FieldId, cascadeDelete: true)
                .Index(t => t.FieldId)
                .Index(t => t.CostSheetId);
            
            AddColumn("dbo.Offers", "CostSheet_Id", c => c.Int());
            AddColumn("dbo.Offers", "costCenterCurrencyId", c => c.Int());
            AddColumn("dbo.Offers", "SER", c => c.Double(nullable: false));
            AddColumn("dbo.Offers", "MER", c => c.Double(nullable: false));
            AddColumn("dbo.Offers", "totalOfferAmount", c => c.Double(nullable: false));
            AddColumn("dbo.Offers", "totalOfferAmountSER", c => c.Double(nullable: false));
            AddColumn("dbo.Offers", "totalOfferAmountMER", c => c.Double(nullable: false));
            AddColumn("dbo.Offers", "totalbudgetCost", c => c.Double(nullable: false));
            AddColumn("dbo.Offers", "budgetMarginSER", c => c.Double(nullable: false));
            AddColumn("dbo.Offers", "budgetMarginMER", c => c.Double(nullable: false));
            CreateIndex("dbo.Offers", "CostSheet_Id");
            CreateIndex("dbo.Offers", "costCenterCurrencyId");
            AddForeignKey("dbo.Offers", "costCenterCurrencyId", "dbo.Currencies", "Id");
            AddForeignKey("dbo.Offers", "CostSheet_Id", "dbo.CostSheets", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Offers", "CostSheet_Id", "dbo.CostSheets");
            DropForeignKey("dbo.Offers", "costCenterCurrencyId", "dbo.Currencies");
            DropForeignKey("dbo.CostSheetOfferFields", "FieldId", "dbo.CostSheetFields");
            DropForeignKey("dbo.CostSheetOfferFields", "CostSheetId", "dbo.CostSheets");
            DropIndex("dbo.Offers", new[] { "costCenterCurrencyId" });
            DropIndex("dbo.Offers", new[] { "CostSheet_Id" });
            DropIndex("dbo.CostSheetOfferFields", new[] { "CostSheetId" });
            DropIndex("dbo.CostSheetOfferFields", new[] { "FieldId" });
            DropColumn("dbo.Offers", "budgetMarginMER");
            DropColumn("dbo.Offers", "budgetMarginSER");
            DropColumn("dbo.Offers", "totalbudgetCost");
            DropColumn("dbo.Offers", "totalOfferAmountMER");
            DropColumn("dbo.Offers", "totalOfferAmountSER");
            DropColumn("dbo.Offers", "totalOfferAmount");
            DropColumn("dbo.Offers", "MER");
            DropColumn("dbo.Offers", "SER");
            DropColumn("dbo.Offers", "costCenterCurrencyId");
            DropColumn("dbo.Offers", "CostSheet_Id");
            DropTable("dbo.CostSheetOfferFields");
        }
    }
}
