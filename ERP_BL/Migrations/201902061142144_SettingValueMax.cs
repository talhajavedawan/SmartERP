namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SettingValueMax : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.UserSettings", "settingValue", c => c.String(unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UserSettings", "settingValue", c => c.String(maxLength: 8000, unicode: false));
        }
    }
}
