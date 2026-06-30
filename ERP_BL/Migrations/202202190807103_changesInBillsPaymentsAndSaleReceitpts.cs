namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changesInBillsPaymentsAndSaleReceitpts : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tabAdminBill", "GLPostingDate", c => c.DateTime());
            AddColumn("dbo.Payments", "GLPostingDate", c => c.DateTime());
            AddColumn("dbo.SalesReceipts", "GLPostingDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SalesReceipts", "GLPostingDate");
            DropColumn("dbo.Payments", "GLPostingDate");
            DropColumn("dbo.tabAdminBill", "GLPostingDate");
        }
    }
}
