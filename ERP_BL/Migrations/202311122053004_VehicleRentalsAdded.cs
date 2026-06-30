namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VehicleRentalsAdded : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.PayeeCompanies", newName: "CompanyPayees");
            DropPrimaryKey("dbo.CompanyPayees");
            CreateTable(
                "dbo.RentedVehicleOwners",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OwnerName = c.String(),
                        isActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VehicleExpenses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreationDate = c.DateTime(),
                        FuelDate = c.DateTime(),
                        expenseType = c.Int(nullable: false),
                        CurrentMeterReading = c.Double(nullable: false),
                        LastMeterReading = c.Double(nullable: false),
                        MeterReadingDifference = c.Double(nullable: false),
                        Litres = c.Double(nullable: false),
                        ExpenseAmount = c.Double(nullable: false),
                        payeeId = c.Int(),
                        maintenanceHeadId = c.Int(),
                        AdminBill_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MaintenanceHeads", t => t.maintenanceHeadId)
                .ForeignKey("dbo.Payees", t => t.payeeId)
                .ForeignKey("dbo.tabAdminBill", t => t.AdminBill_Id)
                .Index(t => t.payeeId)
                .Index(t => t.maintenanceHeadId)
                .Index(t => t.AdminBill_Id);
            
            CreateTable(
                "dbo.MaintenanceHeads",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        HeadName = c.String(),
                        isActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Payees", "isVehicleType", c => c.Boolean(nullable: false));
            AddColumn("dbo.Payees", "isOwned", c => c.Boolean(nullable: false));
            AddColumn("dbo.Payees", "companyId", c => c.Int());
            AddColumn("dbo.Payees", "vehicleOwnerId", c => c.Int());
            AddColumn("dbo.tabAdminBill", "isVehicleType", c => c.Boolean(nullable: false));
            AddPrimaryKey("dbo.CompanyPayees", new[] { "Company_Id", "Payee_Id" });
            CreateIndex("dbo.Payees", "companyId");
            CreateIndex("dbo.Payees", "vehicleOwnerId");
            AddForeignKey("dbo.Payees", "companyId", "dbo.tabCompany", "Id");
            AddForeignKey("dbo.Payees", "vehicleOwnerId", "dbo.RentedVehicleOwners", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VehicleExpenses", "AdminBill_Id", "dbo.tabAdminBill");
            DropForeignKey("dbo.VehicleExpenses", "payeeId", "dbo.Payees");
            DropForeignKey("dbo.VehicleExpenses", "maintenanceHeadId", "dbo.MaintenanceHeads");
            DropForeignKey("dbo.Payees", "vehicleOwnerId", "dbo.RentedVehicleOwners");
            DropForeignKey("dbo.Payees", "companyId", "dbo.tabCompany");
            DropIndex("dbo.VehicleExpenses", new[] { "AdminBill_Id" });
            DropIndex("dbo.VehicleExpenses", new[] { "maintenanceHeadId" });
            DropIndex("dbo.VehicleExpenses", new[] { "payeeId" });
            DropIndex("dbo.Payees", new[] { "vehicleOwnerId" });
            DropIndex("dbo.Payees", new[] { "companyId" });
            DropPrimaryKey("dbo.CompanyPayees");
            DropColumn("dbo.tabAdminBill", "isVehicleType");
            DropColumn("dbo.Payees", "vehicleOwnerId");
            DropColumn("dbo.Payees", "companyId");
            DropColumn("dbo.Payees", "isOwned");
            DropColumn("dbo.Payees", "isVehicleType");
            DropTable("dbo.MaintenanceHeads");
            DropTable("dbo.VehicleExpenses");
            DropTable("dbo.RentedVehicleOwners");
            AddPrimaryKey("dbo.CompanyPayees", new[] { "Payee_Id", "Company_Id" });
            RenameTable(name: "dbo.CompanyPayees", newName: "PayeeCompanies");
        }
    }
}
