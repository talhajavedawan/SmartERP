namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TargetRewardNaturesAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TargetRewardNatures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NatureName = c.String(),
                        isActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.TargetRewards", "targetRewardNatureId", c => c.Int());
            CreateIndex("dbo.TargetRewards", "targetRewardNatureId");
            AddForeignKey("dbo.TargetRewards", "targetRewardNatureId", "dbo.TargetRewardNatures", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TargetRewards", "targetRewardNatureId", "dbo.TargetRewardNatures");
            DropIndex("dbo.TargetRewards", new[] { "targetRewardNatureId" });
            DropColumn("dbo.TargetRewards", "targetRewardNatureId");
            DropTable("dbo.TargetRewardNatures");
        }
    }
}
