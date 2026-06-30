namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BookerStatementItemsAndClaimDiscountsAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BookerStatementItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        offerId = c.Int(),
                        product_Id = c.Int(nullable: false),
                        unit = c.Double(nullable: false),
                        quantity = c.Double(nullable: false),
                        weight = c.Double(nullable: false),
                        amount = c.Double(nullable: false),
                        amountGST = c.Double(nullable: false),
                        passOnValue = c.Double(nullable: false),
                        taxNameId = c.Int(),
                        focSamplingId = c.Int(),
                        focValue = c.Double(nullable: false),
                        claimDiscountId = c.Int(),
                        claimDiscountValue = c.Double(nullable: false),
                        netAmount = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ClaimDiscounts", t => t.claimDiscountId)
                .ForeignKey("dbo.FOCSamplings", t => t.focSamplingId)
                .ForeignKey("dbo.Offers", t => t.offerId)
                .ForeignKey("dbo.Products", t => t.product_Id, cascadeDelete: true)
                .ForeignKey("dbo.TaxNames", t => t.taxNameId)
                .Index(t => t.offerId)
                .Index(t => t.product_Id)
                .Index(t => t.taxNameId)
                .Index(t => t.focSamplingId)
                .Index(t => t.claimDiscountId);
            
            CreateTable(
                "dbo.ClaimDiscounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        discountName = c.String(),
                        isActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.FOCSamplings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        samplingtName = c.String(),
                        isActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BookerStatementItems", "taxNameId", "dbo.TaxNames");
            DropForeignKey("dbo.BookerStatementItems", "product_Id", "dbo.Products");
            DropForeignKey("dbo.BookerStatementItems", "offerId", "dbo.Offers");
            DropForeignKey("dbo.BookerStatementItems", "focSamplingId", "dbo.FOCSamplings");
            DropForeignKey("dbo.BookerStatementItems", "claimDiscountId", "dbo.ClaimDiscounts");
            DropIndex("dbo.BookerStatementItems", new[] { "claimDiscountId" });
            DropIndex("dbo.BookerStatementItems", new[] { "focSamplingId" });
            DropIndex("dbo.BookerStatementItems", new[] { "taxNameId" });
            DropIndex("dbo.BookerStatementItems", new[] { "product_Id" });
            DropIndex("dbo.BookerStatementItems", new[] { "offerId" });
            DropTable("dbo.FOCSamplings");
            DropTable("dbo.ClaimDiscounts");
            DropTable("dbo.BookerStatementItems");
        }
    }
}
