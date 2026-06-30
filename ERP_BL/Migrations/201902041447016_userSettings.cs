namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userSettings : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserSettings",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        userId = c.Int(nullable: false),
                        settingkey = c.String(nullable: false, maxLength: 128, unicode: false),
                        settingValue = c.String(maxLength: 8000, unicode: false),
                        lastModified = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.id, t.userId, t.settingkey })
                .ForeignKey("dbo.Users", t => t.userId, cascadeDelete: true)
                .Index(t => t.userId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserSettings", "userId", "dbo.Users");
            DropIndex("dbo.UserSettings", new[] { "userId" });
            DropTable("dbo.UserSettings");
        }
    }
}
