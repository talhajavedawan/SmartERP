namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class prinicipaldepartment : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PrincipalDepartments",
                c => new
                    {
                        Principal_Id = c.Int(nullable: false),
                        Department_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Principal_Id, t.Department_Id })
                .ForeignKey("dbo.Principals", t => t.Principal_Id, cascadeDelete: true)
                .ForeignKey("dbo.tabDepartment", t => t.Department_Id, cascadeDelete: true)
                .Index(t => t.Principal_Id)
                .Index(t => t.Department_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PrincipalDepartments", "Department_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.PrincipalDepartments", "Principal_Id", "dbo.Principals");
            DropIndex("dbo.PrincipalDepartments", new[] { "Department_Id" });
            DropIndex("dbo.PrincipalDepartments", new[] { "Principal_Id" });
            DropTable("dbo.PrincipalDepartments");
        }
    }
}
