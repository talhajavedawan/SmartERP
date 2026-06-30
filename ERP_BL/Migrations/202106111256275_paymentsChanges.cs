namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class paymentsChanges : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Payments", "transactionType", c => c.Int(nullable: false));
            AddColumn("dbo.Payments", "paymentAdminBillTemplate", c => c.Int(nullable: false));
            AddColumn("dbo.Payments", "transactionGroupId", c => c.Int(nullable: false));
            AddColumn("dbo.Payments", "AdminBill_Id", c => c.Int());
            CreateIndex("dbo.Payments", "AdminBill_Id");
            AddForeignKey("dbo.Payments", "AdminBill_Id", "dbo.tabAdminBill", "Id");
            DropColumn("dbo.Payments", "paymentType");
            DropColumn("dbo.Payments", "TransactionType_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Payments", "TransactionType_Id", c => c.Int(nullable: false));
            AddColumn("dbo.Payments", "paymentType", c => c.Int(nullable: false));
            DropForeignKey("dbo.Payments", "AdminBill_Id", "dbo.tabAdminBill");
            DropIndex("dbo.Payments", new[] { "AdminBill_Id" });
            DropColumn("dbo.Payments", "AdminBill_Id");
            DropColumn("dbo.Payments", "transactionGroupId");
            DropColumn("dbo.Payments", "paymentAdminBillTemplate");
            DropColumn("dbo.Payments", "transactionType");
        }
    }
}
