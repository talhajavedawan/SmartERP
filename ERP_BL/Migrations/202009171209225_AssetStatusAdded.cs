namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AssetStatusAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AssetStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.String(),
                        isApproved = c.Boolean(nullable: false),
                        isActive = c.Boolean(),
                        backcolor = c.String(),
                        forecolor = c.String(),
                        HierarchicalIndex = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Assets", "assetStatus_Id", c => c.Int());
            CreateIndex("dbo.Assets", "assetStatus_Id");
            AddForeignKey("dbo.Assets", "assetStatus_Id", "dbo.AssetStatus", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Assets", "assetStatus_Id", "dbo.AssetStatus");
            DropIndex("dbo.Assets", new[] { "assetStatus_Id" });
            DropColumn("dbo.Assets", "assetStatus_Id");
            DropTable("dbo.AssetStatus");
        }
    }
}
