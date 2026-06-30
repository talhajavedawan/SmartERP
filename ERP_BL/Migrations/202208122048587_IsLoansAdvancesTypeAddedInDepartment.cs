namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IsLoansAdvancesTypeAddedInDepartment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tabDepartment", "IsLoansAdvancesType", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tabDepartment", "IsLoansAdvancesType");
        }
    }
}
