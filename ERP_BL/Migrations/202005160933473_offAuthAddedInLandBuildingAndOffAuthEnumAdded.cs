namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class offAuthAddedInLandBuildingAndOffAuthEnumAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OfficialAuths", "officialAuthtype", c => c.Int(nullable: false));
            AddColumn("dbo.Buildings", "OfficialAuths_Id", c => c.Int());
            AddColumn("dbo.Lands", "OfficialAuths_Id", c => c.Int());
            CreateIndex("dbo.Buildings", "OfficialAuths_Id");
            CreateIndex("dbo.Lands", "OfficialAuths_Id");
            AddForeignKey("dbo.Buildings", "OfficialAuths_Id", "dbo.OfficialAuths", "Id");
            AddForeignKey("dbo.Lands", "OfficialAuths_Id", "dbo.OfficialAuths", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Lands", "OfficialAuths_Id", "dbo.OfficialAuths");
            DropForeignKey("dbo.Buildings", "OfficialAuths_Id", "dbo.OfficialAuths");
            DropIndex("dbo.Lands", new[] { "OfficialAuths_Id" });
            DropIndex("dbo.Buildings", new[] { "OfficialAuths_Id" });
            DropColumn("dbo.Lands", "OfficialAuths_Id");
            DropColumn("dbo.Buildings", "OfficialAuths_Id");
            DropColumn("dbo.OfficialAuths", "officialAuthtype");
        }
    }
}
