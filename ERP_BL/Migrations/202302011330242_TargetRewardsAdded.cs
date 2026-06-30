namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TargetRewardsAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TargetRewards",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        user_Id = c.Int(),
                        RewardAmount = c.Double(nullable: false),
                        toDoTaskSales_Id = c.Int(),
                        toDoTaskFinance_Id = c.Int(),
                        toDoTaskOthers_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ToDoTasks", t => t.toDoTaskFinance_Id)
                .ForeignKey("dbo.ToDoTasks", t => t.toDoTaskOthers_Id)
                .ForeignKey("dbo.ToDoTasks", t => t.toDoTaskSales_Id)
                .ForeignKey("dbo.Users", t => t.user_Id)
                .Index(t => t.user_Id)
                .Index(t => t.toDoTaskSales_Id)
                .Index(t => t.toDoTaskFinance_Id)
                .Index(t => t.toDoTaskOthers_Id);
            
            AddColumn("dbo.ToDoTasks", "TotalOrdersCount", c => c.Int(nullable: false));
            AddColumn("dbo.ToDoTasks", "UnderApprovalOrdersCount", c => c.Int(nullable: false));
            AddColumn("dbo.ToDoTasks", "ApprovedOrdersCount", c => c.Int(nullable: false));
            AddColumn("dbo.ToDoTasks", "UnderClosingOrdersCount", c => c.Int(nullable: false));
            AddColumn("dbo.ToDoTasks", "ClosedOrdersCount", c => c.Int(nullable: false));
            AddColumn("dbo.Functions", "functionType", c => c.Int());
            DropTable("dbo.ReceiptCOAs");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ReceiptCOAs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.TargetRewards", "user_Id", "dbo.Users");
            DropForeignKey("dbo.TargetRewards", "toDoTaskSales_Id", "dbo.ToDoTasks");
            DropForeignKey("dbo.TargetRewards", "toDoTaskOthers_Id", "dbo.ToDoTasks");
            DropForeignKey("dbo.TargetRewards", "toDoTaskFinance_Id", "dbo.ToDoTasks");
            DropIndex("dbo.TargetRewards", new[] { "toDoTaskOthers_Id" });
            DropIndex("dbo.TargetRewards", new[] { "toDoTaskFinance_Id" });
            DropIndex("dbo.TargetRewards", new[] { "toDoTaskSales_Id" });
            DropIndex("dbo.TargetRewards", new[] { "user_Id" });
            DropColumn("dbo.Functions", "functionType");
            DropColumn("dbo.ToDoTasks", "ClosedOrdersCount");
            DropColumn("dbo.ToDoTasks", "UnderClosingOrdersCount");
            DropColumn("dbo.ToDoTasks", "ApprovedOrdersCount");
            DropColumn("dbo.ToDoTasks", "UnderApprovalOrdersCount");
            DropColumn("dbo.ToDoTasks", "TotalOrdersCount");
            DropTable("dbo.TargetRewards");
        }
    }
}
