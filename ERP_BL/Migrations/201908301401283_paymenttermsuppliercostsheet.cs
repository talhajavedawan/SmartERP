namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class paymenttermsuppliercostsheet : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CostSheets", "paymenttermWithSupplier", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CostSheets", "paymenttermWithSupplier");
        }
    }
}
