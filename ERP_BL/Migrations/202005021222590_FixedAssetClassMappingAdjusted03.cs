namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixedAssetClassMappingAdjusted03 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PurchaseInfoes", "currencyId", "dbo.Currencies");
            DropIndex("dbo.PurchaseInfoes", new[] { "currencyId" });
            RenameColumn(table: "dbo.PurchaseInfoes", name: "currencyId", newName: "currecncy_Id");
            AlterColumn("dbo.PurchaseInfoes", "currecncy_Id", c => c.Int());
            CreateIndex("dbo.PurchaseInfoes", "currecncy_Id");
            AddForeignKey("dbo.PurchaseInfoes", "currecncy_Id", "dbo.Currencies", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PurchaseInfoes", "currecncy_Id", "dbo.Currencies");
            DropIndex("dbo.PurchaseInfoes", new[] { "currecncy_Id" });
            AlterColumn("dbo.PurchaseInfoes", "currecncy_Id", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.PurchaseInfoes", name: "currecncy_Id", newName: "currencyId");
            CreateIndex("dbo.PurchaseInfoes", "currencyId");
            AddForeignKey("dbo.PurchaseInfoes", "currencyId", "dbo.Currencies", "Id", cascadeDelete: true);
        }
    }
}
