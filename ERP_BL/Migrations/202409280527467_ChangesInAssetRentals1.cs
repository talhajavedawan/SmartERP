namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangesInAssetRentals1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AssetRentals", "PurchasingDate", c => c.DateTime());
            AlterColumn("dbo.AssetRentals", "ProgressiveCost", c => c.Double());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AssetRentals", "ProgressiveCost", c => c.String());
            DropColumn("dbo.AssetRentals", "PurchasingDate");
        }
    }
}
