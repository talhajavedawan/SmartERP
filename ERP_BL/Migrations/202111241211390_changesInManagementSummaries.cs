namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changesInManagementSummaries : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ManagementSummaries", "ParentId", c => c.Int());
            CreateIndex("dbo.ManagementSummaries", "ParentId");
            AddForeignKey("dbo.ManagementSummaries", "ParentId", "dbo.ManagementSummaries", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ManagementSummaries", "ParentId", "dbo.ManagementSummaries");
            DropIndex("dbo.ManagementSummaries", new[] { "ParentId" });
            DropColumn("dbo.ManagementSummaries", "ParentId");
        }
    }
}
