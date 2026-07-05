using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using ERP_BL.Databases;
using ERP_BL.Fields;
using ERP_BL.Reports;

namespace ERP_BL.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<ERP_BL.Databases.DBContextERP>
    {
        public Configuration()
        {
            //AutomaticMigrationsEnabled = false;
            this.AutomaticMigrationDataLossAllowed = true;
            this.AutomaticMigrationsEnabled = false;
        }
        protected override void Seed(ERP_BL.Databases.DBContextERP context)
        {
            IList<IndustryType> industryTypes = new List<IndustryType>();
            industryTypes.Add(new IndustryType() { Id = 1, name = "Information Technology", isActive = true, isApproved = true, addedDate = System.DateTime.Now, user_Id = null });
            industryTypes.Add(new IndustryType() { Id = 2, name = "Service Provider", isActive = true, isApproved = true, addedDate = System.DateTime.Now, user_Id = null });
            industryTypes.Add(new IndustryType() { Id = 3, name = "Contractor", isActive = true, isApproved = true, addedDate = System.DateTime.Now, user_Id = null });
            industryTypes.Add(new IndustryType() { Id = 4, name = "Procurement", isActive = true, isApproved = true, addedDate = System.DateTime.Now, user_Id = null });
            industryTypes.Add(new IndustryType() { Id = 5, name = "Engineering & Services", isActive = true, isApproved = true, addedDate = System.DateTime.Now, user_Id = null });
            IList<Currency> currencies = new List<Currency>();
            currencies.Add(new Currency() { Id = 1, CurrencyName = "Dollar", Symbol = "$", Abbrivation = "USD", Country = "USA" });
            currencies.Add(new Currency() { Id = 2, CurrencyName = "Euro", Symbol = "€", Abbrivation = "EUR", Country = "Europe" });
            currencies.Add(new Currency() { Id = 3, CurrencyName = "Pound", Symbol = "£", Abbrivation = "GBP", Country = "United Kingdom" });
            currencies.Add(new Currency() { Id = 4, CurrencyName = "Pakistani Rupee", Symbol = "Rs", Abbrivation = "PKR", Country = "Pakistan" });
            IList<Permission> permissions = new List<Permission>();
            //Comapny Related
            permissions.Add(new Permission() { Id = 1, Added = System.DateTime.Now, LastModified = System.DateTime.Now, Name = "Company Center", Description = "Allow User To Open Company Center" });
            permissions.Add(new Permission() { Id = 2, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 1, Name = "Add New Comapny", Description = "Allow User To Open New Company" });
            permissions.Add(new Permission() { Id = 3, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 1, Name = "Edit Company", Description = "Allow User To Edit Company Information" });
            permissions.Add(new Permission() { Id = 4, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 1, Name = "Add New Department", Description = "Allow User To Open New Department" });
            permissions.Add(new Permission() { Id = 5, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 1, Name = "Edit Department", Description = "Allow User To Edit Department Information" });
            permissions.Add(new Permission() { Id = 6, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 1, Name = "View InActive Companies", Description = "Allow User To View InActive Companies" });
            permissions.Add(new Permission() { Id = 7, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 1, Name = "View InActive Departments", Description = "Allow User To View InActive Departments" });
            permissions.Add(new Permission() { Id = 8, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 1, Name = "Mark Company as  InActive", Description = "Allow User To Mark Company as InActive." });
            permissions.Add(new Permission() { Id = 9, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 1, Name = "Mark Department as InActive", Description = "Allow User To Mark Department as InActive." });


            //Employee 
            permissions.Add(new Permission() { Id = 10, Added = System.DateTime.Now, LastModified = System.DateTime.Now, Name = "Employee Center", Description = "Allow User To Open Employee Center" });
            permissions.Add(new Permission() { Id = 11, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 10, Name = "Add New Employee", Description = "Allow User To Add New Employee " });
            permissions.Add(new Permission() { Id = 12, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 10, Name = "Edit Employee", Description = "Allow User To Edit Employee Information" });
            permissions.Add(new Permission() { Id = 13, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 10, Name = "View InActive Employees", Description = "Allow User To View InActive Employees" });
            permissions.Add(new Permission() { Id = 14, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 10, Name = "Mark Employee as InActive", Description = "Allow User To Mark Employee as InActive." });

            //Customer
            permissions.Add(new Permission() { Id = 20, Added = System.DateTime.Now, LastModified = System.DateTime.Now, Name = "Customer Center", Description = "Allow User To Open Customer Center" });
            permissions.Add(new Permission() { Id = 21, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 20, Name = "Add New Customer", Description = "Allow User To Add New Customer" });
            permissions.Add(new Permission() { Id = 22, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 20, Name = "Edit Customer", Description = "Allow User To Edit Customer" });
            permissions.Add(new Permission() { Id = 23, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 20, Name = "See Transactions", Description = "Allow User To View Transaction done From his Account" });
            permissions.Add(new Permission() { Id = 24, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 20, Name = "View InActive Customers", Description = "Allow User To View InActive Customers" });
            permissions.Add(new Permission() { Id = 25, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 20, Name = "Mark Customer as InActive", Description = "Allow User To Mark Customer as InActive." });


            //Vendor
            permissions.Add(new Permission() { Id = 30, Added = System.DateTime.Now, LastModified = System.DateTime.Now, Name = "Vendor Center", Description = "Allow User To Open Vendor Center" });
            permissions.Add(new Permission() { Id = 31, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 30, Name = "Add New Vendor", Description = "Allow User To Add Vendor" });
            permissions.Add(new Permission() { Id = 32, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 30, Name = "Edit Vendor", Description = "Allow User To Edit Vendor Information" });
            permissions.Add(new Permission() { Id = 33, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 30, Name = "View InActive Vendor", Description = "Allow User To View InActive Vendors" });
            permissions.Add(new Permission() { Id = 34, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 30, Name = "Mark Vendor as InActive", Description = "Allow User To Mark Vendor as InActive." });
            permissions.Add(new Permission() { Id = 35, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 30, Name = "View InActive Vendor Payment Statuses", Description = "Allow User To View InActive Vendors" });
            permissions.Add(new Permission() { Id = 36, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 30, Name = "Add Vendor Payment Status", Description = "Allow User To Add New Vendor Payment Status" });
            permissions.Add(new Permission() { Id = 37, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 30, Name = "Edit Vendor Payment Status", Description = "Allow User To edit Vendor Payment Status" });


            //Principal
            permissions.Add(new Permission() { Id = 40, Added = System.DateTime.Now, LastModified = System.DateTime.Now, Name = "Principal Center", Description = "Allow User To Open Principal Center" });
            permissions.Add(new Permission() { Id = 41, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 40, Name = "Add New Principal", Description = "Allow User To Add New Principal" });
            permissions.Add(new Permission() { Id = 42, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 40, Name = "Edit Principal", Description = "Allow User To Edit Principal" });
            permissions.Add(new Permission() { Id = 43, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 40, Name = "View InActive Principal", Description = "Allow User To View InActive Principals" });
            permissions.Add(new Permission() { Id = 44, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 40, Name = "Mark Prinicpal as InActive", Description = "Allow User To Mark Principal as InActive." });

            //procurment
            permissions.Add(new Permission() { Id = 50, Added = System.DateTime.Now, LastModified = System.DateTime.Now, Name = "Procurment Panel", Description = "Allow User To Open Procurmant Panel" });
            permissions.Add(new Permission() { Id = 51, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 50, Name = "Inquiries", Description = "Inquiries" });

            permissions.Add(new Permission() { Id = 52, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 51, Name = "Add Inquiry", Description = "Allow User To Add New Inquiry" });
            permissions.Add(new Permission() { Id = 53, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 51, Name = "Edit Inquiry", Description = "Allow User To Edit Inquiry" });
            permissions.Add(new Permission() { Id = 54, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 51, Name = "View Inquiry Details", Description = "Allow User To View Inquiry Details" });
            permissions.Add(new Permission() { Id = 55, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 51, Name = "List Of Inquiries", Description = "Allow User To View List Of Inquiries" });
            permissions.Add(new Permission() { Id = 56, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 51, Name = "Add Inquiry Status", Description = "Allow User to Add New Status for Inquiry" });
            permissions.Add(new Permission() { Id = 57, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 51, Name = "Create Offer from Inquiry", Description = "Allow User To Create Offer from an Existing Inquiry" });
            permissions.Add(new Permission() { Id = 58, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 51, Name = "Edit Inquiry Status", Description = "Allow User To Edit Inquiry Status" });
            permissions.Add(new Permission() { Id = 59, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 51, Name = "View InActive Inquiry Statuses", Description = "Allow User To View InActive Inquiry Statuses" });
            permissions.Add(new Permission() { Id = 60, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 51, Name = "Add Inquiry without Approval", Description = "Allow User To Add New Inquiry without Approval" });
            permissions.Add(new Permission() { Id = 61, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 51, Name = "View InActive Inquiries", Description = "Allow User To View InActive Inquiries" });
            permissions.Add(new Permission() { Id = 62, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 51, Name = "Close Inquiry without Approval", Description = "Allow User To Close Inquiry without Approval" });

            permissions.Add(new Permission() { Id = 301, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 50, Name = "Offers", Description = "Offers" });

            permissions.Add(new Permission() { Id = 302, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 301, Name = "Add New Offer ", Description = "Allow User To Add New Offer" });
            permissions.Add(new Permission() { Id = 303, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 301, Name = "Edit Offer", Description = "Allow User To Edit Offer Details" });
            permissions.Add(new Permission() { Id = 304, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 301, Name = "Offer Details", Description = "Allow User To View Offer Details" });
            permissions.Add(new Permission() { Id = 305, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 301, Name = "List Of Offers", Description = "Allow User To view List of Offers" });
            permissions.Add(new Permission() { Id = 306, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 301, Name = "Add Offer Status", Description = "Allow User To Add new Status for Offer" });
            permissions.Add(new Permission() { Id = 307, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 301, Name = "View InActive Offers", Description = "Allow User To View InActive Offers" });
            permissions.Add(new Permission() { Id = 308, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 301, Name = "Add Offer without Approval", Description = "Allow User To Add New Offer without Approval" });
            permissions.Add(new Permission() { Id = 309, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 301, Name = "Edit Offer Status", Description = "Allow User To Edit Offer Status" });
            permissions.Add(new Permission() { Id = 310, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 301, Name = "View InActive Offer Statuses", Description = "Allow User To View InActive Offer Statuses" });
            permissions.Add(new Permission() { Id = 311, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 301, Name = "Create Sale Order from Offer", Description = "Allow User To Create Purchase Order from an existing Offer" });
            permissions.Add(new Permission() { Id = 312, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 301, Name = "Close Offer without Approval", Description = "Allow User To Close Offer without Approval" });

            permissions.Add(new Permission() { Id = 331, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 50, Name = "Sale Orders", Description = "Sale Orders" });
            permissions.Add(new Permission() { Id = 332, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "Add Sale Order", Description = "Allow User To Add Sale Order" });
            permissions.Add(new Permission() { Id = 333, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "Edit Sale Order", Description = "Allow User To Edit Sale Order" });
            permissions.Add(new Permission() { Id = 334, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "View Sale Order", Description = "Allow User To View Sale Order" });
            permissions.Add(new Permission() { Id = 335, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "List of Sale Orders", Description = "Allow User To View List of Sale Order" });
            permissions.Add(new Permission() { Id = 336, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "Add Sale Order Status", Description = "Allow User To Add new status for Sale Order" });
            permissions.Add(new Permission() { Id = 337, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "Add Sale Order without Approval", Description = "Allow User To Add New Sale Order without Approval" });
            permissions.Add(new Permission() { Id = 338, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "View InActive Sale Orders", Description = "Allow User To View InActive Sale Orders" });
            permissions.Add(new Permission() { Id = 339, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "Close Sale Order", Description = "Allow User To Close Sale Order" });
            permissions.Add(new Permission() { Id = 340, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "Edit Sale Order Status", Description = "Allow User To Edit Sale Order Status" });
            permissions.Add(new Permission() { Id = 341, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "View InActive Sale Order Statuses", Description = "Allow User To View InActive Sale Order Statuses" });
            permissions.Add(new Permission() { Id = 342, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "Create Po from Sale Order", Description = "Allow User To Create Purchase Order from an existing SaleOrder" });
            permissions.Add(new Permission() { Id = 343, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "Close Sale Order without Approval", Description = "Allow User To Close Sale Order without Approval" });
            permissions.Add(new Permission() { Id = 344, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "Edit Unapproved Sale Order", Description = "Allow User To Edit Unapproved Sale Order without Approval" });
            permissions.Add(new Permission() { Id = 344, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "View Market Exchange Rate in Sale Order", Description = "Allow User To View Market Exchange Rate in Sale Order" });


            permissions.Add(new Permission() { Id = 361, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 50, Name = "Purchase Orders", Description = "Purchase Orders" });

            permissions.Add(new Permission() { Id = 362, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 361, Name = "Add Purchase Order", Description = "Allow User To Add Purchase Order" });
            permissions.Add(new Permission() { Id = 363, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 361, Name = "Edit Purchase Order", Description = "Allow User To Edit Purchase Order" });
            permissions.Add(new Permission() { Id = 364, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 361, Name = "View Purchase Order", Description = "Allow User To View Purchase Order" });
            permissions.Add(new Permission() { Id = 365, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 361, Name = "List of Purchase Orders", Description = "Allow User To View List of Purchase Order" });
            permissions.Add(new Permission() { Id = 366, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 361, Name = "Add Purchase Order Status", Description = "Allow User To Add new status for Purchase Order" });
            permissions.Add(new Permission() { Id = 367, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 361, Name = "Close Inquiry", Description = "Allow User To Close Inquiry" });
            permissions.Add(new Permission() { Id = 368, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 361, Name = "Close Offer", Description = "Allow User To Close Offer" });
            permissions.Add(new Permission() { Id = 369, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 361, Name = "Close Purchase Order", Description = "Allow User To Close Purchase Order" });
            permissions.Add(new Permission() { Id = 370, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 361, Name = "View InActive Purchase Orders", Description = "Allow User To View InActive Purchase Orders" });
            permissions.Add(new Permission() { Id = 370, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 361, Name = "Add Purchase Order without Approval", Description = "Allow User To Add New Purchase Order without Approval" });
            permissions.Add(new Permission() { Id = 371, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 361, Name = "Edit Purchase Order Status", Description = "Allow User To Edit Purchase Order Status" });
            permissions.Add(new Permission() { Id = 372, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 361, Name = "View InActive Purchase Order Statuses", Description = "Allow User To View InActive Purchase Order Statuses" });
            permissions.Add(new Permission() { Id = 373, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 361, Name = "Close Purchase Order without Approval", Description = "Allow User To Close Purchase Order without Approval" });




            //    ////Reports
            //    //permissions.Add(new Permission() { Id = 80, Added = System.DateTime.Now, LastModified = System.DateTime.Now, Name = "Reports", Description = "Allow User To View Reports Menu" });
            //    //permissions.Add(new Permission() { Id = 81, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 80, Name = "View Memorized Reports", Description = "Allow User To View Memorized Reports" });
            //    //permissions.Add(new Permission() { Id = 82, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 80, Name = "Report Center", Description = "Allow User To create new reports in Reports Center " });
            //    //permissions.Add(new Permission() { Id = 83, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 80, Name = "User Procurment Reports", Description = "Allow User To Edit template for User Procurment reports" });
            //    //Lists
            //    permissions.Add(new Permission() { Id = 90, Added = System.DateTime.Now, LastModified = System.DateTime.Now, Name = "Lists", Description = "Allow User To View Lists" });
            //    permissions.Add(new Permission() { Id = 91, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 90, Name = "List of Currencies", Description = "Allow User To View List of Currencies" });
            //    permissions.Add(new Permission() { Id = 92, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 90, Name = "List of Items", Description = "Allow User To View List of Items" });
            //    permissions.Add(new Permission() { Id = 93, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 90, Name = "List of Industry Types", Description = "Allow User To View List of Industry Types" });
            //    permissions.Add(new Permission() { Id = 94, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 90, Name = "List of Payment Terms", Description = "Allow User To View List of Payment Terms" });
            //    permissions.Add(new Permission() { Id = 95, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 90, Name = "List of Incoterms", Description = "Allow User To View List of Incoterms" });
            //    permissions.Add(new Permission() { Id = 96, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 90, Name = "List of Procurment Statuses ", Description = "Allow User To View List of Procurment Statuses" });
            //    permissions.Add(new Permission() { Id = 97, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 90, Name = "List of Inquiry Statuses", Description = "Allow User To View List of Inquiry Statuses" });
            //    permissions.Add(new Permission() { Id = 98, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 90, Name = "List of Offer Statuses", Description = "Allow User To View List of Offer Statuses" });
            //    permissions.Add(new Permission() { Id = 99, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 90, Name = "List of Purchase Order Statuses", Description = "Allow User To View List of Purchase Order Statuses" });

            //users
            permissions.Add(new Permission() { Id = 110, Added = System.DateTime.Now, LastModified = System.DateTime.Now, Name = "Users", Description = "Allow User To view Users List" });

            permissions.Add(new Permission() { Id = 111, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 110, Name = "Manage Users And Roles", Description = "Allow User To Manage Users And Roles" });
            permissions.Add(new Permission() { Id = 112, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 110, Name = "Add New User", Description = "Allow User To Add New User" });
            permissions.Add(new Permission() { Id = 113, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 110, Name = "Add New Role", Description = "Allow User To Add New Role" });
            permissions.Add(new Permission() { Id = 114, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 110, Name = "Edit Role", Description = "Allow User To Edit Role" });
            permissions.Add(new Permission() { Id = 115, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 110, Name = "Edit User", Description = "Allow User To Edit User" });
            //permissions.Add(new Permission() { Id = 116, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 110, Name = "Mark Role as InActive", Description = "Allow User To Mark Role as InActive." });

            //Currencies
            permissions.Add(new Permission() { Id = 400, Added = System.DateTime.Now, LastModified = System.DateTime.Now, Name = "Currencies", Description = "Allow User To Access Currencies" });

            permissions.Add(new Permission() { Id = 401, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 400, Name = "Add Currency", Description = "Allow User To add Currency" });
            permissions.Add(new Permission() { Id = 402, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 400, Name = "Edit Currency", Description = "Allow User To add Currency" });
            permissions.Add(new Permission() { Id = 403, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 400, Name = "Add Exchange Rates", Description = "Allow User To add Exchange Rates" });
            permissions.Add(new Permission() { Id = 404, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 400, Name = "Edit Exchange Rates", Description = "Allow User To Edit Exchange Rates" });
            permissions.Add(new Permission() { Id = 405, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 400, Name = "Add Sales Exchange Rates", Description = "Allow User To Add Sales Exchange Rates" });
            permissions.Add(new Permission() { Id = 406, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 400, Name = "Edit Sales Exchange Rates", Description = "Allow User To Edit Sales Exchange Rates" });



            //Comapny Related
            permissions.Add(new Permission() { Id = 2, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 1, Name = "Add New Comapny", Description = "Allow User To Open New Company" });
            permissions.Add(new Permission() { Id = 3, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 1, Name = "Edit Company", Description = "Allow User To Edit Company Information" });
            permissions.Add(new Permission() { Id = 4, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 1, Name = "Add New Department", Description = "Allow User To Open New Department" });
            permissions.Add(new Permission() { Id = 5, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 1, Name = "Edit Department", Description = "Allow User To Edit Department Information" });
            permissions.Add(new Permission() { Id = 6, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 1, Name = "View InActive Companies", Description = "Allow User To View InActive Companies" });
            permissions.Add(new Permission() { Id = 7, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 1, Name = "View InActive Departments", Description = "Allow User To View InActive Departments" });
            permissions.Add(new Permission() { Id = 8, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 1, Name = "Mark Company as  InActive", Description = "Allow User To Mark Company as InActive." });




            //Employee 
            permissions.Add(new Permission() { Id = 3100, Added = System.DateTime.Now, LastModified = System.DateTime.Now, Name = "HRM", Description = "Leaves Related HRM" });

            permissions.Add(new Permission() { Id = 3101, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3100, Name = "Access Employment Centre", Description = "Allow User To Open Employee Center" });

            permissions.Add(new Permission() { Id = 3103, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3101, Name = "Add New Employee", Description = "Allow User To Add New Employee " });//

            //View Employee Permissions
            permissions.Add(new Permission() { Id = 3104, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3101, Name = "View Employee Register", Description = "Allow User To View All Employee Register." });//

            permissions.Add(new Permission() { Id = 3105, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3101, Name = "View Employee", Description = "Allow User To View Employee" });

            //permissions.Add(new Permission() { Id = 1665, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 1664, Name = "View All Employee Personal Info", Description = "Allow User To View All Employee Personal Info." });
            //permissions.Add(new Permission() { Id = 1666, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 1664, Name = "View All Employee Contact and Address", Description = "Allow User To View All Employee Contact and Address." });
            //permissions.Add(new Permission() { Id = 1667, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 1664, Name = "View All Employee Company and Department", Description = "Allow User To View All Employee Company and Department." });
            //permissions.Add(new Permission() { Id = 1668, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 1664, Name = "View All Employee Qualification", Description = "Allow User To View All Employee Qualification." });
            //permissions.Add(new Permission() { Id = 1669, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 1664, Name = "View All Employee Employment Info", Description = "Allow User To View All Employee Employment Info." });
            //permissions.Add(new Permission() { Id = 1670, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 1664, Name = "View All Employee Sale Target", Description = "Allow User To View All Employee Sale Target." });

            permissions.Add(new Permission() { Id = 3111, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3101, Name = "Add Employee without Approval", Description = "Allow User To Add Employee without Approval" });//
            permissions.Add(new Permission() { Id = 3112, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3101, Name = "Close Employee without Approval", Description = "Allow User To Close Employee without Approval" });//
            permissions.Add(new Permission() { Id = 3113, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3101, Name = "Close Employee", Description = "Allow User To Close Employee" });//
            permissions.Add(new Permission() { Id = 3114, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3101, Name = "Reviewer Level 1 Employee", Description = "Reviewer Level 1 Employee" });
            permissions.Add(new Permission() { Id = 3115, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3101, Name = "Reviewer Level 2 Employee", Description = "Reviewer Level 2 Employee" });
            permissions.Add(new Permission() { Id = 3116, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3101, Name = "Approver for Closing Employee", Description = "Approver for Closing Employee" });
            permissions.Add(new Permission() { Id = 3117, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3101, Name = "Approver for new added Employee", Description = "Approver for new added Employee" });//
            permissions.Add(new Permission() { Id = 3118, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3101, Name = "Edit closed Employee", Description = "Allow User To Edit closed Employee" });//
            permissions.Add(new Permission() { Id = 3119, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3101, Name = "View(Pending for Approval) Employee List", Description = "Allow User To View(Pending for Approval) Employee List" });//
            permissions.Add(new Permission() { Id = 3120, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3101, Name = "View(Pending for closing) Employee List", Description = "Allow User To View(Pending for closing) Employee List" });
            permissions.Add(new Permission() { Id = 3121, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3101, Name = "Edit Creation Date of Employee", Description = "Allow User To Edit Creation Date of Employee" });
            permissions.Add(new Permission() { Id = 3122, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3101, Name = "Attach a file with Employee", Description = "Allow User To Attach a file with Employee" });
            permissions.Add(new Permission() { Id = 3123, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3101, Name = "View list of attached files with Employee", Description = "Allow User To View list of attached files with Employee" });
            permissions.Add(new Permission() { Id = 3124, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3101, Name = "Mark as Void Employee", Description = "Allow User To Mark as Void Employee" });
            permissions.Add(new Permission() { Id = 3125, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3101, Name = "Unmark Void Employee", Description = "Allow User To Unmark Void Employee" });
            permissions.Add(new Permission() { Id = 3126, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3101, Name = "View list of Void Employee", Description = "View list of Void Employee" });

            permissions.Add(new Permission() { Id = 3127, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3101, Name = "ReApprover for new added Employee", Description = "Allow User To ReApprove Employee which is in pending state" });//
            permissions.Add(new Permission() { Id = 3128, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3101, Name = "Edit Employee without ReApproval", Description = "Allow User To Add New Employee without ReApproval" });//
            permissions.Add(new Permission() { Id = 3129, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3101, Name = "View(Pending for ReApproval) Employee List", Description = "Allow User To View List of All(Pending for ReApproval) Employee mapped to his Department" });//
            permissions.Add(new Permission() { Id = 3130, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3101, Name = "View (Employee Register)", Description = "Allow User To View List of All Employee Registers" });
            permissions.Add(new Permission() { Id = 3131, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3101, Name = "View InActive Employees", Description = "Allow User To View InActive Employees" });
            permissions.Add(new Permission() { Id = 3132, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3101, Name = "Mark Employee as InActive", Description = "Allow User To Mark Employee as InActive." });

            permissions.Add(new Permission() { Id = 3133, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3101, Name = "Add Employee Status", Description = "Allow User To Add Employee Status" });//
            permissions.Add(new Permission() { Id = 3134, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3101, Name = "Edit Employee Status", Description = "Allow User To Edit Employee Status" });//
            permissions.Add(new Permission() { Id = 3135, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3101, Name = "View Employee Status List", Description = "Allow User To View Employee Status List" });//
            permissions.Add(new Permission() { Id = 3136, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3101, Name = "Edit Employee", Description = "Allow User To Edit Employee Information" });//
                                                                                                                                                                                                                               //16-19 resserved
                                                                                                                                                                                                                               //Edit Employee persmissions
            permissions.Add(new Permission() { Id = 3137, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3136, Name = "Edit Personal Info", Description = "Allow User To Edit Personal Info." });

            // permissions.Add(new Permission() { Id = 7001, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3137, Name = "Edit Personal Photo", Description = "Allow User To Edit Personal Photo." });

            permissions.Add(new Permission() { Id = 3138, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3136, Name = "Edit Company and Department", Description = "Allow User To Edit Company and Department" });
            permissions.Add(new Permission() { Id = 3139, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3136, Name = "Edit Qualification", Description = "Allow User To Edit Qualification." });
            permissions.Add(new Permission() { Id = 3140, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3136, Name = "Edit Employment Info", Description = "Allow User To Edit Employment Info." });
            permissions.Add(new Permission() { Id = 3141, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3136, Name = "Edit Employee Sale Target", Description = "Allow User To Employee Sale Target." });
            permissions.Add(new Permission() { Id = 3142, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3136, Name = "Edit Employee Work Experience", Description = "Allow User To access emloye work experience tab." });
            permissions.Add(new Permission() { Id = 3143, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3136, Name = "Edit Employee Photo", Description = "Allow User To Upload/Delete photo from Form." });
            permissions.Add(new Permission() { Id = 3144, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3136, Name = "Edit Contact and Address", Description = "Allow User To Edit Contact and Address." });
            permissions.Add(new Permission() { Id = 3150, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3136, Name = "Edit Employee Picture", Description = "Allow User To Edit Employee Multiple Picture." });



            //Skype and Teams
            permissions.Add(new Permission() { Id = 3145, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3144, Name = "Add Skype and Teams Id", Description = "Allow User To Add Skype and Teams Id." });
            permissions.Add(new Permission() { Id = 3146, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3144, Name = "Edit Skype and Teams Id", Description = "Allow User To Edit Skype and Teams Id." });
            permissions.Add(new Permission() { Id = 3147, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3144, Name = "View skype and Teams Id", Description = "Allow User To View skype and Teams Id." });
            // Skype and Teams passwords permissions
            permissions.Add(new Permission() { Id = 3148, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3144, Name = "Add Skype and Teams Password", Description = "Allow User To Add Skype and Teams Password." });
            permissions.Add(new Permission() { Id = 3149, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3144, Name = "View skype and Teams Password", Description = "Allow User To View skype and Teams Password." });
            permissions.Add(new Permission() { Id = 3151, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3144, Name = "Edit Skype and Teams Password", Description = "Allow User To Edit Skype and Teams Password" });

            //HRM LEaves related Permissions (3050 3099)
            permissions.Add(new Permission() { Id = 3154, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3100, Name = "Access Employee Leaves", Description = "Allow User To Open Employee Leaves" });

            permissions.Add(new Permission() { Id = 3155, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3154, Name = "View Leave Register", Description = "Allow User To View Leave Register" });//


            permissions.Add(new Permission() { Id = 3156, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3155, Name = "Access to Allocate leave Button", Description = "Allow User To Access to Allocate leave Button" });//
            permissions.Add(new Permission() { Id = 3157, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3155, Name = "Add leave application (Admin)", Description = "Allow User To Add leave application (Admin)" });//
            permissions.Add(new Permission() { Id = 3158, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3155, Name = "Edit Leave application (Admin)", Description = "Allow User To Edit Leave application (Admin)" });//

            permissions.Add(new Permission() { Id = 3159, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3155, Name = "Add leave Adjustment (Admin)", Description = "Allow User To Add leave Adjustment (Admin)" });


            permissions.Add(new Permission() { Id = 3160, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3155, Name = "Add Leave without Approval", Description = "Allow User To Add Leave without Approval" });
            permissions.Add(new Permission() { Id = 3161, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3155, Name = "Close Leave without Approval", Description = "Allow User To Close Leave without Approval" });
            permissions.Add(new Permission() { Id = 3162, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3155, Name = "Close Leave", Description = "Allow User To Close Leave" });//
            permissions.Add(new Permission() { Id = 3163, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3155, Name = "Reviewer Level 1 Leave", Description = "Reviewer Level 1 Leave" });
            permissions.Add(new Permission() { Id = 3164, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3155, Name = "Reviewer Level 2 Leave", Description = "Reviewer Level 2 Leave" });
            permissions.Add(new Permission() { Id = 3165, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3155, Name = "Approver for Closing Leave", Description = "Approver for Closing Leave" });
            permissions.Add(new Permission() { Id = 3166, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3155, Name = "Approver for new added Leave", Description = "Approver for new added Leave" });//
            permissions.Add(new Permission() { Id = 3167, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3155, Name = "Edit closed Leave", Description = "Allow User To Edit closed Leave" });
            permissions.Add(new Permission() { Id = 3168, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3155, Name = "View(Pending for Approval) Leave List", Description = "Allow User To View(Pending for Approval) Leave List" });//
            permissions.Add(new Permission() { Id = 3169, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3155, Name = "View(Pending for closing) Leave List", Description = "Allow User To View(Pending for closing) Leave List" });//
            permissions.Add(new Permission() { Id = 3170, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3155, Name = "Edit Creation Date of Leave", Description = "Allow User To Edit Creation Date of Leave" });
            permissions.Add(new Permission() { Id = 3171, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3155, Name = "Attach a file with Leave", Description = "Allow User To Attach a file with Leave" });
            permissions.Add(new Permission() { Id = 3172, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3155, Name = "View list of attached files with Leave", Description = "Allow User To View list of attached files with Leave" });
            permissions.Add(new Permission() { Id = 3173, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3155, Name = "Mark as Void Leave", Description = "Allow User To Mark as Void Leave" });
            permissions.Add(new Permission() { Id = 3174, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3155, Name = "Unmark Void Leave", Description = "Allow User To Unmark Void Leave" });
            permissions.Add(new Permission() { Id = 3175, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3155, Name = "View list of Void Leaves", Description = "View list of Void Leave" });


            permissions.Add(new Permission() { Id = 3176, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3155, Name = "ReApprover for added Leave", Description = "Allow User To ReApprove Leave which is in pending state" });//
            permissions.Add(new Permission() { Id = 3177, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3155, Name = "Edit Leave without ReApproval", Description = "Allow User To Add New Sale Leave without ReApproval" });
            permissions.Add(new Permission() { Id = 3178, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3155, Name = "View(Pending for ReApproval) Leave List", Description = "Allow User To View List of All(Pending for ReApproval) Leave mapped to his Department" });
            permissions.Add(new Permission() { Id = 3179, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3155, Name = "Mark Leave as InActive", Description = "Allow User To Mark Leave as InActive." });
            permissions.Add(new Permission() { Id = 3180, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3155, Name = "View All Leave Register", Description = "Allow User To View All Leave Register." });
            permissions.Add(new Permission() { Id = 3181, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3155, Name = "View InActive Leaves", Description = "Allow User To View InActive Leave" });

            permissions.Add(new Permission() { Id = 3182, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3155, Name = "Add Leave Status", Description = "Allow User To Add Leave Status" });//
            permissions.Add(new Permission() { Id = 3183, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3155, Name = "Edit Leave Status", Description = "Allow User To Edit Leave Status" });//
            permissions.Add(new Permission() { Id = 3184, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3155, Name = "View Leave Status List", Description = "Allow User To View Leave Status List" });//
            permissions.Add(new Permission() { Id = 3185, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3100, Name = "Access to Leave Summary", Description = "Allow User To Access to Leave Summary from HRM menu" });

            permissions.Add(new Permission() { Id = 3186, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3100, Name = "Access Data Retrieval Dates Structure", Description = "Allow User To Access Data Retrieval Dates Structure" });





            //Customer
            permissions.Add(new Permission() { Id = 20, Added = System.DateTime.Now, LastModified = System.DateTime.Now, Name = "Customer Center", Description = "Allow User To Open Customer Center" });
            permissions.Add(new Permission() { Id = 21, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 20, Name = "Add New Customer", Description = "Allow User To Add New Customer" });
            permissions.Add(new Permission() { Id = 22, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 20, Name = "Edit Customer", Description = "Allow User To Edit Customer" });
            permissions.Add(new Permission() { Id = 23, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 20, Name = "See Transactions", Description = "Allow User To View Transaction done From his Account" });
            permissions.Add(new Permission() { Id = 24, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 20, Name = "View InActive Customers", Description = "Allow User To View InActive Customers" });
            permissions.Add(new Permission() { Id = 25, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 20, Name = "Mark Customer as InActive", Description = "Allow User To Mark Customer as InActive." });


            //Add Customer info tabs
            permissions.Add(new Permission() { Id = 26, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 20, Name = "Add Customer Info", Description = "Allow User To Add Customer Information" });
            //Add [Customer info tabs] Child permission start
            permissions.Add(new Permission() { Id = 5060, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 26, Name = "Add  Company Name", Description = "Allow User To Add Customer Company Name" });
            permissions.Add(new Permission() { Id = 5061, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 26, Name = "Add  Industry", Description = "Allow User To Add Customer Industry" });
            permissions.Add(new Permission() { Id = 5062, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 26, Name = "Add  Business Type", Description = "Allow User To Add Customer Industry" });
            permissions.Add(new Permission() { Id = 5063, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 26, Name = "Add  Currency", Description = "Allow User To Add Customer Currency" });
            //Add Customer Billing address start
            permissions.Add(new Permission() { Id = 5064, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 26, Name = "Add  Billing address line1", Description = "Allow User To Add Customer Billing address line Number 1" });
            permissions.Add(new Permission() { Id = 5065, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 26, Name = "Add  Billing address line2", Description = "Allow User To Add Customer Billing address line Number 2" });
            permissions.Add(new Permission() { Id = 5066, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 26, Name = "Add  Billing address City", Description = "Allow User To Add Customer Billing address City" });
            permissions.Add(new Permission() { Id = 5067, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 26, Name = "Add  Billing address State", Description = "Allow User To Add Customer Billing address State" });
            permissions.Add(new Permission() { Id = 5068, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 26, Name = "Add  Billing address Country", Description = "Allow User To Add Customer Billing address Country" });
            permissions.Add(new Permission() { Id = 5069, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 26, Name = "Add  Billing address Zip", Description = "Allow User To Add Customer Billing address Zip" });
            permissions.Add(new Permission() { Id = 5070, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 26, Name = "Add  Billing address Region", Description = "Allow User To Add Customer Billing address Region" });
            //Add Customer Shipping address start
            permissions.Add(new Permission() { Id = 5071, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 26, Name = "Add  Shipping address line1", Description = "Allow User To Add Customer Shipping address line Number 1" });
            permissions.Add(new Permission() { Id = 5072, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 26, Name = "Add  Shipping address line2", Description = "Allow User To Add Customer Shipping address line Number 2" });
            permissions.Add(new Permission() { Id = 5073, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 26, Name = "Add  Shipping address City", Description = "Allow User To Add Customer Shipping address City" });
            permissions.Add(new Permission() { Id = 5074, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 26, Name = "Add  Shipping address State", Description = "Allow User To Add Customer Shipping address State" });
            permissions.Add(new Permission() { Id = 5075, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 26, Name = "Add  Shipping address Country", Description = "Allow User To Add Customer Shipping address Country" });
            permissions.Add(new Permission() { Id = 5076, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 26, Name = "Add  Shipping address Zip", Description = "Allow User To Add Customer Shipping address Zip" });
            permissions.Add(new Permission() { Id = 5077, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 26, Name = "Add  Shipping address Region", Description = "Allow User To Add Customer Shipping address Region" });


            permissions.Add(new Permission() { Id = 27, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 20, Name = "Add Customer Address", Description = "Allow User To Add Customer Address Information" });
            permissions.Add(new Permission() { Id = 28, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 20, Name = "Add Customer Tax Info", Description = "Allow User To Add Customer Tax Information" });
            permissions.Add(new Permission() { Id = 5100, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 28, Name = "Add Vat Number", Description = "Allow User To Add Customer VAT Number" });
            permissions.Add(new Permission() { Id = 5101, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 28, Name = "Add NTN number", Description = "Allow User To Add NTN number" });
            permissions.Add(new Permission() { Id = 5102, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 28, Name = "Add Customer sale tax registration Number", Description = "Allow User To Add Customer sale tax registration Number" });


            permissions.Add(new Permission() { Id = 5001, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 20, Name = "Add Customer Contact Info", Description = "Allow User To Add Customer Contact Information" });

            permissions.Add(new Permission() { Id = 5202, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 5001, Name = "Add First Name", Description = "Allow User To Add Customer First Name" });
            permissions.Add(new Permission() { Id = 5203, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 5001, Name = "Add Last Name", Description = "Allow User To Add Customer Last Name" });
            permissions.Add(new Permission() { Id = 5204, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 5001, Name = "Add Default Phone Number", Description = "Allow User To Add Customer Default Phone Number" });
            permissions.Add(new Permission() { Id = 5205, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 5001, Name = "Add More Phone Number", Description = "Allow User To Add More Phone Number" });
            permissions.Add(new Permission() { Id = 5206, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 5001, Name = "Add FAX Number", Description = "Allow User To Add Customer FAX Number" });
            permissions.Add(new Permission() { Id = 5207, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 5001, Name = "Add Default Email", Description = "Allow User To Add Customer Default Email" });
            permissions.Add(new Permission() { Id = 5208, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 5001, Name = "Add More Email", Description = "Allow User To Add Customer More Email" });
            permissions.Add(new Permission() { Id = 5209, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 5001, Name = "Add Website", Description = "Allow User To Add Customer Website" });
            permissions.Add(new Permission() { Id = 5210, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 5001, Name = "Add Link1", Description = "Allow User To Add Customer link one" });
            permissions.Add(new Permission() { Id = 5211, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 5001, Name = "Add Link2", Description = "Allow User To Add Customer Link 2" });
            permissions.Add(new Permission() { Id = 5212, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 5001, Name = "Add Link3", Description = "Allow User To Add Customer Link three" });



            permissions.Add(new Permission() { Id = 5002, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 20, Name = "Add Customer Department Info", Description = "Allow User To Add Customer Department Information" });

            permissions.Add(new Permission() { Id = 5003, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 20, Name = "Add Customer Company Info", Description = "Allow User To Add Customer Company Information" });

            permissions.Add(new Permission() { Id = 5004, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 20, Name = "Edit Customer Info", Description = "Allow User To Edit Customer Information" });
            //Add [Customer info tabs] Child permission start
            permissions.Add(new Permission() { Id = 5300, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 5004, Name = "Edit  Company Name", Description = "Allow User To Edit Customer Company Name" });
            permissions.Add(new Permission() { Id = 5301, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 5004, Name = "Edit  Industry", Description = "Allow User To Edit Customer Industry" });
            permissions.Add(new Permission() { Id = 5302, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 5004, Name = "Edit  Business Type", Description = "Allow User To Edit Customer Industry" });
            permissions.Add(new Permission() { Id = 5303, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 5004, Name = "Edit  Currency", Description = "Allow User To Edit Customer Currency" });
            //Add Customer Billing address start
            permissions.Add(new Permission() { Id = 5304, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 5004, Name = "Edit  Billing address line1", Description = "Allow User To Edit Customer Billing address line Number 1" });
            permissions.Add(new Permission() { Id = 5305, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 5004, Name = "Edit  Billing address line2", Description = "Allow User To Edit Customer Billing address line Number 2" });
            permissions.Add(new Permission() { Id = 5306, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 5004, Name = "Edit  Billing address City", Description = "Allow User To Edit Customer Billing address City" });
            permissions.Add(new Permission() { Id = 5307, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 5004, Name = "Edit  Billing address State", Description = "Allow User To Edit Customer Billing address State" });
            permissions.Add(new Permission() { Id = 5308, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 5004, Name = "Edit  Billing address Country", Description = "Allow User To Edit Customer Billing address Country" });
            permissions.Add(new Permission() { Id = 5309, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 5004, Name = "Edit  Billing address Zip", Description = "Allow User To Edit Customer Billing address Zip" });
            permissions.Add(new Permission() { Id = 5310, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 5004, Name = "Edit  Billing address Region", Description = "Allow User To Edit Customer Billing address Region" });
            //Add Customer Shipping address start
            permissions.Add(new Permission() { Id = 5311, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 5004, Name = "Edit  Shipping address line1", Description = "Allow User To Edit Customer Shipping address line Number 1" });
            permissions.Add(new Permission() { Id = 5312, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 5004, Name = "Edit  Shipping address line2", Description = "Allow User To Edit Customer Shipping address line Number 2" });
            permissions.Add(new Permission() { Id = 5313, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 5004, Name = "Edit  Shipping address City", Description = "Allow User To Edit Customer Shipping address City" });
            permissions.Add(new Permission() { Id = 5314, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 5004, Name = "Edit  Shipping address State", Description = "Allow User To Edit Customer Shipping address State" });
            permissions.Add(new Permission() { Id = 5315, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 5004, Name = "Edit  Shipping address Country", Description = "Allow User To Edit Customer Shipping address Country" });
            permissions.Add(new Permission() { Id = 5316, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 5004, Name = "Edit  Shipping address Zip", Description = "Allow User To Edit Customer Shipping address Zip" });
            permissions.Add(new Permission() { Id = 5317, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 5004, Name = "Edit  Shipping address Region", Description = "Allow User To Edit Customer Shipping address Region" });




            permissions.Add(new Permission() { Id = 5005, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 20, Name = "Edit Customer Address", Description = "Allow User To Edit Customer Address Information" });
            permissions.Add(new Permission() { Id = 5006, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 20, Name = "Edit Customer Tax Info", Description = "Allow User To Edit Customer Tax Information" });

            permissions.Add(new Permission() { Id = 5400, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 5006, Name = "Edit Vat Number", Description = "Allow User To Edit Customer VAT Number" });
            permissions.Add(new Permission() { Id = 5401, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 5006, Name = "Edit NTN number", Description = "Allow User To Edit NTN number" });
            permissions.Add(new Permission() { Id = 5402, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 5006, Name = "Edit Customer sale tax registration Number", Description = "Allow User To Edit Customer sale tax registration Number" });



            permissions.Add(new Permission() { Id = 5007, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 20, Name = "Edit Customer Contact Info", Description = "Allow User To Edit Customer Contact Information" });

            permissions.Add(new Permission() { Id = 5502, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 5007, Name = "Edit First Name", Description = "Allow User To Edit Customer First Name" });
            permissions.Add(new Permission() { Id = 5503, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 5007, Name = "Edit Last Name", Description = "Allow User To Edit Customer Last Name" });
            permissions.Add(new Permission() { Id = 5504, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 5007, Name = "Edit Default Phone Number", Description = "Allow User To Edit Customer Default Phone Number" });
            permissions.Add(new Permission() { Id = 5505, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 5007, Name = "Edit More Phone Number", Description = "Allow User To Edit More Phone Number" });
            permissions.Add(new Permission() { Id = 5506, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 5007, Name = "Edit FAX Number", Description = "Allow User To Edit Customer FAX Number" });
            permissions.Add(new Permission() { Id = 5507, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 5007, Name = "Edit Default Email", Description = "Allow User To Edit Customer Default Email" });
            permissions.Add(new Permission() { Id = 5508, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 5007, Name = "Edit More Email", Description = "Allow User To Edit Customer More Email" });
            permissions.Add(new Permission() { Id = 5509, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 5007, Name = "Edit Website", Description = "Allow User To Edit Customer Website" });
            permissions.Add(new Permission() { Id = 5510, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 5007, Name = "Edit Link1", Description = "Allow User To Edit Customer link one" });
            permissions.Add(new Permission() { Id = 5511, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 5007, Name = "Edit Link2", Description = "Allow User To Edit Customer Link 2" });
            permissions.Add(new Permission() { Id = 5512, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 5007, Name = "Edit Link3", Description = "Allow User To Edit Customer Link three" });



            permissions.Add(new Permission() { Id = 5008, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 20, Name = "Edit Customer Department Info", Description = "Allow User To Edit Customer Department Information" });
            permissions.Add(new Permission() { Id = 5009, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 20, Name = "Edit Customer Company Info", Description = "Allow User To Edit Customer Company Information" });
            permissions.Add(new Permission() { Id = 5010, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 20, Name = "View Customer Detail Info", Description = "Allow User To View Customer Company Information" });
            permissions.Add(new Permission() { Id = 5011, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 20, Name = "Update Contact Person Detail", Description = "Allow User To View Customer Company Information" });
            permissions.Add(new Permission() { Id = 5010, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 20, Name = "View Contact Detail Info", Description = "Allow User To View contact person Information" });
            permissions.Add(new Permission() { Id = 5011, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 20, Name = "View Customer Register", Description = "Allow User To View Customer Register" });
            permissions.Add(new Permission() { Id = 5012, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 20, Name = "View Customer Sale Orders", Description = "Allow User To View Customer Sale Orders" });
            //Vendor
            permissions.Add(new Permission() { Id = 29, Added = System.DateTime.Now, LastModified = System.DateTime.Now, Name = "Vendor", Description = "Allow User To Open Vendor" });
            permissions.Add(new Permission() { Id = 30, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 29, Name = "Vendor Center", Description = "Allow User To Open Vendor Center" });
            permissions.Add(new Permission() { Id = 31, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 30, Name = "Add New Vendor", Description = "Allow User To Add Vendor" });
            permissions.Add(new Permission() { Id = 32, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 30, Name = "Edit Vendor", Description = "Allow User To Edit Vendor Information" });
            permissions.Add(new Permission() { Id = 33, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 30, Name = "View InActive Vendor", Description = "Allow User To View InActive Vendors" });
            permissions.Add(new Permission() { Id = 34, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 30, Name = "Mark Vendor as InActive", Description = "Allow User To Mark Vendor as InActive." });
            permissions.Add(new Permission() { Id = 35, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 30, Name = "View InActive Vendor Payment Statuses", Description = "Allow User To View InActive Vendors" });
            permissions.Add(new Permission() { Id = 36, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 30, Name = "Add Vendor Payment Status", Description = "Allow User To Add New Vendor Payment Status" });
            permissions.Add(new Permission() { Id = 37, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 30, Name = "Edit Vendor Payment Status", Description = "Allow User To edit Vendor Payment Status" });
            permissions.Add(new Permission() { Id = 38, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 30, Name = "View Vendor Purchase Orders", Description = "Allow User To View Vendor Purchase Orders" });
            permissions.Add(new Permission() { Id = 39, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 30, Name = "View Vendor Register", Description = "Allow User To View Vendor Register" });
            permissions.Add(new Permission() { Id = 45, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 30, Name = "Mark Vendor as BlackList", Description = "Allow User To Mark Vendor as BlackList" });
            permissions.Add(new Permission() { Id = 46, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 30, Name = "View Vendor finder", Description = "Allow User To View Vendor finder" });


            //Principal
            permissions.Add(new Permission() { Id = 40, Added = System.DateTime.Now, LastModified = System.DateTime.Now, Name = "Principal Center", Description = "Allow User To Open Principal Center" });
            permissions.Add(new Permission() { Id = 41, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 40, Name = "Add New Principal", Description = "Allow User To Add New Principal" });
            permissions.Add(new Permission() { Id = 42, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 40, Name = "Edit Principal", Description = "Allow User To Edit Principal" });
            permissions.Add(new Permission() { Id = 43, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 40, Name = "View InActive Principal", Description = "Allow User To View InActive Principals" });
            permissions.Add(new Permission() { Id = 44, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 40, Name = "Mark Prinicpal as InActive", Description = "Allow User To Mark Principal as InActive." });
            //procurment
            permissions.Add(new Permission() { Id = 50, Added = System.DateTime.Now, LastModified = System.DateTime.Now, Name = "Procurment Panel", Description = "Allow User To Open Procurmant Panel" });
            //// 360 View 
            permissions.Add(new Permission() { Id = 120, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 50, Name = "View Transactions in Tracking 360 Window", Description = "View Transactions in Tracking 360 Window" });
            permissions.Add(new Permission() { Id = 121, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 50, Name = "View list of attached files in Tracking 360 Window", Description = "View list of attached files in Tracking 360 Window" });
            permissions.Add(new Permission() { Id = 122, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 50, Name = "View CostSheet in Tracking 360 Window", Description = "View CostSheet in Tracking 360 Window" });
            permissions.Add(new Permission() { Id = 123, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 50, Name = "View Bill Tracking in 360 tracking Window", Description = "View Bill Tracking in 360 tracking Window" });

            ////// Inquiry
            ///
            {
                permissions.Add(new Permission() { Id = 51, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 50, Name = "Inquiries", Description = "Inquiries" });

                permissions.Add(new Permission() { Id = 52, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 51, Name = "Add Inquiry", Description = "Allow User To Add New Inquiry" });
                permissions.Add(new Permission() { Id = 53, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 51, Name = "Edit Inquiry", Description = "Allow User To Edit Inquiry" });
                permissions.Add(new Permission() { Id = 54, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 51, Name = "View Inquiry Details", Description = "Allow User To View Inquiry Details" });
                permissions.Add(new Permission() { Id = 55, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 51, Name = "List Of Inquiries", Description = "Allow User To View List Of Inquiries" });
                permissions.Add(new Permission() { Id = 56, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 51, Name = "Add Inquiry Status", Description = "Allow User to Add New Status for Inquiry" });
                permissions.Add(new Permission() { Id = 57, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 51, Name = "Create Offer from Inquiry", Description = "Allow User To Create Offer from an Existing Inquiry" });
                permissions.Add(new Permission() { Id = 58, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 51, Name = "Edit Inquiry Status", Description = "Allow User To Edit Inquiry Status" });
                permissions.Add(new Permission() { Id = 59, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 51, Name = "View InActive Inquiry Statuses", Description = "Allow User To View InActive Inquiry Statuses" });
                permissions.Add(new Permission() { Id = 60, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 51, Name = "Add Inquiry without Approval", Description = "Allow User To Add New Inquiry without Approval" });
                permissions.Add(new Permission() { Id = 61, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 51, Name = "View InActive Inquiries", Description = "Allow User To View InActive Inquiries" });
                permissions.Add(new Permission() { Id = 62, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 51, Name = "Close Inquiry without Approval", Description = "Allow User To Close Inquiry without Approval" });
                permissions.Add(new Permission() { Id = 63, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 51, Name = "Close Inquiry", Description = "Allow User To Close Inquiry with with approval" });
                permissions.Add(new Permission() { Id = 64, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 51, Name = "Reviewer Level 1", Description = "Allow User to mark Inquiry as Reviewed once" });
                permissions.Add(new Permission() { Id = 65, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 51, Name = "Reviewer Level 2", Description = "Allow User To Mark Inquiry as Reviewed and move it to Approved Inquiries List" });
                permissions.Add(new Permission() { Id = 67, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 51, Name = "Approver for Closing", Description = "Allow User To Close Inquiry which is in pending state" });
                permissions.Add(new Permission() { Id = 68, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 51, Name = "Approver for new added Inquiry", Description = "Allow User To Approve Inquiry which is in pending state" });
                permissions.Add(new Permission() { Id = 69, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 51, Name = "Edit Closed Inquiry", Description = "Allow User To Edit Closed Inquiry" });
                permissions.Add(new Permission() { Id = 70, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 51, Name = "View(Pending for Approval) Inquiry List", Description = "Allow User To View List of All(Pending for Approval) Inquiries mapped to his Department" });
                permissions.Add(new Permission() { Id = 71, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 51, Name = "View(Pending for closing) Inquiry List", Description = "Allow User To View List of All(Pending for Approval) Inquiries mapped to his Department" });
                permissions.Add(new Permission() { Id = 72, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 51, Name = "Edit Creation Date of Inquiry", Description = "Allow User To Edit Creation Date of Inquiry" });
                permissions.Add(new Permission() { Id = 73, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 51, Name = "Attach a file with Inquiry", Description = "Allow User To Attach a file with Inquiry" });
                permissions.Add(new Permission() { Id = 74, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 51, Name = "View list of attached files with Inquiry", Description = "Allow User To View list of attached files with Inquiry" });
                permissions.Add(new Permission() { Id = 75, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 51, Name = "Mark as Void Inquiry", Description = "Allow User To Mark as Void Inquiry" });
                permissions.Add(new Permission() { Id = 76, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 51, Name = "Unmark Void Inquiry", Description = "Allow User To unmark as Void Inquiry" });
                permissions.Add(new Permission() { Id = 78, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 51, Name = "View list of Void Inquiries", Description = "View list of Void Inquiries" });
                permissions.Add(new Permission() { Id = 79, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 51, Name = "Move Inquiry to Inter Company", Description = "Allow User To Move Inquiry to Inter Company mapped to his Department" });
            }

            ///////////////////////////////////////////////////////////////
            ///
            {
                permissions.Add(new Permission() { Id = 301, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 50, Name = "Offers", Description = "Offers" });

                permissions.Add(new Permission() { Id = 302, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 301, Name = "Add New Offer ", Description = "Allow User To Add New Offer" });
                permissions.Add(new Permission() { Id = 303, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 301, Name = "Edit Offer", Description = "Allow User To Edit Offer Details" });
                permissions.Add(new Permission() { Id = 304, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 301, Name = "Offer Details", Description = "Allow User To View Offer Details" });
                permissions.Add(new Permission() { Id = 305, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 301, Name = "List Of Offers", Description = "Allow User To view List of Offers" });
                permissions.Add(new Permission() { Id = 306, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 301, Name = "Add Offer Status", Description = "Allow User To Add new Status for Offer" });
                permissions.Add(new Permission() { Id = 307, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 301, Name = "View InActive Offers", Description = "Allow User To View InActive Offers" });
                permissions.Add(new Permission() { Id = 308, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 301, Name = "Add Offer without Approval", Description = "Allow User To Add New Offer without Approval" });
                permissions.Add(new Permission() { Id = 309, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 301, Name = "Edit Offer Status", Description = "Allow User To Edit Offer Status" });
                permissions.Add(new Permission() { Id = 310, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 301, Name = "View InActive Offer Statuses", Description = "Allow User To View InActive Offer Statuses" });
                permissions.Add(new Permission() { Id = 311, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 301, Name = "Create Sale Order from Offer", Description = "Allow User To Create Purchase Order from an existing Offer" });
                permissions.Add(new Permission() { Id = 312, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 301, Name = "Close Offer without Approval", Description = "Allow User To Close Offer without Approval" });
                permissions.Add(new Permission() { Id = 313, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 301, Name = "Close Offer", Description = "Allow User To Close Offer" });
                permissions.Add(new Permission() { Id = 314, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 301, Name = "Reviewer Level 1 Offer", Description = "Allow User to mark Offer as Reviewed once" });
                permissions.Add(new Permission() { Id = 315, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 301, Name = "Reviewer Level 2 Offer", Description = "Allow User To Mark Offer as Reviewed and move it to Approved  List" });
                permissions.Add(new Permission() { Id = 316, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 301, Name = "Approver for Closing Offer", Description = "Allow User To Close Offer which is in pending state" });
                permissions.Add(new Permission() { Id = 317, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 301, Name = "Approver for new added Offer", Description = "Allow User To Approve Offer which is in pending state" });
                permissions.Add(new Permission() { Id = 318, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 301, Name = "Edit closed Offer", Description = "Allow User To Edit Closed Offer" });
                permissions.Add(new Permission() { Id = 319, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 301, Name = "View(Pending for Approval) Offer List", Description = "Allow User To View List of All(Pending for Approval) Offers mapped to his Department" });
                permissions.Add(new Permission() { Id = 320, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 301, Name = "View(Pending for closing) Offer List", Description = "Allow User To View List of All(Pending for Closing) Offer mapped to his Department" });
                permissions.Add(new Permission() { Id = 321, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 301, Name = "Edit Creation Date of Offer", Description = "Allow User To Edit Creation Date of Offer" });
                permissions.Add(new Permission() { Id = 322, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 301, Name = "Attach a file with Offer", Description = "Allow User To Attach a file with Offer" });
                permissions.Add(new Permission() { Id = 323, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 301, Name = "View list of attached files with Offer", Description = "Allow User To View list of attached files with Offer" });
                permissions.Add(new Permission() { Id = 324, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 301, Name = "Mark as Void Offer", Description = "Allow User To Mark as Void Offer" });
                permissions.Add(new Permission() { Id = 325, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 301, Name = "Unmark Void Offer", Description = "Allow User To unmark as Void Offer" });
                permissions.Add(new Permission() { Id = 326, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 301, Name = "View list of Void Offers", Description = "View list of Void Offers" });
                permissions.Add(new Permission() { Id = 327, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 301, Name = "Move Offer to Inter Company", Description = "Allow User To Move Offer to Inter Company mapped to his Department" });
                permissions.Add(new Permission() { Id = 328, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 301, Name = "Add Vendor Comparative Statement", Description = "Allow User To Add Vendor Comparative Statement" });
                permissions.Add(new Permission() { Id = 329, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 301, Name = "View Vendor Comparative Statement", Description = "Allow User To View Vendor Comparative Statement" });
                permissions.Add(new Permission() { Id = 330, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 301, Name = "Import Offer Items", Description = "Allow User To Import Offer Items" });
                permissions.Add(new Permission() { Id = 9000, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 301, Name = "View Offer Register", Description = "Allow User To View Offer Register" });
                permissions.Add(new Permission() { Id = 9001, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 301, Name = "Edit Cost Center after Offer Approval", Description = "Allow User To Edit Cost Center after Offer Approval" });

            }
            ///////////////////////////////////
            /// Sale Order
            /// 
            ///
            {
                permissions.Add(new Permission() { Id = 331, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 50, Name = "Sale Orders", Description = "Sale Orders" });
                permissions.Add(new Permission() { Id = 332, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "Add Sale Order", Description = "Allow User To Add Sale Order" });
                permissions.Add(new Permission() { Id = 333, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "Edit Sale Order", Description = "Allow User To Edit Sale Order" });
                permissions.Add(new Permission() { Id = 334, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "View Sale Order", Description = "Allow User To View Sale Order" });
                permissions.Add(new Permission() { Id = 335, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "List of Sale Orders", Description = "Allow User To View List of Sale Order" });
                permissions.Add(new Permission() { Id = 336, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "Add Sale Order Status", Description = "Allow User To Add new status for Sale Order" });
                permissions.Add(new Permission() { Id = 337, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "Add Sale Order without Approval", Description = "Allow User To Add New Sale Order without Approval" });
                permissions.Add(new Permission() { Id = 338, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "View InActive Sale Orders", Description = "Allow User To View InActive Sale Orders" });
                permissions.Add(new Permission() { Id = 339, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "Close Sale Order", Description = "Allow User To Close Sale Order" });
                permissions.Add(new Permission() { Id = 340, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "Edit Sale Order Status", Description = "Allow User To Edit Sale Order Status" });
                permissions.Add(new Permission() { Id = 341, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "View InActive Sale Order Statuses", Description = "Allow User To View InActive Sale Order Statuses" });
                permissions.Add(new Permission() { Id = 342, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "Create Po from Sale Order", Description = "Allow User To Create Purchase Order from an existing SaleOrder" });
                permissions.Add(new Permission() { Id = 343, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "Close Sale Order without Approval", Description = "Allow User To Close Sale Order without Approval" });
                permissions.Add(new Permission() { Id = 344, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "Edit Unapproved Sale Order", Description = "Allow User To Edit Unapproved Sale Order without Approval" });

                permissions.Add(new Permission() { Id = 346, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "Reviewer Level 1 SaleOrder", Description = "Allow User to mark SaleOrder as Reviewed once" });
                permissions.Add(new Permission() { Id = 347, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "Reviewer Level 2 SaleOrder", Description = "Allow User To Mark SaleOrder as Reviewed and move it to Approved List" });
                permissions.Add(new Permission() { Id = 348, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "Approver for Closing SaleOrder", Description = "Allow User To Close SaleOrder which is in pending state" });
                permissions.Add(new Permission() { Id = 349, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "Approver for new added SaleOrder", Description = "Allow User To Approve SaleOrder which is in pending state" });
                permissions.Add(new Permission() { Id = 350, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "Edit closed SaleOrder", Description = "Allow User To Edit Closed Sale Order" });
                permissions.Add(new Permission() { Id = 351, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "View(Pending for closing) SaleOrder List", Description = "Allow User To View List of All(Pending for Approval) SaleOrder mapped to his Department" });
                permissions.Add(new Permission() { Id = 352, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "View(Pending for Approval) SaleOrder List", Description = "Allow User To View List of All(Pending for Approval) SaleOrder mapped to his Department" });
                permissions.Add(new Permission() { Id = 353, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "View Sale Register", Description = "Allow User To View Sale Register List" });
                permissions.Add(new Permission() { Id = 354, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "Create SaleInvoice from SaleOrder", Description = "Allow User To Create Sale Invoice from an existing SaleOrder" });
                permissions.Add(new Permission() { Id = 355, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "Edit Creation Date of SaleOrder", Description = "Allow User To Edit Creation Date of SaleOrder" });
                permissions.Add(new Permission() { Id = 356, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "Attach a file with SaleOrder", Description = "Allow User To Attach a file with SaleOrder" });
                permissions.Add(new Permission() { Id = 357, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "View list of attached files with SaleOrder", Description = "Allow User To View list of attached files with SaleOrder" });
                permissions.Add(new Permission() { Id = 358, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "Edit FOB and CFR value for products in SaleOrder", Description = "Allow User To Edit FOB and CFR value for products in SaleOrder" });
                permissions.Add(new Permission() { Id = 359, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "Edit Commision Field without Approval", Description = "Allow User To Edit Commision Amount in Sale Order" });

                permissions.Add(new Permission() { Id = 360, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "ReApprover for new added SaleOrder", Description = "Allow User To ReApprove SaleOrder which is in pending state" });
                permissions.Add(new Permission() { Id = 361, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "Add Sale Order without ReApproval", Description = "Allow User To Add New Sale Order without ReApproval" });
                permissions.Add(new Permission() { Id = 362, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "View(Pending for ReApproval) SaleOrder List", Description = "Allow User To View List of All(Pending for ReApproval) SaleOrder mapped to his Department" });
                permissions.Add(new Permission() { Id = 363, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "Mark as Void SaleOrder", Description = "Allow User To Mark as Void SaleOrder" });
                permissions.Add(new Permission() { Id = 364, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "Unmark Void SaleOrder", Description = "Allow User To unmark as Void SaleOrder" });
                permissions.Add(new Permission() { Id = 365, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "View list of Void SaleOrders", Description = "View list of Void SaleOrders" });
                permissions.Add(new Permission() { Id = 366, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "Edit (Pending for closing) SaleOrder", Description = "Allow User To Edit (Pending for Closing) SaleOrder mapped to his Department" });
                //permissions.Add(new Permission() { Id = 367, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "Edit FOB and CFR value for products in Sale Order", Description = "Allow User To Edit FOB and CFR value for products in SaleOrder" });
                permissions.Add(new Permission() { Id = 368, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "Move SaleOrder to Inter Company", Description = "Allow User To Move SaleOrder to Inter Company mapped to his Department" });
                permissions.Add(new Permission() { Id = 369, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "Edit Sale Order delivery date", Description = "Allow User To Edit Sale Order delivery date" });
                permissions.Add(new Permission() { Id = 370, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "Edit Sale Order delivery date Final", Description = "Allow User To Edit Sale Order delivery date Final" });
                permissions.Add(new Permission() { Id = 371, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "Close Sale Order without receiving fully Collection", Description = "Allow User To Close Sale Order without receiving fully Collection" });
                permissions.Add(new Permission() { Id = 372, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "Edit RSBC from Sale Order", Description = "Edit RSBC from Sale Order" });
                permissions.Add(new Permission() { Id = 373, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "View CostSheet when Sale Order closed", Description = "Allow User To View CostSheet when Sale Order closed" });
                permissions.Add(new Permission() { Id = 374, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "Edit Sale Order value after approval", Description = "Allow User To Edit Sale Order value after approval" });
                permissions.Add(new Permission() { Id = 375, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "Edit Sale Order value before approval", Description = "Allow User To Edit Sale Order value before approval" });
                permissions.Add(new Permission() { Id = 376, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "Can attach document when SO Closed", Description = "Allow User To attach document when SO Closed" });
                permissions.Add(new Permission() { Id = 377, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "Edit Cost Center after SO Approval", Description = "Allow User To Edit Cost Center after SO Approval" });
                permissions.Add(new Permission() { Id = 378, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "Copy Sale Order Template", Description = "Allow User To Copy Sale Order Template" });
                permissions.Add(new Permission() { Id = 379, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "SO Exchange Rates", Description = "Allow User To SO Exchange Rates" });
                permissions.Add(new Permission() { Id = 380, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 379, Name = "Edit SO SER", Description = "Allow User To Edit SO SER" });
                permissions.Add(new Permission() { Id = 381, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 379, Name = "Edit SO MER", Description = "Allow User To Edit SO MER" });
                permissions.Add(new Permission() { Id = 382, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 379, Name = "View Sale Exchange Rate in Sale Order", Description = "Allow User To View Sale Exchange Rate in Sale Order" });
                permissions.Add(new Permission() { Id = 344, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 379, Name = "View Market Exchange Rate in Sale Order", Description = "Allow User To View Market Exchange Rate in Sale Order" });
                permissions.Add(new Permission() { Id = 345, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "View Customer Tracking By SO", Description = "Allow User To View Customer Tracking By SO" });
                permissions.Add(new Permission() { Id = 346, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "View Vendor Tracking By PO", Description = "Allow User To View Vendor Tracking By PO" });
                permissions.Add(new Permission() { Id = 347, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "Add Audit Year", Description = "Allow User To Add Audit Year" });
                permissions.Add(new Permission() { Id = 348, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "List of PerformanceSheet heads", Description = "Allow User To View List of PerformanceSheet heads" });
                permissions.Add(new Permission() { Id = 349, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "View Performance Sheet", Description = "Allow User To View Performance Sheet" });
                permissions.Add(new Permission() { Id = 350, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "Edit Performance Sheet", Description = "Allow User To Edit Performance Sheet" });
                permissions.Add(new Permission() { Id = 351, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "Edit Performance Sheet Initial Points", Description = "Allow User To Edit Performance Sheet Initial Points" });
                permissions.Add(new Permission() { Id = 352, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "Edit Performance Sheet Revised Points", Description = "Allow User To Edit Performance Sheet Revised Points" });
                permissions.Add(new Permission() { Id = 353, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "Create Sale order from reference key", Description = "Allow User To Create Sale order from reference key" });
                permissions.Add(new Permission() { Id = 354, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "Can Generate SO Reference key", Description = "Allow User To Generate SO Reference key" });
                permissions.Add(new Permission() { Id = 355, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "View linked sale orders", Description = "Allow User To view linked sale orders" });

                permissions.Add(new Permission() { Id = 356, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "Can View PRIT Register", Description = "Allow User To view PRIT Register" });
                permissions.Add(new Permission() { Id = 357, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "Can Add Audit Year Adjustment", Description = "Allow User To add Audit Year Adjustment" });
                permissions.Add(new Permission() { Id = 358, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "Can View Audit Year Register", Description = "Allow User To view Audit Year Adjustment Register" });
                permissions.Add(new Permission() { Id = 359, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 701, Name = "Can Add Insurance", Description = "Allow User To Add Insurance" });

                permissions.Add(new Permission() { Id = 383, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "Add Vendor Bill Reference Number in Sale Order", Description = "Allow User to Add Vendor Bill Reference Number in Sale Order" });
                permissions.Add(new Permission() { Id = 384, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "Can Edit Vendor Bill Reference Number After Approval in Sale Order", Description = "Allow User to Edit Vendor Bill Reference Number After Approval in Sale Order" });
                permissions.Add(new Permission() { Id = 387, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "View List SO Ref Keys", Description = "Allow User to View List SO Ref Keys" });
                permissions.Add(new Permission() { Id = 388, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "Update SO Ref Keys", Description = "Allow User to Update SO Ref Keys" });

                permissions.Add(new Permission() { Id = 385, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "View List SO Ref Keys", Description = "Allow User to View List SO Ref Keys" });
                permissions.Add(new Permission() { Id = 386, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "Update SO Ref Keys", Description = "Allow User to Update SO Ref Keys" });
                permissions.Add(new Permission() { Id = 387, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "Allow user to delink Approved SaleOrder", Description = "Allow user to delink Approved SaleOrder" });
                permissions.Add(new Permission() { Id = 389, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "Allow user to delink unApproved SaleOrder", Description = "Allow user to delink unApproved SaleOrder" });

                permissions.Add(new Permission() { Id = 390, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "Can Close Sale Order without Closing Vendor Bill", Description = "Allow user to Close Sale Order without Closing Vendor Bill" });
            }

            ///// Cost Sheet
            ///
            {
                permissions.Add(new Permission() { Id = 500, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "Cost Sheet", Description = "Allow user to view Cost Sheet " });
                permissions.Add(new Permission() { Id = 501, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 500, Name = "Edit Budgeted Costs", Description = "Allow User To Edit Budgeted Costs in cost sheet" });
                permissions.Add(new Permission() { Id = 502, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 500, Name = "Edit Actual Costs", Description = "Allow User To Edit Actual Costs in cost sheet" });
                permissions.Add(new Permission() { Id = 503, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 500, Name = "Cost Sheet Settings", Description = "Allow User To add new fields in a cost sheet and make old ones inactive" });
                permissions.Add(new Permission() { Id = 504, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 500, Name = "Edit Revised Costs", Description = "Allow User To Edit Revised Costs in cost sheet" });
                permissions.Add(new Permission() { Id = 505, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 500, Name = "Edit Revised Costs with out Approval", Description = "Allow User To Edit Revised Costs in cost sheet" });
                permissions.Add(new Permission() { Id = 506, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 500, Name = "View edit history of Costs", Description = "Allow User To View edit history of costs in cost sheet" });
                permissions.Add(new Permission() { Id = 507, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 500, Name = "Edit Adjustment System Costs", Description = "Allow User To Edit Adjustment System Costs in cost sheet" });
                permissions.Add(new Permission() { Id = 508, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 500, Name = "Edit Vendor Nature Manual", Description = "Allow User ToEdit Vendor Nature Manual" });
                permissions.Add(new Permission() { Id = 509, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 500, Name = "Edit Vendor Nature", Description = "Allow User To Edit Vendor Nature" });
                permissions.Add(new Permission() { Id = 510, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 500, Name = "Edit Vendor Rating", Description = "Allow User To Edit Vendor Rating" });
                permissions.Add(new Permission() { Id = 511, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 500, Name = "Edit Advance Costs", Description = "Allow User To Edit Advance Costs in cost sheet" });



            }
            ///Summary Sheet
            {
                permissions.Add(new Permission() { Id = 550, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 331, Name = "Commission Summary Sheet", Description = "Allow user to view Cost Sheet " });
                permissions.Add(new Permission() { Id = 551, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 550, Name = "Edit Offer Fields", Description = "Allow User To Edit Budgeted Costs in cost sheet" });
                permissions.Add(new Permission() { Id = 552, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 550, Name = "Edit SaleOrder Fields", Description = "Allow User To Edit Actual Costs in cost sheet" });
                permissions.Add(new Permission() { Id = 553, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 550, Name = "Add Marketing Cost in Summary Sheet", Description = "Allow User To Add Marketing Cost in Summary Sheet" });
                permissions.Add(new Permission() { Id = 554, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 550, Name = "Marketing Fields Settings", Description = "Allow User To Add new Marketing fields for Summary Sheet" });
            }
            //Memorandum Sales
            {
                permissions.Add(new Permission() { Id = 601, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 50, Name = "Memorandum Sales", Description = "Memorandum Sales" });
                permissions.Add(new Permission() { Id = 602, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 601, Name = "Add Memorandum Sale", Description = "Allow User To Add Memorandum Sale" });
                permissions.Add(new Permission() { Id = 603, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 601, Name = "Edit Memorandum Sale", Description = "Allow User To Edit Memorandum Sale" });
                permissions.Add(new Permission() { Id = 604, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 601, Name = "View Memorandum Sale", Description = "Allow User To View Memorandum Sale" });
                permissions.Add(new Permission() { Id = 605, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 601, Name = "List of Memorandum Sales", Description = "Allow User To View List of Memorandum Sale" });
                permissions.Add(new Permission() { Id = 606, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 601, Name = "Add Memorandum Sale Status", Description = "Allow User To Add new status for Memorandum Sale" });
                permissions.Add(new Permission() { Id = 607, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 601, Name = "Add Memorandum Sale without Approval", Description = "Allow User To Add New Memorandum Sale without Approval" });
                permissions.Add(new Permission() { Id = 608, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 601, Name = "View InActive Memorandum Sales", Description = "Allow User To View InActive Memorandum Sales" });
                permissions.Add(new Permission() { Id = 609, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 601, Name = "Close Memorandum Sale", Description = "Allow User To Close Memorandum Sale" });
                permissions.Add(new Permission() { Id = 610, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 601, Name = "Edit Memorandum Sale Status", Description = "Allow User To Edit Memorandum Sale Status" });
                permissions.Add(new Permission() { Id = 611, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 601, Name = "View InActive Memorandum Sale Statuses", Description = "Allow User To View InActive Memorandum Sale Statuses" });

                permissions.Add(new Permission() { Id = 612, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 601, Name = "Close Memorandum Sale without Approval", Description = "Allow User To Close Memorandum Sale without Approval" });
                permissions.Add(new Permission() { Id = 613, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 601, Name = "Edit Unapproved Memorandum Sale", Description = "Allow User To Edit Unapproved Memorandum Sale without Approval" });


                permissions.Add(new Permission() { Id = 614, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 601, Name = "Reviewer Level 1 MemorandumSale", Description = "Allow User to mark MemorandumSale as Reviewed once" });
                permissions.Add(new Permission() { Id = 615, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 601, Name = "Reviewer Level 2 MemorandumSale", Description = "Allow User To Mark MemorandumSale as Reviewed and move it to Approved List" });
                permissions.Add(new Permission() { Id = 616, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 601, Name = "Approver for Closing MemorandumSale", Description = "Allow User To Close MemorandumSale which is in pending state" });
                permissions.Add(new Permission() { Id = 617, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 601, Name = "Approver for new added MemorandumSale", Description = "Allow User To Approve Offer which is in pending state" });
                permissions.Add(new Permission() { Id = 618, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 601, Name = "Edit closed MemorandumSale", Description = "Allow User To Edit Closed Memorandum Sale" });
                permissions.Add(new Permission() { Id = 619, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 601, Name = "View(Pending for closing) MemorandumSale List", Description = "Allow User To View List of All(Pending for closing) MemorandumSale mapped to his Department" });
                permissions.Add(new Permission() { Id = 620, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 601, Name = "View(Pending for Approval) MemorandumSale List", Description = "Allow User To View List of All(Pending for Approval) MemorandumSale mapped to his Department" });
                permissions.Add(new Permission() { Id = 621, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 601, Name = "Edit Creation Date of MemorandumSale", Description = "Allow User To Edit Creation Date of MemorandumSale" });
                permissions.Add(new Permission() { Id = 622, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 601, Name = "Attach a file with MemorandumSale", Description = "Allow User To Attach a file with MemorandumSale" });
                permissions.Add(new Permission() { Id = 623, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 601, Name = "View list of attached files with MemorandumSale", Description = "Allow User To View list of attached files with MemorandumSale" });
                permissions.Add(new Permission() { Id = 625, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 601, Name = "Unmark Void MemorandumSale", Description = "Allow User To unmark as Void MemorandumSale" });
                permissions.Add(new Permission() { Id = 626, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 601, Name = "View list of Void MemorandumSales", Description = "View list of Void MemorandumSales" });
                permissions.Add(new Permission() { Id = 627, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 601, Name = "Move MemorandumSale to Inter Company", Description = "Allow User To Move MemorandumSale to Inter Company mapped to his Department" });
                permissions.Add(new Permission() { Id = 628, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 601, Name = "Edit FOB and CFR value for products in MemorandumSale", Description = "Allow User To Edit FOB and CFR value for products in MemorandumSale" });
            }

            //// Sale Invoices
            //{
            //    permissions.Add(new Permission() { Id = 701, Added = DateTime.Now, LastModified = DateTime.Now, ParentId = 50, Name = "Sale Invoices", Description = "Sale Invoices" });

            //    permissions.Add(new Permission() { Id = 702, Added = DateTime.Now, LastModified = DateTime.Now, ParentId = 701, Name = "Add Sale Invoice", Description = "Allow User To Add Sale Invoice" });
            //    permissions.Add(new Permission() { Id = 703, Added = DateTime.Now, LastModified = DateTime.Now, ParentId = 701, Name = "Edit Sale Invoice", Description = "Allow User To Edit Sale Invoice" });
            //    permissions.Add(new Permission() { Id = 704, Added = DateTime.Now, LastModified = DateTime.Now, ParentId = 701, Name = "View Sale Invoice", Description = "Allow User To View Sale Invoice" });
            //    permissions.Add(new Permission() { Id = 705, Added = DateTime.Now, LastModified = DateTime.Now, ParentId = 701, Name = "List of Sale Invoices", Description = "Allow User To View List of Sale Invoice" });

            //    permissions.Add(new Permission() { Id = 706, Added = DateTime.Now, LastModified = DateTime.Now, ParentId = 701, Name = "Add Sale Invoice Status", Description = "Allow User To Add new status for Sale Invoice" });
            //    permissions.Add(new Permission() { Id = 707, Added = DateTime.Now, LastModified = DateTime.Now, ParentId = 701, Name = "Add Sale Invoice without Approval", Description = "Allow User To Add New Sale Invoice without Approval" });

            //    permissions.Add(new Permission() { Id = 708, Added = DateTime.Now, LastModified = DateTime.Now, ParentId = 701, Name = "View InActive Sale Invoices", Description = "Allow User To View InActive Sale Invoices" });
            //    permissions.Add(new Permission() { Id = 709, Added = DateTime.Now, LastModified = DateTime.Now, ParentId = 701, Name = "Close Sale Invoice", Description = "Allow User To Close Sale Invoice" });
            //    permissions.Add(new Permission() { Id = 710, Added = DateTime.Now, LastModified = DateTime.Now, ParentId = 701, Name = "Edit Sale Invoice Status", Description = "Allow User To Edit Sale Invoice Status" });
            //    permissions.Add(new Permission() { Id = 711, Added = DateTime.Now, LastModified = DateTime.Now, ParentId = 701, Name = "View InActive Sale Invoice Statuses", Description = "Allow User To View InActive Sale Invoice Statuses" });

            //    permissions.Add(new Permission() { Id = 713, Added = DateTime.Now, LastModified = DateTime.Now, ParentId = 701, Name = "Close Sale Invoice without Approval", Description = "Allow User To Close Sale Invoice without Approval" });
            //    permissions.Add(new Permission() { Id = 714, Added = DateTime.Now, LastModified = DateTime.Now, ParentId = 701, Name = "Edit Unapproved Sale Invoice", Description = "Allow User To Edit Unapproved Sale Invoice without Approval" });

            //    permissions.Add(new Permission() { Id = 716, Added = DateTime.Now, LastModified = DateTime.Now, ParentId = 701, Name = "Reviewer Level 1 SaleInvoice", Description = "Allow User to mark SaleInvoice as Reviewed once" });
            //    permissions.Add(new Permission() { Id = 717, Added = DateTime.Now, LastModified = DateTime.Now, ParentId = 701, Name = "Reviewer Level 2 SaleInvoice", Description = "Allow User To Mark SaleInvoice as Reviewed and move it to Approved List" });

            //    permissions.Add(new Permission() { Id = 718, Added = DateTime.Now, LastModified = DateTime.Now, ParentId = 701, Name = "Approver for Closing SaleInvoice", Description = "Allow User To Close SaleInvoice which is in pending state" });
            //    permissions.Add(new Permission() { Id = 719, Added = DateTime.Now, LastModified = DateTime.Now, ParentId = 701, Name = "Approver for new added SaleInvoice", Description = "Allow User To Approve Offer which is in pending state" });

            //    permissions.Add(new Permission() { Id = 720, Added = DateTime.Now, LastModified = DateTime.Now, ParentId = 701, Name = "Edit closed SaleInvoice", Description = "Allow User To Edit Closed Sale Invoice" });

            //    permissions.Add(new Permission() { Id = 721, Added = DateTime.Now, LastModified = DateTime.Now, ParentId = 701, Name = "View(Pending for closing) SaleInvoice List", Description = "Allow User To View Pending Closing SaleInvoices" });
            //    permissions.Add(new Permission() { Id = 722, Added = DateTime.Now, LastModified = DateTime.Now, ParentId = 701, Name = "View(Pending for Approval) SaleInvoice List", Description = "Allow User To View Pending Approval SaleInvoices" });

            //    permissions.Add(new Permission() { Id = 723, Added = DateTime.Now, LastModified = DateTime.Now, ParentId = 701, Name = "Edit Creation Date of SaleInvoice", Description = "Allow User To Edit Creation Date of SaleInvoice" });
            //    permissions.Add(new Permission() { Id = 724, Added = DateTime.Now, LastModified = DateTime.Now, ParentId = 701, Name = "Attach a file with SaleInvoice", Description = "Allow User To Attach a file with SaleInvoice" });
            //    permissions.Add(new Permission() { Id = 725, Added = DateTime.Now, LastModified = DateTime.Now, ParentId = 701, Name = "View list of attached files with SaleInvoice", Description = "Allow User To View attached files" });

            //    permissions.Add(new Permission() { Id = 726, Added = DateTime.Now, LastModified = DateTime.Now, ParentId = 701, Name = "Mark as Void SaleInvoice", Description = "Allow User To Mark as Void SaleInvoice" });
            //    permissions.Add(new Permission() { Id = 727, Added = DateTime.Now, LastModified = DateTime.Now, ParentId = 701, Name = "Unmark Void SaleInvoice", Description = "Allow User To Unmark Void SaleInvoice" });
            //    permissions.Add(new Permission() { Id = 728, Added = DateTime.Now, LastModified = DateTime.Now, ParentId = 701, Name = "View list of Void SaleInvoices", Description = "View void invoices" });

            //    permissions.Add(new Permission() { Id = 744, Added = DateTime.Now, LastModified = DateTime.Now, ParentId = 701, Name = "Move SaleInvoice to Inter Company", Description = "Allow User To Move SaleInvoice to Inter Company mapped to his Department" });

            //    permissions.Add(new Permission() { Id = 730, Added = DateTime.Now, LastModified = DateTime.Now, ParentId = 701, Name = "Close Invoice without receiving fully Collection", Description = "Allow User To Close Invoice without full payment" });
            //    permissions.Add(new Permission() { Id = 731, Added = DateTime.Now, LastModified = DateTime.Now, ParentId = 701, Name = "View Sales Invoice Register", Description = "Allow User To View Sales Invoice Register" });
            //    permissions.Add(new Permission() { Id = 732, Added = DateTime.Now, LastModified = DateTime.Now, ParentId = 701, Name = "Edit Sales Invoice Value after approval", Description = "Allow User To Edit after approval" });
            //    permissions.Add(new Permission() { Id = 733, Added = DateTime.Now, LastModified = DateTime.Now, ParentId = 701, Name = "Edit Sales Invoice Value before approval", Description = "Allow User To Edit before approval" });

            //    permissions.Add(new Permission() { Id = 734, Added = DateTime.Now, LastModified = DateTime.Now, ParentId = 701, Name = "Can attach document when Sale Invoice Closed", Description = "Allow attachments after close" });

            //    permissions.Add(new Permission() { Id = 735, Added = DateTime.Now, LastModified = DateTime.Now, ParentId = 701, Name = "SI Exchange Rates", Description = "Exchange rate permissions group" });
            //    permissions.Add(new Permission() { Id = 736, Added = DateTime.Now, LastModified = DateTime.Now, ParentId = 735, Name = "Edit SI SER", Description = "Allow SER edit" });
            //    permissions.Add(new Permission() { Id = 737, Added = DateTime.Now, LastModified = DateTime.Now, ParentId = 735, Name = "Edit SI MER", Description = "Allow MER edit" });

            //    permissions.Add(new Permission() { Id = 738, Added = DateTime.Now, LastModified = DateTime.Now, ParentId = 701, Name = "View Market Exchange Rate in Sale Invoice", Description = "Allow viewing exchange rates" });

            //    permissions.Add(new Permission() { Id = 739, Added = DateTime.Now, LastModified = DateTime.Now, ParentId = 701, Name = "Update CostSheet from Sales Invoice", Description = "Sync cost sheet" });
            //    permissions.Add(new Permission() { Id = 740, Added = DateTime.Now, LastModified = DateTime.Now, ParentId = 701, Name = "Edit is-InterCompany receivable check", Description = "Intercompany control" });
            //    permissions.Add(new Permission() { Id = 741, Added = DateTime.Now, LastModified = DateTime.Now, ParentId = 701, Name = "Edit STL stamp", Description = "Edit stamp" });
            //    permissions.Add(new Permission() { Id = 742, Added = DateTime.Now, LastModified = DateTime.Now, ParentId = 701, Name = "Edit STL discount", Description = "Edit discount" });
            //    permissions.Add(new Permission() { Id = 743, Added = DateTime.Now, LastModified = DateTime.Now, ParentId = 701, Name = "View Customer Credits Report", Description = "View credit report" });
            //}

            ////////////////////
            /// Purcahse order

            {
                permissions.Add(new Permission() { Id = 1001, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 50, Name = "Purchase Orders", Description = "Purchase Orders" });
                permissions.Add(new Permission() { Id = 1002, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 1001, Name = "Add Purchase Order", Description = "Allow User To Add Purchase Order" });
                permissions.Add(new Permission() { Id = 1003, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 1001, Name = "Edit Purchase Order", Description = "Allow User To Edit Purchase Order" });
                permissions.Add(new Permission() { Id = 1004, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 1001, Name = "View Purchase Order", Description = "Allow User To View Purchase Order" });
                permissions.Add(new Permission() { Id = 1005, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 1001, Name = "List of Purchase Orders", Description = "Allow User To View List of Purchase Order" });
                permissions.Add(new Permission() { Id = 1006, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 1001, Name = "Add Purchase Order Status", Description = "Allow User To Add new status for Purchase Order" });
                permissions.Add(new Permission() { Id = 1007, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 1001, Name = "Add Purchase Order without Approval", Description = "Allow User To Add New Purchase Order without Approval" });
                permissions.Add(new Permission() { Id = 1008, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 1001, Name = "View InActive Purchase Orders", Description = "Allow User To View InActive Purchase Orders" });
                permissions.Add(new Permission() { Id = 1009, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 1001, Name = "Close Purchase Order", Description = "Allow User To Close Purchase Order" });
                permissions.Add(new Permission() { Id = 1010, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 1001, Name = "Edit Purchase Order Status", Description = "Allow User To Edit Purchase Order Status" });
                permissions.Add(new Permission() { Id = 1011, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 1001, Name = "View InActive Purchase Order Statuses", Description = "Allow User To View InActive Purchase Order Statuses" });
                //permissions.Add(new Permission() { Id = 1012, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 1001, Name = "Create Po from Purchase Order", Description = "Allow User To Create Purchase Order from an existing PurchaseOrder" });
                permissions.Add(new Permission() { Id = 1013, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 1001, Name = "Close Purchase Order without Approval", Description = "Allow User To Close Purchase Order without Approval" });
                permissions.Add(new Permission() { Id = 1014, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 1001, Name = "Edit Unapproved Purchase Order", Description = "Allow User To Edit Unapproved Purchase Order without Approval" });
                permissions.Add(new Permission() { Id = 1014, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 1001, Name = "View Market Exchange Rate in Purchase Order", Description = "Allow User To View Market Exchange Rate in Purchase Order" });

                permissions.Add(new Permission() { Id = 1016, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 1001, Name = "Reviewer Level 1 PurchaseOrder", Description = "Allow User to mark PurchaseOrder as Reviewed once" });
                permissions.Add(new Permission() { Id = 1017, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 1001, Name = "Reviewer Level 2 PurchaseOrder", Description = "Allow User To Mark PurchaseOrder as Reviewed and move it to Approved List" });
                permissions.Add(new Permission() { Id = 1018, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 1001, Name = "Approver for Closing PurchaseOrder", Description = "Allow User To Close PurchaseOrder which is in pending state" });
                permissions.Add(new Permission() { Id = 1019, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 1001, Name = "Approver for new added PurchaseOrder", Description = "Allow User To Approve PurchaseOrder which is in pending state" });
                permissions.Add(new Permission() { Id = 1020, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 1001, Name = "Edit closed PurchaseOrder", Description = "Allow User To Edit Closed Purchase Order" });
                permissions.Add(new Permission() { Id = 1021, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 1001, Name = "View(Pending for closing) PurchaseOrder List", Description = "Allow User To View List of All(Pending for Approval) PurchaseOrder mapped to his Department" });
                permissions.Add(new Permission() { Id = 1022, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 1001, Name = "View(Pending for Approval) PurchaseOrder List", Description = "Allow User To View List of All(Pending for Approval) PurchaseOrder mapped to his Department" });
                permissions.Add(new Permission() { Id = 1023, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 1001, Name = "View Purchase Register", Description = "Allow User To View Purchase Register List" });
                permissions.Add(new Permission() { Id = 1024, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 1001, Name = "Create PurchaseInvoice from PurchaseOrder", Description = "Allow User To Create Purchase Invoice from an existing PurchaseOrder" });
                permissions.Add(new Permission() { Id = 1025, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 1001, Name = "Edit Creation Date of PurchaseOrder", Description = "Allow User To Edit Creation Date of PurchaseOrder" });
                permissions.Add(new Permission() { Id = 1026, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 1001, Name = "Attach a file with PurchaseOrder", Description = "Allow User To Attach a file with PurchaseOrder" });
                permissions.Add(new Permission() { Id = 1027, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 1001, Name = "View list of attached files with PurchaseOrder", Description = "Allow User To View list of attached files with PurchaseOrder" });
                permissions.Add(new Permission() { Id = 1028, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 1001, Name = "Edit FOB and CFR value for products in PurchaseOrder", Description = "Allow User To Edit FOB and CFR value for products in PurchaseOrder" });
                permissions.Add(new Permission() { Id = 1029, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 1001, Name = "Edit Commision Field without Approval", Description = "Allow User To Edit Commision Amount in Purchase Order" });

                permissions.Add(new Permission() { Id = 1030, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 1001, Name = "ReApprover for new added PurchaseOrder", Description = "Allow User To ReApprove PurchaseOrder which is in pending state" });
                permissions.Add(new Permission() { Id = 1031, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 1001, Name = "Add Purchase Order without ReApproval", Description = "Allow User To Add New Purchase Order without ReApproval" });
                permissions.Add(new Permission() { Id = 1032, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 1001, Name = "View(Pending for ReApproval) PurchaseOrder List", Description = "Allow User To View List of All(Pending for ReApproval) PurchaseOrder mapped to his Department" });
                permissions.Add(new Permission() { Id = 1033, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 1001, Name = "Mark as Void PurchaseOrder", Description = "Allow User To Mark as Void PurchaseOrder" });
                permissions.Add(new Permission() { Id = 1034, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 1001, Name = "Unmark Void PurchaseOrder", Description = "Allow User To unmark as Void PurchaseOrder" });
                permissions.Add(new Permission() { Id = 1035, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 1001, Name = "View list of Void PurchaseOrders", Description = "View list of Void PurchaseOrders" });
                permissions.Add(new Permission() { Id = 1036, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 1001, Name = "Edit (Pending for closing) PurchaseOrder", Description = "Allow User To Edit (Pending for Closing) PurchaseOrder mapped to his Department" });
                //permissions.Add(new Permission() { Id = 1037, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 1001, Name = "Edit FOB and CFR value for products in Unapproved Purchase Order", Description = "Allow User To Edit FOB and CFR value for products in PurchaseOrder" });
                permissions.Add(new Permission() { Id = 1038, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 1001, Name = "Move PurchaseOrder to Inter Company", Description = "Allow User To Move PurchaseOrder to Inter Company mapped to his Department" });
                permissions.Add(new Permission() { Id = 1039, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 1001, Name = "Update CostSheet from Purchase Order", Description = "Update CostSheet from Purchase Order" });
                permissions.Add(new Permission() { Id = 1040, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 1001, Name = "View CostSheet when Purchase Order closed", Description = "Allow User To View CostSheet when Purchase Order closed" });

                permissions.Add(new Permission() { Id = 1041, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 1001, Name = "Can Edit Basic Information After Approval in PO", Description = "Allow User To Edit Basic Information After Approval in PO" });
                permissions.Add(new Permission() { Id = 1042, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 1001, Name = "Can Edit Vendor in Purchase Order", Description = "Allow User To Edit Vendor in Purchase Order" });
                permissions.Add(new Permission() { Id = 1043, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 1001, Name = "Copy Purchase Order Template", Description = "Allow User To Copy Purchase Order Template" });


            }
            {
                permissions.Add(new Permission() { Id = 2001, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 50, Name = "Bills", Description = "Bills" });
                permissions.Add(new Permission() { Id = 2002, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2001, Name = "Add Bill", Description = "Allow User To Add Bill" });
                permissions.Add(new Permission() { Id = 2003, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2001, Name = "Edit Bill", Description = "Allow User To Edit Bill" });
                permissions.Add(new Permission() { Id = 2004, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2001, Name = "View Bill", Description = "Allow User To View Bill" });
                permissions.Add(new Permission() { Id = 2005, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2001, Name = "List of Bills", Description = "Allow User To View List of Bill" });
                permissions.Add(new Permission() { Id = 2037, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2001, Name = "List of Bills(Pending for Apprvoal)", Description = "Allow User To View List of Pending for Apprvoal Bills" });

                permissions.Add(new Permission() { Id = 2006, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2001, Name = "Add Bill Status", Description = "Allow User To Add new status for Bill" });
                permissions.Add(new Permission() { Id = 2007, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2001, Name = "Add Bill without Approval", Description = "Allow User To Add New Bill without Approval" });
                permissions.Add(new Permission() { Id = 2008, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2001, Name = "View InActive Bills", Description = "Allow User To View InActive Bills" });
                permissions.Add(new Permission() { Id = 2009, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2001, Name = "Close Bill", Description = "Allow User To Close Bill" });
                permissions.Add(new Permission() { Id = 2020, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2001, Name = "Edit Bill Status", Description = "Allow User To Edit Bill Status" });
                permissions.Add(new Permission() { Id = 2011, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2001, Name = "View InActive Bill Statuses", Description = "Allow User To View InActive Bill Statuses" });
                //permissions.Add(new Permission() { Id = 2012, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2001, Name = "Create Po from Bill", Description = "Allow User To Create Bill from an existing Bill" });
                permissions.Add(new Permission() { Id = 2013, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2001, Name = "Close Bill without Approval", Description = "Allow User To Close Bill without Approval" });
                permissions.Add(new Permission() { Id = 2014, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2001, Name = "Edit Unapproved Bill", Description = "Allow User To Edit Unapproved Bill without Approval" });
                permissions.Add(new Permission() { Id = 2012, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2001, Name = "View Market Exchange Rate in Bill", Description = "Allow User To View Market Exchange Rate in Bill" });

                permissions.Add(new Permission() { Id = 2016, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2001, Name = "Reviewer Level 1 Bill", Description = "Allow User to mark Bill as Reviewed once" });
                permissions.Add(new Permission() { Id = 2017, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2001, Name = "Reviewer Level 2 Bill", Description = "Allow User To Mark Bill as Reviewed and move it to Approved List" });
                permissions.Add(new Permission() { Id = 2018, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2001, Name = "Approver for Closing Bill", Description = "Allow User To Close Bill which is in pending state" });
                permissions.Add(new Permission() { Id = 2019, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2001, Name = "Approver for new added Bill", Description = "Allow User To Approve Bill which is in pending state" });
                permissions.Add(new Permission() { Id = 2020, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2001, Name = "Edit closed Bill", Description = "Allow User To Edit Closed Bill" });
                permissions.Add(new Permission() { Id = 2021, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2001, Name = "View(Pending for closing) Bill List", Description = "Allow User To View List of All(Pending for Approval) Bill mapped to his Department" });
                permissions.Add(new Permission() { Id = 2022, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2001, Name = "View(Pending for Approval) Bill List", Description = "Allow User To View List of All(Pending for Approval) Bill mapped to his Department" });
                permissions.Add(new Permission() { Id = 2023, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2001, Name = "View Bill Register", Description = "Allow User To view Bill Register List" });
                permissions.Add(new Permission() { Id = 2024, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2001, Name = "Create PurchaseInvoice from Bill", Description = "Allow User To Create Purchase Invoice from an existing Bill" });
                permissions.Add(new Permission() { Id = 2025, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2001, Name = "Edit Creation Date of Bill", Description = "Allow User To Edit Creation Date of Bill" });
                permissions.Add(new Permission() { Id = 2026, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2001, Name = "Attach a file with Bill", Description = "Allow User To Attach a file with Bill" });
                permissions.Add(new Permission() { Id = 2027, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2001, Name = "View list of attached files with Bill", Description = "Allow User To View list of attached files with Bill" });

                permissions.Add(new Permission() { Id = 2030, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2001, Name = "ReApprover for new added Bill", Description = "Allow User To ReApprove Bill which is in pending state" });
                permissions.Add(new Permission() { Id = 2031, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2001, Name = "Add Bill without ReApproval", Description = "Allow User To Add New Bill without ReApproval" });
                permissions.Add(new Permission() { Id = 2032, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2001, Name = "View(Pending for ReApproval) Bill List", Description = "Allow User To View List of All(Pending for ReApproval) Bill mapped to his Department" });
                permissions.Add(new Permission() { Id = 2033, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2001, Name = "Mark as Void Bill", Description = "Allow User To Mark as Void Bill" });
                permissions.Add(new Permission() { Id = 2034, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2001, Name = "Unmark Void Bill", Description = "Allow User To unmark as Void Bill" });
                permissions.Add(new Permission() { Id = 2035, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2001, Name = "View list of Void Bills", Description = "View list of Void Bills" });
                permissions.Add(new Permission() { Id = 2036, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2001, Name = "Edit (Pending for closing) Bill", Description = "Allow User To Edit (Pending for Closing) Bill mapped to his Department" });
                //permissions.Add(new Permission() { Id = 2038, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2001, Name = "Edit FOB and CFR value for products in Unapproved Bills", Description = "Allow User To Edit FOB and CFR value for products in Bill" });

                permissions.Add(new Permission() { Id = 2039, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2001, Name = "Add Bill Type", Description = "Allow User To Add Bill Type" });
                permissions.Add(new Permission() { Id = 2040, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2001, Name = "Edit Bill Type", Description = "Allow User To Edit Bill Type" });
                permissions.Add(new Permission() { Id = 2041, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2001, Name = "Move Bill to Inter Company", Description = "Allow User To Move Bill to Inter Company mapped to his Department" });
                permissions.Add(new Permission() { Id = 2042, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2001, Name = "Update CostSheet from Bill", Description = "Allow User To Update CostSheet from Bill" });
                permissions.Add(new Permission() { Id = 2043, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 1001, Name = "View CostSheet when Bill closed", Description = "Allow User To View CostSheet when Bill closed" });
                permissions.Add(new Permission() { Id = 2044, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2001, Name = "Can Edit Vendor After Approval", Description = "Allow User To Edit Vendor After Approval" });
                permissions.Add(new Permission() { Id = 2045, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2001, Name = "Can Add Summary Memo for Vendor Bills", Description = "Allow User Can Add Summary Memo for Vendor Bills Add Summary Memo for Vendor Bills" });

                //Bill Reference
                permissions.Add(new Permission() { Id = 2046, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2001, Name = "Vendor Bill Reference", Description = "Allow User to Access Vendor Bill Reference" });
                permissions.Add(new Permission() { Id = 2047, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2046, Name = "Add Vendor Bill Reference", Description = "Allow User To Add Vendor Bill Reference" });
                permissions.Add(new Permission() { Id = 2048, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2046, Name = "Edit Vendor Bill Reference", Description = "Allow User To Edit Vendor Bill Reference" });
                permissions.Add(new Permission() { Id = 2049, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2046, Name = "List of Vendor Bill Reference", Description = "Allow User To View List of Vendor Bill Reference" });

                permissions.Add(new Permission() { Id = 2050, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2001, Name = "Add Vendor Bill Reference Number in Vendor Bill", Description = "Allow User To Add Vendor Bill Reference Number in Vendor Bill" });
                permissions.Add(new Permission() { Id = 2051, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2001, Name = "Edit GL Posting Date of VendorBill", Description = "Allow User To Edit GL Posting Date of VendorBill" });
                permissions.Add(new Permission() { Id = 2052, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2001, Name = "Can Edit Vendor Bill Reference Number After Approval", Description = "Allow User To Edit Vendor Bill Reference Number After Approval in Vendor Bill" });
                permissions.Add(new Permission() { Id = 2053, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2001, Name = "Edit Bill Amount and Bill Amount (SOC) value Under Approval for products in Bill", Description = "Allow User To Edit Bill Amount and Bill Amount (SOC) value Under Approval for products in Bill" });
                permissions.Add(new Permission() { Id = 2054, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2001, Name = "Edit Bill Amount and Bill Amount (SOC) value After Approval for products in Bill", Description = "Allow User To Edit Bill Amount and Bill Amount (SOC) value After Approval for products in Bill" });
                permissions.Add(new Permission() { Id = 2055, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2001, Name = "Close Vendor Bills without Payments", Description = "Allow User To Close Vendor Bills without Payments" });

                permissions.Add(new Permission() { Id = 2056, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2001, Name = "Link Vendor Bill with Loan Advance", Description = "Allow User To Link Vendor Bill with Loan Advance" });

                permissions.Add(new Permission() { Id = 2057, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2001, Name = "Can Approve Adjustments in Vendor Bills", Description = "Allow User To Approve Adjustments in Vendor Bills" });
            }


            {
                permissions.Add(new Permission() { Id = 4501, Added = System.DateTime.Now, LastModified = System.DateTime.Now, Name = "Targets", Description = "Targets" });

                permissions.Add(new Permission() { Id = 4500, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4501, Name = "Target Module", Description = "Target Module" });
                permissions.Add(new Permission() { Id = 4502, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4500, Name = "Add New Target", Description = "Allow User To Add New Target" });
                permissions.Add(new Permission() { Id = 4503, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4500, Name = "Edit Target", Description = "Allow User To Edit Target" });
                permissions.Add(new Permission() { Id = 4504, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4500, Name = "View Target", Description = "Allow User To View Target" });
                permissions.Add(new Permission() { Id = 4505, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4500, Name = "List of Targets", Description = "Allow User To View List of Targets" });
                permissions.Add(new Permission() { Id = 4506, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4500, Name = "List of Groups", Description = "Allow User To View List of Groups" });

                permissions.Add(new Permission() { Id = 4507, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4500, Name = "Add New Group", Description = "Allow User To Add New Groups" });
                permissions.Add(new Permission() { Id = 4508, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4500, Name = "Edit Group", Description = "Allow User To Edit Group" });
                permissions.Add(new Permission() { Id = 4509, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4500, Name = "View Group", Description = "Allow User To View Groups" });
                permissions.Add(new Permission() { Id = 4510, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4500, Name = "Add New Step", Description = "Allow User To Add New Step" });
                permissions.Add(new Permission() { Id = 4511, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4500, Name = "Edit Step", Description = "Allow User To Edit Steps" });
                permissions.Add(new Permission() { Id = 4512, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4500, Name = "View Step", Description = "Allow User To View Step" });

                permissions.Add(new Permission() { Id = 4513, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4500, Name = "Target Status", Description = "Target Status" });
                permissions.Add(new Permission() { Id = 4514, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4513, Name = "Add Target Status", Description = "Allow User To Add Target Status" });
                permissions.Add(new Permission() { Id = 4515, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4513, Name = "Edit Target Status", Description = "Allow User To Edit Target Status" });
                permissions.Add(new Permission() { Id = 4536, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4513, Name = "View InActive Statuses in Targets", Description = "Allow User To View InActive Statuses in Targets" });

                permissions.Add(new Permission() { Id = 4516, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4500, Name = "Mark as Void Target", Description = "Allow User To Mark as Void Target" });
                permissions.Add(new Permission() { Id = 4517, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4500, Name = "Unmark Void Target", Description = "Allow User To unmark as Void Target" });
                permissions.Add(new Permission() { Id = 4518, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4500, Name = "View list of Void Target", Description = "View list of Void Target" });

                permissions.Add(new Permission() { Id = 4519, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4500, Name = "View Group Target Register", Description = "Allow User To View Group Target Register" });
                permissions.Add(new Permission() { Id = 4520, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4500, Name = "View Target Register", Description = "Allow User To View Target Register" });

                permissions.Add(new Permission() { Id = 4521, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4505, Name = "List of Standard Group Targets", Description = "Allow User To View List of Standard Group Targets" });
                permissions.Add(new Permission() { Id = 4522, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4505, Name = "List of Optional Group Targets", Description = "Allow User To View List of Optional Group Targets" });

                permissions.Add(new Permission() { Id = 4523, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4500, Name = "Add Targets Without Approval", Description = "Allow User To Add Targets Without Approval" });
                permissions.Add(new Permission() { Id = 4524, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4500, Name = "Add Steps Without Approval", Description = "Allow User To Add Steps Without Approval" });
                permissions.Add(new Permission() { Id = 4525, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4500, Name = "Add Steps Without ReApproval", Description = "Allow User To Add Steps Without ReApproval" });

                permissions.Add(new Permission() { Id = 4526, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4500, Name = "View list of Pending for Approval Targets", Description = "Allow User To View list of Pending for Approval Targets" });
                permissions.Add(new Permission() { Id = 4527, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4500, Name = "View list of Pending for ReApproval Targets", Description = "Allow User To View list of Pending for ReApproval Targets" });

                permissions.Add(new Permission() { Id = 4528, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4500, Name = "View list of Pending for Approval Steps", Description = "Allow User To View list of Pending for Approval Steps" });
                permissions.Add(new Permission() { Id = 4529, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4500, Name = "View list of Pending for ReApproval Steps", Description = "Allow User To View list of Pending for ReApproval Steps" });
                permissions.Add(new Permission() { Id = 4530, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4500, Name = "View list of All Target Groups", Description = "Allow User To View list of All Target Groups" });

                permissions.Add(new Permission() { Id = 4531, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4500, Name = "Mark as Void Step", Description = "Allow User To Mark as Void Step" });
                permissions.Add(new Permission() { Id = 4532, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4500, Name = "UnMark Void Step", Description = "Allow User To UnMark as Void Step" });
                permissions.Add(new Permission() { Id = 4533, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4500, Name = "View list of Void Steps", Description = "Allow User To View list of Void Steps" });

                permissions.Add(new Permission() { Id = 4534, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4500, Name = "View Group Step Register", Description = "Allow User To View Group Step Register" });
                permissions.Add(new Permission() { Id = 4535, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4500, Name = "View Step Register", Description = "Allow User To View Step Register" });

                permissions.Add(new Permission() { Id = 4537, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4500, Name = "Delete Step", Description = "Allow User To Delete Step" });

                permissions.Add(new Permission() { Id = 4538, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4500, Name = "Delete Group", Description = "Allow User To Delete Groups" });
                permissions.Add(new Permission() { Id = 4539, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4500, Name = "By Pass Authority of Target Groups", Description = "Allow User To By Pass Authority of Target Groups" });

                permissions.Add(new Permission() { Id = 4540, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4500, Name = "Target Rewards", Description = "Target Rewards" });
                permissions.Add(new Permission() { Id = 4541, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4540, Name = "View List of Target Rewards", Description = "Allow User To View List of Target Rewards" });

                permissions.Add(new Permission() { Id = 4542, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4540, Name = "Sales Target Rewards", Description = "Sales Target Rewards" });
                permissions.Add(new Permission() { Id = 4543, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4542, Name = "Can Add Sales Target Rewards", Description = "Allow User To Add Sales Target Rewards" });
                permissions.Add(new Permission() { Id = 4544, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4542, Name = "Can View All Sales Target Rewards", Description = "Allow User To View Sales Target Rewards" });

                permissions.Add(new Permission() { Id = 4545, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4540, Name = "Finance Target Rewards", Description = "Finance Target Rewards" });
                permissions.Add(new Permission() { Id = 4546, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4545, Name = "Can Add Finance Target Rewards", Description = "Allow User To Add Finance Target Rewards" });
                permissions.Add(new Permission() { Id = 4547, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4545, Name = "Can View All Finance Target Rewards", Description = "Allow User To View Finance Target Rewards" });

                permissions.Add(new Permission() { Id = 4548, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4540, Name = "Other Target Rewards", Description = "Other Target Rewards" });
                permissions.Add(new Permission() { Id = 4549, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4548, Name = "Can Add Other Target Rewards", Description = "Allow User To Add Other Target Rewards" });
                permissions.Add(new Permission() { Id = 4550, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4548, Name = "Can View All Other Target Rewards", Description = "Allow User To View Other Target Rewards" });

                permissions.Add(new Permission() { Id = 4551, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4540, Name = "Attach a file with Target Reward", Description = "Allow User To Attach a file with Target Reward" });
                permissions.Add(new Permission() { Id = 4552, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4540, Name = "View list of attached files with Target Reward", Description = "Allow User To View list of attached files with Target Reward" });
                permissions.Add(new Permission() { Id = 4554, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4540, Name = "Mark as Void Target Reward", Description = "Allow User To Mark as Void Target Reward" });
                permissions.Add(new Permission() { Id = 4555, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4540, Name = "Unmark Void Target Reward", Description = "Allow User To unmark as Void Target Reward" });

                permissions.Add(new Permission() { Id = 4556, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4540, Name = "Close Target Reward without Approval", Description = "Allow User To Close Target Reward without Approval" });
                permissions.Add(new Permission() { Id = 4557, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4540, Name = "Edit Unapproved Target Reward", Description = "Allow User To Edit Unapproved Target Reward without Approval" });

                permissions.Add(new Permission() { Id = 4559, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4540, Name = "Reviewer Level 1 Target Reward", Description = "Allow User to mark Target Reward as Reviewed once" });
                permissions.Add(new Permission() { Id = 4560, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4540, Name = "Reviewer Level 2 Target Reward", Description = "Allow User To Mark Target Reward as Reviewed and move it to Approved List" });
                permissions.Add(new Permission() { Id = 4561, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4540, Name = "Approver for Closing Target Reward", Description = "Allow User To Close Target Reward which is in pending state" });
                permissions.Add(new Permission() { Id = 4562, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4540, Name = "Approver for new added Target Reward", Description = "Allow User To Approve Target Reward which is in pending state" });
                permissions.Add(new Permission() { Id = 4563, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4540, Name = "Edit closed Target Reward", Description = "Allow User To Edit Closed Target Reward" });
                permissions.Add(new Permission() { Id = 4564, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4540, Name = "View(Pending for closing) Target Reward List", Description = "Allow User To View List of All(Pending for Approval) Target Reward mapped to his Department" });
                permissions.Add(new Permission() { Id = 4565, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4540, Name = "View(Pending for Approval) Target Reward List", Description = "Allow User To View List of All(Pending for Approval) Target Reward mapped to his Department" });
                permissions.Add(new Permission() { Id = 4566, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4540, Name = "View Target Reward Register", Description = "Allow User To view Target Reward Register List" });
                permissions.Add(new Permission() { Id = 4568, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4540, Name = "Edit Creation Date of Target Reward", Description = "Allow User To Edit Creation Date of Target Reward" });

                permissions.Add(new Permission() { Id = 4571, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4540, Name = "ReApprover for new added Target Reward", Description = "Allow User To ReApprove Target Reward which is in pending state" });
                permissions.Add(new Permission() { Id = 4572, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4540, Name = "Add Target Reward without ReApproval", Description = "Allow User To Add New Target Reward without ReApproval" });
                permissions.Add(new Permission() { Id = 4573, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4540, Name = "View(Pending for ReApproval) Target Reward List", Description = "Allow User To View List of All(Pending for ReApproval) Target Reward mapped to his Department" });
                permissions.Add(new Permission() { Id = 4576, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4540, Name = "View list of Void Target Rewards", Description = "View list of Void Target Rewards" });
                permissions.Add(new Permission() { Id = 4577, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4540, Name = "Edit (Pending for closing) Target Reward", Description = "Allow User To Edit (Pending for Closing) Target Reward mapped to his Department" });

                permissions.Add(new Permission() { Id = 4578, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4540, Name = "Target Reward Status", Description = "Target Reward Status" });
                permissions.Add(new Permission() { Id = 4579, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4578, Name = "Add Target Reward Status", Description = "Allow User To Add Target Reward Status" });
                permissions.Add(new Permission() { Id = 4580, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4578, Name = "Edit Target Reward Status", Description = "Allow User To Edit Target Reward Status" });
                permissions.Add(new Permission() { Id = 4581, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4578, Name = "View InActive Statuses in Target Rewards", Description = "Allow User To View InActive Statuses in Target Rewards" });
                permissions.Add(new Permission() { Id = 4582, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4578, Name = "View List of Target Reward Statuses", Description = "Allow User To View List of Target Reward Statuses" });

                permissions.Add(new Permission() { Id = 4553, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4500, Name = "Export Target Reports", Description = "Allow User To Export Target Reports" });

                permissions.Add(new Permission() { Id = 4554, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4500, Name = "Over Rule Calculation Type", Description = "Allow User To Over Rule Calculation Type" });
                permissions.Add(new Permission() { Id = 4555, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4500, Name = "Over Rule Group Level", Description = "Allow User To Over Rule Group Level" });

                permissions.Add(new Permission() { Id = 4556, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4500, Name = "Save Layout of the Sale Order Grids", Description = "Allow User to Save Layout of the Sale Order Grids" });

                permissions.Add(new Permission() { Id = 4570, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4501, Name = "Target Reports", Description = "Target Reports" });





                permissions.Add(new Permission() { Id = 4583, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4500, Name = "Can Make a Copy of Targets", Description = "Allow User to Make a Copy of Targets" });
                permissions.Add(new Permission() { Id = 4584, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4500, Name = "Can Mark or UnMark Targets as Basketed", Description = "Allow User to Mark or UnMark Targets as Basketed" });

                permissions.Add(new Permission() { Id = 4585, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4500, Name = "View List of Basketed Targets", Description = "Allow User to View List of Basketed Targets" });
                permissions.Add(new Permission() { Id = 4586, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4500, Name = "View List of Basketed Steps", Description = "Allow User to View List of Basketed Steps" });
            }

            ///Payment Permissions
            ///
            {

                permissions.Add(new Permission() { Id = 4601, Added = System.DateTime.Now, LastModified = System.DateTime.Now, Name = "Petty Cash", Description = "Petty Cash" });
                permissions.Add(new Permission() { Id = 4602, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4601, Name = "Can view Petty Cash Enteries without Departmental Authority", Description = "Allow User to view Petty Cash Enteries without Departmental Authority" });
                permissions.Add(new Permission() { Id = 4603, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4601, Name = "Can Open Transactions without Authority from Petty Cash", Description = "Allow User to Open Transactions without Authority" });
                permissions.Add(new Permission() { Id = 4604, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4601, Name = "Can view Petty Cash Enteries without Company & Departmental Authority", Description = "Allow User to view Petty Cash Enteries without Company & Departmental Authority" });
            }
            permissions.Add(new Permission() { Id = 4701, Added = System.DateTime.Now, LastModified = System.DateTime.Now, Name = "VAT Book", Description = "VAT Book" });
            permissions.Add(new Permission() { Id = 4702, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4701, Name = "Can view VAT Book Enteries without Departmental Authority", Description = "Allow User to view VAT Book Enteries without Departmental Authority" });
            permissions.Add(new Permission() { Id = 4703, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4701, Name = "Can Open Transactions without Authority from VAT Book", Description = "Allow User to Open Transactions without Authority" });
            permissions.Add(new Permission() { Id = 4704, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4701, Name = "Can view VAT Book Enteries without Company & Departmental Authority", Description = "Allow User to view VAT Book Enteries without Company & Departmental Authority" });
            permissions.Add(new Permission() { Id = 4705, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4701, Name = "VAT Book Reference", Description = "Allow User to Access VAT Book Reference" });
            permissions.Add(new Permission() { Id = 4706, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4705, Name = "Add VAT Book Reference", Description = "Allow User To Add VAT Book Reference" });
            permissions.Add(new Permission() { Id = 4707, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4705, Name = "Edit VAT Book Reference", Description = "Allow User To Edit VAT Book Reference" });
            permissions.Add(new Permission() { Id = 4709, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4705, Name = "List of VAT Book Reference", Description = "Allow User To View List of VAT Book Reference" });


            permissions.Add(new Permission() { Id = 4800, Added = System.DateTime.Now, ParentId = 3600, LastModified = System.DateTime.Now, Name = "CashFlow", Description = "CashFlow" });
            permissions.Add(new Permission() { Id = 4801, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4800, Name = "Can view Cashflow statement", Description = "Allow User to view Cashflow statement" });
            permissions.Add(new Permission() { Id = 4802, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4800, Name = "Can view sales invoice from Cashflow statement", Description = "Allow User to view sales invoice from Cashflow statement" });
            permissions.Add(new Permission() { Id = 4803, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4800, Name = "Can view purchase orders from Cashflow statement", Description = "Allow User to view purchase orders from Cashflow statement" });
            permissions.Add(new Permission() { Id = 4804, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4800, Name = "Can view banks from Cashflow statement", Description = "Allow User to view banks from Cashflow statement" });
            permissions.Add(new Permission() { Id = 4805, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4800, Name = "Can view Cashflow Summaries", Description = "Allow User to view Cashflow Summaries" });
            permissions.Add(new Permission() { Id = 4806, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4800, Name = "Can view adminBills from Cashflow statement", Description = "Allow User to view adminBills from Cashflow statement" });
            permissions.Add(new Permission() { Id = 480, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4800, Name = "Export CashFlow Reports", Description = "Allow User to Export CashFlow Reports" });



            //    //Reports

            //    //{
            //    //    var reportObj = new Permission() { Added = System.DateTime.Now, LastModified = System.DateTime.Now, Name = "Reports", Description = "Allow User To View Reports Menu" };
            //    //    permissions.Add(reportObj);

            //    //    var reportCenterObj = new Permission() { Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentPermission = reportObj, Name = "Report Center", Description = "Allow User To create new reports in Reports Center " };
            //    //    permissions.Add(reportCenterObj);

            //    //    var memorizedReports = new Permission() { Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentPermission = reportObj, Name = "Memorized Reports", Description = "Allow User To Memorized Reports" };
            //    //    permissions.Add(memorizedReports);

            //    //    var memorizedReportObj = new Permission() { Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentPermission = memorizedReports, Name = "View Memorized Reports", Description = "Allow User To View Memorized Reports" };
            //    //    permissions.Add(memorizedReportObj);

            //    //    var standardReports= new Permission() { Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentPermission = reportObj, Name = "Standard Reports", Description = "Allow user to Standard Reports" };
            //    //    permissions.Add(standardReports);

            //    //    var standardReportObj = new Permission() { Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentPermission = standardReports, Name = "View Standard Reports", Description = "Allow user to View Standard Reports" };
            //    //    permissions.Add(standardReportObj);

            //    //    var reportGroupObj = new Permission() { Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentPermission = reportObj, Name = "Access to Report Group", Description = "Allow User To Access Report Group" };
            //    //    permissions.Add(reportGroupObj);

            //    //    var standardGroupObj = new Permission() { Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentPermission = reportGroupObj, Name = "Access to View Standard Groups", Description = "Allow User To View Standard Groups" };
            //    //    permissions.Add(standardGroupObj);

            //    //    var MemorizedGroupObj = new Permission() { Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentPermission = reportGroupObj, Name = "Access to View Memorized Groups", Description = "Allow User To View Memorized Groups" };
            //    //    permissions.Add(MemorizedGroupObj);

            //    //    // New Permissions

            //    //    permissions.Add(new Permission() { Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentPermission = reportObj, Name = "Export Inquiries Reports", Description = "Allow User To Export Inquiry report" });
            //    //    permissions.Add(new Permission() { Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentPermission = reportObj, Name = "Export Offers Reports", Description = "Allow User To Export Offers report" });
            //    //    permissions.Add(new Permission() { Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentPermission = reportObj, Name = "Export Sale Orders Reports", Description = "Allow User To Export Sale Orders report" });
            //    //    permissions.Add(new Permission() { Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentPermission = reportObj, Name = "Export Purchase Orders Reports", Description = "Allow User To Export Purchase Orders report" });
            //    //    permissions.Add(new Permission() { Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentPermission = reportObj, Name = "Export Bills Reports", Description = "Allow User To Export Bills report" });
            //    //    permissions.Add(new Permission() { Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentPermission = reportObj, Name = "Export Sale Invoices Reports", Description = "Allow User To Export Sale Invoices report" });


            //    //    //permissions.Add(new Permission() { Id = 80, Added = System.DateTime.Now, LastModified = System.DateTime.Now, Name = "Reports", Description = "Allow User To View Reports Menu" });

            //    //    //View Memorized Reports
            //    //    //permissions.Add(new Permission() { Id = 81, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 80, Name = "View Memorized Reports", Description = "Allow User To View Memorized Reports" });
            //    //    permissions.Add(new Permission() { Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentPermission = memorizedReportObj, Name = "Save as new Memorized Report", Description = "Allow user to Save as new after view report" });
            //    //    permissions.Add(new Permission() { Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentPermission = memorizedReportObj, Name = "Access to Rename Memorized Report", Description = "Allow User To Rename Memorized Report " });
            //    //    permissions.Add(new Permission() { Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentPermission = memorizedReportObj, Name = "Access to Update Memorized Report", Description = "Allow User to Update Memorized Report " });
            //    //    permissions.Add(new Permission() { Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentPermission = memorizedReportObj, Name = "Access to Delete Memorized Report", Description = "Allow User to Delete Memorized Report " });
            //    //    permissions.Add(new Permission() { Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentPermission = memorizedReportObj, Name = "Access to Export to Standard Report", Description = "Allow User to Export to Standard Report " });


            //    //    //permissions.Add(new Permission() { Id = 82, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 80, Name = "Report Center", Description = "Allow User To create new reports in Reports Center " });

            //    //    //View Standard Reports
            //    //    //permissions.Add(new Permission() { Id = 83, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 80, Name = "View Standard Reports", Description = "Allow user to View Standard Reports" });
            //    //    permissions.Add(new Permission() { Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentPermission = standardReportObj, Name = "Save as new Standard Report", Description = "Allow user to Save as new after view report" });
            //    //    permissions.Add(new Permission() { Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentPermission = standardReportObj, Name = "Access to Rename Standard Report", Description = "Allow User To Rename Standard Report " });
            //    //    permissions.Add(new Permission() { Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentPermission = standardReportObj, Name = "Access to Update Standard Report", Description = "Allow User to Update Standard Report " });
            //    //    permissions.Add(new Permission() { Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentPermission = standardReportObj, Name = "Access to Delete Standard Report", Description = "Allow User to Delete Standard Report " });
            //    //    permissions.Add(new Permission() { Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentPermission = standardReportObj, Name = "Access to Export to Memorized Report", Description = "Allow User to Export to Memorized Report" });

            //    //    //permissions.Add(new Permission() { Id = 84, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 80, Name = "Access to Report Group", Description = "Allow User To Access Report Group" });


            //    //    // Standard Groups
            //    //    //permissions.Add(new Permission() { Id = 85, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 80, Name = "Access to View Standard Groups", Description = "Allow User To View Standard Groups" });
            //    //    permissions.Add(new Permission() { Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentPermission = standardGroupObj, Name = "Access to Add new Standard Group", Description = "Allow User To Add new Standard Group" });
            //    //    permissions.Add(new Permission() { Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentPermission = standardGroupObj, Name = "Access to Edit Standard Group", Description = "Allow User To Edit Standard Group" });
            //    //    //Memorized Groups
            //    //    //permissions.Add(new Permission() { Id = 86, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 80, Name = "Access to View Memorized Groups", Description = "Allow User To View Memorized Groups" });
            //    //    permissions.Add(new Permission() { Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentPermission = MemorizedGroupObj, Name = "Access to Add new Memorized Group", Description = "Allow User To Add new Memorized Group" });
            //    //    permissions.Add(new Permission() { Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentPermission = MemorizedGroupObj, Name = "Access to Edit Memorized Groups", Description = "Allow User To Add new Memorized Group" });
            //    //}

            {

                //Lists
                permissions.Add(new Permission() { Id = 90, Added = System.DateTime.Now, LastModified = System.DateTime.Now, Name = "Lists", Description = "Allow User To View Lists" });
                permissions.Add(new Permission() { Id = 91, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 90, Name = "List of Currencies", Description = "Allow User To View List of Currencies" });
                permissions.Add(new Permission() { Id = 92, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 90, Name = "List of Items", Description = "Allow User To View List of Items" });
                permissions.Add(new Permission() { Id = 93, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 90, Name = "List of Industry Types", Description = "Allow User To View List of Industry Types" });
                permissions.Add(new Permission() { Id = 94, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 90, Name = "List of Payment Terms", Description = "Allow User To View List of Payment Terms" });
                permissions.Add(new Permission() { Id = 95, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 90, Name = "List of Incoterms", Description = "Allow User To View List of Incoterms" });
                permissions.Add(new Permission() { Id = 96, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 90, Name = "List of Procurment Statuses ", Description = "Allow User To View List of Procurment Statuses" });
                permissions.Add(new Permission() { Id = 97, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 90, Name = "List of Inquiry Statuses", Description = "Allow User To View List of Inquiry Statuses" });
                permissions.Add(new Permission() { Id = 98, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 90, Name = "List of Offer Statuses", Description = "Allow User To View List of Offer Statuses" });
                permissions.Add(new Permission() { Id = 99, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 90, Name = "List of Purchase Order Statuses", Description = "Allow User To View List of Purchase Order Statuses" });
                permissions.Add(new Permission() { Id = 100, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 92, Name = "Edit Items", Description = "Allow User To Edit List of Items" });
                permissions.Add(new Permission() { Id = 101, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 92, Name = "Add New Items", Description = "Allow User To Add new List of Items" });
                permissions.Add(new Permission() { Id = 102, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 90, Name = "View PQ Documents", Description = "Allow User To View PQ Documents" });
                permissions.Add(new Permission() { Id = 103, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 90, Name = "Add PQ Documents", Description = "Allow User To Add PQ Documents" });
                permissions.Add(new Permission() { Id = 104, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 90, Name = "Edit PQ Documents", Description = "Allow User To Edit PQ Documents" });
                permissions.Add(new Permission() { Id = 105, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 90, Name = "Edit Shipping Terms", Description = "Allow User To Edit Shipping Terms" });
                permissions.Add(new Permission() { Id = 106, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 90, Name = "Add Shipping Terms", Description = "Allow User To Add Shipping Terms" });
                permissions.Add(new Permission() { Id = 107, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 90, Name = "Bypass Item Authorities", Description = "Allow User To Bypass Item Authorities" });

            }
            //users
            {
                permissions.Add(new Permission() { Id = 110, Added = System.DateTime.Now, LastModified = System.DateTime.Now, Name = "Users", Description = "Allow User To view Users List" });

                permissions.Add(new Permission() { Id = 111, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 110, Name = "Manage Users And Roles", Description = "Allow User To Manage Users And Roles" });
                permissions.Add(new Permission() { Id = 112, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 110, Name = "Add New User", Description = "Allow User To Add New User" });
                permissions.Add(new Permission() { Id = 113, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 110, Name = "Add New Role", Description = "Allow User To Add New Role" });
                permissions.Add(new Permission() { Id = 114, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 110, Name = "Edit Role", Description = "Allow User To Edit Role" });
                permissions.Add(new Permission() { Id = 115, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 110, Name = "Edit User", Description = "Allow User To Edit User" });
                //permissions.Add(new Permission() { Id = 116, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 110, Name = "Mark Role as InActive", Description = "Allow User To Mark Role as InActive." });
            }
            permissions.Add(new Permission() { Id = 115, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 90, Name = "View Claim/Discount Lists", Description = "Allow User To View Claim/Discount Lists" });
            permissions.Add(new Permission() { Id = 117, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 90, Name = "Edit Claim/Discount", Description = "Allow User To Edit Claim/Discount" });
            permissions.Add(new Permission() { Id = 118, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 90, Name = "Add Claim/Discount", Description = "Allow User To Add Claim/Discount" });
            permissions.Add(new Permission() { Id = 119, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 90, Name = "View FOC/Sampling Lists", Description = "Allow User To View FOC/Sampling Lists" });
            permissions.Add(new Permission() { Id = 120, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 90, Name = "Edit FOC/Sampling", Description = "Allow User To Edit FOC/Sampling" });
            permissions.Add(new Permission() { Id = 121, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 90, Name = "Add FOC/Sampling", Description = "Allow User To Add FOC/Sampling" });
            permissions.Add(new Permission() { Id = 122, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 90, Name = "View PassOn Lists", Description = "Allow User To View PassOn Lists" });
            permissions.Add(new Permission() { Id = 123, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 90, Name = "Edit PassOn", Description = "Allow User To Edit PassOn" });
            permissions.Add(new Permission() { Id = 124, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 90, Name = "Add PassOn", Description = "Allow User To Add PassOn" });
            permissions.Add(new Permission() { Id = 125, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 301, Name = "Add Unique Number", Description = "Allow User Add Unique Number" });
            permissions.Add(new Permission() { Id = 126, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 301, Name = "Edit Unique Number", Description = "Allow User Edit Unique Number" });
            permissions.Add(new Permission() { Id = 127, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 301, Name = "View Unique Number Lists", Description = "Allow User View Unique Number Lists" });

            //Currencies
            {
                permissions.Add(new Permission() { Id = 400, Added = System.DateTime.Now, LastModified = System.DateTime.Now, Name = "Currencies", Description = "Allow User To Access Currencies" });

                permissions.Add(new Permission() { Id = 401, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 400, Name = "Add Currency", Description = "Allow User To add Currency" });
                permissions.Add(new Permission() { Id = 402, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 400, Name = "Edit Currency", Description = "Allow User To add Currency" });
                //permissions.Add(new Permission() { Id = 403, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 400, Name = "Add Exchange Rates", Description = "Allow User To add Market Exchange Rates" });
                //permissions.Add(new Permission() { Id = 404, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 400, Name = "Edit Exchange Rates", Description = "Allow User To Edit Market Exchange Rates" });
                permissions.Add(new Permission() { Id = 403, Added = System.DateTime.Now, ParentId = 400, LastModified = System.DateTime.Now, Name = "Add Exchange Rates", Description = "Allow User to Add Exchange Rates" });
                permissions.Add(new Permission() { Id = 404, Added = System.DateTime.Now, ParentId = 400, LastModified = System.DateTime.Now, Name = "Edit Exchange Rates", Description = "Allow User to Edit Exchange Rates" });
                permissions.Add(new Permission() { Id = 405, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 400, Name = "Add Sales Exchange Rates", Description = "Allow User To Add Sales Exchange Rates" });
                permissions.Add(new Permission() { Id = 406, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 400, Name = "Edit Sales Exchange Rates", Description = "Allow User To Edit Sales Exchange Rates" });
                permissions.Add(new Permission() { Id = 407, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 400, Name = "List market Exchange Rates", Description = "Allow User To List All Market Exchange Rates" });
                permissions.Add(new Permission() { Id = 408, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 400, Name = "List Sales Exchange Rates", Description = "Allow User To List All Sales Exchange Rates" });
                permissions.Add(new Permission() { Id = 409, Added = System.DateTime.Now, ParentId = 400, LastModified = System.DateTime.Now, Name = "View Exchange Rates", Description = "Allow User to View Exchange Rates" });
                permissions.Add(new Permission() { Id = 410, Added = System.DateTime.Now, ParentId = 404, LastModified = System.DateTime.Now, Name = "Edit Exchange Rates Base Currency", Description = "Allow User to Edit Exchange Rates Base Currency" });
                permissions.Add(new Permission() { Id = 411, Added = System.DateTime.Now, ParentId = 404, LastModified = System.DateTime.Now, Name = "Edit Exchange Rates Transaction Currency", Description = "Allow User to Edit Exchange Rates Transaction Currency" });
                permissions.Add(new Permission() { Id = 412, Added = System.DateTime.Now, ParentId = 404, LastModified = System.DateTime.Now, Name = "Edit Exchange Rates Target Year", Description = "Allow User to Edit Exchange Rates Target Year" });
                permissions.Add(new Permission() { Id = 413, Added = System.DateTime.Now, ParentId = 404, LastModified = System.DateTime.Now, Name = "Edit Exchange Rates Type", Description = "Allow User to Edit Exchange Rates Type" });



            }

            permissions.Add(new Permission() { Id = 8000, Added = System.DateTime.Now, LastModified = System.DateTime.Now, Name = "Inventory", Description = "View Inventory" });
            {
                permissions.Add(new Permission() { Id = 8001, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8000, Name = "Inventory Adjustment", Description = "Inventory Adjustments" });
                permissions.Add(new Permission() { Id = 8002, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8001, Name = "Add Inventory Adjustment", Description = "Allow User To Add Inventory Adjustment" });
                permissions.Add(new Permission() { Id = 8003, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8001, Name = "Edit Inventory Adjustment", Description = "Allow User To Edit Inventory Adjustment" });
                permissions.Add(new Permission() { Id = 8004, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8001, Name = "View Inventory Adjustment", Description = "Allow User To View Inventory Adjustment" });
                permissions.Add(new Permission() { Id = 8005, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8001, Name = "List of Inventory Adjustments", Description = "Allow User To View List of Inventory Adjustment" });
                permissions.Add(new Permission() { Id = 8006, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8001, Name = "Add Inventory Adjustment Status", Description = "Allow User To Add new status for Inventory Adjustment" });
                permissions.Add(new Permission() { Id = 8007, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8001, Name = "Add Inventory Adjustment without Approval", Description = "Allow User To Add New Inventory Adjustment without Approval" });
                permissions.Add(new Permission() { Id = 8008, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8001, Name = "View InActive Inventory Adjustments", Description = "Allow User To View InActive Inventory Adjustments" });
                permissions.Add(new Permission() { Id = 8009, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8001, Name = "Close Inventory Adjustment", Description = "Allow User To Close Inventory Adjustment" });
                permissions.Add(new Permission() { Id = 8010, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8001, Name = "Edit Inventory Adjustment Status", Description = "Allow User To Edit Inventory Adjustment Status" });
                permissions.Add(new Permission() { Id = 8011, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8001, Name = "View InActive Inventory Adjustment Statuses", Description = "Allow User To View InActive Inventory Adjustment Statuses" });

                permissions.Add(new Permission() { Id = 8012, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8001, Name = "Close Inventory Adjustment without Approval", Description = "Allow User To Close Inventory Adjustment without Approval" });
                permissions.Add(new Permission() { Id = 8013, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8001, Name = "Edit Unapproved Inventory Adjustment", Description = "Allow User To Edit Unapproved Inventory Adjustment without Approval" });
                permissions.Add(new Permission() { Id = 8014, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8001, Name = "View Market Exchange Rate in Inventory Adjustment", Description = "Allow User To View Market Exchange Rate in Inventory Adjustment" });

                permissions.Add(new Permission() { Id = 8015, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8001, Name = "Reviewer Level 1 Inventory Adjustment", Description = "Allow User to mark Inventory Adjustment as Reviewed once" });
                permissions.Add(new Permission() { Id = 8016, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8001, Name = "Reviewer Level 2 Inventory Adjustment", Description = "Allow User To Mark Inventory Adjustment as Reviewed and move it to Approved List" });
                permissions.Add(new Permission() { Id = 8017, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8001, Name = "Approver for Closing Inventory Adjustment", Description = "Allow User To Close Inventory Adjustment which is in pending state" });
                permissions.Add(new Permission() { Id = 8018, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8001, Name = "Approver for new added Inventory Adjustment", Description = "Allow User To Approve Inventory Adjustment which is in pending state" });
                permissions.Add(new Permission() { Id = 8019, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8001, Name = "Edit closed Inventory Adjustment", Description = "Allow User To Edit Closed Inventory Adjustment" });
                permissions.Add(new Permission() { Id = 8020, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8001, Name = "View(Pending for closing) Inventory Adjustment List", Description = "Allow User To View List of All(Pending for Closing) Inventory Adjustment mapped to his Department" });
                permissions.Add(new Permission() { Id = 8021, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8001, Name = "View(Pending for Approval) Inventory Adjustment List", Description = "Allow User To View List of All(Pending for Approval) Inventory Adjustment mapped to his Department" });
                permissions.Add(new Permission() { Id = 8022, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8001, Name = "Edit Creation Date of Inventory Adjustment", Description = "Allow User To Edit Creation Date of Inventory Adjustment" });
                permissions.Add(new Permission() { Id = 8023, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8001, Name = "Attach a file with Inventory Adjustment", Description = "Allow User To Attach a file with Inventory Adjustment" });
                permissions.Add(new Permission() { Id = 8024, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8001, Name = "View list of attached files with Inventory Adjustment", Description = "Allow User To View list of attached files with Inventory Adjustment" });
                permissions.Add(new Permission() { Id = 8025, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8001, Name = "Mark as Void Inventory Adjustment", Description = "Allow User To Mark as Void Inventory Adjustment" });
                permissions.Add(new Permission() { Id = 8026, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8001, Name = "Unmark Void Inventory Adjustment", Description = "Allow User To unmark as Void Inventory Adjustment" });
                permissions.Add(new Permission() { Id = 8027, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8001, Name = "View list of Void Inventory Adjustments", Description = "View list of Void SaleInvoices" });
                permissions.Add(new Permission() { Id = 8028, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8001, Name = "View Inventory Adjustment Register", Description = "Allow User To View Inventory Adjustment Register" });
                permissions.Add(new Permission() { Id = 8029, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8001, Name = "Export Inventory Adjustments Reports", Description = "Allow User To Export Inventory Adjustments Reports" });
                permissions.Add(new Permission() { Id = 8030, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8001, Name = "Mark as Void Inventory Adjustments", Description = "Allow User To Mark as Void Inventory Adjustments" });
                permissions.Add(new Permission() { Id = 8031, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8001, Name = "Unmark Void Inventory Adjustments", Description = "Allow User To Unmark Void Inventory Adjustments" });
                //permissions.Add(new Permission() { Id = 4033, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 701, Name = "Edit GL Posting Date of Inventory Adjustment", Description = "Allow User To Edit GL Posting Date of Inventory Adjustment" });

            }






            //Fixed Asset Adjustment (ID = 1600 and onwards)
            {
                //Open Fixed Asset Adjustment

                permissions.Add(new Permission() { Id = 1600, Added = System.DateTime.Now, LastModified = DateTime.Now, Name = "Fixed Assets Adjustment", Description = "Allow User To access Fixed Assets adjustment sub menu" });


            }
            //Banking Permission
            permissions.Add(new Permission() { Id = 2600, Added = System.DateTime.Now, LastModified = System.DateTime.Now, Name = "Banking", Description = "Banking" });



            //Bank Lists (ID = 1700 and onwards)
            {

                ////Old

                //Open Bank lists Sub-menu

                permissions.Add(new Permission() { Id = 1700, Added = System.DateTime.Now, LastModified = DateTime.Now, ParentId = 2600, Name = "Bank Lists", Description = "Allow User To access Bank lists sub menu" });

                //View Bank list, Account List and Collection Method List

                permissions.Add(new Permission() { Id = 1705, Added = System.DateTime.Now, LastModified = DateTime.Now, ParentId = 1700, Name = "View List of Banks", Description = "Allow User To View Banks List" });
                permissions.Add(new Permission() { Id = 1710, Added = System.DateTime.Now, LastModified = DateTime.Now, ParentId = 1700, Name = "View List of Accounts", Description = "Allow User To View Accounts List" });
                permissions.Add(new Permission() { Id = 1711, Added = System.DateTime.Now, LastModified = DateTime.Now, ParentId = 1710, Name = "View List of All Accounts", Description = "Allow User To View All Accounts List" });
                permissions.Add(new Permission() { Id = 1712, Added = System.DateTime.Now, LastModified = DateTime.Now, ParentId = 1710, Name = "View List of Company Accounts", Description = "Allow User To View Company Accounts List" });
                permissions.Add(new Permission() { Id = 1713, Added = System.DateTime.Now, LastModified = DateTime.Now, ParentId = 1710, Name = "View List of Vendor Accounts", Description = "Allow User To View Vendor Accounts List" });
                permissions.Add(new Permission() { Id = 1714, Added = System.DateTime.Now, LastModified = DateTime.Now, ParentId = 1710, Name = "View List of Personal Accounts", Description = "Allow User To View Personal Accounts List" });

                permissions.Add(new Permission() { Id = 1715, Added = System.DateTime.Now, LastModified = DateTime.Now, ParentId = 1700, Name = "View List of Collection Methods", Description = "Allow User To View Collection Methods List" });
                permissions.Add(new Permission() { Id = 1716, Added = System.DateTime.Now, LastModified = DateTime.Now, ParentId = 1700, Name = "View List of Deductions", Description = "Allow User To View Deductions List" });

                permissions.Add(new Permission() { Id = 1717, Added = System.DateTime.Now, LastModified = DateTime.Now, ParentId = 1710, Name = "View List of Customer Accounts", Description = "Allow User To View Customer Accounts List" });


                //Add/Update Banks and Contact Person
                permissions.Add(new Permission() { Id = 1720, Added = System.DateTime.Now, LastModified = DateTime.Now, ParentId = 1705, Name = "Add New Banks", Description = "Allow User To Add New Banks" });
                permissions.Add(new Permission() { Id = 1725, Added = System.DateTime.Now, LastModified = DateTime.Now, ParentId = 1705, Name = "Update Existing Bank", Description = "Allow User To Update Existing" });
                permissions.Add(new Permission() { Id = 1730, Added = System.DateTime.Now, LastModified = DateTime.Now, ParentId = 1705, Name = "Add New Contact Person", Description = "Allow User To Add New Contact Person" });
                permissions.Add(new Permission() { Id = 1735, Added = System.DateTime.Now, LastModified = DateTime.Now, ParentId = 1705, Name = "Update Existing Contact Person", Description = "Allow User To Update Existing Contact Person" });

                //Add and Update Account
                permissions.Add(new Permission() { Id = 1740, Added = System.DateTime.Now, LastModified = DateTime.Now, ParentId = 1710, Name = "Add New Account", Description = "Allow User To Add New Collection Method" });
                permissions.Add(new Permission() { Id = 1745, Added = System.DateTime.Now, LastModified = DateTime.Now, ParentId = 1710, Name = "Update Existing Account", Description = "Allow User To Update Existing Collection Method" });

                //Add and Update Collection method

                permissions.Add(new Permission() { Id = 1750, Added = System.DateTime.Now, LastModified = DateTime.Now, ParentId = 1715, Name = "Add New Collection Method", Description = "Allow User To Add New Collection Method" });
                permissions.Add(new Permission() { Id = 1755, Added = System.DateTime.Now, LastModified = DateTime.Now, ParentId = 1715, Name = "Update Existing Collection Method", Description = "Allow User To Update Existing Collection Method" });

                //Add and Update Deductions
                permissions.Add(new Permission() { Id = 1760, Added = System.DateTime.Now, LastModified = DateTime.Now, ParentId = 1716, Name = "Add New Deduction", Description = "Allow User To Add New Deduction" });
                permissions.Add(new Permission() { Id = 1765, Added = System.DateTime.Now, LastModified = DateTime.Now, ParentId = 1716, Name = "Update Existing Deduction", Description = "Allow User To Update Existing Deduction" });


            }




            ///Sale Receipts Permissions
            ///
            {
                permissions.Add(new Permission() { Id = 2601, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2600, Name = "Sale Receipts", Description = "Sale Receipts" });
                permissions.Add(new Permission() { Id = 2602, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2601, Name = "Add Sale Receipt", Description = "Allow User To Add Sale Receipt" });//Done
                permissions.Add(new Permission() { Id = 2603, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2601, Name = "Edit Sale Receipt", Description = "Allow User To Edit Sale Receipt" });//Done
                permissions.Add(new Permission() { Id = 2604, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2601, Name = "View Sale Receipt", Description = "Allow User To View Sale Receipt" });//Done
                permissions.Add(new Permission() { Id = 2605, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2601, Name = "List of Sale Receipts", Description = "Allow User To View List of Sale Receipts" });//Done
                permissions.Add(new Permission() { Id = 2606, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2601, Name = "Add Sale Receipt Status", Description = "Allow User To Add new status for Sale Receipt" });//Done
                permissions.Add(new Permission() { Id = 2607, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2601, Name = "Add Sale Receipt without Approval", Description = "Allow User To Add New Sale Receipt without Approval" });
                permissions.Add(new Permission() { Id = 2608, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2601, Name = "View InActive Sale Receipts", Description = "Allow User To View InActive Sale Receipts" });
                permissions.Add(new Permission() { Id = 2609, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2601, Name = "Close Sale Receipt", Description = "Allow User To Close Sale Receipt" });
                permissions.Add(new Permission() { Id = 2610, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2601, Name = "Edit Sale Receipt Status", Description = "Allow User To Edit Sale Receipt Status" });//Done
                permissions.Add(new Permission() { Id = 2611, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2601, Name = "View InActive Sale Receipt Statuses", Description = "Allow User To View InActive Sale Receipt Statuses" });

                permissions.Add(new Permission() { Id = 2613, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2601, Name = "Close Sale Receipt without Approval", Description = "Allow User To Close Sale Receipt without Approval" });
                permissions.Add(new Permission() { Id = 2614, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2601, Name = "Edit Unapproved Sale Receipt", Description = "Allow User To Edit Unapproved Sale Receipt without Approval" });

                permissions.Add(new Permission() { Id = 2616, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2601, Name = "Reviewer Level 1 SaleReceipt", Description = "Allow User to mark SaleReceipt as Reviewed once" });
                permissions.Add(new Permission() { Id = 2617, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2601, Name = "Reviewer Level 2 SaleReceipt", Description = "Allow User To Mark SaleReceipt as Reviewed and move it to Approved List" });
                permissions.Add(new Permission() { Id = 2618, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2601, Name = "Approver for Closing SaleReceipt", Description = "Allow User To Close SaleReceipt which is in pending state" });
                permissions.Add(new Permission() { Id = 2619, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2601, Name = "Approver for new added SaleReceipt", Description = "Allow User To Approve Offer which is in pending state" });
                permissions.Add(new Permission() { Id = 2620, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2601, Name = "Edit closed SaleReceipt", Description = "Allow User To Edit Closed Sale Receipts" });
                permissions.Add(new Permission() { Id = 2621, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2601, Name = "View(Pending for closing) SaleReceipt List", Description = "Allow User To View List of All(Pending for Closing) SaleReceipts mapped to his Department" });
                permissions.Add(new Permission() { Id = 2622, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2601, Name = "View(Pending for Approval) SaleReceipt List", Description = "Allow User To View List of All(Pending for Approval) SaleReceipts mapped to his Department" });
                permissions.Add(new Permission() { Id = 2623, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2601, Name = "Edit Creation Date of SaleReceipt", Description = "Allow User To Edit Creation Date of SaleReceipt" });
                permissions.Add(new Permission() { Id = 2624, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2601, Name = "Attach a file with SaleReceipt", Description = "Allow User To Attach a file with SaleReceipt" });
                permissions.Add(new Permission() { Id = 2625, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2601, Name = "View list of attached files with SaleReceipt", Description = "Allow User To View list of attached files with SaleReceipt" });
                permissions.Add(new Permission() { Id = 2626, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2601, Name = "Mark as Void SaleReceipt", Description = "Allow User To Mark as Void SaleReceipt" });
                permissions.Add(new Permission() { Id = 2627, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2601, Name = "Unmark Void SaleReceipt", Description = "Allow User To unmark as Void SaleReceipt" });
                permissions.Add(new Permission() { Id = 2628, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2601, Name = "View list of Void SaleReceipts", Description = "View list of Void SaleReceipts" });

                //New Permissions
                permissions.Add(new Permission() { Id = 2629, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2601, Name = "ReApprover for new added SaleReceipt", Description = "Allow User To ReApprove SaleReceipt which is in pending state" });
                permissions.Add(new Permission() { Id = 2630, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2601, Name = "Add SaleReceipt without ReApproval", Description = "Allow User To Add New SaleReceipt without ReApproval" });
                permissions.Add(new Permission() { Id = 2631, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2601, Name = "View(Pending for ReApproval) SaleReceipts List", Description = "Allow User To View List of All(Pending for ReApproval) SaleReceipt mapped to his Department" });

                permissions.Add(new Permission() { Id = 2632, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2601, Name = "Edit Collection Amount value in SaleReceipt After Approval", Description = "Allow User To Edit Collection Amount value in SaleReceipt After Approval" });
                permissions.Add(new Permission() { Id = 2633, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2601, Name = "Update System Cost from Sales Receipt", Description = "Allow User To Update CostSheet from Sales Receipt" });
                permissions.Add(new Permission() { Id = 2634, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2601, Name = "View Cost Center from Sales Receipt", Description = "Allow User To View Cost Center from Sales Receipt" });
                permissions.Add(new Permission() { Id = 2635, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2601, Name = "Edit GL Posting Date of SaleReceipt", Description = "Allow User To Edit GL Posting Date of SaleReceipt" });
                permissions.Add(new Permission() { Id = 2636, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2601, Name = "Bypass SaleReceipt Bank Account", Description = "Allow User To Bypass SaleReceipt Bank Account" });

                permissions.Add(new Permission() { Id = 2637, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2601, Name = "Loans and Advances Sale Receipts", Description = "Allow User To Add Sale Receipts" });
                permissions.Add(new Permission() { Id = 2638, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2637, Name = "Add Loans and Advances Sale Receipts", Description = "Allow User To Add Loans and Advances Sale Receipts" });
                permissions.Add(new Permission() { Id = 2639, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2637, Name = "Edit Loans and Advances Sale Receipts", Description = "Allow User To Edit Loans and Advances Sale Receipts" });
                permissions.Add(new Permission() { Id = 2640, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2637, Name = "View Loans and Advances Sale Receipts", Description = "Allow User To View Loans and Advances Sale Receipts" });
                permissions.Add(new Permission() { Id = 2641, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2601, Name = "Edit Collection Amount value in SaleReceipt Under Approval", Description = "Allow User To Edit Collection Amount value in SaleReceipt Under Approval" });

                permissions.Add(new Permission() { Id = 2642, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2601, Name = "Direct Receipts", Description = "Allow User To Add Direct Receipts" });
                permissions.Add(new Permission() { Id = 2643, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2642, Name = "Add Direct Receipts", Description = "Allow User To Add Direct Receipts" });
                permissions.Add(new Permission() { Id = 2644, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2642, Name = "Edit Direct Receipts", Description = "Allow User To Edit Direct Receipts" });
                permissions.Add(new Permission() { Id = 2645, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2642, Name = "View Direct Receipts", Description = "Allow User To View Direct Receipts" });

                permissions.Add(new Permission() { Id = 2646, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2601, Name = "Customer Credit Receipts", Description = "Allow User To Add Customer Credit Receipts" });
                permissions.Add(new Permission() { Id = 2647, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2646, Name = "Add Customer Credit Receipts", Description = "Allow User To Add Customer Credit Receipts" });
                permissions.Add(new Permission() { Id = 2648, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2646, Name = "Edit Customer Credit Receipts", Description = "Allow User To Edit Customer Credit Receipts" });
                permissions.Add(new Permission() { Id = 2649, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2646, Name = "View Customer Credit Receipts", Description = "Allow User To View Customer Credit Receipts" });

                permissions.Add(new Permission() { Id = 2650, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2601, Name = "Create Direct Receipt from Payments", Description = "Allow User To Create Direct Receipt from Payments" });
                permissions.Add(new Permission() { Id = 2651, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2601, Name = "Create Direct Receipt from Receipts", Description = "Allow User To Create Direct Receipt from Receipts" });

                permissions.Add(new Permission() { Id = 2652, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2601, Name = "Rental Receipts", Description = "Allow User To Add Rental Receipts" });
                permissions.Add(new Permission() { Id = 2653, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2652, Name = "Add Rental Receipts", Description = "Allow User To Add Rental Receipts" });
                permissions.Add(new Permission() { Id = 2654, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2652, Name = "Edit Rental Receipts", Description = "Allow User To Edit Rental Receipts" });
                permissions.Add(new Permission() { Id = 2655, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2652, Name = "View Rental Receipts", Description = "Allow User To View Rental Receipts" });
            }

            ///Inter-Bank Transfer Permissions
            ///Inter-Bank Transfer Permissions
            ///
            {
                permissions.Add(new Permission() { Id = 2801, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2600, Name = "Inter-Bank Transfer", Description = "Inter-Bank Transfer" });
                permissions.Add(new Permission() { Id = 2801, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2600, Name = "Inter-Bank Transfer", Description = "Inter-Bank Transfer" });
                permissions.Add(new Permission() { Id = 2802, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2801, Name = "Add Inter-Bank Transfer", Description = "Allow User To Add Inter-Bank Transfer" });
                permissions.Add(new Permission() { Id = 2803, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2801, Name = "Edit Inter-Bank Transfer", Description = "Allow User To Edit Inter-Bank Transfer" });
                permissions.Add(new Permission() { Id = 2804, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2801, Name = "View Inter-Bank Transfer", Description = "Allow User To View Inter-Bank Transfer" });
                permissions.Add(new Permission() { Id = 2805, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2801, Name = "List of Inter-Bank Transfer", Description = "Allow User To View List of Inter-Bank Transfer" });
                permissions.Add(new Permission() { Id = 2806, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2801, Name = "Add Inter-Bank Transfer Status", Description = "Allow User To Add new status for Inter-Bank Transfer" });
                permissions.Add(new Permission() { Id = 2807, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2801, Name = "Add Inter-Bank Transfer without Approval", Description = "Allow User To Add New Inter-Bank Transfer without Approval" });
                permissions.Add(new Permission() { Id = 2808, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2801, Name = "View InActive Inter-Bank Transfer", Description = "Allow User To View InActive Inter-Bank Transfer" });
                permissions.Add(new Permission() { Id = 2809, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2801, Name = "Close Inter-Bank Transfer", Description = "Allow User To Close Inter-Bank Transfer" });
                permissions.Add(new Permission() { Id = 2810, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2801, Name = "Edit Inter-Bank Transfer Status", Description = "Allow User To Edit Inter-Bank Transfer Status" });
                permissions.Add(new Permission() { Id = 2811, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2801, Name = "View InActive Inter-Bank Transfer Statuses", Description = "Allow User To View InActive Inter-Bank Transfer Statuses" });

                permissions.Add(new Permission() { Id = 2813, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2801, Name = "Close Inter-Bank Transfer without Approval", Description = "Allow User To Close Inter-Bank Transfer without Approval" });
                permissions.Add(new Permission() { Id = 2814, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2801, Name = "Edit Unapproved Inter-Bank Transfer", Description = "Allow User To Edit Unapproved Inter-Bank Transfer without Approval" });

                permissions.Add(new Permission() { Id = 2816, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2801, Name = "Reviewer Level 1 Inter-Bank Transfer", Description = "Allow User to mark Inter-Bank Transfer as Reviewed once" });
                permissions.Add(new Permission() { Id = 2817, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2801, Name = "Reviewer Level 2 Inter-Bank Transfer", Description = "Allow User To Mark Inter-Bank Transfer as Reviewed and move it to Approved List" });
                permissions.Add(new Permission() { Id = 2818, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2801, Name = "Approver for Closing Inter-Bank Transfer", Description = "Allow User To Close Inter-Bank Transfer which is in pending state" });
                permissions.Add(new Permission() { Id = 2819, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2801, Name = "Approver for new added Inter-Bank Transfer", Description = "Allow User To Approve Inter-Bank Transfer which is in pending state" });
                permissions.Add(new Permission() { Id = 2820, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2801, Name = "Edit closed Inter-Bank Transfer", Description = "Allow User To Edit Closed Inter-Bank Transfer" });
                permissions.Add(new Permission() { Id = 2821, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2801, Name = "View(Pending for closing) Inter-Bank Transfer List", Description = "Allow User To View List of All(Pending for Closing) Inter-Bank Transfer mapped to his Department" });
                permissions.Add(new Permission() { Id = 2822, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2801, Name = "View(Pending for Approval) Inter-Bank Transfer List", Description = "Allow User To View List of All(Pending for Approval) Inter-Bank Transfer mapped to his Department" });
                permissions.Add(new Permission() { Id = 2823, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2801, Name = "Edit Creation Date of Inter-Bank Transfer", Description = "Allow User To Edit Creation Date of Inter-Bank Transfer" });
                permissions.Add(new Permission() { Id = 2824, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2801, Name = "Attach a file with Inter-Bank Transfer", Description = "Allow User To Attach a file with Inter-Bank Transfer" });
                permissions.Add(new Permission() { Id = 2825, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2801, Name = "View list of attached files with Inter-Bank Transfer", Description = "Allow User To View list of attached files with Inter-Bank Transfer" });
                permissions.Add(new Permission() { Id = 2826, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2801, Name = "Mark as Void Inter-Bank Transfer", Description = "Allow User To Mark as Void Inter-Bank Transfer" });
                permissions.Add(new Permission() { Id = 2827, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2801, Name = "Unmark Void Inter-Bank Transfer", Description = "Allow User To unmark as Void Inter-Bank Transfer" });
                permissions.Add(new Permission() { Id = 2828, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2801, Name = "View list of Void Inter-Bank Transfer", Description = "View list of Void Inter-Bank Transfer" });

                //New Permissions
                permissions.Add(new Permission() { Id = 2829, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2801, Name = "ReApprover for new added Inter-Bank Transfer", Description = "Allow User To ReApprove Inter-Bank Transfer which is in pending state" });
                permissions.Add(new Permission() { Id = 2830, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2801, Name = "Add Inter-Bank Transfer without ReApproval", Description = "Allow User To Add New Inter-Bank Transfer without ReApproval" });
                permissions.Add(new Permission() { Id = 2831, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2801, Name = "View(Pending for ReApproval) Inter-Bank Transfer List", Description = "Allow User To View List of All(Pending for ReApproval) Inter-Bank Transfer mapped to his Department" });

                permissions.Add(new Permission() { Id = 2832, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2801, Name = "Edit Amount in Inter-Bank Transfer", Description = "Allow User To Edit Amount in Inter-Bank Transfer" });
                permissions.Add(new Permission() { Id = 2833, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2801, Name = "Edit GL Posting Date of Inter-Bank Transfer", Description = "Allow User To Edit GL Posting Date of Inter-Bank Transfer" });
                permissions.Add(new Permission() { Id = 2834, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2801, Name = "Edit Creation Date of Inter-Bank Transfer", Description = "Allow User To Edit Creation Date of Inter-Bank Transfer" });
                permissions.Add(new Permission() { Id = 2835, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2801, Name = "Edit Accounting Fields After Approval", Description = "Allow User To Edit Accounting Fields After Approval" });

                permissions.Add(new Permission() { Id = 2836, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2801, Name = "Edit Amount in Inter-Bank Transfer Under Approval", Description = "Allow User To Edit Amount in Inter-Bank Transfer Under Approval" });
                permissions.Add(new Permission() { Id = 2837, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2801, Name = "Edit Amount in Inter-Bank Transfer After Approval", Description = "Allow User To Edit Amount in Inter-Bank Transfer After Approval" });

            }


            ///Admin Bills Permissions
            ///
            {
                permissions.Add(new Permission() { Id = 2901, Added = System.DateTime.Now, LastModified = System.DateTime.Now, Name = "Admin Bills", Description = "Admin Bills" });
                permissions.Add(new Permission() { Id = 2902, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2901, Name = "Add Admin Bill", Description = "Allow User To Add Admin Bill" });//Done
                permissions.Add(new Permission() { Id = 2903, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2901, Name = "Edit Admin Bill", Description = "Allow User To Edit Admin Bill" });//Done
                permissions.Add(new Permission() { Id = 2904, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2901, Name = "View Admin Bill", Description = "Allow User To View Admin Bill" });//Done
                permissions.Add(new Permission() { Id = 2905, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2901, Name = "List of Admin Bills", Description = "Allow User To View List of Admin Bills" });//Done
                permissions.Add(new Permission() { Id = 2906, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2901, Name = "Add Admin Bill Status", Description = "Allow User To Add new status for Admin Bill" });//Done
                permissions.Add(new Permission() { Id = 2907, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2901, Name = "Add Admin Bill without Approval", Description = "Allow User To Add New Admin Bill without Approval" });
                permissions.Add(new Permission() { Id = 2908, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2901, Name = "View InActive Admin Bills", Description = "Allow User To View InActive Admin Bills" });
                permissions.Add(new Permission() { Id = 2909, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2901, Name = "Close Admin Bill", Description = "Allow User To Close Admin Bill" });
                permissions.Add(new Permission() { Id = 2910, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2901, Name = "Edit Admin Bill Status", Description = "Allow User To Edit Admin Bill Status" });//Done
                permissions.Add(new Permission() { Id = 2911, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2901, Name = "View InActive Admin Bill Statuses", Description = "Allow User To View InActive Admin Bill Statuses" });

                permissions.Add(new Permission() { Id = 2913, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2901, Name = "Close Admin Bill without Approval", Description = "Allow User To Close Admin Bill without Approval" });
                permissions.Add(new Permission() { Id = 2914, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2901, Name = "Edit Unapproved Admin Bill", Description = "Allow User To Edit Unapproved Admin Bill without Approval" });

                permissions.Add(new Permission() { Id = 2916, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2901, Name = "Reviewer Level 1 Admin Bill", Description = "Allow User to mark Admin Bill as Reviewed once" });
                permissions.Add(new Permission() { Id = 2917, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2901, Name = "Reviewer Level 2 Admin Bill", Description = "Allow User To Mark Admin Bill as Reviewed and move it to Approved List" });
                permissions.Add(new Permission() { Id = 2918, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2901, Name = "Approver for Closing Admin Bill", Description = "Allow User To Close Admin Bill which is in pending state" });
                permissions.Add(new Permission() { Id = 2919, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2901, Name = "Approver for new added Admin Bill", Description = "Allow User To Approve Admin Bill which is in pending state" });
                permissions.Add(new Permission() { Id = 2920, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2901, Name = "Edit closed Admin Bill", Description = "Allow User To Edit Closed Admin Bills" });
                permissions.Add(new Permission() { Id = 2921, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2901, Name = "View(Pending for closing) Admin Bill List", Description = "Allow User To View List of All(Pending for Closing) Admin Bills mapped to his Department" });
                permissions.Add(new Permission() { Id = 2922, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2901, Name = "View(Pending for Approval) Admin Bill List", Description = "Allow User To View List of All(Pending for Approval) Admin Bills mapped to his Department" });
                permissions.Add(new Permission() { Id = 2923, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2901, Name = "Edit Creation Date of Admin Bill", Description = "Allow User To Edit Creation Date of Admin Bill" });
                permissions.Add(new Permission() { Id = 2924, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2901, Name = "Attach a file with Admin Bill", Description = "Allow User To Attach a file with Admin Bill" });
                permissions.Add(new Permission() { Id = 2925, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2901, Name = "View list of attached files with Admin Bill", Description = "Allow User To View list of attached files with Admin Bill" });
                permissions.Add(new Permission() { Id = 2926, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2901, Name = "Mark as Void Admin Bill", Description = "Allow User To Mark as Void Admin Bill" });
                permissions.Add(new Permission() { Id = 2927, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2901, Name = "Unmark Void Admin Bill", Description = "Allow User To unmark as Void Admin Bill" });
                permissions.Add(new Permission() { Id = 2928, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2901, Name = "View list of Void Admin Bills", Description = "View list of Void Admin Bills" });

                //New Permissions
                permissions.Add(new Permission() { Id = 2929, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2901, Name = "ReApprover for new added Admin Bill", Description = "Allow User To ReApprove Admin Bill which is in pending state" });
                permissions.Add(new Permission() { Id = 2930, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2901, Name = "Add Admin Bill without ReApproval", Description = "Allow User To Add New Admin Bill without ReApproval" });
                permissions.Add(new Permission() { Id = 2931, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2901, Name = "View(Pending for ReApproval) Admin Bills List", Description = "Allow User To View List of All(Pending for ReApproval) Admin Bill mapped to his Department" });


                permissions.Add(new Permission() { Id = 2932, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2903, Name = "Edit Admin Bill Fields", Description = "Allow User To Admin Bill Fields" });
                permissions.Add(new Permission() { Id = 2933, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2932, Name = "Add Bill Reference Number in Admin Bill", Description = "Allow User To Add Bill Reference Number in Admin Bill" });
                permissions.Add(new Permission() { Id = 2934, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2932, Name = "Edit Unapproved Admin Bills with Value", Description = "Allow User To Edit Unapproved Admin Bills with Value" });

                permissions.Add(new Permission() { Id = 2936, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2932, Name = "Edit Open Admin Bills with Value", Description = "Allow User To Edit Open Admin Bills with Value" });

                permissions.Add(new Permission() { Id = 2938, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2932, Name = "Edit Pending for Closing Admin Bills with Value", Description = "Allow User To Edit Pending for Closing Admin Bills with Value" });

                permissions.Add(new Permission() { Id = 2940, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2932, Name = "Edit Closed Admin Bills with Value", Description = "Allow User To Edit Closed Admin Bills with Value" });

                permissions.Add(new Permission() { Id = 2941, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2901, Name = "Enable Employee List in Admin Bills", Description = "Allow User To Enable Employee List in Admin Bills" });

                permissions.Add(new Permission() { Id = 2942, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2901, Name = "Close Admin Bills without Payments", Description = "Allow User To Close Admin Bills without Payments" });

                permissions.Add(new Permission() { Id = 2943, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2901, Name = "Can Add Summary Memo for Admin Bills", Description = "Allow User To Add Summary Memo for Admin Bills" });
                permissions.Add(new Permission() { Id = 2944, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2901, Name = "Edit GL Posting Date of Admin Bills", Description = "Allow User To Edit GL Posting Date of Admin Bills" });

                permissions.Add(new Permission() { Id = 2945, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2901, Name = "Can Edit MER in Admin Bills", Description = "Allow User To Edit MER in Admin Bills" });

            }

            //Admin Bill Lists Permissions
            {
                permissions.Add(new Permission() { Id = 2950, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2901, Name = "Admin Bills Lists", Description = "Admin Bills Lists" });

                //Payee
                permissions.Add(new Permission() { Id = 2951, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2950, Name = "Payee", Description = "Allow User to Access Payee" });
                permissions.Add(new Permission() { Id = 2952, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2951, Name = "Add Payee", Description = "Allow User To Add Payee" });
                permissions.Add(new Permission() { Id = 2953, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2951, Name = "Edit Payee", Description = "Allow User To Edit Payee" });
                permissions.Add(new Permission() { Id = 2954, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2951, Name = "List of Payees", Description = "Allow User To View List of Payees" });

                //Bill Reference
                permissions.Add(new Permission() { Id = 2960, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2950, Name = "Bill Reference", Description = "Allow User to Access Bill Reference" });
                permissions.Add(new Permission() { Id = 2961, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2960, Name = "Add Bill Reference", Description = "Allow User To Add Bill Reference" });
                permissions.Add(new Permission() { Id = 2962, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2960, Name = "Edit Bill Reference", Description = "Allow User To Edit Bill Reference" });
                permissions.Add(new Permission() { Id = 2963, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2960, Name = "List of Bill Reference", Description = "Allow User To View List of Bill Reference" });

                //Admin Bill Link

                permissions.Add(new Permission() { Id = 2970, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2950, Name = "Admin Bill Link", Description = "Allow User to Access Admin Bill Link" });
                permissions.Add(new Permission() { Id = 2971, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2970, Name = "Add Admin Bill Link", Description = "Allow User To Add Admin Bill Link" });
                permissions.Add(new Permission() { Id = 2972, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2970, Name = "Edit Admin Bill Link", Description = "Allow User To Edit Bill Reference" });
                permissions.Add(new Permission() { Id = 2973, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 2970, Name = "List of Admin Bill Link", Description = "Allow User To View List of Admin Bill Link" });
            }


            ///Payment Permissions
            ///
            {

                permissions.Add(new Permission() { Id = 3501, Added = System.DateTime.Now, LastModified = System.DateTime.Now, Name = "Payments", Description = "Payments" });

                permissions.Add(new Permission() { Id = 3535, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3501, Name = "Admin Bill Payment", Description = "Allow User To Add Payment" });
                permissions.Add(new Permission() { Id = 3502, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3535, Name = "Add Admin Bill Payment", Description = "Allow User To Add Payment" });
                permissions.Add(new Permission() { Id = 3503, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3535, Name = "Edit Admin Bill Payment", Description = "Allow User To Edit Payment" });
                permissions.Add(new Permission() { Id = 3504, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3535, Name = "View Admin Bill Payment", Description = "Allow User To View Payment" });

                permissions.Add(new Permission() { Id = 3540, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3501, Name = "Vendor Bill Payment", Description = "Allow User To Add Payment" });
                permissions.Add(new Permission() { Id = 3541, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3540, Name = "Add Vendor Bill Payment", Description = "Allow User To Add Payment" });
                permissions.Add(new Permission() { Id = 3542, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3540, Name = "Edit Vendor Bill Payment", Description = "Allow User To Edit Payment" });
                permissions.Add(new Permission() { Id = 3543, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3540, Name = "View Vendor Bill Payment", Description = "Allow User To View Payment" });

                permissions.Add(new Permission() { Id = 3544, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3501, Name = "Purchase Invoice Payment", Description = "Allow User To Add Payment" });
                permissions.Add(new Permission() { Id = 3545, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3544, Name = "Add Purchase Invoice Payment", Description = "Allow User To Add Purchase Invoice Payment" });
                permissions.Add(new Permission() { Id = 3546, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3544, Name = "Edit Purchase Invoice Payment", Description = "Allow User To Edit Purchase Invoice Payment" });
                permissions.Add(new Permission() { Id = 3547, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3544, Name = "View Purchase Invoice Payment", Description = "Allow User To View Purchase Invoice Payment" });

                permissions.Add(new Permission() { Id = 3505, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3501, Name = "List of Payment", Description = "Allow User To View List of Payment" });
                permissions.Add(new Permission() { Id = 3506, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3501, Name = "Add Payment Status", Description = "Allow User To Add new status for Payment" });
                permissions.Add(new Permission() { Id = 3507, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3501, Name = "Add Payment without Approval", Description = "Allow User To Add New Payment without Approval" });
                permissions.Add(new Permission() { Id = 3508, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3501, Name = "View InActive Payment", Description = "Allow User To View InActive Payment" });
                permissions.Add(new Permission() { Id = 3509, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3501, Name = "Close Payment", Description = "Allow User To Close Payment" });
                permissions.Add(new Permission() { Id = 3510, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3501, Name = "Edit Payment Status", Description = "Allow User To Edit Payment Status" });
                permissions.Add(new Permission() { Id = 3511, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3501, Name = "View InActive Payment Statuses", Description = "Allow User To View InActive Payment Statuses" });

                permissions.Add(new Permission() { Id = 3512, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3501, Name = "Close Payment without Approval", Description = "Allow User To Close Payment without Approval" });
                permissions.Add(new Permission() { Id = 3513, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3501, Name = "Edit Unapproved Payment", Description = "Allow User To Edit Unapproved Payment without Approval" });

                permissions.Add(new Permission() { Id = 3514, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3501, Name = "Reviewer Level 1 Payment", Description = "Allow User to mark Payment as Reviewed once" });
                permissions.Add(new Permission() { Id = 3515, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3501, Name = "Reviewer Level 2 Payment", Description = "Allow User To Mark Payment as Reviewed and move it to Approved List" });
                permissions.Add(new Permission() { Id = 3516, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3501, Name = "Approver for Closing Payment", Description = "Allow User To Close Payment which is in pending state" });
                permissions.Add(new Permission() { Id = 3517, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3501, Name = "Approver for new added Payment", Description = "Allow User To Approve Payment which is in pending state" });
                permissions.Add(new Permission() { Id = 3518, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3501, Name = "Edit closed Payment", Description = "Allow User To Edit Closed Payment" });
                permissions.Add(new Permission() { Id = 3519, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3501, Name = "View(Pending for closing) Payment List", Description = "Allow User To View List of All(Pending for Closing) Payment mapped to his Department" });
                permissions.Add(new Permission() { Id = 3520, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3501, Name = "View(Pending for Approval) Payment List", Description = "Allow User To View List of All(Pending for Approval) Payment mapped to his Department" });
                permissions.Add(new Permission() { Id = 3521, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3501, Name = "Edit Creation Date of Payment", Description = "Allow User To Edit Creation Date of Payment" });
                permissions.Add(new Permission() { Id = 3522, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3501, Name = "Attach a file with Payment", Description = "Allow User To Attach a file with Payment" });
                permissions.Add(new Permission() { Id = 3523, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3501, Name = "View list of attached files with Payment", Description = "Allow User To View list of attached files with Payment" });
                permissions.Add(new Permission() { Id = 3524, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3501, Name = "Mark as Void Payment", Description = "Allow User To Mark as Void Payment" });
                permissions.Add(new Permission() { Id = 3525, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3501, Name = "Unmark Void Payment", Description = "Allow User To unmark as Void Payment" });
                permissions.Add(new Permission() { Id = 3526, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3501, Name = "View list of Void Payment", Description = "View list of Void Payment" });

                //New Permissions
                permissions.Add(new Permission() { Id = 3527, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3501, Name = "ReApprover for new added Payment", Description = "Allow User To ReApprove Payment which is in pending state" });
                permissions.Add(new Permission() { Id = 3528, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3501, Name = "Add Payment without ReApproval", Description = "Allow User To Add New Payment without ReApproval" });
                permissions.Add(new Permission() { Id = 3529, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3501, Name = "View(Pending for ReApproval) Payment List", Description = "Allow User To View List of All(Pending for ReApproval) Payment mapped to his Department" });

                permissions.Add(new Permission() { Id = 3530, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3501, Name = "Edit Amount in Payment", Description = "Allow User To Edit Amount in Payment" });


                permissions.Add(new Permission() { Id = 3531, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3501, Name = "Add Payment Reference Number in Payments", Description = "Allow User To Add Payment Reference Number in Payments" });
                permissions.Add(new Permission() { Id = 3532, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3501, Name = "Export Payments Reports", Description = "Allow User To Export Payments Reports" });

                permissions.Add(new Permission() { Id = 3533, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3501, Name = "Update System Cost from Payment", Description = "Allow User To Update CostSheet from Payment" });
                permissions.Add(new Permission() { Id = 3534, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3501, Name = "View Cost Center from Payment", Description = "Allow User To View Cost Center from Payment" });

                permissions.Add(new Permission() { Id = 3560, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3501, Name = "Can Edit Deductions After Approval", Description = "Allow User To Edit Deductions After Approval" });
                permissions.Add(new Permission() { Id = 3561, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3501, Name = "Can View Vendor Bank Accounts in Payments", Description = "Allow User To View Vendor Bank Accounts in Payments" });
                permissions.Add(new Permission() { Id = 3562, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3501, Name = "Edit GL Posting Date of Payment", Description = "Allow User To Edit GL Posting Date of Payment" });
                permissions.Add(new Permission() { Id = 3563, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3501, Name = "Bypass Payment Bank Account", Description = "Allow User To Bypass SaleReceipt Bank Account" });

                permissions.Add(new Permission() { Id = 3564, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3501, Name = "Loans and Advances Payment", Description = "Allow User To Add Payment" });
                permissions.Add(new Permission() { Id = 3565, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3564, Name = "Add Loans and Advances Payment", Description = "Allow User To Add Loans and Advances Payment" });
                permissions.Add(new Permission() { Id = 3566, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3564, Name = "Edit Loans and Advances Payment", Description = "Allow User To Edit Loans and Advances Payment" });
                permissions.Add(new Permission() { Id = 3567, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3564, Name = "View Loans and Advances Payment", Description = "Allow User To View Loans and Advances Payment" });

                permissions.Add(new Permission() { Id = 3568, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3501, Name = "Target Rewards Payment", Description = "Allow User To Add Payment" });
                permissions.Add(new Permission() { Id = 3569, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3568, Name = "Add Target Rewards Payment", Description = "Allow User To Add Target Rewards Payment" });
                permissions.Add(new Permission() { Id = 3570, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3568, Name = "Edit Target Rewards Payment", Description = "Allow User To Edit Target Rewards Payment" });
                permissions.Add(new Permission() { Id = 3571, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3568, Name = "View Target Rewards Payment", Description = "Allow User To View Target Rewards Payment" });

                permissions.Add(new Permission() { Id = 3572, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3501, Name = "Can Reload Payments", Description = "Allow User To Reload Payments" });

            }



            //    //Online USers Permissions(ID 1000 and onwards)
            //    permissions.Add(new Permission() { Id = 1100, Added = System.DateTime.Now, LastModified = System.DateTime.Now, Name = "Online Utility", Description = "Allow to view Online users" });
            //    permissions.Add(new Permission() { Id = 1000, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 1100, Name = "View Online Users", Description = "Allow user to view online users" });
            //    permissions.Add(new Permission() { Id = 1101, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 1100, Name = "View User History", Description = "Allow user to view online users History" });



            //Background Images
            permissions.Add(new Permission() { Id = 3550, Added = System.DateTime.Now, LastModified = System.DateTime.Now, Name = "Upload Background Image", Description = "Allow user to upload Background Image" });

            //This permission is set to access Rental module Tenancy contract
            permissions.Add(new Permission() { Id = 3900, Added = System.DateTime.Now, LastModified = System.DateTime.Now, Name = "Rental Assets", Description = "Allow user to Access Rental Assets" });
            permissions.Add(new Permission() { Id = 3901, ParentId = 3900, Added = System.DateTime.Now, LastModified = System.DateTime.Now, Name = "Add Rental Asset Status", Description = "Allow user to Add rental assets status" });
            permissions.Add(new Permission() { Id = 3902, ParentId = 3900, Added = System.DateTime.Now, LastModified = System.DateTime.Now, Name = "Edit Rental Asset Status", Description = "Allow user to Edit rental assets status" });
            permissions.Add(new Permission() { Id = 3903, ParentId = 3900, Added = System.DateTime.Now, LastModified = System.DateTime.Now, Name = "View InActive Rental Asset Statuses", Description = "Allow user to View InActive rental assets status" });

            ////User Profile realted permissions
            {
                permissions.Add(new Permission() { Id = 3200, Added = System.DateTime.Now, LastModified = System.DateTime.Now, Name = "User Profile", Description = "Permissions related User Profiles" });

                permissions.Add(new Permission() { Id = 3201, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3200, Name = "Access to Profile", Description = "Allow User To Access to Profile" });
                permissions.Add(new Permission() { Id = 3202, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3200, Name = "Access to Summary", Description = "Allow User To Access to Summary" });
                permissions.Add(new Permission() { Id = 3203, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3200, Name = "Access to Teams", Description = "Allow User To Access to Teams" });
                permissions.Add(new Permission() { Id = 3204, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3200, Name = "Access to Permissions", Description = "Allow User To Access to Permissions" });
                permissions.Add(new Permission() { Id = 3205, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3200, Name = "Access to Edit Button in Profile", Description = "Allow User To Access Edit Button in Profile" });

                permissions.Add(new Permission() { Id = 3206, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3200, Name = "Access to Leaves", Description = "Allow User To Access to Leaves" });



                //Leaves Application tree
                permissions.Add(new Permission() { Id = 3210, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3206, Name = "Add New Leave", Description = "Allow User To Add New Leave " });//
                permissions.Add(new Permission() { Id = 3211, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3206, Name = "Edit Unapproved Leave", Description = "Allow User To Edit Unapproved Leave Information" });//
                permissions.Add(new Permission() { Id = 3213, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3206, Name = "View(Pending for Approval) own Leave List", Description = "Allow User To View(Pending for Approval) Leave List" });//
                permissions.Add(new Permission() { Id = 3214, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3206, Name = "View(Pending for ReApproval)  own Leave List", Description = "Allow User To View List of All(Pending for ReApproval) Leave mapped to his Department" });//
                permissions.Add(new Permission() { Id = 3215, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3206, Name = "View list of Void own Leaves", Description = "View list of Void Leave" });//
                permissions.Add(new Permission() { Id = 3216, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3206, Name = "View Approved Leave Application Register", Description = "Allow User To View Approved Leave Application Register" });//
                permissions.Add(new Permission() { Id = 3217, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3206, Name = "View own Leave Register", Description = "Allow User To View Leave Register" });//
                permissions.Add(new Permission() { Id = 3218, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3206, Name = "View CLosed Year Leave Application", Description = "Allow User ToView CLosed Year Leave Application" });//
                permissions.Add(new Permission() { Id = 3219, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3206, Name = "Attach a file with own Leave", Description = "Allow User To Access to Leaves" });
                permissions.Add(new Permission() { Id = 3220, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3206, Name = "View list of attached files with own Leave", Description = "View list of attached files with Leave" });
                permissions.Add(new Permission() { Id = 3221, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3206, Name = "Mark as Void Unapproved own Leave", Description = "Mark as Void Unapproved Leave" });//
                permissions.Add(new Permission() { Id = 3222, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3206, Name = "Unmark Void Unapproved own Leave", Description = "Allow User To Unmark Void Unapproved Leave" });//
                permissions.Add(new Permission() { Id = 3223, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3206, Name = "Add New own Leave Adjustment", Description = "Allow User To Add New Leave Adjustment" });//
            }

            //    //COA permissions(3600 -3699)
            permissions.Add(new Permission() { Id = 3600, Added = System.DateTime.Now, LastModified = System.DateTime.Now, Name = "Accountant Center", Description = "Allow User To Open Accountant Center" });
            permissions.Add(new Permission() { Id = 3601, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3600, Name = "Add Chart of Account", Description = "Allow User To Add Chart of Account" });
            permissions.Add(new Permission() { Id = 3602, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3600, Name = "Edit Chart of Account", Description = "Allow User To Edit Chart of Account" });
            permissions.Add(new Permission() { Id = 3603, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3600, Name = "View Chart of Account", Description = "Allow User To View Chart of Account" });
            permissions.Add(new Permission() { Id = 3604, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3600, Name = "List of Chart of Accounts", Description = "Allow User To View List of Chart of Accounts" });
            permissions.Add(new Permission() { Id = 3605, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3600, Name = "Add Chart of Account without Approval", Description = "Allow User To Add New Chart of Account without Approval" });
            permissions.Add(new Permission() { Id = 3606, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3600, Name = "Approver for new added Chart of Account", Description = "Allow User To Approve Chart of Account which is in pending state" });
            permissions.Add(new Permission() { Id = 3607, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3600, Name = "Edit Unapproved Chart of Account", Description = "Allow User To Edit Unapproved Chart of Account" });
            permissions.Add(new Permission() { Id = 3608, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3600, Name = "Add Chart of Account without ReApproval", Description = "Allow User To Add ChartofAccount without ReApproval" });
            permissions.Add(new Permission() { Id = 3609, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3600, Name = "ReApprover for new added ChartofAccount", Description = "Allow User ToReApprover for new added ChartofAccount" });
            permissions.Add(new Permission() { Id = 3610, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3600, Name = "Reviewer Level 1 Chart of Accounts", Description = "Allow User To Reviewer Level 1 Account" });
            permissions.Add(new Permission() { Id = 3611, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3600, Name = "Reviewer Level 2 Chart of Accounts", Description = "Allow User To Reviewer Level 2 Account" });
            permissions.Add(new Permission() { Id = 3612, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3600, Name = "View(Pending for Approval) Chart of Accounts List", Description = "Allow User To View(Pending for Approval) Account List" });
            permissions.Add(new Permission() { Id = 3613, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3600, Name = "View(Pending for ReApproval) Chart of Accounts List", Description = "Allow User To View(Pending for ReApproval) Account List" });
            permissions.Add(new Permission() { Id = 3614, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3600, Name = "View list of Void Chart of Accounts", Description = "Allow User To View list of Void Accounts" });
            permissions.Add(new Permission() { Id = 3615, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3600, Name = "View Chart of Account Register", Description = "Allow User To View Account Register" });
            permissions.Add(new Permission() { Id = 3616, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3600, Name = "Close Chart of Account without Approval", Description = "Allow User To View Account Register" });
            permissions.Add(new Permission() { Id = 3617, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3600, Name = "Approver for Closing Chart of Account", Description = "Allow User To Approver for Closing Chart of Account" });
            permissions.Add(new Permission() { Id = 3618, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3600, Name = "View(Pending for closing) Chart of Account List", Description = "Allow User To View(Pending for closing) Chart of Account List" });
            permissions.Add(new Permission() { Id = 3619, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3600, Name = "View InActive Chart of Accounts", Description = "Allow User To View InActive Chart of Accounts" });
            permissions.Add(new Permission() { Id = 3620, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3600, Name = "View(Pending for Approval) Chart of Account List", Description = "Allow User To View(Pending for Approval) Chart of Account List" });
            permissions.Add(new Permission() { Id = 3621, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3600, Name = "Unmark Void Chart of Account", Description = "Allow User To Unmark Void Chart of Account" });
            permissions.Add(new Permission() { Id = 3622, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3600, Name = "Mark as Void Chart of Account", Description = "Allow User To Mark as Void Chart of Account" });
            permissions.Add(new Permission() { Id = 3623, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3600, Name = "Reconcile Chart of Account", Description = "Allow User To Reconcile Chart of Account" });
            permissions.Add(new Permission() { Id = 3624, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3600, Name = "Clear COA Transactions", Description = "Allow User To Clear COA Transactions" });
            permissions.Add(new Permission() { Id = 3625, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3600, Name = "View Reconcilation History", Description = "Allow User View Reconcilation History" });
            permissions.Add(new Permission() { Id = 3626, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3600, Name = "Export Chart of Accounts Reports", Description = "Allow User To Export Chart of Accounts Reports" });
            permissions.Add(new Permission() { Id = 3627, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3600, Name = "Post/Un-Post to GL", Description = "Allow User To Post/Un-Post to GL" });

            permissions.Add(new Permission() { Id = 3628, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3600, Name = "Bypass COA Permissions", Description = "Allow User To Bypass COA Permissions" });
            permissions.Add(new Permission() { Id = 3629, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3600, Name = "View General Journal", Description = "Allow User To View General Journal" });
            permissions.Add(new Permission() { Id = 3630, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3600, Name = "Access to COA Admin Panel", Description = "Allow user to Access COA Admin Panel" });
            permissions.Add(new Permission() { Id = 3631, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3600, Name = "Add Manual Bank Balances", Description = "Allow user to Add Manual Bank Balances" });

            //JVPermissions (3700-3799)
            {
                permissions.Add(new Permission() { Id = 3700, Added = System.DateTime.Now, LastModified = System.DateTime.Now, Name = "JV", Description = "General Journal Entries" });
                permissions.Add(new Permission() { Id = 3701, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3700, Name = "Add JV", Description = "Allow User To Add JV" });//Done
                permissions.Add(new Permission() { Id = 3702, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3700, Name = "Edit JV", Description = "Allow User To Edit JV" });//Done
                permissions.Add(new Permission() { Id = 3703, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3700, Name = "View JV", Description = "Allow User To View JV" });//Done
                permissions.Add(new Permission() { Id = 3704, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3700, Name = "List of JV", Description = "Allow User To View List of JV" });//Done
                permissions.Add(new Permission() { Id = 3705, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3700, Name = "Add JV Status", Description = "Allow User To Add new status for JV" });//Done
                permissions.Add(new Permission() { Id = 3706, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3700, Name = "Add JV without Approval", Description = "Allow User To Add New JV without Approval" });
                permissions.Add(new Permission() { Id = 3707, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3700, Name = "View InActive JV", Description = "Allow User To View InActive JV" });
                permissions.Add(new Permission() { Id = 3708, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3700, Name = "Close JV", Description = "Allow User To Close Sale Receipt" });
                permissions.Add(new Permission() { Id = 3709, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3700, Name = "Edit JV Status", Description = "Allow User To Edit JV Status" });//Done
                permissions.Add(new Permission() { Id = 3710, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3700, Name = "View InActive JV Statuses", Description = "Allow User To View InActive JV Statuses" });

                permissions.Add(new Permission() { Id = 3711, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3700, Name = "Close JV without Approval", Description = "Allow User To Close JV without Approval" });
                permissions.Add(new Permission() { Id = 3712, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3700, Name = "Edit Unapproved JV", Description = "Allow User To Edit Unapproved JV without Approval" });

                permissions.Add(new Permission() { Id = 3713, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3700, Name = "Reviewer Level 1 JV", Description = "Allow User to mark JV as Reviewed once" });
                permissions.Add(new Permission() { Id = 3714, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3700, Name = "Reviewer Level 2 JV", Description = "Allow User To Mark JV as Reviewed and move it to Approved List" });
                permissions.Add(new Permission() { Id = 3715, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3700, Name = "Approver for Closing JV", Description = "Allow User To Close JV which is in pending state" });
                permissions.Add(new Permission() { Id = 3716, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3700, Name = "Approver for new added JV", Description = "Allow User To Approve Offer which is in pending state" });
                permissions.Add(new Permission() { Id = 3717, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3700, Name = "Edit closed JV", Description = "Allow User To Edit Closed JV" });
                permissions.Add(new Permission() { Id = 3718, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3700, Name = "View(Pending for closing) JV List", Description = "Allow User To View List of All(Pending for Closing) JV mapped to his Department" });
                permissions.Add(new Permission() { Id = 3719, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3700, Name = "View(Pending for Approval) JV List", Description = "Allow User To View List of All(Pending for Approval) JV mapped to his Department" });
                permissions.Add(new Permission() { Id = 3720, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3700, Name = "Edit Creation Date of JV", Description = "Allow User To Edit Creation Date of JV" });
                permissions.Add(new Permission() { Id = 3721, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3700, Name = "Attach a file with JV", Description = "Allow User To Attach a file with JV" });
                permissions.Add(new Permission() { Id = 3722, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3700, Name = "View list of attached files with JV", Description = "Allow User To View list of attached files with JV" });
                permissions.Add(new Permission() { Id = 3723, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3700, Name = "Mark as Void JV", Description = "Allow User To Mark as Void JV" });
                permissions.Add(new Permission() { Id = 3724, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3700, Name = "Unmark Void JV", Description = "Allow User To unmark as Void JV" });
                permissions.Add(new Permission() { Id = 3725, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3700, Name = "View list of Void JV", Description = "View list of Void JV" });

                permissions.Add(new Permission() { Id = 3726, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3700, Name = "View JV Register", Description = "View list of JV Register" });
                permissions.Add(new Permission() { Id = 3727, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3700, Name = "View(Pending for ReApproval) JV List", Description = "View list of Pending for ReApprovals" });

                permissions.Add(new Permission() { Id = 3728, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3700, Name = "View list of Void Journal Vouchers", Description = "View list of Void Journal Vouchers" });
            }


            //    //Trial-Balance Permission (3800 to 3850)

            permissions.Add(new Permission() { Id = 3800, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3600, Name = "Trial Balance", Description = "Allow User To Open Trial Balance" });
            permissions.Add(new Permission() { Id = 3801, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3600, Name = "Profit & Loss", Description = "Allow User To Open Profit & Loss" });
            permissions.Add(new Permission() { Id = 3802, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3600, Name = "View Balance Sheet Detail", Description = "Allow User To View Balance Sheet Detail" });
            permissions.Add(new Permission() { Id = 3803, Added = System.DateTime.Now, LastModified = System.DateTime.Now, Name = "Export Trial Balance Reports", ParentId = 3800, Description = "Allow User To Export Trial Balance Reports" });


            permissions.Add(new Permission() { Id = 3804, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3600, Name = "Chart of Account Group", Description = "Allow user to Access Chart of Account Group" });
            permissions.Add(new Permission() { Id = 3805, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3804, Name = "Add Chart of Account Group", Description = "Allow user to Access Add Chart of Account Group" });
            permissions.Add(new Permission() { Id = 3806, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3804, Name = "Update Chart of Account Group", Description = "Allow user to Access Update Chart of Account Group" });
            permissions.Add(new Permission() { Id = 3807, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3804, Name = "List of Chart of Account Groups", Description = "Allow user to Access Chart of Account Group" });




            ///////////////////////
            ///Purchase Invoices Permissions 
            ///
            {
                permissions.Add(new Permission() { Id = 4000, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 50, Name = "Purchase Invoices", Description = "Purchase Invoices" });
                permissions.Add(new Permission() { Id = 4001, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4000, Name = "Add Purchase Invoice", Description = "Allow User To Add Purchase Invoice" });
                permissions.Add(new Permission() { Id = 4002, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4000, Name = "Edit Purchase Invoice", Description = "Allow User To Edit Purchase Invoice" });
                permissions.Add(new Permission() { Id = 4003, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4000, Name = "View Purchase Invoice", Description = "Allow User To View Purchase Invoice" });
                permissions.Add(new Permission() { Id = 4004, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4000, Name = "List of Purchase Invoices", Description = "Allow User To View List of PurchaseInvoice" });
                permissions.Add(new Permission() { Id = 4005, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4000, Name = "Add Purchase Invoice Status", Description = "Allow User To Add new status for PurchaseInvoice" });
                permissions.Add(new Permission() { Id = 4006, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4000, Name = "Add Purchase Invoice without Approval", Description = "Allow User To Add New PurchaseInvoice without Approval" });
                permissions.Add(new Permission() { Id = 4007, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4000, Name = "View InActive Purchase Invoices", Description = "Allow User To View InActive Purchase Invoices" });
                permissions.Add(new Permission() { Id = 4008, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4000, Name = "Close Purchase Invoice", Description = "Allow User To Close PurchaseInvoice" });
                permissions.Add(new Permission() { Id = 4009, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4000, Name = "Edit Purchase Invoice Status", Description = "Allow User To Edit PurchaseInvoice Status" });
                permissions.Add(new Permission() { Id = 4010, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4000, Name = "View InActive Purchase Invoice Statuses", Description = "Allow User To View InActive PurchaseInvoice Statuses" });

                permissions.Add(new Permission() { Id = 4011, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4000, Name = "Close Purchase Invoice without Approval", Description = "Allow User To Close PurchaseInvoice without Approval" });
                permissions.Add(new Permission() { Id = 4012, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4000, Name = "Edit Unapproved Purchase Invoice", Description = "Allow User To Edit Unapproved PurchaseInvoice without Approval" });
                permissions.Add(new Permission() { Id = 4013, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4000, Name = "View Market Exchange Rate in Purchase Invoice", Description = "Allow User To View Market Exchange Rate in PurchaseInvoice" });

                permissions.Add(new Permission() { Id = 4014, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4000, Name = "Reviewer Level 1 PurchaseInvoice", Description = "Allow User to mark PurchaseInvoice as Reviewed once" });
                permissions.Add(new Permission() { Id = 4015, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4000, Name = "Reviewer Level 2 PurchaseInvoice", Description = "Allow User To Mark PurchaseInvoice as Reviewed and move it to Approved List" });
                permissions.Add(new Permission() { Id = 4016, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4000, Name = "Approver for Closing PurchaseInvoice", Description = "Allow User To Close PurchaseInvoice which is in pending state" });
                permissions.Add(new Permission() { Id = 4017, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4000, Name = "Approver for new added PurchaseInvoice", Description = "Allow User To Approve PurchaseInvoice which is in pending state" });
                permissions.Add(new Permission() { Id = 4018, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4000, Name = "Edit closed PurchaseInvoice", Description = "Allow User To Edit Closed PurchaseInvoice" });
                permissions.Add(new Permission() { Id = 4019, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4000, Name = "View(Pending for closing) PurchaseInvoice List", Description = "Allow User To View List of All(Pending for Closing) PurchaseInvoice mapped to his Department" });
                permissions.Add(new Permission() { Id = 4020, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4000, Name = "View(Pending for Approval) PurchaseInvoice List", Description = "Allow User To View List of All(Pending for Approval) PurchaseInvoice mapped to his Department" });
                permissions.Add(new Permission() { Id = 4021, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4000, Name = "Edit Creation Date of PurchaseInvoice", Description = "Allow User To Edit Creation Date of PurchaseInvoice" });
                permissions.Add(new Permission() { Id = 4023, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4000, Name = "Attach a file with PurchaseInvoice", Description = "Allow User To Attach a file with PurchaseInvoice" });
                permissions.Add(new Permission() { Id = 4024, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4000, Name = "View list of attached files with PurchaseInvoice", Description = "Allow User To View list of attached files with PurchaseInvoice" });
                permissions.Add(new Permission() { Id = 4025, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4000, Name = "Mark as Void PurchaseInvoice", Description = "Allow User To Mark as Void PurchaseInvoice" });
                permissions.Add(new Permission() { Id = 4026, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4000, Name = "Unmark Void PurchaseInvoice", Description = "Allow User To unmark as Void PurchaseInvoice" });
                permissions.Add(new Permission() { Id = 4027, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4000, Name = "View list of Void PurchaseInvoices", Description = "View list of Void SaleInvoices" });
                permissions.Add(new Permission() { Id = 4028, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4000, Name = "Move PurchaseInvoice to Inter Company", Description = "Allow User To Move PurchaseInvoice to Inter Company mapped to his Department" });
                permissions.Add(new Permission() { Id = 4029, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4000, Name = "Edit FOB and CFR value for products in PurchaseInvoice", Description = "Allow User To Edit FOB and CFR value for products in SaleInvoice" });
                permissions.Add(new Permission() { Id = 4030, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4000, Name = "Close PurchaseInvoice without receiving fully Collection", Description = "Allow User To Close PurchaseInvoice without receiving fully Collection" });
                permissions.Add(new Permission() { Id = 4031, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4000, Name = "View Purchase Invoice Register", Description = "Allow User To View Purchase Invoice Register" });
                permissions.Add(new Permission() { Id = 4032, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4000, Name = "Export Purchase Invoices Reports", Description = "Allow User To Export Purchase Invoices Reports" });
                permissions.Add(new Permission() { Id = 4033, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 701, Name = "Edit GL Posting Date of PurchaseInvoice", Description = "Allow User To Edit GL Posting Date of PurchaseInvoice" });

            }

            ///////////////////////
            ///Loans Permissions 
            ///
            {
                permissions.Add(new Permission() { Id = 4100, Added = System.DateTime.Now, LastModified = System.DateTime.Now, Name = "Loans", Description = "Loans" });
                permissions.Add(new Permission() { Id = 4101, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4100, Name = "Add Loans", Description = "Allow User to Add Loans" });
                permissions.Add(new Permission() { Id = 4102, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4100, Name = "Edit Loans", Description = "Allow User to Edit Loans" });
                permissions.Add(new Permission() { Id = 4103, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4100, Name = "View Loans", Description = "Allow User To View Loans" });
                permissions.Add(new Permission() { Id = 4104, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4100, Name = "List of Loans", Description = "Allow User To View List of Loans" });
                permissions.Add(new Permission() { Id = 4105, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4100, Name = "Add Loans Status", Description = "Allow User To Add new status for Loans" });
                permissions.Add(new Permission() { Id = 4106, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4100, Name = "Add Loans without Approval", Description = "Allow User To Add New Loans without Approval" });
                permissions.Add(new Permission() { Id = 4107, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4100, Name = "View InActive Loans", Description = "Allow User To View InActive Loans" });
                permissions.Add(new Permission() { Id = 4108, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4100, Name = "Close Loans", Description = "Allow User To Close Loans" });
                permissions.Add(new Permission() { Id = 4109, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4100, Name = "Edit Loans Status", Description = "Allow User To Edit Loans Status" });
                permissions.Add(new Permission() { Id = 4110, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4100, Name = "View InActive Loans Statuses", Description = "Allow User To View InActive Loans Statuses" });

                permissions.Add(new Permission() { Id = 4111, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4100, Name = "Close Loans without Approval", Description = "Allow User To Close Loans without Approval" });
                permissions.Add(new Permission() { Id = 4112, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4100, Name = "Edit Unapproved Loans", Description = "Allow User To Edit Unapproved Loans without Approval" });

                permissions.Add(new Permission() { Id = 4113, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4100, Name = "Reviewer Level 1 Loans", Description = "Allow User to mark Loans as Reviewed once" });
                permissions.Add(new Permission() { Id = 4114, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4100, Name = "Reviewer Level 2 Loans", Description = "Allow User To Mark Loans as Reviewed and move it to Approved List" });
                permissions.Add(new Permission() { Id = 4115, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4100, Name = "Approver for Closing Loans", Description = "Allow User To Close Loans which is in pending state" });
                permissions.Add(new Permission() { Id = 4116, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4100, Name = "Approver for new added Loans", Description = "Allow User To Approve Loans which is in pending state" });
                permissions.Add(new Permission() { Id = 4117, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4100, Name = "Edit closed Loans", Description = "Allow User To Edit Closed Loans" });
                permissions.Add(new Permission() { Id = 4118, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4100, Name = "View(Pending for closing) Loans List", Description = "Allow User To View List of All(Pending for Closing) Loans mapped to his Department" });
                permissions.Add(new Permission() { Id = 4119, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4100, Name = "View(Pending for Approval) Loans List", Description = "Allow User To View List of All(Pending for Approval) Loans mapped to his Department" });
                permissions.Add(new Permission() { Id = 4120, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4100, Name = "Edit Creation Date of Loans", Description = "Allow User To Edit Creation Date of Loans" });
                permissions.Add(new Permission() { Id = 4121, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4100, Name = "Attach a file with Loans", Description = "Allow User To Attach a file with Loans" });
                permissions.Add(new Permission() { Id = 4122, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4100, Name = "View list of attached files with Loans", Description = "Allow User To View list of attached files with Loans" });
                permissions.Add(new Permission() { Id = 4123, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4100, Name = "Mark as Void Loans", Description = "Allow User To Mark as Void Loans" });
                permissions.Add(new Permission() { Id = 4124, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4100, Name = "Unmark Void Loans", Description = "Allow User To unmark as Void Loans" });
                permissions.Add(new Permission() { Id = 4125, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4100, Name = "View list of Void Loans", Description = "View list of Void Loans" });

                //New Permissions
                permissions.Add(new Permission() { Id = 4126, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4100, Name = "ReApprover for new added Loans", Description = "Allow User To ReApprove Loans which is in pending state" });
                permissions.Add(new Permission() { Id = 4127, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4100, Name = "Add Loans without ReApproval", Description = "Allow User To Add New Loans without ReApproval" });
                permissions.Add(new Permission() { Id = 4128, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4100, Name = "View(Pending for ReApproval) Loans List", Description = "Allow User To View List of All(Pending for ReApproval) Loans mapped to his Department" });

            }



            ///Loans Advance Permissions
            ///
            {
                permissions.Add(new Permission() { Id = 6001, Added = System.DateTime.Now, LastModified = System.DateTime.Now, Name = "Loans and Advances", Description = "Loans and Advances" });

                permissions.Add(new Permission() { Id = 5999, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6001, Name = "Advances", Description = "Advances" });

                permissions.Add(new Permission() { Id = 6000, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 5999, Name = "Admin Bill Loans Advances", Description = "Admin Bill Loans Advances" });
                permissions.Add(new Permission() { Id = 6002, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6000, Name = "Add Loans and Advances", Description = "Allow User To Add Loans and Advances" });//Done
                permissions.Add(new Permission() { Id = 6003, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6000, Name = "Edit Loans and Advances", Description = "Allow User To Edit Loans and Advances" });//Done
                permissions.Add(new Permission() { Id = 6004, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6000, Name = "View Loans and Advances", Description = "Allow User To View Loans and Advances" });//Done
                permissions.Add(new Permission() { Id = 6005, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 5999, Name = "List of Loans and Advances", Description = "Allow User To View List of Loans and Advances" });//Done
                permissions.Add(new Permission() { Id = 6006, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 5999, Name = "Add Loans and Advances Status", Description = "Allow User To Add new status for Loans and Advances" });//Done
                permissions.Add(new Permission() { Id = 6007, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 5999, Name = "Add Loans and Advances without Approval", Description = "Allow User To Add New Loans and Advances without Approval" });
                permissions.Add(new Permission() { Id = 6008, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 5999, Name = "View InActive Loans and Advances", Description = "Allow User To View InActive Loans and Advances" });
                permissions.Add(new Permission() { Id = 6009, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 5999, Name = "Close Loans and Advances", Description = "Allow User To Close Loans and Advances" });
                permissions.Add(new Permission() { Id = 6010, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 5999, Name = "Edit Loans and Advances Status", Description = "Allow User To Edit Loans and Advances Status" });//Done
                permissions.Add(new Permission() { Id = 6011, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 5999, Name = "View InActive Loans and Advances Statuses", Description = "Allow User To View InActive Loans and Advances Statuses" });

                permissions.Add(new Permission() { Id = 6013, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 5999, Name = "Close Loans and Advances without Approval", Description = "Allow User To Close Loans and Advances without Approval" });
                permissions.Add(new Permission() { Id = 6014, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 5999, Name = "Edit Unapproved Loans and Advances", Description = "Allow User To Edit Unapproved Loans and Advances without Approval" });

                permissions.Add(new Permission() { Id = 6016, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 5999, Name = "Reviewer Level 1 Loans and Advances", Description = "Allow User to mark Loans and Advances as Reviewed once" });
                permissions.Add(new Permission() { Id = 6017, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 5999, Name = "Reviewer Level 2 Loans and Advances", Description = "Allow User To Mark Loans and Advances as Reviewed and move it to Approved List" });
                permissions.Add(new Permission() { Id = 6018, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 5999, Name = "Approver for Closing Loans and Advances", Description = "Allow User To Close Loans and Advances which is in pending state" });
                permissions.Add(new Permission() { Id = 6019, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 5999, Name = "Approver for new added Loans and Advances", Description = "Allow User To Approve Loans and Advances which is in pending state" });
                permissions.Add(new Permission() { Id = 6020, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 5999, Name = "Edit closed Loans and Advances", Description = "Allow User To Edit Closed Loans and Advances" });
                permissions.Add(new Permission() { Id = 6021, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 5999, Name = "View(Pending for closing) Loans and Advances List", Description = "Allow User To View List of All(Pending for Closing) Loans and Advances mapped to his Department" });
                permissions.Add(new Permission() { Id = 6022, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 5999, Name = "View(Pending for Approval) Loans and Advances List", Description = "Allow User To View List of All(Pending for Approval) Loans and Advances mapped to his Department" });
                permissions.Add(new Permission() { Id = 6023, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 5999, Name = "Edit Creation Date of Loans and Advances", Description = "Allow User To Edit Creation Date of Loans and Advances" });
                permissions.Add(new Permission() { Id = 6024, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 5999, Name = "Attach a file with Loans and Advances", Description = "Allow User To Attach a file with Loans and Advances" });
                permissions.Add(new Permission() { Id = 6025, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 5999, Name = "View list of attached files with Loans and Advances", Description = "Allow User To View list of attached files with Loans and Advances" });
                permissions.Add(new Permission() { Id = 6026, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 5999, Name = "Mark as Void Loans and Advances", Description = "Allow User To Mark as Void Loans and Advances" });
                permissions.Add(new Permission() { Id = 6027, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 5999, Name = "Unmark Void Loans and Advances", Description = "Allow User To unmark as Void Loans and Advances" });
                permissions.Add(new Permission() { Id = 6028, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 5999, Name = "View list of Void Loans and Advances", Description = "View list of Void Loans and Advances" });

                //New Permissions
                permissions.Add(new Permission() { Id = 6029, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 5999, Name = "ReApprover for new added Loans and Advances", Description = "Allow User To ReApprove Loans and Advances which is in pending state" });
                permissions.Add(new Permission() { Id = 6030, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 5999, Name = "Add Loans and Advances without ReApproval", Description = "Allow User To Add New Loans and Advances without ReApproval" });
                permissions.Add(new Permission() { Id = 6031, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 5999, Name = "View(Pending for ReApproval) Loans and Advances List", Description = "Allow User To View List of All(Pending for ReApproval) Loans and Advances mapped to his Department" });

                permissions.Add(new Permission() { Id = 6032, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 5999, Name = "Edit Loan Approved Amount", Description = "Allow User to Edit Loan Approved Amount" });
                permissions.Add(new Permission() { Id = 6033, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 5999, Name = "Adjustments", Description = "Adjustments" });
                permissions.Add(new Permission() { Id = 6034, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6033, Name = "Add Adjustments", Description = "Allow User to View Adjustments" });
                permissions.Add(new Permission() { Id = 6035, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6033, Name = "Edit Adjustments", Description = "Allow User to Edit Adjustments" });
                permissions.Add(new Permission() { Id = 6036, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6033, Name = "Add Adjustments without Approval", Description = "Allow User to Add Adjustments without Approval" });
                permissions.Add(new Permission() { Id = 6037, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6033, Name = "View(Pending for Approval) Adjustments List", Description = "Allow User to View(Pending for Approval) Adjustments List" });


                permissions.Add(new Permission() { Id = 6039, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6000, Name = "Can Create Admin Bill From Loans Advances", Description = "Allow User to Create Admin Bill From Loans Advances" });

                permissions.Add(new Permission() { Id = 6040, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 5999, Name = "Can Edit MER in Loans Advances", Description = "Allow User To Edit MER in Loans Advances" });



                permissions.Add(new Permission() { Id = 6042, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 5999, Name = "Vendor Bill Loans Advances", Description = "Vendor Bill Loans Advances" });
                permissions.Add(new Permission() { Id = 6043, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6042, Name = "Add Vendor Bill Loans Advances", Description = "Allow User To Add Vendor Bill Loans Advances" });
                permissions.Add(new Permission() { Id = 6044, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6042, Name = "Edit Vendor Bill Loans Advances", Description = "Allow User To Edit Vendor Bill Loans Advances" });
                permissions.Add(new Permission() { Id = 6045, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6042, Name = "View Vendor Bill Loans Advances", Description = "Allow User To View Vendor Bill Loans Advances" });
                permissions.Add(new Permission() { Id = 6050, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6042, Name = "Can Create Vendor Bill From Loans Advances", Description = "Allow User to Create Vendor Bill From Loans Advances" });


                permissions.Add(new Permission() { Id = 6038, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 5999, Name = "Export Advances Reports", Description = "Allow User to Export Advances Reports" });

                permissions.Add(new Permission() { Id = 6046, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6001, Name = "Company Loans", Description = "Company Loans" });

                permissions.Add(new Permission() { Id = 6051, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6046, Name = "Admin Bill Company Loans", Description = "Admin Bill Company Loans" });

                permissions.Add(new Permission() { Id = 6047, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6051, Name = "Add Company Loans", Description = "Allow User To Add Company Loans" });
                permissions.Add(new Permission() { Id = 6048, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6051, Name = "Edit Company Loans", Description = "Allow User To Edit Company Loans" });
                permissions.Add(new Permission() { Id = 6049, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6051, Name = "View Company Loans", Description = "Allow User To View Company Loans" });


                permissions.Add(new Permission() { Id = 6052, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6046, Name = "List of Company Loans", Description = "Allow User To View List of Company Loans" });//Done
                permissions.Add(new Permission() { Id = 6054, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6046, Name = "Add Company Loans without Approval", Description = "Allow User To Add New Company Loans without Approval" });
                permissions.Add(new Permission() { Id = 6055, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6046, Name = "View InActive Company Loans", Description = "Allow User To View InActive Company Loans" });
                permissions.Add(new Permission() { Id = 6056, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6046, Name = "Close Company Loans", Description = "Allow User To Close Company Loans" });

                permissions.Add(new Permission() { Id = 6057, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6046, Name = "Close Company Loans without Approval", Description = "Allow User To Close Company Loans without Approval" });
                permissions.Add(new Permission() { Id = 6058, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6046, Name = "Edit Unapproved Company Loans", Description = "Allow User To Edit Unapproved Company Loans without Approval" });

                permissions.Add(new Permission() { Id = 6059, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6046, Name = "Reviewer Level 1 Company Loans", Description = "Allow User to mark Company Loans as Reviewed once" });
                permissions.Add(new Permission() { Id = 6060, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6046, Name = "Reviewer Level 2 Company Loans", Description = "Allow User To Mark Company Loans as Reviewed and move it to Approved List" });
                permissions.Add(new Permission() { Id = 6061, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6046, Name = "Approver for Closing Company Loans", Description = "Allow User To Close Company Loans which is in pending state" });
                permissions.Add(new Permission() { Id = 6062, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6046, Name = "Approver for new added Company Loans", Description = "Allow User To Approve Company Loans which is in pending state" });
                permissions.Add(new Permission() { Id = 6063, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6046, Name = "Edit closed Company Loans", Description = "Allow User To Edit Closed Company Loans" });
                permissions.Add(new Permission() { Id = 6064, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6046, Name = "View(Pending for closing) Company Loans List", Description = "Allow User To View List of All(Pending for Closing) Company Loans mapped to his Department" });
                permissions.Add(new Permission() { Id = 6065, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6046, Name = "View(Pending for Approval) Company Loans List", Description = "Allow User To View List of All(Pending for Approval) Company Loans mapped to his Department" });
                permissions.Add(new Permission() { Id = 6066, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6046, Name = "Edit Creation Date of Company Loans", Description = "Allow User To Edit Creation Date of Company Loans" });
                permissions.Add(new Permission() { Id = 6067, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6046, Name = "Attach a file with Company Loans", Description = "Allow User To Attach a file with Company Loans" });
                permissions.Add(new Permission() { Id = 6068, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6046, Name = "View list of attached files with Company Loans", Description = "Allow User To View list of attached files with Company Loans" });
                permissions.Add(new Permission() { Id = 6069, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6046, Name = "Mark as Void Company Loans", Description = "Allow User To Mark as Void Company Loans" });
                permissions.Add(new Permission() { Id = 6070, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6046, Name = "Unmark Void Company Loans", Description = "Allow User To unmark as Void Company Loans" });
                permissions.Add(new Permission() { Id = 6071, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6046, Name = "View list of Void Company Loans", Description = "View list of Void Company Loans" });

                //New Permissions
                permissions.Add(new Permission() { Id = 6072, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6046, Name = "ReApprover for new added Company Loans", Description = "Allow User To ReApprove Company Loans which is in pending state" });
                permissions.Add(new Permission() { Id = 6073, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6046, Name = "Add Company Loans without ReApproval", Description = "Allow User To Add New Company Loans without ReApproval" });
                permissions.Add(new Permission() { Id = 6074, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6046, Name = "View(Pending for ReApproval) Company Loans List", Description = "Allow User To View List of All(Pending for ReApproval) Company Loans mapped to his Department" });

                permissions.Add(new Permission() { Id = 6075, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6046, Name = "Edit Company Loans Approved Amount", Description = "Allow User to Edit Company Loans Approved Amount" });

                //permissions.Add(new Permission() { Id = 6038, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6001, Name = "Admin Bills for Loans Advances", Description = "Admin Bills for Loans Advances" });
                permissions.Add(new Permission() { Id = 6076, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6051, Name = "Can Create Admin Bill From Company Loans", Description = "Allow User to Create Admin Bill From Company Loans" });

                permissions.Add(new Permission() { Id = 6077, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6046, Name = "Can Edit MER in Company Loans", Description = "Allow User To Edit MER in Company Loans" });

                permissions.Add(new Permission() { Id = 6078, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6046, Name = "Vendor Bill Company Loans", Description = "Vendor Bill Company Loans" });
                permissions.Add(new Permission() { Id = 6079, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6078, Name = "Add Vendor Bill Company Loans", Description = "Allow User To Add Vendor Bill Company Loans" });
                permissions.Add(new Permission() { Id = 6080, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6078, Name = "Edit Vendor Bill Company Loans", Description = "Allow User To Edit Vendor Bill Company Loans" });
                permissions.Add(new Permission() { Id = 6081, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6078, Name = "View Vendor Bill Company Loans", Description = "Allow User To View Vendor Bill Company Loans" });
                permissions.Add(new Permission() { Id = 6082, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6078, Name = "Can Create Vendor Bill From Company Loans", Description = "Allow User to Create Vendor Bill From Company Loans" });

                permissions.Add(new Permission() { Id = 6083, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6046, Name = "Export Company Loans Reports", Description = "Allow User to Export Company Loans Reports" });
            }


            {
                permissions.Add(new Permission() { Id = 8499, Added = System.DateTime.Now, LastModified = System.DateTime.Now, Name = "Rentals", Description = "Rentals" });

                //Rental Contracts
                permissions.Add(new Permission() { Id = 8500, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8499, Name = "Rental Contracts", Description = "Rental Contracts" });

                permissions.Add(new Permission() { Id = 8501, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8500, Name = "Add Rental Contracts", Description = "Allow User To Add Rental Contracts" });//Done
                permissions.Add(new Permission() { Id = 8502, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8500, Name = "Edit Rental Contracts", Description = "Allow User To Edit Rental Contracts" });//Done
                permissions.Add(new Permission() { Id = 8503, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8500, Name = "View Rental Contracts", Description = "Allow User To View Rental Contracts" });//Done
                permissions.Add(new Permission() { Id = 8504, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8500, Name = "List of Rental Contracts", Description = "Allow User To View List of Rental Contracts" });//Done
                permissions.Add(new Permission() { Id = 8505, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8500, Name = "Add Rental Contract Status", Description = "Allow User To Add new status for Rental Contracts" });//Done
                permissions.Add(new Permission() { Id = 8506, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8500, Name = "Add Rental Contracts without Approval", Description = "Allow User To Add New Rental Contracts without Approval" });
                permissions.Add(new Permission() { Id = 8507, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8500, Name = "View InActive Rental Contracts", Description = "Allow User To View InActive Rental Contracts" });
                permissions.Add(new Permission() { Id = 8508, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8500, Name = "Close Rental Contracts", Description = "Allow User To Close Rental Contracts" });
                permissions.Add(new Permission() { Id = 8509, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8500, Name = "Edit Rental Contract Status", Description = "Allow User To Edit Rental Contracts Status" });//Done
                permissions.Add(new Permission() { Id = 8510, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8500, Name = "View InActive Rental Contracts Statuses", Description = "Allow User To View InActive Rental Contracts Statuses" });

                permissions.Add(new Permission() { Id = 8511, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8500, Name = "Close Rental Contracts without Approval", Description = "Allow User To Close Rental Contracts without Approval" });
                permissions.Add(new Permission() { Id = 8512, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8500, Name = "Edit Unapproved Rental Contracts", Description = "Allow User To Edit Unapproved Rental Contracts without Approval" });

                permissions.Add(new Permission() { Id = 8513, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8500, Name = "Reviewer Level 1 Rental Contracts", Description = "Allow User to mark Rental Contracts as Reviewed once" });
                permissions.Add(new Permission() { Id = 8514, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8500, Name = "Reviewer Level 2 Rental Contracts", Description = "Allow User To Mark Rental Contracts as Reviewed and move it to Approved List" });
                permissions.Add(new Permission() { Id = 8515, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8500, Name = "Approver for Closing Rental Contracts", Description = "Allow User To Close Rental Contracts which is in pending state" });
                permissions.Add(new Permission() { Id = 8516, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8500, Name = "Approver for new added Rental Contracts", Description = "Allow User To Approve Rental Contracts which is in pending state" });
                permissions.Add(new Permission() { Id = 8517, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8500, Name = "Edit closed Rental Contracts", Description = "Allow User To Edit Closed Rental Contracts" });
                permissions.Add(new Permission() { Id = 8518, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8500, Name = "View(Pending for closing) Rental Contracts List", Description = "Allow User To View List of All(Pending for Closing) Rental Contracts mapped to his Department" });
                permissions.Add(new Permission() { Id = 8519, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8500, Name = "View(Pending for Approval) Rental Contracts List", Description = "Allow User To View List of All(Pending for Approval) Rental Contracts mapped to his Department" });
                permissions.Add(new Permission() { Id = 8520, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8500, Name = "Edit Creation Date of Rental Contracts", Description = "Allow User To Edit Creation Date of Rental Contracts" });
                permissions.Add(new Permission() { Id = 8521, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8500, Name = "Attach a file with Rental Contracts", Description = "Allow User To Attach a file with Rental Contracts" });
                permissions.Add(new Permission() { Id = 8522, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8500, Name = "View list of attached files with Rental Contracts", Description = "Allow User To View list of attached files with Rental Contracts" });
                permissions.Add(new Permission() { Id = 8523, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8500, Name = "Mark as Void Rental Contracts", Description = "Allow User To Mark as Void Rental Contracts" });
                permissions.Add(new Permission() { Id = 8524, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8500, Name = "Unmark Void Rental Contracts", Description = "Allow User To unmark as Void Rental Contracts" });
                permissions.Add(new Permission() { Id = 8525, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8500, Name = "View list of Void Rental Contracts", Description = "View list of Void Rental Contracts" });

                permissions.Add(new Permission() { Id = 8526, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8500, Name = "ReApprover for new added Rental Contracts", Description = "Allow User To ReApprove Rental Contracts which is in pending state" });
                permissions.Add(new Permission() { Id = 8527, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8500, Name = "Add Rental Contracts without ReApproval", Description = "Allow User To Add New Rental Contracts without ReApproval" });
                permissions.Add(new Permission() { Id = 8528, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8500, Name = "View(Pending for ReApproval) Rental Contracts List", Description = "Allow User To View List of All(Pending for ReApproval) Rental Contracts mapped to his Department" });

                //Assets
                permissions.Add(new Permission() { Id = 8549, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = null, Name = "Assets", Description = "Assets" });
                permissions.Add(new Permission() { Id = 8550, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8549, Name = "Asset Statuses", Description = "Asset Statuses" });
                permissions.Add(new Permission() { Id = 8551, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8550, Name = "Add New Asset Status", Description = "Allow User To Add New Asset Status" });
                permissions.Add(new Permission() { Id = 8552, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8550, Name = "Update Asset Status", Description = "Allow User To Update Asset Status" });
                permissions.Add(new Permission() { Id = 8553, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8550, Name = "View List of Asset Status", Description = "Allow User To View List of Asset Status" });

                permissions.Add(new Permission() { Id = 8554, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8549, Name = "Add New Assets", Description = "Allow User To Add Assets" });//Done
                permissions.Add(new Permission() { Id = 8555, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8549, Name = "Edit Assets", Description = "Allow User To Edit Assets" });//Done
                permissions.Add(new Permission() { Id = 8556, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8549, Name = "View Assets", Description = "Allow User To View Assets" });//Done
                permissions.Add(new Permission() { Id = 8557, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8549, Name = "View List of Assets", Description = "Allow User To View List of Assets" });//Done
                permissions.Add(new Permission() { Id = 8558, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8549, Name = "Add Assets without Approval", Description = "Allow User To Add New Assets without Approval" });
                permissions.Add(new Permission() { Id = 8559, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8549, Name = "View InActive Assets", Description = "Allow User To View InActive Assets" });
                permissions.Add(new Permission() { Id = 8560, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8549, Name = "Close Assets", Description = "Allow User To Close Assets" });
                permissions.Add(new Permission() { Id = 8561, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8549, Name = "View InActive Asset Statuses", Description = "Allow User To View InActive Assets Statuses" });

                permissions.Add(new Permission() { Id = 8562, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8549, Name = "Close Assets without Approval", Description = "Allow User To Close Assets without Approval" });
                permissions.Add(new Permission() { Id = 8563, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8549, Name = "Edit Unapproved Assets", Description = "Allow User To Edit Unapproved Assets without Approval" });

                permissions.Add(new Permission() { Id = 8564, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8549, Name = "Reviewer Level 1 Assets", Description = "Allow User to mark Assets as Reviewed once" });
                permissions.Add(new Permission() { Id = 8565, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8549, Name = "Reviewer Level 2 Assets", Description = "Allow User To Mark Assets as Reviewed and move it to Approved List" });
                permissions.Add(new Permission() { Id = 8566, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8549, Name = "Approver for Closing Assets", Description = "Allow User To Close Assets which is in pending state" });
                permissions.Add(new Permission() { Id = 8567, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8549, Name = "Approver for new added Assets", Description = "Allow User To Approve Assets which is in pending state" });
                permissions.Add(new Permission() { Id = 8568, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8549, Name = "Edit closed Assets", Description = "Allow User To Edit Closed Assets" });
                permissions.Add(new Permission() { Id = 8569, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8549, Name = "View(Pending for closing) Assets List", Description = "Allow User To View List of All(Pending for Closing) Assets mapped to his Department" });
                permissions.Add(new Permission() { Id = 8570, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8549, Name = "View(Pending for Approval) Assets List", Description = "Allow User To View List of All(Pending for Approval) Assets mapped to his Department" });
                permissions.Add(new Permission() { Id = 8571, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8549, Name = "Edit Creation Date of Assets", Description = "Allow User To Edit Creation Date of Assets" });
                permissions.Add(new Permission() { Id = 8572, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8549, Name = "Attach a file with Assets", Description = "Allow User To Attach a file with Assets" });
                permissions.Add(new Permission() { Id = 8573, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8549, Name = "View list of attached files with Assets", Description = "Allow User To View list of attached files with Assets" });
                permissions.Add(new Permission() { Id = 8574, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8549, Name = "Mark as Void Assets", Description = "Allow User To Mark as Void Assets" });
                permissions.Add(new Permission() { Id = 8575, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8549, Name = "Unmark Void Assets", Description = "Allow User To unmark as Void Assets" });
                permissions.Add(new Permission() { Id = 8576, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8549, Name = "View list of Void Assets", Description = "View list of Void Assets" });

                permissions.Add(new Permission() { Id = 8577, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8549, Name = "ReApprover for new added Assets", Description = "Allow User To ReApprove Assets which is in pending state" });
                permissions.Add(new Permission() { Id = 8578, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8549, Name = "Add Assets without ReApproval", Description = "Allow User To Add New Assets without ReApproval" });
                permissions.Add(new Permission() { Id = 8579, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8549, Name = "View(Pending for ReApproval) Assets List", Description = "Allow User To View List of All(Pending for ReApproval) Assets mapped to his Department" });

                permissions.Add(new Permission() { Id = 8580, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8549, Name = "Export Assets Reports", Description = "Allow User To Export Assets Reports" });

                //Rental Orders
                permissions.Add(new Permission() { Id = 8600, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8499, Name = "Rental Orders", Description = "Rental Orders" });

                permissions.Add(new Permission() { Id = 8601, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8600, Name = "Add Rental Orders", Description = "Allow User To Add Rental Orders" });//Done
                permissions.Add(new Permission() { Id = 8602, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8600, Name = "Edit Rental Orders", Description = "Allow User To Edit Rental Orders" });//Done
                permissions.Add(new Permission() { Id = 8603, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8600, Name = "View Rental Orders", Description = "Allow User To View Rental Orders" });//Done
                permissions.Add(new Permission() { Id = 8604, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8600, Name = "List of Rental Orders", Description = "Allow User To View List of Rental Orders" });//Done
                permissions.Add(new Permission() { Id = 8606, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8600, Name = "Add Rental Orders without Approval", Description = "Allow User To Add New Rental Orders without Approval" });
                permissions.Add(new Permission() { Id = 8607, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8600, Name = "View InActive Rental Orders", Description = "Allow User To View InActive Rental Orders" });
                permissions.Add(new Permission() { Id = 8608, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8600, Name = "Close Rental Orders", Description = "Allow User To Close Rental Orders" });
                permissions.Add(new Permission() { Id = 8610, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8600, Name = "View InActive Rental Orders Statuses", Description = "Allow User To View InActive Rental Orders Statuses" });

                permissions.Add(new Permission() { Id = 8611, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8600, Name = "Close Rental Orders without Approval", Description = "Allow User To Close Rental Orders without Approval" });
                permissions.Add(new Permission() { Id = 8612, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8600, Name = "Edit Unapproved Rental Orders", Description = "Allow User To Edit Unapproved Rental Orders without Approval" });

                permissions.Add(new Permission() { Id = 8613, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8600, Name = "Reviewer Level 1 Rental Orders", Description = "Allow User to mark Rental Orders as Reviewed once" });
                permissions.Add(new Permission() { Id = 8614, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8600, Name = "Reviewer Level 2 Rental Orders", Description = "Allow User To Mark Rental Orders as Reviewed and move it to Approved List" });
                permissions.Add(new Permission() { Id = 8615, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8600, Name = "Approver for Closing Rental Orders", Description = "Allow User To Close Rental Orders which is in pending state" });
                permissions.Add(new Permission() { Id = 8616, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8600, Name = "Approver for new added Rental Orders", Description = "Allow User To Approve Rental Orders which is in pending state" });
                permissions.Add(new Permission() { Id = 8617, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8600, Name = "Edit closed Rental Orders", Description = "Allow User To Edit Closed Rental Orders" });
                permissions.Add(new Permission() { Id = 8618, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8600, Name = "View(Pending for closing) Rental Orders List", Description = "Allow User To View List of All(Pending for Closing) Rental Orders mapped to his Department" });
                permissions.Add(new Permission() { Id = 8619, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8600, Name = "View(Pending for Approval) Rental Orders List", Description = "Allow User To View List of All(Pending for Approval) Rental Orders mapped to his Department" });
                permissions.Add(new Permission() { Id = 8620, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8600, Name = "Edit Creation Date of Rental Orders", Description = "Allow User To Edit Creation Date of Rental Orders" });
                permissions.Add(new Permission() { Id = 8621, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8600, Name = "Attach a file with Rental Orders", Description = "Allow User To Attach a file with Rental Orders" });
                permissions.Add(new Permission() { Id = 8622, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8600, Name = "View list of attached files with Rental Orders", Description = "Allow User To View list of attached files with Rental Orders" });
                permissions.Add(new Permission() { Id = 8623, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8600, Name = "Mark as Void Rental Orders", Description = "Allow User To Mark as Void Rental Orders" });
                permissions.Add(new Permission() { Id = 8624, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8600, Name = "Unmark Void Rental Orders", Description = "Allow User To unmark as Void Rental Orders" });
                permissions.Add(new Permission() { Id = 8625, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8600, Name = "View list of Void Rental Orders", Description = "View list of Void Rental Orders" });

                permissions.Add(new Permission() { Id = 8626, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8600, Name = "ReApprover for new added Rental Orders", Description = "Allow User To ReApprove Rental Orders which is in pending state" });
                permissions.Add(new Permission() { Id = 8627, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8600, Name = "Add Rental Orders without ReApproval", Description = "Allow User To Add New Rental Orders without ReApproval" });
                permissions.Add(new Permission() { Id = 8628, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8600, Name = "View(Pending for ReApproval) Rental Orders List", Description = "Allow User To View List of All(Pending for ReApproval) Rental Orders mapped to his Department" });
                permissions.Add(new Permission() { Id = 8629, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8600, Name = "Can Create Rental Order From Rental Contract", Description = "Allow User To Create Rental Order From Rental Contract" });

                permissions.Add(new Permission() { Id = 8650, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8600, Name = "Rental Order Statuses", Description = "Asset Statuses" });
                permissions.Add(new Permission() { Id = 8651, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8650, Name = "Add New Rental Order Status", Description = "Allow User To Add New Asset Status" });
                permissions.Add(new Permission() { Id = 8652, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8650, Name = "Update Rental Order Status", Description = "Allow User To Update Asset Status" });
                permissions.Add(new Permission() { Id = 8653, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8650, Name = "View List of Rental Order Status", Description = "Allow User To View List of Asset Status" });


                //Tenants
                permissions.Add(new Permission() { Id = 8700, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8499, Name = "Tenants", Description = "Tenants" });
                permissions.Add(new Permission() { Id = 8701, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8700, Name = "Tenant Statuses", Description = "Tenant Statuses" });
                permissions.Add(new Permission() { Id = 8702, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8701, Name = "Add New Tenant Status", Description = "Allow User To Add New Tenant Status" });
                permissions.Add(new Permission() { Id = 8703, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8701, Name = "Update Tenant Status", Description = "Allow User To Update Tenant Status" });
                permissions.Add(new Permission() { Id = 8704, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8701, Name = "View List of Tenant Status", Description = "Allow User To View List of Tenant Status" });

                permissions.Add(new Permission() { Id = 8705, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8700, Name = "Add New Tenants", Description = "Allow User To Add Tenants" });//Done
                permissions.Add(new Permission() { Id = 8706, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8700, Name = "Edit Tenants", Description = "Allow User To Edit Tenants" });//Done
                permissions.Add(new Permission() { Id = 8707, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8700, Name = "View Tenants", Description = "Allow User To View Tenants" });//Done
                permissions.Add(new Permission() { Id = 8708, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8700, Name = "View List of Tenants", Description = "Allow User To View List of Tenants" });//Done
                permissions.Add(new Permission() { Id = 8709, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8700, Name = "Add Tenants without Approval", Description = "Allow User To Add New Tenants without Approval" });
                permissions.Add(new Permission() { Id = 8710, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8700, Name = "View InActive Tenants", Description = "Allow User To View InActive Tenants" });
                permissions.Add(new Permission() { Id = 8711, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8700, Name = "Close Tenants", Description = "Allow User To Close Tenants" });
                permissions.Add(new Permission() { Id = 8712, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8700, Name = "View InActive Tenant Statuses", Description = "Allow User To View InActive Assets Statuses" });

                permissions.Add(new Permission() { Id = 8713, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8700, Name = "Close Tenants without Approval", Description = "Allow User To Close Tenants without Approval" });
                permissions.Add(new Permission() { Id = 8714, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8700, Name = "Edit Unapproved Tenants", Description = "Allow User To Edit Unapproved Tenants without Approval" });

                permissions.Add(new Permission() { Id = 8715, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8700, Name = "Reviewer Level 1 Tenants", Description = "Allow User to mark Tenants as Reviewed once" });
                permissions.Add(new Permission() { Id = 8716, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8700, Name = "Reviewer Level 2 Tenants", Description = "Allow User To Mark Tenants as Reviewed and move it to Approved List" });
                permissions.Add(new Permission() { Id = 8717, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8700, Name = "Approver for Closing Tenants", Description = "Allow User To Close Tenants which is in pending state" });
                permissions.Add(new Permission() { Id = 8718, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8700, Name = "Approver for new added Tenants", Description = "Allow User To Approve Tenants which is in pending state" });
                permissions.Add(new Permission() { Id = 8719, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8700, Name = "Edit closed Tenants", Description = "Allow User To Edit Closed Tenants" });
                permissions.Add(new Permission() { Id = 8720, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8700, Name = "View(Pending for closing) Tenants List", Description = "Allow User To View List of All(Pending for Closing) Tenants mapped to his Department" });
                permissions.Add(new Permission() { Id = 8721, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8700, Name = "View(Pending for Approval) Tenants List", Description = "Allow User To View List of All(Pending for Approval) Tenants mapped to his Department" });
                permissions.Add(new Permission() { Id = 8722, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8700, Name = "Edit Creation Date of Tenants", Description = "Allow User To Edit Creation Date of Tenants" });
                permissions.Add(new Permission() { Id = 8723, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8700, Name = "Attach a file with Tenants", Description = "Allow User To Attach a file with Tenants" });
                permissions.Add(new Permission() { Id = 8724, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8700, Name = "View list of attached files with Tenants", Description = "Allow User To View list of attached files with Tenants" });
                permissions.Add(new Permission() { Id = 8725, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8700, Name = "Mark as Void Tenants", Description = "Allow User To Mark as Void Tenants" });
                permissions.Add(new Permission() { Id = 8726, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8700, Name = "Unmark Void Tenants", Description = "Allow User To unmark as Void Tenants" });
                permissions.Add(new Permission() { Id = 8727, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8700, Name = "View list of Void Tenants", Description = "View list of Void Tenants" });

                permissions.Add(new Permission() { Id = 8728, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8700, Name = "ReApprover for new added Tenants", Description = "Allow User To ReApprove Tenants which is in pending state" });
                permissions.Add(new Permission() { Id = 8729, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8700, Name = "Add Tenants without ReApproval", Description = "Allow User To Add New Tenants without ReApproval" });
                permissions.Add(new Permission() { Id = 8730, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8700, Name = "View(Pending for ReApproval) Tenants List", Description = "Allow User To View List of All(Pending for ReApproval) Tenants mapped to his Department" });


                //Rental Invoices
                permissions.Add(new Permission() { Id = 8900, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8499, Name = "Rental Invoices", Description = "Rental Invoices" });

                permissions.Add(new Permission() { Id = 8901, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8900, Name = "Add Rental Invoices", Description = "Allow User To Add Rental Invoices" });//Done
                permissions.Add(new Permission() { Id = 8902, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8900, Name = "Edit Rental Invoices", Description = "Allow User To Edit Rental Invoices" });//Done
                permissions.Add(new Permission() { Id = 8903, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8900, Name = "View Rental Invoices", Description = "Allow User To View Rental Invoices" });//Done
                permissions.Add(new Permission() { Id = 8904, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8900, Name = "List of Rental Invoices", Description = "Allow User To View List of Rental Invoices" });//Done
                permissions.Add(new Permission() { Id = 8906, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8900, Name = "Add Rental Invoices without Approval", Description = "Allow User To Add New Rental Invoices without Approval" });
                permissions.Add(new Permission() { Id = 8907, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8900, Name = "View InActive Rental Invoices", Description = "Allow User To View InActive Rental Invoices" });
                permissions.Add(new Permission() { Id = 8908, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8900, Name = "Close Rental Invoices", Description = "Allow User To Close Rental Invoices" });
                permissions.Add(new Permission() { Id = 8910, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8900, Name = "View InActive Rental Invoices Statuses", Description = "Allow User To View InActive Rental Invoices Statuses" });

                permissions.Add(new Permission() { Id = 8911, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8900, Name = "Close Rental Invoices without Approval", Description = "Allow User To Close Rental Invoices without Approval" });
                permissions.Add(new Permission() { Id = 8912, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8900, Name = "Edit Unapproved Rental Invoices", Description = "Allow User To Edit Unapproved Rental Invoices without Approval" });

                permissions.Add(new Permission() { Id = 8913, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8900, Name = "Reviewer Level 1 Rental Invoices", Description = "Allow User to mark Rental Invoices as Reviewed once" });
                permissions.Add(new Permission() { Id = 8914, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8900, Name = "Reviewer Level 2 Rental Invoices", Description = "Allow User To Mark Rental Invoices as Reviewed and move it to Approved List" });
                permissions.Add(new Permission() { Id = 8915, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8900, Name = "Approver for Closing Rental Invoices", Description = "Allow User To Close Rental Invoices which is in pending state" });
                permissions.Add(new Permission() { Id = 8916, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8900, Name = "Approver for new added Rental Invoices", Description = "Allow User To Approve Rental Invoices which is in pending state" });
                permissions.Add(new Permission() { Id = 8917, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8900, Name = "Edit closed Rental Invoices", Description = "Allow User To Edit Closed Rental Invoices" });
                permissions.Add(new Permission() { Id = 8918, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8900, Name = "View(Pending for closing) Rental Invoices List", Description = "Allow User To View List of All(Pending for Closing) Rental Invoices mapped to his Department" });
                permissions.Add(new Permission() { Id = 8919, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8900, Name = "View(Pending for Approval) Rental Invoices List", Description = "Allow User To View List of All(Pending for Approval) Rental Invoices mapped to his Department" });
                permissions.Add(new Permission() { Id = 8920, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8900, Name = "Edit Creation Date of Rental Invoices", Description = "Allow User To Edit Creation Date of Rental Invoices" });
                permissions.Add(new Permission() { Id = 8921, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8900, Name = "Attach a file with Rental Invoices", Description = "Allow User To Attach a file with Rental Invoices" });
                permissions.Add(new Permission() { Id = 8922, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8900, Name = "View list of attached files with Rental Invoices", Description = "Allow User To View list of attached files with Rental Invoices" });
                permissions.Add(new Permission() { Id = 8923, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8900, Name = "Mark as Void Rental Invoices", Description = "Allow User To Mark as Void Rental Invoices" });
                permissions.Add(new Permission() { Id = 8924, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8900, Name = "Unmark Void Rental Invoices", Description = "Allow User To unmark as Void Rental Invoices" });
                permissions.Add(new Permission() { Id = 8925, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8900, Name = "View list of Void Rental Invoices", Description = "View list of Void Rental Invoices" });

                permissions.Add(new Permission() { Id = 8926, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8900, Name = "ReApprover for new added Rental Invoices", Description = "Allow User To ReApprove Rental Invoices which is in pending state" });
                permissions.Add(new Permission() { Id = 8927, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8900, Name = "Add Rental Invoices without ReApproval", Description = "Allow User To Add New Rental Invoices without ReApproval" });
                permissions.Add(new Permission() { Id = 8928, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8900, Name = "View(Pending for ReApproval) Rental Invoices List", Description = "Allow User To View List of All(Pending for ReApproval) Rental Invoices mapped to his Department" });
                permissions.Add(new Permission() { Id = 8929, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8900, Name = "Can Create Rental Invoice From Rental Order", Description = "Allow User To Create Rental Invoice From Rental Order" });

                permissions.Add(new Permission() { Id = 8950, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8900, Name = "Rental Invoice Statuses", Description = " Rental Invoice Statuses" });
                permissions.Add(new Permission() { Id = 8951, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8950, Name = "Add New Rental Invoice Status", Description = "Allow User To Add New Rental Invoice Status" });
                permissions.Add(new Permission() { Id = 8952, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8950, Name = "Update Rental Invoice Status", Description = "Allow User To Update Rental Invoice Status" });
                permissions.Add(new Permission() { Id = 8953, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8950, Name = "View List of Rental Invoice Status", Description = "Allow User To View List of Rental Invoices Status" });
            }


            ///Employment Salary Permissions
            ///
            {
                permissions.Add(new Permission() { Id = 6100, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 3100, Name = "Payroll", Description = "Payroll" });
                permissions.Add(new Permission() { Id = 6101, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6100, Name = "Employment Salary", Description = "Employment Salary" });
                permissions.Add(new Permission() { Id = 6102, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6101, Name = "Add Employment Salary", Description = "Allow User To Add Employment Salary" });
                permissions.Add(new Permission() { Id = 6103, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6101, Name = "Edit Employment Salary", Description = "Allow User To Edit Employment Salary" });
                permissions.Add(new Permission() { Id = 6104, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6101, Name = "View Employment Salary", Description = "Allow User To View Employment Salary" });
                permissions.Add(new Permission() { Id = 6105, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6101, Name = "List of Employment Salary", Description = "Allow User To View List of Employment Salary" });
                permissions.Add(new Permission() { Id = 6106, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6101, Name = "Add Employment Salary without Approval", Description = "Allow User To Add New Employment Salary without Approval" });
                permissions.Add(new Permission() { Id = 6107, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6101, Name = "Approver for new added Employment Salary", Description = "Allow User To Approve Employment Salary which is in pending state" });
            }

            ///Tasks Permissions
            ///
            {
                permissions.Add(new Permission() { Id = 6200, Added = System.DateTime.Now, LastModified = System.DateTime.Now, Name = "Tasks", Description = "Tasks" });

                permissions.Add(new Permission() { Id = 6199, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6200, Name = "Task Statuses", Description = "Task Statuses" });

                permissions.Add(new Permission() { Id = 6201, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6200, Name = "Add User Task", Description = "Allow User to Add User Task" });

                permissions.Add(new Permission() { Id = 6202, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6200, Name = "View User Task", Description = "Allow User To View User Task" });
                permissions.Add(new Permission() { Id = 6203, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6200, Name = "Edit User Task", Description = "Allow User To Edit User Task" });
                permissions.Add(new Permission() { Id = 6204, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6200, Name = "List of User Tasks", Description = "Allow User To View List of User Tasks" });
                permissions.Add(new Permission() { Id = 6205, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6199, Name = "Add User Task Status", Description = "Allow User To Add new status for User Tasks" });//Done
                permissions.Add(new Permission() { Id = 6207, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6200, Name = "View Closed User Tasks", Description = "Allow User To View InActive User Tasks" });
                permissions.Add(new Permission() { Id = 6208, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6200, Name = "Close User Tasks", Description = "Allow User To Close User Tasks" });
                permissions.Add(new Permission() { Id = 6209, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6199, Name = "Edit User Tasks Status", Description = "Allow User To Edit User Tasks Status" });//Done
                permissions.Add(new Permission() { Id = 6210, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6200, Name = "View InActive User Tasks Statuses", Description = "Allow User To View InActive User Tasks Statuses" });

                permissions.Add(new Permission() { Id = 6211, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6200, Name = "Edit Creation Date of User Tasks", Description = "Allow User To Edit Creation Date of User Tasks" });
                permissions.Add(new Permission() { Id = 6212, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6200, Name = "Attach a file with User Tasks", Description = "Allow User To Attach a file with User Tasks" });
                permissions.Add(new Permission() { Id = 6213, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6200, Name = "View list of attached files with User Tasks", Description = "Allow User To View list of attached files with User Tasks" });
                permissions.Add(new Permission() { Id = 6214, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6200, Name = "Mark as Void User Tasks", Description = "Allow User To Mark as Void User Tasks" });
                permissions.Add(new Permission() { Id = 6215, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6200, Name = "Unmark Void User Tasks", Description = "Allow User To unmark as Void User Tasks" });
                permissions.Add(new Permission() { Id = 6216, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6200, Name = "View list of Void User Tasks", Description = "View list of Void User Tasks" });

                permissions.Add(new Permission() { Id = 6217, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6200, Name = "Close User Tasks without Approval", Description = "Allow User To Close User Tasks without Approval" });
                permissions.Add(new Permission() { Id = 6218, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6200, Name = "Approver for Closing User Task", Description = "Allow User To Close User Tasks which is in pending state" });
                permissions.Add(new Permission() { Id = 6219, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6200, Name = "Can Add Tracking Details", Description = "Allow User to Add Tracking Details" });
                permissions.Add(new Permission() { Id = 6220, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6200, Name = "By Pass Permissions", Description = "Allow User By Pass Permissions" });
                permissions.Add(new Permission() { Id = 6221, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6220, Name = "Can View All Tasks for Allowed Departments (Member)", Description = "Allow User to View All Tasks for Allowed Departments" });
                permissions.Add(new Permission() { Id = 6222, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6220, Name = "Can View All Tasks for All Departments (Member/Non Member)", Description = "Allow User to View All Tasks for All Departments (Member/Non Member)" });
                permissions.Add(new Permission() { Id = 6223, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6220, Name = "Can Add Users in Tasks (Not creator of the Task)", Description = "Allow User to Add Users in Tasks (Not creator of the Task)" });

                permissions.Add(new Permission() { Id = 6224, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6201, Name = "Add User Task for Sale Order", Description = "Allow User to Add User Task for Sale Order" });
                permissions.Add(new Permission() { Id = 6225, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6201, Name = "Add User Task for Purchase Order", Description = "Allow User to Add User Task for Purchase Order" });
                permissions.Add(new Permission() { Id = 6226, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6201, Name = "Add User Task for Sale Invoice", Description = "Allow User to Add User Task for Sale Invoice" });
                permissions.Add(new Permission() { Id = 6227, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6201, Name = "Add User Task for Offer", Description = "Allow User to Add User Task for Offer" });
                permissions.Add(new Permission() { Id = 6228, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6201, Name = "Add New Task Not referring to Any Module", Description = "Allow User to Add New Task Not referring to Any Module" });
                permissions.Add(new Permission() { Id = 6229, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6200, Name = "Can View Non-Departmental Users in Tasks", Description = "Allow User to View Non-Departmental Users in Tasks" });

                permissions.Add(new Permission() { Id = 6230, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6200, Name = "Approver for new Added Task", Description = "Allow User to Approve Task which is in Pending State" });
                permissions.Add(new Permission() { Id = 6231, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6200, Name = "Reviewer Level 1 Task", Description = "Allow User to mark Task as Reviewed once" });
                permissions.Add(new Permission() { Id = 6232, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6200, Name = "Reviewer Level 2 Task", Description = "Allow User To Mark Task as Reviewed and move it to Approved List" });

                permissions.Add(new Permission() { Id = 6233, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6200, Name = "Can Add Efficiency Points in Tasks", Description = "Allow User To Add Efficiency Points in Tasks" });

                permissions.Add(new Permission() { Id = 6234, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6200, Name = "Tax Tasks", Description = "Tax Tasks" });
                permissions.Add(new Permission() { Id = 6235, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6234, Name = "Add Tax Tasks", Description = "Allow User To Add Tax Tasks" });
                permissions.Add(new Permission() { Id = 6236, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6234, Name = "View Tax Tasks", Description = "Allow User To View Tax Tasks" });
                permissions.Add(new Permission() { Id = 6237, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6234, Name = "Edit Tax Tasks", Description = "Allow User To Edit Tax Tasks" });
                permissions.Add(new Permission() { Id = 6246, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6234, Name = "Edit Tax Task Under Approval", Description = "Allow User To Edit Tax Task Under Approval" });

                permissions.Add(new Permission() { Id = 6238, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6200, Name = "Edit and Save Task Execution Details", Description = "Allow User To Edit and Save Task Execution Details" });
                permissions.Add(new Permission() { Id = 6239, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6200, Name = "Export Task Report", Description = "Allow User To Export Task Report" });

                permissions.Add(new Permission() { Id = 6240, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6200, Name = "Checklist", Description = "Allow User To Add Checklist" });
                permissions.Add(new Permission() { Id = 6241, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6240, Name = "Edit Received Quantity in Checklist", Description = "Allow User To Edit Received Quantity in Checklist" });
                permissions.Add(new Permission() { Id = 6242, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6240, Name = "Edit Received Weight in Checklist", Description = "Allow User To Edit Received Weight in Checklist" });
                permissions.Add(new Permission() { Id = 6243, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6240, Name = "Edit Packing Dimensions in Checklist", Description = "Allow User To Edit Packing Dimensions in Checklist" });
                permissions.Add(new Permission() { Id = 6244, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6240, Name = "Edit Packing Style in Checklist", Description = "Allow User To Edit Packing Style in Checklist" });

                permissions.Add(new Permission() { Id = 6245, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6200, Name = "Edit User Task Under Approval", Description = "Allow User To Edit User Task Under Approval" });
            }

            ///Traveling Records Permissions
            ///
            {
                permissions.Add(new Permission() { Id = 7000, Added = System.DateTime.Now, LastModified = System.DateTime.Now, Name = "Docs", Description = "Docs" });

                permissions.Add(new Permission() { Id = 7001, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 7000, Name = "Traveling Record", Description = "Traveling Record" });
                permissions.Add(new Permission() { Id = 7002, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 7001, Name = "Add Traveling Record", Description = "Allow User To Add Traveling Record" });
                permissions.Add(new Permission() { Id = 7003, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 7001, Name = "Edit Traveling Record", Description = "Allow User To Edit Traveling Record" });
                permissions.Add(new Permission() { Id = 7004, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 7001, Name = "View Traveling Record", Description = "Allow User To View Traveling Record" });
                permissions.Add(new Permission() { Id = 7005, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 7001, Name = "List of Traveling Record", Description = "Allow User To View List of Traveling Record" });
                permissions.Add(new Permission() { Id = 7006, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 7001, Name = "Add Traveling Status", Description = "Allow User To Add new status for Traveling Record" });
                permissions.Add(new Permission() { Id = 7007, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 7001, Name = "Add Traveling Record without Approval", Description = "Allow User To Add New Traveling Record without Approval" });
                permissions.Add(new Permission() { Id = 7008, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 7001, Name = "View InActive Traveling Record", Description = "Allow User To View InActive Traveling Record" });
                permissions.Add(new Permission() { Id = 7009, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 7001, Name = "Close Traveling Record", Description = "Allow User To Close Traveling Record" });
                permissions.Add(new Permission() { Id = 7010, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 7001, Name = "Edit Traveling Status", Description = "Allow User To Edit Traveling Status" });
                permissions.Add(new Permission() { Id = 7011, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 7001, Name = "View InActive Traveling Statuses", Description = "Allow User To View InActive Traveling Statuses" });

                permissions.Add(new Permission() { Id = 7012, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 7001, Name = "Close Traveling Record without Approval", Description = "Allow User To Close Traveling Record without Approval" });
                permissions.Add(new Permission() { Id = 7013, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 7001, Name = "Edit Unapproved Traveling Record", Description = "Allow User To Edit Unapproved Traveling Record" });

                permissions.Add(new Permission() { Id = 7015, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 7001, Name = "Reviewer Level 1 Traveling Record", Description = "Allow User to mark Traveling Record as Reviewed once" });
                permissions.Add(new Permission() { Id = 7016, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 7001, Name = "Reviewer Level 2 Traveling Record", Description = "Allow User To Mark Traveling Record as Reviewed and move it to Approved List" });
                permissions.Add(new Permission() { Id = 7017, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 7001, Name = "Approver for Closing Traveling Record", Description = "Allow User To Close Traveling Record which is in pending state" });
                permissions.Add(new Permission() { Id = 7018, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 7001, Name = "Approver for new added Traveling Record", Description = "Allow User To Approve Traveling Record which is in pending state" });
                permissions.Add(new Permission() { Id = 7019, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 7001, Name = "Edit closed Traveling Record", Description = "Allow User To Edit Closed Traveling Record" });
                permissions.Add(new Permission() { Id = 7020, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 7001, Name = "View(Pending for closing) Traveling Record", Description = "Allow User To View List of All(Pending for Closing) Traveling Record mapped to his Department" });
                permissions.Add(new Permission() { Id = 7021, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 7001, Name = "View(Pending for Approval) Traveling Record List", Description = "Allow User To View List of All(Pending for Approval) Traveling Record mapped to his Department" });
                permissions.Add(new Permission() { Id = 7022, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 7001, Name = "Edit Creation Date of Traveling Record", Description = "Allow User To Edit Creation Date of Traveling Record" });
                permissions.Add(new Permission() { Id = 7023, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 7001, Name = "Attach a file with Traveling Record", Description = "Allow User To Attach a file with Traveling Record" });
                permissions.Add(new Permission() { Id = 7024, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 7001, Name = "View list of attached files with Traveling Record", Description = "Allow User To View list of attached files with Traveling Record" });
                permissions.Add(new Permission() { Id = 7025, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 7001, Name = "Mark as Void Traveling Record", Description = "Allow User To Mark as Void Traveling Record" });
                permissions.Add(new Permission() { Id = 7026, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 7001, Name = "Unmark Void Traveling Record", Description = "Allow User To unmark as Void Traveling Record" });
                permissions.Add(new Permission() { Id = 7027, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 7001, Name = "View list of Void Traveling Record", Description = "View list of Void Traveling Record" });

                //New Permissions
                permissions.Add(new Permission() { Id = 7028, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 7001, Name = "ReApprover for new added Traveling Record", Description = "Allow User To ReApprove Traveling Record which is in pending state" });
                permissions.Add(new Permission() { Id = 7029, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 7001, Name = "Add Traveling Record without ReApproval", Description = "Allow User To Add New Traveling Record without ReApproval" });
                permissions.Add(new Permission() { Id = 7030, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 7001, Name = "View(Pending for ReApproval) Traveling Record List", Description = "Allow User To View List of All(Pending for ReApproval) Traveling Record mapped to his Department" });
                permissions.Add(new Permission() { Id = 7031, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 7001, Name = "View List of Traveling Statuses", Description = "Allow User To View List of Traveling Statuses" });



            }



            {
                permissions.Add(new Permission() { Id = 8800, Added = System.DateTime.Now, LastModified = System.DateTime.Now, Name = "Memos", Description = "Memos" });

                permissions.Add(new Permission() { Id = 8801, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8800, Name = "Add New Memo", Description = "Allow User to Add New Memo" });

                permissions.Add(new Permission() { Id = 8802, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8800, Name = "View Memo", Description = "Allow User To View Memo" });
                permissions.Add(new Permission() { Id = 8803, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8800, Name = "Edit Memo", Description = "Allow User To Edit Memo" });
                permissions.Add(new Permission() { Id = 8804, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8800, Name = "List of Memos", Description = "Allow User To View List of Memos" });

                permissions.Add(new Permission() { Id = 8805, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8800, Name = "List of Void Memos", Description = "Allow User To View List of Void Memos" });
                permissions.Add(new Permission() { Id = 8806, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8800, Name = "Mark as Void Memos", Description = "Allow User To Mark as Void Memos" });
                permissions.Add(new Permission() { Id = 8807, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8800, Name = "Unmark Void Memos", Description = "Allow User To Unmark Void Memos" });

                permissions.Add(new Permission() { Id = 8808, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8800, Name = "Can Add Comment in Group Memos", Description = "Allow User To Add Comment in Group Memos" });
                permissions.Add(new Permission() { Id = 8809, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8800, Name = "Can Attach file in Memos", Description = "Allow User To Attach file in Memos" });

                permissions.Add(new Permission() { Id = 8850, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8800, Name = "Performance Review", Description = "Performance Review" });
                permissions.Add(new Permission() { Id = 8851, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8850, Name = "Can Create Performance Review from Memo", Description = "Allow User to Create Performance Review from Memo" });
                permissions.Add(new Permission() { Id = 8852, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 8850, Name = "Can View Performance Review Summary", Description = "Allow User to View Performance Review Summary" });
            }

            //    //var reportsHead = context.Permissions.FirstOrDefault(x => x.Name == "Reports");
            //    //permissions.Add(new Permission() { Id = 4200, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = reportsHead.Id, Name = "Shared Reports", Description = "Allow User access Shared Reports" });
            //    //permissions.Add(new Permission() { Id = 4201, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4200, Name = "Shared Report Groups", Description = "Allow User to access Shared Report Groups" });
            //    //permissions.Add(new Permission() { Id = 4202, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4201, Name = "Add Shared Report Groups", Description = "Allow User to Add Shared Report Groups" });
            //    //permissions.Add(new Permission() { Id = 4203, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4201, Name = "Edit Shared Report Groups", Description = "Allow User to Edit Shared Report Groups" });
            //    //permissions.Add(new Permission() { Id = 4204, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4201, Name = "View Shared Report Groups Register", Description = "Allow User to View Shared Report Groups Register" });
            //    //permissions.Add(new Permission() { Id = 4205, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4201, Name = "Bypass shared group permissions", Description = "Allow User to Bypass shared group permissions" });
            //    //permissions.Add(new Permission() { Id = 80, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = reportsHead.Id, Name = "Share Inquiry Report", Description = "Allow User To Share Inquiry Report" });
            //    //permissions.Add(new Permission() { Id = 82, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = reportsHead.Id, Name = "Share Purchase Orders Report", Description = "Allow User To Share Sale Orders Report" });


            //    {

            //        //if (context.Permissions.FirstOrDefault(x => x.Name == "Reports") == null)
            //        //{
            //        //    permissions.Add(new Permission() { Id = 80, Added = System.DateTime.Now, LastModified = System.DateTime.Now, Name = "Reports", Description = "Allow User To View Reports Menu" });
            //        //    permissions.Add(new Permission() { Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 80, Name = "Access to Report Group", Description = "Allow User To Access Report Group" });

            //        //    if (context.Permissions.FirstOrDefault(x => x.Name == "Memorized Reports") == null)
            //        //    {
            //        //        permissions.Add(new Permission() { Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 80, Name = "Memorized Reports", Description = "Allow User To Memorized Reports" });

            //        //        if (context.Permissions.FirstOrDefault(x => x.Name == "View Memorized Reports") == null)
            //        //        {
            //        //            permissions.Add(new Permission() { Id = 81, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 80, Name = "View Memorized Reports", Description = "Allow User To View Memorized Reports" });
            //        //            permissions.Add(new Permission() { Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 81, Name = "Save as new Memorized Report", Description = "Allow user to Save as new after view report" });
            //        //            permissions.Add(new Permission() { Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 81, Name = "Access to Rename Memorized Report", Description = "Allow User To Rename Memorized Report " });
            //        //            permissions.Add(new Permission() { Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 81, Name = "Access to Update Memorized Report", Description = "Allow User to Update Memorized Report " });
            //        //            permissions.Add(new Permission() { Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 81, Name = "Access to Delete Memorized Report", Description = "Allow User to Delete Memorized Report " });
            //        //            permissions.Add(new Permission() { Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 81, Name = "Access to Export to Standard Report", Description = "Allow User to Export to Standard Report " });

            //        //        }
            //        //    }
            //        //    if (context.Permissions.FirstOrDefault(x => x.Name == "Standard Reports") == null)
            //        //    {
            //        //        permissions.Add(new Permission() { Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 80, Name = "Standard Reports", Description = "Allow user to Standard Reports" });

            //        //        if (context.Permissions.FirstOrDefault(x => x.Name == "View Standard Reports") == null)
            //        //        {
            //        //            permissions.Add(new Permission() { Id = 83, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 80, Name = "View Standard Reports", Description = "Allow user to View Standard Reports" });
            //        //            permissions.Add(new Permission() { Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 83, Name = "Save as new Standard Report", Description = "Allow user to Save as new after view report" });
            //        //            permissions.Add(new Permission() { Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 83, Name = "Access to Rename Standard Report", Description = "Allow User To Rename Standard Report " });
            //        //            permissions.Add(new Permission() { Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 83, Name = "Access to Update Standard Report", Description = "Allow User to Update Standard Report " });
            //        //            permissions.Add(new Permission() { Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 83, Name = "Access to Delete Standard Report", Description = "Allow User to Delete Standard Report " });
            //        //            permissions.Add(new Permission() { Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 83, Name = "Access to Export to Memorized Report", Description = "Allow User to Export to Memorized Report" });
            //        //        }
            //        //    }
            //        //    if (context.Permissions.FirstOrDefault(x => x.Name == "Access to View Standard Groups") == null)
            //        //    {
            //        //        permissions.Add(new Permission() { Id = 85, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 80, Name = "Access to View Standard Groups", Description = "Allow User To View Standard Groups" });
            //        //        permissions.Add(new Permission() { Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 85, Name = "Access to Add new Standard Group", Description = "Allow User To Add new Standard Group" });
            //        //        permissions.Add(new Permission() { Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 85, Name = "Access to Edit Standard Group", Description = "Allow User To Edit Standard Group" });


            //        //    }
            //        //    if (context.Permissions.FirstOrDefault(x => x.Name == "Access to View Memorized Groups") == null)
            //        //    {
            //        //        permissions.Add(new Permission() { Id = 86, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 80, Name = "Access to View Memorized Groups", Description = "Allow User To View Memorized Groups" });
            //        //        permissions.Add(new Permission() { Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 86, Name = "Access to Add new Memorized Group", Description = "Allow User To Add new Memorized Group" });
            //        //        permissions.Add(new Permission() { Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 86, Name = "Access to Edit Memorized Groups", Description = "Allow User To Add new Memorized Group" });
            //        //    }
            //        //}



            //        //if (context.Permissions.FirstOrDefault(x => x.Name == "Export Inquiries Reports") == null)
            //        //{
            //        //    var parentReport = context.Permissions.FirstOrDefault(x => x.Name == "Reports");
            //        //    permissions.Add(new Permission() { Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = parentReport.Id, Name = "Export Inquiries Reports", Description = "Allow User To export Inquiries report" });
            //        //    context.GridReportGroups.Add(new GridReportGroup() { groupName = "Inquiries", isActive = true, gridReportType = Enums.GridReportType.StandardReport });
            //        //    context.GridReportGroups.Add(new GridReportGroup() { groupName = "Inquiries", isActive = true, gridReportType = Enums.GridReportType.MemorizedReport });
            //        //}
            //        //if (context.Permissions.FirstOrDefault(x => x.Name == "Export Offers Reports") == null)
            //        //{
            //        //    var parentReport = context.Permissions.FirstOrDefault(x => x.Name == "Reports");
            //        //    permissions.Add(new Permission() { Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = parentReport.Id, Name = "Export Inquiries Reports", Description = "Allow User To export Offers report" });
            //        //    context.GridReportGroups.Add(new GridReportGroup() { groupName = "Offers", isActive = true, gridReportType = Enums.GridReportType.StandardReport });
            //        //    context.GridReportGroups.Add(new GridReportGroup() { groupName = "Offers", isActive = true, gridReportType = Enums.GridReportType.MemorizedReport });
            //        //}
            //        //if (context.Permissions.FirstOrDefault(x => x.Name == "Export Sale Orders Reports") == null)
            //        //{
            //        //    var parentReport = context.Permissions.FirstOrDefault(x => x.Name == "Reports");
            //        //    permissions.Add(new Permission() { Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = parentReport.Id, Name = "Export Sale Orders Reports", Description = "Allow User To Export Sale Orders report" });
            //        //    context.GridReportGroups.Add(new GridReportGroup() { groupName = "Sale Orders", isActive = true, gridReportType = Enums.GridReportType.StandardReport });
            //        //    context.GridReportGroups.Add(new GridReportGroup() { groupName = "Sale Orders", isActive = true, gridReportType = Enums.GridReportType.MemorizedReport });
            //        //}
            //        //if (context.Permissions.FirstOrDefault(x => x.Name == "Export Purchase Orders Reports") == null)
            //        //{
            //        //    var parentReport = context.Permissions.FirstOrDefault(x => x.Name == "Reports");
            //        //    permissions.Add(new Permission() { Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = parentReport.Id, Name = "Export Purchase Orders Reports", Description = "Allow User To Export Purchase Orders report" });
            //        //    context.GridReportGroups.Add(new GridReportGroup() { groupName = "Purchase Orders", isActive = true, gridReportType = Enums.GridReportType.StandardReport });
            //        //    context.GridReportGroups.Add(new GridReportGroup() { groupName = "Purchase Orders", isActive = true, gridReportType = Enums.GridReportType.MemorizedReport });
            //        //}
            //        //if (context.Permissions.FirstOrDefault(x => x.Name == "Export Bills Reports") == null)
            //        //{
            //        //    var parentReport = context.Permissions.FirstOrDefault(x => x.Name == "Reports");
            //        //    permissions.Add(new Permission() { Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = parentReport.Id, Name = "Export Bills Reports", Description = "Allow User To Export Bills report" });
            //        //    context.GridReportGroups.Add(new GridReportGroup() { groupName = "Bills", isActive = true, gridReportType = Enums.GridReportType.StandardReport });
            //        //    context.GridReportGroups.Add(new GridReportGroup() { groupName = "Bills", isActive = true, gridReportType = Enums.GridReportType.MemorizedReport });
            //        //}
            //        //if (context.Permissions.FirstOrDefault(x => x.Name == "Export Sale Invoices Reports") == null)
            //        //{
            //        //    var parentReport = context.Permissions.FirstOrDefault(x => x.Name == "Reports");
            //        //    permissions.Add(new Permission() { Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = parentReport.Id, Name = "Export Sale Invoices Reports", Description = "Allow User To Export Sale Invoices report" });
            //        //    context.GridReportGroups.Add(new GridReportGroup() { groupName = "Sale Invoices", isActive = true, gridReportType = Enums.GridReportType.StandardReport });
            //        //    context.GridReportGroups.Add(new GridReportGroup() { groupName = "Sale Invoices", isActive = true, gridReportType = Enums.GridReportType.MemorizedReport });
            //        //}
            permissions.Add(new Permission() { Id = 6300, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 50, Name = "Budget", Description = "Budget" });
            permissions.Add(new Permission() { Id = 6301, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6300, Name = "Add Budget", Description = "Allow User To Add Budget" });
            permissions.Add(new Permission() { Id = 6302, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6300, Name = "Edit Budget", Description = "Allow User To Edit Budget" });
            permissions.Add(new Permission() { Id = 6303, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6300, Name = "View Budget", Description = "Allow User To View Budget" });
            permissions.Add(new Permission() { Id = 6304, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6300, Name = "List of Budget", Description = "Allow User To View List of Budget" });
            permissions.Add(new Permission() { Id = 6305, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6300, Name = "Add Budget Status", Description = "Allow User To Add new status for Budget" });
            permissions.Add(new Permission() { Id = 6306, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6300, Name = "Add Budget without Approval", Description = "Allow User To Add New Budget without Approval" });
            permissions.Add(new Permission() { Id = 6307, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6300, Name = "View InActive Budget", Description = "Allow User To View InActive Budget" });
            permissions.Add(new Permission() { Id = 6308, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6300, Name = "Close Budget", Description = "Allow User To Close Budget" });
            permissions.Add(new Permission() { Id = 6309, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6300, Name = "Edit Budget Status", Description = "Allow User To Edit Budget Status" });
            permissions.Add(new Permission() { Id = 6310, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6300, Name = "View InActive Budget Statuses", Description = "Allow User To View InActive Budget Statuses" });
            permissions.Add(new Permission() { Id = 6311, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6300, Name = "Close Budget without Approval", Description = "Allow User To Close Budget without Approval" });
            permissions.Add(new Permission() { Id = 6312, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6300, Name = "Edit Unapproved Budget", Description = "Allow User To Edit Unapproved Budget without Approval" });
            permissions.Add(new Permission() { Id = 6313, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6300, Name = "Reviewer Level 1 Budget", Description = "Allow User to mark Budget as Reviewed once" });
            permissions.Add(new Permission() { Id = 6314, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6300, Name = "Reviewer Level 2 Budget", Description = "Allow User To Mark Budget as Reviewed and move it to Approved List" });
            permissions.Add(new Permission() { Id = 6315, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6300, Name = "Approver for Closing Budget", Description = "Allow User To Close Budget which is in pending state" });
            permissions.Add(new Permission() { Id = 6316, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6300, Name = "Approver for new added Budget", Description = "Allow User To Approve Budget which is in pending state" });
            permissions.Add(new Permission() { Id = 6317, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6300, Name = "Edit closed Budget", Description = "Allow User To Edit Closed Budget" });
            permissions.Add(new Permission() { Id = 6318, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6300, Name = "View(Pending for closing) Budget List", Description = "Allow User To View List of All(Pending for Approval) Budget mapped to his Department" });
            permissions.Add(new Permission() { Id = 6319, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6300, Name = "View(Pending for Approval) Budget List", Description = "Allow User To View List of All(Pending for Approval) Budget mapped to his Department" });
            permissions.Add(new Permission() { Id = 6320, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6300, Name = "View Budget Register", Description = "Allow User To View Sale Register List" });
            permissions.Add(new Permission() { Id = 6321, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6300, Name = "Edit Creation Date of Budget", Description = "Allow User To Edit Creation Date of Budget" });
            permissions.Add(new Permission() { Id = 6322, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6300, Name = "Attach a file with Budget", Description = "Allow User To Attach a file with Budget" });
            permissions.Add(new Permission() { Id = 6323, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6300, Name = "View list of attached files with Budget", Description = "Allow User To View list of attached files with Budget" });
            permissions.Add(new Permission() { Id = 6324, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6300, Name = "ReApprover for new added Budget", Description = "Allow User To ReApprove Budget which is in pending state" });
            permissions.Add(new Permission() { Id = 6325, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6300, Name = "Add Budget without ReApproval", Description = "Allow User To Add New Sale Order without ReApproval" });
            permissions.Add(new Permission() { Id = 6326, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6300, Name = "View(Pending for ReApproval) Budget List", Description = "Allow User To View List of All(Pending for ReApproval) Budget mapped to his Department" });
            permissions.Add(new Permission() { Id = 6327, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6300, Name = "Mark as Void Budget", Description = "Allow User To Mark as Void Budget" });
            permissions.Add(new Permission() { Id = 6328, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6300, Name = "Unmark Void Budget", Description = "Allow User To unmark as Void Budget" });
            permissions.Add(new Permission() { Id = 6329, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6300, Name = "View list of Void Budget", Description = "View list of Void Budget" });
            permissions.Add(new Permission() { Id = 6330, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6300, Name = "Edit (Pending for closing) Budget", Description = "Allow User To Edit (Pending for Closing) Budget mapped to his Department" });
            permissions.Add(new Permission() { Id = 6331, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6300, Name = "Edit RSBC from Budget", Description = "Edit RSBC from Budget" });
            permissions.Add(new Permission() { Id = 6332, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6300, Name = "Can attach document when Budget Closed", Description = "Allow User To attach document when Budget Closed" });
            permissions.Add(new Permission() { Id = 6333, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6300, Name = "Can revised budget from SO", Description = "Allow User To revised budget from SO" });
            permissions.Add(new Permission() { Id = 6334, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6300, Name = "Can Link SO with Budget", Description = "Allow User To Link SO with Budget" });
            permissions.Add(new Permission() { Id = 6335, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6300, Name = "Punch Budget System Cost", Description = "Allow User To Punch Budget System Cost" });
            permissions.Add(new Permission() { Id = 6336, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6300, Name = "Can Link PO with Budget", Description = "Allow User To Link PO with Budget" });

            //STL
            {
                permissions.Add(new Permission() { Id = 6500, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 4100, Name = "STL", Description = "STL" });
                permissions.Add(new Permission() { Id = 6501, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6500, Name = "Add STL", Description = "Allow User To Add STL" });
                permissions.Add(new Permission() { Id = 6502, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6500, Name = "Edit STL", Description = "Allow User To Edit STL" });
                permissions.Add(new Permission() { Id = 6503, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6500, Name = "View STL", Description = "Allow User To View STL" });
                permissions.Add(new Permission() { Id = 6504, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6500, Name = "List of STL", Description = "Allow User To View List of STL" });
                permissions.Add(new Permission() { Id = 6505, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6500, Name = "Add STL Status", Description = "Allow User To Add new status for STL" });
                permissions.Add(new Permission() { Id = 6506, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6500, Name = "Add STL without Approval", Description = "Allow User To Add New STL without Approval" });
                permissions.Add(new Permission() { Id = 6507, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6500, Name = "View InActive STL", Description = "Allow User To View InActive STL" });
                permissions.Add(new Permission() { Id = 6508, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6500, Name = "Close STL", Description = "Allow User To Close STL" });
                permissions.Add(new Permission() { Id = 6509, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6500, Name = "Edit STL Status", Description = "Allow User To Edit STL Status" });
                permissions.Add(new Permission() { Id = 6510, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6500, Name = "View InActive STL Statuses", Description = "Allow User To View InActive STL Statuses" });
                permissions.Add(new Permission() { Id = 6511, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6500, Name = "Close STL without Approval", Description = "Allow User To Close STL without Approval" });
                permissions.Add(new Permission() { Id = 6512, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6500, Name = "Edit Unapproved STL", Description = "Allow User To Edit Unapproved STL without Approval" });
                permissions.Add(new Permission() { Id = 6513, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6500, Name = "Reviewer Level 1 STL", Description = "Allow User to mark STL as Reviewed once" });
                permissions.Add(new Permission() { Id = 6514, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6500, Name = "Reviewer Level 2 STL", Description = "Allow User To Mark STL as Reviewed and move it to Approved List" });
                permissions.Add(new Permission() { Id = 6515, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6500, Name = "Approver for Closing STL", Description = "Allow User To Close STL which is in pending state" });
                permissions.Add(new Permission() { Id = 6516, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6500, Name = "Approver for new added STL", Description = "Allow User To Approve STL which is in pending state" });
                permissions.Add(new Permission() { Id = 6517, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6500, Name = "Edit closed STL", Description = "Allow User To Edit Closed STL" });
                permissions.Add(new Permission() { Id = 6518, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6500, Name = "View(Pending for closing) STL List", Description = "Allow User To View List of All(Pending for Approval) STL mapped to his Department" });
                permissions.Add(new Permission() { Id = 6519, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6500, Name = "View(Pending for Approval) STL List", Description = "Allow User To View List of All(Pending for Approval) STL mapped to his Department" });
                permissions.Add(new Permission() { Id = 6520, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6500, Name = "View STL Register", Description = "Allow User To View STL Register List" });
                permissions.Add(new Permission() { Id = 6521, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6500, Name = "Edit Creation Date of STL", Description = "Allow User To Edit Creation Date of STL" });
                permissions.Add(new Permission() { Id = 6523, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6500, Name = "Attach a file with STL", Description = "Allow User To Attach a file with STL" });
                permissions.Add(new Permission() { Id = 6524, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6500, Name = "View list of attached files with STL", Description = "Allow User To View list of attached files with STL" });
                permissions.Add(new Permission() { Id = 6525, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6500, Name = "ReApprover for new added STL", Description = "Allow User To ReApprove STL which is in pending state" });
                permissions.Add(new Permission() { Id = 6526, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6500, Name = "Add STL without ReApproval", Description = "Allow User To Add New STL without ReApproval" });
                permissions.Add(new Permission() { Id = 6527, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6500, Name = "View(Pending for ReApproval) STL List", Description = "Allow User To View List of All(Pending for ReApproval) STL mapped to his Department" });
                permissions.Add(new Permission() { Id = 6528, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6500, Name = "Mark as Void STL", Description = "Allow User To Mark as Void STL" });
                permissions.Add(new Permission() { Id = 6529, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6500, Name = "Unmark Void STL", Description = "Allow User To unmark as Void SaleOrder" });
                permissions.Add(new Permission() { Id = 6530, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6500, Name = "View list of Void STL", Description = "View list of Void STL" });
                permissions.Add(new Permission() { Id = 6531, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6500, Name = "Edit (Pending for closing) STL", Description = "Allow User To Edit (Pending for Closing) STL mapped to his Department" });
                permissions.Add(new Permission() { Id = 6532, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6500, Name = "Export STL Reports", Description = "Allow User To Export STL Reports" });
                permissions.Add(new Permission() { Id = 6533, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6500, Name = "Can attach document when STL Closed", Description = "Allow User To attach document when STL Closed" });
                permissions.Add(new Permission() { Id = 6534, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6500, Name = "Edit GL-Posting Date", Description = "Allow User To Edit GL-Posting Date" });
                permissions.Add(new Permission() { Id = 6535, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6500, Name = "Add STL Remarks", Description = "Allow User To Add STL Remarks" });
            }

            //Polling
            {
                permissions.Add(new Permission() { Id = 6700, Added = System.DateTime.Now, LastModified = System.DateTime.Now, Name = "Polling", Description = "Polling" });

                permissions.Add(new Permission() { Id = 6701, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6700, Name = "View List of Pollings", Description = "Allow User To View List of Pollings" });
                permissions.Add(new Permission() { Id = 6702, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6700, Name = "Create New Poll", Description = "Allow User To Create New Poll" });
                permissions.Add(new Permission() { Id = 6703, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6700, Name = "Edit Poll", Description = "Allow User To Edit Poll" });
                permissions.Add(new Permission() { Id = 6704, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6700, Name = "By Pass Hidden Polling Permissions", Description = "Allow User To By Pass Hidden Polling Permissions" });
                permissions.Add(new Permission() { Id = 6705, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6700, Name = "View InActive Pollings List", Description = "Allow User To View InActive Pollings List" });
                permissions.Add(new Permission() { Id = 6706, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6700, Name = "View List of All Pollings", Description = "Allow User To View List of All Pollings" });
                permissions.Add(new Permission() { Id = 6707, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 6700, Name = "Delete Polling", Description = "Allow User To Delete Pollings" });
            }



            //        if (context.Permissions.FirstOrDefault(x => x.Name == "Export Sale Receipts Reports") == null)
            //        {
            //            var parentReport = context.Permissions.FirstOrDefault(x => x.Name == "Reports");
            //            permissions.Add(new Permission() { Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentPermission = parentReport, Name = "Export Sale Receipts Reports", Description = "Allow User To export sale receipt report" });
            //            context.GridReportGroups.Add(new GridReportGroup() { groupName = "Sale Receipts", isActive = true, gridReportType = Enums.GridReportType.StandardReport });
            //            context.GridReportGroups.Add(new GridReportGroup() { groupName = "Sale Receipts", isActive = true, gridReportType = Enums.GridReportType.MemorizedReport });
            //        }
            //        if (context.Permissions.FirstOrDefault(x => x.Name == "Export Inter-Bank Transfers Reports") == null)
            //        {
            //            var parentReport = context.Permissions.FirstOrDefault(x => x.Name == "Reports");
            //            permissions.Add(new Permission() { Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentPermission = parentReport, Name = "Export Inter-Bank Transfers Reports", Description = "Allow User To export Inter-Bank Transfer report" });
            //            context.GridReportGroups.Add(new GridReportGroup() { groupName = "Inter-Bank Transfers", isActive = true, gridReportType = Enums.GridReportType.StandardReport });
            //            context.GridReportGroups.Add(new GridReportGroup() { groupName = "Inter-Bank Transfers", isActive = true, gridReportType = Enums.GridReportType.MemorizedReport });
            //        }
            //        {
            permissions.Add(new Permission() { Id = 9500, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 50, Name = "ModuleContract", Description = "ModuleContract" });

            permissions.Add(new Permission() { Id = 9501, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 9500, Name = "Add New ModuleContract ", Description = "Allow User To Add New ModuleContract" });
            permissions.Add(new Permission() { Id = 9502, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 9500, Name = "Edit ModuleContract", Description = "Allow User To Edit ModuleContract Details" });
            permissions.Add(new Permission() { Id = 9503, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 9500, Name = "ModuleContract Details", Description = "Allow User To View ModuleContract Details" });
            permissions.Add(new Permission() { Id = 9504, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 9500, Name = "List Of ModuleContracts", Description = "Allow User To view List of ModuleContracts" });
            permissions.Add(new Permission() { Id = 9505, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 9500, Name = "Add ModuleContract Status", Description = "Allow User To Add new Status for ModuleContract" });
            permissions.Add(new Permission() { Id = 9506, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 9500, Name = "View InActive ModuleContracts", Description = "Allow User To View InActive ModuleContracts" });
            permissions.Add(new Permission() { Id = 9507, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 9500, Name = "Add ModuleContract without Approval", Description = "Allow User To Add New ModuleContract without Approval" });
            permissions.Add(new Permission() { Id = 9508, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 9500, Name = "Edit ModuleContract Status", Description = "Allow User To Edit ModuleContract Status" });
            permissions.Add(new Permission() { Id = 9509, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 9500, Name = "View InActive ModuleContract Statuses", Description = "Allow User To View InActive ModuleContract Statuses" });
            permissions.Add(new Permission() { Id = 9510, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 9500, Name = "Create Sale Order from ModuleContract", Description = "Allow User To Create Purchase Order from an existing ModuleContract" });
            permissions.Add(new Permission() { Id = 9511, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 9500, Name = "Close ModuleContract without Approval", Description = "Allow User To Close ModuleContract without Approval" });
            permissions.Add(new Permission() { Id = 9512, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 9500, Name = "Close ModuleContract", Description = "Allow User To Close ModuleContract" });
            permissions.Add(new Permission() { Id = 9513, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 9500, Name = "Reviewer Level 1 ModuleContract", Description = "Allow User to mark ModuleContract as Reviewed once" });
            permissions.Add(new Permission() { Id = 9514, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 9500, Name = "Reviewer Level 2 ModuleContract", Description = "Allow User To Mark ModuleContract as Reviewed and move it to Approved  List" });
            permissions.Add(new Permission() { Id = 9515, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 9500, Name = "Approver for Closing ModuleContract", Description = "Allow User To Close ModuleContract which is in pending state" });
            permissions.Add(new Permission() { Id = 9516, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 9500, Name = "Approver for new added ModuleContract", Description = "Allow User To Approve ModuleContract which is in pending state" });
            permissions.Add(new Permission() { Id = 9517, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 9500, Name = "Edit closed ModuleContract", Description = "Allow User To Edit Closed ModuleContract" });
            permissions.Add(new Permission() { Id = 9518, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 9500, Name = "View(Pending for Approval) ModuleContract List", Description = "Allow User To View List of All(Pending for Approval) ModuleContracts mapped to his Department" });
            permissions.Add(new Permission() { Id = 9519, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 9500, Name = "View(Pending for closing) ModuleContract List", Description = "Allow User To View List of All(Pending for Closing) ModuleContract mapped to his Department" });
            permissions.Add(new Permission() { Id = 9520, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 9500, Name = "Edit Creation Date of ModuleContract", Description = "Allow User To Edit Creation Date of ModuleContract" });
            permissions.Add(new Permission() { Id = 9521, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 9500, Name = "Attach a file with ModuleContract", Description = "Allow User To Attach a file with ModuleContract" });
            permissions.Add(new Permission() { Id = 9522, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 9500, Name = "View list of attached files with ModuleContract", Description = "Allow User To View list of attached files with ModuleContract" });
            permissions.Add(new Permission() { Id = 9523, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 9500, Name = "Mark as Void ModuleContract", Description = "Allow User To Mark as Void ModuleContract" });
            permissions.Add(new Permission() { Id = 9524, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 9500, Name = "Unmark Void ModuleContract", Description = "Allow User To unmark as Void ModuleContract" });
            permissions.Add(new Permission() { Id = 9525, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 9500, Name = "View list of Void ModuleContracts", Description = "View list of Void ModuleContracts" });
            permissions.Add(new Permission() { Id = 9526, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 9500, Name = "Move ModuleContract to Inter Company", Description = "Allow User To Move ModuleContract to Inter Company mapped to his Department" });
            permissions.Add(new Permission() { Id = 9527, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 9500, Name = "Add Vendor Comparative Statement", Description = "Allow User To Add Vendor Comparative Statement" });
            permissions.Add(new Permission() { Id = 9528, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 9500, Name = "View Vendor Comparative Statement", Description = "Allow User To View Vendor Comparative Statement" });
            permissions.Add(new Permission() { Id = 9529, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 9500, Name = "Import ModuleContract Items", Description = "Allow User To Import ModuleContract Items" });
            permissions.Add(new Permission() { Id = 9530, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 9500, Name = "View ModuleContract Register", Description = "Allow User To View ModuleContract Register" });
            permissions.Add(new Permission() { Id = 9800, Added = System.DateTime.Now, LastModified = System.DateTime.Now, Name = "File", Description = "File" });
            permissions.Add(new Permission() { Id = 9700, Added = System.DateTime.Now, LastModified = System.DateTime.Now, Name = "Documents", Description = "Documents" });
                        permissions.Add(new Permission() { Id = 9701, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 9700, Name = "Add Document", Description = "Allow User To Add Document" });
                        permissions.Add(new Permission() { Id = 9702, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 9700, Name = "Edit Document", Description = "Allow User To Edit Document" });
                        permissions.Add(new Permission() { Id = 9703, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 9700, Name = "View Document", Description = "Allow User To View Document" });
                        permissions.Add(new Permission() { Id = 9704, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 9700, Name = "List of Documents", Description = "Allow User To View List of Document" });

                        permissions.Add(new Permission() { Id = 9706, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 9700, Name = "Add Document without Approval", Description = "Allow User To Add New Document without Approval" });
                        permissions.Add(new Permission() { Id = 9707, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 9700, Name = "View InActive Documents", Description = "Allow User To View InActive Documents" });
                        permissions.Add(new Permission() { Id = 9708, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 9700, Name = "Close Document", Description = "Allow User To Close Document" });


                        permissions.Add(new Permission() { Id = 9709, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 9700, Name = "Close Document without Approval", Description = "Allow User To Close Document without Approval" });
                        permissions.Add(new Permission() { Id = 9710, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 9700, Name = "Edit Unapproved Document", Description = "Allow User To Edit Unapproved Document without Approval" });

                        permissions.Add(new Permission() { Id = 9711, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 9700, Name = "Reviewer Level 1 Document", Description = "Allow User to mark Document as Reviewed once" });
                        permissions.Add(new Permission() { Id = 9712, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 9700, Name = "Reviewer Level 2 Document", Description = "Allow User To Mark Document as Reviewed and move it to Approved List" });
                        permissions.Add(new Permission() { Id = 9713, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 9700, Name = "Approver for Closing Document", Description = "Allow User To Close Document which is in pending state" });
                        permissions.Add(new Permission() { Id = 9714, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 9700, Name = "Approver for new Added Document", Description = "Allow User To Approve Document which is in pending state" });
                        permissions.Add(new Permission() { Id = 9715, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 9700, Name = "Edit Closed Document", Description = "Allow User To Edit Closed Document" });
                        permissions.Add(new Permission() { Id = 9716, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 9700, Name = "View(Pending for closing) Document List", Description = "Allow User To View List of All(Pending for Approval) Document mapped to his Department" });
                        permissions.Add(new Permission() { Id = 9717, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 9700, Name = "View(Pending for Approval) Document List", Description = "Allow User To View List of All(Pending for Approval) Document mapped to his Department" });
                        permissions.Add(new Permission() { Id = 9718, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 9700, Name = "View Document Register", Description = "Allow User To View Document Register List" });
                        permissions.Add(new Permission() { Id = 9719, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 9700, Name = "Edit Creation Date of Document", Description = "Allow User To Edit Creation Date of Document" });
                        permissions.Add(new Permission() { Id = 9720, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 9700, Name = "Attach a file with Document", Description = "Allow User To Attach a file with Document" });
                        permissions.Add(new Permission() { Id = 9721, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 9700, Name = "View List of Attached files with Document", Description = "Allow User To View List of Attached files with Document" });

                        permissions.Add(new Permission() { Id = 9722, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 9700, Name = "ReApprover for new Added Document", Description = "Allow User To ReApprove Document which is in pending state" });
                        permissions.Add(new Permission() { Id = 9723, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 9700, Name = "Add Document Without ReApproval", Description = "Allow User To Add New Document without ReApproval" });
                        permissions.Add(new Permission() { Id = 9724, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 9700, Name = "View(Pending for ReApproval) Document List", Description = "Allow User To View List of All(Pending for ReApproval) Document mapped to his Department" });
                        permissions.Add(new Permission() { Id = 9725, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 9700, Name = "Mark as Void Document", Description = "Allow User To Mark as Void Document" });
                        permissions.Add(new Permission() { Id = 9726, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 9700, Name = "Unmark Void Document", Description = "Allow User To unmark as Void Document" });
                        permissions.Add(new Permission() { Id = 9727, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 9700, Name = "View list of Void Documents", Description = "View list of Void Documents" });
                        permissions.Add(new Permission() { Id = 9728, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 9700, Name = "Edit (Pending for closing) Document", Description = "Allow User To Edit (Pending for Closing) Document mapped to his Department" });

                        permissions.Add(new Permission() { Id = 9729, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 9700, Name = "Document Status", Description = "Document Status" });
                        permissions.Add(new Permission() { Id = 9730, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 9729, Name = "Add Document Status", Description = "Allow User To Add new status for Document" });
                        permissions.Add(new Permission() { Id = 9731, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 9729, Name = "Edit Document Status", Description = "Allow User To Edit Document Status" });
                        permissions.Add(new Permission() { Id = 9732, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 9729, Name = "View List of Document Statuses", Description = "Allow User To View List of Document Statuses" });
                        permissions.Add(new Permission() { Id = 9733, Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentId = 9729, Name = "View InActive Document Statuses", Description = "Allow User To View InActive Document Statuses" });
                    


                
            //        //SharedGridGroup Report Groups
            //        List<SharedGridGroup> sharedGroups = new List<SharedGridGroup>();
            //    sharedGroups.Add(new SharedGridGroup() { groupName = "Inquiries", isVoid = false });
            //    sharedGroups.Add(new SharedGridGroup() { groupName = "Sale Orders", isVoid = false });
            //    sharedGroups.Add(new SharedGridGroup() { groupName = "Offers", isVoid = false });
            //    sharedGroups.Add(new SharedGridGroup() { groupName = "Purchase Orders", isVoid = false });
            //    sharedGroups.Add(new SharedGridGroup() { groupName = "Bills", isVoid = false });
            //    sharedGroups.Add(new SharedGridGroup() { groupName = "Sale Invoices", isVoid = false });
            //    sharedGroups.Add(new SharedGridGroup() { groupName = "Sale Receipts", isVoid = false });
            //    sharedGroups.Add(new SharedGridGroup() { groupName = "Inter-Bank Transfers", isVoid = false });
            //    sharedGroups.Add(new SharedGridGroup() { groupName = "Admin Bills", isVoid = false });
            //    sharedGroups.Add(new SharedGridGroup() { groupName = "Payments", isVoid = false });
            //    sharedGroups.Add(new SharedGridGroup() { groupName = "Purchase Invoices", isVoid = false });
            //    sharedGroups.Add(new SharedGridGroup() { groupName = "Chart of Accounts", isVoid = false });

            //    var dbParentGroups = context.sharedGridGroups.Where(x => x.parentId == null).ToList();
            //    if (dbParentGroups.Count == 0)
            //    {
            //        context.sharedGridGroups.AddRange(sharedGroups);
            //    }



            //    //    permissions.Add(standardReports);
            //    //    var reportGroupObj = new Permission() { Added = System.DateTime.Now, LastModified = System.DateTime.Now, ParentPermission = reportObj, Name = "Access to Report Group", Description = "Allow User To Access Report Group" };
            //    //    permissions.Add(reportGroupObj);



            //    //GridReportGroups seeding
            //    //    IList<GridReportGroup> groups = new List<GridReportGroup>();
            //    //if (context.GridReportGroups.Count() == 0)
            //    //{
            //    //    groups.Add(new GridReportGroup() { groupName = "Inquiries", isActive = true, gridReportType = Enums.GridReportType.StandardReport });
            //    //    groups.Add(new GridReportGroup() { groupName = "Sale Orders", isActive = true, gridReportType = Enums.GridReportType.StandardReport });
            //    //    groups.Add(new GridReportGroup() { groupName = "Offers", isActive = true, gridReportType = Enums.GridReportType.StandardReport });
            //    //    groups.Add(new GridReportGroup() { groupName = "Inquiries", isActive = true, gridReportType = Enums.GridReportType.MemorizedReport });
            //    //    groups.Add(new GridReportGroup() { groupName = "Sale Orders", isActive = true, gridReportType = Enums.GridReportType.MemorizedReport });
            //    //    groups.Add(new GridReportGroup() { groupName = "Offers", isActive = true, gridReportType = Enums.GridReportType.MemorizedReport });
            //    //    groups.Add(new GridReportGroup() { groupName = "Purchase Orders", isActive = true, gridReportType = Enums.GridReportType.StandardReport });
            //    //    groups.Add(new GridReportGroup() { groupName = "Bills", isActive = true, gridReportType = Enums.GridReportType.StandardReport });
            //    //    groups.Add(new GridReportGroup() { groupName = "Purchase Orders", isActive = true, gridReportType = Enums.GridReportType.MemorizedReport });
            //    //    groups.Add(new GridReportGroup() { groupName = "Bills", isActive = true, gridReportType = Enums.GridReportType.MemorizedReport });
            //    //    groups.Add(new GridReportGroup() { groupName = "Sale Invoices", isActive = true, gridReportType = Enums.GridReportType.StandardReport });
            //    //    groups.Add(new GridReportGroup() { groupName = "Sale Invoices", isActive = true, gridReportType = Enums.GridReportType.MemorizedReport });
            //    //    groups.Add(new GridReportGroup() { groupName = "Sale Receipt", isActive = true, gridReportType = Enums.GridReportType.StandardReport });
            //    //    groups.Add(new GridReportGroup() { groupName = "Sale Receipt", isActive = true, gridReportType = Enums.GridReportType.MemorizedReport });

            //    //}

            //    //if (groups.Count != context.GridReportGroups.Count())
            //    //{
            //    //    context.GridReportGroups.AddRange(groups);
            //    //}
            //    //Asset Nature Seeding



            //    ////if(context.fields.Count() == 0)
            //    ////{
            //    ////    //Fields Seeding
            //    //    fields.Add(new Field() { Id = 1, Identity = 1, Name = "cmbInquiryType", elementType = "ComboBox", Description = "Inquiry Type List", transactionType = Enums.TransactionItemType.Inquiry, LastModified = DateTime.Now });
            //    //    fields.Add(new Field() { Id = 2, Identity = 2, Name = "lookupCompany", elementType = "LookUpEdit", Description = "Company List", transactionType = Enums.TransactionItemType.Inquiry, LastModified = DateTime.Now });
            //    //    fields.Add(new Field() { Id = 3, Identity = 3, Name = "lookupDepartment", elementType = "LookUpEdit", Description = "Department List", transactionType = Enums.TransactionItemType.Inquiry, LastModified = DateTime.Now });
            //    //    fields.Add(new Field() { Id = 4, Identity = 4, Name = "lookupCustomer", elementType = "LookUpEdit", Description = "Customer List", transactionType = Enums.TransactionItemType.Inquiry, LastModified = DateTime.Now });
            //    //    fields.Add(new Field() { Id = 5, Identity = 5, Name = "cmbEmployee", elementType = "ComboBox", Description = "Employee List", transactionType = Enums.TransactionItemType.Inquiry, LastModified = DateTime.Now });
            //    ////}

            //    //if (context.fields.Count() != fields.Count()+1)
            //    //{
            //    //    IList<Field> fieldsToAdd = new List<Field>();

            //    //    foreach(var _field in fields)
            //    //    {
            //    //        if(context.fields.FirstOrDefault(x=>x.Identity == _field.Identity) == null)
            //    //        {
            //    //            fieldsToAdd.Add(_field);
            //    //        }
            //    //    }
            //    //    context.fields.AddRange(fieldsToAdd);z
            //    //}

            //    //    fields.Add(new Field() { Id = 1, Identity = 1, Name = "cmbInquiryType", elementType = "ComboBox", Description = "Inquiry Type List", transactionType = Enums.TransactionItemType.Inquiry, LastModified = DateTime.Now });
            //    //    fields.Add(new Field() { Id = 2, Identity = 2, Name = "lookupCompany", elementType = "LookUpEdit", Description = "Company List", transactionType = Enums.TransactionItemType.Inquiry, LastModified = DateTime.Now });
            //    //    fields.Add(new Field() { Id = 3, Identity = 3, Name = "lookupDepartment", elementType = "LookUpEdit", Description = "Department List", transactionType = Enums.TransactionItemType.Inquiry, LastModified = DateTime.Now });
            //    //    fields.Add(new Field() { Id = 4, Identity = 4, Name = "lookupCustomer", elementType = "LookUpEdit", Description = "Customer List", transactionType = Enums.TransactionItemType.Inquiry, LastModified = DateTime.Now });
            //    //    fields.Add(new Field() { Id = 5, Identity = 5, Name = "cmbEmployee", elementType = "ComboBox", Description = "Employee List", transactionType = Enums.TransactionItemType.Inquiry, LastModified = DateTime.Now });

            //    //    if (context.fields.Count() != fields.Count() + 1)
            //    //    {
            //    //        IList<Field> fieldsToAdd = new List<Field>();

            //    //        foreach (var _field in fields)
            //    //        {
            //    //            if (context.fields.FirstOrDefault(x => x.Identity == _field.Identity) == null)
            //    //            {
            //    //                fieldsToAdd.Add(_field);
            //    //            }
            //    //        }
            //    //        context.fields.AddRange(fieldsToAdd);
            //    //    }
            //    //}

            if (context.currencies.Count() == 0)
            {
                context.currencies.AddRange(currencies);
            }
            if (context.IndustryTypes.Count() == 0)
                context.IndustryTypes.AddRange(industryTypes);

            if (context.Permissions.Count() != permissions.Count() + 1)
                if (context.Permissions.Count() == 0)
                {
                    context.Permissions.AddRange(permissions);
                }
                else
                {
                    foreach (Permission permission in permissions)
                    {
                        Permission permis = context.Permissions.FirstOrDefault(x => x.Name == permission.Name);
                        //if (permission.Name == "Create Po from Offer")
                        //{
                        //}
                        if (permission.ParentId != null)
                        {
                            Permission permissionaa = permissions.FirstOrDefault(x => x.Id == permission.ParentId);
                            if (permissionaa != null /*&& permissionaa.ParentId != null*/)
                            {
                                Permission parentpermission = context.Permissions.FirstOrDefault(x => x.Name == permissionaa.Name);
                                if (parentpermission != null)
                                    permission.ParentId = parentpermission.Id;
                            }
                            else
                            {
                                permission.ParentId = null;
                            }
                        }
                        if (permis == null)
                        {
                            context.Permissions.Add(permission);
                            context.SaveChanges();
                        }
                        else if (permis.Name != permission.Name || permis.ParentId != permission.ParentId)
                        {
                            permis.Name = permission.Name;
                            permis.ParentId = permission.ParentId;
                            //permis.ParentId = permission.ParentId;
                            context.SaveChanges();
                        }
                    }
                }
            base.Seed(context);
            context.SaveChanges();
        }


    }
}
