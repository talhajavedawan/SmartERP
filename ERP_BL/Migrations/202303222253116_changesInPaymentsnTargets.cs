namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changesInPaymentsnTargets : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Payments", "paymentTargetRewardsTemplate", c => c.Int());
            AddColumn("dbo.Payments", "TargetReward_Id", c => c.Int());
            AddColumn("dbo.Payments", "taskGroups_Id", c => c.Int());
            AddColumn("dbo.TargetRewards", "CreationDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.TargetRewards", "currencyId", c => c.Int());
            AddColumn("dbo.AttachmentCategories", "TargetReward", c => c.Int());
            AddColumn("dbo.GridReports", "from", c => c.DateTime());
            AddColumn("dbo.GridReports", "to", c => c.DateTime());
            AddColumn("dbo.GridReports", "company_Id", c => c.Int());
            CreateIndex("dbo.Payments", "TargetReward_Id");
            CreateIndex("dbo.Payments", "taskGroups_Id");
            CreateIndex("dbo.TargetRewards", "currencyId");
            CreateIndex("dbo.GridReports", "company_Id");
            AddForeignKey("dbo.TargetRewards", "currencyId", "dbo.Currencies", "Id");
            AddForeignKey("dbo.Payments", "TargetReward_Id", "dbo.TargetRewards", "Id");
            AddForeignKey("dbo.Payments", "taskGroups_Id", "dbo.TaskGroups", "Id");
            AddForeignKey("dbo.GridReports", "company_Id", "dbo.tabCompany", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GridReports", "company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.Payments", "taskGroups_Id", "dbo.TaskGroups");
            DropForeignKey("dbo.Payments", "TargetReward_Id", "dbo.TargetRewards");
            DropForeignKey("dbo.TargetRewards", "currencyId", "dbo.Currencies");
            DropIndex("dbo.GridReports", new[] { "company_Id" });
            DropIndex("dbo.TargetRewards", new[] { "currencyId" });
            DropIndex("dbo.Payments", new[] { "taskGroups_Id" });
            DropIndex("dbo.Payments", new[] { "TargetReward_Id" });
            DropColumn("dbo.GridReports", "company_Id");
            DropColumn("dbo.GridReports", "to");
            DropColumn("dbo.GridReports", "from");
            DropColumn("dbo.AttachmentCategories", "TargetReward");
            DropColumn("dbo.TargetRewards", "currencyId");
            DropColumn("dbo.TargetRewards", "CreationDate");
            DropColumn("dbo.Payments", "taskGroups_Id");
            DropColumn("dbo.Payments", "TargetReward_Id");
            DropColumn("dbo.Payments", "paymentTargetRewardsTemplate");
        }
    }
}
