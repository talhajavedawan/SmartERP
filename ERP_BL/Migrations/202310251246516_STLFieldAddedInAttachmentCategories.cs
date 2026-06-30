namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class STLFieldAddedInAttachmentCategories : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AttachmentCategories", "STL", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AttachmentCategories", "STL");
        }
    }
}
