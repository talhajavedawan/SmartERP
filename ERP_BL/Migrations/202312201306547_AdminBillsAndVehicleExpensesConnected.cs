namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdminBillsAndVehicleExpensesConnected : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.VehicleExpenses", name: "AdminBill_Id", newName: "AdminBillForVehicleExpenses_Id");
            RenameIndex(table: "dbo.VehicleExpenses", name: "IX_AdminBill_Id", newName: "IX_AdminBillForVehicleExpenses_Id");
            AddColumn("dbo.VehicleExpenses", "AdminBillForFuelExpenses_Id", c => c.Int());
            CreateIndex("dbo.VehicleExpenses", "AdminBillForFuelExpenses_Id");
            AddForeignKey("dbo.VehicleExpenses", "AdminBillForFuelExpenses_Id", "dbo.tabAdminBill", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VehicleExpenses", "AdminBillForFuelExpenses_Id", "dbo.tabAdminBill");
            DropIndex("dbo.VehicleExpenses", new[] { "AdminBillForFuelExpenses_Id" });
            DropColumn("dbo.VehicleExpenses", "AdminBillForFuelExpenses_Id");
            RenameIndex(table: "dbo.VehicleExpenses", name: "IX_AdminBillForVehicleExpenses_Id", newName: "IX_AdminBill_Id");
            RenameColumn(table: "dbo.VehicleExpenses", name: "AdminBillForVehicleExpenses_Id", newName: "AdminBill_Id");
        }
    }
}
