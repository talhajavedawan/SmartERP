namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixedAssetClassMappingAdjusted : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.CompanyEmployees", newName: "EmployeeCompanies");
            RenameTable(name: "dbo.DepartmentEmployees", newName: "EmployeeDepartments");
            RenameTable(name: "dbo.OfferVendors", newName: "VendorOffers");
            RenameTable(name: "dbo.SaleOrderVendors", newName: "VendorSaleOrders");
            DropForeignKey("dbo.Assets", "coOwnerID", "dbo.Employees");
            DropIndex("dbo.Assets", new[] { "coOwnerID" });
            RenameColumn(table: "dbo.Assets", name: "OwnerID", newName: "owner_EmpId");
            RenameIndex(table: "dbo.Assets", name: "IX_OwnerID", newName: "IX_owner_EmpId");
            DropPrimaryKey("dbo.EmployeeCompanies");
            DropPrimaryKey("dbo.EmployeeDepartments");
            DropPrimaryKey("dbo.VendorOffers");
            DropPrimaryKey("dbo.VendorSaleOrders");
            AddPrimaryKey("dbo.EmployeeCompanies", new[] { "Employee_EmpId", "Company_Id" });
            AddPrimaryKey("dbo.EmployeeDepartments", new[] { "Employee_EmpId", "Department_Id" });
            AddPrimaryKey("dbo.VendorOffers", new[] { "Vendor_Id", "Offer_Id" });
            AddPrimaryKey("dbo.VendorSaleOrders", new[] { "Vendor_Id", "SaleOrder_Id" });
            DropColumn("dbo.Assets", "coOwnerID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Assets", "coOwnerID", c => c.Int());
            DropPrimaryKey("dbo.VendorSaleOrders");
            DropPrimaryKey("dbo.VendorOffers");
            DropPrimaryKey("dbo.EmployeeDepartments");
            DropPrimaryKey("dbo.EmployeeCompanies");
            AddPrimaryKey("dbo.VendorSaleOrders", new[] { "SaleOrder_Id", "Vendor_Id" });
            AddPrimaryKey("dbo.VendorOffers", new[] { "Offer_Id", "Vendor_Id" });
            AddPrimaryKey("dbo.EmployeeDepartments", new[] { "Department_Id", "Employee_EmpId" });
            AddPrimaryKey("dbo.EmployeeCompanies", new[] { "Company_Id", "Employee_EmpId" });
            RenameIndex(table: "dbo.Assets", name: "IX_owner_EmpId", newName: "IX_OwnerID");
            RenameColumn(table: "dbo.Assets", name: "owner_EmpId", newName: "OwnerID");
            CreateIndex("dbo.Assets", "coOwnerID");
            AddForeignKey("dbo.Assets", "coOwnerID", "dbo.Employees", "EmpId");
            RenameTable(name: "dbo.VendorSaleOrders", newName: "SaleOrderVendors");
            RenameTable(name: "dbo.VendorOffers", newName: "OfferVendors");
            RenameTable(name: "dbo.EmployeeDepartments", newName: "DepartmentEmployees");
            RenameTable(name: "dbo.EmployeeCompanies", newName: "CompanyEmployees");
        }
    }
}
