namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesInToDoTasks1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ToDoTasks", "SOField1", c => c.String());
            AddColumn("dbo.ToDoTasks", "searchedFromDate1", c => c.DateTime());
            AddColumn("dbo.ToDoTasks", "searchedToDate1", c => c.DateTime());
            AddColumn("dbo.ToDoTasks", "SystemPoints1", c => c.Double(nullable: false));
            AddColumn("dbo.ToDoTasks", "SOField2", c => c.String());
            AddColumn("dbo.ToDoTasks", "searchedFromDate2", c => c.DateTime());
            AddColumn("dbo.ToDoTasks", "searchedToDate2", c => c.DateTime());
            AddColumn("dbo.ToDoTasks", "SystemPoints2", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ToDoTasks", "SystemPoints2");
            DropColumn("dbo.ToDoTasks", "searchedToDate2");
            DropColumn("dbo.ToDoTasks", "searchedFromDate2");
            DropColumn("dbo.ToDoTasks", "SOField2");
            DropColumn("dbo.ToDoTasks", "SystemPoints1");
            DropColumn("dbo.ToDoTasks", "searchedToDate1");
            DropColumn("dbo.ToDoTasks", "searchedFromDate1");
            DropColumn("dbo.ToDoTasks", "SOField1");
        }
    }
}
