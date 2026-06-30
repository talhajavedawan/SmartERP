namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TaxAddedInTasks : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tasks", "TaxFrom", c => c.DateTime());
            AddColumn("dbo.Tasks", "TaxTo", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tasks", "TaxTo");
            DropColumn("dbo.Tasks", "TaxFrom");
        }
    }
}
