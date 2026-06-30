namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EfficiencyPointsAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EfficiencyPoints",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        title = c.String(),
                        isActive = c.Boolean(nullable: false),
                        Points = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.TaskEfficiencies", "efficiencyPoints_Id", c => c.Int());
            AddColumn("dbo.TaskEfficiencies", "TotalPoints", c => c.Double(nullable: false));
            AddColumn("dbo.TaskEfficiencies", "AchievedPoints", c => c.Double(nullable: false));
            AddColumn("dbo.NotificationFlags", "canGlow", c => c.Boolean(nullable: false));
            AddColumn("dbo.Notifications", "Glow", c => c.Boolean(nullable: false));
            CreateIndex("dbo.TaskEfficiencies", "efficiencyPoints_Id");
            AddForeignKey("dbo.TaskEfficiencies", "efficiencyPoints_Id", "dbo.EfficiencyPoints", "Id");
            DropColumn("dbo.Tasks", "EfficiencyPoints");
            DropColumn("dbo.TaskEfficiencies", "efficiencyPoints");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TaskEfficiencies", "efficiencyPoints", c => c.Int(nullable: false));
            AddColumn("dbo.Tasks", "EfficiencyPoints", c => c.Double(nullable: false));
            DropForeignKey("dbo.TaskEfficiencies", "efficiencyPoints_Id", "dbo.EfficiencyPoints");
            DropIndex("dbo.TaskEfficiencies", new[] { "efficiencyPoints_Id" });
            DropColumn("dbo.Notifications", "Glow");
            DropColumn("dbo.NotificationFlags", "canGlow");
            DropColumn("dbo.TaskEfficiencies", "AchievedPoints");
            DropColumn("dbo.TaskEfficiencies", "TotalPoints");
            DropColumn("dbo.TaskEfficiencies", "efficiencyPoints_Id");
            DropTable("dbo.EfficiencyPoints");
        }
    }
}
