namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class transactionHolderIdLogicAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bills", "transactionHolderId", c => c.Int());
            AddColumn("dbo.Inquiries", "transactionHolderId", c => c.Int());
            AddColumn("dbo.Offers", "transactionHolderId", c => c.Int());
            AddColumn("dbo.PurchaseOrders", "transactionHolderId", c => c.Int());
            AddColumn("dbo.Payments", "transactionHolderId", c => c.Int());
            AddColumn("dbo.PurchaseInvoices", "transactionHolderId", c => c.Int());
            AddColumn("dbo.SaleInvoices", "transactionHolderId", c => c.Int());
            AddColumn("dbo.SaleOrders", "transactionHolderId", c => c.Int());
            AddColumn("dbo.SalesReceipts", "transactionHolderId", c => c.Int());
            AddColumn("dbo.TravelingRecords", "residentFromDate", c => c.DateTime());
            AddColumn("dbo.TravelingRecords", "residentToDate", c => c.DateTime());
            AddColumn("dbo.TravelingRecords", "residentCountryId", c => c.Int());
            AddColumn("dbo.VisitingCountries", "sequence", c => c.Int(nullable: false));
            AddColumn("dbo.VisitingCountries", "transactionGroupId", c => c.Int(nullable: false));
            AddColumn("dbo.VisitingCountries", "SystemRefNo", c => c.String());
            AlterColumn("dbo.ResidentCountries", "fromDate", c => c.DateTime());
            AlterColumn("dbo.ResidentCountries", "toDate", c => c.DateTime());
            CreateIndex("dbo.Bills", "transactionHolderId");
            CreateIndex("dbo.Inquiries", "transactionHolderId");
            CreateIndex("dbo.Offers", "transactionHolderId");
            CreateIndex("dbo.PurchaseOrders", "transactionHolderId");
            CreateIndex("dbo.Payments", "transactionHolderId");
            CreateIndex("dbo.PurchaseInvoices", "transactionHolderId");
            CreateIndex("dbo.SaleInvoices", "transactionHolderId");
            CreateIndex("dbo.SaleOrders", "transactionHolderId");
            CreateIndex("dbo.SalesReceipts", "transactionHolderId");
            CreateIndex("dbo.TravelingRecords", "residentCountryId");
            AddForeignKey("dbo.Bills", "transactionHolderId", "dbo.Employees", "EmpId");
            AddForeignKey("dbo.Inquiries", "transactionHolderId", "dbo.Employees", "EmpId");
            AddForeignKey("dbo.Offers", "transactionHolderId", "dbo.Employees", "EmpId");
            AddForeignKey("dbo.Payments", "transactionHolderId", "dbo.Employees", "EmpId");
            AddForeignKey("dbo.PurchaseInvoices", "transactionHolderId", "dbo.Employees", "EmpId");
            AddForeignKey("dbo.PurchaseOrders", "transactionHolderId", "dbo.Employees", "EmpId");
            AddForeignKey("dbo.SaleInvoices", "transactionHolderId", "dbo.Employees", "EmpId");
            AddForeignKey("dbo.SaleOrders", "transactionHolderId", "dbo.Employees", "EmpId");
            AddForeignKey("dbo.SalesReceipts", "transactionHolderId", "dbo.Employees", "EmpId");
            AddForeignKey("dbo.TravelingRecords", "residentCountryId", "dbo.ResidentCountries", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TravelingRecords", "residentCountryId", "dbo.ResidentCountries");
            DropForeignKey("dbo.SalesReceipts", "transactionHolderId", "dbo.Employees");
            DropForeignKey("dbo.SaleOrders", "transactionHolderId", "dbo.Employees");
            DropForeignKey("dbo.SaleInvoices", "transactionHolderId", "dbo.Employees");
            DropForeignKey("dbo.PurchaseOrders", "transactionHolderId", "dbo.Employees");
            DropForeignKey("dbo.PurchaseInvoices", "transactionHolderId", "dbo.Employees");
            DropForeignKey("dbo.Payments", "transactionHolderId", "dbo.Employees");
            DropForeignKey("dbo.Offers", "transactionHolderId", "dbo.Employees");
            DropForeignKey("dbo.Inquiries", "transactionHolderId", "dbo.Employees");
            DropForeignKey("dbo.Bills", "transactionHolderId", "dbo.Employees");
            DropIndex("dbo.TravelingRecords", new[] { "residentCountryId" });
            DropIndex("dbo.SalesReceipts", new[] { "transactionHolderId" });
            DropIndex("dbo.SaleOrders", new[] { "transactionHolderId" });
            DropIndex("dbo.SaleInvoices", new[] { "transactionHolderId" });
            DropIndex("dbo.PurchaseInvoices", new[] { "transactionHolderId" });
            DropIndex("dbo.Payments", new[] { "transactionHolderId" });
            DropIndex("dbo.PurchaseOrders", new[] { "transactionHolderId" });
            DropIndex("dbo.Offers", new[] { "transactionHolderId" });
            DropIndex("dbo.Inquiries", new[] { "transactionHolderId" });
            DropIndex("dbo.Bills", new[] { "transactionHolderId" });
            AlterColumn("dbo.ResidentCountries", "toDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.ResidentCountries", "fromDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.VisitingCountries", "SystemRefNo");
            DropColumn("dbo.VisitingCountries", "transactionGroupId");
            DropColumn("dbo.VisitingCountries", "sequence");
            DropColumn("dbo.TravelingRecords", "residentCountryId");
            DropColumn("dbo.TravelingRecords", "residentToDate");
            DropColumn("dbo.TravelingRecords", "residentFromDate");
            DropColumn("dbo.SalesReceipts", "transactionHolderId");
            DropColumn("dbo.SaleOrders", "transactionHolderId");
            DropColumn("dbo.SaleInvoices", "transactionHolderId");
            DropColumn("dbo.PurchaseInvoices", "transactionHolderId");
            DropColumn("dbo.Payments", "transactionHolderId");
            DropColumn("dbo.PurchaseOrders", "transactionHolderId");
            DropColumn("dbo.Offers", "transactionHolderId");
            DropColumn("dbo.Inquiries", "transactionHolderId");
            DropColumn("dbo.Bills", "transactionHolderId");
        }
    }
}
