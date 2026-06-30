namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdjustmentsAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Adjustments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        transactionGroupId = c.Int(nullable: false),
                        AdjustmentDate = c.DateTime(),
                        ReferenceNo = c.String(),
                        AdjustmentAmount = c.Double(nullable: false),
                        adminBillId = c.Int(),
                        isApproved = c.Boolean(),
                        ApprovedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tabAdminBill", t => t.adminBillId)
                .Index(t => t.adminBillId);
            
            AddColumn("dbo.tabAdminBill", "LoansAdvanceId", c => c.Int());
            CreateIndex("dbo.tabAdminBill", "LoansAdvanceId");
            AddForeignKey("dbo.tabAdminBill", "LoansAdvanceId", "dbo.LoansAdvances", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tabAdminBill", "LoansAdvanceId", "dbo.LoansAdvances");
            DropForeignKey("dbo.Adjustments", "adminBillId", "dbo.tabAdminBill");
            DropIndex("dbo.Adjustments", new[] { "adminBillId" });
            DropIndex("dbo.tabAdminBill", new[] { "LoansAdvanceId" });
            DropColumn("dbo.tabAdminBill", "LoansAdvanceId");
            DropTable("dbo.Adjustments");
        }
    }
}
