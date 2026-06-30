namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class isTaskTypeAddedInEmployee : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employees", "isTaskType", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Employees", "isTaskType");
        }
    }
}
