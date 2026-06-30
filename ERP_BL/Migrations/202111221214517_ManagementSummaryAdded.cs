namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ManagementSummaryAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ManagementSummaries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SummaryName = c.String(),
                        isActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.tabAdminBill", "hasSummary", c => c.Boolean(nullable: false));
            AddColumn("dbo.tabAdminBill", "managementSummary_Id", c => c.Int());
            AddColumn("dbo.tabAdminBill", "SummaryMemo", c => c.String());
            CreateIndex("dbo.tabAdminBill", "managementSummary_Id");
            AddForeignKey("dbo.tabAdminBill", "managementSummary_Id", "dbo.ManagementSummaries", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tabAdminBill", "managementSummary_Id", "dbo.ManagementSummaries");
            DropIndex("dbo.tabAdminBill", new[] { "managementSummary_Id" });
            DropColumn("dbo.tabAdminBill", "SummaryMemo");
            DropColumn("dbo.tabAdminBill", "managementSummary_Id");
            DropColumn("dbo.tabAdminBill", "hasSummary");
            DropTable("dbo.ManagementSummaries");
        }
    }
}
