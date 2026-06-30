namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changesinRentalOrdersAndRentalContracts : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.RentalOrders", name: "RentalContract_Id", newName: "rentalContractId");
            RenameColumn(table: "dbo.RentalInvoices", name: "RentalOrder_Id", newName: "rentalOrderId");
            RenameIndex(table: "dbo.RentalOrders", name: "IX_RentalContract_Id", newName: "IX_rentalContractId");
            RenameIndex(table: "dbo.RentalInvoices", name: "IX_RentalOrder_Id", newName: "IX_rentalOrderId");
            AddColumn("dbo.RentalContracts", "RentAmountMER", c => c.Double(nullable: false));
            AddColumn("dbo.RentalOrders", "RentMonth", c => c.DateTime());
            AddColumn("dbo.RentalOrders", "RentAmountMER", c => c.Double(nullable: false));
            AlterColumn("dbo.RentalContracts", "fromDate", c => c.DateTime());
            AlterColumn("dbo.RentalContracts", "toDate", c => c.DateTime());
            AlterColumn("dbo.RentalOrders", "fromDate", c => c.DateTime());
            AlterColumn("dbo.RentalOrders", "toDate", c => c.DateTime());
            AlterColumn("dbo.RentalInvoices", "fromDate", c => c.DateTime());
            AlterColumn("dbo.RentalInvoices", "toDate", c => c.DateTime());
            DropColumn("dbo.RentalOrders", "NumberOfMonths");
            DropColumn("dbo.RentalOrders", "TotalRentAmount");
            DropColumn("dbo.RentalOrders", "isActive");
        }
        
        public override void Down()
        {
            AddColumn("dbo.RentalOrders", "isActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.RentalOrders", "TotalRentAmount", c => c.Double(nullable: false));
            AddColumn("dbo.RentalOrders", "NumberOfMonths", c => c.Double(nullable: false));
            AlterColumn("dbo.RentalInvoices", "toDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.RentalInvoices", "fromDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.RentalOrders", "toDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.RentalOrders", "fromDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.RentalContracts", "toDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.RentalContracts", "fromDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.RentalOrders", "RentAmountMER");
            DropColumn("dbo.RentalOrders", "RentMonth");
            DropColumn("dbo.RentalContracts", "RentAmountMER");
            RenameIndex(table: "dbo.RentalInvoices", name: "IX_rentalOrderId", newName: "IX_RentalOrder_Id");
            RenameIndex(table: "dbo.RentalOrders", name: "IX_rentalContractId", newName: "IX_RentalContract_Id");
            RenameColumn(table: "dbo.RentalInvoices", name: "rentalOrderId", newName: "RentalOrder_Id");
            RenameColumn(table: "dbo.RentalOrders", name: "rentalContractId", newName: "RentalContract_Id");
        }
    }
}
