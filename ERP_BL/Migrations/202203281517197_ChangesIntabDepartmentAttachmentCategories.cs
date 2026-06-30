namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesIntabDepartmentAttachmentCategories : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tabDepartment", "IsProcurementType", c => c.Boolean(nullable: false));
            AddColumn("dbo.tabDepartment", "IsInventoryType", c => c.Boolean(nullable: false));
            AddColumn("dbo.tabDepartment", "IsVendorBillType", c => c.Boolean(nullable: false));
            AddColumn("dbo.tabDepartment", "IsInterBankTransferType", c => c.Boolean(nullable: false));
            AddColumn("dbo.tabDepartment", "IsInterCompTransferType", c => c.Boolean(nullable: false));
            AddColumn("dbo.AttachmentCategories", "Inquiry", c => c.Int());
            AddColumn("dbo.AttachmentCategories", "Offer", c => c.Int());
            AddColumn("dbo.AttachmentCategories", "PO", c => c.Int());
            AddColumn("dbo.AttachmentCategories", "SO", c => c.Int());
            AddColumn("dbo.AttachmentCategories", "SI", c => c.Int());
            AddColumn("dbo.AttachmentCategories", "SR", c => c.Int());
            AddColumn("dbo.AttachmentCategories", "PI", c => c.Int());
            AddColumn("dbo.AttachmentCategories", "Payment", c => c.Int());
            AddColumn("dbo.AttachmentCategories", "VBill", c => c.Int());
            AddColumn("dbo.AttachmentCategories", "ABill", c => c.Int());
            AddColumn("dbo.AttachmentCategories", "IBT", c => c.Int());
            AddColumn("dbo.AttachmentCategories", "ICBT", c => c.Int());
            AddColumn("dbo.AttachmentCategories", "MS", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AttachmentCategories", "MS");
            DropColumn("dbo.AttachmentCategories", "ICBT");
            DropColumn("dbo.AttachmentCategories", "IBT");
            DropColumn("dbo.AttachmentCategories", "ABill");
            DropColumn("dbo.AttachmentCategories", "VBill");
            DropColumn("dbo.AttachmentCategories", "Payment");
            DropColumn("dbo.AttachmentCategories", "PI");
            DropColumn("dbo.AttachmentCategories", "SR");
            DropColumn("dbo.AttachmentCategories", "SI");
            DropColumn("dbo.AttachmentCategories", "SO");
            DropColumn("dbo.AttachmentCategories", "PO");
            DropColumn("dbo.AttachmentCategories", "Offer");
            DropColumn("dbo.AttachmentCategories", "Inquiry");
            DropColumn("dbo.tabDepartment", "IsInterCompTransferType");
            DropColumn("dbo.tabDepartment", "IsInterBankTransferType");
            DropColumn("dbo.tabDepartment", "IsVendorBillType");
            DropColumn("dbo.tabDepartment", "IsInventoryType");
            DropColumn("dbo.tabDepartment", "IsProcurementType");
        }
    }
}
