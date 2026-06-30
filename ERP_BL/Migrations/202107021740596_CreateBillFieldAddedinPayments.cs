namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateBillFieldAddedinPayments : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Payments", "createdFromBill", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Payments", "createdFromBill");
        }
    }
}
