namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class applyMERasSER : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tabDepartment", "applyMERasSER", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tabDepartment", "applyMERasSER");
        }
    }
}
