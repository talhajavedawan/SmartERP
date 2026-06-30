namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VendorBillAdjustmentsAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.VendorBillAdjustments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        loansAdvanceType = c.Int(nullable: false),
                        transactionGroupId = c.Int(nullable: false),
                        AdjustmentDate = c.DateTime(),
                        ReferenceNo = c.String(),
                        AdjustmentAmount = c.Double(nullable: false),
                        bill_Id = c.Int(),
                        isApproved = c.Boolean(),
                        ApprovedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Bills", t => t.bill_Id)
                .Index(t => t.bill_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VendorBillAdjustments", "bill_Id", "dbo.Bills");
            DropIndex("dbo.VendorBillAdjustments", new[] { "bill_Id" });
            DropTable("dbo.VendorBillAdjustments");
        }
    }
}
