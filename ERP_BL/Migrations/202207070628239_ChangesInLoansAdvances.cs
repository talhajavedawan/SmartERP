namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesInLoansAdvances : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LoansAdvances", "SystemRef", c => c.String());
            AddColumn("dbo.LoansAdvances", "AppliedAmountOC", c => c.Double(nullable: false));
            AddColumn("dbo.LoansAdvances", "currencyId", c => c.Int());
            AddColumn("dbo.LoansAdvances", "ClosingDate", c => c.DateTime());
            AddColumn("dbo.LoansAdvances", "LastStatusChangeDate", c => c.DateTime());
            CreateIndex("dbo.LoansAdvances", "currencyId");
            AddForeignKey("dbo.LoansAdvances", "currencyId", "dbo.Currencies", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LoansAdvances", "currencyId", "dbo.Currencies");
            DropIndex("dbo.LoansAdvances", new[] { "currencyId" });
            DropColumn("dbo.LoansAdvances", "LastStatusChangeDate");
            DropColumn("dbo.LoansAdvances", "ClosingDate");
            DropColumn("dbo.LoansAdvances", "currencyId");
            DropColumn("dbo.LoansAdvances", "AppliedAmountOC");
            DropColumn("dbo.LoansAdvances", "SystemRef");
        }
    }
}
