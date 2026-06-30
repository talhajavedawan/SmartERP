namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesInTodoAndUsers : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.ToDoTasks", new[] { "calculationType_Id" });
            AddColumn("dbo.Users", "isRDCKeyApproved", c => c.Boolean(nullable: false));
            AddColumn("dbo.Users", "rdcMachineKey", c => c.String());
            AddColumn("dbo.ToDoTasks", "TotalQuantity", c => c.Double());
            AddColumn("dbo.ToDoTaskStatus", "MinPercentage", c => c.Double(nullable: false));
            AddColumn("dbo.ToDoTaskStatus", "MaxPercentage", c => c.Double(nullable: false));
            CreateIndex("dbo.ToDoTasks", "CalculationType_Id");
            DropColumn("dbo.StatusCalculationTypes", "stepType");
            DropColumn("dbo.ToDoTaskStatus", "Percentage");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ToDoTaskStatus", "Percentage", c => c.Double());
            AddColumn("dbo.StatusCalculationTypes", "stepType", c => c.Int(nullable: false));
            DropIndex("dbo.ToDoTasks", new[] { "CalculationType_Id" });
            DropColumn("dbo.ToDoTaskStatus", "MaxPercentage");
            DropColumn("dbo.ToDoTaskStatus", "MinPercentage");
            DropColumn("dbo.ToDoTasks", "TotalQuantity");
            DropColumn("dbo.Users", "rdcMachineKey");
            DropColumn("dbo.Users", "isRDCKeyApproved");
            CreateIndex("dbo.ToDoTasks", "calculationType_Id");
        }
    }
}
