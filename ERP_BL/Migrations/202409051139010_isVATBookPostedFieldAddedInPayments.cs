namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class isVATBookPostedFieldAddedInPayments : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Payments", "isVATBookPosted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Payments", "isVATBookPosted");
        }
    }
}
