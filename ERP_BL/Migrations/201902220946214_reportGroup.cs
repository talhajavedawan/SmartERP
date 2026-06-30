namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class reportGroup : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ReportGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        group = c.String(),
                        parentId = c.Int(),
                        isActive = c.Boolean(nullable: false),
                        user_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ReportGroups", t => t.parentId)
                .ForeignKey("dbo.Users", t => t.user_Id)
                .Index(t => t.parentId)
                .Index(t => t.user_Id);
            
            AddColumn("dbo.Reports", "groupId", c => c.Int());
            CreateIndex("dbo.Reports", "groupId");
            AddForeignKey("dbo.Reports", "groupId", "dbo.ReportGroups", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ReportGroups", "user_Id", "dbo.Users");
            DropForeignKey("dbo.Reports", "groupId", "dbo.ReportGroups");
            DropForeignKey("dbo.ReportGroups", "parentId", "dbo.ReportGroups");
            DropIndex("dbo.ReportGroups", new[] { "user_Id" });
            DropIndex("dbo.ReportGroups", new[] { "parentId" });
            DropIndex("dbo.Reports", new[] { "groupId" });
            DropColumn("dbo.Reports", "groupId");
            DropTable("dbo.ReportGroups");
        }
    }
}
