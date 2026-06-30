namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class paymentsToBillConnection : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Payments", "Bill_Id", c => c.Int());
            CreateIndex("dbo.Payments", "Bill_Id");
            AddForeignKey("dbo.Payments", "Bill_Id", "dbo.Bills", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Payments", "Bill_Id", "dbo.Bills");
            DropIndex("dbo.Payments", new[] { "Bill_Id" });
            DropColumn("dbo.Payments", "Bill_Id");
        }
    }
}
