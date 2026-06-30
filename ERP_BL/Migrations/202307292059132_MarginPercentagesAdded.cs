namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MarginPercentagesAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MarginPercentages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        marginpercentageTypeId = c.Int(),
                        percentage = c.Double(nullable: false),
                        COA_Id = c.Int(),
                        isAdjusted = c.Boolean(nullable: false),
                        isManual = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ChartofAccounts", t => t.COA_Id)
                .ForeignKey("dbo.MarginPercentageTypes", t => t.marginpercentageTypeId)
                .Index(t => t.marginpercentageTypeId)
                .Index(t => t.COA_Id);
            
            CreateTable(
                "dbo.MarginPercentageTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TypeName = c.String(),
                        isActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ReversalSettlements",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        refNO = c.String(),
                        reversalSettlementAmount = c.Double(nullable: false),
                        stl_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.STLs", t => t.stl_Id)
                .Index(t => t.stl_Id);
            
            CreateTable(
                "dbo.STLSettlements",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        refNO = c.String(),
                        settlementAmount = c.Double(nullable: false),
                        stl_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.STLs", t => t.stl_Id)
                .Index(t => t.stl_Id);
            
            AddColumn("dbo.JournalTransactions", "STLId", c => c.Int());
            AddColumn("dbo.STLs", "InterestAmountCD", c => c.Double(nullable: false));
            AddColumn("dbo.STLs", "cashMarginPerc_Id", c => c.Int());
            AddColumn("dbo.STLs", "customer_Id", c => c.Int());
            AddColumn("dbo.STLs", "vendor_Id", c => c.Int());
            CreateIndex("dbo.JournalTransactions", "STLId");
            CreateIndex("dbo.STLs", "cashMarginPerc_Id");
            CreateIndex("dbo.STLs", "customer_Id");
            CreateIndex("dbo.STLs", "vendor_Id");
            AddForeignKey("dbo.STLs", "cashMarginPerc_Id", "dbo.MarginPercentages", "Id");
            AddForeignKey("dbo.STLs", "customer_Id", "dbo.CustomerCompanies", "Id");
            AddForeignKey("dbo.JournalTransactions", "STLId", "dbo.STLs", "Id");
            AddForeignKey("dbo.STLs", "vendor_Id", "dbo.tabVendor", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.STLs", "vendor_Id", "dbo.tabVendor");
            DropForeignKey("dbo.STLSettlements", "stl_Id", "dbo.STLs");
            DropForeignKey("dbo.ReversalSettlements", "stl_Id", "dbo.STLs");
            DropForeignKey("dbo.JournalTransactions", "STLId", "dbo.STLs");
            DropForeignKey("dbo.STLs", "customer_Id", "dbo.CustomerCompanies");
            DropForeignKey("dbo.STLs", "cashMarginPerc_Id", "dbo.MarginPercentages");
            DropForeignKey("dbo.MarginPercentages", "marginpercentageTypeId", "dbo.MarginPercentageTypes");
            DropForeignKey("dbo.MarginPercentages", "COA_Id", "dbo.ChartofAccounts");
            DropIndex("dbo.STLSettlements", new[] { "stl_Id" });
            DropIndex("dbo.ReversalSettlements", new[] { "stl_Id" });
            DropIndex("dbo.MarginPercentages", new[] { "COA_Id" });
            DropIndex("dbo.MarginPercentages", new[] { "marginpercentageTypeId" });
            DropIndex("dbo.STLs", new[] { "vendor_Id" });
            DropIndex("dbo.STLs", new[] { "customer_Id" });
            DropIndex("dbo.STLs", new[] { "cashMarginPerc_Id" });
            DropIndex("dbo.JournalTransactions", new[] { "STLId" });
            DropColumn("dbo.STLs", "vendor_Id");
            DropColumn("dbo.STLs", "customer_Id");
            DropColumn("dbo.STLs", "cashMarginPerc_Id");
            DropColumn("dbo.STLs", "InterestAmountCD");
            DropColumn("dbo.JournalTransactions", "STLId");
            DropTable("dbo.STLSettlements");
            DropTable("dbo.ReversalSettlements");
            DropTable("dbo.MarginPercentageTypes");
            DropTable("dbo.MarginPercentages");
        }
    }
}
