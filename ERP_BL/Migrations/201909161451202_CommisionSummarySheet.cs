namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CommisionSummarySheet : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CommentLogs", "User_id", "dbo.Users");
            DropForeignKey("dbo.Users", "CommentLog_Id", "dbo.CommentLogs");
            DropForeignKey("dbo.CommentLogs", "UserId", "dbo.Users");
            DropIndex("dbo.CommentLogs", new[] { "UserId" });
            DropIndex("dbo.CommentLogs", new[] { "User_id" });
            DropIndex("dbo.Users", new[] { "CommentLog_Id" });
            CreateTable(
                "dbo.CommissionSummarySheets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Timestamp = c.DateTime(nullable: false),
                        Info = c.String(),
                        OfferCurrencyId = c.Int(),
                        totalOfferFOB = c.Double(nullable: false),
                        totalOfferCFR = c.Double(nullable: false),
                        OfferDeliveryTerm = c.String(),
                        OfferDeliveryDate = c.DateTime(),
                        OfferPacking = c.String(),
                        OfferCommission = c.Double(nullable: false),
                        TransactionType = c.Int(nullable: false),
                        TransactionId = c.Int(nullable: false),
                        paymenttermWithSupplier = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Currencies", t => t.OfferCurrencyId)
                .Index(t => t.OfferCurrencyId);
            
            
            
            
            AddColumn("dbo.SaleOrders", "transshipment", c => c.Boolean());
            AddColumn("dbo.SaleOrders", "packing", c => c.String());
            AddColumn("dbo.SaleOrders", "LCShipmentDate", c => c.DateTime());
            AddColumn("dbo.SaleOrders", "LCExpiryDate", c => c.DateTime());
            AddColumn("dbo.SaleOrders", "LCAmedmentNo", c => c.String());
            AddColumn("dbo.SaleOrders", "LCShipmentAmendmentDate", c => c.DateTime());
            AddColumn("dbo.SaleOrders", "LCExpiryAmedmentDate", c => c.DateTime());
            AddColumn("dbo.SaleOrders", "CommissionSummarySheetId", c => c.Int());
            
            CreateIndex("dbo.SaleOrders", "CommissionSummarySheetId");
            AddForeignKey("dbo.SaleOrders", "CommissionSummarySheetId", "dbo.CommissionSummarySheets", "Id");
            
           
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "CommentLog_Id", c => c.Int());
            AddColumn("dbo.CommentLogs", "User_id", c => c.Int());
            AddColumn("dbo.CommentLogs", "UserId", c => c.Int(nullable: false));
            DropForeignKey("dbo.CommentLogs", "employeeId", "dbo.Employees");
            DropForeignKey("dbo.SaleOrders", "CommissionSummarySheetId", "dbo.CommissionSummarySheets");
            DropForeignKey("dbo.CommissionSummarySheets", "OfferCurrencyId", "dbo.Currencies");
            DropIndex("dbo.CommissionSummarySheets", new[] { "OfferCurrencyId" });
            DropIndex("dbo.SaleOrders", new[] { "CommissionSummarySheetId" });
            DropIndex("dbo.CommentLogs", new[] { "employeeId" });
            DropColumn("dbo.SaleOrders", "CommissionSummarySheetId");
            DropColumn("dbo.SaleOrders", "LCExpiryAmedmentDate");
            DropColumn("dbo.SaleOrders", "LCShipmentAmendmentDate");
            DropColumn("dbo.SaleOrders", "LCAmedmentNo");
            DropColumn("dbo.SaleOrders", "LCExpiryDate");
            DropColumn("dbo.SaleOrders", "LCShipmentDate");
            DropColumn("dbo.SaleOrders", "packing");
            DropColumn("dbo.SaleOrders", "transshipment");
            DropColumn("dbo.CommentLogs", "employeeId");
            DropTable("dbo.UserCommentLogs");
            DropTable("dbo.CommissionSummarySheets");
            CreateIndex("dbo.Users", "CommentLog_Id");
            CreateIndex("dbo.CommentLogs", "User_id");
            CreateIndex("dbo.CommentLogs", "UserId");
            AddForeignKey("dbo.CommentLogs", "UserId", "dbo.Users", "id", cascadeDelete: true);
            AddForeignKey("dbo.Users", "CommentLog_Id", "dbo.CommentLogs", "Id");
            AddForeignKey("dbo.CommentLogs", "User_id", "dbo.Users", "id");
        }
    }
}
