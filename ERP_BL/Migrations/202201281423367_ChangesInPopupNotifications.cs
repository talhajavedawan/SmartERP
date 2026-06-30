namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesInPopupNotifications : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tabPopupNotifications", "FontSize", c => c.Double(nullable: false));
            AddColumn("dbo.tabPopupNotifications", "fontWeight", c => c.String());
            AddColumn("dbo.tabPopupNotifications", "Italic", c => c.String());
            AddColumn("dbo.tabPopupNotifications", "FontSizeHeading", c => c.Double(nullable: false));
            AddColumn("dbo.tabPopupNotifications", "fontWeightHeading", c => c.String());
            AddColumn("dbo.tabPopupNotifications", "ItalicHeading", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tabPopupNotifications", "ItalicHeading");
            DropColumn("dbo.tabPopupNotifications", "fontWeightHeading");
            DropColumn("dbo.tabPopupNotifications", "FontSizeHeading");
            DropColumn("dbo.tabPopupNotifications", "Italic");
            DropColumn("dbo.tabPopupNotifications", "fontWeight");
            DropColumn("dbo.tabPopupNotifications", "FontSize");
        }
    }
}
