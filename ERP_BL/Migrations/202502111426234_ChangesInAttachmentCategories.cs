namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesInAttachmentCategories : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AttachmentCategories", "AssetRental", c => c.Int());
            AddColumn("dbo.AttachmentCategories", "TenantRental", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AttachmentCategories", "TenantRental");
            DropColumn("dbo.AttachmentCategories", "AssetRental");
        }
    }
}
