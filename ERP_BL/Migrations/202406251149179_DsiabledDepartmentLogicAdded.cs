namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DsiabledDepartmentLogicAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CustomerCompanyDepartment1",
                c => new
                    {
                        CustomerCompany_Id = c.Int(nullable: false),
                        Department_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CustomerCompany_Id, t.Department_Id })
                .ForeignKey("dbo.CustomerCompanies", t => t.CustomerCompany_Id, cascadeDelete: true)
                .ForeignKey("dbo.tabDepartment", t => t.Department_Id, cascadeDelete: true)
                .Index(t => t.CustomerCompany_Id)
                .Index(t => t.Department_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CustomerCompanyDepartment1", "Department_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.CustomerCompanyDepartment1", "CustomerCompany_Id", "dbo.CustomerCompanies");
            DropIndex("dbo.CustomerCompanyDepartment1", new[] { "Department_Id" });
            DropIndex("dbo.CustomerCompanyDepartment1", new[] { "CustomerCompany_Id" });
            DropTable("dbo.CustomerCompanyDepartment1");
        }
    }
}
