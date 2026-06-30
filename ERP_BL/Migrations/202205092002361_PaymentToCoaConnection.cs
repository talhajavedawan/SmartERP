namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PaymentToCoaConnection : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Payments", "isBypassBank", c => c.Boolean(nullable: false));
            AddColumn("dbo.Payments", "coaAccountId", c => c.Int());
            CreateIndex("dbo.Payments", "coaAccountId");
            AddForeignKey("dbo.Payments", "coaAccountId", "dbo.ChartofAccounts", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Payments", "coaAccountId", "dbo.ChartofAccounts");
            DropIndex("dbo.Payments", new[] { "coaAccountId" });
            DropColumn("dbo.Payments", "coaAccountId");
            DropColumn("dbo.Payments", "isBypassBank");
        }
    }
}
