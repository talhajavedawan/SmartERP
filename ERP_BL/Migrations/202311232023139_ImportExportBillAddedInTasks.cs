namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ImportExportBillAddedInTasks : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tasks", "ImportBillOfEntry", c => c.Boolean(nullable: false));
            AddColumn("dbo.Tasks", "ExportBillOfEntry", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tasks", "ExportBillOfEntry");
            DropColumn("dbo.Tasks", "ImportBillOfEntry");
        }
    }
}
