namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SaleReceiptStatusAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SalesReceiptStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.String(),
                        isApproved = c.Boolean(nullable: false),
                        isActive = c.Boolean(),
                        backcolor = c.String(),
                        forecolor = c.String(),
                        HierarchicalIndex = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.SalesReceipts", "saleReceiptStatus_Id", c => c.Int());
            CreateIndex("dbo.SalesReceipts", "saleReceiptStatus_Id");
            AddForeignKey("dbo.SalesReceipts", "saleReceiptStatus_Id", "dbo.SalesReceiptStatus", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SalesReceipts", "saleReceiptStatus_Id", "dbo.SalesReceiptStatus");
            DropIndex("dbo.SalesReceipts", new[] { "saleReceiptStatus_Id" });
            DropColumn("dbo.SalesReceipts", "saleReceiptStatus_Id");
            DropTable("dbo.SalesReceiptStatus");
        }
    }
}
