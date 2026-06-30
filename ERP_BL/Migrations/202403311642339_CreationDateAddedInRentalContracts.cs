namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreationDateAddedInRentalContracts : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.RentalContracts", "parentId", "dbo.AssetRentals");
            DropIndex("dbo.RentalContracts", new[] { "parentId" });
            AddColumn("dbo.RentalContracts", "CreationDate", c => c.DateTime());
            DropColumn("dbo.RentalContracts", "isRentable");
            DropColumn("dbo.RentalContracts", "isSubsidary");
            DropColumn("dbo.RentalContracts", "parentId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.RentalContracts", "parentId", c => c.Int());
            AddColumn("dbo.RentalContracts", "isSubsidary", c => c.Boolean(nullable: false));
            AddColumn("dbo.RentalContracts", "isRentable", c => c.Boolean(nullable: false));
            DropColumn("dbo.RentalContracts", "CreationDate");
            CreateIndex("dbo.RentalContracts", "parentId");
            AddForeignKey("dbo.RentalContracts", "parentId", "dbo.AssetRentals", "Id");
        }
    }
}
