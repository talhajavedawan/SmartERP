namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class salesTarget : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.OfferVendors", newName: "VendorOffers");
            RenameTable(name: "dbo.SaleOrderVendors", newName: "VendorSaleOrders");
            DropPrimaryKey("dbo.VendorOffers");
            DropPrimaryKey("dbo.VendorSaleOrders");
            CreateTable(
                "dbo.SalesTargets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StartingDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        TargetAmount = c.Double(nullable: false),
                        CurrencyId = c.Int(),
                        isAchieved = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Currencies", t => t.CurrencyId)
                .Index(t => t.CurrencyId);
            
            AddColumn("dbo.Employees", "SalesTargetId", c => c.Int());
            AddPrimaryKey("dbo.VendorOffers", new[] { "Vendor_Id", "Offer_Id" });
            AddPrimaryKey("dbo.VendorSaleOrders", new[] { "Vendor_Id", "SaleOrder_Id" });
            CreateIndex("dbo.Employees", "SalesTargetId");
            AddForeignKey("dbo.Employees", "SalesTargetId", "dbo.SalesTargets", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Employees", "SalesTargetId", "dbo.SalesTargets");
            DropForeignKey("dbo.SalesTargets", "CurrencyId", "dbo.Currencies");
            DropIndex("dbo.SalesTargets", new[] { "CurrencyId" });
            DropIndex("dbo.Employees", new[] { "SalesTargetId" });
            DropPrimaryKey("dbo.VendorSaleOrders");
            DropPrimaryKey("dbo.VendorOffers");
            DropColumn("dbo.Employees", "SalesTargetId");
            DropTable("dbo.SalesTargets");
            AddPrimaryKey("dbo.VendorSaleOrders", new[] { "SaleOrder_Id", "Vendor_Id" });
            AddPrimaryKey("dbo.VendorOffers", new[] { "Offer_Id", "Vendor_Id" });
            RenameTable(name: "dbo.VendorSaleOrders", newName: "SaleOrderVendors");
            RenameTable(name: "dbo.VendorOffers", newName: "OfferVendors");
        }
    }
}
