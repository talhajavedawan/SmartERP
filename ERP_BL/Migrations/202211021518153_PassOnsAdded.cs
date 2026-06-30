namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PassOnsAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PassOns",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        passOnName = c.String(),
                        isActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.BookerStatementItems", "saleOrderId", c => c.Int());
            AddColumn("dbo.BookerStatementItems", "passOnId", c => c.Int());
            CreateIndex("dbo.BookerStatementItems", "saleOrderId");
            CreateIndex("dbo.BookerStatementItems", "passOnId");
            AddForeignKey("dbo.BookerStatementItems", "passOnId", "dbo.PassOns", "Id");
            AddForeignKey("dbo.BookerStatementItems", "saleOrderId", "dbo.SaleOrders", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BookerStatementItems", "saleOrderId", "dbo.SaleOrders");
            DropForeignKey("dbo.BookerStatementItems", "passOnId", "dbo.PassOns");
            DropIndex("dbo.BookerStatementItems", new[] { "passOnId" });
            DropIndex("dbo.BookerStatementItems", new[] { "saleOrderId" });
            DropColumn("dbo.BookerStatementItems", "passOnId");
            DropColumn("dbo.BookerStatementItems", "saleOrderId");
            DropTable("dbo.PassOns");
        }
    }
}
