namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesInTargetRewards : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TargetRewards", "toDoTaskOthers_Id", "dbo.ToDoTasks");
            DropForeignKey("dbo.TargetRewards", "toDoTaskSales_Id", "dbo.ToDoTasks");
            DropIndex("dbo.TargetRewards", new[] { "toDoTaskSales_Id" });
            DropIndex("dbo.TargetRewards", new[] { "toDoTaskOthers_Id" });
            RenameColumn(table: "dbo.TargetRewards", name: "toDoTaskFinance_Id", newName: "toDoTask_Id");
            RenameIndex(table: "dbo.TargetRewards", name: "IX_toDoTaskFinance_Id", newName: "IX_toDoTask_Id");
            AddColumn("dbo.TargetRewards", "functionType", c => c.Int(nullable: false));
            AlterColumn("dbo.Tasks", "taxYear", c => c.DateTime());
            DropColumn("dbo.TargetRewards", "toDoTaskSales_Id");
            DropColumn("dbo.TargetRewards", "toDoTaskOthers_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TargetRewards", "toDoTaskOthers_Id", c => c.Int());
            AddColumn("dbo.TargetRewards", "toDoTaskSales_Id", c => c.Int());
            AlterColumn("dbo.Tasks", "taxYear", c => c.DateTime(nullable: false));
            DropColumn("dbo.TargetRewards", "functionType");
            RenameIndex(table: "dbo.TargetRewards", name: "IX_toDoTask_Id", newName: "IX_toDoTaskFinance_Id");
            RenameColumn(table: "dbo.TargetRewards", name: "toDoTask_Id", newName: "toDoTaskFinance_Id");
            CreateIndex("dbo.TargetRewards", "toDoTaskOthers_Id");
            CreateIndex("dbo.TargetRewards", "toDoTaskSales_Id");
            AddForeignKey("dbo.TargetRewards", "toDoTaskSales_Id", "dbo.ToDoTasks", "Id");
            AddForeignKey("dbo.TargetRewards", "toDoTaskOthers_Id", "dbo.ToDoTasks", "Id");
        }
    }
}
