namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdminBillNewFieldAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tabAdminBill", "isPostToGL", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tabAdminBill", "isPostToGL");
        }
    }
}
