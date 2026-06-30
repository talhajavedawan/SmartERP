namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ConnectionForAssetRentalsAndadress : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.AssetRentals", name: "adress_Id", newName: "address_Id");
            RenameIndex(table: "dbo.AssetRentals", name: "IX_adress_Id", newName: "IX_address_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.AssetRentals", name: "IX_address_Id", newName: "IX_adress_Id");
            RenameColumn(table: "dbo.AssetRentals", name: "address_Id", newName: "adress_Id");
        }
    }
}
