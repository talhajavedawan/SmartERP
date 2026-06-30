namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class netcommision : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SaleOrders", "netCommision", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.CommissionSummarySheets", "netOfferCommission", c => c.Double(nullable: false));
            AddColumn("dbo.CommissionSummarySheets", "netSOCommission", c => c.Double(nullable: false));
            AddColumn("dbo.SummaryFieldValues", "CommissionSummarySheet_Id", c => c.Int());
            CreateIndex("dbo.SummaryFieldValues", "CommissionSummarySheet_Id");
            AddForeignKey("dbo.SummaryFieldValues", "CommissionSummarySheet_Id", "dbo.CommissionSummarySheets", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SummaryFieldValues", "CommissionSummarySheet_Id", "dbo.CommissionSummarySheets");
            DropIndex("dbo.SummaryFieldValues", new[] { "CommissionSummarySheet_Id" });
            DropColumn("dbo.SummaryFieldValues", "CommissionSummarySheet_Id");
            DropColumn("dbo.CommissionSummarySheets", "netSOCommission");
            DropColumn("dbo.CommissionSummarySheets", "netOfferCommission");
            DropColumn("dbo.SaleOrders", "netCommision");
        }
    }
}
