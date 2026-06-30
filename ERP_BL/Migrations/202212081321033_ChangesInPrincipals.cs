namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesInPrincipals : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Principals", "IsSubsidary", c => c.Boolean(nullable: false));
            AddColumn("dbo.Principals", "ParentID", c => c.Int());
            CreateIndex("dbo.Principals", "ParentID");
            AddForeignKey("dbo.Principals", "ParentID", "dbo.Principals", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Principals", "ParentID", "dbo.Principals");
            DropIndex("dbo.Principals", new[] { "ParentID" });
            DropColumn("dbo.Principals", "ParentID");
            DropColumn("dbo.Principals", "IsSubsidary");
        }
    }
}
