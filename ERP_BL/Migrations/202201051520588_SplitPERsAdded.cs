namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SplitPERsAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SplitPERs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Year = c.DateTime(),
                        Month = c.DateTime(),
                        Amount = c.Double(nullable: false),
                        SaleOrder_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SaleOrders", t => t.SaleOrder_Id)
                .Index(t => t.SaleOrder_Id);
            
            AddColumn("dbo.Payments", "isPostToGL", c => c.Boolean(nullable: false));
            AddColumn("dbo.SalesReceipts", "isPostToGL", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SplitPERs", "SaleOrder_Id", "dbo.SaleOrders");
            DropIndex("dbo.SplitPERs", new[] { "SaleOrder_Id" });
            DropColumn("dbo.SalesReceipts", "isPostToGL");
            DropColumn("dbo.Payments", "isPostToGL");
            DropTable("dbo.SplitPERs");
        }
    }
}
