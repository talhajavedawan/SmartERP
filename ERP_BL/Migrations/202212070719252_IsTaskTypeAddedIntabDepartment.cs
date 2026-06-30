namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IsTaskTypeAddedIntabDepartment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tabDepartment", "IsTaskType", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tabDepartment", "IsTaskType");
        }
    }
}
