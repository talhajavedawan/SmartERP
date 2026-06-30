namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CustomertoCompanyMapping : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.VendorDepartments", newName: "DepartmentVendors");
            RenameTable(name: "dbo.VendorPurchaseOrders", newName: "PurchaseOrderVendors");
            RenameTable(name: "dbo.VendorSaleOrders", newName: "SaleOrderVendors");
            RenameTable(name: "dbo.VendorOffers", newName: "OfferVendors");
            DropPrimaryKey("dbo.DepartmentVendors");
            DropPrimaryKey("dbo.PurchaseOrderVendors");
            DropPrimaryKey("dbo.SaleOrderVendors");
            DropPrimaryKey("dbo.OfferVendors");
            CreateTable(
                "dbo.CompanyCustomerCompanies",
                c => new
                    {
                        Company_Id = c.Int(nullable: false),
                        CustomerCompany_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Company_Id, t.CustomerCompany_Id })
                .ForeignKey("dbo.tabCompany", t => t.Company_Id, cascadeDelete: true)
                .ForeignKey("dbo.CustomerCompanies", t => t.CustomerCompany_Id, cascadeDelete: true)
                .Index(t => t.Company_Id)
                .Index(t => t.CustomerCompany_Id);
            
            AddColumn("dbo.tabVendor", "IsSubsidary", c => c.Boolean(nullable: false));
            AddColumn("dbo.tabVendor", "ParentID", c => c.Int());
            AddColumn("dbo.SaleInvoices", "PaymentYear", c => c.Int());
            AddColumn("dbo.SaleInvoices", "PaymentQuarter", c => c.Int());
            AddPrimaryKey("dbo.DepartmentVendors", new[] { "Department_Id", "Vendor_Id" });
            AddPrimaryKey("dbo.PurchaseOrderVendors", new[] { "PurchaseOrder_Id", "Vendor_Id" });
            AddPrimaryKey("dbo.SaleOrderVendors", new[] { "SaleOrder_Id", "Vendor_Id" });
            AddPrimaryKey("dbo.OfferVendors", new[] { "Offer_Id", "Vendor_Id" });
            CreateIndex("dbo.tabVendor", "ParentID");
            AddForeignKey("dbo.tabVendor", "ParentID", "dbo.tabVendor", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CompanyCustomerCompanies", "CustomerCompany_Id", "dbo.CustomerCompanies");
            DropForeignKey("dbo.CompanyCustomerCompanies", "Company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.tabVendor", "ParentID", "dbo.tabVendor");
            DropIndex("dbo.CompanyCustomerCompanies", new[] { "CustomerCompany_Id" });
            DropIndex("dbo.CompanyCustomerCompanies", new[] { "Company_Id" });
            DropIndex("dbo.tabVendor", new[] { "ParentID" });
            DropPrimaryKey("dbo.OfferVendors");
            DropPrimaryKey("dbo.SaleOrderVendors");
            DropPrimaryKey("dbo.PurchaseOrderVendors");
            DropPrimaryKey("dbo.DepartmentVendors");
            DropColumn("dbo.SaleInvoices", "PaymentQuarter");
            DropColumn("dbo.SaleInvoices", "PaymentYear");
            DropColumn("dbo.tabVendor", "ParentID");
            DropColumn("dbo.tabVendor", "IsSubsidary");
            DropTable("dbo.CompanyCustomerCompanies");
            AddPrimaryKey("dbo.OfferVendors", new[] { "Vendor_Id", "Offer_Id" });
            AddPrimaryKey("dbo.SaleOrderVendors", new[] { "Vendor_Id", "SaleOrder_Id" });
            AddPrimaryKey("dbo.PurchaseOrderVendors", new[] { "Vendor_Id", "PurchaseOrder_Id" });
            AddPrimaryKey("dbo.DepartmentVendors", new[] { "Vendor_Id", "Department_Id" });
            RenameTable(name: "dbo.OfferVendors", newName: "VendorOffers");
            RenameTable(name: "dbo.SaleOrderVendors", newName: "VendorSaleOrders");
            RenameTable(name: "dbo.PurchaseOrderVendors", newName: "VendorPurchaseOrders");
            RenameTable(name: "dbo.DepartmentVendors", newName: "VendorDepartments");
        }
    }
}
