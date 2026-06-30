namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TaskGroupsAndTaskTargetTypesAndToDoTasksADDED : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.InterBankTransfers", name: "interBankTransStatus_Id", newName: "statusId");
            RenameIndex(table: "dbo.InterBankTransfers", name: "IX_interBankTransStatus_Id", newName: "IX_statusId");
            CreateTable(
                "dbo.TaskGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        creationDate = c.DateTime(nullable: false),
                        GroupName = c.String(),
                        GroupCreatorId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.GroupCreatorId)
                .Index(t => t.GroupCreatorId);
            
            CreateTable(
                "dbo.TaskTargetTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TargetTypeName = c.String(),
                        isActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ToDoTasks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        creationDate = c.DateTime(nullable: false),
                        TaskCreatorId = c.Int(),
                        TaskName = c.String(),
                        TaskDescription = c.String(),
                        taskGroupId = c.Int(),
                        parentTaskId = c.Int(),
                        assignedToId = c.Int(),
                        targetTypeId = c.Int(),
                        isImportant = c.Boolean(nullable: false),
                        isCompleted = c.Boolean(nullable: false),
                        StartDate = c.DateTime(),
                        TentativeClosingDate = c.DateTime(),
                        ActualClosingDate = c.DateTime(),
                        TaskPoints = c.Int(nullable: false),
                        StepDueDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.assignedToId)
                .ForeignKey("dbo.ToDoTasks", t => t.parentTaskId)
                .ForeignKey("dbo.Users", t => t.TaskCreatorId)
                .ForeignKey("dbo.TaskGroups", t => t.taskGroupId)
                .ForeignKey("dbo.TaskTargetTypes", t => t.targetTypeId)
                .Index(t => t.TaskCreatorId)
                .Index(t => t.taskGroupId)
                .Index(t => t.parentTaskId)
                .Index(t => t.assignedToId)
                .Index(t => t.targetTypeId);
            
            CreateTable(
                "dbo.ToDoTaskThemes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ThemeColorCode = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TaskGroupsUsers",
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
            
            AddColumn("dbo.SaleOrders", "SystemMargin", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.SaleOrders", "SalesSystemMargin", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.SaleOrders", "SalesMarketMargin", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ToDoTasks", "targetTypeId", "dbo.TaskTargetTypes");
            DropForeignKey("dbo.ToDoTasks", "taskGroupId", "dbo.TaskGroups");
            DropForeignKey("dbo.ToDoTasks", "TaskCreatorId", "dbo.Users");
            DropForeignKey("dbo.ToDoTasks", "parentTaskId", "dbo.ToDoTasks");
            DropForeignKey("dbo.ToDoTasks", "assignedToId", "dbo.Users");
            DropForeignKey("dbo.TaskGroupsUsers", "User_id", "dbo.Users");
            DropForeignKey("dbo.TaskGroupsUsers", "TaskGroups_Id", "dbo.TaskGroups");
            DropForeignKey("dbo.TaskGroups", "GroupCreatorId", "dbo.Users");
            DropIndex("dbo.TaskGroupsUsers", new[] { "User_id" });
            DropIndex("dbo.TaskGroupsUsers", new[] { "TaskGroups_Id" });
            DropIndex("dbo.ToDoTasks", new[] { "targetTypeId" });
            DropIndex("dbo.ToDoTasks", new[] { "assignedToId" });
            DropIndex("dbo.ToDoTasks", new[] { "parentTaskId" });
            DropIndex("dbo.ToDoTasks", new[] { "taskGroupId" });
            DropIndex("dbo.ToDoTasks", new[] { "TaskCreatorId" });
            DropIndex("dbo.TaskGroups", new[] { "GroupCreatorId" });
            DropColumn("dbo.SaleOrders", "SalesMarketMargin");
            DropColumn("dbo.SaleOrders", "SalesSystemMargin");
            DropColumn("dbo.SaleOrders", "SystemMargin");
            DropTable("dbo.TaskGroupsUsers");
            DropTable("dbo.ToDoTaskThemes");
            DropTable("dbo.ToDoTasks");
            DropTable("dbo.TaskTargetTypes");
            DropTable("dbo.TaskGroups");
            RenameIndex(table: "dbo.InterBankTransfers", name: "IX_statusId", newName: "IX_interBankTransStatus_Id");
            RenameColumn(table: "dbo.InterBankTransfers", name: "statusId", newName: "interBankTransStatus_Id");
        }
    }
}
