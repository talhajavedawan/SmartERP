namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class contactPerson_SalesReceiptsAdded : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.CompanyEmployees", newName: "EmployeeCompanies");
            RenameTable(name: "dbo.DepartmentCustomerCompanies", newName: "CustomerCompanyDepartments");
            RenameTable(name: "dbo.OfferVendors", newName: "VendorOffers");
            RenameTable(name: "dbo.SaleOrderVendors", newName: "VendorSaleOrders");
            DropPrimaryKey("dbo.EmployeeCompanies");
            DropPrimaryKey("dbo.CustomerCompanyDepartments");
            DropPrimaryKey("dbo.VendorOffers");
            DropPrimaryKey("dbo.VendorSaleOrders");
            CreateTable(
                "dbo.Accounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        accountType = c.Int(nullable: false),
                        AccountNo = c.String(),
                        AccountNick = c.String(),
                        nature = c.Int(nullable: false),
                        COANo = c.String(),
                        company_Id = c.Int(),
                        currency_Id = c.Int(),
                        department_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tabCompany", t => t.company_Id)
                .ForeignKey("dbo.Currencies", t => t.currency_Id)
                .ForeignKey("dbo.tabDepartment", t => t.department_Id)
                .Index(t => t.company_Id)
                .Index(t => t.currency_Id)
                .Index(t => t.department_Id);
            
            CreateTable(
                "dbo.Banks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BankName = c.String(),
                        BranchCode = c.String(),
                        Location = c.String(),
                        address_Id = c.Int(),
                        contact_Id = c.Int(),
                        contactPerson_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tabAddress", t => t.address_Id)
                .ForeignKey("dbo.tabContact", t => t.contact_Id)
                .ForeignKey("dbo.ContactPersons", t => t.contactPerson_Id)
                .Index(t => t.address_Id)
                .Index(t => t.contact_Id)
                .Index(t => t.contactPerson_Id);
            
            CreateTable(
                "dbo.ContactPersons",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        contact_Id = c.Int(),
                        designation_DesigId = c.Int(),
                        person_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tabContact", t => t.contact_Id)
                .ForeignKey("dbo.Designations", t => t.designation_DesigId)
                .ForeignKey("dbo.tabPerson", t => t.person_Id)
                .Index(t => t.contact_Id)
                .Index(t => t.designation_DesigId)
                .Index(t => t.person_Id);
            
            CreateTable(
                "dbo.CollectionMethods",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MethodName = c.String(),
                        isActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SalesReceipts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        receiptType = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        SystemRefNo = c.String(),
                        ReceiptRefNo = c.String(),
                        CollectionAmount = c.Double(nullable: false),
                        BankId = c.Int(nullable: false),
                        AccountId = c.Int(nullable: false),
                        AppliesToSales = c.Boolean(nullable: false),
                        collectionMethod_Id = c.Int(),
                        company_Id = c.Int(),
                        Currency_Id = c.Int(),
                        Customer_Id = c.Int(),
                        department_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Accounts", t => t.AccountId, cascadeDelete: true)
                .ForeignKey("dbo.Banks", t => t.BankId, cascadeDelete: true)
                .ForeignKey("dbo.CollectionMethods", t => t.collectionMethod_Id)
                .ForeignKey("dbo.tabCompany", t => t.company_Id)
                .ForeignKey("dbo.Currencies", t => t.Currency_Id)
                .ForeignKey("dbo.CustomerCompanies", t => t.Customer_Id)
                .ForeignKey("dbo.tabDepartment", t => t.department_Id)
                .Index(t => t.BankId)
                .Index(t => t.AccountId)
                .Index(t => t.collectionMethod_Id)
                .Index(t => t.company_Id)
                .Index(t => t.Currency_Id)
                .Index(t => t.Customer_Id)
                .Index(t => t.department_Id);
            
            AddPrimaryKey("dbo.EmployeeCompanies", new[] { "Employee_EmpId", "Company_Id" });
            AddPrimaryKey("dbo.CustomerCompanyDepartments", new[] { "CustomerCompany_Id", "Department_Id" });
            AddPrimaryKey("dbo.VendorOffers", new[] { "Vendor_Id", "Offer_Id" });
            AddPrimaryKey("dbo.VendorSaleOrders", new[] { "Vendor_Id", "SaleOrder_Id" });
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SalesReceipts", "department_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.SalesReceipts", "Customer_Id", "dbo.CustomerCompanies");
            DropForeignKey("dbo.SalesReceipts", "Currency_Id", "dbo.Currencies");
            DropForeignKey("dbo.SalesReceipts", "company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.SalesReceipts", "collectionMethod_Id", "dbo.CollectionMethods");
            DropForeignKey("dbo.SalesReceipts", "BankId", "dbo.Banks");
            DropForeignKey("dbo.SalesReceipts", "AccountId", "dbo.Accounts");
            DropForeignKey("dbo.Banks", "contactPerson_Id", "dbo.ContactPersons");
            DropForeignKey("dbo.ContactPersons", "person_Id", "dbo.tabPerson");
            DropForeignKey("dbo.ContactPersons", "designation_DesigId", "dbo.Designations");
            DropForeignKey("dbo.ContactPersons", "contact_Id", "dbo.tabContact");
            DropForeignKey("dbo.Banks", "contact_Id", "dbo.tabContact");
            DropForeignKey("dbo.Banks", "address_Id", "dbo.tabAddress");
            DropForeignKey("dbo.Accounts", "department_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.Accounts", "currency_Id", "dbo.Currencies");
            DropForeignKey("dbo.Accounts", "company_Id", "dbo.tabCompany");
            DropIndex("dbo.SalesReceipts", new[] { "department_Id" });
            DropIndex("dbo.SalesReceipts", new[] { "Customer_Id" });
            DropIndex("dbo.SalesReceipts", new[] { "Currency_Id" });
            DropIndex("dbo.SalesReceipts", new[] { "company_Id" });
            DropIndex("dbo.SalesReceipts", new[] { "collectionMethod_Id" });
            DropIndex("dbo.SalesReceipts", new[] { "AccountId" });
            DropIndex("dbo.SalesReceipts", new[] { "BankId" });
            DropIndex("dbo.ContactPersons", new[] { "person_Id" });
            DropIndex("dbo.ContactPersons", new[] { "designation_DesigId" });
            DropIndex("dbo.ContactPersons", new[] { "contact_Id" });
            DropIndex("dbo.Banks", new[] { "contactPerson_Id" });
            DropIndex("dbo.Banks", new[] { "contact_Id" });
            DropIndex("dbo.Banks", new[] { "address_Id" });
            DropIndex("dbo.Accounts", new[] { "department_Id" });
            DropIndex("dbo.Accounts", new[] { "currency_Id" });
            DropIndex("dbo.Accounts", new[] { "company_Id" });
            DropPrimaryKey("dbo.VendorSaleOrders");
            DropPrimaryKey("dbo.VendorOffers");
            DropPrimaryKey("dbo.CustomerCompanyDepartments");
            DropPrimaryKey("dbo.EmployeeCompanies");
            DropTable("dbo.SalesReceipts");
            DropTable("dbo.CollectionMethods");
            DropTable("dbo.ContactPersons");
            DropTable("dbo.Banks");
            DropTable("dbo.Accounts");
            AddPrimaryKey("dbo.VendorSaleOrders", new[] { "SaleOrder_Id", "Vendor_Id" });
            AddPrimaryKey("dbo.VendorOffers", new[] { "Offer_Id", "Vendor_Id" });
            AddPrimaryKey("dbo.CustomerCompanyDepartments", new[] { "Department_Id", "CustomerCompany_Id" });
            AddPrimaryKey("dbo.EmployeeCompanies", new[] { "Company_Id", "Employee_EmpId" });
            RenameTable(name: "dbo.VendorSaleOrders", newName: "SaleOrderVendors");
            RenameTable(name: "dbo.VendorOffers", newName: "OfferVendors");
            RenameTable(name: "dbo.CustomerCompanyDepartments", newName: "DepartmentCustomerCompanies");
            RenameTable(name: "dbo.EmployeeCompanies", newName: "CompanyEmployees");
        }
    }
}
