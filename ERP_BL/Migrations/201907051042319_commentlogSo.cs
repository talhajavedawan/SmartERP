namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class commentlogSo : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.EmployeeCompanies", newName: "CompanyEmployees");
            RenameTable(name: "dbo.EmployeeDepartments", newName: "DepartmentEmployees");
            RenameTable(name: "dbo.VendorSaleOrders", newName: "SaleOrderVendors");
            RenameTable(name: "dbo.VendorOffers", newName: "OfferVendors");
            DropPrimaryKey("dbo.CompanyEmployees");
            DropPrimaryKey("dbo.DepartmentEmployees");
            DropPrimaryKey("dbo.SaleOrderVendors");
            DropPrimaryKey("dbo.OfferVendors");
            CreateTable(
                "dbo.CommentLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        Timestamp = c.DateTime(nullable: false),
                        Comment = c.String(),
                        isRead = c.Boolean(nullable: false),
                        ReadTimestamp = c.DateTime(),
                        TransactionType = c.Int(nullable: false),
                        TransactionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            AddPrimaryKey("dbo.CompanyEmployees", new[] { "Company_Id", "Employee_EmpId" });
            AddPrimaryKey("dbo.DepartmentEmployees", new[] { "Department_Id", "Employee_EmpId" });
            AddPrimaryKey("dbo.SaleOrderVendors", new[] { "SaleOrder_Id", "Vendor_Id" });
            AddPrimaryKey("dbo.OfferVendors", new[] { "Offer_Id", "Vendor_Id" });
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CommentLogs", "UserId", "dbo.Users");
            DropIndex("dbo.CommentLogs", new[] { "UserId" });
            DropPrimaryKey("dbo.OfferVendors");
            DropPrimaryKey("dbo.SaleOrderVendors");
            DropPrimaryKey("dbo.DepartmentEmployees");
            DropPrimaryKey("dbo.CompanyEmployees");
            DropTable("dbo.CommentLogs");
            AddPrimaryKey("dbo.OfferVendors", new[] { "Vendor_Id", "Offer_Id" });
            AddPrimaryKey("dbo.SaleOrderVendors", new[] { "Vendor_Id", "SaleOrder_Id" });
            AddPrimaryKey("dbo.DepartmentEmployees", new[] { "Employee_EmpId", "Department_Id" });
            AddPrimaryKey("dbo.CompanyEmployees", new[] { "Employee_EmpId", "Company_Id" });
            RenameTable(name: "dbo.OfferVendors", newName: "VendorOffers");
            RenameTable(name: "dbo.SaleOrderVendors", newName: "VendorSaleOrders");
            RenameTable(name: "dbo.DepartmentEmployees", newName: "EmployeeDepartments");
            RenameTable(name: "dbo.CompanyEmployees", newName: "EmployeeCompanies");
        }
    }
}
