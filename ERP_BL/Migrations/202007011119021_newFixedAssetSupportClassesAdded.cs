namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newFixedAssetSupportClassesAdded : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.BuyingStatus", newName: "VehicleBuyingStatus");
            DropForeignKey("dbo.ConditionImages", "Vehicle_Id", "dbo.Vehicles");
            DropIndex("dbo.ConditionImages", new[] { "Vehicle_Id" });
            RenameColumn(table: "dbo.ConditionImages", name: "Vehicle_Id", newName: "vehicleId");
            CreateTable(
                "dbo.Revaluations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        assetId = c.Int(nullable: false),
                        revaluationDate = c.DateTime(nullable: false),
                        amountMR = c.Double(nullable: false),
                        MER = c.Double(nullable: false),
                        amountMER = c.Double(nullable: false),
                        transGroupID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Assets", t => t.assetId, cascadeDelete: true)
                .Index(t => t.assetId);
            
            CreateTable(
                "dbo.VehicleCurrentStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        currentStatusName = c.String(),
                        isActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VehicleTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TypeName = c.String(),
                        isActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Vehicles", "BuyingStatus_Id", c => c.Int());
            AddColumn("dbo.Vehicles", "currentStatus_Id", c => c.Int());
            AddColumn("dbo.Vehicles", "vehicleType_Id", c => c.Int());
            AlterColumn("dbo.ConditionImages", "vehicleId", c => c.Int(nullable: false));
            CreateIndex("dbo.ConditionImages", "vehicleId");
            CreateIndex("dbo.Vehicles", "BuyingStatus_Id");
            CreateIndex("dbo.Vehicles", "currentStatus_Id");
            CreateIndex("dbo.Vehicles", "vehicleType_Id");
            AddForeignKey("dbo.Vehicles", "BuyingStatus_Id", "dbo.VehicleBuyingStatus", "Id");
            AddForeignKey("dbo.Vehicles", "currentStatus_Id", "dbo.VehicleCurrentStatus", "Id");
            AddForeignKey("dbo.Vehicles", "vehicleType_Id", "dbo.VehicleTypes", "Id");
            AddForeignKey("dbo.ConditionImages", "vehicleId", "dbo.Vehicles", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ConditionImages", "vehicleId", "dbo.Vehicles");
            DropForeignKey("dbo.Vehicles", "vehicleType_Id", "dbo.VehicleTypes");
            DropForeignKey("dbo.Vehicles", "currentStatus_Id", "dbo.VehicleCurrentStatus");
            DropForeignKey("dbo.Vehicles", "BuyingStatus_Id", "dbo.VehicleBuyingStatus");
            DropForeignKey("dbo.Revaluations", "assetId", "dbo.Assets");
            DropIndex("dbo.Vehicles", new[] { "vehicleType_Id" });
            DropIndex("dbo.Vehicles", new[] { "currentStatus_Id" });
            DropIndex("dbo.Vehicles", new[] { "BuyingStatus_Id" });
            DropIndex("dbo.ConditionImages", new[] { "vehicleId" });
            DropIndex("dbo.Revaluations", new[] { "assetId" });
            AlterColumn("dbo.ConditionImages", "vehicleId", c => c.Int());
            DropColumn("dbo.Vehicles", "vehicleType_Id");
            DropColumn("dbo.Vehicles", "currentStatus_Id");
            DropColumn("dbo.Vehicles", "BuyingStatus_Id");
            DropTable("dbo.VehicleTypes");
            DropTable("dbo.VehicleCurrentStatus");
            DropTable("dbo.Revaluations");
            RenameColumn(table: "dbo.ConditionImages", name: "vehicleId", newName: "Vehicle_Id");
            CreateIndex("dbo.ConditionImages", "Vehicle_Id");
            AddForeignKey("dbo.ConditionImages", "Vehicle_Id", "dbo.Vehicles", "Id");
            RenameTable(name: "dbo.VehicleBuyingStatus", newName: "BuyingStatus");
        }
    }
}
