namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesIntabBackground : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tabBackground", "isUpdate", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tabBackground", "isUpdate");
        }
    }
}
