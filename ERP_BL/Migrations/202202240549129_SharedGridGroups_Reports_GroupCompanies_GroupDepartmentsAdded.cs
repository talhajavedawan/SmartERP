namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SharedGridGroups_Reports_GroupCompanies_GroupDepartmentsAdded : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.GridReports", "userId", "dbo.Users");
            DropIndex("dbo.GridReports", new[] { "userId" });
            DropPrimaryKey("dbo.GridReports");
            CreateTable(
                "dbo.SharedGridGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        groupName = c.String(),
                        parentId = c.Int(),
                        isVoid = c.Boolean(nullable: false),
                        userId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SharedGridGroups", t => t.parentId)
                .ForeignKey("dbo.Users", t => t.userId)
                .Index(t => t.parentId)
                .Index(t => t.userId);
            
            CreateTable(
                "dbo.SharedReports",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        settingkey = c.String(nullable: false, maxLength: 128, unicode: false),
                        userId = c.Int(),
                        settingValue = c.String(unicode: false),
                        lastModified = c.DateTime(nullable: false),
                        reportName = c.String(),
                        sharedGroupId = c.Int(),
                    })
                .PrimaryKey(t => new { t.Id, t.settingkey })
                .ForeignKey("dbo.Users", t => t.userId)
                .ForeignKey("dbo.SharedGridGroups", t => t.sharedGroupId)
                .Index(t => t.userId)
                .Index(t => t.sharedGroupId);
            
            CreateTable(
                "dbo.SharedGridGroupCompanies",
                c => new
                    {
                        SharedGridGroup_Id = c.Int(nullable: false),
                        Company_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.SharedGridGroup_Id, t.Company_Id })
                .ForeignKey("dbo.SharedGridGroups", t => t.SharedGridGroup_Id, cascadeDelete: true)
                .ForeignKey("dbo.tabCompany", t => t.Company_Id, cascadeDelete: true)
                .Index(t => t.SharedGridGroup_Id)
                .Index(t => t.Company_Id);
            
            CreateTable(
                "dbo.SharedGridGroupDepartments",
                c => new
                    {
                        SharedGridGroup_Id = c.Int(nullable: false),
                        Department_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.SharedGridGroup_Id, t.Department_Id })
                .ForeignKey("dbo.SharedGridGroups", t => t.SharedGridGroup_Id, cascadeDelete: true)
                .ForeignKey("dbo.tabDepartment", t => t.Department_Id, cascadeDelete: true)
                .Index(t => t.SharedGridGroup_Id)
                .Index(t => t.Department_Id);
            
            CreateTable(
                "dbo.SharedGridGroupEmployees",
                c => new
                    {
                        SharedGridGroup_Id = c.Int(nullable: false),
                        Employee_EmpId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.SharedGridGroup_Id, t.Employee_EmpId })
                .ForeignKey("dbo.SharedGridGroups", t => t.SharedGridGroup_Id, cascadeDelete: true)
                .ForeignKey("dbo.Employees", t => t.Employee_EmpId, cascadeDelete: true)
                .Index(t => t.SharedGridGroup_Id)
                .Index(t => t.Employee_EmpId);
            
            AddColumn("dbo.ToDoTasks", "stage", c => c.String());
            AddColumn("dbo.ToDoTasks", "isApproved", c => c.Boolean());
            AddColumn("dbo.ToDoTasks", "ApprovedDate", c => c.DateTime());
            AddColumn("dbo.ToDoTasks", "isReApproved", c => c.Boolean());
            AddColumn("dbo.ToDoTasks", "ReApprovalDate", c => c.DateTime());
            AddColumn("dbo.ToDoTasks", "TargetYear", c => c.DateTime());
            AddColumn("dbo.ToDoTasks", "TargetMonth", c => c.DateTime());
            AddColumn("dbo.InterBankTransfers", "GLPostingDate", c => c.DateTime());
            AddColumn("dbo.InterCompanyBankTransfers", "GLPostingDate", c => c.DateTime());
            AddColumn("dbo.RentalReceiveAmounts", "creatorId", c => c.Int());
            AlterColumn("dbo.GridReports", "userId", c => c.Int());
            AddPrimaryKey("dbo.GridReports", new[] { "Id", "settingkey" });
            CreateIndex("dbo.GridReports", "userId");
            CreateIndex("dbo.RentalReceiveAmounts", "creatorId");
            AddForeignKey("dbo.RentalReceiveAmounts", "creatorId", "dbo.Employees", "EmpId");
            AddForeignKey("dbo.GridReports", "userId", "dbo.Users", "id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GridReports", "userId", "dbo.Users");
            DropForeignKey("dbo.RentalReceiveAmounts", "creatorId", "dbo.Employees");
            DropForeignKey("dbo.SharedGridGroups", "userId", "dbo.Users");
            DropForeignKey("dbo.SharedReports", "sharedGroupId", "dbo.SharedGridGroups");
            DropForeignKey("dbo.SharedReports", "userId", "dbo.Users");
            DropForeignKey("dbo.SharedGridGroups", "parentId", "dbo.SharedGridGroups");
            DropForeignKey("dbo.SharedGridGroupEmployees", "Employee_EmpId", "dbo.Employees");
            DropForeignKey("dbo.SharedGridGroupEmployees", "SharedGridGroup_Id", "dbo.SharedGridGroups");
            DropForeignKey("dbo.SharedGridGroupDepartments", "Department_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.SharedGridGroupDepartments", "SharedGridGroup_Id", "dbo.SharedGridGroups");
            DropForeignKey("dbo.SharedGridGroupCompanies", "Company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.SharedGridGroupCompanies", "SharedGridGroup_Id", "dbo.SharedGridGroups");
            DropIndex("dbo.SharedGridGroupEmployees", new[] { "Employee_EmpId" });
            DropIndex("dbo.SharedGridGroupEmployees", new[] { "SharedGridGroup_Id" });
            DropIndex("dbo.SharedGridGroupDepartments", new[] { "Department_Id" });
            DropIndex("dbo.SharedGridGroupDepartments", new[] { "SharedGridGroup_Id" });
            DropIndex("dbo.SharedGridGroupCompanies", new[] { "Company_Id" });
            DropIndex("dbo.SharedGridGroupCompanies", new[] { "SharedGridGroup_Id" });
            DropIndex("dbo.RentalReceiveAmounts", new[] { "creatorId" });
            DropIndex("dbo.GridReports", new[] { "userId" });
            DropIndex("dbo.SharedReports", new[] { "sharedGroupId" });
            DropIndex("dbo.SharedReports", new[] { "userId" });
            DropIndex("dbo.SharedGridGroups", new[] { "userId" });
            DropIndex("dbo.SharedGridGroups", new[] { "parentId" });
            DropPrimaryKey("dbo.GridReports");
            AlterColumn("dbo.GridReports", "userId", c => c.Int(nullable: false));
            DropColumn("dbo.RentalReceiveAmounts", "creatorId");
            DropColumn("dbo.InterCompanyBankTransfers", "GLPostingDate");
            DropColumn("dbo.InterBankTransfers", "GLPostingDate");
            DropColumn("dbo.ToDoTasks", "TargetMonth");
            DropColumn("dbo.ToDoTasks", "TargetYear");
            DropColumn("dbo.ToDoTasks", "ReApprovalDate");
            DropColumn("dbo.ToDoTasks", "isReApproved");
            DropColumn("dbo.ToDoTasks", "ApprovedDate");
            DropColumn("dbo.ToDoTasks", "isApproved");
            DropColumn("dbo.ToDoTasks", "stage");
            DropTable("dbo.SharedGridGroupEmployees");
            DropTable("dbo.SharedGridGroupDepartments");
            DropTable("dbo.SharedGridGroupCompanies");
            DropTable("dbo.SharedReports");
            DropTable("dbo.SharedGridGroups");
            AddPrimaryKey("dbo.GridReports", new[] { "Id", "userId", "settingkey" });
            CreateIndex("dbo.GridReports", "userId");
            AddForeignKey("dbo.GridReports", "userId", "dbo.Users", "id", cascadeDelete: true);
        }
    }
}
