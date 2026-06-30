namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesInTodoTaskAndEmployee : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employees", "receivableAccountId", c => c.Int());
            AddColumn("dbo.ToDoTasks", "ManualBudgetedMarginME", c => c.Double(nullable: false));
            AddColumn("dbo.ToDoTasks", "ManualBudgetedMarginSE", c => c.Double(nullable: false));
            AddColumn("dbo.ToDoTasks", "ManualActualMarginME", c => c.Double(nullable: false));
            AddColumn("dbo.ToDoTasks", "ManualActualMarginSE", c => c.Double(nullable: false));
            AddColumn("dbo.ToDoTasks", "ManualCommissionME", c => c.Double(nullable: false));
            AddColumn("dbo.ToDoTasks", "ManualCommissionSE", c => c.Double(nullable: false));
            AddColumn("dbo.ToDoTasks", "commisioninSE", c => c.Double(nullable: false));
            AddColumn("dbo.ToDoTasks", "systemMarginOC", c => c.Double(nullable: false));
            AddColumn("dbo.ToDoTasks", "ManualSystemMarginSE", c => c.Double(nullable: false));
            AddColumn("dbo.ToDoTasks", "systemMarginSE", c => c.Double(nullable: false));
            AddColumn("dbo.ToDoTasks", "ManualSystemMarginME", c => c.Double(nullable: false));
            AddColumn("dbo.ToDoTasks", "systemMarginME", c => c.Double(nullable: false));
            CreateIndex("dbo.Employees", "receivableAccountId");
            AddForeignKey("dbo.Employees", "receivableAccountId", "dbo.ChartofAccounts", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Employees", "receivableAccountId", "dbo.ChartofAccounts");
            DropIndex("dbo.Employees", new[] { "receivableAccountId" });
            DropColumn("dbo.ToDoTasks", "systemMarginME");
            DropColumn("dbo.ToDoTasks", "ManualSystemMarginME");
            DropColumn("dbo.ToDoTasks", "systemMarginSE");
            DropColumn("dbo.ToDoTasks", "ManualSystemMarginSE");
            DropColumn("dbo.ToDoTasks", "systemMarginOC");
            DropColumn("dbo.ToDoTasks", "commisioninSE");
            DropColumn("dbo.ToDoTasks", "ManualCommissionSE");
            DropColumn("dbo.ToDoTasks", "ManualCommissionME");
            DropColumn("dbo.ToDoTasks", "ManualActualMarginSE");
            DropColumn("dbo.ToDoTasks", "ManualActualMarginME");
            DropColumn("dbo.ToDoTasks", "ManualBudgetedMarginSE");
            DropColumn("dbo.ToDoTasks", "ManualBudgetedMarginME");
            DropColumn("dbo.Employees", "receivableAccountId");
        }
    }
}
