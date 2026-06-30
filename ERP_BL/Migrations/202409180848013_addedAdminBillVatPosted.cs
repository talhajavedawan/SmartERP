namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedAdminBillVatPosted : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tabAdminBill", "isVATBookPosted", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tabAdminBill", "isVATBookPosted");
        }
    }
}
