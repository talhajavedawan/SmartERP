namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changesForLoanAdvance : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bills", "LoansAdvanceId", c => c.Int());
            AddColumn("dbo.Adjustments", "loansAdvanceType", c => c.Int(nullable: false));
            AddColumn("dbo.Adjustments", "billId", c => c.Int());
            AddColumn("dbo.LoansAdvances", "loansAdvanceType", c => c.Int(nullable: false));
            AddColumn("dbo.LoansAdvances", "vendorId", c => c.Int());
            CreateIndex("dbo.Bills", "LoansAdvanceId");
            CreateIndex("dbo.Adjustments", "billId");
            CreateIndex("dbo.LoansAdvances", "vendorId");
            AddForeignKey("dbo.LoansAdvances", "vendorId", "dbo.tabVendor", "Id");
            AddForeignKey("dbo.Adjustments", "billId", "dbo.Bills", "Id");
            AddForeignKey("dbo.Bills", "LoansAdvanceId", "dbo.LoansAdvances", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Bills", "LoansAdvanceId", "dbo.LoansAdvances");
            DropForeignKey("dbo.Adjustments", "billId", "dbo.Bills");
            DropForeignKey("dbo.LoansAdvances", "vendorId", "dbo.tabVendor");
            DropIndex("dbo.LoansAdvances", new[] { "vendorId" });
            DropIndex("dbo.Adjustments", new[] { "billId" });
            DropIndex("dbo.Bills", new[] { "LoansAdvanceId" });
            DropColumn("dbo.LoansAdvances", "vendorId");
            DropColumn("dbo.LoansAdvances", "loansAdvanceType");
            DropColumn("dbo.Adjustments", "billId");
            DropColumn("dbo.Adjustments", "loansAdvanceType");
            DropColumn("dbo.Bills", "LoansAdvanceId");
        }
    }
}
