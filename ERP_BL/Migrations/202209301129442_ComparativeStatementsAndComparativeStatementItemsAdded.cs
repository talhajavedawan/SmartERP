namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ComparativeStatementsAndComparativeStatementItemsAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ComparativeStatements",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        offerId = c.Int(),
                        VendorId = c.Int(),
                        offerValue = c.Double(nullable: false),
                        incoTerm_Id = c.Int(),
                        offerCurrencyId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Incoterms", t => t.incoTerm_Id)
                .ForeignKey("dbo.Offers", t => t.offerId)
                .ForeignKey("dbo.Currencies", t => t.offerCurrencyId)
                .ForeignKey("dbo.tabVendor", t => t.VendorId)
                .Index(t => t.offerId)
                .Index(t => t.VendorId)
                .Index(t => t.incoTerm_Id)
                .Index(t => t.offerCurrencyId);
            
            CreateTable(
                "dbo.ComparativeStatementItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        costSheetFieldId = c.Int(),
                        convertedCurrencyId = c.Int(),
                        itemCurrencyId = c.Int(),
                        comparativeStatementId = c.Int(),
                        itemIncoTermId = c.Int(),
                        MER = c.Double(nullable: false),
                        amountOC = c.Double(nullable: false),
                        amountMER = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ComparativeStatements", t => t.comparativeStatementId)
                .ForeignKey("dbo.Currencies", t => t.convertedCurrencyId)
                .ForeignKey("dbo.CostSheetFields", t => t.costSheetFieldId)
                .ForeignKey("dbo.Currencies", t => t.itemCurrencyId)
                .ForeignKey("dbo.Incoterms", t => t.itemIncoTermId)
                .Index(t => t.costSheetFieldId)
                .Index(t => t.convertedCurrencyId)
                .Index(t => t.itemCurrencyId)
                .Index(t => t.comparativeStatementId)
                .Index(t => t.itemIncoTermId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ComparativeStatements", "VendorId", "dbo.tabVendor");
            DropForeignKey("dbo.ComparativeStatements", "offerCurrencyId", "dbo.Currencies");
            DropForeignKey("dbo.ComparativeStatements", "offerId", "dbo.Offers");
            DropForeignKey("dbo.ComparativeStatements", "incoTerm_Id", "dbo.Incoterms");
            DropForeignKey("dbo.ComparativeStatementItems", "itemIncoTermId", "dbo.Incoterms");
            DropForeignKey("dbo.ComparativeStatementItems", "itemCurrencyId", "dbo.Currencies");
            DropForeignKey("dbo.ComparativeStatementItems", "costSheetFieldId", "dbo.CostSheetFields");
            DropForeignKey("dbo.ComparativeStatementItems", "convertedCurrencyId", "dbo.Currencies");
            DropForeignKey("dbo.ComparativeStatementItems", "comparativeStatementId", "dbo.ComparativeStatements");
            DropIndex("dbo.ComparativeStatementItems", new[] { "itemIncoTermId" });
            DropIndex("dbo.ComparativeStatementItems", new[] { "comparativeStatementId" });
            DropIndex("dbo.ComparativeStatementItems", new[] { "itemCurrencyId" });
            DropIndex("dbo.ComparativeStatementItems", new[] { "convertedCurrencyId" });
            DropIndex("dbo.ComparativeStatementItems", new[] { "costSheetFieldId" });
            DropIndex("dbo.ComparativeStatements", new[] { "offerCurrencyId" });
            DropIndex("dbo.ComparativeStatements", new[] { "incoTerm_Id" });
            DropIndex("dbo.ComparativeStatements", new[] { "VendorId" });
            DropIndex("dbo.ComparativeStatements", new[] { "offerId" });
            DropTable("dbo.ComparativeStatementItems");
            DropTable("dbo.ComparativeStatements");
        }
    }
}
