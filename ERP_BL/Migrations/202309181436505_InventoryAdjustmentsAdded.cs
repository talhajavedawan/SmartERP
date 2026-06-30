namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InventoryAdjustmentsAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.InventoryAdjustments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ReferenceNo = c.String(),
                        CreationDate = c.DateTime(),
                        AdjustmentType = c.Int(nullable: false),
                        creator_Id = c.Int(),
                        company_Id = c.Int(),
                        depId_Id = c.Int(),
                        currency_Id = c.Int(nullable: false),
                        chartofAccount_Id = c.Int(nullable: false),
                        AdjustmentDate = c.DateTime(),
                        PendingForClosing = c.Boolean(),
                        PendingForReApproval = c.Boolean(),
                        isApproved = c.Boolean(),
                        ReApprovalDate = c.DateTime(),
                        isReApproved = c.Boolean(),
                        ApprovedDate = c.DateTime(),
                        isVoid = c.Boolean(nullable: false),
                        stage = c.String(),
                        ClosingDate = c.DateTime(),
                        LastStatusChangeDate = c.DateTime(),
                        isReviewed = c.Boolean(),
                        needReview = c.Boolean(),
                        AdjustmentStatus_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.InventoryAdjustmentStatus", t => t.AdjustmentStatus_Id)
                .ForeignKey("dbo.ChartofAccounts", t => t.chartofAccount_Id, cascadeDelete: true)
                .ForeignKey("dbo.tabCompany", t => t.company_Id)
                .ForeignKey("dbo.Users", t => t.creator_Id)
                .ForeignKey("dbo.Currencies", t => t.currency_Id, cascadeDelete: true)
                .ForeignKey("dbo.tabDepartment", t => t.depId_Id)
                .Index(t => t.creator_Id)
                .Index(t => t.company_Id)
                .Index(t => t.depId_Id)
                .Index(t => t.currency_Id)
                .Index(t => t.chartofAccount_Id)
                .Index(t => t.AdjustmentStatus_Id);
            
            CreateTable(
                "dbo.InventoryAdjustmentStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.String(),
                        isApproved = c.Boolean(nullable: false),
                        isActive = c.Boolean(),
                        backcolor = c.String(),
                        forecolor = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Inventories", "moduleType", c => c.Int(nullable: false));
            AddColumn("dbo.Inventories", "isAdjusted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Inventories", "adjustment_Id", c => c.Int());
            CreateIndex("dbo.Inventories", "adjustment_Id");
            AddForeignKey("dbo.Inventories", "adjustment_Id", "dbo.InventoryAdjustments", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Inventories", "adjustment_Id", "dbo.InventoryAdjustments");
            DropForeignKey("dbo.InventoryAdjustments", "depId_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.InventoryAdjustments", "currency_Id", "dbo.Currencies");
            DropForeignKey("dbo.InventoryAdjustments", "creator_Id", "dbo.Users");
            DropForeignKey("dbo.InventoryAdjustments", "company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.InventoryAdjustments", "chartofAccount_Id", "dbo.ChartofAccounts");
            DropForeignKey("dbo.InventoryAdjustments", "AdjustmentStatus_Id", "dbo.InventoryAdjustmentStatus");
            DropIndex("dbo.InventoryAdjustments", new[] { "AdjustmentStatus_Id" });
            DropIndex("dbo.InventoryAdjustments", new[] { "chartofAccount_Id" });
            DropIndex("dbo.InventoryAdjustments", new[] { "currency_Id" });
            DropIndex("dbo.InventoryAdjustments", new[] { "depId_Id" });
            DropIndex("dbo.InventoryAdjustments", new[] { "company_Id" });
            DropIndex("dbo.InventoryAdjustments", new[] { "creator_Id" });
            DropIndex("dbo.Inventories", new[] { "adjustment_Id" });
            DropColumn("dbo.Inventories", "adjustment_Id");
            DropColumn("dbo.Inventories", "isAdjusted");
            DropColumn("dbo.Inventories", "moduleType");
            DropTable("dbo.InventoryAdjustmentStatus");
            DropTable("dbo.InventoryAdjustments");
        }
    }
}
