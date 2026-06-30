namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BookerStatementItemsToSaleInvoiceConnected : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PaymentTerms", "ParentId", c => c.Int());
            AddColumn("dbo.SaleInvoices", "totalDistributionAmount", c => c.Double(nullable: false));
            AddColumn("dbo.SaleInvoices", "totalGSTAmount", c => c.Double(nullable: false));
            AddColumn("dbo.SaleInvoices", "totalAmountAfterGST", c => c.Double(nullable: false));
            AddColumn("dbo.SaleInvoices", "totalClaimDiscount", c => c.Double(nullable: false));
            AddColumn("dbo.SaleInvoices", "totalPassOn", c => c.Double(nullable: false));
            AddColumn("dbo.SaleInvoices", "totalFocSampling", c => c.Double(nullable: false));
            AddColumn("dbo.SaleInvoices", "totalNetAmount", c => c.Double(nullable: false));
            AddColumn("dbo.SaleOrders", "totalDistributionAmount", c => c.Double(nullable: false));
            AddColumn("dbo.SaleOrders", "totalGSTAmount", c => c.Double(nullable: false));
            AddColumn("dbo.SaleOrders", "totalAmountAfterGST", c => c.Double(nullable: false));
            AddColumn("dbo.SaleOrders", "totalClaimDiscount", c => c.Double(nullable: false));
            AddColumn("dbo.SaleOrders", "totalPassOn", c => c.Double(nullable: false));
            AddColumn("dbo.SaleOrders", "totalFocSampling", c => c.Double(nullable: false));
            AddColumn("dbo.SaleOrders", "totalNetAmount", c => c.Double(nullable: false));
            AddColumn("dbo.BookerStatementItems", "siQuantity", c => c.Double(nullable: false));
            AddColumn("dbo.BookerStatementItems", "siWeight", c => c.Double(nullable: false));
            AddColumn("dbo.BookerStatementItems", "siAmount", c => c.Double(nullable: false));
            AddColumn("dbo.BookerStatementItems", "siAmountGST", c => c.Double(nullable: false));
            AddColumn("dbo.BookerStatementItems", "siPassOnValue", c => c.Double(nullable: false));
            AddColumn("dbo.BookerStatementItems", "siFocValue", c => c.Double(nullable: false));
            AddColumn("dbo.BookerStatementItems", "siClaimDiscountValue", c => c.Double(nullable: false));
            AddColumn("dbo.BookerStatementItems", "siNetAmount", c => c.Double(nullable: false));
            AddColumn("dbo.BookerStatementItems", "saleInvoiceId", c => c.Int());
            AddColumn("dbo.ClaimDiscounts", "chartofAccountId", c => c.Int());
            AddColumn("dbo.FOCSamplings", "chartofAccountId", c => c.Int());
            AddColumn("dbo.PassOns", "chartofAccountId", c => c.Int());
            CreateIndex("dbo.PaymentTerms", "ParentId");
            CreateIndex("dbo.BookerStatementItems", "saleInvoiceId");
            CreateIndex("dbo.ClaimDiscounts", "chartofAccountId");
            CreateIndex("dbo.FOCSamplings", "chartofAccountId");
            CreateIndex("dbo.PassOns", "chartofAccountId");
            AddForeignKey("dbo.PaymentTerms", "ParentId", "dbo.PaymentTerms", "Id");
            AddForeignKey("dbo.ClaimDiscounts", "chartofAccountId", "dbo.ChartofAccounts", "Id");
            AddForeignKey("dbo.FOCSamplings", "chartofAccountId", "dbo.ChartofAccounts", "Id");
            AddForeignKey("dbo.PassOns", "chartofAccountId", "dbo.ChartofAccounts", "Id");
            AddForeignKey("dbo.BookerStatementItems", "saleInvoiceId", "dbo.SaleInvoices", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BookerStatementItems", "saleInvoiceId", "dbo.SaleInvoices");
            DropForeignKey("dbo.PassOns", "chartofAccountId", "dbo.ChartofAccounts");
            DropForeignKey("dbo.FOCSamplings", "chartofAccountId", "dbo.ChartofAccounts");
            DropForeignKey("dbo.ClaimDiscounts", "chartofAccountId", "dbo.ChartofAccounts");
            DropForeignKey("dbo.PaymentTerms", "ParentId", "dbo.PaymentTerms");
            DropIndex("dbo.PassOns", new[] { "chartofAccountId" });
            DropIndex("dbo.FOCSamplings", new[] { "chartofAccountId" });
            DropIndex("dbo.ClaimDiscounts", new[] { "chartofAccountId" });
            DropIndex("dbo.BookerStatementItems", new[] { "saleInvoiceId" });
            DropIndex("dbo.PaymentTerms", new[] { "ParentId" });
            DropColumn("dbo.PassOns", "chartofAccountId");
            DropColumn("dbo.FOCSamplings", "chartofAccountId");
            DropColumn("dbo.ClaimDiscounts", "chartofAccountId");
            DropColumn("dbo.BookerStatementItems", "saleInvoiceId");
            DropColumn("dbo.BookerStatementItems", "siNetAmount");
            DropColumn("dbo.BookerStatementItems", "siClaimDiscountValue");
            DropColumn("dbo.BookerStatementItems", "siFocValue");
            DropColumn("dbo.BookerStatementItems", "siPassOnValue");
            DropColumn("dbo.BookerStatementItems", "siAmountGST");
            DropColumn("dbo.BookerStatementItems", "siAmount");
            DropColumn("dbo.BookerStatementItems", "siWeight");
            DropColumn("dbo.BookerStatementItems", "siQuantity");
            DropColumn("dbo.SaleOrders", "totalNetAmount");
            DropColumn("dbo.SaleOrders", "totalFocSampling");
            DropColumn("dbo.SaleOrders", "totalPassOn");
            DropColumn("dbo.SaleOrders", "totalClaimDiscount");
            DropColumn("dbo.SaleOrders", "totalAmountAfterGST");
            DropColumn("dbo.SaleOrders", "totalGSTAmount");
            DropColumn("dbo.SaleOrders", "totalDistributionAmount");
            DropColumn("dbo.SaleInvoices", "totalNetAmount");
            DropColumn("dbo.SaleInvoices", "totalFocSampling");
            DropColumn("dbo.SaleInvoices", "totalPassOn");
            DropColumn("dbo.SaleInvoices", "totalClaimDiscount");
            DropColumn("dbo.SaleInvoices", "totalAmountAfterGST");
            DropColumn("dbo.SaleInvoices", "totalGSTAmount");
            DropColumn("dbo.SaleInvoices", "totalDistributionAmount");
            DropColumn("dbo.PaymentTerms", "ParentId");
        }
    }
}
