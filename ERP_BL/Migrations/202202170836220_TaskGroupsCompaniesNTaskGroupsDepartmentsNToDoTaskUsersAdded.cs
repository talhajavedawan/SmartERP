namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TaskGroupsCompaniesNTaskGroupsDepartmentsNToDoTaskUsersAdded : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ToDoTasks", "assignedToId", "dbo.Users");
            DropIndex("dbo.ToDoTasks", new[] { "assignedToId" });
            CreateTable(
                "dbo.TaskGroupsCompanies",
                c => new
                    {
                        TaskGroups_Id = c.Int(nullable: false),
                        Company_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.TaskGroups_Id, t.Company_Id })
                .ForeignKey("dbo.TaskGroups", t => t.TaskGroups_Id, cascadeDelete: true)
                .ForeignKey("dbo.tabCompany", t => t.Company_Id, cascadeDelete: true)
                .Index(t => t.TaskGroups_Id)
                .Index(t => t.Company_Id);
            
            CreateTable(
                "dbo.TaskGroupsDepartments",
                c => new
                    {
                        TaskGroups_Id = c.Int(nullable: false),
                        Department_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.TaskGroups_Id, t.Department_Id })
                .ForeignKey("dbo.TaskGroups", t => t.TaskGroups_Id, cascadeDelete: true)
                .ForeignKey("dbo.tabDepartment", t => t.Department_Id, cascadeDelete: true)
                .Index(t => t.TaskGroups_Id)
                .Index(t => t.Department_Id);
            
            CreateTable(
                "dbo.ToDoTaskUsers",
                c => new
                    {
                        ToDoTask_Id = c.Int(nullable: false),
                        User_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ToDoTask_Id, t.User_id })
                .ForeignKey("dbo.ToDoTasks", t => t.ToDoTask_Id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_id, cascadeDelete: true)
                .Index(t => t.ToDoTask_Id)
                .Index(t => t.User_id);
            
            AddColumn("dbo.Bills", "GLPostingDate", c => c.DateTime());
            AddColumn("dbo.TaskGroups", "parentId", c => c.Int());
            AddColumn("dbo.PurchaseInvoices", "GLPostingDate", c => c.DateTime());
            AddColumn("dbo.SaleInvoices", "GLPostingDate", c => c.DateTime());
            AddColumn("dbo.ToDoTasks", "SystemPoints", c => c.Double(nullable: false));
            AddColumn("dbo.ToDoTasks", "stepCount", c => c.Int(nullable: false));
            AddColumn("dbo.ToDoTasks", "achievedStepsCount", c => c.Int(nullable: false));
            AddColumn("dbo.ToDoTasks", "isVoid", c => c.Boolean(nullable: false));
            AddColumn("dbo.ToDoTasks", "SOField", c => c.String());
            AddColumn("dbo.ToDoTasks", "searchedFromDate", c => c.DateTime());
            AddColumn("dbo.ToDoTasks", "searchedToDate", c => c.DateTime());
            CreateIndex("dbo.TaskGroups", "parentId");
            AddForeignKey("dbo.TaskGroups", "parentId", "dbo.TaskGroups", "Id");
            DropColumn("dbo.ToDoTasks", "assignedToId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ToDoTasks", "assignedToId", c => c.Int());
            DropForeignKey("dbo.ToDoTaskUsers", "User_id", "dbo.Users");
            DropForeignKey("dbo.ToDoTaskUsers", "ToDoTask_Id", "dbo.ToDoTasks");
            DropForeignKey("dbo.TaskGroups", "parentId", "dbo.TaskGroups");
            DropForeignKey("dbo.TaskGroupsDepartments", "Department_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.TaskGroupsDepartments", "TaskGroups_Id", "dbo.TaskGroups");
            DropForeignKey("dbo.TaskGroupsCompanies", "Company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.TaskGroupsCompanies", "TaskGroups_Id", "dbo.TaskGroups");
            DropIndex("dbo.ToDoTaskUsers", new[] { "User_id" });
            DropIndex("dbo.ToDoTaskUsers", new[] { "ToDoTask_Id" });
            DropIndex("dbo.TaskGroupsDepartments", new[] { "Department_Id" });
            DropIndex("dbo.TaskGroupsDepartments", new[] { "TaskGroups_Id" });
            DropIndex("dbo.TaskGroupsCompanies", new[] { "Company_Id" });
            DropIndex("dbo.TaskGroupsCompanies", new[] { "TaskGroups_Id" });
            DropIndex("dbo.TaskGroups", new[] { "parentId" });
            DropColumn("dbo.ToDoTasks", "searchedToDate");
            DropColumn("dbo.ToDoTasks", "searchedFromDate");
            DropColumn("dbo.ToDoTasks", "SOField");
            DropColumn("dbo.ToDoTasks", "isVoid");
            DropColumn("dbo.ToDoTasks", "achievedStepsCount");
            DropColumn("dbo.ToDoTasks", "stepCount");
            DropColumn("dbo.ToDoTasks", "SystemPoints");
            DropColumn("dbo.SaleInvoices", "GLPostingDate");
            DropColumn("dbo.PurchaseInvoices", "GLPostingDate");
            DropColumn("dbo.TaskGroups", "parentId");
            DropColumn("dbo.Bills", "GLPostingDate");
            DropTable("dbo.ToDoTaskUsers");
            DropTable("dbo.TaskGroupsDepartments");
            DropTable("dbo.TaskGroupsCompanies");
            CreateIndex("dbo.ToDoTasks", "assignedToId");
            AddForeignKey("dbo.ToDoTasks", "assignedToId", "dbo.Users", "id");
        }
    }
}
