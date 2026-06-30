namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changestabBackground : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tabBackground", "userId", c => c.Int());
            AddColumn("dbo.tabBackground", "UploadedTime", c => c.DateTime());
            CreateIndex("dbo.tabBackground", "userId");
            AddForeignKey("dbo.tabBackground", "userId", "dbo.Users", "id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tabBackground", "userId", "dbo.Users");
            DropIndex("dbo.tabBackground", new[] { "userId" });
            DropColumn("dbo.tabBackground", "UploadedTime");
            DropColumn("dbo.tabBackground", "userId");
        }
    }
}
