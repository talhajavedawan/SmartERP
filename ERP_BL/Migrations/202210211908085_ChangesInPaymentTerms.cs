namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesInPaymentTerms : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PaymentTerms", "isSaleOrderType", c => c.Boolean(nullable: false));
            AddColumn("dbo.PaymentTerms", "isPurchaseOrderType", c => c.Boolean(nullable: false));
            AddColumn("dbo.PaymentTerms", "isSaleInvoiceType", c => c.Boolean(nullable: false));
            AddColumn("dbo.PaymentTerms", "isPurchaseInvoiceType", c => c.Boolean(nullable: false));
            AddColumn("dbo.PaymentTerms", "isPaymentType", c => c.Boolean(nullable: false));
            AddColumn("dbo.PaymentTerms", "isVnedorBillType", c => c.Boolean(nullable: false));
            AddColumn("dbo.PaymentTerms", "isOfferType", c => c.Boolean(nullable: false));
            AddColumn("dbo.PaymentTerms", "isCostSheetType", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PaymentTerms", "isCostSheetType");
            DropColumn("dbo.PaymentTerms", "isOfferType");
            DropColumn("dbo.PaymentTerms", "isVnedorBillType");
            DropColumn("dbo.PaymentTerms", "isPaymentType");
            DropColumn("dbo.PaymentTerms", "isPurchaseInvoiceType");
            DropColumn("dbo.PaymentTerms", "isSaleInvoiceType");
            DropColumn("dbo.PaymentTerms", "isPurchaseOrderType");
            DropColumn("dbo.PaymentTerms", "isSaleOrderType");
        }
    }
}
