namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WarehousesTablesAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Warehouses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        WarehouseName = c.String(),
                        isActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Tasks", "warehouseId", c => c.Int());
            CreateIndex("dbo.Tasks", "warehouseId");
            AddForeignKey("dbo.Tasks", "warehouseId", "dbo.Warehouses", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tasks", "warehouseId", "dbo.Warehouses");
            DropIndex("dbo.Tasks", new[] { "warehouseId" });
            DropColumn("dbo.Tasks", "warehouseId");
            DropTable("dbo.Warehouses");
        }
    }
}
