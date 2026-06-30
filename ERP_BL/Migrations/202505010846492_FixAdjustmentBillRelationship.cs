namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixAdjustmentBillRelationship : DbMigration
    {
        public override void Up()
        {
            // Add new column
            AddColumn("dbo.Adjustments", "vendorBill_Id", c => c.Int());

            // Copy data from old to new column
            Sql("UPDATE [dbo].[Adjustments] SET [vendorBill_Id] = [billId]");

            // Drop old relationship
            DropForeignKey("dbo.Adjustments", "billId", "dbo.Bills");
            DropIndex("dbo.Adjustments", "IX_billId");

            // Remove old column
            DropColumn("dbo.Adjustments", "billId");

            // Create new relationship
            CreateIndex("dbo.Adjustments", "vendorBill_Id", name: "IX_vendorBill_Id");
            AddForeignKey("dbo.Adjustments", "vendorBill_Id", "dbo.Bills", "Id");
        }

        public override void Down()
        {
            // Reverse the process
            AddColumn("dbo.Adjustments", "billId", c => c.Int());
            Sql("UPDATE [dbo].[Adjustments] SET [billId] = [vendorBill_Id]");

            DropForeignKey("dbo.Adjustments", "vendorBill_Id", "dbo.Bills");
            DropIndex("dbo.Adjustments", "IX_vendorBill_Id");

            DropColumn("dbo.Adjustments", "vendorBill_Id");

            CreateIndex("dbo.Adjustments", "billId", name: "IX_billId");
            AddForeignKey("dbo.Adjustments", "billId", "dbo.Bills", "Id");
        }
    }
}
