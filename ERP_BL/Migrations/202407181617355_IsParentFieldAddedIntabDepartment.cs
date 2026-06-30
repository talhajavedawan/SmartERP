namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IsParentFieldAddedIntabDepartment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tabDepartment", "IsParent", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tabDepartment", "IsParent");
        }
    }
}
