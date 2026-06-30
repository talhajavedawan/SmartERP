namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesInToDoTasks : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ToDoTasks", "GroupCompanies", c => c.String());
            AddColumn("dbo.ToDoTasks", "GroupDepartments", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ToDoTasks", "GroupDepartments");
            DropColumn("dbo.ToDoTasks", "GroupCompanies");
        }
    }
}
