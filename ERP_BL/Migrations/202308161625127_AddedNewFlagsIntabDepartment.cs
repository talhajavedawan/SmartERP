namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedNewFlagsIntabDepartment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tabDepartment", "IsInquiryType", c => c.Boolean(nullable: false));
            AddColumn("dbo.tabDepartment", "IsOfferType", c => c.Boolean(nullable: false));
            AddColumn("dbo.tabDepartment", "IsSaleOrderType", c => c.Boolean(nullable: false));
            AddColumn("dbo.tabDepartment", "IsSaleInvoiceType", c => c.Boolean(nullable: false));
            AddColumn("dbo.tabDepartment", "IsSaleReceiptType", c => c.Boolean(nullable: false));
            AddColumn("dbo.tabDepartment", "IsPurchaseOrderType", c => c.Boolean(nullable: false));
            AddColumn("dbo.tabDepartment", "IsPurchaseInvoiceType", c => c.Boolean(nullable: false));
            AddColumn("dbo.tabDepartment", "IsPaymentType", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tabDepartment", "IsPaymentType");
            DropColumn("dbo.tabDepartment", "IsPurchaseInvoiceType");
            DropColumn("dbo.tabDepartment", "IsPurchaseOrderType");
            DropColumn("dbo.tabDepartment", "IsSaleReceiptType");
            DropColumn("dbo.tabDepartment", "IsSaleInvoiceType");
            DropColumn("dbo.tabDepartment", "IsSaleOrderType");
            DropColumn("dbo.tabDepartment", "IsOfferType");
            DropColumn("dbo.tabDepartment", "IsInquiryType");
        }
    }
}
