namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PaymentsToPaymentTermsJoin : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Payments", "paymentterm_Id", c => c.Int());
            CreateIndex("dbo.Payments", "paymentterm_Id");
            AddForeignKey("dbo.Payments", "paymentterm_Id", "dbo.PaymentTerms", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Payments", "paymentterm_Id", "dbo.PaymentTerms");
            DropIndex("dbo.Payments", new[] { "paymentterm_Id" });
            DropColumn("dbo.Payments", "paymentterm_Id");
        }
    }
}
