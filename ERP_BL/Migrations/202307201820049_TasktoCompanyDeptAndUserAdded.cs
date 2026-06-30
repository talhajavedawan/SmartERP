namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TasktoCompanyDeptAndUserAdded : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.TaskGroupsCompanies", newName: "CompanyTaskGroups");
            RenameTable(name: "dbo.TaskGroupsDepartments", newName: "DepartmentTaskGroups");
            DropPrimaryKey("dbo.CompanyTaskGroups");
            DropPrimaryKey("dbo.DepartmentTaskGroups");
            CreateTable(
                "dbo.TaskGroupsUser1",
                c => new
                    {
                        TaskGroups_Id = c.Int(nullable: false),
                        User_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.TaskGroups_Id, t.User_id })
                .ForeignKey("dbo.TaskGroups", t => t.TaskGroups_Id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_id, cascadeDelete: true)
                .Index(t => t.TaskGroups_Id)
                .Index(t => t.User_id);
            
            CreateTable(
                "dbo.DepartmentTaskGroups1",
                c => new
                    {
                        Department_Id = c.Int(nullable: false),
                        TaskGroups_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Department_Id, t.TaskGroups_Id })
                .ForeignKey("dbo.tabDepartment", t => t.Department_Id, cascadeDelete: true)
                .ForeignKey("dbo.TaskGroups", t => t.TaskGroups_Id, cascadeDelete: true)
                .Index(t => t.Department_Id)
                .Index(t => t.TaskGroups_Id);
            
            CreateTable(
                "dbo.CompanyTaskGroups1",
                c => new
                    {
                        Company_Id = c.Int(nullable: false),
                        TaskGroups_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Company_Id, t.TaskGroups_Id })
                .ForeignKey("dbo.tabCompany", t => t.Company_Id, cascadeDelete: true)
                .ForeignKey("dbo.TaskGroups", t => t.TaskGroups_Id, cascadeDelete: true)
                .Index(t => t.Company_Id)
                .Index(t => t.TaskGroups_Id);
            
            AddColumn("dbo.TaskGroups", "settingValue", c => c.String(unicode: false));
            AddColumn("dbo.TaskGroups", "targetsTransactionType", c => c.Int(nullable: false));
            AddColumn("dbo.STLs", "stlRef", c => c.String());
            AddColumn("dbo.STLs", "paymentMaturityDate", c => c.DateTime());
            AddColumn("dbo.STLs", "GLPostingDate", c => c.DateTime());
            AddColumn("dbo.STLs", "InterestAmount", c => c.Double(nullable: false));
            AddColumn("dbo.STLs", "paymentDate", c => c.DateTime());
            AddPrimaryKey("dbo.CompanyTaskGroups", new[] { "Company_Id", "TaskGroups_Id" });
            AddPrimaryKey("dbo.DepartmentTaskGroups", new[] { "Department_Id", "TaskGroups_Id" });
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CompanyTaskGroups1", "TaskGroups_Id", "dbo.TaskGroups");
            DropForeignKey("dbo.CompanyTaskGroups1", "Company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.DepartmentTaskGroups1", "TaskGroups_Id", "dbo.TaskGroups");
            DropForeignKey("dbo.DepartmentTaskGroups1", "Department_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.TaskGroupsUser1", "User_id", "dbo.Users");
            DropForeignKey("dbo.TaskGroupsUser1", "TaskGroups_Id", "dbo.TaskGroups");
            DropIndex("dbo.CompanyTaskGroups1", new[] { "TaskGroups_Id" });
            DropIndex("dbo.CompanyTaskGroups1", new[] { "Company_Id" });
            DropIndex("dbo.DepartmentTaskGroups1", new[] { "TaskGroups_Id" });
            DropIndex("dbo.DepartmentTaskGroups1", new[] { "Department_Id" });
            DropIndex("dbo.TaskGroupsUser1", new[] { "User_id" });
            DropIndex("dbo.TaskGroupsUser1", new[] { "TaskGroups_Id" });
            DropPrimaryKey("dbo.DepartmentTaskGroups");
            DropPrimaryKey("dbo.CompanyTaskGroups");
            DropColumn("dbo.STLs", "paymentDate");
            DropColumn("dbo.STLs", "InterestAmount");
            DropColumn("dbo.STLs", "GLPostingDate");
            DropColumn("dbo.STLs", "paymentMaturityDate");
            DropColumn("dbo.STLs", "stlRef");
            DropColumn("dbo.TaskGroups", "targetsTransactionType");
            DropColumn("dbo.TaskGroups", "settingValue");
            DropTable("dbo.CompanyTaskGroups1");
            DropTable("dbo.DepartmentTaskGroups1");
            DropTable("dbo.TaskGroupsUser1");
            AddPrimaryKey("dbo.DepartmentTaskGroups", new[] { "TaskGroups_Id", "Department_Id" });
            AddPrimaryKey("dbo.CompanyTaskGroups", new[] { "TaskGroups_Id", "Company_Id" });
            RenameTable(name: "dbo.DepartmentTaskGroups", newName: "TaskGroupsDepartments");
            RenameTable(name: "dbo.CompanyTaskGroups", newName: "TaskGroupsCompanies");
        }
    }
}
