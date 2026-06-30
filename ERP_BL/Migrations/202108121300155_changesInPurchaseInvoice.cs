namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changesInPurchaseInvoice : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PurchaseInvoices", "POPaymentterm_Id", c => c.Int());
            AddColumn("dbo.PurchaseInvoices", "incoterm_Id", c => c.Int());
            AddColumn("dbo.PurchaseInvoices", "TitleValue1Id", c => c.Int());
            AddColumn("dbo.PurchaseInvoices", "TitleValue2Id", c => c.Int());
            AddColumn("dbo.PurchaseInvoices", "CostSheet_Id", c => c.Int());
            CreateIndex("dbo.PurchaseInvoices", "POPaymentterm_Id");
            CreateIndex("dbo.PurchaseInvoices", "incoterm_Id");
            CreateIndex("dbo.PurchaseInvoices", "TitleValue1Id");
            CreateIndex("dbo.PurchaseInvoices", "TitleValue2Id");
            CreateIndex("dbo.PurchaseInvoices", "CostSheet_Id");
            AddForeignKey("dbo.PurchaseInvoices", "CostSheet_Id", "dbo.CostSheets", "Id");
            AddForeignKey("dbo.PurchaseInvoices", "incoterm_Id", "dbo.PaymentTerms", "Id");
            AddForeignKey("dbo.PurchaseInvoices", "POPaymentterm_Id", "dbo.PaymentTerms", "Id");
            AddForeignKey("dbo.PurchaseInvoices", "TitleValue1Id", "dbo.Incoterms", "Id");
            AddForeignKey("dbo.PurchaseInvoices", "TitleValue2Id", "dbo.Incoterms", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PurchaseInvoices", "TitleValue2Id", "dbo.Incoterms");
            DropForeignKey("dbo.PurchaseInvoices", "TitleValue1Id", "dbo.Incoterms");
            DropForeignKey("dbo.PurchaseInvoices", "POPaymentterm_Id", "dbo.PaymentTerms");
            DropForeignKey("dbo.PurchaseInvoices", "incoterm_Id", "dbo.PaymentTerms");
            DropForeignKey("dbo.PurchaseInvoices", "CostSheet_Id", "dbo.CostSheets");
            DropIndex("dbo.PurchaseInvoices", new[] { "CostSheet_Id" });
            DropIndex("dbo.PurchaseInvoices", new[] { "TitleValue2Id" });
            DropIndex("dbo.PurchaseInvoices", new[] { "TitleValue1Id" });
            DropIndex("dbo.PurchaseInvoices", new[] { "incoterm_Id" });
            DropIndex("dbo.PurchaseInvoices", new[] { "POPaymentterm_Id" });
            DropColumn("dbo.PurchaseInvoices", "CostSheet_Id");
            DropColumn("dbo.PurchaseInvoices", "TitleValue2Id");
            DropColumn("dbo.PurchaseInvoices", "TitleValue1Id");
            DropColumn("dbo.PurchaseInvoices", "incoterm_Id");
            DropColumn("dbo.PurchaseInvoices", "POPaymentterm_Id");
        }
    }
}
