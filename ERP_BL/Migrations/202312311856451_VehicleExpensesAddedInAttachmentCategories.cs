namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VehicleExpensesAddedInAttachmentCategories : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AttachmentCategories", "VehicleExpenses", c => c.Int());
        }
public override void Down()
        {
            DropColumn("dbo.AttachmentCategories", "VehicleExpenses");
        }
    }
}
