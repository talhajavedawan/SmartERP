namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AllResetDeployment1_1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tabAddress",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Line1 = c.String(),
                        Line2 = c.String(),
                        Zip = c.Int(),
                        State = c.String(),
                        Country = c.String(),
                        City = c.String(),
                        region = c.String(),
                        addressType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Bids",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        value = c.String(),
                        refNo = c.String(),
                        bankName = c.String(),
                        issueDate = c.DateTime(nullable: false),
                        expireDate = c.DateTime(nullable: false),
                        submitDate = c.DateTime(nullable: false),
                        isactive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.tabCompany",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CompanyName = c.String(),
                        industryTypeId = c.Int(nullable: false),
                        BizType = c.Int(nullable: false),
                        EmployeerNo = c.String(),
                        openingDate = c.DateTime(nullable: false),
                        closingDate = c.DateTime(),
                        addressId = c.Int(nullable: false),
                        contactId = c.Int(nullable: false),
                        compnayType = c.Int(nullable: false),
                        IsSubsidary = c.Boolean(nullable: false),
                        ParentID = c.Int(),
                        CurrencyId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tabAddress", t => t.addressId, cascadeDelete: true)
                .ForeignKey("dbo.tabContact", t => t.contactId, cascadeDelete: true)
                .ForeignKey("dbo.Currencies", t => t.CurrencyId)
                .ForeignKey("dbo.IndustryTypes", t => t.industryTypeId, cascadeDelete: true)
                .ForeignKey("dbo.tabCompany", t => t.ParentID)
                .Index(t => t.industryTypeId)
                .Index(t => t.addressId)
                .Index(t => t.contactId)
                .Index(t => t.ParentID)
                .Index(t => t.CurrencyId);
            
            CreateTable(
                "dbo.tabContact",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ContactNo = c.String(),
                        Fax = c.String(),
                        Email = c.String(),
                        Website = c.String(),
                        SMLink1 = c.String(),
                        SMLink2 = c.String(),
                        SMLink3 = c.String(),
                        contactType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Currencies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CurrencyName = c.String(),
                        Symbol = c.String(),
                        Abbrivation = c.String(),
                        Country = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.tabDepartment",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DeptName = c.String(),
                        Code = c.String(maxLength: 20),
                        Abbrivation = c.String(maxLength: 20),
                        Timestamp = c.DateTime(nullable: false),
                        userID = c.Int(),
                        IsSubsidary = c.Boolean(nullable: false),
                        ParentID = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tabDepartment", t => t.ParentID)
                .ForeignKey("dbo.Users", t => t.userID)
                .Index(t => t.userID)
                .Index(t => t.ParentID);
            
            CreateTable(
                "dbo.CustomerCompanies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsSubsidary = c.Boolean(nullable: false),
                        ParentID = c.Int(),
                        billingAddres_Id = c.Int(),
                        company_Id = c.Int(),
                        contact_Id = c.Int(),
                        contactPerson_Id = c.Int(),
                        shippingAddress_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tabAddress", t => t.billingAddres_Id)
                .ForeignKey("dbo.tabCompany", t => t.company_Id)
                .ForeignKey("dbo.tabContact", t => t.contact_Id)
                .ForeignKey("dbo.tabPerson", t => t.contactPerson_Id)
                .ForeignKey("dbo.CustomerCompanies", t => t.ParentID)
                .ForeignKey("dbo.tabAddress", t => t.shippingAddress_Id)
                .Index(t => t.ParentID)
                .Index(t => t.billingAddres_Id)
                .Index(t => t.company_Id)
                .Index(t => t.contact_Id)
                .Index(t => t.contactPerson_Id)
                .Index(t => t.shippingAddress_Id);
            
            CreateTable(
                "dbo.tabPerson",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FName = c.String(),
                        LName = c.String(),
                        FatherName = c.String(),
                        NextKin = c.String(),
                        CNIC = c.String(nullable: false),
                        DOB = c.DateTime(nullable: false),
                        Gender = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        EmpId = c.Int(nullable: false, identity: true),
                        MaritalStatus = c.String(),
                        Disability = c.Boolean(nullable: false),
                        DisDescription = c.String(),
                        isActive = c.Boolean(nullable: false),
                        Status = c.Int(nullable: false),
                        JoinDate = c.DateTime(nullable: false),
                        BasicPay = c.Double(nullable: false),
                        address_Id = c.Int(),
                        contact_Id = c.Int(),
                        Desig_DesigId = c.Int(),
                        person_Id = c.Int(),
                    })
                .PrimaryKey(t => t.EmpId)
                .ForeignKey("dbo.tabAddress", t => t.address_Id)
                .ForeignKey("dbo.tabContact", t => t.contact_Id)
                .ForeignKey("dbo.Designations", t => t.Desig_DesigId)
                .ForeignKey("dbo.tabPerson", t => t.person_Id)
                .Index(t => t.address_Id)
                .Index(t => t.contact_Id)
                .Index(t => t.Desig_DesigId)
                .Index(t => t.person_Id);
            
            CreateTable(
                "dbo.Designations",
                c => new
                    {
                        DesigId = c.Int(nullable: false, identity: true),
                        Desig = c.String(),
                    })
                .PrimaryKey(t => t.DesigId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        employeeId = c.Int(nullable: false),
                        userName = c.String(maxLength: 25, unicode: false),
                        password = c.String(),
                        isActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Employees", t => t.employeeId, cascadeDelete: true)
                .Index(t => t.employeeId)
                .Index(t => t.userName, unique: true);
            
            CreateTable(
                "dbo.IndustryTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        isApproved = c.Boolean(nullable: false),
                        isActive = c.Boolean(nullable: false),
                        user_Id = c.Int(),
                        addedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.user_Id)
                .Index(t => t.user_Id);
            
            CreateTable(
                "dbo.Incoterms",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        term = c.String(),
                        discription = c.String(),
                        isActive = c.Boolean(nullable: false),
                        user_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.user_Id)
                .Index(t => t.user_Id);
            
            CreateTable(
                "dbo.Inquiries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        referenceNo = c.String(),
                        fileNo = c.String(),
                        inquiryDate = c.DateTime(nullable: false),
                        responseDate = c.DateTime(nullable: false),
                        alertDate = c.DateTime(nullable: false),
                        closingDate = c.DateTime(nullable: false),
                        customerCompany_Id = c.Int(nullable: false),
                        dept_Id = c.Int(nullable: false),
                        company_Id = c.Int(),
                        user_Id = c.Int(nullable: false),
                        allocation_Id = c.Int(nullable: false),
                        inquirytype = c.Int(nullable: false),
                        inquiryStatus_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tabCompany", t => t.company_Id)
                .ForeignKey("dbo.CustomerCompanies", t => t.customerCompany_Id, cascadeDelete: true)
                .ForeignKey("dbo.tabDepartment", t => t.dept_Id, cascadeDelete: true)
                .ForeignKey("dbo.Employees", t => t.allocation_Id, cascadeDelete: true)
                .ForeignKey("dbo.InquiryStatus", t => t.inquiryStatus_Id)
                .ForeignKey("dbo.Users", t => t.user_Id, cascadeDelete: false)
                .Index(t => t.customerCompany_Id)
                .Index(t => t.dept_Id)
                .Index(t => t.company_Id)
                .Index(t => t.user_Id)
                .Index(t => t.allocation_Id)
                .Index(t => t.inquiryStatus_Id);
            
            CreateTable(
                "dbo.InquiryStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.String(),
                        isApproved = c.Boolean(nullable: false),
                        isActive = c.Boolean(nullable: false),
                        backcolor = c.String(),
                        forecolor = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.InquiryProducts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UOM = c.String(),
                        ownDiscription = c.String(),
                        quantity = c.Double(nullable: false),
                        product_Id = c.Int(nullable: false),
                        Inquiry_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.product_Id, cascadeDelete: true)
                .ForeignKey("dbo.Inquiries", t => t.Inquiry_Id)
                .Index(t => t.product_Id)
                .Index(t => t.Inquiry_Id);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        item = c.String(),
                        code = c.String(maxLength: 32, unicode: false),
                        nature_Id = c.Int(nullable: false),
                        unitOfMeasureId = c.Int(nullable: false),
                        categoryId = c.Int(nullable: false),
                        itemDescription = c.String(),
                        ownDescription = c.String(),
                        isActive = c.Boolean(nullable: false),
                        user_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProductCategories", t => t.categoryId, cascadeDelete: true)
                .ForeignKey("dbo.ProductNatures", t => t.nature_Id, cascadeDelete: true)
                .ForeignKey("dbo.UnitOfMeasures", t => t.unitOfMeasureId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.user_Id)
                .Index(t => t.code, unique: true)
                .Index(t => t.nature_Id)
                .Index(t => t.unitOfMeasureId)
                .Index(t => t.categoryId)
                .Index(t => t.user_Id);
            
            CreateTable(
                "dbo.ProductCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        category = c.String(),
                        discription = c.String(),
                        parentId = c.Int(),
                        isActive = c.Boolean(nullable: false),
                        user_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProductCategories", t => t.parentId)
                .ForeignKey("dbo.Users", t => t.user_Id)
                .Index(t => t.parentId)
                .Index(t => t.user_Id);
            
            CreateTable(
                "dbo.ProductNatures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        nature = c.String(),
                        isActive = c.Boolean(nullable: false),
                        user_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.user_Id)
                .Index(t => t.user_Id);
            
            CreateTable(
                "dbo.UnitOfMeasures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        unitOfMeasure = c.String(),
                        isActive = c.Boolean(nullable: false),
                        user_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.user_Id)
                .Index(t => t.user_Id);
            
            CreateTable(
                "dbo.Offers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        referenceNo = c.String(),
                        fileNo = c.String(),
                        ourreferenceNo = c.String(),
                        offerDate = c.DateTime(nullable: false),
                        offerValidityDate = c.DateTime(nullable: false),
                        deliveryDate = c.DateTime(nullable: false),
                        maker = c.String(),
                        origin = c.String(),
                        responseDate = c.DateTime(nullable: false),
                        exchngeRate = c.Single(nullable: false),
                        bidOpenDate = c.DateTime(nullable: false),
                        alertDate = c.DateTime(nullable: false),
                        closingDate = c.DateTime(nullable: false),
                        commision = c.String(),
                        comments = c.String(),
                        deliveryTime = c.String(),
                        totalFOBValue = c.Double(nullable: false),
                        totalCFRValue = c.Double(nullable: false),
                        isPercentTax = c.Boolean(nullable: false),
                        salesTax = c.Double(nullable: false),
                        customerCompany_Id = c.Int(nullable: false),
                        dept_Id = c.Int(nullable: false),
                        user_Id = c.Int(),
                        allocation_Id = c.Int(nullable: false),
                        vendor_Id = c.Int(nullable: false),
                        company_Id = c.Int(),
                        offertype = c.Int(nullable: false),
                        inquiry_Id = c.Int(),
                        bid_Id = c.Int(),
                        currency_Id = c.Int(),
                        paymentterm_Id = c.Int(nullable: false),
                        incoterm_Id = c.Int(nullable: false),
                        offerStatus_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Bids", t => t.bid_Id)
                .ForeignKey("dbo.tabCompany", t => t.company_Id)
                .ForeignKey("dbo.Currencies", t => t.currency_Id)
                .ForeignKey("dbo.CustomerCompanies", t => t.customerCompany_Id, cascadeDelete: true)
                .ForeignKey("dbo.tabDepartment", t => t.dept_Id, cascadeDelete: true)
                .ForeignKey("dbo.Employees", t => t.allocation_Id, cascadeDelete: true)
                .ForeignKey("dbo.Incoterms", t => t.incoterm_Id, cascadeDelete: true)
                .ForeignKey("dbo.Inquiries", t => t.inquiry_Id)
                .ForeignKey("dbo.OfferStatus", t => t.offerStatus_Id)
                .ForeignKey("dbo.PaymentTerms", t => t.paymentterm_Id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.user_Id)
                .ForeignKey("dbo.tabVendor", t => t.vendor_Id, cascadeDelete: true)
                .Index(t => t.customerCompany_Id)
                .Index(t => t.dept_Id)
                .Index(t => t.user_Id)
                .Index(t => t.allocation_Id)
                .Index(t => t.vendor_Id)
                .Index(t => t.company_Id)
                .Index(t => t.inquiry_Id)
                .Index(t => t.bid_Id)
                .Index(t => t.currency_Id)
                .Index(t => t.paymentterm_Id)
                .Index(t => t.incoterm_Id)
                .Index(t => t.offerStatus_Id);
            
            CreateTable(
                "dbo.OfferStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.String(),
                        isApproved = c.Boolean(nullable: false),
                        isActive = c.Boolean(nullable: false),
                        backcolor = c.String(),
                        forecolor = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PaymentTerms",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        term = c.String(),
                        daysofMonthDue = c.Int(nullable: false),
                        discountPercent = c.Single(nullable: false),
                        discountDays = c.Int(nullable: false),
                        discountonDayofMonth = c.Int(nullable: false),
                        minimumDaytoPay = c.Int(nullable: false),
                        netDueDays = c.Int(nullable: false),
                        type = c.String(),
                        isApproved = c.Boolean(nullable: false),
                        isActive = c.Boolean(nullable: false),
                        user_Id = c.Int(nullable: false),
                        addedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.user_Id, cascadeDelete: false)
                .Index(t => t.user_Id);
            
            CreateTable(
                "dbo.ProcurementProducts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        caption1 = c.String(),
                        value1 = c.Double(nullable: false),
                        caption2 = c.String(),
                        value2 = c.Double(nullable: false),
                        caption3 = c.String(),
                        value3 = c.Double(nullable: false),
                        product_Id = c.Int(nullable: false),
                        Offer_Id = c.Int(),
                        PurchaseOrder_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.InquiryProducts", t => t.product_Id, cascadeDelete: true)
                .ForeignKey("dbo.Offers", t => t.Offer_Id)
                .ForeignKey("dbo.PurchaseOrders", t => t.PurchaseOrder_Id)
                .Index(t => t.product_Id)
                .Index(t => t.Offer_Id)
                .Index(t => t.PurchaseOrder_Id);
            
            CreateTable(
                "dbo.tabVendor",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        billingAddres_Id = c.Int(),
                        company_Id = c.Int(),
                        contact_Id = c.Int(),
                        contactPerson_Id = c.Int(),
                        shippingAddress_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tabAddress", t => t.billingAddres_Id)
                .ForeignKey("dbo.tabCompany", t => t.company_Id)
                .ForeignKey("dbo.tabContact", t => t.contact_Id)
                .ForeignKey("dbo.tabPerson", t => t.contactPerson_Id)
                .ForeignKey("dbo.tabAddress", t => t.shippingAddress_Id)
                .Index(t => t.billingAddres_Id)
                .Index(t => t.company_Id)
                .Index(t => t.contact_Id)
                .Index(t => t.contactPerson_Id)
                .Index(t => t.shippingAddress_Id);
            
            CreateTable(
                "dbo.Principals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        billingAddres_Id = c.Int(),
                        company_Id = c.Int(),
                        contact_Id = c.Int(),
                        contactPerson_Id = c.Int(),
                        shippingAddress_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tabAddress", t => t.billingAddres_Id)
                .ForeignKey("dbo.tabCompany", t => t.company_Id)
                .ForeignKey("dbo.tabContact", t => t.contact_Id)
                .ForeignKey("dbo.tabPerson", t => t.contactPerson_Id)
                .ForeignKey("dbo.tabAddress", t => t.shippingAddress_Id)
                .Index(t => t.billingAddres_Id)
                .Index(t => t.company_Id)
                .Index(t => t.contact_Id)
                .Index(t => t.contactPerson_Id)
                .Index(t => t.shippingAddress_Id);
            
            CreateTable(
                "dbo.PurchaseOrders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        referenceNo = c.String(),
                        fileNo = c.String(),
                        purchaseOrderDate = c.DateTime(nullable: false),
                        deliveryDate = c.DateTime(nullable: false),
                        shipmentDate = c.DateTime(nullable: false),
                        orderConfirmationDate = c.DateTime(nullable: false),
                        billLaddingDate = c.DateTime(nullable: false),
                        lCDate = c.DateTime(nullable: false),
                        materialReciptDate = c.DateTime(nullable: false),
                        targetYear = c.Int(nullable: false),
                        lCnumber = c.String(),
                        exchngeRate = c.Single(nullable: false),
                        commision = c.String(),
                        maker = c.String(),
                        origin = c.String(),
                        comments = c.String(),
                        deliveryTime = c.String(),
                        totalFOBValue = c.Double(nullable: false),
                        totalCFRValue = c.Double(nullable: false),
                        isPercentTax = c.Boolean(nullable: false),
                        salesTax = c.Double(nullable: false),
                        customerCompany_Id = c.Int(nullable: false),
                        dept_Id = c.Int(nullable: false),
                        user_Id = c.Int(),
                        allocation_Id = c.Int(nullable: false),
                        vendor_Id = c.Int(nullable: false),
                        company_Id = c.Int(),
                        purchaseOrdertype = c.Int(nullable: false),
                        offer_Id = c.Int(),
                        bid_Id = c.Int(),
                        currency_Id = c.Int(nullable: false),
                        paymentterm_Id = c.Int(nullable: false),
                        incoterm_Id = c.Int(nullable: false),
                        purchaseOrderStatus_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Bids", t => t.bid_Id)
                .ForeignKey("dbo.tabCompany", t => t.company_Id)
                .ForeignKey("dbo.Currencies", t => t.currency_Id, cascadeDelete: true)
                .ForeignKey("dbo.CustomerCompanies", t => t.customerCompany_Id, cascadeDelete: true)
                .ForeignKey("dbo.tabDepartment", t => t.dept_Id, cascadeDelete: true)
                .ForeignKey("dbo.Employees", t => t.allocation_Id, cascadeDelete: true)
                .ForeignKey("dbo.Incoterms", t => t.incoterm_Id, cascadeDelete: true)
                .ForeignKey("dbo.Offers", t => t.offer_Id)
                .ForeignKey("dbo.PaymentTerms", t => t.paymentterm_Id, cascadeDelete: true)
                .ForeignKey("dbo.SaleOrderStatus", t => t.purchaseOrderStatus_Id)
                .ForeignKey("dbo.Users", t => t.user_Id)
                .ForeignKey("dbo.tabVendor", t => t.vendor_Id, cascadeDelete: true)
                .Index(t => t.customerCompany_Id)
                .Index(t => t.dept_Id)
                .Index(t => t.user_Id)
                .Index(t => t.allocation_Id)
                .Index(t => t.vendor_Id)
                .Index(t => t.company_Id)
                .Index(t => t.offer_Id)
                .Index(t => t.bid_Id)
                .Index(t => t.currency_Id)
                .Index(t => t.paymentterm_Id)
                .Index(t => t.incoterm_Id)
                .Index(t => t.purchaseOrderStatus_Id);
            
            CreateTable(
                "dbo.SaleOrderStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.String(),
                        isApproved = c.Boolean(nullable: false),
                        isActive = c.Boolean(nullable: false),
                        backcolor = c.String(),
                        forecolor = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.tabpUser",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        userName = c.String(),
                        PasswordStored = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DepartmentCompanies",
                c => new
                    {
                        Department_Id = c.Int(nullable: false),
                        Company_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Department_Id, t.Company_Id })
                .ForeignKey("dbo.tabDepartment", t => t.Department_Id, cascadeDelete: true)
                .ForeignKey("dbo.tabCompany", t => t.Company_Id, cascadeDelete: true)
                .Index(t => t.Department_Id)
                .Index(t => t.Company_Id);
            
            CreateTable(
                "dbo.CustomerCompanyDepartments",
                c => new
                    {
                        CustomerCompany_Id = c.Int(nullable: false),
                        Department_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CustomerCompany_Id, t.Department_Id })
                .ForeignKey("dbo.CustomerCompanies", t => t.CustomerCompany_Id, cascadeDelete: true)
                .ForeignKey("dbo.tabDepartment", t => t.Department_Id, cascadeDelete: true)
                .Index(t => t.CustomerCompany_Id)
                .Index(t => t.Department_Id);
            
            CreateTable(
                "dbo.EmployeeCompanies",
                c => new
                    {
                        Employee_EmpId = c.Int(nullable: false),
                        Company_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Employee_EmpId, t.Company_Id })
                .ForeignKey("dbo.Employees", t => t.Employee_EmpId, cascadeDelete: true)
                .ForeignKey("dbo.tabCompany", t => t.Company_Id, cascadeDelete: true)
                .Index(t => t.Employee_EmpId)
                .Index(t => t.Company_Id);
            
            CreateTable(
                "dbo.EmployeeDepartments",
                c => new
                    {
                        Employee_EmpId = c.Int(nullable: false),
                        Department_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Employee_EmpId, t.Department_Id })
                .ForeignKey("dbo.Employees", t => t.Employee_EmpId, cascadeDelete: true)
                .ForeignKey("dbo.tabDepartment", t => t.Department_Id, cascadeDelete: true)
                .Index(t => t.Employee_EmpId)
                .Index(t => t.Department_Id);
            
            CreateStoredProcedure(
                "dbo.Company_Insert",
                p => new
                    {
                        CompanyName = p.String(),
                        industryTypeId = p.Int(),
                        BizType = p.Int(),
                        EmployeerNo = p.String(),
                        openingDate = p.DateTime(),
                        closingDate = p.DateTime(),
                        addressId = p.Int(),
                        contactId = p.Int(),
                        compnayType = p.Int(),
                        IsSubsidary = p.Boolean(),
                        ParentID = p.Int(),
                        CurrencyId = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[tabCompany]([CompanyName], [industryTypeId], [BizType], [EmployeerNo], [openingDate], [closingDate], [addressId], [contactId], [compnayType], [IsSubsidary], [ParentID], [CurrencyId])
                      VALUES (@CompanyName, @industryTypeId, @BizType, @EmployeerNo, @openingDate, @closingDate, @addressId, @contactId, @compnayType, @IsSubsidary, @ParentID, @CurrencyId)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[tabCompany]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[tabCompany] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.Company_Update",
                p => new
                    {
                        Id = p.Int(),
                        CompanyName = p.String(),
                        industryTypeId = p.Int(),
                        BizType = p.Int(),
                        EmployeerNo = p.String(),
                        openingDate = p.DateTime(),
                        closingDate = p.DateTime(),
                        addressId = p.Int(),
                        contactId = p.Int(),
                        compnayType = p.Int(),
                        IsSubsidary = p.Boolean(),
                        ParentID = p.Int(),
                        CurrencyId = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[tabCompany]
                      SET [CompanyName] = @CompanyName, [industryTypeId] = @industryTypeId, [BizType] = @BizType, [EmployeerNo] = @EmployeerNo, [openingDate] = @openingDate, [closingDate] = @closingDate, [addressId] = @addressId, [contactId] = @contactId, [compnayType] = @compnayType, [IsSubsidary] = @IsSubsidary, [ParentID] = @ParentID, [CurrencyId] = @CurrencyId
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.Company_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[tabCompany]
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.Company_Delete");
            DropStoredProcedure("dbo.Company_Update");
            DropStoredProcedure("dbo.Company_Insert");
            DropForeignKey("dbo.PurchaseOrders", "vendor_Id", "dbo.tabVendor");
            DropForeignKey("dbo.PurchaseOrders", "user_Id", "dbo.Users");
            DropForeignKey("dbo.PurchaseOrders", "purchaseOrderStatus_Id", "dbo.SaleOrderStatus");
            DropForeignKey("dbo.ProcurementProducts", "PurchaseOrder_Id", "dbo.PurchaseOrders");
            DropForeignKey("dbo.PurchaseOrders", "paymentterm_Id", "dbo.PaymentTerms");
            DropForeignKey("dbo.PurchaseOrders", "offer_Id", "dbo.Offers");
            DropForeignKey("dbo.PurchaseOrders", "incoterm_Id", "dbo.Incoterms");
            DropForeignKey("dbo.PurchaseOrders", "allocation_Id", "dbo.Employees");
            DropForeignKey("dbo.PurchaseOrders", "dept_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.PurchaseOrders", "customerCompany_Id", "dbo.CustomerCompanies");
            DropForeignKey("dbo.PurchaseOrders", "currency_Id", "dbo.Currencies");
            DropForeignKey("dbo.PurchaseOrders", "company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.PurchaseOrders", "bid_Id", "dbo.Bids");
            DropForeignKey("dbo.Principals", "shippingAddress_Id", "dbo.tabAddress");
            DropForeignKey("dbo.Principals", "contactPerson_Id", "dbo.tabPerson");
            DropForeignKey("dbo.Principals", "contact_Id", "dbo.tabContact");
            DropForeignKey("dbo.Principals", "company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.Principals", "billingAddres_Id", "dbo.tabAddress");
            DropForeignKey("dbo.Offers", "vendor_Id", "dbo.tabVendor");
            DropForeignKey("dbo.tabVendor", "shippingAddress_Id", "dbo.tabAddress");
            DropForeignKey("dbo.tabVendor", "contactPerson_Id", "dbo.tabPerson");
            DropForeignKey("dbo.tabVendor", "contact_Id", "dbo.tabContact");
            DropForeignKey("dbo.tabVendor", "company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.tabVendor", "billingAddres_Id", "dbo.tabAddress");
            DropForeignKey("dbo.Offers", "user_Id", "dbo.Users");
            DropForeignKey("dbo.ProcurementProducts", "Offer_Id", "dbo.Offers");
            DropForeignKey("dbo.ProcurementProducts", "product_Id", "dbo.InquiryProducts");
            DropForeignKey("dbo.Offers", "paymentterm_Id", "dbo.PaymentTerms");
            DropForeignKey("dbo.PaymentTerms", "user_Id", "dbo.Users");
            DropForeignKey("dbo.Offers", "offerStatus_Id", "dbo.OfferStatus");
            DropForeignKey("dbo.Offers", "inquiry_Id", "dbo.Inquiries");
            DropForeignKey("dbo.Offers", "incoterm_Id", "dbo.Incoterms");
            DropForeignKey("dbo.Offers", "allocation_Id", "dbo.Employees");
            DropForeignKey("dbo.Offers", "dept_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.Offers", "customerCompany_Id", "dbo.CustomerCompanies");
            DropForeignKey("dbo.Offers", "currency_Id", "dbo.Currencies");
            DropForeignKey("dbo.Offers", "company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.Offers", "bid_Id", "dbo.Bids");
            DropForeignKey("dbo.Inquiries", "user_Id", "dbo.Users");
            DropForeignKey("dbo.InquiryProducts", "Inquiry_Id", "dbo.Inquiries");
            DropForeignKey("dbo.InquiryProducts", "product_Id", "dbo.Products");
            DropForeignKey("dbo.Products", "user_Id", "dbo.Users");
            DropForeignKey("dbo.Products", "unitOfMeasureId", "dbo.UnitOfMeasures");
            DropForeignKey("dbo.UnitOfMeasures", "user_Id", "dbo.Users");
            DropForeignKey("dbo.Products", "nature_Id", "dbo.ProductNatures");
            DropForeignKey("dbo.ProductNatures", "user_Id", "dbo.Users");
            DropForeignKey("dbo.ProductCategories", "user_Id", "dbo.Users");
            DropForeignKey("dbo.Products", "categoryId", "dbo.ProductCategories");
            DropForeignKey("dbo.ProductCategories", "parentId", "dbo.ProductCategories");
            DropForeignKey("dbo.Inquiries", "inquiryStatus_Id", "dbo.InquiryStatus");
            DropForeignKey("dbo.Inquiries", "allocation_Id", "dbo.Employees");
            DropForeignKey("dbo.Inquiries", "dept_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.Inquiries", "customerCompany_Id", "dbo.CustomerCompanies");
            DropForeignKey("dbo.Inquiries", "company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.Incoterms", "user_Id", "dbo.Users");
            DropForeignKey("dbo.tabCompany", "ParentID", "dbo.tabCompany");
            DropForeignKey("dbo.tabCompany", "industryTypeId", "dbo.IndustryTypes");
            DropForeignKey("dbo.IndustryTypes", "user_Id", "dbo.Users");
            DropForeignKey("dbo.tabDepartment", "userID", "dbo.Users");
            DropForeignKey("dbo.Users", "employeeId", "dbo.Employees");
            DropForeignKey("dbo.tabDepartment", "ParentID", "dbo.tabDepartment");
            DropForeignKey("dbo.Employees", "person_Id", "dbo.tabPerson");
            DropForeignKey("dbo.Employees", "Desig_DesigId", "dbo.Designations");
            DropForeignKey("dbo.EmployeeDepartments", "Department_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.EmployeeDepartments", "Employee_EmpId", "dbo.Employees");
            DropForeignKey("dbo.Employees", "contact_Id", "dbo.tabContact");
            DropForeignKey("dbo.EmployeeCompanies", "Company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.EmployeeCompanies", "Employee_EmpId", "dbo.Employees");
            DropForeignKey("dbo.Employees", "address_Id", "dbo.tabAddress");
            DropForeignKey("dbo.CustomerCompanies", "shippingAddress_Id", "dbo.tabAddress");
            DropForeignKey("dbo.CustomerCompanies", "ParentID", "dbo.CustomerCompanies");
            DropForeignKey("dbo.CustomerCompanyDepartments", "Department_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.CustomerCompanyDepartments", "CustomerCompany_Id", "dbo.CustomerCompanies");
            DropForeignKey("dbo.CustomerCompanies", "contactPerson_Id", "dbo.tabPerson");
            DropForeignKey("dbo.CustomerCompanies", "contact_Id", "dbo.tabContact");
            DropForeignKey("dbo.CustomerCompanies", "company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.CustomerCompanies", "billingAddres_Id", "dbo.tabAddress");
            DropForeignKey("dbo.DepartmentCompanies", "Company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.DepartmentCompanies", "Department_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.tabCompany", "CurrencyId", "dbo.Currencies");
            DropForeignKey("dbo.tabCompany", "contactId", "dbo.tabContact");
            DropForeignKey("dbo.tabCompany", "addressId", "dbo.tabAddress");
            DropIndex("dbo.EmployeeDepartments", new[] { "Department_Id" });
            DropIndex("dbo.EmployeeDepartments", new[] { "Employee_EmpId" });
            DropIndex("dbo.EmployeeCompanies", new[] { "Company_Id" });
            DropIndex("dbo.EmployeeCompanies", new[] { "Employee_EmpId" });
            DropIndex("dbo.CustomerCompanyDepartments", new[] { "Department_Id" });
            DropIndex("dbo.CustomerCompanyDepartments", new[] { "CustomerCompany_Id" });
            DropIndex("dbo.DepartmentCompanies", new[] { "Company_Id" });
            DropIndex("dbo.DepartmentCompanies", new[] { "Department_Id" });
            DropIndex("dbo.PurchaseOrders", new[] { "purchaseOrderStatus_Id" });
            DropIndex("dbo.PurchaseOrders", new[] { "incoterm_Id" });
            DropIndex("dbo.PurchaseOrders", new[] { "paymentterm_Id" });
            DropIndex("dbo.PurchaseOrders", new[] { "currency_Id" });
            DropIndex("dbo.PurchaseOrders", new[] { "bid_Id" });
            DropIndex("dbo.PurchaseOrders", new[] { "offer_Id" });
            DropIndex("dbo.PurchaseOrders", new[] { "company_Id" });
            DropIndex("dbo.PurchaseOrders", new[] { "vendor_Id" });
            DropIndex("dbo.PurchaseOrders", new[] { "allocation_Id" });
            DropIndex("dbo.PurchaseOrders", new[] { "user_Id" });
            DropIndex("dbo.PurchaseOrders", new[] { "dept_Id" });
            DropIndex("dbo.PurchaseOrders", new[] { "customerCompany_Id" });
            DropIndex("dbo.Principals", new[] { "shippingAddress_Id" });
            DropIndex("dbo.Principals", new[] { "contactPerson_Id" });
            DropIndex("dbo.Principals", new[] { "contact_Id" });
            DropIndex("dbo.Principals", new[] { "company_Id" });
            DropIndex("dbo.Principals", new[] { "billingAddres_Id" });
            DropIndex("dbo.tabVendor", new[] { "shippingAddress_Id" });
            DropIndex("dbo.tabVendor", new[] { "contactPerson_Id" });
            DropIndex("dbo.tabVendor", new[] { "contact_Id" });
            DropIndex("dbo.tabVendor", new[] { "company_Id" });
            DropIndex("dbo.tabVendor", new[] { "billingAddres_Id" });
            DropIndex("dbo.ProcurementProducts", new[] { "PurchaseOrder_Id" });
            DropIndex("dbo.ProcurementProducts", new[] { "Offer_Id" });
            DropIndex("dbo.ProcurementProducts", new[] { "product_Id" });
            DropIndex("dbo.PaymentTerms", new[] { "user_Id" });
            DropIndex("dbo.Offers", new[] { "offerStatus_Id" });
            DropIndex("dbo.Offers", new[] { "incoterm_Id" });
            DropIndex("dbo.Offers", new[] { "paymentterm_Id" });
            DropIndex("dbo.Offers", new[] { "currency_Id" });
            DropIndex("dbo.Offers", new[] { "bid_Id" });
            DropIndex("dbo.Offers", new[] { "inquiry_Id" });
            DropIndex("dbo.Offers", new[] { "company_Id" });
            DropIndex("dbo.Offers", new[] { "vendor_Id" });
            DropIndex("dbo.Offers", new[] { "allocation_Id" });
            DropIndex("dbo.Offers", new[] { "user_Id" });
            DropIndex("dbo.Offers", new[] { "dept_Id" });
            DropIndex("dbo.Offers", new[] { "customerCompany_Id" });
            DropIndex("dbo.UnitOfMeasures", new[] { "user_Id" });
            DropIndex("dbo.ProductNatures", new[] { "user_Id" });
            DropIndex("dbo.ProductCategories", new[] { "user_Id" });
            DropIndex("dbo.ProductCategories", new[] { "parentId" });
            DropIndex("dbo.Products", new[] { "user_Id" });
            DropIndex("dbo.Products", new[] { "categoryId" });
            DropIndex("dbo.Products", new[] { "unitOfMeasureId" });
            DropIndex("dbo.Products", new[] { "nature_Id" });
            DropIndex("dbo.Products", new[] { "code" });
            DropIndex("dbo.InquiryProducts", new[] { "Inquiry_Id" });
            DropIndex("dbo.InquiryProducts", new[] { "product_Id" });
            DropIndex("dbo.Inquiries", new[] { "inquiryStatus_Id" });
            DropIndex("dbo.Inquiries", new[] { "allocation_Id" });
            DropIndex("dbo.Inquiries", new[] { "user_Id" });
            DropIndex("dbo.Inquiries", new[] { "company_Id" });
            DropIndex("dbo.Inquiries", new[] { "dept_Id" });
            DropIndex("dbo.Inquiries", new[] { "customerCompany_Id" });
            DropIndex("dbo.Incoterms", new[] { "user_Id" });
            DropIndex("dbo.IndustryTypes", new[] { "user_Id" });
            DropIndex("dbo.Users", new[] { "userName" });
            DropIndex("dbo.Users", new[] { "employeeId" });
            DropIndex("dbo.Employees", new[] { "person_Id" });
            DropIndex("dbo.Employees", new[] { "Desig_DesigId" });
            DropIndex("dbo.Employees", new[] { "contact_Id" });
            DropIndex("dbo.Employees", new[] { "address_Id" });
            DropIndex("dbo.CustomerCompanies", new[] { "shippingAddress_Id" });
            DropIndex("dbo.CustomerCompanies", new[] { "contactPerson_Id" });
            DropIndex("dbo.CustomerCompanies", new[] { "contact_Id" });
            DropIndex("dbo.CustomerCompanies", new[] { "company_Id" });
            DropIndex("dbo.CustomerCompanies", new[] { "billingAddres_Id" });
            DropIndex("dbo.CustomerCompanies", new[] { "ParentID" });
            DropIndex("dbo.tabDepartment", new[] { "ParentID" });
            DropIndex("dbo.tabDepartment", new[] { "userID" });
            DropIndex("dbo.tabCompany", new[] { "CurrencyId" });
            DropIndex("dbo.tabCompany", new[] { "ParentID" });
            DropIndex("dbo.tabCompany", new[] { "contactId" });
            DropIndex("dbo.tabCompany", new[] { "addressId" });
            DropIndex("dbo.tabCompany", new[] { "industryTypeId" });
            DropTable("dbo.EmployeeDepartments");
            DropTable("dbo.EmployeeCompanies");
            DropTable("dbo.CustomerCompanyDepartments");
            DropTable("dbo.DepartmentCompanies");
            DropTable("dbo.tabpUser");
            DropTable("dbo.SaleOrderStatus");
            DropTable("dbo.PurchaseOrders");
            DropTable("dbo.Principals");
            DropTable("dbo.tabVendor");
            DropTable("dbo.ProcurementProducts");
            DropTable("dbo.PaymentTerms");
            DropTable("dbo.OfferStatus");
            DropTable("dbo.Offers");
            DropTable("dbo.UnitOfMeasures");
            DropTable("dbo.ProductNatures");
            DropTable("dbo.ProductCategories");
            DropTable("dbo.Products");
            DropTable("dbo.InquiryProducts");
            DropTable("dbo.InquiryStatus");
            DropTable("dbo.Inquiries");
            DropTable("dbo.Incoterms");
            DropTable("dbo.IndustryTypes");
            DropTable("dbo.Users");
            DropTable("dbo.Designations");
            DropTable("dbo.Employees");
            DropTable("dbo.tabPerson");
            DropTable("dbo.CustomerCompanies");
            DropTable("dbo.tabDepartment");
            DropTable("dbo.Currencies");
            DropTable("dbo.tabContact");
            DropTable("dbo.tabCompany");
            DropTable("dbo.Bids");
            DropTable("dbo.tabAddress");
        }
    }
}
