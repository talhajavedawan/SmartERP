namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changesInSalesReceiptsColumns : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SalesReceipts", "isBypassBank", c => c.Boolean(nullable: false));
            AddColumn("dbo.SalesReceipts", "coaAccountId", c => c.Int());
            CreateIndex("dbo.SalesReceipts", "coaAccountId");
            AddForeignKey("dbo.SalesReceipts", "coaAccountId", "dbo.ChartofAccounts", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SalesReceipts", "coaAccountId", "dbo.ChartofAccounts");
            DropIndex("dbo.SalesReceipts", new[] { "coaAccountId" });
            DropColumn("dbo.SalesReceipts", "coaAccountId");
            DropColumn("dbo.SalesReceipts", "isBypassBank");
        }
    }
}
