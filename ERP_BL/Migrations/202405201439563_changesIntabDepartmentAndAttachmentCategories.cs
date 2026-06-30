namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changesIntabDepartmentAndAttachmentCategories : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tabDepartment", "IsAssetType", c => c.Boolean(nullable: false));
            AddColumn("dbo.tabDepartment", "IsRentalContractType", c => c.Boolean(nullable: false));
            AddColumn("dbo.tabDepartment", "IsRentalOrderType", c => c.Boolean(nullable: false));
            AddColumn("dbo.tabDepartment", "IsRentalInvoiceType", c => c.Boolean(nullable: false));
            AddColumn("dbo.tabDepartment", "IsRentalReceiptType", c => c.Boolean(nullable: false));
            AddColumn("dbo.AttachmentCategories", "RentalContract", c => c.Int());
            AddColumn("dbo.AttachmentCategories", "RentalOrder", c => c.Int());
            AddColumn("dbo.AttachmentCategories", "RentalInvoice", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AttachmentCategories", "RentalInvoice");
            DropColumn("dbo.AttachmentCategories", "RentalOrder");
            DropColumn("dbo.AttachmentCategories", "RentalContract");
            DropColumn("dbo.tabDepartment", "IsRentalReceiptType");
            DropColumn("dbo.tabDepartment", "IsRentalInvoiceType");
            DropColumn("dbo.tabDepartment", "IsRentalOrderType");
            DropColumn("dbo.tabDepartment", "IsRentalContractType");
            DropColumn("dbo.tabDepartment", "IsAssetType");
        }
    }
}
