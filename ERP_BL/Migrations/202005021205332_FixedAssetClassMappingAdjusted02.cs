namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixedAssetClassMappingAdjusted02 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.EmployeeCompanies", newName: "CompanyEmployees");
            RenameTable(name: "dbo.EmployeeDepartments", newName: "DepartmentEmployees");
            RenameTable(name: "dbo.VendorSaleOrders", newName: "SaleOrderVendors");
            RenameTable(name: "dbo.VendorOffers", newName: "OfferVendors");
            DropForeignKey("dbo.Assets", "owner_EmpId", "dbo.Employees");
            DropPrimaryKey("dbo.CompanyEmployees");
            DropPrimaryKey("dbo.DepartmentEmployees");
            DropPrimaryKey("dbo.SaleOrderVendors");
            DropPrimaryKey("dbo.OfferVendors");
            AddColumn("dbo.Assets", "coOwnerID", c => c.Int());
            AddColumn("dbo.Assets", "Employee_EmpId", c => c.Int());
            AddPrimaryKey("dbo.CompanyEmployees", new[] { "Company_Id", "Employee_EmpId" });
            AddPrimaryKey("dbo.DepartmentEmployees", new[] { "Department_Id", "Employee_EmpId" });
            AddPrimaryKey("dbo.SaleOrderVendors", new[] { "SaleOrder_Id", "Vendor_Id" });
            AddPrimaryKey("dbo.OfferVendors", new[] { "Offer_Id", "Vendor_Id" });
            CreateIndex("dbo.Assets", "coOwnerID");
            CreateIndex("dbo.Assets", "Employee_EmpId");
            AddForeignKey("dbo.Assets", "coOwnerID", "dbo.Employees", "EmpId");
            AddForeignKey("dbo.Assets", "Employee_EmpId", "dbo.Employees", "EmpId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Assets", "Employee_EmpId", "dbo.Employees");
            DropForeignKey("dbo.Assets", "coOwnerID", "dbo.Employees");
            DropIndex("dbo.Assets", new[] { "Employee_EmpId" });
            DropIndex("dbo.Assets", new[] { "coOwnerID" });
            DropPrimaryKey("dbo.OfferVendors");
            DropPrimaryKey("dbo.SaleOrderVendors");
            DropPrimaryKey("dbo.DepartmentEmployees");
            DropPrimaryKey("dbo.CompanyEmployees");
            DropColumn("dbo.Assets", "Employee_EmpId");
            DropColumn("dbo.Assets", "coOwnerID");
            AddPrimaryKey("dbo.OfferVendors", new[] { "Vendor_Id", "Offer_Id" });
            AddPrimaryKey("dbo.SaleOrderVendors", new[] { "Vendor_Id", "SaleOrder_Id" });
            AddPrimaryKey("dbo.DepartmentEmployees", new[] { "Employee_EmpId", "Department_Id" });
            AddPrimaryKey("dbo.CompanyEmployees", new[] { "Employee_EmpId", "Company_Id" });
            AddForeignKey("dbo.Assets", "owner_EmpId", "dbo.Employees", "EmpId");
            RenameTable(name: "dbo.OfferVendors", newName: "VendorOffers");
            RenameTable(name: "dbo.SaleOrderVendors", newName: "VendorSaleOrders");
            RenameTable(name: "dbo.DepartmentEmployees", newName: "EmployeeDepartments");
            RenameTable(name: "dbo.CompanyEmployees", newName: "EmployeeCompanies");
        }
    }
}
