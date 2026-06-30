namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DepartmentLevelsAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DepartmentLevels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        ParentID = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DepartmentLevels", t => t.ParentID)
                .Index(t => t.ParentID);
            
            AddColumn("dbo.tabDepartment", "LevelID", c => c.Int());
            CreateIndex("dbo.tabDepartment", "LevelID");
            AddForeignKey("dbo.tabDepartment", "LevelID", "dbo.DepartmentLevels", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tabDepartment", "LevelID", "dbo.DepartmentLevels");
            DropForeignKey("dbo.DepartmentLevels", "ParentID", "dbo.DepartmentLevels");
            DropIndex("dbo.DepartmentLevels", new[] { "ParentID" });
            DropIndex("dbo.tabDepartment", new[] { "LevelID" });
            DropColumn("dbo.tabDepartment", "LevelID");
            DropTable("dbo.DepartmentLevels");
        }
    }
}
