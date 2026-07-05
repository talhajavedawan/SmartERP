namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class temp : DbMigration
    {
        public override void Up()
        {
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
                        mainBankId = c.Int(),
                        IBAN = c.String(),
                        COA_Type = c.Int(),
                        COA_accountId = c.Int(),
                        accountsCategory = c.Int(nullable: false),
                        industryTypeId = c.Int(),
                        vendor_Id = c.Int(),
                        isActive = c.Boolean(nullable: false),
                        isAdjustmentAccount = c.Boolean(nullable: false),
                        bank_Id = c.Int(),
                        company_Id = c.Int(),
                        currency_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Banks", t => t.bank_Id)
                .ForeignKey("dbo.ChartofAccounts", t => t.COA_accountId)
                .ForeignKey("dbo.tabCompany", t => t.company_Id)
                .ForeignKey("dbo.Currencies", t => t.currency_Id)
                .ForeignKey("dbo.IndustryTypes", t => t.industryTypeId)
                .ForeignKey("dbo.MainBanks", t => t.mainBankId)
                .ForeignKey("dbo.tabVendor", t => t.vendor_Id)
                .Index(t => t.mainBankId)
                .Index(t => t.COA_accountId)
                .Index(t => t.industryTypeId)
                .Index(t => t.vendor_Id)
                .Index(t => t.bank_Id)
                .Index(t => t.company_Id)
                .Index(t => t.currency_Id);
            
            CreateTable(
                "dbo.Banks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        bankId = c.Int(),
                        BankName = c.String(),
                        BranchCode = c.String(),
                        Location = c.String(),
                        SwiftCode = c.String(),
                        address_Id = c.Int(),
                        contact_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tabAddress", t => t.address_Id)
                .ForeignKey("dbo.tabContact", t => t.contact_Id)
                .ForeignKey("dbo.MainBanks", t => t.bankId)
                .Index(t => t.bankId)
                .Index(t => t.address_Id)
                .Index(t => t.contact_Id);
            
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
                "dbo.tabCompany",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CompanyName = c.String(),
                        industryTypeId = c.Int(),
                        BizType = c.Int(nullable: false),
                        EmployeerNo = c.String(),
                        CustomerVAT = c.String(),
                        SaleTaxRegistrationNumber = c.String(),
                        openingDate = c.DateTime(nullable: false),
                        closingDate = c.DateTime(),
                        addressId = c.Int(nullable: false),
                        contactId = c.Int(nullable: false),
                        compnayType = c.Int(nullable: false),
                        IsSubsidary = c.Boolean(nullable: false),
                        ParentID = c.Int(),
                        CurrencyId = c.Int(),
                        isActive = c.Boolean(nullable: false),
                        isLinkable = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tabAddress", t => t.addressId, cascadeDelete: true)
                .ForeignKey("dbo.tabContact", t => t.contactId, cascadeDelete: true)
                .ForeignKey("dbo.Currencies", t => t.CurrencyId)
                .ForeignKey("dbo.IndustryTypes", t => t.industryTypeId)
                .ForeignKey("dbo.tabCompany", t => t.ParentID)
                .Index(t => t.industryTypeId)
                .Index(t => t.addressId)
                .Index(t => t.contactId)
                .Index(t => t.ParentID)
                .Index(t => t.CurrencyId);
            
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        EmpId = c.Int(nullable: false, identity: true),
                        coreCompanyId = c.Int(),
                        coreDeptId = c.Int(),
                        MaritalStatus = c.String(),
                        Disability = c.Boolean(nullable: false),
                        DisDescription = c.String(),
                        isActive = c.Boolean(nullable: false),
                        Status = c.Int(nullable: false),
                        JoinDate = c.DateTime(nullable: false),
                        HireDate = c.DateTime(),
                        AllowOpenTransactions = c.Boolean(nullable: false),
                        RetrievalDate = c.DateTime(),
                        InquiryDataRetrievalDate = c.DateTime(),
                        OfferDataRetrievalDate = c.DateTime(),
                        SODataRetrievalDate = c.DateTime(),
                        MemorandumSaleDataRetrievalDate = c.DateTime(),
                        SIDataRetrievalDate = c.DateTime(),
                        PODataRetrievalDate = c.DateTime(),
                        PIDataRetrievalDate = c.DateTime(),
                        VendorBillDataRetrievalDate = c.DateTime(),
                        SRDataRetrievalDate = c.DateTime(),
                        FixedAssetsDataRetrievalDate = c.DateTime(),
                        IBTDataRetrievalDate = c.DateTime(),
                        AdminBillDataRetrievalDate = c.DateTime(),
                        PaymentDataRetrievalDate = c.DateTime(),
                        LoansAdvancesDataRetrievalDate = c.DateTime(),
                        BasicPay = c.Double(nullable: false),
                        SupervisorId = c.Int(),
                        DesignationTitle = c.String(),
                        JobDescription = c.String(),
                        SalesTargetId = c.Int(),
                        EmployeeId = c.String(),
                        HrInfoId = c.Int(),
                        empDescription = c.String(),
                        isAdminBillType = c.Boolean(nullable: false),
                        isTaskType = c.Boolean(nullable: false),
                        isMultiUser = c.Boolean(nullable: false),
                        receivableAccountId = c.Int(),
                        address_Id = c.Int(),
                        address2_Id = c.Int(),
                        contact_Id = c.Int(),
                        Desig_DesigId = c.Int(),
                        emergencyontact_Id = c.Int(),
                        empFunction_Id = c.Int(),
                        employeeApproval_Id = c.Int(),
                        employeeStatus_Id = c.Int(),
                        person_Id = c.Int(),
                    })
                .PrimaryKey(t => t.EmpId)
                .ForeignKey("dbo.tabAddress", t => t.address_Id)
                .ForeignKey("dbo.tabAddress", t => t.address2_Id)
                .ForeignKey("dbo.tabDepartment", t => t.coreDeptId)
                .ForeignKey("dbo.tabContact", t => t.contact_Id)
                .ForeignKey("dbo.Designations", t => t.Desig_DesigId)
                .ForeignKey("dbo.Emergencyontacts", t => t.emergencyontact_Id)
                .ForeignKey("dbo.Functions", t => t.empFunction_Id)
                .ForeignKey("dbo.EmployeeApprovals", t => t.employeeApproval_Id)
                .ForeignKey("dbo.EmployeeWorkingStatus", t => t.employeeStatus_Id)
                .ForeignKey("dbo.EmployeeHRInfoes", t => t.HrInfoId)
                .ForeignKey("dbo.tabPerson", t => t.person_Id)
                .ForeignKey("dbo.ChartofAccounts", t => t.receivableAccountId)
                .ForeignKey("dbo.SalesTargets", t => t.SalesTargetId)
                .ForeignKey("dbo.Employees", t => t.SupervisorId)
                .ForeignKey("dbo.tabCompany", t => t.coreCompanyId)
                .Index(t => t.coreCompanyId)
                .Index(t => t.coreDeptId)
                .Index(t => t.SupervisorId)
                .Index(t => t.SalesTargetId)
                .Index(t => t.HrInfoId)
                .Index(t => t.receivableAccountId)
                .Index(t => t.address_Id)
                .Index(t => t.address2_Id)
                .Index(t => t.contact_Id)
                .Index(t => t.Desig_DesigId)
                .Index(t => t.emergencyontact_Id)
                .Index(t => t.empFunction_Id)
                .Index(t => t.employeeApproval_Id)
                .Index(t => t.employeeStatus_Id)
                .Index(t => t.person_Id);
            
            CreateTable(
                "dbo.Assets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AssetName = c.String(),
                        AssetNatureId = c.Int(),
                        isRentable = c.Boolean(nullable: false),
                        rentalBasis = c.Int(nullable: false),
                        isSubsidary = c.Boolean(nullable: false),
                        parentId = c.Int(),
                        companyId = c.Int(nullable: false),
                        deptId = c.Int(),
                        coOwnerID = c.Int(),
                        mustInsured = c.Boolean(nullable: false),
                        isInsured = c.Boolean(nullable: false),
                        isRented = c.Boolean(nullable: false),
                        isOwned = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(),
                        OwnerId = c.Int(),
                        CoOwnerAssetId = c.Int(),
                        stage = c.String(),
                        isVoid = c.Boolean(nullable: false),
                        isReviewed = c.Boolean(),
                        needReview = c.Boolean(),
                        PendingForClosing = c.Boolean(),
                        PendingForReApproval = c.Boolean(),
                        isApproved = c.Boolean(),
                        ApprovedDate = c.DateTime(),
                        isReApproved = c.Boolean(),
                        ReApprovalDate = c.DateTime(),
                        user_Id = c.Int(),
                        ClosingDate = c.DateTime(),
                        LastStatusChangeDate = c.DateTime(),
                        address_Id = c.Int(),
                        assetStatus_Id = c.Int(),
                        designation_DesigId = c.Int(),
                        handler_id = c.Int(),
                        owner_EmpId = c.Int(),
                        purchaseInfo_Id = c.Int(),
                        RentalAssets_Id = c.Int(),
                        Employee_EmpId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tabAddress", t => t.address_Id)
                .ForeignKey("dbo.AssetNatures", t => t.AssetNatureId)
                .ForeignKey("dbo.AssetOwners", t => t.OwnerId)
                .ForeignKey("dbo.AssetStatus", t => t.assetStatus_Id)
                .ForeignKey("dbo.AssetOwners", t => t.CoOwnerAssetId)
                .ForeignKey("dbo.Employees", t => t.coOwnerID)
                .ForeignKey("dbo.tabDepartment", t => t.deptId)
                .ForeignKey("dbo.Designations", t => t.designation_DesigId)
                .ForeignKey("dbo.Users", t => t.handler_id)
                .ForeignKey("dbo.Employees", t => t.owner_EmpId)
                .ForeignKey("dbo.tabCompany", t => t.companyId, cascadeDelete: true)
                .ForeignKey("dbo.Assets", t => t.parentId)
                .ForeignKey("dbo.PurchaseInfoes", t => t.purchaseInfo_Id)
                .ForeignKey("dbo.RentalAssets", t => t.RentalAssets_Id)
                .ForeignKey("dbo.Users", t => t.user_Id)
                .ForeignKey("dbo.Employees", t => t.Employee_EmpId)
                .Index(t => t.AssetNatureId)
                .Index(t => t.parentId)
                .Index(t => t.companyId)
                .Index(t => t.deptId)
                .Index(t => t.coOwnerID)
                .Index(t => t.OwnerId)
                .Index(t => t.CoOwnerAssetId)
                .Index(t => t.user_Id)
                .Index(t => t.address_Id)
                .Index(t => t.assetStatus_Id)
                .Index(t => t.designation_DesigId)
                .Index(t => t.handler_id)
                .Index(t => t.owner_EmpId)
                .Index(t => t.purchaseInfo_Id)
                .Index(t => t.RentalAssets_Id)
                .Index(t => t.Employee_EmpId);
            
            CreateTable(
                "dbo.AssetNatures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NatureName = c.String(),
                        isSubsdary = c.Boolean(nullable: false),
                        parentId = c.Int(),
                        isActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AssetNatures", t => t.parentId)
                .Index(t => t.parentId);
            
            CreateTable(
                "dbo.AssetOwners",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        creationDate = c.DateTime(nullable: false),
                        isActive = c.Boolean(nullable: false),
                        gender = c.Int(nullable: false),
                        address_Id = c.Int(),
                        contact_Id = c.Int(),
                        owner_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tabAddress", t => t.address_Id)
                .ForeignKey("dbo.tabContact", t => t.contact_Id)
                .ForeignKey("dbo.tabPerson", t => t.owner_Id)
                .Index(t => t.address_Id)
                .Index(t => t.contact_Id)
                .Index(t => t.owner_Id);
            
            CreateTable(
                "dbo.tabContact",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ContactNo = c.String(),
                        ContactNo1 = c.String(),
                        ContactNo2 = c.String(),
                        ContactNo3 = c.String(),
                        SecondaryContact = c.String(),
                        Fax = c.String(),
                        Email = c.String(),
                        Email1 = c.String(),
                        Email2 = c.String(),
                        Email3 = c.String(),
                        Website = c.String(),
                        SMLink1 = c.String(),
                        SMLink2 = c.String(),
                        SMLink3 = c.String(),
                        OfficialSkype = c.String(),
                        OffSkypePassword = c.String(),
                        OfficialTeams = c.String(),
                        OffTeamsPassword = c.String(),
                        PersonalSkype = c.String(),
                        PersonalTeams = c.String(),
                        contactType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.tabPerson",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FName = c.String(),
                        LName = c.String(),
                        FatherName = c.String(),
                        Photo = c.Binary(),
                        Signature = c.Binary(),
                        NextKin = c.String(),
                        CNIC = c.String(),
                        DOB = c.DateTime(),
                        Gender = c.Int(nullable: false),
                        PassportNo = c.String(),
                        BloodGroup = c.String(),
                        CNICexpiryDate = c.DateTime(),
                        passportExpiryDate = c.DateTime(),
                        passportIssueDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PersonPhotoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EmployeePhoto = c.Binary(),
                        PhotoName = c.String(),
                        Person_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tabPerson", t => t.Person_Id)
                .Index(t => t.Person_Id);
            
            CreateTable(
                "dbo.AssetStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.String(),
                        isApproved = c.Boolean(nullable: false),
                        isActive = c.Boolean(),
                        backcolor = c.String(),
                        forecolor = c.String(),
                        HierarchicalIndex = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Designations",
                c => new
                    {
                        DesigId = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        isActive = c.Boolean(nullable: false),
                        ParentId = c.Int(),
                        companyId = c.Int(),
                        departmentId = c.Int(),
                        AnnualLeaveDays = c.Double(nullable: false),
                        AnnualLeaveHours = c.Double(nullable: false),
                        CasualLeaveDays = c.Double(nullable: false),
                        CasualLeaveHours = c.Double(nullable: false),
                        userId = c.Int(),
                    })
                .PrimaryKey(t => t.DesigId)
                .ForeignKey("dbo.tabCompany", t => t.companyId)
                .ForeignKey("dbo.tabDepartment", t => t.departmentId)
                .ForeignKey("dbo.Designations", t => t.ParentId)
                .ForeignKey("dbo.Users", t => t.userId)
                .Index(t => t.ParentId)
                .Index(t => t.companyId)
                .Index(t => t.departmentId)
                .Index(t => t.userId);
            
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
                        IsParent = c.Boolean(nullable: false),
                        IsSubsidary = c.Boolean(nullable: false),
                        IsAdminBillType = c.Boolean(nullable: false),
                        IsManagerial = c.Boolean(nullable: false),
                        IsProcurementType = c.Boolean(nullable: false),
                        IsInquiryType = c.Boolean(nullable: false),
                        IsOfferType = c.Boolean(nullable: false),
                        IsModuleContractType = c.Boolean(nullable: false),
                        IsSaleOrderType = c.Boolean(nullable: false),
                        IsSaleInvoiceType = c.Boolean(nullable: false),
                        IsSaleReceiptType = c.Boolean(nullable: false),
                        IsPurchaseOrderType = c.Boolean(nullable: false),
                        IsPurchaseInvoiceType = c.Boolean(nullable: false),
                        IsPaymentType = c.Boolean(nullable: false),
                        IsInventoryType = c.Boolean(nullable: false),
                        IsVendorBillType = c.Boolean(nullable: false),
                        IsInterBankTransferType = c.Boolean(nullable: false),
                        IsInterCompTransferType = c.Boolean(nullable: false),
                        IsLoansAdvancesType = c.Boolean(nullable: false),
                        IsTaskType = c.Boolean(nullable: false),
                        IsTravelingRecordType = c.Boolean(nullable: false),
                        IsAssetType = c.Boolean(nullable: false),
                        IsRentalContractType = c.Boolean(nullable: false),
                        IsRentalOrderType = c.Boolean(nullable: false),
                        IsRentalInvoiceType = c.Boolean(nullable: false),
                        IsRentalReceiptType = c.Boolean(nullable: false),
                        IsDocumentType = c.Boolean(nullable: false),
                        ParentID = c.Int(),
                        LevelID = c.Int(),
                        isActive = c.Boolean(nullable: false),
                        applyMERasSER = c.Boolean(nullable: false),
                        chartofAccountId = c.Int(),
                        accountPayableId = c.Int(),
                        isLinkable = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ChartofAccounts", t => t.accountPayableId)
                .ForeignKey("dbo.ChartofAccounts", t => t.chartofAccountId)
                .ForeignKey("dbo.DepartmentLevels", t => t.LevelID)
                .ForeignKey("dbo.tabDepartment", t => t.ParentID)
                .ForeignKey("dbo.Users", t => t.userID)
                .Index(t => t.userID)
                .Index(t => t.ParentID)
                .Index(t => t.LevelID)
                .Index(t => t.chartofAccountId)
                .Index(t => t.accountPayableId);
            
            CreateTable(
                "dbo.ChartofAccounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        accountName = c.String(),
                        isActive = c.Boolean(nullable: false),
                        isActiveForTrialBalance = c.Boolean(nullable: false),
                        accountType = c.Int(nullable: false),
                        parentId = c.Int(),
                        userId = c.Int(),
                        description = c.String(),
                        bankAccountNo = c.String(),
                        routingNo = c.String(),
                        accociatedCompany = c.String(),
                        creditCardNo = c.String(),
                        accountNo = c.String(),
                        isOpeningBalance = c.Boolean(nullable: false),
                        creationDate = c.DateTime(),
                        isApproved = c.Boolean(),
                        ApprovedDate = c.DateTime(),
                        isDebitIncrease = c.Boolean(nullable: false),
                        currencyId = c.Int(),
                        stage = c.String(),
                        isReApproved = c.Boolean(nullable: false),
                        ReApprovalDate = c.DateTime(),
                        asOfDate = c.DateTime(),
                        isVoid = c.Boolean(nullable: false),
                        reconcilationDate = c.DateTime(),
                        openingBalance = c.Double(nullable: false),
                        manualBalanceOC = c.Double(nullable: false),
                        manualBalancePKR = c.Double(nullable: false),
                        BalanceOC = c.Double(nullable: false),
                        BalancePKR = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Currencies", t => t.currencyId)
                .ForeignKey("dbo.ChartofAccounts", t => t.parentId)
                .ForeignKey("dbo.Users", t => t.userId)
                .Index(t => t.parentId)
                .Index(t => t.userId)
                .Index(t => t.currencyId);
            
            CreateTable(
                "dbo.AdminBillTypes",
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
                "dbo.Payees",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PayeeName = c.String(),
                        ParentId = c.Int(),
                        isActive = c.Boolean(nullable: false),
                        isSubsidiary = c.Boolean(nullable: false),
                        isVehicleType = c.Boolean(nullable: false),
                        isOwned = c.Boolean(nullable: false),
                        companyId = c.Int(),
                        vehicleOwnerId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tabCompany", t => t.companyId)
                .ForeignKey("dbo.Payees", t => t.ParentId)
                .ForeignKey("dbo.RentedVehicleOwners", t => t.vehicleOwnerId)
                .Index(t => t.ParentId)
                .Index(t => t.companyId)
                .Index(t => t.vehicleOwnerId);
            
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
                "dbo.tabVendor",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        isActive = c.Boolean(nullable: false),
                        isBlackList = c.Boolean(nullable: false),
                        Rating = c.Int(nullable: false),
                        IsSubsidary = c.Boolean(nullable: false),
                        ParentID = c.Int(),
                        vendorNatureId = c.Int(),
                        vendorNatureManualId = c.Int(),
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
                .ForeignKey("dbo.tabVendor", t => t.ParentID)
                .ForeignKey("dbo.tabAddress", t => t.shippingAddress_Id)
                .ForeignKey("dbo.VendorNatures", t => t.vendorNatureId)
                .ForeignKey("dbo.VendorNatureManuals", t => t.vendorNatureManualId)
                .Index(t => t.ParentID)
                .Index(t => t.vendorNatureId)
                .Index(t => t.vendorNatureManualId)
                .Index(t => t.billingAddres_Id)
                .Index(t => t.company_Id)
                .Index(t => t.contact_Id)
                .Index(t => t.contactPerson_Id)
                .Index(t => t.shippingAddress_Id);
            
            CreateTable(
                "dbo.Bills",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        POReferenceNo = c.String(),
                        SOReferenceNo = c.String(),
                        SalesReferenceNo = c.String(),
                        FinanceRefrenceNo = c.String(),
                        VendorName = c.String(),
                        OfferReferenceNo = c.String(),
                        SyetmReferenceNo = c.String(),
                        saleOrderDate = c.DateTime(),
                        BillDate = c.DateTime(),
                        CreationDate = c.DateTime(),
                        ClosingDate = c.DateTime(),
                        LastStatusChangeDate = c.DateTime(),
                        DeliveryDate = c.DateTime(),
                        ShipmentDate = c.DateTime(),
                        OrderConfirmationDate = c.DateTime(),
                        BillOfLaddingDate = c.DateTime(),
                        lCDate = c.DateTime(),
                        MaterialReciptDate = c.DateTime(),
                        RevisedShipmentDate = c.DateTime(),
                        PaymentDueStartDate = c.DateTime(),
                        ExpectedPayment = c.DateTime(),
                        PaymentDueAgeing = c.DateTime(),
                        CreditDays = c.Int(),
                        TargetYear = c.Int(),
                        TargetMonth = c.Int(),
                        LCnumber = c.String(),
                        ExchangeRate = c.Single(),
                        NetCommision = c.Double(),
                        Commision = c.Double(),
                        commisioninBase = c.Double(),
                        SOC_ER = c.Double(),
                        SoAmountSOC_ER = c.Double(),
                        marginExchangeRate = c.Double(),
                        margin = c.Double(),
                        BudgetedMargininBase = c.Double(),
                        SalesBudgetedMargin = c.Double(),
                        BudgetedMarginPercent = c.Double(),
                        RevisedMargin = c.Double(),
                        RevisedMargininBase = c.Double(),
                        SalesRevisedMargin = c.Double(),
                        RevisedMarginPercent = c.Double(),
                        ActualMarginPercent = c.Double(),
                        ActualMargin = c.Double(),
                        ActualMargininBase = c.Double(),
                        SalesActualMargin = c.Double(),
                        transshipment = c.Boolean(),
                        packing = c.String(),
                        LCShipmentDate = c.DateTime(),
                        LCExpiryDate = c.DateTime(),
                        deliveryTerm = c.String(),
                        LCAmedmentNo = c.String(),
                        LCShipmentAmendmentDate = c.DateTime(),
                        LCExpiryAmedmentDate = c.DateTime(),
                        maker = c.String(),
                        origin = c.String(),
                        OwnDescription = c.String(),
                        comments = c.String(),
                        deliveryTime = c.String(),
                        totalFOBValue = c.Double(nullable: false),
                        totalCFRValue = c.Double(nullable: false),
                        RemainingFOBValue = c.Double(nullable: false),
                        RemainingCFRValue = c.Double(nullable: false),
                        totalBaseFOBValue = c.Double(nullable: false),
                        totalBaseCFRValue = c.Double(nullable: false),
                        UnInvoicedTotalWeight = c.Decimal(precision: 18, scale: 2),
                        UnInvoicedTotalQuantity = c.Decimal(precision: 18, scale: 2),
                        ReceivedAmount = c.Double(nullable: false),
                        TotalWeight = c.Decimal(precision: 18, scale: 2),
                        TotalQuantity = c.Decimal(precision: 18, scale: 2),
                        POAmountSER = c.Double(),
                        isPercentTax = c.Boolean(),
                        salesTax = c.Double(),
                        hasTax = c.Boolean(),
                        tax_Id = c.Int(),
                        billWithTax = c.Double(),
                        hasWHT = c.Boolean(),
                        WHT_Id = c.Int(),
                        billAfterTax = c.Double(),
                        SupplierReferenceNo = c.String(),
                        SupplyDate = c.DateTime(),
                        stage = c.String(),
                        InvoiceStage = c.String(),
                        billType_Id = c.Int(),
                        isVoid = c.Boolean(nullable: false),
                        isReviewed = c.Boolean(),
                        needReview = c.Boolean(),
                        PendingForClosing = c.Boolean(),
                        PendingForReApproval = c.Boolean(),
                        isApproved = c.Boolean(),
                        ApprovedDate = c.DateTime(),
                        ReApprovalDate = c.DateTime(),
                        isReApproved = c.Boolean(),
                        saleOrder_Id = c.Int(),
                        purchaseOrder_Id = c.Int(),
                        CostSheet_Id = c.Int(),
                        CommissionSummarySheetId = c.Int(),
                        dept_Id = c.Int(nullable: false),
                        company_Id = c.Int(),
                        InterDepartment_Id = c.Int(),
                        InterCompany_Id = c.Int(),
                        isInterCompany = c.Boolean(),
                        loanAdvanceCompany_Id = c.Int(),
                        loanAdvanceDept_Id = c.Int(),
                        customerCompany_Id = c.Int(nullable: false),
                        SOWarrantyId = c.Int(),
                        POWarrantyId = c.Int(),
                        user_Id = c.Int(),
                        allocation_Id = c.Int(nullable: false),
                        vendorPaymentId = c.Int(),
                        vendor_Id = c.Int(),
                        billVendor_Id = c.Int(),
                        billVendorName = c.String(),
                        POVendor_Id = c.Int(),
                        POVendorName = c.String(),
                        SoPaymentterm_Id = c.Int(),
                        POPaymentterm_Id = c.Int(),
                        bid_Id = c.Int(),
                        currency_Id = c.Int(nullable: false),
                        SOCurrency_Id = c.Int(),
                        incoterm_Id = c.Int(),
                        TitleValue1Id = c.Int(),
                        TitleValue2Id = c.Int(),
                        CardUserId = c.Int(),
                        CreditCardNoId = c.Int(),
                        PettyCashRefId = c.Int(),
                        isDeposit = c.Boolean(),
                        billCategoryId = c.Int(),
                        hasSummary = c.Boolean(nullable: false),
                        managementSummary_Id = c.Int(),
                        SummaryMemo = c.String(),
                        BillRefNoId = c.Int(),
                        vendorBillNature_Id = c.Int(),
                        GLPostingDate = c.DateTime(),
                        taxAmount = c.Double(),
                        transactionHolderId = c.Int(),
                        holderChangeDate = c.DateTime(nullable: false),
                        LoansAdvanceId = c.Int(),
                        statusClass_Id = c.Int(),
                        VATBookRefId = c.Int(),
                        BillStatus_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BillStatus", t => t.BillStatus_Id)
                .ForeignKey("dbo.SaleOrders", t => t.saleOrder_Id)
                .ForeignKey("dbo.Employees", t => t.allocation_Id, cascadeDelete: true)
                .ForeignKey("dbo.Bids", t => t.bid_Id)
                .ForeignKey("dbo.BillCategories", t => t.billCategoryId)
                .ForeignKey("dbo.VendorBillReferences", t => t.BillRefNoId)
                .ForeignKey("dbo.BillTypes", t => t.billType_Id)
                .ForeignKey("dbo.tabVendor", t => t.billVendor_Id)
                .ForeignKey("dbo.CardHolders", t => t.CardUserId)
                .ForeignKey("dbo.CommissionSummarySheets", t => t.CommissionSummarySheetId)
                .ForeignKey("dbo.tabCompany", t => t.company_Id)
                .ForeignKey("dbo.CostSheets", t => t.CostSheet_Id)
                .ForeignKey("dbo.CreditCards", t => t.CreditCardNoId)
                .ForeignKey("dbo.Currencies", t => t.currency_Id, cascadeDelete: true)
                .ForeignKey("dbo.CustomerCompanies", t => t.customerCompany_Id, cascadeDelete: true)
                .ForeignKey("dbo.tabDepartment", t => t.dept_Id, cascadeDelete: true)
                .ForeignKey("dbo.Incoterms", t => t.incoterm_Id)
                .ForeignKey("dbo.tabCompany", t => t.InterCompany_Id)
                .ForeignKey("dbo.tabDepartment", t => t.InterDepartment_Id)
                .ForeignKey("dbo.tabCompany", t => t.loanAdvanceCompany_Id)
                .ForeignKey("dbo.tabDepartment", t => t.loanAdvanceDept_Id)
                .ForeignKey("dbo.LoansAdvances", t => t.LoansAdvanceId)
                .ForeignKey("dbo.ManagementSummaries", t => t.managementSummary_Id)
                .ForeignKey("dbo.BillRefNumbers", t => t.PettyCashRefId)
                .ForeignKey("dbo.PaymentTerms", t => t.POPaymentterm_Id)
                .ForeignKey("dbo.tabVendor", t => t.POVendor_Id)
                .ForeignKey("dbo.Warranties", t => t.POWarrantyId)
                .ForeignKey("dbo.PurchaseOrders", t => t.purchaseOrder_Id)
                .ForeignKey("dbo.Currencies", t => t.SOCurrency_Id)
                .ForeignKey("dbo.PaymentTerms", t => t.SoPaymentterm_Id)
                .ForeignKey("dbo.Warranties", t => t.SOWarrantyId)
                .ForeignKey("dbo.StatusClasses", t => t.statusClass_Id)
                .ForeignKey("dbo.TaxNames", t => t.tax_Id)
                .ForeignKey("dbo.Incoterms", t => t.TitleValue1Id)
                .ForeignKey("dbo.Incoterms", t => t.TitleValue2Id)
                .ForeignKey("dbo.Users", t => t.user_Id)
                .ForeignKey("dbo.VATBookRefNumbers", t => t.VATBookRefId)
                .ForeignKey("dbo.tabVendor", t => t.vendor_Id)
                .ForeignKey("dbo.VendorBillNatures", t => t.vendorBillNature_Id)
                .ForeignKey("dbo.VendorPaymentStatus", t => t.vendorPaymentId)
                .ForeignKey("dbo.TaxNames", t => t.WHT_Id)
                .ForeignKey("dbo.Employees", t => t.transactionHolderId)
                .Index(t => t.tax_Id)
                .Index(t => t.WHT_Id)
                .Index(t => t.billType_Id)
                .Index(t => t.saleOrder_Id)
                .Index(t => t.purchaseOrder_Id)
                .Index(t => t.CostSheet_Id)
                .Index(t => t.CommissionSummarySheetId)
                .Index(t => t.dept_Id)
                .Index(t => t.company_Id)
                .Index(t => t.InterDepartment_Id)
                .Index(t => t.InterCompany_Id)
                .Index(t => t.loanAdvanceCompany_Id)
                .Index(t => t.loanAdvanceDept_Id)
                .Index(t => t.customerCompany_Id)
                .Index(t => t.SOWarrantyId)
                .Index(t => t.POWarrantyId)
                .Index(t => t.user_Id)
                .Index(t => t.allocation_Id)
                .Index(t => t.vendorPaymentId)
                .Index(t => t.vendor_Id)
                .Index(t => t.billVendor_Id)
                .Index(t => t.POVendor_Id)
                .Index(t => t.SoPaymentterm_Id)
                .Index(t => t.POPaymentterm_Id)
                .Index(t => t.bid_Id)
                .Index(t => t.currency_Id)
                .Index(t => t.SOCurrency_Id)
                .Index(t => t.incoterm_Id)
                .Index(t => t.TitleValue1Id)
                .Index(t => t.TitleValue2Id)
                .Index(t => t.CardUserId)
                .Index(t => t.CreditCardNoId)
                .Index(t => t.PettyCashRefId)
                .Index(t => t.billCategoryId)
                .Index(t => t.managementSummary_Id)
                .Index(t => t.BillRefNoId)
                .Index(t => t.vendorBillNature_Id)
                .Index(t => t.transactionHolderId)
                .Index(t => t.LoansAdvanceId)
                .Index(t => t.statusClass_Id)
                .Index(t => t.VATBookRefId)
                .Index(t => t.BillStatus_Id);
            
            CreateTable(
                "dbo.VendorBillAdjustments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        loansAdvanceType = c.Int(nullable: false),
                        transactionGroupId = c.Int(nullable: false),
                        AdjustmentDate = c.DateTime(),
                        ReferenceNo = c.String(),
                        AdjustmentAmount = c.Double(nullable: false),
                        bill_Id = c.Int(),
                        isApproved = c.Boolean(),
                        ApprovedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Bills", t => t.bill_Id)
                .Index(t => t.bill_Id);
            
            CreateTable(
                "dbo.Adjustments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        loansAdvanceType = c.Int(nullable: false),
                        transactionGroupId = c.Int(nullable: false),
                        AdjustmentDate = c.DateTime(),
                        ReferenceNo = c.String(),
                        AdjustmentAmount = c.Double(nullable: false),
                        adminBillId = c.Int(),
                        vendorBill_Id = c.Int(),
                        isApproved = c.Boolean(),
                        ApprovedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tabAdminBill", t => t.adminBillId)
                .ForeignKey("dbo.Bills", t => t.vendorBill_Id)
                .Index(t => t.adminBillId)
                .Index(t => t.vendorBill_Id);
            
            CreateTable(
                "dbo.tabAdminBill",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        billTypes = c.Int(nullable: false),
                        isProgressiveCost = c.Boolean(),
                        template_Id = c.Int(),
                        CreationDate = c.DateTime(),
                        company_Id = c.Int(),
                        dept_Id = c.Int(),
                        emp_Id = c.Int(),
                        assetRentalId = c.Int(),
                        SystemRefNo = c.String(),
                        FinanceRefNo = c.String(),
                        FinanceRefNo2 = c.String(),
                        TransactionDate = c.DateTime(),
                        currency_Id = c.Int(),
                        statusId = c.Int(),
                        creatorId = c.Int(),
                        COA_Id = c.Int(),
                        CoaCredit_Id = c.Int(),
                        adminBillType_Id = c.Int(),
                        vendor_Id = c.Int(),
                        EmployeeForEveryBill = c.Boolean(nullable: false),
                        employeeForBill_Id = c.Int(),
                        payee_Id = c.Int(),
                        Bill_Number = c.String(),
                        BillingMonthFrom = c.DateTime(),
                        BillingMonthTo = c.DateTime(),
                        DueDate = c.DateTime(),
                        AmountOC = c.Double(nullable: false),
                        MER = c.Double(nullable: false),
                        AmountMER = c.Double(nullable: false),
                        TotalAmountOC = c.Double(nullable: false),
                        RemainingAmountOC = c.Double(nullable: false),
                        CardUserId = c.Int(),
                        BankId = c.Int(),
                        CardHolderType = c.Int(),
                        PrimaryCreditCardNoId = c.Int(),
                        SecondaryCreditCardNoId = c.Int(),
                        transactionGroupId = c.Int(nullable: false),
                        Memo = c.String(),
                        BillRefNoId = c.Int(),
                        stage = c.String(),
                        isVoid = c.Boolean(nullable: false),
                        isReviewed = c.Boolean(),
                        needReview = c.Boolean(),
                        PendingForClosing = c.Boolean(),
                        PendingForReApproval = c.Boolean(),
                        isApproved = c.Boolean(),
                        ApprovedDate = c.DateTime(),
                        isReApproved = c.Boolean(),
                        ReApprovalDate = c.DateTime(),
                        user_Id = c.Int(),
                        ClosingDate = c.DateTime(),
                        LastStatusChangeDate = c.DateTime(),
                        isVehicleType = c.Boolean(nullable: false),
                        hasSummary = c.Boolean(nullable: false),
                        managementSummary_Id = c.Int(),
                        SummaryMemo = c.String(),
                        adminBillNature_Id = c.Int(),
                        hasWAT = c.Boolean(),
                        tax_Id = c.Int(),
                        TaxAmount = c.Double(nullable: false),
                        AmountWithTax = c.Double(nullable: false),
                        isPostToGL = c.Boolean(nullable: false),
                        GLPostingDate = c.DateTime(),
                        isDeposit = c.Boolean(),
                        isVATBookPosted = c.Boolean(),
                        isAmountOC = c.Boolean(),
                        LoansAdvanceId = c.Int(),
                        isAdjustedTax = c.Boolean(),
                        statusClass_Id = c.Int(),
                        VATBookRefId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AdminBillNatures", t => t.adminBillNature_Id)
                .ForeignKey("dbo.AdminBillTypes", t => t.adminBillType_Id)
                .ForeignKey("dbo.AssetRentals", t => t.assetRentalId)
                .ForeignKey("dbo.AdminBillStatus", t => t.statusId)
                .ForeignKey("dbo.Banks", t => t.BankId)
                .ForeignKey("dbo.BillRefNumbers", t => t.BillRefNoId)
                .ForeignKey("dbo.CardHolders", t => t.CardUserId)
                .ForeignKey("dbo.ChartofAccounts", t => t.COA_Id)
                .ForeignKey("dbo.ChartofAccounts", t => t.CoaCredit_Id)
                .ForeignKey("dbo.tabCompany", t => t.company_Id)
                .ForeignKey("dbo.Employees", t => t.creatorId)
                .ForeignKey("dbo.Currencies", t => t.currency_Id)
                .ForeignKey("dbo.tabDepartment", t => t.dept_Id)
                .ForeignKey("dbo.Employees", t => t.emp_Id)
                .ForeignKey("dbo.Employees", t => t.employeeForBill_Id)
                .ForeignKey("dbo.LoansAdvances", t => t.LoansAdvanceId)
                .ForeignKey("dbo.ManagementSummaries", t => t.managementSummary_Id)
                .ForeignKey("dbo.Payees", t => t.payee_Id)
                .ForeignKey("dbo.CreditCards", t => t.PrimaryCreditCardNoId)
                .ForeignKey("dbo.CreditCards", t => t.SecondaryCreditCardNoId)
                .ForeignKey("dbo.StatusClasses", t => t.statusClass_Id)
                .ForeignKey("dbo.TaxNames", t => t.tax_Id)
                .ForeignKey("dbo.Templates", t => t.template_Id)
                .ForeignKey("dbo.Users", t => t.user_Id)
                .ForeignKey("dbo.VATBookRefNumbers", t => t.VATBookRefId)
                .ForeignKey("dbo.tabVendor", t => t.vendor_Id)
                .Index(t => t.template_Id)
                .Index(t => t.company_Id)
                .Index(t => t.dept_Id)
                .Index(t => t.emp_Id)
                .Index(t => t.assetRentalId)
                .Index(t => t.currency_Id)
                .Index(t => t.statusId)
                .Index(t => t.creatorId)
                .Index(t => t.COA_Id)
                .Index(t => t.CoaCredit_Id)
                .Index(t => t.adminBillType_Id)
                .Index(t => t.vendor_Id)
                .Index(t => t.employeeForBill_Id)
                .Index(t => t.payee_Id)
                .Index(t => t.CardUserId)
                .Index(t => t.BankId)
                .Index(t => t.PrimaryCreditCardNoId)
                .Index(t => t.SecondaryCreditCardNoId)
                .Index(t => t.BillRefNoId)
                .Index(t => t.user_Id)
                .Index(t => t.managementSummary_Id)
                .Index(t => t.adminBillNature_Id)
                .Index(t => t.tax_Id)
                .Index(t => t.LoansAdvanceId)
                .Index(t => t.statusClass_Id)
                .Index(t => t.VATBookRefId);
            
            CreateTable(
                "dbo.AdminBillNatures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nature = c.String(),
                        isActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AssetRentals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreationDate = c.DateTime(),
                        AssetName = c.String(),
                        companyId = c.Int(),
                        deptId = c.Int(),
                        AssetNatureId = c.Int(),
                        assetSubNatureId = c.Int(),
                        assetBrandId = c.Int(),
                        assetModelId = c.Int(),
                        assetNumberId = c.Int(),
                        SerialNumber = c.String(),
                        assetTypeId = c.Int(),
                        VendorFromSystem = c.Boolean(nullable: false),
                        vendorId = c.Int(),
                        Vendor = c.String(),
                        AssetNumber = c.String(),
                        Description = c.String(),
                        PurchasingDate = c.DateTime(),
                        PurchasingCostManual = c.Double(nullable: false),
                        PurchasingCost = c.Double(nullable: false),
                        ProgressiveCost = c.Double(),
                        ProgressiveCostManual = c.Double(),
                        TotalCostManual = c.Double(),
                        TotalCost = c.Double(),
                        assetHolderEmployeeId = c.Int(),
                        AssetHolder = c.String(),
                        transactionGroupId = c.Int(nullable: false),
                        SystemRef = c.String(),
                        creatorId = c.Int(),
                        isRentable = c.Boolean(nullable: false),
                        isSubsidary = c.Boolean(nullable: false),
                        isLeased = c.Boolean(nullable: false),
                        parentId = c.Int(),
                        countryId = c.Int(),
                        cityId = c.Int(),
                        assetRentalLocationId = c.Int(),
                        assetRentalUnitId = c.Int(),
                        addressId = c.Int(),
                        statusId = c.Int(),
                        stage = c.String(),
                        isVoid = c.Boolean(nullable: false),
                        isReviewed = c.Boolean(),
                        needReview = c.Boolean(),
                        PendingForClosing = c.Boolean(),
                        PendingForReApproval = c.Boolean(),
                        isApproved = c.Boolean(),
                        ApprovedDate = c.DateTime(),
                        isReApproved = c.Boolean(),
                        ReApprovalDate = c.DateTime(),
                        ClosingDate = c.DateTime(),
                        LastStatusChangeDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tabAddress", t => t.addressId)
                .ForeignKey("dbo.AssetBrands", t => t.assetBrandId)
                .ForeignKey("dbo.Employees", t => t.assetHolderEmployeeId)
                .ForeignKey("dbo.AssetModels", t => t.assetModelId)
                .ForeignKey("dbo.RentalAssetNatures", t => t.AssetNatureId)
                .ForeignKey("dbo.AssetNumbers", t => t.assetNumberId)
                .ForeignKey("dbo.AssetRentalLocations", t => t.assetRentalLocationId)
                .ForeignKey("dbo.AssetRentalUnits", t => t.assetRentalUnitId)
                .ForeignKey("dbo.RentalAssetSubNatures", t => t.assetSubNatureId)
                .ForeignKey("dbo.AssetTypes", t => t.assetTypeId)
                .ForeignKey("dbo.Cities", t => t.cityId)
                .ForeignKey("dbo.tabCompany", t => t.companyId)
                .ForeignKey("dbo.Countries", t => t.countryId)
                .ForeignKey("dbo.AssetRentalStatus", t => t.statusId)
                .ForeignKey("dbo.Users", t => t.creatorId)
                .ForeignKey("dbo.tabDepartment", t => t.deptId)
                .ForeignKey("dbo.AssetRentals", t => t.parentId)
                .ForeignKey("dbo.tabVendor", t => t.vendorId)
                .Index(t => t.companyId)
                .Index(t => t.deptId)
                .Index(t => t.AssetNatureId)
                .Index(t => t.assetSubNatureId)
                .Index(t => t.assetBrandId)
                .Index(t => t.assetModelId)
                .Index(t => t.assetNumberId)
                .Index(t => t.assetTypeId)
                .Index(t => t.vendorId)
                .Index(t => t.assetHolderEmployeeId)
                .Index(t => t.creatorId)
                .Index(t => t.parentId)
                .Index(t => t.countryId)
                .Index(t => t.cityId)
                .Index(t => t.assetRentalLocationId)
                .Index(t => t.assetRentalUnitId)
                .Index(t => t.addressId)
                .Index(t => t.statusId);
            
            CreateTable(
                "dbo.AssetBrands",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BrandName = c.String(),
                        assetNatureId = c.Int(),
                        isActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RentalAssetNatures", t => t.assetNatureId)
                .Index(t => t.assetNatureId);
            
            CreateTable(
                "dbo.AssetModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ModelNumber = c.String(),
                        assetNatureId = c.Int(),
                        isActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RentalAssetNatures", t => t.assetNatureId)
                .Index(t => t.assetNatureId);
            
            CreateTable(
                "dbo.RentalAssetNatures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NatureName = c.String(),
                        isActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RentalAssetSubNatures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NatureName = c.String(),
                        assetNatureId = c.Int(),
                        isActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RentalAssetNatures", t => t.assetNatureId)
                .Index(t => t.assetNatureId);
            
            CreateTable(
                "dbo.AssetNumbers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Number = c.String(),
                        isActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AssetRentalLocations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LocationTitle = c.String(),
                        cityId = c.Int(),
                        countryId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cities", t => t.cityId)
                .ForeignKey("dbo.Countries", t => t.countryId)
                .Index(t => t.cityId)
                .Index(t => t.countryId);
            
            CreateTable(
                "dbo.Cities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CityName = c.String(),
                        Abbriviation = c.String(),
                        PostalCode = c.String(),
                        countryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Countries", t => t.countryId, cascadeDelete: true)
                .Index(t => t.countryId);
            
            CreateTable(
                "dbo.Countries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CountryName = c.String(),
                        Abbriviation = c.String(),
                        CountryCode = c.String(),
                        ResidentDays = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AssetRentalUnits",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UnitNo = c.String(),
                        cityId = c.Int(),
                        countryId = c.Int(),
                        assetRentalLocationId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AssetRentalLocations", t => t.assetRentalLocationId)
                .ForeignKey("dbo.Cities", t => t.cityId)
                .ForeignKey("dbo.Countries", t => t.countryId)
                .Index(t => t.cityId)
                .Index(t => t.countryId)
                .Index(t => t.assetRentalLocationId);
            
            CreateTable(
                "dbo.AssetTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TypeName = c.String(),
                        isActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        employeeId = c.Int(nullable: false),
                        userName = c.String(maxLength: 60, unicode: false),
                        password = c.String(),
                        isActive = c.Boolean(nullable: false),
                        isLoggedIn = c.Boolean(nullable: false),
                        isKeyApproved = c.Boolean(nullable: false),
                        machineKey = c.String(),
                        isRDCKeyApproved = c.Boolean(nullable: false),
                        rdcMachineKey = c.String(),
                        LoginTime = c.DateTime(),
                        isBlink = c.Boolean(nullable: false),
                        EmployeePerformanceReview_Id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Employees", t => t.employeeId, cascadeDelete: true)
                .ForeignKey("dbo.EmployeePerformanceReviews", t => t.EmployeePerformanceReview_Id)
                .Index(t => t.employeeId)
                .Index(t => t.userName, unique: true)
                .Index(t => t.EmployeePerformanceReview_Id);
            
            CreateTable(
                "dbo.CommentLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        employeeId = c.Int(),
                        AssigneeId = c.Int(),
                        ReplyCommentId = c.Int(),
                        CategoryId = c.Int(),
                        Timestamp = c.DateTime(nullable: false),
                        Comment = c.String(),
                        Subject = c.String(),
                        isReply = c.Boolean(nullable: false),
                        isRead = c.Boolean(nullable: false),
                        ReadTimestamp = c.DateTime(),
                        TransactionType = c.Int(nullable: false),
                        TransactionId = c.Int(nullable: false),
                        billSystemRef = c.String(),
                        poSystemRef = c.String(),
                        FlagId = c.Int(),
                        managerId = c.Int(),
                        salesPersonId = c.Int(),
                        financePersonId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CommentCategories", t => t.CategoryId)
                .ForeignKey("dbo.Employees", t => t.financePersonId)
                .ForeignKey("dbo.Employees", t => t.managerId)
                .ForeignKey("dbo.NotificationFlags", t => t.FlagId)
                .ForeignKey("dbo.CommentLogs", t => t.ReplyCommentId)
                .ForeignKey("dbo.Employees", t => t.salesPersonId)
                .ForeignKey("dbo.Employees", t => t.AssigneeId)
                .ForeignKey("dbo.Employees", t => t.employeeId)
                .Index(t => t.employeeId)
                .Index(t => t.AssigneeId)
                .Index(t => t.ReplyCommentId)
                .Index(t => t.CategoryId)
                .Index(t => t.FlagId)
                .Index(t => t.managerId)
                .Index(t => t.salesPersonId)
                .Index(t => t.financePersonId);
            
            CreateTable(
                "dbo.CommentCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        category = c.String(),
                        discription = c.String(),
                        isActive = c.Boolean(nullable: false),
                        user_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.user_Id)
                .Index(t => t.user_Id);
            
            CreateTable(
                "dbo.TransactionItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TransactionType = c.Int(nullable: false),
                        CommentCategory_Id = c.Int(),
                        AttachmentCategory_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CommentCategories", t => t.CommentCategory_Id)
                .ForeignKey("dbo.AttachmentCategories", t => t.AttachmentCategory_Id)
                .Index(t => t.CommentCategory_Id)
                .Index(t => t.AttachmentCategory_Id);
            
            CreateTable(
                "dbo.NotificationFlags",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Flag = c.String(),
                        isApproved = c.Boolean(nullable: false),
                        isActive = c.Boolean(),
                        backcolor = c.String(),
                        forecolor = c.String(),
                        HierarchicalIndex = c.Int(nullable: false),
                        canGlow = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Notifications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(),
                        CcUserId = c.Int(),
                        SendingUserId = c.Int(),
                        Timestamp = c.DateTime(nullable: false),
                        Title = c.String(),
                        Info = c.String(),
                        isRead = c.Boolean(nullable: false),
                        ReadTimestamp = c.DateTime(nullable: false),
                        Description = c.String(),
                        NotificationType = c.Int(nullable: false),
                        TransactionType = c.Int(nullable: false),
                        TransactionId = c.Int(nullable: false),
                        BillReferenceNo = c.String(),
                        PoReferenceNo = c.String(),
                        FlagId = c.Int(),
                        Glow = c.Boolean(nullable: false),
                        commentLogId = c.Int(),
                        isPending = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CcUserId)
                .ForeignKey("dbo.NotificationFlags", t => t.FlagId)
                .ForeignKey("dbo.Users", t => t.SendingUserId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.CcUserId)
                .Index(t => t.SendingUserId)
                .Index(t => t.FlagId);
            
            CreateTable(
                "dbo.Memos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        memoType = c.Int(nullable: false),
                        Subject = c.String(),
                        CreationDate = c.DateTime(),
                        createdById = c.Int(),
                        createdForId = c.Int(),
                        taskGroupId = c.Int(),
                        isVoid = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.createdById)
                .ForeignKey("dbo.TaskGroups", t => t.taskGroupId)
                .ForeignKey("dbo.Users", t => t.createdForId)
                .Index(t => t.createdById)
                .Index(t => t.createdForId)
                .Index(t => t.taskGroupId);
            
            CreateTable(
                "dbo.TaskGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        parentId = c.Int(),
                        creationDate = c.DateTime(nullable: false),
                        GroupName = c.String(),
                        GroupCreatorId = c.Int(),
                        isBackground = c.Boolean(nullable: false),
                        payableAccount_Id = c.Int(),
                        cgsAccount_Id = c.Int(),
                        currency_Id = c.Int(),
                        CalculationType_Id = c.Int(),
                        targetGroup_Id = c.Int(),
                        settingValue = c.String(unicode: false),
                        targetsTransactionType = c.Int(nullable: false),
                        isTitle = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.StatusCalculationTypes", t => t.CalculationType_Id)
                .ForeignKey("dbo.ChartofAccounts", t => t.cgsAccount_Id)
                .ForeignKey("dbo.Currencies", t => t.currency_Id)
                .ForeignKey("dbo.Users", t => t.GroupCreatorId)
                .ForeignKey("dbo.TaskGroups", t => t.parentId)
                .ForeignKey("dbo.ChartofAccounts", t => t.payableAccount_Id)
                .ForeignKey("dbo.TargetGroups", t => t.targetGroup_Id)
                .Index(t => t.parentId)
                .Index(t => t.GroupCreatorId)
                .Index(t => t.payableAccount_Id)
                .Index(t => t.cgsAccount_Id)
                .Index(t => t.currency_Id)
                .Index(t => t.CalculationType_Id)
                .Index(t => t.targetGroup_Id);
            
            CreateTable(
                "dbo.StatusCalculationTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TypeName = c.String(),
                        AchievedField_Id = c.Int(),
                        TotalField_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SoCalculationFields", t => t.AchievedField_Id)
                .ForeignKey("dbo.SoCalculationFields", t => t.TotalField_Id)
                .Index(t => t.AchievedField_Id)
                .Index(t => t.TotalField_Id);
            
            CreateTable(
                "dbo.SoCalculationFields",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DisplayName = c.String(),
                        SOFieldName = c.String(),
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
                        isVoid = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MarketExchangeRates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AddedOn = c.DateTime(nullable: false),
                        effectiveFrom = c.DateTime(nullable: false),
                        effectiveTo = c.DateTime(nullable: false),
                        target_currency_Id = c.Int(),
                        base_currency_Id = c.Int(),
                        Addedbyuser_Id = c.Int(),
                        Editedbyuser_Id = c.Int(),
                        company_Id = c.Int(),
                        LastUpdated = c.DateTime(),
                        isApproved = c.Boolean(),
                        exchangerate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        maxVariationPercent = c.Decimal(precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Addedbyuser_Id)
                .ForeignKey("dbo.Users", t => t.Editedbyuser_Id)
                .ForeignKey("dbo.Currencies", t => t.base_currency_Id)
                .ForeignKey("dbo.Currencies", t => t.target_currency_Id)
                .ForeignKey("dbo.tabCompany", t => t.company_Id)
                .Index(t => t.target_currency_Id)
                .Index(t => t.base_currency_Id)
                .Index(t => t.Addedbyuser_Id)
                .Index(t => t.Editedbyuser_Id)
                .Index(t => t.company_Id);
            
            CreateTable(
                "dbo.SalesExchangeRates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AddedOn = c.DateTime(nullable: false),
                        targetYear = c.Int(nullable: false),
                        effectiveFrom = c.DateTime(nullable: false),
                        effectiveTo = c.DateTime(nullable: false),
                        target_currency_Id = c.Int(),
                        base_currency_Id = c.Int(),
                        Addedbyuser_Id = c.Int(),
                        Editedbyuser_Id = c.Int(),
                        company_Id = c.Int(),
                        LastUpdated = c.DateTime(),
                        isApproved = c.Boolean(),
                        exchangerate = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Addedbyuser_Id)
                .ForeignKey("dbo.Users", t => t.Editedbyuser_Id)
                .ForeignKey("dbo.Currencies", t => t.base_currency_Id)
                .ForeignKey("dbo.Currencies", t => t.target_currency_Id)
                .ForeignKey("dbo.tabCompany", t => t.company_Id)
                .Index(t => t.target_currency_Id)
                .Index(t => t.base_currency_Id)
                .Index(t => t.Addedbyuser_Id)
                .Index(t => t.Editedbyuser_Id)
                .Index(t => t.company_Id);
            
            CreateTable(
                "dbo.TargetGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GroupName = c.String(),
                        parentId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TargetGroups", t => t.parentId)
                .Index(t => t.parentId);
            
            CreateTable(
                "dbo.LoginUserDetails",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        userId = c.Int(),
                        LoginTime = c.DateTime(),
                        LogoutTime = c.DateTime(),
                        crashingDetail = c.String(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Users", t => t.userId)
                .Index(t => t.userId);
            
            CreateTable(
                "dbo.Polls",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreationDate = c.DateTime(),
                        pollingType = c.Int(nullable: false),
                        Title = c.String(),
                        taskGroupId = c.Int(),
                        initiatedById = c.Int(),
                        ValidUntil = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.initiatedById)
                .ForeignKey("dbo.TaskGroups", t => t.taskGroupId)
                .Index(t => t.taskGroupId)
                .Index(t => t.initiatedById);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Company = c.String(),
                        Department = c.String(),
                        RoleName = c.String(),
                        parentId = c.Int(),
                        Added = c.DateTime(nullable: false),
                        LastModified = c.DateTime(nullable: false),
                        isActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Roles", t => t.parentId)
                .Index(t => t.parentId);
            
            CreateTable(
                "dbo.Permissions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Added = c.DateTime(nullable: false),
                        LastModified = c.DateTime(nullable: false),
                        ParentId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Permissions", t => t.ParentId)
                .Index(t => t.ParentId);
            
            CreateTable(
                "dbo.Tasks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        taskTemplate = c.Int(),
                        creationDate = c.DateTime(),
                        SystemId = c.Int(nullable: false),
                        transactionId = c.Int(nullable: false),
                        transactionType = c.Int(),
                        companyId = c.Int(),
                        deptId = c.Int(),
                        customerId = c.Int(),
                        statusId = c.Int(),
                        supervisedById = c.Int(),
                        assignedToId = c.Int(),
                        assignedById = c.Int(),
                        creatorId = c.Int(),
                        currencyId = c.Int(),
                        saleOrderId = c.Int(),
                        purchaseOrderId = c.Int(),
                        saleInvoiceId = c.Int(),
                        inquiryId = c.Int(),
                        offerId = c.Int(),
                        moduleContractId = c.Int(),
                        SystemRef = c.String(),
                        TaskRef = c.String(),
                        ManualAmount = c.Double(nullable: false),
                        StartDate = c.DateTime(),
                        TentativeCompletionDate = c.DateTime(),
                        CompletionDate = c.DateTime(),
                        isCompleted = c.Boolean(nullable: false),
                        Description = c.String(),
                        taskTypeId = c.Int(),
                        vendorId = c.Int(),
                        employeeId = c.Int(),
                        taxableIncome = c.Double(),
                        chargeableTax = c.Double(),
                        NTNno = c.String(),
                        taxYear = c.DateTime(),
                        TaxQuarter = c.Int(),
                        FilerName = c.String(),
                        depositedTax = c.Double(),
                        NoticeRefNo = c.String(),
                        isVoid = c.Boolean(nullable: false),
                        isReviewed = c.Boolean(),
                        needReview = c.Boolean(),
                        PendingForClosing = c.Boolean(),
                        PendingForReApproval = c.Boolean(),
                        isApproved = c.Boolean(),
                        ApprovedDate = c.DateTime(),
                        isReApproved = c.Boolean(),
                        ReApprovalDate = c.DateTime(),
                        ClosingDate = c.DateTime(),
                        LastStatusChangeDate = c.DateTime(),
                        stage = c.String(),
                        OfficeSupportRequired = c.Boolean(nullable: false),
                        LositicSupportRequired = c.Boolean(nullable: false),
                        LogisticAreaFrom = c.String(),
                        LotNo = c.String(),
                        LogisticAreaTo = c.String(),
                        inHouseLogistics = c.Boolean(nullable: false),
                        outsourceLogistics = c.Boolean(nullable: false),
                        PlannedExecutionDate = c.DateTime(),
                        FinalExecutionDate = c.DateTime(),
                        TrackingNoIn = c.String(),
                        ETDin = c.DateTime(),
                        ETAin = c.DateTime(),
                        ADDin = c.DateTime(),
                        TrackingNoOut = c.String(),
                        ETDout = c.DateTime(),
                        ETAout = c.DateTime(),
                        ADDout = c.DateTime(),
                        warehouseId = c.Int(),
                        lotNumberId = c.Int(),
                        ImportBillOfEntry = c.Boolean(nullable: false),
                        ExportBillOfEntry = c.Boolean(nullable: false),
                        checklistId = c.Int(),
                        statusClass_Id = c.Int(),
                        InputTax = c.Double(nullable: false),
                        OutputTax = c.Double(nullable: false),
                        RefundAmountClaim = c.Double(nullable: false),
                        CreditCarriedForward = c.Double(nullable: false),
                        AccumulatedCredit = c.Double(nullable: false),
                        AccumulatedDebit = c.Double(nullable: false),
                        FEDpayable = c.Double(nullable: false),
                        PLpayable = c.Double(nullable: false),
                        SaleTaxPayable = c.Double(nullable: false),
                        TotalAmountPaid = c.Double(nullable: false),
                        TaxFrom = c.DateTime(),
                        TaxTo = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.assignedById)
                .ForeignKey("dbo.Users", t => t.assignedToId)
                .ForeignKey("dbo.TasksStatus", t => t.statusId)
                .ForeignKey("dbo.Checklists", t => t.checklistId)
                .ForeignKey("dbo.tabCompany", t => t.companyId)
                .ForeignKey("dbo.Users", t => t.creatorId)
                .ForeignKey("dbo.Currencies", t => t.currencyId)
                .ForeignKey("dbo.CustomerCompanies", t => t.customerId)
                .ForeignKey("dbo.tabDepartment", t => t.deptId)
                .ForeignKey("dbo.Employees", t => t.employeeId)
                .ForeignKey("dbo.Inquiries", t => t.inquiryId)
                .ForeignKey("dbo.LotNumbers", t => t.lotNumberId)
                .ForeignKey("dbo.ModuleContracts", t => t.moduleContractId)
                .ForeignKey("dbo.Offers", t => t.offerId)
                .ForeignKey("dbo.PurchaseOrders", t => t.purchaseOrderId)
                .ForeignKey("dbo.SaleInvoices", t => t.saleInvoiceId)
                .ForeignKey("dbo.SaleOrders", t => t.saleOrderId)
                .ForeignKey("dbo.StatusClasses", t => t.statusClass_Id)
                .ForeignKey("dbo.Users", t => t.supervisedById)
                .ForeignKey("dbo.TaskTypes", t => t.taskTypeId)
                .ForeignKey("dbo.tabVendor", t => t.vendorId)
                .ForeignKey("dbo.Warehouses", t => t.warehouseId)
                .Index(t => t.companyId)
                .Index(t => t.deptId)
                .Index(t => t.customerId)
                .Index(t => t.statusId)
                .Index(t => t.supervisedById)
                .Index(t => t.assignedToId)
                .Index(t => t.assignedById)
                .Index(t => t.creatorId)
                .Index(t => t.currencyId)
                .Index(t => t.saleOrderId)
                .Index(t => t.purchaseOrderId)
                .Index(t => t.saleInvoiceId)
                .Index(t => t.inquiryId)
                .Index(t => t.offerId)
                .Index(t => t.moduleContractId)
                .Index(t => t.taskTypeId)
                .Index(t => t.vendorId)
                .Index(t => t.employeeId)
                .Index(t => t.warehouseId)
                .Index(t => t.lotNumberId)
                .Index(t => t.checklistId)
                .Index(t => t.statusClass_Id);
            
            CreateTable(
                "dbo.Checklists",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        creationDate = c.DateTime(),
                        TransactionType = c.Int(nullable: false),
                        creatorId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.creatorId)
                .Index(t => t.creatorId);
            
            CreateTable(
                "dbo.ProcurementProducts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        unitPrice = c.Double(nullable: false),
                        caption1 = c.String(),
                        value1 = c.Double(nullable: false),
                        caption2 = c.String(),
                        value2 = c.Double(nullable: false),
                        caption3 = c.String(),
                        value3 = c.Double(nullable: false),
                        UnInvoicedQuantity = c.Double(nullable: false),
                        UnInvoicedWeight = c.Double(nullable: false),
                        InvoicedQuantity = c.Double(nullable: false),
                        InvoicedWeight = c.Double(nullable: false),
                        ReceivedQuantity = c.Double(nullable: false),
                        ReceivedWeight = c.Double(nullable: false),
                        DispatchedQuantity = c.Double(nullable: false),
                        DispatchedWeight = c.Double(nullable: false),
                        PackingDimensions = c.String(),
                        packingStyleId = c.Int(),
                        TotalInvoicedQuantity = c.Double(nullable: false),
                        TotalInvoicedWeight = c.Double(nullable: false),
                        totalCommission = c.Double(nullable: false),
                        UnInvoicedSoAmount = c.Double(nullable: false),
                        totalInvoicedSoAmount = c.Double(nullable: false),
                        NowAmount = c.Double(nullable: false),
                        AmountSOC = c.Double(nullable: false),
                        product_Id = c.Int(nullable: false),
                        fieldId = c.Int(),
                        creditAccountId = c.Int(),
                        debitAccountId = c.Int(),
                        priority = c.Int(nullable: false),
                        interBankTransferId = c.Int(),
                        InterCompanyBankTransfer_Id = c.Int(),
                        SaleInvoice_Id = c.Int(),
                        Offer_Id = c.Int(),
                        MemorandumSale_Id = c.Int(),
                        SaleOrder_Id = c.Int(),
                        PurchaseInvoice_Id = c.Int(),
                        PurchaseOrder_Id = c.Int(),
                        ModuleContract_Id = c.Int(),
                        Checklist_Id = c.Int(),
                        Bill_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.InterCompanyBankTransfers", t => t.InterCompanyBankTransfer_Id)
                .ForeignKey("dbo.SaleInvoices", t => t.SaleInvoice_Id)
                .ForeignKey("dbo.Offers", t => t.Offer_Id)
                .ForeignKey("dbo.MemorandumSales", t => t.MemorandumSale_Id)
                .ForeignKey("dbo.SaleOrders", t => t.SaleOrder_Id)
                .ForeignKey("dbo.PurchaseInvoices", t => t.PurchaseInvoice_Id)
                .ForeignKey("dbo.PurchaseOrders", t => t.PurchaseOrder_Id)
                .ForeignKey("dbo.ModuleContracts", t => t.ModuleContract_Id)
                .ForeignKey("dbo.InterBankTransfers", t => t.interBankTransferId)
                .ForeignKey("dbo.CostSheetFields", t => t.fieldId)
                .ForeignKey("dbo.ChartofAccounts", t => t.creditAccountId)
                .ForeignKey("dbo.ChartofAccounts", t => t.debitAccountId)
                .ForeignKey("dbo.InquiryProducts", t => t.product_Id, cascadeDelete: true)
                .ForeignKey("dbo.PackingStyles", t => t.packingStyleId)
                .ForeignKey("dbo.Checklists", t => t.Checklist_Id)
                .ForeignKey("dbo.Bills", t => t.Bill_Id)
                .Index(t => t.packingStyleId)
                .Index(t => t.product_Id)
                .Index(t => t.fieldId)
                .Index(t => t.creditAccountId)
                .Index(t => t.debitAccountId)
                .Index(t => t.interBankTransferId)
                .Index(t => t.InterCompanyBankTransfer_Id)
                .Index(t => t.SaleInvoice_Id)
                .Index(t => t.Offer_Id)
                .Index(t => t.MemorandumSale_Id)
                .Index(t => t.SaleOrder_Id)
                .Index(t => t.PurchaseInvoice_Id)
                .Index(t => t.PurchaseOrder_Id)
                .Index(t => t.ModuleContract_Id)
                .Index(t => t.Checklist_Id)
                .Index(t => t.Bill_Id);
            
            CreateTable(
                "dbo.CostSheetFields",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AddedbyUserId = c.Int(nullable: false),
                        Timestamp = c.DateTime(nullable: false),
                        Title = c.String(),
                        SortId = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                        isActive = c.Boolean(nullable: false),
                        Maker = c.String(),
                        Origin = c.String(),
                        Packing = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.AddedbyUserId, cascadeDelete: true)
                .Index(t => t.AddedbyUserId);
            
            CreateTable(
                "dbo.JournalTransactions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        debit = c.Double(nullable: false),
                        credit = c.Double(nullable: false),
                        memo = c.String(),
                        creationDate = c.DateTime(),
                        accountId = c.Int(),
                        userId = c.Int(),
                        isAdjustment = c.Boolean(nullable: false),
                        transactionRefno = c.String(),
                        coaTransactionsType = c.Int(nullable: false),
                        journalVoucher_id = c.Int(),
                        InterBankId = c.Int(),
                        SaleInvoiceId = c.Int(),
                        prodId = c.Int(),
                        SaleReceiptId = c.Int(),
                        deptId = c.Int(),
                        taxFlag = c.Boolean(nullable: false),
                        AdminBillId = c.Int(),
                        ReconcilationId = c.Int(),
                        isReconciled = c.Boolean(nullable: false),
                        reconcilationDate = c.DateTime(),
                        reconcilationType = c.Int(nullable: false),
                        Bill_Id = c.Int(),
                        costSheetFieldId = c.Int(),
                        PaymentId = c.Int(),
                        PurchaseInvoiceId = c.Int(),
                        MER = c.Double(nullable: false),
                        total = c.Double(nullable: false),
                        companyId = c.Int(),
                        currencyId = c.Int(),
                        InterCompanyId = c.Int(),
                        TargetRewardId = c.Int(),
                        STLId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ChartofAccounts", t => t.accountId)
                .ForeignKey("dbo.tabAdminBill", t => t.AdminBillId)
                .ForeignKey("dbo.Bills", t => t.Bill_Id)
                .ForeignKey("dbo.tabCompany", t => t.companyId)
                .ForeignKey("dbo.CostSheetFields", t => t.costSheetFieldId)
                .ForeignKey("dbo.Currencies", t => t.currencyId)
                .ForeignKey("dbo.tabDepartment", t => t.deptId)
                .ForeignKey("dbo.JournalVouchers", t => t.journalVoucher_id)
                .ForeignKey("dbo.Payments", t => t.PaymentId)
                .ForeignKey("dbo.InterCompanyBankTransfers", t => t.InterCompanyId)
                .ForeignKey("dbo.STLs", t => t.STLId)
                .ForeignKey("dbo.SaleInvoices", t => t.SaleInvoiceId)
                .ForeignKey("dbo.SalesReceipts", t => t.SaleReceiptId)
                .ForeignKey("dbo.PurchaseInvoices", t => t.PurchaseInvoiceId)
                .ForeignKey("dbo.TargetRewards", t => t.TargetRewardId)
                .ForeignKey("dbo.Products", t => t.prodId)
                .ForeignKey("dbo.InterBankTransfers", t => t.InterBankId)
                .ForeignKey("dbo.Reconcilations", t => t.ReconcilationId)
                .ForeignKey("dbo.Users", t => t.userId)
                .Index(t => t.accountId)
                .Index(t => t.userId)
                .Index(t => t.journalVoucher_id)
                .Index(t => t.InterBankId)
                .Index(t => t.SaleInvoiceId)
                .Index(t => t.prodId)
                .Index(t => t.SaleReceiptId)
                .Index(t => t.deptId)
                .Index(t => t.AdminBillId)
                .Index(t => t.ReconcilationId)
                .Index(t => t.Bill_Id)
                .Index(t => t.costSheetFieldId)
                .Index(t => t.PaymentId)
                .Index(t => t.PurchaseInvoiceId)
                .Index(t => t.companyId)
                .Index(t => t.currencyId)
                .Index(t => t.InterCompanyId)
                .Index(t => t.TargetRewardId)
                .Index(t => t.STLId);
            
            CreateTable(
                "dbo.InterBankTransfers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreationDate = c.DateTime(),
                        TransactionDate = c.DateTime(),
                        InstrumentDate = c.DateTime(),
                        FinanceRefNo = c.String(),
                        company_Id = c.Int(),
                        dept_Id = c.Int(),
                        emp_Id = c.Int(),
                        AmountOC = c.Double(nullable: false),
                        currency_Id = c.Int(),
                        MER = c.Double(nullable: false),
                        AmountMER = c.Double(nullable: false),
                        bankFrom_Id = c.Int(),
                        accountFrom_Id = c.Int(),
                        bankTo_Id = c.Int(),
                        accountTo_Id = c.Int(),
                        transferType = c.Int(nullable: false),
                        SystemRefNo = c.String(),
                        InstrumentNo = c.String(),
                        statusId = c.Int(),
                        transferMethod_Id = c.Int(),
                        stage = c.String(),
                        isVoid = c.Boolean(nullable: false),
                        isReviewed = c.Boolean(),
                        needReview = c.Boolean(),
                        PendingForClosing = c.Boolean(),
                        PendingForReApproval = c.Boolean(),
                        isApproved = c.Boolean(),
                        ApprovedDate = c.DateTime(),
                        isReApproved = c.Boolean(),
                        ReApprovalDate = c.DateTime(),
                        user_Id = c.Int(),
                        ClosingDate = c.DateTime(),
                        LastStatusChangeDate = c.DateTime(),
                        AmountOCPaid = c.Double(nullable: false),
                        AmountMERPaid = c.Double(nullable: false),
                        taxFlag = c.Int(nullable: false),
                        taxTypeId = c.Int(),
                        taxNameId = c.Int(),
                        TaxAmountOC = c.Double(nullable: false),
                        TaxAmountMER = c.Double(nullable: false),
                        industryTypeId = c.Int(),
                        vendor_Id = c.Int(),
                        currencyFromId = c.Int(),
                        AmountFrom = c.Double(nullable: false),
                        MERfrom = c.Double(nullable: false),
                        AmountMERfrom = c.Double(nullable: false),
                        currencyToId = c.Int(),
                        AmountTo = c.Double(nullable: false),
                        MERto = c.Double(nullable: false),
                        AmountMERto = c.Double(nullable: false),
                        AmountER = c.Double(nullable: false),
                        Description = c.String(),
                        GLPostingDate = c.DateTime(),
                        isDeposit = c.Boolean(),
                        isPettyCashAmountOC = c.Boolean(),
                        PettyCashRefId = c.Int(),
                        paymentGroupId = c.Int(nullable: false),
                        receiptGroupId = c.Int(nullable: false),
                        vendorBillId = c.Int(),
                        adminBillGroupId = c.Int(nullable: false),
                        adminBillId = c.Int(),
                        IsAdjustedVATfrom = c.Boolean(),
                        IsAdjustedVATto = c.Boolean(),
                        stlId = c.Int(),
                        statusClass_Id = c.Int(),
                        VATBookRefId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Accounts", t => t.accountFrom_Id)
                .ForeignKey("dbo.Accounts", t => t.accountTo_Id)
                .ForeignKey("dbo.tabAdminBill", t => t.adminBillId)
                .ForeignKey("dbo.Banks", t => t.bankFrom_Id)
                .ForeignKey("dbo.Banks", t => t.bankTo_Id)
                .ForeignKey("dbo.tabCompany", t => t.company_Id)
                .ForeignKey("dbo.Currencies", t => t.currency_Id)
                .ForeignKey("dbo.Currencies", t => t.currencyFromId)
                .ForeignKey("dbo.Currencies", t => t.currencyToId)
                .ForeignKey("dbo.tabDepartment", t => t.dept_Id)
                .ForeignKey("dbo.Employees", t => t.emp_Id)
                .ForeignKey("dbo.IndustryTypes", t => t.industryTypeId)
                .ForeignKey("dbo.STLs", t => t.stlId)
                .ForeignKey("dbo.InterBankTransferStatus", t => t.statusId)
                .ForeignKey("dbo.BillRefNumbers", t => t.PettyCashRefId)
                .ForeignKey("dbo.StatusClasses", t => t.statusClass_Id)
                .ForeignKey("dbo.TaxNames", t => t.taxNameId)
                .ForeignKey("dbo.TaxTypes", t => t.taxTypeId)
                .ForeignKey("dbo.TranferMethods", t => t.transferMethod_Id)
                .ForeignKey("dbo.Users", t => t.user_Id)
                .ForeignKey("dbo.VATBookRefNumbers", t => t.VATBookRefId)
                .ForeignKey("dbo.tabVendor", t => t.vendor_Id)
                .ForeignKey("dbo.Bills", t => t.vendorBillId)
                .Index(t => t.company_Id)
                .Index(t => t.dept_Id)
                .Index(t => t.emp_Id)
                .Index(t => t.currency_Id)
                .Index(t => t.bankFrom_Id)
                .Index(t => t.accountFrom_Id)
                .Index(t => t.bankTo_Id)
                .Index(t => t.accountTo_Id)
                .Index(t => t.statusId)
                .Index(t => t.transferMethod_Id)
                .Index(t => t.user_Id)
                .Index(t => t.taxTypeId)
                .Index(t => t.taxNameId)
                .Index(t => t.industryTypeId)
                .Index(t => t.vendor_Id)
                .Index(t => t.currencyFromId)
                .Index(t => t.currencyToId)
                .Index(t => t.PettyCashRefId)
                .Index(t => t.vendorBillId)
                .Index(t => t.adminBillId)
                .Index(t => t.stlId)
                .Index(t => t.statusClass_Id)
                .Index(t => t.VATBookRefId);
            
            CreateTable(
                "dbo.IBTbankCharges",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        deduction_Id = c.Int(),
                        InterBankTransferFromId = c.Int(),
                        InterBankTransferToId = c.Int(),
                        Amount = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Deductions", t => t.deduction_Id)
                .ForeignKey("dbo.InterBankTransfers", t => t.InterBankTransferFromId)
                .ForeignKey("dbo.InterBankTransfers", t => t.InterBankTransferToId)
                .Index(t => t.deduction_Id)
                .Index(t => t.InterBankTransferFromId)
                .Index(t => t.InterBankTransferToId);
            
            CreateTable(
                "dbo.Deductions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        title = c.String(),
                        isActive = c.Boolean(nullable: false),
                        receiptType = c.Int(nullable: false),
                        chartofAccountId = c.Int(),
                        paymentAmount = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ChartofAccounts", t => t.chartofAccountId)
                .Index(t => t.chartofAccountId);
            
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
                        isVendorType = c.Boolean(nullable: false),
                        isVoid = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.user_Id)
                .Index(t => t.user_Id);
            
            CreateTable(
                "dbo.CustomerCompanies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsSubsidary = c.Boolean(nullable: false),
                        ParentID = c.Int(),
                        isActive = c.Boolean(nullable: false),
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
                "dbo.ContactPersons",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        designation = c.String(),
                        isActive = c.Boolean(nullable: false),
                        customerCompanyId = c.Int(),
                        ReligionId = c.Int(),
                        bank_Id = c.Int(),
                        contact_Id = c.Int(),
                        person_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Banks", t => t.bank_Id)
                .ForeignKey("dbo.tabContact", t => t.contact_Id)
                .ForeignKey("dbo.CustomerCompanies", t => t.customerCompanyId)
                .ForeignKey("dbo.tabPerson", t => t.person_Id)
                .ForeignKey("dbo.Religions", t => t.ReligionId)
                .Index(t => t.customerCompanyId)
                .Index(t => t.ReligionId)
                .Index(t => t.bank_Id)
                .Index(t => t.contact_Id)
                .Index(t => t.person_Id);
            
            CreateTable(
                "dbo.Religions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ReligionName = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AuditYearAdjustments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        user_Id = c.Int(),
                        auditYearAdjustmentType = c.Int(nullable: false),
                        company_Id = c.Int(nullable: false),
                        dept_Id = c.Int(nullable: false),
                        customerCompany_Id = c.Int(nullable: false),
                        allocation_Id = c.Int(),
                        principal_Id = c.Int(nullable: false),
                        auditYear = c.DateTime(),
                        SOAmount = c.Double(nullable: false),
                        auditCurrency_Id = c.Int(),
                        comissionOC = c.Double(nullable: false),
                        netComissionOC = c.Double(nullable: false),
                        comissionAudit = c.Double(nullable: false),
                        netComissionAudit = c.Double(nullable: false),
                        BudgetCost = c.Double(nullable: false),
                        ActualCost = c.Double(nullable: false),
                        SystemCost = c.Double(nullable: false),
                        BudgetMargin = c.Double(nullable: false),
                        ActualMargin = c.Double(nullable: false),
                        SystemMargin = c.Double(nullable: false),
                        BudgetCostAudit = c.Double(nullable: false),
                        ActualCostAudit = c.Double(nullable: false),
                        SystemCostAudit = c.Double(nullable: false),
                        SOAmountAudit = c.Double(nullable: false),
                        BudgetMarginAudit = c.Double(nullable: false),
                        ActualMarginAudit = c.Double(nullable: false),
                        SystemMarginAudit = c.Double(nullable: false),
                        ExhangeRate = c.Double(nullable: false),
                        revenue = c.Double(nullable: false),
                        defferedIncome = c.Double(nullable: false),
                        cgs = c.Double(nullable: false),
                        accountReceivable = c.Double(nullable: false),
                        accountPayable = c.Double(nullable: false),
                        bank = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Currencies", t => t.auditCurrency_Id)
                .ForeignKey("dbo.tabCompany", t => t.company_Id, cascadeDelete: true)
                .ForeignKey("dbo.CustomerCompanies", t => t.customerCompany_Id, cascadeDelete: true)
                .ForeignKey("dbo.tabDepartment", t => t.dept_Id, cascadeDelete: true)
                .ForeignKey("dbo.Employees", t => t.allocation_Id)
                .ForeignKey("dbo.Principals", t => t.principal_Id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.user_Id)
                .Index(t => t.user_Id)
                .Index(t => t.company_Id)
                .Index(t => t.dept_Id)
                .Index(t => t.customerCompany_Id)
                .Index(t => t.allocation_Id)
                .Index(t => t.principal_Id)
                .Index(t => t.auditCurrency_Id);
            
            CreateTable(
                "dbo.Principals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        targetAmount = c.Double(nullable: false),
                        marginTargetAmount = c.Double(nullable: false),
                        isActive = c.Boolean(nullable: false),
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
                .ForeignKey("dbo.Principals", t => t.ParentID)
                .ForeignKey("dbo.tabAddress", t => t.shippingAddress_Id)
                .Index(t => t.ParentID)
                .Index(t => t.billingAddres_Id)
                .Index(t => t.company_Id)
                .Index(t => t.contact_Id)
                .Index(t => t.contactPerson_Id)
                .Index(t => t.shippingAddress_Id);
            
            CreateTable(
                "dbo.InterBankTransferVATs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        taxNameId = c.Int(),
                        InterBankTransferFromId = c.Int(),
                        InterBankTransferToId = c.Int(),
                        Amount = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.InterBankTransfers", t => t.InterBankTransferFromId)
                .ForeignKey("dbo.InterBankTransfers", t => t.InterBankTransferToId)
                .ForeignKey("dbo.TaxNames", t => t.taxNameId)
                .Index(t => t.taxNameId)
                .Index(t => t.InterBankTransferFromId)
                .Index(t => t.InterBankTransferToId);
            
            CreateTable(
                "dbo.TaxNames",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        taxTypeId = c.Int(),
                        percentage = c.Double(nullable: false),
                        COA_Id = c.Int(),
                        isAdjusted = c.Boolean(nullable: false),
                        isManual = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ChartofAccounts", t => t.COA_Id)
                .ForeignKey("dbo.TaxTypes", t => t.taxTypeId)
                .Index(t => t.taxTypeId)
                .Index(t => t.COA_Id);
            
            CreateTable(
                "dbo.TaxTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TypeName = c.String(),
                        isActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.InterBankTransferStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.String(),
                        isApproved = c.Boolean(nullable: false),
                        isActive = c.Boolean(),
                        backcolor = c.String(),
                        forecolor = c.String(),
                        HierarchicalIndex = c.Int(nullable: false),
                        isDisable = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.StatusClasses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClassName = c.String(),
                        transactionType = c.Int(nullable: false),
                        isApproved = c.Boolean(nullable: false),
                        isActive = c.Boolean(),
                        backcolor = c.String(),
                        forecolor = c.String(),
                        isDisable = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AdminBillStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.String(),
                        isApproved = c.Boolean(nullable: false),
                        isActive = c.Boolean(),
                        backcolor = c.String(),
                        forecolor = c.String(),
                        HierarchicalIndex = c.Int(nullable: false),
                        isDisable = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AssetRentalStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.String(),
                        isApproved = c.Boolean(nullable: false),
                        isActive = c.Boolean(),
                        backcolor = c.String(),
                        forecolor = c.String(),
                        HierarchicalIndex = c.Int(nullable: false),
                        isDisable = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BillStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.String(),
                        isApproved = c.Boolean(nullable: false),
                        isActive = c.Boolean(),
                        backcolor = c.String(),
                        forecolor = c.String(),
                        HierarchicalIndex = c.Int(nullable: false),
                        isDisable = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DocumentStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.String(),
                        isApproved = c.Boolean(nullable: false),
                        isActive = c.Boolean(),
                        backcolor = c.String(),
                        forecolor = c.String(),
                        HierarchicalIndex = c.Int(nullable: false),
                        isDisable = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Documents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreationDate = c.DateTime(),
                        documentType_Id = c.Int(),
                        documentTemplate_Id = c.Int(),
                        documentAuthority_Id = c.Int(),
                        dept_Id = c.Int(),
                        company_Id = c.Int(),
                        transactionGroupId = c.Int(nullable: false),
                        SystemRefNo = c.String(),
                        DocumentNo = c.String(),
                        country_Id = c.Int(),
                        creator_Id = c.Int(),
                        status_Id = c.Int(),
                        statusClass_Id = c.Int(),
                        employee_Id = c.Int(),
                        Employee = c.String(),
                        IssueDate = c.DateTime(),
                        ExpiryDate = c.DateTime(),
                        isOpen = c.Boolean(nullable: false),
                        stage = c.String(),
                        isVoid = c.Boolean(nullable: false),
                        isReviewed = c.Boolean(),
                        needReview = c.Boolean(),
                        PendingForClosing = c.Boolean(),
                        PendingForReApproval = c.Boolean(),
                        isApproved = c.Boolean(),
                        ApprovedDate = c.DateTime(),
                        ReApprovalDate = c.DateTime(),
                        isReApproved = c.Boolean(),
                        ClosingDate = c.DateTime(),
                        LastStatusChangeDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Countries", t => t.country_Id)
                .ForeignKey("dbo.Users", t => t.creator_Id)
                .ForeignKey("dbo.DocumentAuthorities", t => t.documentAuthority_Id)
                .ForeignKey("dbo.DocumentTemplates", t => t.documentTemplate_Id)
                .ForeignKey("dbo.DocumentTypes", t => t.documentType_Id)
                .ForeignKey("dbo.Employees", t => t.employee_Id)
                .ForeignKey("dbo.DocumentStatus", t => t.status_Id)
                .ForeignKey("dbo.StatusClasses", t => t.statusClass_Id)
                .ForeignKey("dbo.tabDepartment", t => t.dept_Id)
                .ForeignKey("dbo.tabCompany", t => t.company_Id)
                .Index(t => t.documentType_Id)
                .Index(t => t.documentTemplate_Id)
                .Index(t => t.documentAuthority_Id)
                .Index(t => t.dept_Id)
                .Index(t => t.company_Id)
                .Index(t => t.country_Id)
                .Index(t => t.creator_Id)
                .Index(t => t.status_Id)
                .Index(t => t.statusClass_Id)
                .Index(t => t.employee_Id);
            
            CreateTable(
                "dbo.DocumentAuthorities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AuthorityName = c.String(),
                        documentTemplate_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DocumentTemplates", t => t.documentTemplate_Id)
                .Index(t => t.documentTemplate_Id);
            
            CreateTable(
                "dbo.DocumentTemplates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TemplateName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DocumentTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TypeName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.InquiryStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.String(),
                        isApproved = c.Boolean(nullable: false),
                        isActive = c.Boolean(),
                        backcolor = c.String(),
                        forecolor = c.String(),
                        isVoid = c.Boolean(nullable: false),
                        isDisable = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Inquiries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        referenceNo = c.String(),
                        SalesReferenceNo = c.String(),
                        inquiryDate = c.DateTime(nullable: false),
                        CreationDate = c.DateTime(),
                        ClosingDate = c.DateTime(),
                        LastStatusChangeDate = c.DateTime(),
                        DeliveryDueDate = c.DateTime(nullable: false),
                        alertDate = c.DateTime(nullable: false),
                        TotalWeight = c.Decimal(precision: 18, scale: 2),
                        TotalQuantity = c.Decimal(precision: 18, scale: 2),
                        lastSubmissionDate = c.DateTime(nullable: false),
                        OwnDescription = c.String(),
                        Comments = c.String(),
                        isVoid = c.Boolean(nullable: false),
                        customerCompany_Id = c.Int(nullable: false),
                        dept_Id = c.Int(nullable: false),
                        company_Id = c.Int(),
                        InterDepartment_Id = c.Int(),
                        InterCompany_Id = c.Int(),
                        isInterCompany = c.Boolean(),
                        ApprovedDate = c.DateTime(),
                        isApproved = c.Boolean(),
                        PendingForClosing = c.Boolean(),
                        isReviewed = c.Boolean(),
                        needReview = c.Boolean(),
                        stage = c.String(),
                        user_Id = c.Int(),
                        allocation_Id = c.Int(),
                        inquirytype = c.Int(nullable: false),
                        emailBody = c.String(),
                        emailSubject = c.String(),
                        transactionHolderId = c.Int(),
                        holderChangeDate = c.DateTime(nullable: false),
                        statusClass_Id = c.Int(),
                        inquiryStatus_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CustomerCompanies", t => t.customerCompany_Id, cascadeDelete: true)
                .ForeignKey("dbo.Employees", t => t.allocation_Id)
                .ForeignKey("dbo.InquiryStatus", t => t.inquiryStatus_Id)
                .ForeignKey("dbo.StatusClasses", t => t.statusClass_Id)
                .ForeignKey("dbo.Users", t => t.user_Id)
                .ForeignKey("dbo.tabDepartment", t => t.dept_Id, cascadeDelete: true)
                .ForeignKey("dbo.tabDepartment", t => t.InterDepartment_Id)
                .ForeignKey("dbo.Employees", t => t.transactionHolderId)
                .ForeignKey("dbo.tabCompany", t => t.company_Id)
                .ForeignKey("dbo.tabCompany", t => t.InterCompany_Id)
                .Index(t => t.customerCompany_Id)
                .Index(t => t.dept_Id)
                .Index(t => t.company_Id)
                .Index(t => t.InterDepartment_Id)
                .Index(t => t.InterCompany_Id)
                .Index(t => t.user_Id)
                .Index(t => t.allocation_Id)
                .Index(t => t.transactionHolderId)
                .Index(t => t.statusClass_Id)
                .Index(t => t.inquiryStatus_Id);
            
            CreateTable(
                "dbo.InquiryProducts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UOM = c.String(),
                        ownDiscription = c.String(),
                        quantity = c.Double(nullable: false),
                        Weight = c.Decimal(precision: 18, scale: 2),
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
                        isTitle = c.Boolean(nullable: false),
                        user_Id = c.Int(),
                        productType = c.Int(nullable: false),
                        incomeAccount_id = c.Int(),
                        parentId = c.Int(),
                        cgsAccount_id = c.Int(),
                        company_id = c.Int(),
                        cgsInvenAccount_id = c.Int(),
                        cAssetAccount_id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ChartofAccounts", t => t.cAssetAccount_id)
                .ForeignKey("dbo.ProductCategories", t => t.categoryId, cascadeDelete: true)
                .ForeignKey("dbo.ChartofAccounts", t => t.cgsAccount_id)
                .ForeignKey("dbo.ChartofAccounts", t => t.cgsInvenAccount_id)
                .ForeignKey("dbo.tabCompany", t => t.company_id)
                .ForeignKey("dbo.ChartofAccounts", t => t.incomeAccount_id)
                .ForeignKey("dbo.ProductNatures", t => t.nature_Id, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.parentId)
                .ForeignKey("dbo.UnitOfMeasures", t => t.unitOfMeasureId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.user_Id)
                .Index(t => t.code, unique: true)
                .Index(t => t.nature_Id)
                .Index(t => t.unitOfMeasureId)
                .Index(t => t.categoryId)
                .Index(t => t.user_Id)
                .Index(t => t.incomeAccount_id)
                .Index(t => t.parentId)
                .Index(t => t.cgsAccount_id)
                .Index(t => t.company_id)
                .Index(t => t.cgsInvenAccount_id)
                .Index(t => t.cAssetAccount_id);
            
            CreateTable(
                "dbo.BookerStatementItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        offerId = c.Int(),
                        ModuleContractId = c.Int(),
                        product_Id = c.Int(nullable: false),
                        unit = c.Double(nullable: false),
                        siQuantity = c.Double(nullable: false),
                        quantity = c.Double(nullable: false),
                        siWeight = c.Double(nullable: false),
                        weight = c.Double(nullable: false),
                        amount = c.Double(nullable: false),
                        siAmount = c.Double(nullable: false),
                        amountGST = c.Double(nullable: false),
                        siAmountGST = c.Double(nullable: false),
                        passOnValue = c.Double(nullable: false),
                        siPassOnValue = c.Double(nullable: false),
                        taxNameId = c.Int(),
                        focSamplingId = c.Int(),
                        focValue = c.Double(nullable: false),
                        siFocValue = c.Double(nullable: false),
                        claimDiscountId = c.Int(),
                        claimDiscountValue = c.Double(nullable: false),
                        siClaimDiscountValue = c.Double(nullable: false),
                        netAmount = c.Double(nullable: false),
                        siNetAmount = c.Double(nullable: false),
                        saleOrderId = c.Int(),
                        passOnId = c.Int(),
                        saleInvoiceId = c.Int(),
                        purchaseOrderId = c.Int(),
                        purchaseInvoiceId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ClaimDiscounts", t => t.claimDiscountId)
                .ForeignKey("dbo.FOCSamplings", t => t.focSamplingId)
                .ForeignKey("dbo.ModuleContracts", t => t.ModuleContractId)
                .ForeignKey("dbo.PurchaseOrders", t => t.purchaseOrderId)
                .ForeignKey("dbo.PurchaseInvoices", t => t.purchaseInvoiceId)
                .ForeignKey("dbo.SaleInvoices", t => t.saleInvoiceId)
                .ForeignKey("dbo.SaleOrders", t => t.saleOrderId)
                .ForeignKey("dbo.Offers", t => t.offerId)
                .ForeignKey("dbo.PassOns", t => t.passOnId)
                .ForeignKey("dbo.Products", t => t.product_Id, cascadeDelete: true)
                .ForeignKey("dbo.TaxNames", t => t.taxNameId)
                .Index(t => t.offerId)
                .Index(t => t.ModuleContractId)
                .Index(t => t.product_Id)
                .Index(t => t.taxNameId)
                .Index(t => t.focSamplingId)
                .Index(t => t.claimDiscountId)
                .Index(t => t.saleOrderId)
                .Index(t => t.passOnId)
                .Index(t => t.saleInvoiceId)
                .Index(t => t.purchaseOrderId)
                .Index(t => t.purchaseInvoiceId);
            
            CreateTable(
                "dbo.ClaimDiscounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        discountName = c.String(),
                        isActive = c.Boolean(nullable: false),
                        chartofAccountId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ChartofAccounts", t => t.chartofAccountId)
                .Index(t => t.chartofAccountId);
            
            CreateTable(
                "dbo.FOCSamplings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        samplingtName = c.String(),
                        isActive = c.Boolean(nullable: false),
                        chartofAccountId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ChartofAccounts", t => t.chartofAccountId)
                .Index(t => t.chartofAccountId);
            
            CreateTable(
                "dbo.ModuleContracts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OfferReferenceNo = c.String(),
                        SalesReferenceNo = c.String(),
                        ModuleContractReferenceNo = c.String(),
                        commisionRefrenceNo = c.String(),
                        ModuleContractDate = c.DateTime(),
                        CreationDate = c.DateTime(),
                        LastStatusChangeDate = c.DateTime(),
                        OfferDate = c.DateTime(),
                        ModuleContractValidityDate = c.DateTime(),
                        deliveryDate = c.DateTime(),
                        maker = c.String(),
                        origin = c.String(),
                        OwnDescription = c.String(),
                        isVoid = c.Boolean(nullable: false),
                        responseDate = c.DateTime(),
                        exchngeRate = c.Single(nullable: false),
                        bidOpenDate = c.DateTime(),
                        alertDate = c.DateTime(),
                        closingDate = c.DateTime(),
                        commision = c.Decimal(precision: 18, scale: 2),
                        marginExchangeRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        margin = c.Decimal(precision: 18, scale: 2),
                        comments = c.String(),
                        totalFOBValue = c.Double(nullable: false),
                        totalCFRValue = c.Double(nullable: false),
                        totalBaseFOBValue = c.Double(nullable: false),
                        totalBaseCFRValue = c.Double(nullable: false),
                        isPercentTax = c.Boolean(nullable: false),
                        salesTax = c.Double(nullable: false),
                        TotalWeight = c.Decimal(precision: 18, scale: 2),
                        TotalQuantity = c.Decimal(precision: 18, scale: 2),
                        customerCompany_Id = c.Int(nullable: false),
                        dept_Id = c.Int(nullable: false),
                        user_Id = c.Int(),
                        allocation_Id = c.Int(nullable: false),
                        company_Id = c.Int(),
                        ModuleContracttype = c.Int(nullable: false),
                        principal_Id = c.Int(),
                        offer_Id = c.Int(),
                        isReviewed = c.Boolean(),
                        needReview = c.Boolean(),
                        stage = c.String(),
                        bid_Id = c.Int(),
                        currency_Id = c.Int(),
                        paymentterm_Id = c.Int(nullable: false),
                        incoterm_Id = c.Int(nullable: false),
                        PendingForClosing = c.Boolean(),
                        isApproved = c.Boolean(),
                        ApprovedDate = c.DateTime(),
                        TitleValue1Id = c.Int(),
                        TitleValue2Id = c.Int(),
                        uniqueNumber = c.String(),
                        transactionHolderId = c.Int(),
                        holderChangeDate = c.DateTime(nullable: false),
                        statusClass_Id = c.Int(),
                        InterDepartment_Id = c.Int(),
                        InterCompany_Id = c.Int(),
                        isInterCompany = c.Boolean(),
                        contractValue = c.Double(nullable: false),
                        budgetCost = c.Double(nullable: false),
                        budgetMargin = c.Double(nullable: false),
                        ModuleContractStatus_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Bids", t => t.bid_Id)
                .ForeignKey("dbo.Currencies", t => t.currency_Id)
                .ForeignKey("dbo.CustomerCompanies", t => t.customerCompany_Id, cascadeDelete: true)
                .ForeignKey("dbo.Employees", t => t.allocation_Id, cascadeDelete: true)
                .ForeignKey("dbo.Incoterms", t => t.incoterm_Id, cascadeDelete: true)
                .ForeignKey("dbo.ModuleContractStatus", t => t.ModuleContractStatus_Id)
                .ForeignKey("dbo.Offers", t => t.offer_Id)
                .ForeignKey("dbo.PaymentTerms", t => t.paymentterm_Id, cascadeDelete: true)
                .ForeignKey("dbo.Principals", t => t.principal_Id)
                .ForeignKey("dbo.StatusClasses", t => t.statusClass_Id)
                .ForeignKey("dbo.Incoterms", t => t.TitleValue1Id)
                .ForeignKey("dbo.Incoterms", t => t.TitleValue2Id)
                .ForeignKey("dbo.Employees", t => t.transactionHolderId)
                .ForeignKey("dbo.Users", t => t.user_Id)
                .ForeignKey("dbo.tabDepartment", t => t.dept_Id, cascadeDelete: true)
                .ForeignKey("dbo.tabDepartment", t => t.InterDepartment_Id)
                .ForeignKey("dbo.tabCompany", t => t.company_Id)
                .ForeignKey("dbo.tabCompany", t => t.InterCompany_Id)
                .Index(t => t.customerCompany_Id)
                .Index(t => t.dept_Id)
                .Index(t => t.user_Id)
                .Index(t => t.allocation_Id)
                .Index(t => t.company_Id)
                .Index(t => t.principal_Id)
                .Index(t => t.offer_Id)
                .Index(t => t.bid_Id)
                .Index(t => t.currency_Id)
                .Index(t => t.paymentterm_Id)
                .Index(t => t.incoterm_Id)
                .Index(t => t.TitleValue1Id)
                .Index(t => t.TitleValue2Id)
                .Index(t => t.transactionHolderId)
                .Index(t => t.statusClass_Id)
                .Index(t => t.InterDepartment_Id)
                .Index(t => t.InterCompany_Id)
                .Index(t => t.ModuleContractStatus_Id);
            
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
                "dbo.ComparativeStatements",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        offerId = c.Int(),
                        VendorId = c.Int(),
                        offerValue = c.Double(nullable: false),
                        incoTerm_Id = c.Int(),
                        offerCurrencyId = c.Int(),
                        creatorId = c.Int(),
                        vendorName = c.String(),
                        vendorIncoTermId = c.Int(),
                        productId = c.Int(),
                        isSelect = c.Boolean(nullable: false),
                        ModuleContract_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.creatorId)
                .ForeignKey("dbo.Offers", t => t.offerId)
                .ForeignKey("dbo.Incoterms", t => t.incoTerm_Id)
                .ForeignKey("dbo.Incoterms", t => t.vendorIncoTermId)
                .ForeignKey("dbo.Currencies", t => t.offerCurrencyId)
                .ForeignKey("dbo.ProcurementProducts", t => t.productId)
                .ForeignKey("dbo.tabVendor", t => t.VendorId)
                .ForeignKey("dbo.ModuleContracts", t => t.ModuleContract_Id)
                .Index(t => t.offerId)
                .Index(t => t.VendorId)
                .Index(t => t.incoTerm_Id)
                .Index(t => t.offerCurrencyId)
                .Index(t => t.creatorId)
                .Index(t => t.vendorIncoTermId)
                .Index(t => t.productId)
                .Index(t => t.ModuleContract_Id);
            
            CreateTable(
                "dbo.ComparativeStatementItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        costSheetFieldId = c.Int(),
                        convertedCurrencyId = c.Int(),
                        itemCurrencyId = c.Int(),
                        comparativeStatementId = c.Int(),
                        MER = c.Double(nullable: false),
                        amountOC = c.Double(nullable: false),
                        amountMER = c.Double(nullable: false),
                        ownDescription = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ComparativeStatements", t => t.comparativeStatementId)
                .ForeignKey("dbo.Currencies", t => t.convertedCurrencyId)
                .ForeignKey("dbo.CostSheetFields", t => t.costSheetFieldId)
                .ForeignKey("dbo.Currencies", t => t.itemCurrencyId)
                .Index(t => t.costSheetFieldId)
                .Index(t => t.convertedCurrencyId)
                .Index(t => t.itemCurrencyId)
                .Index(t => t.comparativeStatementId);
            
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
                "dbo.PurchaseOrders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        POReferenceNo = c.String(),
                        SOReferenceNo = c.String(),
                        SalesReferenceNo = c.String(),
                        FinanceRefrenceNo = c.String(),
                        VendorName = c.String(),
                        OfferReferenceNo = c.String(),
                        SyetmReferenceNo = c.String(),
                        saleOrderDate = c.DateTime(),
                        PurchaseOrderDate = c.DateTime(),
                        CreationDate = c.DateTime(),
                        ClosingDate = c.DateTime(),
                        LastStatusChangeDate = c.DateTime(),
                        DeliveryDate = c.DateTime(),
                        ShipmentDate = c.DateTime(),
                        OrderConfirmationDate = c.DateTime(),
                        BillOfLaddingDate = c.DateTime(),
                        lCDate = c.DateTime(),
                        MaterialReciptDate = c.DateTime(),
                        RevisedShipmentDate = c.DateTime(),
                        PaymentDueStartDate = c.DateTime(),
                        ExpectedPayment = c.DateTime(),
                        PaymentDueAgeing = c.DateTime(),
                        CreditDays = c.Int(nullable: false),
                        TargetYear = c.Int(nullable: false),
                        TargetMonth = c.Int(nullable: false),
                        LCnumber = c.String(),
                        ExchangeRate = c.Single(nullable: false),
                        NetCommision = c.Double(),
                        Commision = c.Double(),
                        commisioninBase = c.Double(),
                        SOC_ER = c.Double(nullable: false),
                        SoAmountSOC_ER = c.Double(nullable: false),
                        marginExchangeRate = c.Double(nullable: false),
                        margin = c.Double(),
                        BudgetedMargininBase = c.Double(),
                        SalesBudgetedMargin = c.Double(),
                        BudgetedMarginPercent = c.Double(),
                        RevisedMargin = c.Double(),
                        RevisedMargininBase = c.Double(),
                        SalesRevisedMargin = c.Double(),
                        RevisedMarginPercent = c.Double(),
                        ActualMarginPercent = c.Double(),
                        ActualMargin = c.Double(),
                        ActualMargininBase = c.Double(),
                        SalesActualMargin = c.Double(),
                        transshipment = c.Boolean(),
                        packing = c.String(),
                        LCShipmentDate = c.DateTime(),
                        LCExpiryDate = c.DateTime(),
                        deliveryTerm = c.String(),
                        LCAmedmentNo = c.String(),
                        LCShipmentAmendmentDate = c.DateTime(),
                        LCExpiryAmedmentDate = c.DateTime(),
                        maker = c.String(),
                        origin = c.String(),
                        OwnDescription = c.String(),
                        comments = c.String(),
                        deliveryTime = c.String(),
                        totalFOBValue = c.Double(nullable: false),
                        totalCFRValue = c.Double(nullable: false),
                        RemainingFOBValue = c.Double(nullable: false),
                        RemainingCFRValue = c.Double(nullable: false),
                        totalBaseFOBValue = c.Double(nullable: false),
                        totalBaseCFRValue = c.Double(nullable: false),
                        UnInvoicedTotalWeight = c.Decimal(precision: 18, scale: 2),
                        UnInvoicedTotalQuantity = c.Decimal(precision: 18, scale: 2),
                        ReceivedAmount = c.Double(nullable: false),
                        TotalWeight = c.Decimal(precision: 18, scale: 2),
                        TotalQuantity = c.Decimal(precision: 18, scale: 2),
                        hasTax = c.Boolean(),
                        tax_Id = c.Int(),
                        billWithTax = c.Double(),
                        hasWHT = c.Boolean(),
                        WHT_Id = c.Int(),
                        billAfterTax = c.Double(),
                        POAmountSER = c.Double(nullable: false),
                        isPercentTax = c.Boolean(nullable: false),
                        salesTax = c.Double(nullable: false),
                        stage = c.String(),
                        InvoiceStage = c.String(),
                        PurchaseOrdertype = c.Int(nullable: false),
                        isVoid = c.Boolean(nullable: false),
                        isReviewed = c.Boolean(),
                        needReview = c.Boolean(),
                        PendingForClosing = c.Boolean(),
                        PendingForReApproval = c.Boolean(),
                        isApproved = c.Boolean(),
                        ApprovedDate = c.DateTime(),
                        ReApprovalDate = c.DateTime(),
                        isReApproved = c.Boolean(),
                        saleOrder_Id = c.Int(),
                        CostSheet_Id = c.Int(),
                        CommissionSummarySheetId = c.Int(),
                        dept_Id = c.Int(nullable: false),
                        company_Id = c.Int(),
                        InterDepartment_Id = c.Int(),
                        InterCompany_Id = c.Int(),
                        isInterCompany = c.Boolean(),
                        customerCompany_Id = c.Int(nullable: false),
                        SOWarrantyId = c.Int(),
                        POWarrantyId = c.Int(),
                        user_Id = c.Int(),
                        allocation_Id = c.Int(nullable: false),
                        vendorPaymentId = c.Int(),
                        SoPaymentterm_Id = c.Int(),
                        POPaymentterm_Id = c.Int(nullable: false),
                        bid_Id = c.Int(),
                        currency_Id = c.Int(nullable: false),
                        SOCurrency_Id = c.Int(),
                        incoterm_Id = c.Int(nullable: false),
                        TitleValue1Id = c.Int(),
                        TitleValue2Id = c.Int(),
                        proforma = c.String(),
                        stlRefNo = c.String(),
                        totaltaxAmount = c.Double(nullable: false),
                        CostSheetFieldId = c.Int(nullable: false),
                        isAdjustedTax = c.Boolean(nullable: false),
                        isDiscount = c.Boolean(nullable: false),
                        Discount = c.Double(nullable: false),
                        isFreight = c.Boolean(nullable: false),
                        Freight = c.Double(nullable: false),
                        isCOO = c.Boolean(nullable: false),
                        COO = c.Double(nullable: false),
                        totalAmount = c.Double(nullable: false),
                        totalAmountGST = c.Double(nullable: false),
                        totalClaimDisount = c.Double(nullable: false),
                        totalPassOn = c.Double(nullable: false),
                        totalFocSampling = c.Double(nullable: false),
                        totalPOAdvance = c.Double(nullable: false),
                        expectedPaymentAmount = c.Double(nullable: false),
                        totalPOSattled = c.Double(nullable: false),
                        Budget_Id = c.Int(),
                        transactionHolderId = c.Int(),
                        holderChangeDate = c.DateTime(nullable: false),
                        statusClass_Id = c.Int(),
                        lotNo = c.String(),
                        lotNumberId = c.Int(),
                        PurchaseOrderStatus_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.allocation_Id, cascadeDelete: true)
                .ForeignKey("dbo.Bids", t => t.bid_Id)
                .ForeignKey("dbo.BudgetCostSheets", t => t.Budget_Id)
                .ForeignKey("dbo.CommissionSummarySheets", t => t.CommissionSummarySheetId)
                .ForeignKey("dbo.PaymentTerms", t => t.POPaymentterm_Id, cascadeDelete: true)
                .ForeignKey("dbo.PaymentTerms", t => t.SoPaymentterm_Id)
                .ForeignKey("dbo.Warranties", t => t.POWarrantyId)
                .ForeignKey("dbo.Warranties", t => t.SOWarrantyId)
                .ForeignKey("dbo.CostSheets", t => t.CostSheet_Id)
                .ForeignKey("dbo.Currencies", t => t.currency_Id, cascadeDelete: true)
                .ForeignKey("dbo.CustomerCompanies", t => t.customerCompany_Id, cascadeDelete: true)
                .ForeignKey("dbo.SaleOrders", t => t.saleOrder_Id)
                .ForeignKey("dbo.LotNumbers", t => t.lotNumberId)
                .ForeignKey("dbo.PurchaseOrderStatus", t => t.PurchaseOrderStatus_Id)
                .ForeignKey("dbo.Currencies", t => t.SOCurrency_Id)
                .ForeignKey("dbo.StatusClasses", t => t.statusClass_Id)
                .ForeignKey("dbo.TaxNames", t => t.tax_Id)
                .ForeignKey("dbo.Users", t => t.user_Id)
                .ForeignKey("dbo.VendorPaymentStatus", t => t.vendorPaymentId)
                .ForeignKey("dbo.TaxNames", t => t.WHT_Id)
                .ForeignKey("dbo.Incoterms", t => t.incoterm_Id, cascadeDelete: true)
                .ForeignKey("dbo.Incoterms", t => t.TitleValue1Id)
                .ForeignKey("dbo.Incoterms", t => t.TitleValue2Id)
                .ForeignKey("dbo.tabDepartment", t => t.dept_Id, cascadeDelete: true)
                .ForeignKey("dbo.tabDepartment", t => t.InterDepartment_Id)
                .ForeignKey("dbo.Employees", t => t.transactionHolderId)
                .ForeignKey("dbo.tabCompany", t => t.company_Id)
                .ForeignKey("dbo.tabCompany", t => t.InterCompany_Id)
                .Index(t => t.tax_Id)
                .Index(t => t.WHT_Id)
                .Index(t => t.saleOrder_Id)
                .Index(t => t.CostSheet_Id)
                .Index(t => t.CommissionSummarySheetId)
                .Index(t => t.dept_Id)
                .Index(t => t.company_Id)
                .Index(t => t.InterDepartment_Id)
                .Index(t => t.InterCompany_Id)
                .Index(t => t.customerCompany_Id)
                .Index(t => t.SOWarrantyId)
                .Index(t => t.POWarrantyId)
                .Index(t => t.user_Id)
                .Index(t => t.allocation_Id)
                .Index(t => t.vendorPaymentId)
                .Index(t => t.SoPaymentterm_Id)
                .Index(t => t.POPaymentterm_Id)
                .Index(t => t.bid_Id)
                .Index(t => t.currency_Id)
                .Index(t => t.SOCurrency_Id)
                .Index(t => t.incoterm_Id)
                .Index(t => t.TitleValue1Id)
                .Index(t => t.TitleValue2Id)
                .Index(t => t.Budget_Id)
                .Index(t => t.transactionHolderId)
                .Index(t => t.statusClass_Id)
                .Index(t => t.lotNumberId)
                .Index(t => t.PurchaseOrderStatus_Id);
            
            CreateTable(
                "dbo.BudgetCostSheets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        companyId = c.Int(),
                        deptId = c.Int(),
                        From = c.DateTime(nullable: false),
                        To = c.DateTime(nullable: false),
                        PendingForClosing = c.Boolean(),
                        PendingForReApproval = c.Boolean(),
                        isApproved = c.Boolean(),
                        ReApprovalDate = c.DateTime(),
                        isReApproved = c.Boolean(),
                        ApprovedDate = c.DateTime(),
                        isReviewed = c.Boolean(),
                        needReview = c.Boolean(),
                        stage = c.String(),
                        isVoid = c.Boolean(nullable: false),
                        empId = c.Int(),
                        LastStatusChangeDate = c.DateTime(),
                        ClosingDate = c.DateTime(),
                        TotalIncome = c.Double(nullable: false),
                        TotalCGS = c.Double(nullable: false),
                        TotalExpense = c.Double(nullable: false),
                        TotalGrossProfit = c.Double(nullable: false),
                        TotalNetProfit = c.Double(nullable: false),
                        TotalRSBCIncome = c.Double(nullable: false),
                        TotalRSBCCGS = c.Double(nullable: false),
                        TotalRSBCExpense = c.Double(nullable: false),
                        TotalRSBCGrossProfit = c.Double(nullable: false),
                        TotalRSBCNetProfit = c.Double(nullable: false),
                        CreationDate = c.DateTime(),
                        currencyId = c.Int(),
                        refNo = c.String(),
                        BudgetMonth = c.DateTime(),
                        budgetCostSheetStatus_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BudgetCostSheetStatus", t => t.budgetCostSheetStatus_Id)
                .ForeignKey("dbo.tabCompany", t => t.companyId)
                .ForeignKey("dbo.Currencies", t => t.currencyId)
                .ForeignKey("dbo.tabDepartment", t => t.deptId)
                .ForeignKey("dbo.Employees", t => t.empId)
                .Index(t => t.companyId)
                .Index(t => t.deptId)
                .Index(t => t.empId)
                .Index(t => t.currencyId)
                .Index(t => t.budgetCostSheetStatus_Id);
            
            CreateTable(
                "dbo.BudgetCostFields",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Head_Id = c.Int(),
                        BudgetedCost = c.Double(nullable: false),
                        bmPerc = c.Double(nullable: false),
                        RSBC = c.Double(nullable: false),
                        rsbcPerc = c.Double(nullable: false),
                        AdjustmentCost = c.Double(nullable: false),
                        addedRSBC = c.Double(nullable: false),
                        BudgetCostSheet_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BudgetSheetHeads", t => t.Head_Id)
                .ForeignKey("dbo.BudgetCostSheets", t => t.BudgetCostSheet_Id)
                .Index(t => t.Head_Id)
                .Index(t => t.BudgetCostSheet_Id);
            
            CreateTable(
                "dbo.BudgetSheetHeads",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        HeadName = c.String(),
                        isActive = c.Boolean(nullable: false),
                        SortId = c.Int(nullable: false),
                        creatorId = c.Int(),
                        isIncome = c.Boolean(nullable: false),
                        isCGS = c.Boolean(nullable: false),
                        isExpense = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.creatorId)
                .Index(t => t.creatorId);
            
            CreateTable(
                "dbo.BudgetCostSheetStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.String(),
                        isApproved = c.Boolean(nullable: false),
                        isActive = c.Boolean(),
                        backcolor = c.String(),
                        forecolor = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CommissionSummarySheets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Timestamp = c.DateTime(nullable: false),
                        Info = c.String(),
                        OfferCurrencyId = c.Int(),
                        totalOfferFOB = c.Double(nullable: false),
                        totalOfferCFR = c.Double(nullable: false),
                        OfferDeliveryTerm = c.String(),
                        OfferDeliveryDate = c.DateTime(),
                        OfferPacking = c.String(),
                        OfferCommission = c.Double(nullable: false),
                        SOCommission = c.Double(nullable: false),
                        TransactionType = c.Int(nullable: false),
                        TransactionId = c.Int(nullable: false),
                        Offerpaymentterm = c.String(),
                        Offertranshipment = c.Boolean(),
                        netOfferCommission = c.Double(nullable: false),
                        netSOCommission = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Currencies", t => t.OfferCurrencyId)
                .Index(t => t.OfferCurrencyId);
            
            CreateTable(
                "dbo.SummaryFieldValues",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FieldId = c.Int(nullable: false),
                        Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Type = c.Int(nullable: false),
                        CommissionSummarySheet_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SummarySheetFields", t => t.FieldId, cascadeDelete: true)
                .ForeignKey("dbo.CommissionSummarySheets", t => t.CommissionSummarySheet_Id)
                .Index(t => t.FieldId)
                .Index(t => t.CommissionSummarySheet_Id);
            
            CreateTable(
                "dbo.SummarySheetFields",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AddedbyUserId = c.Int(nullable: false),
                        Timestamp = c.DateTime(nullable: false),
                        Title = c.String(),
                        SortId = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                        isActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.AddedbyUserId, cascadeDelete: true)
                .Index(t => t.AddedbyUserId);
            
            CreateTable(
                "dbo.CostSheets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Timestamp = c.DateTime(nullable: false),
                        Info = c.String(),
                        TransactionType = c.Int(nullable: false),
                        TransactionId = c.Int(nullable: false),
                        TotalBudgetedMargin = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalActualMargin = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalRevisedMargin = c.Decimal(nullable: false, precision: 18, scale: 2),
                        paymenttermWithSupplier = c.String(),
                        PODeliveryDate = c.DateTime(),
                        maker = c.String(),
                        origin = c.String(),
                        SupplierWarrantyId = c.Int(),
                        packing = c.String(),
                        paymentterm_Id = c.Int(),
                        incoterm_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Incoterms", t => t.incoterm_Id)
                .ForeignKey("dbo.PaymentTerms", t => t.paymentterm_Id)
                .ForeignKey("dbo.Warranties", t => t.SupplierWarrantyId)
                .Index(t => t.SupplierWarrantyId)
                .Index(t => t.paymentterm_Id)
                .Index(t => t.incoterm_Id);
            
            CreateTable(
                "dbo.CostFieldHistories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FieldId = c.Int(nullable: false),
                        CostSheetId = c.Int(nullable: false),
                        Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FieldType = c.Int(nullable: false),
                        timeStamp = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CostSheets", t => t.CostSheetId, cascadeDelete: true)
                .ForeignKey("dbo.CostSheetFields", t => t.FieldId, cascadeDelete: true)
                .Index(t => t.FieldId)
                .Index(t => t.CostSheetId);
            
            CreateTable(
                "dbo.CostSheetBillFields",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FieldId = c.Int(nullable: false),
                        CostSheetId = c.Int(nullable: false),
                        Bill_Id = c.Int(nullable: false),
                        Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FieldType = c.Int(nullable: false),
                        timeStamp = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CostSheets", t => t.CostSheetId, cascadeDelete: true)
                .ForeignKey("dbo.CostSheetFields", t => t.FieldId, cascadeDelete: true)
                .Index(t => t.FieldId)
                .Index(t => t.CostSheetId);
            
            CreateTable(
                "dbo.CostSheetOfferFields",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FieldId = c.Int(nullable: false),
                        CostSheetId = c.Int(nullable: false),
                        Offer_Id = c.Int(nullable: false),
                        Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FieldType = c.Int(nullable: false),
                        timeStamp = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CostSheets", t => t.CostSheetId, cascadeDelete: true)
                .ForeignKey("dbo.CostSheetFields", t => t.FieldId, cascadeDelete: true)
                .Index(t => t.FieldId)
                .Index(t => t.CostSheetId);
            
            CreateTable(
                "dbo.CostSheetPaymentFields",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FieldId = c.Int(nullable: false),
                        CostSheetId = c.Int(nullable: false),
                        Payment_Id = c.Int(nullable: false),
                        Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FieldType = c.Int(nullable: false),
                        timeStamp = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CostSheets", t => t.CostSheetId, cascadeDelete: true)
                .ForeignKey("dbo.CostSheetFields", t => t.FieldId, cascadeDelete: true)
                .Index(t => t.FieldId)
                .Index(t => t.CostSheetId);
            
            CreateTable(
                "dbo.CostSheetPOFields",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FieldId = c.Int(nullable: false),
                        CostSheetId = c.Int(nullable: false),
                        PO_Id = c.Int(nullable: false),
                        Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FieldType = c.Int(nullable: false),
                        timeStamp = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CostSheets", t => t.CostSheetId, cascadeDelete: true)
                .ForeignKey("dbo.CostSheetFields", t => t.FieldId, cascadeDelete: true)
                .Index(t => t.FieldId)
                .Index(t => t.CostSheetId);
            
            CreateTable(
                "dbo.CostSheetSaleReceiptFields",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FieldId = c.Int(nullable: false),
                        CostSheetId = c.Int(nullable: false),
                        Receipt_Id = c.Int(nullable: false),
                        Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FieldType = c.Int(nullable: false),
                        timeStamp = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CostSheets", t => t.CostSheetId, cascadeDelete: true)
                .ForeignKey("dbo.CostSheetFields", t => t.FieldId, cascadeDelete: true)
                .Index(t => t.FieldId)
                .Index(t => t.CostSheetId);
            
            CreateTable(
                "dbo.CostSheetSIFields",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        FieldId = c.Int(nullable: false),
                        CostSheetId = c.Int(nullable: false),
                        SI_Id = c.Int(nullable: false),
                        Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FieldType = c.Int(nullable: false),
                        timeStamp = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.CostSheets", t => t.CostSheetId, cascadeDelete: true)
                .ForeignKey("dbo.CostSheetFields", t => t.FieldId, cascadeDelete: true)
                .Index(t => t.FieldId)
                .Index(t => t.CostSheetId);
            
            CreateTable(
                "dbo.CostSheetSOFields",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FieldId = c.Int(nullable: false),
                        CostSheetId = c.Int(nullable: false),
                        SO_Id = c.Int(nullable: false),
                        Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FieldType = c.Int(nullable: false),
                        timeStamp = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CostSheets", t => t.CostSheetId, cascadeDelete: true)
                .ForeignKey("dbo.CostSheetFields", t => t.FieldId, cascadeDelete: true)
                .Index(t => t.FieldId)
                .Index(t => t.CostSheetId);
            
            CreateTable(
                "dbo.FieldValues",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FieldId = c.Int(nullable: false),
                        Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Type = c.Int(nullable: false),
                        stringValue = c.String(),
                        dateValue = c.DateTime(),
                        isdgGood = c.Boolean(nullable: false),
                        isPacking = c.Boolean(nullable: false),
                        drawingRequired = c.Boolean(nullable: false),
                        isExportLicense = c.Boolean(nullable: false),
                        adjSCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CostSheet_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CostSheetFields", t => t.FieldId, cascadeDelete: true)
                .ForeignKey("dbo.CostSheets", t => t.CostSheet_Id)
                .Index(t => t.FieldId)
                .Index(t => t.CostSheet_Id);
            
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
                        user_Id = c.Int(),
                        addedDate = c.DateTime(nullable: false),
                        isSaleOrderType = c.Boolean(nullable: false),
                        isPurchaseOrderType = c.Boolean(nullable: false),
                        isSaleInvoiceType = c.Boolean(nullable: false),
                        isPurchaseInvoiceType = c.Boolean(nullable: false),
                        isPaymentType = c.Boolean(nullable: false),
                        isVnedorBillType = c.Boolean(nullable: false),
                        isOfferType = c.Boolean(nullable: false),
                        isCostSheetType = c.Boolean(nullable: false),
                        ParentId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PaymentTerms", t => t.ParentId)
                .ForeignKey("dbo.Users", t => t.user_Id)
                .Index(t => t.user_Id)
                .Index(t => t.ParentId);
            
            CreateTable(
                "dbo.Warranties",
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
                "dbo.JournalVouchers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        entryNo = c.String(),
                        voucherRefno = c.String(),
                        postingDate = c.DateTime(nullable: false),
                        userId = c.Int(),
                        isVoid = c.Boolean(nullable: false),
                        coaTransactionsType = c.Int(nullable: false),
                        statusId = c.Int(),
                        LastStatusChangeDate = c.DateTime(),
                        isReviewed = c.Boolean(),
                        needReview = c.Boolean(),
                        PendingForClosing = c.Boolean(),
                        PendingForReApproval = c.Boolean(),
                        isApproved = c.Boolean(),
                        ApprovedDate = c.DateTime(),
                        ReApprovalDate = c.DateTime(),
                        isReApproved = c.Boolean(),
                        stage = c.String(),
                        ClosingDate = c.DateTime(),
                        currencyId = c.Int(),
                        company_Id = c.Int(),
                        dept_Id = c.Int(),
                        emp_Id = c.Int(),
                        MER = c.Double(nullable: false),
                        bill_Id = c.Int(),
                        purchaseOrder_Id = c.Int(),
                        paymentGroupId = c.Int(nullable: false),
                        receiptGroupId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tabCompany", t => t.company_Id)
                .ForeignKey("dbo.Currencies", t => t.currencyId)
                .ForeignKey("dbo.tabDepartment", t => t.dept_Id)
                .ForeignKey("dbo.Employees", t => t.emp_Id)
                .ForeignKey("dbo.JournalVoucherStatus", t => t.statusId)
                .ForeignKey("dbo.Users", t => t.userId)
                .ForeignKey("dbo.PurchaseOrders", t => t.purchaseOrder_Id)
                .ForeignKey("dbo.Bills", t => t.bill_Id)
                .Index(t => t.userId)
                .Index(t => t.statusId)
                .Index(t => t.currencyId)
                .Index(t => t.company_Id)
                .Index(t => t.dept_Id)
                .Index(t => t.emp_Id)
                .Index(t => t.bill_Id)
                .Index(t => t.purchaseOrder_Id);
            
            CreateTable(
                "dbo.JournalVoucherStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.String(),
                        isApproved = c.Boolean(nullable: false),
                        isActive = c.Boolean(),
                        backcolor = c.String(),
                        forecolor = c.String(),
                        HierarchicalIndex = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.LoansAdvances",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreationDate = c.DateTime(),
                        advanceTemplate = c.Int(nullable: false),
                        loansAdvanceType = c.Int(nullable: false),
                        transactionGroupId = c.Int(nullable: false),
                        companyId = c.Int(),
                        deptId = c.Int(),
                        creatorId = c.Int(),
                        SystemRef = c.String(),
                        isEmployee = c.Boolean(nullable: false),
                        applicantTypeId = c.Int(),
                        applicantId = c.Int(),
                        employeeId = c.Int(),
                        vendorId = c.Int(),
                        statusId = c.Int(),
                        AppliedAmountOC = c.Double(nullable: false),
                        LoanAmountOC = c.Double(nullable: false),
                        MER = c.Double(nullable: false),
                        LoanAmountMER = c.Double(nullable: false),
                        Purpose = c.String(),
                        LoanTenureDays = c.Int(nullable: false),
                        LoanReturnDate = c.DateTime(),
                        SaleInvoiceId = c.Int(),
                        saleOrderId = c.Int(),
                        purchaseOrderId = c.Int(),
                        currencyId = c.Int(),
                        stage = c.String(),
                        isVoid = c.Boolean(nullable: false),
                        isReviewed = c.Boolean(),
                        needReview = c.Boolean(),
                        PendingForClosing = c.Boolean(),
                        PendingForReApproval = c.Boolean(),
                        isApproved = c.Boolean(),
                        ApprovedDate = c.DateTime(),
                        isReApproved = c.Boolean(),
                        ReApprovalDate = c.DateTime(),
                        ClosingDate = c.DateTime(),
                        LastStatusChangeDate = c.DateTime(),
                        PettyCashRefId = c.Int(),
                        isDeposit = c.Boolean(),
                        statusClass_Id = c.Int(),
                        VATBookRefId = c.Int(),
                        isLinkable = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.LoanApplicants", t => t.applicantId)
                .ForeignKey("dbo.Employees", t => t.employeeId)
                .ForeignKey("dbo.LoanApplicantTypes", t => t.applicantTypeId)
                .ForeignKey("dbo.tabCompany", t => t.companyId)
                .ForeignKey("dbo.Users", t => t.creatorId)
                .ForeignKey("dbo.Currencies", t => t.currencyId)
                .ForeignKey("dbo.tabDepartment", t => t.deptId)
                .ForeignKey("dbo.BillRefNumbers", t => t.PettyCashRefId)
                .ForeignKey("dbo.PurchaseOrders", t => t.purchaseOrderId)
                .ForeignKey("dbo.SaleInvoices", t => t.SaleInvoiceId)
                .ForeignKey("dbo.SaleOrders", t => t.saleOrderId)
                .ForeignKey("dbo.LoansAdvanceStatus", t => t.statusId)
                .ForeignKey("dbo.StatusClasses", t => t.statusClass_Id)
                .ForeignKey("dbo.VATBookRefNumbers", t => t.VATBookRefId)
                .ForeignKey("dbo.tabVendor", t => t.vendorId)
                .Index(t => t.companyId)
                .Index(t => t.deptId)
                .Index(t => t.creatorId)
                .Index(t => t.applicantTypeId)
                .Index(t => t.applicantId)
                .Index(t => t.employeeId)
                .Index(t => t.vendorId)
                .Index(t => t.statusId)
                .Index(t => t.SaleInvoiceId)
                .Index(t => t.saleOrderId)
                .Index(t => t.purchaseOrderId)
                .Index(t => t.currencyId)
                .Index(t => t.PettyCashRefId)
                .Index(t => t.statusClass_Id)
                .Index(t => t.VATBookRefId);
            
            CreateTable(
                "dbo.LoanApplicants",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        applicantTypeId = c.Int(),
                        LenderType = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.LoanApplicantTypes", t => t.applicantTypeId)
                .Index(t => t.applicantTypeId);
            
            CreateTable(
                "dbo.LoanApplicantTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TypeName = c.String(),
                        accountId = c.Int(),
                        LenderType = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ChartofAccounts", t => t.accountId)
                .Index(t => t.accountId);
            
            CreateTable(
                "dbo.Payments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        transactionType = c.Int(nullable: false),
                        paymentAdminBillTemplate = c.Int(),
                        paymentVendorBillTemplate = c.Int(),
                        paymentPurchaseInvoiceTemplate = c.Int(),
                        paymentLoansAdvancesTemplate = c.Int(),
                        paymentTargetRewardsTemplate = c.Int(),
                        CreationDate = c.DateTime(),
                        transactionGroupId = c.Int(nullable: false),
                        AdminBill_Id = c.Int(),
                        Bill_Id = c.Int(),
                        PInvoice_Id = c.Int(),
                        TargetReward_Id = c.Int(),
                        taskGroups_Id = c.Int(),
                        company_Id = c.Int(),
                        creditCardBankId = c.Int(),
                        PrimaryCreditCardNoId = c.Int(),
                        vendor_Id = c.Int(),
                        DebitedDate = c.DateTime(),
                        PaymentDate = c.DateTime(),
                        SystemRefNo = c.String(),
                        PaymentRefNo = c.String(),
                        currency_Id = c.Int(),
                        PaymentAmount = c.Double(nullable: false),
                        statusId = c.Int(),
                        paymentMethodId = c.Int(),
                        bankId = c.Int(),
                        accountId = c.Int(),
                        PaymentRefNoId = c.Int(),
                        InstrumentNo = c.String(),
                        InstrumentDate = c.DateTime(),
                        BillCreationDate = c.DateTime(),
                        BillFinanceRefNo = c.String(),
                        BillNumber = c.String(),
                        BillingMonth = c.DateTime(),
                        BillDueDate = c.DateTime(),
                        BillAmount = c.Double(nullable: false),
                        DebitedAmount = c.Double(nullable: false),
                        Deductions = c.Double(nullable: false),
                        DeductionExchangeRate = c.Double(nullable: false),
                        DeductionSOC = c.Double(nullable: false),
                        stage = c.String(),
                        isVoid = c.Boolean(nullable: false),
                        isReviewed = c.Boolean(),
                        needReview = c.Boolean(),
                        PendingForClosing = c.Boolean(),
                        PendingForReApproval = c.Boolean(),
                        isApproved = c.Boolean(),
                        ApprovedDate = c.DateTime(),
                        isReApproved = c.Boolean(),
                        ReApprovalDate = c.DateTime(),
                        user_Id = c.Int(),
                        ClosingDate = c.DateTime(),
                        LastStatusChangeDate = c.DateTime(),
                        createdFromBill = c.Boolean(nullable: false),
                        InterCompany_Id = c.Int(),
                        isInterCompany = c.Boolean(),
                        InterDepartment_Id = c.Int(),
                        CostSheetId = c.Int(),
                        isPostToGL = c.Boolean(nullable: false),
                        GLPostingDate = c.DateTime(),
                        isDeposit = c.Boolean(),
                        isBypassBank = c.Boolean(nullable: false),
                        coaAccountId = c.Int(),
                        LoansAdvanceId = c.Int(),
                        IsAdjusted = c.Boolean(),
                        totalVATamount = c.Double(nullable: false),
                        paymentterm_Id = c.Int(),
                        transactionHolderId = c.Int(),
                        holderChangeDate = c.DateTime(nullable: false),
                        statusClass_Id = c.Int(),
                        VATBookRefId = c.Int(),
                        isVATBookPosted = c.Boolean(nullable: false),
                        insuranceRequired = c.Boolean(nullable: false),
                        insuranceApplied = c.Boolean(nullable: false),
                        insuranceNotApplicable = c.Boolean(nullable: false),
                        insuranceAppliedBy_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Accounts", t => t.accountId)
                .ForeignKey("dbo.Banks", t => t.bankId)
                .ForeignKey("dbo.ChartofAccounts", t => t.coaAccountId)
                .ForeignKey("dbo.tabCompany", t => t.company_Id)
                .ForeignKey("dbo.CostSheets", t => t.CostSheetId)
                .ForeignKey("dbo.Banks", t => t.creditCardBankId)
                .ForeignKey("dbo.Currencies", t => t.currency_Id)
                .ForeignKey("dbo.Employees", t => t.insuranceAppliedBy_Id)
                .ForeignKey("dbo.PaymentMethods", t => t.paymentMethodId)
                .ForeignKey("dbo.BillRefNumbers", t => t.PaymentRefNoId)
                .ForeignKey("dbo.PaymentTerms", t => t.paymentterm_Id)
                .ForeignKey("dbo.CreditCards", t => t.PrimaryCreditCardNoId)
                .ForeignKey("dbo.PurchaseInvoices", t => t.PInvoice_Id)
                .ForeignKey("dbo.PaymentStatus", t => t.statusId)
                .ForeignKey("dbo.StatusClasses", t => t.statusClass_Id)
                .ForeignKey("dbo.TargetRewards", t => t.TargetReward_Id)
                .ForeignKey("dbo.TaskGroups", t => t.taskGroups_Id)
                .ForeignKey("dbo.Users", t => t.user_Id)
                .ForeignKey("dbo.VATBookRefNumbers", t => t.VATBookRefId)
                .ForeignKey("dbo.tabVendor", t => t.vendor_Id)
                .ForeignKey("dbo.LoansAdvances", t => t.LoansAdvanceId)
                .ForeignKey("dbo.tabAdminBill", t => t.AdminBill_Id)
                .ForeignKey("dbo.Bills", t => t.Bill_Id)
                .ForeignKey("dbo.tabDepartment", t => t.InterDepartment_Id)
                .ForeignKey("dbo.Employees", t => t.transactionHolderId)
                .ForeignKey("dbo.tabCompany", t => t.InterCompany_Id)
                .Index(t => t.AdminBill_Id)
                .Index(t => t.Bill_Id)
                .Index(t => t.PInvoice_Id)
                .Index(t => t.TargetReward_Id)
                .Index(t => t.taskGroups_Id)
                .Index(t => t.company_Id)
                .Index(t => t.creditCardBankId)
                .Index(t => t.PrimaryCreditCardNoId)
                .Index(t => t.vendor_Id)
                .Index(t => t.currency_Id)
                .Index(t => t.statusId)
                .Index(t => t.paymentMethodId)
                .Index(t => t.bankId)
                .Index(t => t.accountId)
                .Index(t => t.PaymentRefNoId)
                .Index(t => t.user_Id)
                .Index(t => t.InterCompany_Id)
                .Index(t => t.InterDepartment_Id)
                .Index(t => t.CostSheetId)
                .Index(t => t.coaAccountId)
                .Index(t => t.LoansAdvanceId)
                .Index(t => t.paymentterm_Id)
                .Index(t => t.transactionHolderId)
                .Index(t => t.statusClass_Id)
                .Index(t => t.VATBookRefId)
                .Index(t => t.insuranceAppliedBy_Id);
            
            CreateTable(
                "dbo.BudgetSystemCostFields",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Head_Id = c.Int(),
                        BudgetedCost = c.Double(nullable: false),
                        RSBC = c.Double(nullable: false),
                        addedSystemCost = c.Double(nullable: false),
                        Payment_Id = c.Int(),
                        PurchaseInvoice_Id = c.Int(),
                        SaleInvoice_Id = c.Int(),
                        SalesReceipt_Id = c.Int(),
                        Bill_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BudgetSheetHeads", t => t.Head_Id)
                .ForeignKey("dbo.Payments", t => t.Payment_Id)
                .ForeignKey("dbo.PurchaseInvoices", t => t.PurchaseInvoice_Id)
                .ForeignKey("dbo.SaleInvoices", t => t.SaleInvoice_Id)
                .ForeignKey("dbo.SalesReceipts", t => t.SalesReceipt_Id)
                .ForeignKey("dbo.Bills", t => t.Bill_Id)
                .Index(t => t.Head_Id)
                .Index(t => t.Payment_Id)
                .Index(t => t.PurchaseInvoice_Id)
                .Index(t => t.SaleInvoice_Id)
                .Index(t => t.SalesReceipt_Id)
                .Index(t => t.Bill_Id);
            
            CreateTable(
                "dbo.PaymentDeductions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        deduction_id = c.Int(),
                        Amount = c.Double(nullable: false),
                        Payment_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Deductions", t => t.deduction_id)
                .ForeignKey("dbo.Payments", t => t.Payment_Id)
                .Index(t => t.deduction_id)
                .Index(t => t.Payment_Id);
            
            CreateTable(
                "dbo.PaymentMethods",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MethodName = c.String(),
                        isActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BillRefNumbers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        companyId = c.Int(),
                        BillReferenceNo = c.String(),
                        isActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tabCompany", t => t.companyId)
                .Index(t => t.companyId);
            
            CreateTable(
                "dbo.PaymentTaxes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        taxNameId = c.Int(),
                        Amount = c.Double(nullable: false),
                        Payment_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TaxNames", t => t.taxNameId)
                .ForeignKey("dbo.Payments", t => t.Payment_Id)
                .Index(t => t.taxNameId)
                .Index(t => t.Payment_Id);
            
            CreateTable(
                "dbo.PettyCashes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreationDate = c.DateTime(),
                        TransactionType = c.Int(nullable: false),
                        companyId = c.Int(),
                        deptId = c.Int(),
                        Description = c.String(),
                        currencyId = c.Int(),
                        FinanceRefNo = c.String(),
                        SystemRefNo = c.String(),
                        debit = c.Double(nullable: false),
                        credit = c.Double(nullable: false),
                        SaleReceiptId = c.Int(),
                        interBankTransferId = c.Int(),
                        PaymentId = c.Int(),
                        AdminBillId = c.Int(),
                        MER = c.Double(nullable: false),
                        total = c.Double(nullable: false),
                        InterCompanyId = c.Int(),
                        LoansAdvanceId = c.Int(),
                        billId = c.Int(),
                        PettyCashRefId = c.Int(),
                        InterCompanyBankTransfer_Id = c.Int(),
                        InterCompanyBankTransfer_Id1 = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tabAdminBill", t => t.AdminBillId)
                .ForeignKey("dbo.Bills", t => t.billId)
                .ForeignKey("dbo.tabCompany", t => t.companyId)
                .ForeignKey("dbo.tabDepartment", t => t.deptId)
                .ForeignKey("dbo.InterBankTransfers", t => t.interBankTransferId)
                .ForeignKey("dbo.InterCompanyBankTransfers", t => t.InterCompanyBankTransfer_Id)
                .ForeignKey("dbo.InterCompanyBankTransfers", t => t.InterCompanyBankTransfer_Id1)
                .ForeignKey("dbo.SalesReceipts", t => t.SaleReceiptId)
                .ForeignKey("dbo.InterCompanyBankTransfers", t => t.InterCompanyId)
                .ForeignKey("dbo.LoansAdvances", t => t.LoansAdvanceId)
                .ForeignKey("dbo.Currencies", t => t.currencyId)
                .ForeignKey("dbo.Payments", t => t.PaymentId)
                .ForeignKey("dbo.BillRefNumbers", t => t.PettyCashRefId)
                .Index(t => t.companyId)
                .Index(t => t.deptId)
                .Index(t => t.currencyId)
                .Index(t => t.SaleReceiptId)
                .Index(t => t.interBankTransferId)
                .Index(t => t.PaymentId)
                .Index(t => t.AdminBillId)
                .Index(t => t.InterCompanyId)
                .Index(t => t.LoansAdvanceId)
                .Index(t => t.billId)
                .Index(t => t.PettyCashRefId)
                .Index(t => t.InterCompanyBankTransfer_Id)
                .Index(t => t.InterCompanyBankTransfer_Id1);
            
            CreateTable(
                "dbo.InterCompanyBankTransfers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreationDate = c.DateTime(),
                        SystemRefNo = c.String(),
                        FinanceRefNo = c.String(),
                        TransactionDate = c.DateTime(),
                        InstrumentNo = c.String(),
                        InstrumentDate = c.DateTime(),
                        StatusId = c.Int(),
                        transferMethod_Id = c.Int(),
                        AmountOC = c.Double(nullable: false),
                        currency_Id = c.Int(),
                        MER = c.Double(nullable: false),
                        AmountMER = c.Double(nullable: false),
                        companyFrom_Id = c.Int(),
                        deptFrom_Id = c.Int(),
                        bankFrom_Id = c.Int(),
                        accountFrom_Id = c.Int(),
                        companyTo_Id = c.Int(),
                        deptTo_Id = c.Int(),
                        bankTo_Id = c.Int(),
                        accountTo_Id = c.Int(),
                        emp_Id = c.Int(),
                        transferType = c.Int(nullable: false),
                        stage = c.String(),
                        isVoid = c.Boolean(nullable: false),
                        isReviewed = c.Boolean(),
                        needReview = c.Boolean(),
                        PendingForClosing = c.Boolean(),
                        PendingForReApproval = c.Boolean(),
                        isApproved = c.Boolean(),
                        ApprovedDate = c.DateTime(),
                        isReApproved = c.Boolean(),
                        ReApprovalDate = c.DateTime(),
                        user_Id = c.Int(),
                        ClosingDate = c.DateTime(),
                        LastStatusChangeDate = c.DateTime(),
                        COAdebit_Id = c.Int(),
                        COAcredit_Id = c.Int(),
                        currencyFromId = c.Int(),
                        AmountFrom = c.Double(nullable: false),
                        MERfrom = c.Double(nullable: false),
                        AmountMERfrom = c.Double(nullable: false),
                        currencyToId = c.Int(),
                        AmountTo = c.Double(nullable: false),
                        MERto = c.Double(nullable: false),
                        AmountMERto = c.Double(nullable: false),
                        AmountER = c.Double(nullable: false),
                        Description = c.String(),
                        GLPostingDate = c.DateTime(),
                        isDepositFrom = c.Boolean(),
                        isDepositTo = c.Boolean(),
                        isAmountOCfrom = c.Boolean(),
                        isAmountOCto = c.Boolean(),
                        PettyCashRefFromId = c.Int(),
                        PettyCashRefToId = c.Int(),
                        paymentGroupId = c.Int(nullable: false),
                        receiptGroupId = c.Int(nullable: false),
                        vendorBillId = c.Int(),
                        adminBillGroupId = c.Int(nullable: false),
                        adminBillId = c.Int(),
                        IsAdjustedVATfrom = c.Boolean(),
                        IsAdjustedVATto = c.Boolean(),
                        stlId = c.Int(),
                        statusClass_Id = c.Int(),
                        VATBookRefId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Accounts", t => t.accountFrom_Id)
                .ForeignKey("dbo.Accounts", t => t.accountTo_Id)
                .ForeignKey("dbo.tabAdminBill", t => t.adminBillId)
                .ForeignKey("dbo.Banks", t => t.bankFrom_Id)
                .ForeignKey("dbo.Banks", t => t.bankTo_Id)
                .ForeignKey("dbo.Bills", t => t.vendorBillId)
                .ForeignKey("dbo.ChartofAccounts", t => t.COAcredit_Id)
                .ForeignKey("dbo.ChartofAccounts", t => t.COAdebit_Id)
                .ForeignKey("dbo.tabCompany", t => t.companyFrom_Id)
                .ForeignKey("dbo.tabCompany", t => t.companyTo_Id)
                .ForeignKey("dbo.Currencies", t => t.currency_Id)
                .ForeignKey("dbo.Currencies", t => t.currencyFromId)
                .ForeignKey("dbo.Currencies", t => t.currencyToId)
                .ForeignKey("dbo.tabDepartment", t => t.deptFrom_Id)
                .ForeignKey("dbo.tabDepartment", t => t.deptTo_Id)
                .ForeignKey("dbo.Employees", t => t.emp_Id)
                .ForeignKey("dbo.InterBankTransferStatus", t => t.StatusId)
                .ForeignKey("dbo.BillRefNumbers", t => t.PettyCashRefFromId)
                .ForeignKey("dbo.BillRefNumbers", t => t.PettyCashRefToId)
                .ForeignKey("dbo.StatusClasses", t => t.statusClass_Id)
                .ForeignKey("dbo.STLs", t => t.stlId)
                .ForeignKey("dbo.TranferMethods", t => t.transferMethod_Id)
                .ForeignKey("dbo.Users", t => t.user_Id)
                .ForeignKey("dbo.VATBookRefNumbers", t => t.VATBookRefId)
                .Index(t => t.StatusId)
                .Index(t => t.transferMethod_Id)
                .Index(t => t.currency_Id)
                .Index(t => t.companyFrom_Id)
                .Index(t => t.deptFrom_Id)
                .Index(t => t.bankFrom_Id)
                .Index(t => t.accountFrom_Id)
                .Index(t => t.companyTo_Id)
                .Index(t => t.deptTo_Id)
                .Index(t => t.bankTo_Id)
                .Index(t => t.accountTo_Id)
                .Index(t => t.emp_Id)
                .Index(t => t.user_Id)
                .Index(t => t.COAdebit_Id)
                .Index(t => t.COAcredit_Id)
                .Index(t => t.currencyFromId)
                .Index(t => t.currencyToId)
                .Index(t => t.PettyCashRefFromId)
                .Index(t => t.PettyCashRefToId)
                .Index(t => t.vendorBillId)
                .Index(t => t.adminBillId)
                .Index(t => t.stlId)
                .Index(t => t.statusClass_Id)
                .Index(t => t.VATBookRefId);
            
            CreateTable(
                "dbo.InterCompBankTransferCharges",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        deduction_Id = c.Int(),
                        InterCompanyBankTransferFromId = c.Int(),
                        InterCompanyBankTransferToId = c.Int(),
                        Amount = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Deductions", t => t.deduction_Id)
                .ForeignKey("dbo.InterCompanyBankTransfers", t => t.InterCompanyBankTransferFromId)
                .ForeignKey("dbo.InterCompanyBankTransfers", t => t.InterCompanyBankTransferToId)
                .Index(t => t.deduction_Id)
                .Index(t => t.InterCompanyBankTransferFromId)
                .Index(t => t.InterCompanyBankTransferToId);
            
            CreateTable(
                "dbo.InterCompBankTransferVATs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        taxNameId = c.Int(),
                        InterCompanyBankTransferFromId = c.Int(),
                        InterCompanyBankTransferToId = c.Int(),
                        Amount = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.InterCompanyBankTransfers", t => t.InterCompanyBankTransferFromId)
                .ForeignKey("dbo.InterCompanyBankTransfers", t => t.InterCompanyBankTransferToId)
                .ForeignKey("dbo.TaxNames", t => t.taxNameId)
                .Index(t => t.taxNameId)
                .Index(t => t.InterCompanyBankTransferFromId)
                .Index(t => t.InterCompanyBankTransferToId);
            
            CreateTable(
                "dbo.STLs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        paymentGroupId = c.Double(nullable: false),
                        CreationDate = c.DateTime(),
                        company_Id = c.Int(),
                        dept_Id = c.Int(),
                        user_Id = c.Int(),
                        statusId = c.Int(),
                        LastStatusChangeDate = c.DateTime(),
                        stlPaymentDate = c.DateTime(),
                        creditTenure = c.DateTime(),
                        extendedCreditTenure = c.DateTime(),
                        creditTenureNo = c.Double(nullable: false),
                        extendedCreditTenureNo = c.Double(nullable: false),
                        paymentDueDays = c.Double(nullable: false),
                        stlUtilizedDays = c.Double(nullable: false),
                        paymentAmountOC = c.Double(nullable: false),
                        interestPercent = c.Double(nullable: false),
                        interestAmountCurrency_Id = c.Int(),
                        stage = c.String(),
                        salesReferenceNo = c.String(),
                        isVoid = c.Boolean(nullable: false),
                        isReviewed = c.Boolean(),
                        needReview = c.Boolean(),
                        PendingForClosing = c.Boolean(),
                        PendingForReApproval = c.Boolean(),
                        isApproved = c.Boolean(),
                        ApprovedDate = c.DateTime(),
                        isReApproved = c.Boolean(),
                        ReApprovalDate = c.DateTime(),
                        ClosingDate = c.DateTime(),
                        paymentBank_Id = c.Int(),
                        paymentCurrency_Id = c.Int(),
                        paymentAccount_Id = c.Int(),
                        settlmentAmount = c.Double(nullable: false),
                        settlmentBalance = c.Double(nullable: false),
                        marginReversal = c.Double(nullable: false),
                        stlBank_Id = c.Int(),
                        stlCurrency_Id = c.Int(),
                        stlPaymentAmountOC = c.Double(nullable: false),
                        stlAccount_Id = c.Int(),
                        cashMarginBank_Id = c.Int(),
                        cashMarginCurrency_Id = c.Int(),
                        paymentAmountSTL = c.Double(nullable: false),
                        paymentSTLER = c.Double(nullable: false),
                        paymentAmountSTLMER = c.Double(nullable: false),
                        cashMarginDrAccount_Id = c.Int(),
                        cashMarginCrAccount_Id = c.Int(),
                        cashMarginPercent = c.Double(nullable: false),
                        cashMarginAmount = c.Double(nullable: false),
                        stlRef = c.String(),
                        paymentMaturityDate = c.DateTime(),
                        GLPostingDate = c.DateTime(),
                        InterestAmount = c.Double(nullable: false),
                        InterestAmountCD = c.Double(nullable: false),
                        paymentDate = c.DateTime(),
                        interest_Id = c.Int(),
                        cashMarginPerc_Id = c.Int(),
                        SER = c.Double(nullable: false),
                        MER = c.Double(nullable: false),
                        STLAmountSER = c.Double(nullable: false),
                        STLAmountMER = c.Double(nullable: false),
                        customer_Id = c.Int(),
                        vendor_Id = c.Int(),
                        stlRemarks = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Banks", t => t.cashMarginBank_Id)
                .ForeignKey("dbo.Accounts", t => t.cashMarginDrAccount_Id)
                .ForeignKey("dbo.Currencies", t => t.cashMarginCurrency_Id)
                .ForeignKey("dbo.MarginPercentages", t => t.cashMarginPerc_Id)
                .ForeignKey("dbo.tabCompany", t => t.company_Id)
                .ForeignKey("dbo.CustomerCompanies", t => t.customer_Id)
                .ForeignKey("dbo.tabDepartment", t => t.dept_Id)
                .ForeignKey("dbo.STLInterests", t => t.interest_Id)
                .ForeignKey("dbo.Currencies", t => t.interestAmountCurrency_Id)
                .ForeignKey("dbo.Accounts", t => t.paymentAccount_Id)
                .ForeignKey("dbo.Banks", t => t.paymentBank_Id)
                .ForeignKey("dbo.Currencies", t => t.paymentCurrency_Id)
                .ForeignKey("dbo.Accounts", t => t.stlAccount_Id)
                .ForeignKey("dbo.Banks", t => t.stlBank_Id)
                .ForeignKey("dbo.Currencies", t => t.stlCurrency_Id)
                .ForeignKey("dbo.STLStatus", t => t.statusId)
                .ForeignKey("dbo.Users", t => t.user_Id)
                .ForeignKey("dbo.tabVendor", t => t.vendor_Id)
                .Index(t => t.company_Id)
                .Index(t => t.dept_Id)
                .Index(t => t.user_Id)
                .Index(t => t.statusId)
                .Index(t => t.interestAmountCurrency_Id)
                .Index(t => t.paymentBank_Id)
                .Index(t => t.paymentCurrency_Id)
                .Index(t => t.paymentAccount_Id)
                .Index(t => t.stlBank_Id)
                .Index(t => t.stlCurrency_Id)
                .Index(t => t.stlAccount_Id)
                .Index(t => t.cashMarginBank_Id)
                .Index(t => t.cashMarginCurrency_Id)
                .Index(t => t.cashMarginDrAccount_Id)
                .Index(t => t.interest_Id)
                .Index(t => t.cashMarginPerc_Id)
                .Index(t => t.customer_Id)
                .Index(t => t.vendor_Id);
            
            CreateTable(
                "dbo.MarginPercentages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        marginpercentageTypeId = c.Int(),
                        percentage = c.Double(nullable: false),
                        COA_Id = c.Int(),
                        isAdjusted = c.Boolean(nullable: false),
                        isManual = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ChartofAccounts", t => t.COA_Id)
                .ForeignKey("dbo.MarginPercentageTypes", t => t.marginpercentageTypeId)
                .Index(t => t.marginpercentageTypeId)
                .Index(t => t.COA_Id);
            
            CreateTable(
                "dbo.MarginPercentageTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TypeName = c.String(),
                        isActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.STLInterests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        interestTypeId = c.Int(),
                        percentage = c.Double(nullable: false),
                        COA_Id = c.Int(),
                        isAdjusted = c.Boolean(nullable: false),
                        isManual = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ChartofAccounts", t => t.COA_Id)
                .ForeignKey("dbo.STLInterestTypes", t => t.interestTypeId)
                .Index(t => t.interestTypeId)
                .Index(t => t.COA_Id);
            
            CreateTable(
                "dbo.STLInterestTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TypeName = c.String(),
                        isActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ReversalSettlements",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        refNO = c.String(),
                        reversalSettlementAmount = c.Double(nullable: false),
                        stl_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.STLs", t => t.stl_Id)
                .Index(t => t.stl_Id);
            
            CreateTable(
                "dbo.STLSettlements",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        refNO = c.String(),
                        settlementAmount = c.Double(nullable: false),
                        stl_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.STLs", t => t.stl_Id)
                .Index(t => t.stl_Id);
            
            CreateTable(
                "dbo.STLStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.String(),
                        isApproved = c.Boolean(nullable: false),
                        isActive = c.Boolean(),
                        backcolor = c.String(),
                        forecolor = c.String(),
                        HierarchicalIndex = c.Int(nullable: false),
                        isDisable = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TranferMethods",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MethodName = c.String(),
                        isActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VATBookRefNumbers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        companyId = c.Int(),
                        VATBookReferenceNo = c.String(),
                        isActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tabCompany", t => t.companyId)
                .Index(t => t.companyId);
            
            CreateTable(
                "dbo.VATBooks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FinanceRefNo = c.String(),
                        SystemRefNo = c.String(),
                        VATBookRef = c.String(),
                        debit = c.Double(nullable: false),
                        credit = c.Double(nullable: false),
                        MER = c.Double(nullable: false),
                        total = c.Double(nullable: false),
                        CreationDate = c.DateTime(),
                        GLPostingDate = c.DateTime(),
                        TransactionType = c.Int(nullable: false),
                        companyId = c.Int(),
                        customerId = c.Int(),
                        vendorId = c.Int(),
                        deptId = c.Int(),
                        Description = c.String(),
                        currencyId = c.Int(),
                        saleInvoiceId = c.Int(),
                        purchaseInvoiceId = c.Int(),
                        vendorBillId = c.Int(),
                        saleReceiptId = c.Int(),
                        interBankTransferId = c.Int(),
                        paymentId = c.Int(),
                        adminBillId = c.Int(),
                        interCompanyId = c.Int(),
                        loansAdvanceId = c.Int(),
                        VATBookRefNumberRefId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tabAdminBill", t => t.adminBillId)
                .ForeignKey("dbo.tabCompany", t => t.companyId)
                .ForeignKey("dbo.CustomerCompanies", t => t.customerId)
                .ForeignKey("dbo.tabDepartment", t => t.deptId)
                .ForeignKey("dbo.InterBankTransfers", t => t.interBankTransferId)
                .ForeignKey("dbo.InterCompanyBankTransfers", t => t.interCompanyId)
                .ForeignKey("dbo.LoansAdvances", t => t.loansAdvanceId)
                .ForeignKey("dbo.Currencies", t => t.currencyId)
                .ForeignKey("dbo.Payments", t => t.paymentId)
                .ForeignKey("dbo.SalesReceipts", t => t.saleReceiptId)
                .ForeignKey("dbo.SaleInvoices", t => t.saleInvoiceId)
                .ForeignKey("dbo.PurchaseInvoices", t => t.purchaseInvoiceId)
                .ForeignKey("dbo.VATBookRefNumbers", t => t.VATBookRefNumberRefId)
                .ForeignKey("dbo.tabVendor", t => t.vendorId)
                .ForeignKey("dbo.Bills", t => t.vendorBillId)
                .Index(t => t.companyId)
                .Index(t => t.customerId)
                .Index(t => t.vendorId)
                .Index(t => t.deptId)
                .Index(t => t.currencyId)
                .Index(t => t.saleInvoiceId)
                .Index(t => t.purchaseInvoiceId)
                .Index(t => t.vendorBillId)
                .Index(t => t.saleReceiptId)
                .Index(t => t.interBankTransferId)
                .Index(t => t.paymentId)
                .Index(t => t.adminBillId)
                .Index(t => t.interCompanyId)
                .Index(t => t.loansAdvanceId)
                .Index(t => t.VATBookRefNumberRefId);
            
            CreateTable(
                "dbo.PurchaseInvoices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PIReferenceNo = c.String(),
                        SOReferenceNo = c.String(),
                        SalesReferenceNo = c.String(),
                        FinanceRefrenceNo = c.String(),
                        VendorName = c.String(),
                        OfferReferenceNo = c.String(),
                        totalInvoiceAmount = c.Double(nullable: false),
                        POCFRValue = c.Double(nullable: false),
                        totalBaseAmount = c.Double(nullable: false),
                        exchangeRate = c.Single(nullable: false),
                        marginExchangeRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        stage = c.String(),
                        ClosingDate = c.DateTime(),
                        POReferenceNo = c.String(),
                        PODate = c.DateTime(),
                        PODeliveryDate = c.DateTime(),
                        SODate = c.DateTime(),
                        SODeliveryDate = c.DateTime(),
                        LastStatusChangeDate = c.DateTime(),
                        CreationDate = c.DateTime(),
                        PurchaseInvoicetype = c.Int(nullable: false),
                        isVoid = c.Boolean(nullable: false),
                        isReviewed = c.Boolean(),
                        needReview = c.Boolean(),
                        PendingForClosing = c.Boolean(),
                        PendingForReApproval = c.Boolean(),
                        isApproved = c.Boolean(),
                        ApprovedDate = c.DateTime(),
                        ReApprovalDate = c.DateTime(),
                        isReApproved = c.Boolean(),
                        currency_Id = c.Int(nullable: false),
                        purchaseOrder_Id = c.Int(),
                        dept_Id = c.Int(nullable: false),
                        company_Id = c.Int(),
                        InterDepartment_Id = c.Int(),
                        InterCompany_Id = c.Int(),
                        isInterCompany = c.Boolean(),
                        customerCompany_Id = c.Int(nullable: false),
                        user_Id = c.Int(),
                        allocation_Id = c.Int(nullable: false),
                        TotalWeight = c.Decimal(precision: 18, scale: 2),
                        TotalQuantity = c.Decimal(precision: 18, scale: 2),
                        vendor_Id = c.Int(),
                        vendorPaymentId = c.Int(),
                        tax_Id = c.Int(),
                        POCER = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PIAmuontSOC = c.Decimal(nullable: false, precision: 18, scale: 2),
                        POPaymentterm_Id = c.Int(),
                        incoterm_Id = c.Int(),
                        TitleValue1Id = c.Int(),
                        TitleValue2Id = c.Int(),
                        CostSheet_Id = c.Int(),
                        GLPostingDate = c.DateTime(),
                        totaltaxAmount = c.Double(nullable: false),
                        isAdjustedTax = c.Boolean(nullable: false),
                        transactionHolderId = c.Int(),
                        holderChangeDate = c.DateTime(nullable: false),
                        statusClass_Id = c.Int(),
                        VATBookRefId = c.Int(),
                        PurchaseInvoiceStatus_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CostSheets", t => t.CostSheet_Id)
                .ForeignKey("dbo.Currencies", t => t.currency_Id, cascadeDelete: true)
                .ForeignKey("dbo.CustomerCompanies", t => t.customerCompany_Id, cascadeDelete: true)
                .ForeignKey("dbo.Employees", t => t.allocation_Id, cascadeDelete: true)
                .ForeignKey("dbo.PaymentTerms", t => t.incoterm_Id)
                .ForeignKey("dbo.PaymentTerms", t => t.POPaymentterm_Id)
                .ForeignKey("dbo.PurchaseInvoiceStatus", t => t.PurchaseInvoiceStatus_Id)
                .ForeignKey("dbo.PurchaseOrders", t => t.purchaseOrder_Id)
                .ForeignKey("dbo.StatusClasses", t => t.statusClass_Id)
                .ForeignKey("dbo.TaxNames", t => t.tax_Id)
                .ForeignKey("dbo.Incoterms", t => t.TitleValue1Id)
                .ForeignKey("dbo.Incoterms", t => t.TitleValue2Id)
                .ForeignKey("dbo.Users", t => t.user_Id)
                .ForeignKey("dbo.VATBookRefNumbers", t => t.VATBookRefId)
                .ForeignKey("dbo.tabVendor", t => t.vendor_Id)
                .ForeignKey("dbo.VendorPaymentStatus", t => t.vendorPaymentId)
                .ForeignKey("dbo.tabDepartment", t => t.dept_Id, cascadeDelete: true)
                .ForeignKey("dbo.tabDepartment", t => t.InterDepartment_Id)
                .ForeignKey("dbo.Employees", t => t.transactionHolderId)
                .ForeignKey("dbo.tabCompany", t => t.company_Id)
                .ForeignKey("dbo.tabCompany", t => t.InterCompany_Id)
                .Index(t => t.currency_Id)
                .Index(t => t.purchaseOrder_Id)
                .Index(t => t.dept_Id)
                .Index(t => t.company_Id)
                .Index(t => t.InterDepartment_Id)
                .Index(t => t.InterCompany_Id)
                .Index(t => t.customerCompany_Id)
                .Index(t => t.user_Id)
                .Index(t => t.allocation_Id)
                .Index(t => t.vendor_Id)
                .Index(t => t.vendorPaymentId)
                .Index(t => t.tax_Id)
                .Index(t => t.POPaymentterm_Id)
                .Index(t => t.incoterm_Id)
                .Index(t => t.TitleValue1Id)
                .Index(t => t.TitleValue2Id)
                .Index(t => t.CostSheet_Id)
                .Index(t => t.transactionHolderId)
                .Index(t => t.statusClass_Id)
                .Index(t => t.VATBookRefId)
                .Index(t => t.PurchaseInvoiceStatus_Id);
            
            CreateTable(
                "dbo.Inventories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        creationDate = c.DateTime(),
                        prodId = c.Int(),
                        Quantity = c.Double(nullable: false),
                        Weight = c.Double(nullable: false),
                        Debit = c.Double(nullable: false),
                        Credit = c.Double(nullable: false),
                        UnitRate = c.Double(nullable: false),
                        MER = c.Double(nullable: false),
                        transactionRefno = c.String(),
                        TransactionsType = c.Int(nullable: false),
                        moduleType = c.Int(nullable: false),
                        Balance = c.Double(nullable: false),
                        currencyId = c.Int(),
                        userId = c.Int(),
                        companyId = c.Int(),
                        deptId = c.Int(),
                        PurchaseInvoiceId = c.Int(),
                        SaleInviceId = c.Int(),
                        AverageCost = c.Double(nullable: false),
                        AmountOC = c.Double(nullable: false),
                        AmountMER = c.Double(nullable: false),
                        bookerItemId = c.Int(),
                        isAdjusted = c.Boolean(nullable: false),
                        adjustment_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BookerStatementItems", t => t.bookerItemId)
                .ForeignKey("dbo.tabCompany", t => t.companyId)
                .ForeignKey("dbo.Users", t => t.userId)
                .ForeignKey("dbo.Currencies", t => t.currencyId)
                .ForeignKey("dbo.tabDepartment", t => t.deptId)
                .ForeignKey("dbo.InventoryAdjustments", t => t.adjustment_Id)
                .ForeignKey("dbo.Products", t => t.prodId)
                .ForeignKey("dbo.PurchaseInvoices", t => t.PurchaseInvoiceId)
                .ForeignKey("dbo.SaleInvoices", t => t.SaleInviceId)
                .Index(t => t.prodId)
                .Index(t => t.currencyId)
                .Index(t => t.userId)
                .Index(t => t.companyId)
                .Index(t => t.deptId)
                .Index(t => t.PurchaseInvoiceId)
                .Index(t => t.SaleInviceId)
                .Index(t => t.bookerItemId)
                .Index(t => t.adjustment_Id);
            
            CreateTable(
                "dbo.InventoryAdjustments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ReferenceNo = c.String(),
                        CreationDate = c.DateTime(),
                        AdjustmentType = c.Int(nullable: false),
                        creator_Id = c.Int(),
                        company_Id = c.Int(),
                        depId_Id = c.Int(),
                        currency_Id = c.Int(nullable: false),
                        chartofAccount_Id = c.Int(nullable: false),
                        AdjustmentDate = c.DateTime(),
                        PendingForClosing = c.Boolean(),
                        PendingForReApproval = c.Boolean(),
                        isApproved = c.Boolean(),
                        ReApprovalDate = c.DateTime(),
                        isReApproved = c.Boolean(),
                        ApprovedDate = c.DateTime(),
                        isVoid = c.Boolean(nullable: false),
                        stage = c.String(),
                        ClosingDate = c.DateTime(),
                        LastStatusChangeDate = c.DateTime(),
                        isReviewed = c.Boolean(),
                        needReview = c.Boolean(),
                        AdjustmentStatus_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.InventoryAdjustmentStatus", t => t.AdjustmentStatus_Id)
                .ForeignKey("dbo.ChartofAccounts", t => t.chartofAccount_Id, cascadeDelete: true)
                .ForeignKey("dbo.tabCompany", t => t.company_Id)
                .ForeignKey("dbo.Users", t => t.creator_Id)
                .ForeignKey("dbo.Currencies", t => t.currency_Id, cascadeDelete: true)
                .ForeignKey("dbo.tabDepartment", t => t.depId_Id)
                .Index(t => t.creator_Id)
                .Index(t => t.company_Id)
                .Index(t => t.depId_Id)
                .Index(t => t.currency_Id)
                .Index(t => t.chartofAccount_Id)
                .Index(t => t.AdjustmentStatus_Id);
            
            CreateTable(
                "dbo.InventoryAdjustmentStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.String(),
                        isApproved = c.Boolean(nullable: false),
                        isActive = c.Boolean(),
                        backcolor = c.String(),
                        forecolor = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SaleInvoices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        referenceNo = c.String(),
                        SalesReferenceNo = c.String(),
                        FinanceRefrenceNo = c.String(),
                        BatchRefrenceNo = c.String(),
                        commisionRefrenceNo = c.String(),
                        offerReferenceNo = c.String(),
                        saleInvoiceDate = c.DateTime(),
                        CreationDate = c.DateTime(),
                        deliveryDate = c.DateTime(),
                        ETDDate = c.DateTime(),
                        ETADate = c.DateTime(),
                        ClosingDate = c.DateTime(),
                        LastStatusChangeDate = c.DateTime(),
                        LastStatusClassChangeDate = c.DateTime(),
                        BLAWBDate = c.DateTime(),
                        lCDate = c.DateTime(),
                        materialReciptDate = c.DateTime(),
                        PaymentDueStartDate = c.DateTime(),
                        ExpectedPayment = c.DateTime(),
                        PaymentDueAgeing = c.DateTime(),
                        paymentOnDate = c.DateTime(),
                        isPaid = c.Boolean(nullable: false),
                        isRedInvoice = c.Boolean(nullable: false),
                        deliveryDays = c.Double(nullable: false),
                        BLdeliveryRefNo = c.String(),
                        CreditDays = c.Int(nullable: false),
                        isVoid = c.Boolean(nullable: false),
                        targetYear = c.Int(nullable: false),
                        targetMonth = c.Int(nullable: false),
                        lCnumber = c.String(),
                        exchangeRate = c.Single(nullable: false),
                        marginExchangeRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        maker = c.String(),
                        lotNo = c.String(),
                        invoiceNo = c.String(),
                        invoiceDate = c.DateTime(),
                        origin = c.String(),
                        OwnDescription = c.String(),
                        comments = c.String(),
                        deliveryTime = c.String(),
                        totalInvoiceAmount = c.Double(nullable: false),
                        PaymentYear = c.Int(),
                        PaymentQuarter = c.Int(),
                        SOCFRValue = c.Double(nullable: false),
                        totalBaseAmount = c.Double(nullable: false),
                        RemainingBaseAmount = c.Double(nullable: false),
                        UnInvoicedTotalWeight = c.Decimal(precision: 18, scale: 2),
                        UnInvoicedTotalQuantity = c.Decimal(precision: 18, scale: 2),
                        ReceivedAmount = c.Double(nullable: false),
                        TotalWeight = c.Decimal(precision: 18, scale: 2),
                        TotalQuantity = c.Decimal(precision: 18, scale: 2),
                        SoAmountSER = c.Double(nullable: false),
                        isPercentTax = c.Boolean(nullable: false),
                        salesTax = c.Double(nullable: false),
                        customerCompany_Id = c.Int(nullable: false),
                        isReviewed = c.Boolean(),
                        needReview = c.Boolean(),
                        stage = c.String(),
                        dept_Id = c.Int(nullable: false),
                        CommissionSummarySheetId = c.Int(),
                        user_Id = c.Int(),
                        allocation_Id = c.Int(nullable: false),
                        principal_Id = c.Int(nullable: false),
                        company_Id = c.Int(),
                        InterDepartment_Id = c.Int(),
                        InterCompany_Id = c.Int(),
                        isInterCompany = c.Boolean(),
                        saleInvoicetype = c.Int(nullable: false),
                        SaleOrderId = c.Int(),
                        currency_Id = c.Int(nullable: false),
                        paymentterm_Id = c.Int(nullable: false),
                        incoterm_Id = c.Int(nullable: false),
                        PendingForClosing = c.Boolean(),
                        isApproved = c.Boolean(),
                        ApprovedDate = c.DateTime(),
                        TitleValue1Id = c.Int(),
                        TitleValue2Id = c.Int(),
                        SItaxSubject = c.String(),
                        CISubject = c.String(),
                        DNSubject = c.String(),
                        Commission = c.String(),
                        GLPostingDate = c.DateTime(),
                        bank_Id = c.Int(),
                        account_Id = c.Int(),
                        totaltaxAmount = c.Double(nullable: false),
                        CostSheet_Id = c.Int(),
                        amountSOC = c.Double(nullable: false),
                        totalDistributionAmount = c.Double(nullable: false),
                        totalGSTAmount = c.Double(nullable: false),
                        totalAmountAfterGST = c.Double(nullable: false),
                        totalClaimDiscount = c.Double(nullable: false),
                        totalPassOn = c.Double(nullable: false),
                        totalFocSampling = c.Double(nullable: false),
                        totalNetAmount = c.Double(nullable: false),
                        isInterCompanyReceivable = c.Boolean(nullable: false),
                        isAdvancePayment = c.Boolean(nullable: false),
                        transactionHolderId = c.Int(),
                        holderChangeDate = c.DateTime(nullable: false),
                        isSTLGenerated = c.Boolean(nullable: false),
                        isSTLDiscount = c.Boolean(nullable: false),
                        stlDiscountCurrency_Id = c.Int(),
                        stlDiscountAmount = c.Double(nullable: false),
                        stlSTLCurrency_Id = c.Int(),
                        stlAmount = c.Double(nullable: false),
                        lotNumberId = c.Int(),
                        ExpectedDiscountDate = c.DateTime(),
                        statusClass_Id = c.Int(),
                        insuranceRequired = c.Boolean(nullable: false),
                        insuranceApplied = c.Boolean(nullable: false),
                        insuranceNotApplicable = c.Boolean(nullable: false),
                        insuranceAppliedBy_Id = c.Int(),
                        VATBookRefId = c.Int(),
                        saleInvoiceStatus_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Accounts", t => t.account_Id)
                .ForeignKey("dbo.Banks", t => t.bank_Id)
                .ForeignKey("dbo.CommissionSummarySheets", t => t.CommissionSummarySheetId)
                .ForeignKey("dbo.CostSheets", t => t.CostSheet_Id)
                .ForeignKey("dbo.Currencies", t => t.currency_Id, cascadeDelete: true)
                .ForeignKey("dbo.CustomerCompanies", t => t.customerCompany_Id, cascadeDelete: true)
                .ForeignKey("dbo.Employees", t => t.allocation_Id, cascadeDelete: true)
                .ForeignKey("dbo.Incoterms", t => t.incoterm_Id, cascadeDelete: true)
                .ForeignKey("dbo.Employees", t => t.insuranceAppliedBy_Id)
                .ForeignKey("dbo.LotNumbers", t => t.lotNumberId)
                .ForeignKey("dbo.PaymentTerms", t => t.paymentterm_Id, cascadeDelete: true)
                .ForeignKey("dbo.Principals", t => t.principal_Id, cascadeDelete: true)
                .ForeignKey("dbo.SaleInvoiceStatus", t => t.saleInvoiceStatus_Id)
                .ForeignKey("dbo.SaleOrders", t => t.SaleOrderId)
                .ForeignKey("dbo.StatusClasses", t => t.statusClass_Id)
                .ForeignKey("dbo.Currencies", t => t.stlSTLCurrency_Id)
                .ForeignKey("dbo.Currencies", t => t.stlDiscountCurrency_Id)
                .ForeignKey("dbo.Incoterms", t => t.TitleValue1Id)
                .ForeignKey("dbo.Incoterms", t => t.TitleValue2Id)
                .ForeignKey("dbo.Users", t => t.user_Id)
                .ForeignKey("dbo.VATBookRefNumbers", t => t.VATBookRefId)
                .ForeignKey("dbo.tabDepartment", t => t.dept_Id, cascadeDelete: true)
                .ForeignKey("dbo.tabDepartment", t => t.InterDepartment_Id)
                .ForeignKey("dbo.Employees", t => t.transactionHolderId)
                .ForeignKey("dbo.tabCompany", t => t.company_Id)
                .ForeignKey("dbo.tabCompany", t => t.InterCompany_Id)
                .Index(t => t.customerCompany_Id)
                .Index(t => t.dept_Id)
                .Index(t => t.CommissionSummarySheetId)
                .Index(t => t.user_Id)
                .Index(t => t.allocation_Id)
                .Index(t => t.principal_Id)
                .Index(t => t.company_Id)
                .Index(t => t.InterDepartment_Id)
                .Index(t => t.InterCompany_Id)
                .Index(t => t.SaleOrderId)
                .Index(t => t.currency_Id)
                .Index(t => t.paymentterm_Id)
                .Index(t => t.incoterm_Id)
                .Index(t => t.TitleValue1Id)
                .Index(t => t.TitleValue2Id)
                .Index(t => t.bank_Id)
                .Index(t => t.account_Id)
                .Index(t => t.CostSheet_Id)
                .Index(t => t.transactionHolderId)
                .Index(t => t.stlDiscountCurrency_Id)
                .Index(t => t.stlSTLCurrency_Id)
                .Index(t => t.lotNumberId)
                .Index(t => t.statusClass_Id)
                .Index(t => t.insuranceAppliedBy_Id)
                .Index(t => t.VATBookRefId)
                .Index(t => t.saleInvoiceStatus_Id);
            
            CreateTable(
                "dbo.CustomerCredits",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SerialNo = c.Int(nullable: false),
                        SaleInvoiceId = c.Int(),
                        CustomerCompanyId = c.Int(),
                        creditAmount = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CustomerCompanies", t => t.CustomerCompanyId)
                .ForeignKey("dbo.SaleInvoices", t => t.SaleInvoiceId)
                .Index(t => t.SaleInvoiceId)
                .Index(t => t.CustomerCompanyId);
            
            CreateTable(
                "dbo.LotNumbers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        companyId = c.Int(),
                        deptId = c.Int(),
                        LotNo = c.String(),
                        isActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tabCompany", t => t.companyId)
                .ForeignKey("dbo.tabDepartment", t => t.deptId)
                .Index(t => t.companyId)
                .Index(t => t.deptId);
            
            CreateTable(
                "dbo.SaleInvoiceStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.String(),
                        isApproved = c.Boolean(nullable: false),
                        isActive = c.Boolean(),
                        backcolor = c.String(),
                        forecolor = c.String(),
                        isDisable = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SaleOrders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        referenceNo = c.String(),
                        SalesReferenceNo = c.String(),
                        FinanceRefrenceNo = c.String(),
                        commisionRefrenceNo = c.String(),
                        offerReferenceNo = c.String(),
                        saleOrderDate = c.DateTime(),
                        CreationDate = c.DateTime(),
                        ClosingDate = c.DateTime(),
                        LastStatusChangeDate = c.DateTime(),
                        deliveryDate = c.DateTime(),
                        shipmentDate = c.DateTime(),
                        orderConfirmationDate = c.DateTime(),
                        billLaddingDate = c.DateTime(),
                        lCDate = c.DateTime(),
                        materialReciptDate = c.DateTime(),
                        revisedShipmentDate = c.DateTime(),
                        PaymentDueStartDate = c.DateTime(),
                        ExpectedPayment = c.DateTime(),
                        PaymentDueAgeing = c.DateTime(),
                        CreditDays = c.Int(nullable: false),
                        targetYear = c.Int(nullable: false),
                        targetMonth = c.Int(nullable: false),
                        lCnumber = c.String(),
                        exchangeRate = c.Single(nullable: false),
                        netCommision = c.Decimal(precision: 18, scale: 2),
                        commision = c.Decimal(precision: 18, scale: 2),
                        commisioninBase = c.Decimal(precision: 18, scale: 2),
                        PER = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SoAmountPER = c.Double(nullable: false),
                        PERValue = c.Double(nullable: false),
                        marginExchangeRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        margin = c.Decimal(precision: 18, scale: 2),
                        BudgetedMargininBase = c.Decimal(precision: 18, scale: 2),
                        SalesBudgetedMargin = c.Decimal(precision: 18, scale: 2),
                        BudgetedMarginPercent = c.Decimal(precision: 18, scale: 2),
                        RevisedMargin = c.Decimal(precision: 18, scale: 2),
                        RevisedMargininBase = c.Decimal(precision: 18, scale: 2),
                        SalesRevisedMargin = c.Decimal(precision: 18, scale: 2),
                        RevisedMarginPercent = c.Decimal(precision: 18, scale: 2),
                        ActualMarginPercent = c.Decimal(precision: 18, scale: 2),
                        ActualMargin = c.Decimal(precision: 18, scale: 2),
                        ActualMargininBase = c.Decimal(precision: 18, scale: 2),
                        SalesActualMargin = c.Decimal(precision: 18, scale: 2),
                        SystemMargin = c.Decimal(precision: 18, scale: 2),
                        SalesSystemMargin = c.Decimal(precision: 18, scale: 2),
                        SalesMarketMargin = c.Decimal(precision: 18, scale: 2),
                        transshipment = c.Boolean(),
                        packing = c.String(),
                        LCShipmentDate = c.DateTime(),
                        LCExpiryDate = c.DateTime(),
                        deliveryTerm = c.String(),
                        LCAmedmentNo = c.String(),
                        LCShipmentAmendmentDate = c.DateTime(),
                        LCExpiryAmedmentDate = c.DateTime(),
                        maker = c.String(),
                        origin = c.String(),
                        OwnDescription = c.String(),
                        comments = c.String(),
                        deliveryTime = c.String(),
                        totalFOBValue = c.Double(nullable: false),
                        totalCFRValue = c.Double(nullable: false),
                        RemainingFOBValue = c.Double(nullable: false),
                        RemainingCFRValue = c.Double(nullable: false),
                        totalBaseFOBValue = c.Double(nullable: false),
                        totalBaseCFRValue = c.Double(nullable: false),
                        RemainingBaseFOBValue = c.Double(nullable: false),
                        RemainingBaseCFRValue = c.Double(nullable: false),
                        UnInvoicedTotalWeight = c.Decimal(precision: 18, scale: 2),
                        UnInvoicedTotalQuantity = c.Decimal(precision: 18, scale: 2),
                        ReceivedAmount = c.Double(nullable: false),
                        TotalWeight = c.Decimal(precision: 18, scale: 2),
                        TotalQuantity = c.Decimal(precision: 18, scale: 2),
                        SoAmountSER = c.Double(nullable: false),
                        isVoid = c.Boolean(nullable: false),
                        isPercentTax = c.Boolean(nullable: false),
                        salesTax = c.Double(nullable: false),
                        customerCompany_Id = c.Int(nullable: false),
                        isReviewed = c.Boolean(),
                        needReview = c.Boolean(),
                        stage = c.String(),
                        InvoiceStage = c.String(),
                        WarrantyId = c.Int(),
                        dept_Id = c.Int(nullable: false),
                        CostSheet_Id = c.Int(),
                        CommissionSummarySheetId = c.Int(),
                        user_Id = c.Int(),
                        allocation_Id = c.Int(nullable: false),
                        principal_Id = c.Int(nullable: false),
                        company_Id = c.Int(),
                        InterDepartment_Id = c.Int(),
                        InterCompany_Id = c.Int(),
                        isInterCompany = c.Boolean(),
                        saleOrdertype = c.Int(nullable: false),
                        vendorPaymentId = c.Int(),
                        offer_Id = c.Int(),
                        bid_Id = c.Int(),
                        currency_Id = c.Int(nullable: false),
                        costCentercurrency_Id = c.Int(),
                        costCenterExchangeRate = c.Double(nullable: false),
                        costCenterAmount = c.Double(nullable: false),
                        paymentterm_Id = c.Int(nullable: false),
                        incoterm_Id = c.Int(nullable: false),
                        BillRefNoId = c.Int(),
                        PendingForClosing = c.Boolean(),
                        PendingForReApproval = c.Boolean(),
                        isApproved = c.Boolean(),
                        ReApprovalDate = c.DateTime(),
                        isReApproved = c.Boolean(),
                        ApprovedDate = c.DateTime(),
                        TitleValue1Id = c.Int(),
                        TitleValue2Id = c.Int(),
                        saleExchangerateId = c.Int(),
                        marketExchangerateId = c.Int(),
                        taxNameId = c.Int(),
                        deliveryDateFinal = c.DateTime(),
                        totaltaxAmount = c.Double(nullable: false),
                        interBankTransfer_Id = c.Int(),
                        totalComissionSER = c.Double(nullable: false),
                        totalComissionMER = c.Double(nullable: false),
                        totalNetComissionSER = c.Double(nullable: false),
                        totalNetComissionMER = c.Double(nullable: false),
                        ParentSO_Id = c.Int(),
                        totalDistributionAmount = c.Double(nullable: false),
                        totalGSTAmount = c.Double(nullable: false),
                        totalAmountAfterGST = c.Double(nullable: false),
                        totalClaimDiscount = c.Double(nullable: false),
                        totalPassOn = c.Double(nullable: false),
                        totalFocSampling = c.Double(nullable: false),
                        totalNetAmount = c.Double(nullable: false),
                        revisedBudgetAmount = c.Double(nullable: false),
                        ccSER = c.Double(nullable: false),
                        ccMER = c.Double(nullable: false),
                        Budget_Id = c.Int(),
                        PerformanceSheet_Id = c.Int(),
                        auditYear = c.DateTime(),
                        SaleOrderKey_Id = c.Int(),
                        transactionHolderId = c.Int(),
                        holderChangeDate = c.DateTime(nullable: false),
                        statusClass_Id = c.Int(),
                        isGeneratedBySOLink = c.Boolean(nullable: false),
                        auditYearAdjustment_Id = c.Int(),
                        moduleContract_Id = c.Int(),
                        saleOrderStatus_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AuditYearAdjustments", t => t.auditYearAdjustment_Id)
                .ForeignKey("dbo.Bids", t => t.bid_Id)
                .ForeignKey("dbo.VendorBillReferences", t => t.BillRefNoId)
                .ForeignKey("dbo.BudgetCostSheets", t => t.Budget_Id)
                .ForeignKey("dbo.CommissionSummarySheets", t => t.CommissionSummarySheetId)
                .ForeignKey("dbo.Currencies", t => t.costCentercurrency_Id)
                .ForeignKey("dbo.CostSheets", t => t.CostSheet_Id)
                .ForeignKey("dbo.Currencies", t => t.currency_Id, cascadeDelete: true)
                .ForeignKey("dbo.CustomerCompanies", t => t.customerCompany_Id, cascadeDelete: true)
                .ForeignKey("dbo.Employees", t => t.allocation_Id, cascadeDelete: true)
                .ForeignKey("dbo.Incoterms", t => t.incoterm_Id, cascadeDelete: true)
                .ForeignKey("dbo.InterBankTransfers", t => t.interBankTransfer_Id)
                .ForeignKey("dbo.MarketExchangeRates", t => t.marketExchangerateId)
                .ForeignKey("dbo.ModuleContracts", t => t.moduleContract_Id)
                .ForeignKey("dbo.Offers", t => t.offer_Id)
                .ForeignKey("dbo.SaleOrders", t => t.ParentSO_Id)
                .ForeignKey("dbo.PaymentTerms", t => t.paymentterm_Id, cascadeDelete: true)
                .ForeignKey("dbo.PerformanceSheets", t => t.PerformanceSheet_Id)
                .ForeignKey("dbo.Principals", t => t.principal_Id, cascadeDelete: true)
                .ForeignKey("dbo.SaleOrdeRrefKeys", t => t.SaleOrderKey_Id)
                .ForeignKey("dbo.SalesExchangeRates", t => t.saleExchangerateId)
                .ForeignKey("dbo.SaleOrderStatus", t => t.saleOrderStatus_Id)
                .ForeignKey("dbo.StatusClasses", t => t.statusClass_Id)
                .ForeignKey("dbo.TaxNames", t => t.taxNameId)
                .ForeignKey("dbo.Incoterms", t => t.TitleValue1Id)
                .ForeignKey("dbo.Incoterms", t => t.TitleValue2Id)
                .ForeignKey("dbo.Users", t => t.user_Id)
                .ForeignKey("dbo.VendorPaymentStatus", t => t.vendorPaymentId)
                .ForeignKey("dbo.Warranties", t => t.WarrantyId)
                .ForeignKey("dbo.tabDepartment", t => t.dept_Id, cascadeDelete: true)
                .ForeignKey("dbo.tabDepartment", t => t.InterDepartment_Id)
                .ForeignKey("dbo.Employees", t => t.transactionHolderId)
                .ForeignKey("dbo.tabCompany", t => t.company_Id)
                .ForeignKey("dbo.tabCompany", t => t.InterCompany_Id)
                .Index(t => t.customerCompany_Id)
                .Index(t => t.WarrantyId)
                .Index(t => t.dept_Id)
                .Index(t => t.CostSheet_Id)
                .Index(t => t.CommissionSummarySheetId)
                .Index(t => t.user_Id)
                .Index(t => t.allocation_Id)
                .Index(t => t.principal_Id)
                .Index(t => t.company_Id)
                .Index(t => t.InterDepartment_Id)
                .Index(t => t.InterCompany_Id)
                .Index(t => t.vendorPaymentId)
                .Index(t => t.offer_Id)
                .Index(t => t.bid_Id)
                .Index(t => t.currency_Id)
                .Index(t => t.costCentercurrency_Id)
                .Index(t => t.paymentterm_Id)
                .Index(t => t.incoterm_Id)
                .Index(t => t.BillRefNoId)
                .Index(t => t.TitleValue1Id)
                .Index(t => t.TitleValue2Id)
                .Index(t => t.saleExchangerateId)
                .Index(t => t.marketExchangerateId)
                .Index(t => t.taxNameId)
                .Index(t => t.interBankTransfer_Id)
                .Index(t => t.ParentSO_Id)
                .Index(t => t.Budget_Id)
                .Index(t => t.PerformanceSheet_Id)
                .Index(t => t.SaleOrderKey_Id)
                .Index(t => t.transactionHolderId)
                .Index(t => t.statusClass_Id)
                .Index(t => t.auditYearAdjustment_Id)
                .Index(t => t.moduleContract_Id)
                .Index(t => t.saleOrderStatus_Id);
            
            CreateTable(
                "dbo.VendorBillReferences",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Reference = c.String(),
                        companyId = c.Int(),
                        isActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tabCompany", t => t.companyId)
                .Index(t => t.companyId);
            
            CreateTable(
                "dbo.MemorandumSales",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        referenceNo = c.String(),
                        CustomerReferenceNo = c.String(),
                        PrincipleReferenceNo = c.String(),
                        CustomerReferenceDate = c.DateTime(),
                        PrincipleReferenceDate = c.DateTime(),
                        ExpectedClosingDate = c.DateTime(),
                        memorandumSalesDate = c.DateTime(),
                        CreationDate = c.DateTime(),
                        InvoiceDate = c.DateTime(),
                        ApplySaleRegister = c.Boolean(nullable: false),
                        exchangeRate = c.Single(nullable: false),
                        ClosingDate = c.DateTime(),
                        LastStatusChangeDate = c.DateTime(),
                        OwnDescription = c.String(),
                        isVoid = c.Boolean(nullable: false),
                        comments = c.String(),
                        totalFOBValue = c.Double(nullable: false),
                        totalCFRValue = c.Double(nullable: false),
                        customerCompany_Id = c.Int(nullable: false),
                        isReviewed = c.Boolean(),
                        needReview = c.Boolean(),
                        stage = c.String(),
                        dept_Id = c.Int(nullable: false),
                        user_Id = c.Int(),
                        principal_Id = c.Int(nullable: false),
                        company_Id = c.Int(),
                        InterDepartment_Id = c.Int(),
                        InterCompany_Id = c.Int(),
                        isInterCompany = c.Boolean(),
                        SaleOrder_Id = c.Int(),
                        Offer_Id = c.Int(),
                        TotalWeight = c.Decimal(precision: 18, scale: 2),
                        TotalQuantity = c.Decimal(precision: 18, scale: 2),
                        isPercentTax = c.Boolean(nullable: false),
                        salesTax = c.Double(nullable: false),
                        PendingForClosing = c.Boolean(),
                        isApproved = c.Boolean(),
                        ApprovedDate = c.DateTime(),
                        TitleValue1Id = c.Int(),
                        TitleValue2Id = c.Int(),
                        memorandumSaleStatus_Id = c.Int(),
                        ModuleContract_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CustomerCompanies", t => t.customerCompany_Id, cascadeDelete: true)
                .ForeignKey("dbo.MemorandumSaleStatus", t => t.memorandumSaleStatus_Id)
                .ForeignKey("dbo.Offers", t => t.Offer_Id)
                .ForeignKey("dbo.Principals", t => t.principal_Id, cascadeDelete: true)
                .ForeignKey("dbo.SaleOrders", t => t.SaleOrder_Id)
                .ForeignKey("dbo.Incoterms", t => t.TitleValue1Id)
                .ForeignKey("dbo.Incoterms", t => t.TitleValue2Id)
                .ForeignKey("dbo.Users", t => t.user_Id)
                .ForeignKey("dbo.ModuleContracts", t => t.ModuleContract_Id)
                .ForeignKey("dbo.tabDepartment", t => t.dept_Id, cascadeDelete: true)
                .ForeignKey("dbo.tabDepartment", t => t.InterDepartment_Id)
                .ForeignKey("dbo.tabCompany", t => t.company_Id)
                .ForeignKey("dbo.tabCompany", t => t.InterCompany_Id)
                .Index(t => t.customerCompany_Id)
                .Index(t => t.dept_Id)
                .Index(t => t.user_Id)
                .Index(t => t.principal_Id)
                .Index(t => t.company_Id)
                .Index(t => t.InterDepartment_Id)
                .Index(t => t.InterCompany_Id)
                .Index(t => t.SaleOrder_Id)
                .Index(t => t.Offer_Id)
                .Index(t => t.TitleValue1Id)
                .Index(t => t.TitleValue2Id)
                .Index(t => t.memorandumSaleStatus_Id)
                .Index(t => t.ModuleContract_Id);
            
            CreateTable(
                "dbo.MemorandumSaleStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.String(),
                        isApproved = c.Boolean(nullable: false),
                        isActive = c.Boolean(),
                        backcolor = c.String(),
                        forecolor = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Offers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        InquiryReferenceNo = c.String(),
                        SalesReferenceNo = c.String(),
                        offerReferenceNo = c.String(),
                        commisionRefrenceNo = c.String(),
                        OfferDate = c.DateTime(),
                        CreationDate = c.DateTime(),
                        LastStatusChangeDate = c.DateTime(),
                        InquiryDate = c.DateTime(),
                        offerValidityDate = c.DateTime(),
                        deliveryDate = c.DateTime(),
                        maker = c.String(),
                        origin = c.String(),
                        OwnDescription = c.String(),
                        isVoid = c.Boolean(nullable: false),
                        responseDate = c.DateTime(),
                        exchngeRate = c.Single(nullable: false),
                        bidOpenDate = c.DateTime(),
                        alertDate = c.DateTime(),
                        closingDate = c.DateTime(),
                        commision = c.Decimal(precision: 18, scale: 2),
                        marginExchangeRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        margin = c.Decimal(precision: 18, scale: 2),
                        comments = c.String(),
                        totalFOBValue = c.Double(nullable: false),
                        totalCFRValue = c.Double(nullable: false),
                        totalBaseFOBValue = c.Double(nullable: false),
                        totalBaseCFRValue = c.Double(nullable: false),
                        isPercentTax = c.Boolean(nullable: false),
                        salesTax = c.Double(nullable: false),
                        TotalWeight = c.Decimal(precision: 18, scale: 2),
                        TotalQuantity = c.Decimal(precision: 18, scale: 2),
                        customerCompany_Id = c.Int(nullable: false),
                        dept_Id = c.Int(nullable: false),
                        user_Id = c.Int(),
                        allocation_Id = c.Int(nullable: false),
                        company_Id = c.Int(),
                        InterDepartment_Id = c.Int(),
                        InterCompany_Id = c.Int(),
                        isInterCompany = c.Boolean(),
                        offertype = c.Int(nullable: false),
                        principal_Id = c.Int(),
                        inquiry_Id = c.Int(),
                        isReviewed = c.Boolean(),
                        needReview = c.Boolean(),
                        stage = c.String(),
                        bid_Id = c.Int(),
                        currency_Id = c.Int(),
                        paymentterm_Id = c.Int(nullable: false),
                        incoterm_Id = c.Int(nullable: false),
                        PendingForClosing = c.Boolean(),
                        isApproved = c.Boolean(),
                        ApprovedDate = c.DateTime(),
                        TitleValue1Id = c.Int(),
                        TitleValue2Id = c.Int(),
                        uniqueNumber = c.String(),
                        transactionHolderId = c.Int(),
                        holderChangeDate = c.DateTime(nullable: false),
                        statusClass_Id = c.Int(),
                        CostSheet_Id = c.Int(),
                        costCenterCurrencyId = c.Int(),
                        SER = c.Double(nullable: false),
                        MER = c.Double(nullable: false),
                        totalOfferAmount = c.Double(nullable: false),
                        totalOfferAmountSER = c.Double(nullable: false),
                        totalOfferAmountMER = c.Double(nullable: false),
                        totalbudgetCost = c.Double(nullable: false),
                        budgetMarginSER = c.Double(nullable: false),
                        budgetMarginMER = c.Double(nullable: false),
                        offerStatus_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Bids", t => t.bid_Id)
                .ForeignKey("dbo.Currencies", t => t.costCenterCurrencyId)
                .ForeignKey("dbo.CostSheets", t => t.CostSheet_Id)
                .ForeignKey("dbo.Currencies", t => t.currency_Id)
                .ForeignKey("dbo.CustomerCompanies", t => t.customerCompany_Id, cascadeDelete: true)
                .ForeignKey("dbo.Employees", t => t.allocation_Id, cascadeDelete: true)
                .ForeignKey("dbo.Incoterms", t => t.incoterm_Id, cascadeDelete: true)
                .ForeignKey("dbo.Inquiries", t => t.inquiry_Id)
                .ForeignKey("dbo.OfferStatus", t => t.offerStatus_Id)
                .ForeignKey("dbo.PaymentTerms", t => t.paymentterm_Id, cascadeDelete: true)
                .ForeignKey("dbo.Principals", t => t.principal_Id)
                .ForeignKey("dbo.StatusClasses", t => t.statusClass_Id)
                .ForeignKey("dbo.Incoterms", t => t.TitleValue1Id)
                .ForeignKey("dbo.Incoterms", t => t.TitleValue2Id)
                .ForeignKey("dbo.Users", t => t.user_Id)
                .ForeignKey("dbo.tabDepartment", t => t.dept_Id, cascadeDelete: true)
                .ForeignKey("dbo.tabDepartment", t => t.InterDepartment_Id)
                .ForeignKey("dbo.Employees", t => t.transactionHolderId)
                .ForeignKey("dbo.tabCompany", t => t.company_Id)
                .ForeignKey("dbo.tabCompany", t => t.InterCompany_Id)
                .Index(t => t.customerCompany_Id)
                .Index(t => t.dept_Id)
                .Index(t => t.user_Id)
                .Index(t => t.allocation_Id)
                .Index(t => t.company_Id)
                .Index(t => t.InterDepartment_Id)
                .Index(t => t.InterCompany_Id)
                .Index(t => t.principal_Id)
                .Index(t => t.inquiry_Id)
                .Index(t => t.bid_Id)
                .Index(t => t.currency_Id)
                .Index(t => t.paymentterm_Id)
                .Index(t => t.incoterm_Id)
                .Index(t => t.TitleValue1Id)
                .Index(t => t.TitleValue2Id)
                .Index(t => t.transactionHolderId)
                .Index(t => t.statusClass_Id)
                .Index(t => t.CostSheet_Id)
                .Index(t => t.costCenterCurrencyId)
                .Index(t => t.offerStatus_Id);
            
            CreateTable(
                "dbo.OfferStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.String(),
                        isApproved = c.Boolean(nullable: false),
                        isActive = c.Boolean(),
                        backcolor = c.String(),
                        forecolor = c.String(),
                        isDisable = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PerformanceSheets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        soNumber = c.String(),
                        customerId = c.Int(),
                        deptId = c.Int(),
                        supervoisedId = c.Int(),
                        staffLevelOneId = c.Int(),
                        staffLevelTwoId = c.Int(),
                        totalPoints = c.Double(nullable: false),
                        totalPointsPerc = c.Double(nullable: false),
                        totalAveragePoints = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CustomerCompanies", t => t.customerId)
                .ForeignKey("dbo.tabDepartment", t => t.deptId)
                .ForeignKey("dbo.Employees", t => t.staffLevelOneId)
                .ForeignKey("dbo.Employees", t => t.staffLevelTwoId)
                .ForeignKey("dbo.Employees", t => t.supervoisedId)
                .Index(t => t.customerId)
                .Index(t => t.deptId)
                .Index(t => t.supervoisedId)
                .Index(t => t.staffLevelOneId)
                .Index(t => t.staffLevelTwoId);
            
            CreateTable(
                "dbo.PerfomarmanceSheetFields",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Head_Id = c.Int(),
                        point = c.Double(nullable: false),
                        revisedPoint = c.Double(nullable: false),
                        PerformanceSheet_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PerformanceSheetHeads", t => t.Head_Id)
                .ForeignKey("dbo.PerformanceSheets", t => t.PerformanceSheet_Id)
                .Index(t => t.Head_Id)
                .Index(t => t.PerformanceSheet_Id);
            
            CreateTable(
                "dbo.PerformanceSheetHeads",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        HeadName = c.String(),
                        isActive = c.Boolean(nullable: false),
                        SortId = c.Int(nullable: false),
                        creatorId = c.Int(),
                        totalPoints = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.creatorId)
                .Index(t => t.creatorId);
            
            CreateTable(
                "dbo.SaleOrdeRrefKeys",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        key = c.String(nullable: false),
                        keyDate = c.DateTime(nullable: false),
                        Creator = c.String(nullable: false),
                        dept_Id = c.Int(nullable: false),
                        comp_Id = c.Int(nullable: false),
                        salesRefNo = c.String(),
                        SaleOrderNumber = c.Int(nullable: false),
                        amountOC = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tabCompany", t => t.comp_Id, cascadeDelete: true)
                .ForeignKey("dbo.tabDepartment", t => t.dept_Id, cascadeDelete: true)
                .Index(t => t.dept_Id)
                .Index(t => t.comp_Id);
            
            CreateTable(
                "dbo.SaleOrderStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.String(),
                        isApproved = c.Boolean(nullable: false),
                        isActive = c.Boolean(),
                        backcolor = c.String(),
                        forecolor = c.String(),
                        isDisable = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SplitPERs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Year = c.DateTime(),
                        Month = c.DateTime(),
                        Amount = c.Double(nullable: false),
                        saleOrderId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SaleOrders", t => t.saleOrderId)
                .Index(t => t.saleOrderId);
            
            CreateTable(
                "dbo.VendorPaymentStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.String(),
                        isApproved = c.Boolean(nullable: false),
                        isActive = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SalesReceipts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        receiptType = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        companyId = c.Int(),
                        deptId = c.Int(),
                        CustomerId = c.Int(),
                        CustomerCreditSerialNo = c.Int(nullable: false),
                        SystemRefNo = c.String(),
                        ReceiptRefNo = c.String(),
                        CurrencyId = c.Int(),
                        rentalInvoiceId = c.Int(),
                        CollectionAmount = c.Double(nullable: false),
                        TotalCollectionAmount = c.Double(nullable: false),
                        collectionMethodId = c.Int(),
                        BankId = c.Int(nullable: false),
                        AccountId = c.Int(nullable: false),
                        AppliesToSales = c.Boolean(nullable: false),
                        transactionGroupId = c.Int(nullable: false),
                        saleInvoiceId = c.Int(),
                        receiptId = c.Int(),
                        paymentId = c.Int(),
                        StatusId = c.Int(),
                        stage = c.String(),
                        isVoid = c.Boolean(nullable: false),
                        isReviewed = c.Boolean(),
                        needReview = c.Boolean(),
                        PendingForClosing = c.Boolean(),
                        PendingForReApproval = c.Boolean(),
                        isApproved = c.Boolean(),
                        ApprovedDate = c.DateTime(),
                        isReApproved = c.Boolean(),
                        ReApprovalDate = c.DateTime(),
                        user_Id = c.Int(),
                        ClosingDate = c.DateTime(),
                        LastStatusChangeDate = c.DateTime(),
                        CreditedDate = c.DateTime(),
                        DepositedDate = c.DateTime(),
                        InstrumentNo = c.String(),
                        InstrumentDate = c.DateTime(),
                        principal_Id = c.Int(),
                        CostSheet_Id = c.Int(),
                        costSheetFieldId = c.Int(),
                        isPostToGL = c.Boolean(nullable: false),
                        GLPostingDate = c.DateTime(),
                        isBypassBank = c.Boolean(nullable: false),
                        coaAccountId = c.Int(),
                        isDeposit = c.Boolean(),
                        isAmountOC = c.Boolean(),
                        PettyCashRefId = c.Int(),
                        LoansAdvanceId = c.Int(),
                        IsAdjustedDedVAT = c.Boolean(),
                        IsAdjustedBankVAT = c.Boolean(),
                        DeductionExchangeRate = c.Double(nullable: false),
                        DeductionSOC = c.Double(nullable: false),
                        COAcredit_Id = c.Int(),
                        Description = c.String(),
                        transactionHolderId = c.Int(),
                        holderChangeDate = c.DateTime(nullable: false),
                        GLMER = c.Double(nullable: false),
                        statusClass_Id = c.Int(),
                        VATBookRefId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Accounts", t => t.AccountId, cascadeDelete: true)
                .ForeignKey("dbo.Banks", t => t.BankId, cascadeDelete: true)
                .ForeignKey("dbo.ChartofAccounts", t => t.coaAccountId)
                .ForeignKey("dbo.ChartofAccounts", t => t.COAcredit_Id)
                .ForeignKey("dbo.CollectionMethods", t => t.collectionMethodId)
                .ForeignKey("dbo.tabCompany", t => t.companyId)
                .ForeignKey("dbo.CostSheets", t => t.CostSheet_Id)
                .ForeignKey("dbo.Currencies", t => t.CurrencyId)
                .ForeignKey("dbo.CustomerCompanies", t => t.CustomerId)
                .ForeignKey("dbo.tabDepartment", t => t.deptId)
                .ForeignKey("dbo.BillRefNumbers", t => t.PettyCashRefId)
                .ForeignKey("dbo.Principals", t => t.principal_Id)
                .ForeignKey("dbo.RentalInvoices", t => t.rentalInvoiceId)
                .ForeignKey("dbo.SalesReceiptStatus", t => t.StatusId)
                .ForeignKey("dbo.SalesReceipts", t => t.receiptId)
                .ForeignKey("dbo.StatusClasses", t => t.statusClass_Id)
                .ForeignKey("dbo.Users", t => t.user_Id)
                .ForeignKey("dbo.VATBookRefNumbers", t => t.VATBookRefId)
                .ForeignKey("dbo.SaleInvoices", t => t.saleInvoiceId)
                .ForeignKey("dbo.Payments", t => t.paymentId)
                .ForeignKey("dbo.LoansAdvances", t => t.LoansAdvanceId)
                .ForeignKey("dbo.Employees", t => t.transactionHolderId)
                .Index(t => t.companyId)
                .Index(t => t.deptId)
                .Index(t => t.CustomerId)
                .Index(t => t.CurrencyId)
                .Index(t => t.rentalInvoiceId)
                .Index(t => t.collectionMethodId)
                .Index(t => t.BankId)
                .Index(t => t.AccountId)
                .Index(t => t.saleInvoiceId)
                .Index(t => t.receiptId)
                .Index(t => t.paymentId)
                .Index(t => t.StatusId)
                .Index(t => t.user_Id)
                .Index(t => t.principal_Id)
                .Index(t => t.CostSheet_Id)
                .Index(t => t.coaAccountId)
                .Index(t => t.PettyCashRefId)
                .Index(t => t.LoansAdvanceId)
                .Index(t => t.COAcredit_Id)
                .Index(t => t.transactionHolderId)
                .Index(t => t.statusClass_Id)
                .Index(t => t.VATBookRefId);
            
            CreateTable(
                "dbo.ReceiptDeductions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        deduction_Id = c.Int(),
                        SalesReceiptForDeductionId = c.Int(),
                        SalesReceiptForBankChargesId = c.Int(),
                        Amount = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Deductions", t => t.deduction_Id)
                .ForeignKey("dbo.SalesReceipts", t => t.SalesReceiptForBankChargesId)
                .ForeignKey("dbo.SalesReceipts", t => t.SalesReceiptForDeductionId)
                .Index(t => t.deduction_Id)
                .Index(t => t.SalesReceiptForDeductionId)
                .Index(t => t.SalesReceiptForBankChargesId);
            
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
                "dbo.ReceiptTaxes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        taxNameId = c.Int(),
                        SalesReceiptForDeductionTaxId = c.Int(),
                        SalesReceiptForBankChargesTaxId = c.Int(),
                        Amount = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SalesReceipts", t => t.SalesReceiptForBankChargesTaxId)
                .ForeignKey("dbo.SalesReceipts", t => t.SalesReceiptForDeductionTaxId)
                .ForeignKey("dbo.TaxNames", t => t.taxNameId)
                .Index(t => t.taxNameId)
                .Index(t => t.SalesReceiptForDeductionTaxId)
                .Index(t => t.SalesReceiptForBankChargesTaxId);
            
            CreateTable(
                "dbo.RentalInvoices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreationDate = c.DateTime(),
                        rentalOrderId = c.Int(),
                        companyId = c.Int(nullable: false),
                        deptId = c.Int(),
                        transactionGroupId = c.Int(nullable: false),
                        SystemRef = c.String(),
                        assetRentalId = c.Int(),
                        TenantRentalId = c.Int(),
                        creatorId = c.Int(),
                        statusId = c.Int(),
                        currencyId = c.Int(),
                        fromDate = c.DateTime(),
                        toDate = c.DateTime(),
                        RentMonth = c.DateTime(),
                        InvoiceAmount = c.Double(nullable: false),
                        MER = c.Double(nullable: false),
                        InvoiceAmountMER = c.Double(nullable: false),
                        TotalInvoiceAmount = c.Double(nullable: false),
                        rentalBasis = c.Int(nullable: false),
                        isActive = c.Boolean(nullable: false),
                        stage = c.String(),
                        isVoid = c.Boolean(nullable: false),
                        isReviewed = c.Boolean(),
                        needReview = c.Boolean(),
                        PendingForClosing = c.Boolean(),
                        PendingForReApproval = c.Boolean(),
                        isApproved = c.Boolean(),
                        ApprovedDate = c.DateTime(),
                        isReApproved = c.Boolean(),
                        ReApprovalDate = c.DateTime(),
                        ClosingDate = c.DateTime(),
                        LastStatusChangeDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AssetRentals", t => t.assetRentalId)
                .ForeignKey("dbo.tabCompany", t => t.companyId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.creatorId)
                .ForeignKey("dbo.Currencies", t => t.currencyId)
                .ForeignKey("dbo.tabDepartment", t => t.deptId)
                .ForeignKey("dbo.RentalOrders", t => t.rentalOrderId)
                .ForeignKey("dbo.RentalInvoiceStatus", t => t.statusId)
                .ForeignKey("dbo.TenantRentals", t => t.TenantRentalId)
                .Index(t => t.rentalOrderId)
                .Index(t => t.companyId)
                .Index(t => t.deptId)
                .Index(t => t.assetRentalId)
                .Index(t => t.TenantRentalId)
                .Index(t => t.creatorId)
                .Index(t => t.statusId)
                .Index(t => t.currencyId);
            
            CreateTable(
                "dbo.RentalOrders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreationDate = c.DateTime(),
                        rentalContractId = c.Int(),
                        companyId = c.Int(nullable: false),
                        deptId = c.Int(),
                        transactionGroupId = c.Int(nullable: false),
                        SystemRef = c.String(),
                        assetRentalId = c.Int(),
                        TenantRentalId = c.Int(),
                        creatorId = c.Int(),
                        statusId = c.Int(),
                        currencyId = c.Int(),
                        fromDate = c.DateTime(),
                        toDate = c.DateTime(),
                        MER = c.Double(nullable: false),
                        RentMonth = c.DateTime(),
                        RentAmount = c.Double(nullable: false),
                        RentAmountMER = c.Double(nullable: false),
                        rentalBasis = c.Int(nullable: false),
                        stage = c.String(),
                        isVoid = c.Boolean(nullable: false),
                        isReviewed = c.Boolean(),
                        needReview = c.Boolean(),
                        PendingForClosing = c.Boolean(),
                        PendingForReApproval = c.Boolean(),
                        isApproved = c.Boolean(),
                        ApprovedDate = c.DateTime(),
                        isReApproved = c.Boolean(),
                        ReApprovalDate = c.DateTime(),
                        ClosingDate = c.DateTime(),
                        LastStatusChangeDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AssetRentals", t => t.assetRentalId)
                .ForeignKey("dbo.tabCompany", t => t.companyId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.creatorId)
                .ForeignKey("dbo.Currencies", t => t.currencyId)
                .ForeignKey("dbo.tabDepartment", t => t.deptId)
                .ForeignKey("dbo.RentalContracts", t => t.rentalContractId)
                .ForeignKey("dbo.RentalOrderStatus", t => t.statusId)
                .ForeignKey("dbo.TenantRentals", t => t.TenantRentalId)
                .Index(t => t.rentalContractId)
                .Index(t => t.companyId)
                .Index(t => t.deptId)
                .Index(t => t.assetRentalId)
                .Index(t => t.TenantRentalId)
                .Index(t => t.creatorId)
                .Index(t => t.statusId)
                .Index(t => t.currencyId);
            
            CreateTable(
                "dbo.RentalContracts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreationDate = c.DateTime(),
                        companyId = c.Int(),
                        deptId = c.Int(),
                        assetRentalId = c.Int(),
                        tenantRentalId = c.Int(),
                        transactionGroupId = c.Int(nullable: false),
                        SystemRef = c.String(),
                        creatorId = c.Int(),
                        statusId = c.Int(),
                        currencyId = c.Int(),
                        fromDate = c.DateTime(),
                        toDate = c.DateTime(),
                        RentAmount = c.Double(nullable: false),
                        MER = c.Double(nullable: false),
                        RentAmountMER = c.Double(nullable: false),
                        NumberOfMonths = c.Double(nullable: false),
                        TotalRentAmount = c.Double(nullable: false),
                        rentalBasis = c.Int(nullable: false),
                        stage = c.String(),
                        isVoid = c.Boolean(nullable: false),
                        isReviewed = c.Boolean(),
                        needReview = c.Boolean(),
                        PendingForClosing = c.Boolean(),
                        PendingForReApproval = c.Boolean(),
                        isApproved = c.Boolean(),
                        ApprovedDate = c.DateTime(),
                        isReApproved = c.Boolean(),
                        ReApprovalDate = c.DateTime(),
                        ClosingDate = c.DateTime(),
                        LastStatusChangeDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tabCompany", t => t.companyId)
                .ForeignKey("dbo.Users", t => t.creatorId)
                .ForeignKey("dbo.Currencies", t => t.currencyId)
                .ForeignKey("dbo.tabDepartment", t => t.deptId)
                .ForeignKey("dbo.RentalContractStatus", t => t.statusId)
                .ForeignKey("dbo.TenantRentals", t => t.tenantRentalId)
                .ForeignKey("dbo.AssetRentals", t => t.assetRentalId)
                .Index(t => t.companyId)
                .Index(t => t.deptId)
                .Index(t => t.assetRentalId)
                .Index(t => t.tenantRentalId)
                .Index(t => t.creatorId)
                .Index(t => t.statusId)
                .Index(t => t.currencyId);
            
            CreateTable(
                "dbo.RentalContractStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.String(),
                        isApproved = c.Boolean(nullable: false),
                        isActive = c.Boolean(),
                        backcolor = c.String(),
                        forecolor = c.String(),
                        HierarchicalIndex = c.Int(nullable: false),
                        isDisable = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TenantRentals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TenantName = c.String(),
                        companyId = c.Int(),
                        deptId = c.Int(),
                        assetRentalId = c.Int(),
                        CreationDate = c.DateTime(),
                        contactId = c.Int(),
                        personId = c.Int(),
                        addressId = c.Int(),
                        statusId = c.Int(),
                        transactionGroupId = c.Int(nullable: false),
                        SystemRef = c.String(),
                        creatorId = c.Int(),
                        stage = c.String(),
                        isVoid = c.Boolean(nullable: false),
                        isReviewed = c.Boolean(),
                        needReview = c.Boolean(),
                        PendingForClosing = c.Boolean(),
                        PendingForReApproval = c.Boolean(),
                        isApproved = c.Boolean(),
                        ApprovedDate = c.DateTime(),
                        isReApproved = c.Boolean(),
                        ReApprovalDate = c.DateTime(),
                        ClosingDate = c.DateTime(),
                        LastStatusChangeDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tabAddress", t => t.addressId)
                .ForeignKey("dbo.AssetRentals", t => t.assetRentalId)
                .ForeignKey("dbo.tabCompany", t => t.companyId)
                .ForeignKey("dbo.tabContact", t => t.contactId)
                .ForeignKey("dbo.Users", t => t.creatorId)
                .ForeignKey("dbo.tabDepartment", t => t.deptId)
                .ForeignKey("dbo.tabPerson", t => t.personId)
                .ForeignKey("dbo.TenantRentalStatus", t => t.statusId)
                .Index(t => t.companyId)
                .Index(t => t.deptId)
                .Index(t => t.assetRentalId)
                .Index(t => t.contactId)
                .Index(t => t.personId)
                .Index(t => t.addressId)
                .Index(t => t.statusId)
                .Index(t => t.creatorId);
            
            CreateTable(
                "dbo.TenantRentalStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.String(),
                        isApproved = c.Boolean(nullable: false),
                        isActive = c.Boolean(),
                        backcolor = c.String(),
                        forecolor = c.String(),
                        HierarchicalIndex = c.Int(nullable: false),
                        isDisable = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RentalOrderStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.String(),
                        isApproved = c.Boolean(nullable: false),
                        isActive = c.Boolean(),
                        backcolor = c.String(),
                        forecolor = c.String(),
                        HierarchicalIndex = c.Int(nullable: false),
                        isDisable = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RentalInvoiceStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.String(),
                        isApproved = c.Boolean(nullable: false),
                        isActive = c.Boolean(),
                        backcolor = c.String(),
                        forecolor = c.String(),
                        HierarchicalIndex = c.Int(nullable: false),
                        isDisable = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SalesReceiptStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.String(),
                        isApproved = c.Boolean(nullable: false),
                        isActive = c.Boolean(),
                        backcolor = c.String(),
                        forecolor = c.String(),
                        HierarchicalIndex = c.Int(nullable: false),
                        isDisable = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PurchaseInvoiceStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.String(),
                        isApproved = c.Boolean(nullable: false),
                        isActive = c.Boolean(),
                        backcolor = c.String(),
                        forecolor = c.String(),
                        HierarchicalIndex = c.Int(nullable: false),
                        isDisable = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CreditCards",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        cardHolderType = c.Int(nullable: false),
                        CompanyId = c.Int(),
                        BankID = c.Int(),
                        PrimaryCardHolderId = c.Int(),
                        SecondaryCardHolderId = c.Int(),
                        CardUserID = c.Int(),
                        PrimaryCardNoId = c.Int(),
                        CardNumber = c.String(),
                        IssueDate = c.DateTime(nullable: false),
                        ExpiryDate = c.DateTime(nullable: false),
                        CVV = c.Int(nullable: false),
                        creditCardTypeId = c.Int(),
                        currencyId = c.Int(),
                        LimitAmount = c.Double(nullable: false),
                        isActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Banks", t => t.BankID)
                .ForeignKey("dbo.CardHolders", t => t.CardUserID)
                .ForeignKey("dbo.tabCompany", t => t.CompanyId)
                .ForeignKey("dbo.CreditCardTypes", t => t.creditCardTypeId)
                .ForeignKey("dbo.Currencies", t => t.currencyId)
                .ForeignKey("dbo.CardHolders", t => t.PrimaryCardHolderId)
                .ForeignKey("dbo.CreditCards", t => t.PrimaryCardNoId)
                .ForeignKey("dbo.CardHolders", t => t.SecondaryCardHolderId)
                .Index(t => t.CompanyId)
                .Index(t => t.BankID)
                .Index(t => t.PrimaryCardHolderId)
                .Index(t => t.SecondaryCardHolderId)
                .Index(t => t.CardUserID)
                .Index(t => t.PrimaryCardNoId)
                .Index(t => t.creditCardTypeId)
                .Index(t => t.currencyId);
            
            CreateTable(
                "dbo.CardHolders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        isActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CreditCardTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PaymentStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.String(),
                        isApproved = c.Boolean(nullable: false),
                        isActive = c.Boolean(),
                        backcolor = c.String(),
                        forecolor = c.String(),
                        HierarchicalIndex = c.Int(nullable: false),
                        isDisable = c.Boolean(nullable: false),
                        isPaid = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TargetRewards",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreationDate = c.DateTime(nullable: false),
                        statusId = c.Int(),
                        user_Id = c.Int(),
                        RewardAmount = c.Double(nullable: false),
                        toDoTask_Id = c.Int(),
                        currencyId = c.Int(),
                        targetRewardNatureId = c.Int(),
                        functionType = c.Int(nullable: false),
                        isApplied = c.Boolean(nullable: false),
                        AppliedDate = c.DateTime(),
                        stage = c.String(),
                        isVoid = c.Boolean(nullable: false),
                        isReviewed = c.Boolean(),
                        needReview = c.Boolean(),
                        PendingForClosing = c.Boolean(),
                        PendingForReApproval = c.Boolean(),
                        isApproved = c.Boolean(),
                        ApprovedDate = c.DateTime(),
                        isReApproved = c.Boolean(),
                        ReApprovalDate = c.DateTime(),
                        ClosingDate = c.DateTime(),
                        LastStatusChangeDate = c.DateTime(),
                        creator_Id = c.Int(),
                        financeRefNo = c.String(),
                        GlPostingDate = c.DateTime(),
                        MER = c.Double(nullable: false),
                        statusClass_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.creator_Id)
                .ForeignKey("dbo.Currencies", t => t.currencyId)
                .ForeignKey("dbo.TargetRewardStatus", t => t.statusId)
                .ForeignKey("dbo.StatusClasses", t => t.statusClass_Id)
                .ForeignKey("dbo.TargetRewardNatures", t => t.targetRewardNatureId)
                .ForeignKey("dbo.ToDoTasks", t => t.toDoTask_Id)
                .ForeignKey("dbo.Users", t => t.user_Id)
                .Index(t => t.statusId)
                .Index(t => t.user_Id)
                .Index(t => t.toDoTask_Id)
                .Index(t => t.currencyId)
                .Index(t => t.targetRewardNatureId)
                .Index(t => t.creator_Id)
                .Index(t => t.statusClass_Id);
            
            CreateTable(
                "dbo.TargetRewardStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.String(),
                        isApproved = c.Boolean(nullable: false),
                        isActive = c.Boolean(),
                        backcolor = c.String(),
                        forecolor = c.String(),
                        HierarchicalIndex = c.Int(nullable: false),
                        isDisable = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TargetRewardNatures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NatureName = c.String(),
                        isActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ToDoTasks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        creationDate = c.DateTime(nullable: false),
                        TaskCreatorId = c.Int(),
                        TaskName = c.String(),
                        TaskDescription = c.String(),
                        taskGroupId = c.Int(),
                        parentTaskId = c.Int(),
                        supervisedById = c.Int(),
                        salesHeadId = c.Int(),
                        targetGroupId = c.Int(),
                        targetTypeId = c.Int(),
                        statusId = c.Int(),
                        isImportant = c.Boolean(nullable: false),
                        isCompleted = c.Boolean(nullable: false),
                        StartDate = c.DateTime(),
                        TentativeClosingDate = c.DateTime(),
                        ActualClosingDate = c.DateTime(),
                        TaskPoints = c.Double(nullable: false),
                        AchievedPoints = c.Double(nullable: false),
                        SystemPoints = c.Double(nullable: false),
                        PercentageAchieved = c.Double(nullable: false),
                        stepCount = c.Int(nullable: false),
                        achievedStepsCount = c.Int(nullable: false),
                        PointsUpdatedOn = c.DateTime(),
                        StepDueDate = c.DateTime(),
                        stage = c.String(),
                        isVoid = c.Boolean(nullable: false),
                        isApproved = c.Boolean(),
                        ApprovedDate = c.DateTime(),
                        isReApproved = c.Boolean(),
                        ReApprovalDate = c.DateTime(),
                        SOField = c.String(),
                        searchedFromDate = c.DateTime(),
                        searchedToDate = c.DateTime(),
                        SOField1 = c.String(),
                        searchedFromDate1 = c.DateTime(),
                        searchedToDate1 = c.DateTime(),
                        SystemPoints1 = c.Double(nullable: false),
                        SOField2 = c.String(),
                        searchedFromDate2 = c.DateTime(),
                        searchedToDate2 = c.DateTime(),
                        SystemPoints2 = c.Double(nullable: false),
                        GroupCompanies = c.String(),
                        GroupDepartments = c.String(),
                        TargetYear = c.DateTime(),
                        TargetMonth = c.DateTime(),
                        isClosed = c.Boolean(nullable: false),
                        EstimatedGrossProfitSE = c.Double(nullable: false),
                        margin = c.Double(nullable: false),
                        ManualBudgetedMarginME = c.Double(nullable: false),
                        BudgetedMargininBase = c.Double(nullable: false),
                        ManualBudgetedMarginSE = c.Double(nullable: false),
                        SalesBudgetedMargin = c.Double(nullable: false),
                        RevisedMargin = c.Double(nullable: false),
                        RevisedMargininBase = c.Double(nullable: false),
                        SalesRevisedMargin = c.Double(nullable: false),
                        ActualMargin = c.Double(nullable: false),
                        ManualActualMarginME = c.Double(nullable: false),
                        ActualMargininBase = c.Double(nullable: false),
                        ManualActualMarginSE = c.Double(nullable: false),
                        SalesActualMargin = c.Double(nullable: false),
                        totalFOBValue = c.Double(nullable: false),
                        totalCFRValue = c.Double(nullable: false),
                        totalBaseCFRValue = c.Double(nullable: false),
                        SoAmountSER = c.Double(nullable: false),
                        SoAmountPER = c.Double(nullable: false),
                        ManualCommissionME = c.Double(nullable: false),
                        commisioninBase = c.Double(nullable: false),
                        commision = c.Double(nullable: false),
                        ManualCommissionSE = c.Double(nullable: false),
                        commisioninSE = c.Double(nullable: false),
                        systemMarginOC = c.Double(nullable: false),
                        ManualSystemMarginSE = c.Double(nullable: false),
                        systemMarginSE = c.Double(nullable: false),
                        ManualSystemMarginME = c.Double(nullable: false),
                        systemMarginME = c.Double(nullable: false),
                        netCommision = c.Double(nullable: false),
                        netCommisionSER = c.Double(nullable: false),
                        netCommisionMER = c.Double(nullable: false),
                        BMgrossProfitSE = c.Double(nullable: false),
                        BMgrossProfitME = c.Double(nullable: false),
                        TotalQuantity = c.Double(),
                        TargetAchievedPercenatage = c.Double(nullable: false),
                        TotalOrdersCount = c.Int(nullable: false),
                        UnderApprovalOrdersCount = c.Int(nullable: false),
                        ApprovedOrdersCount = c.Int(nullable: false),
                        UnderClosingOrdersCount = c.Int(nullable: false),
                        ClosedOrdersCount = c.Int(nullable: false),
                        CalculationType_Id = c.Int(),
                        statusClass_Id = c.Int(),
                        isBasketed = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.StatusCalculationTypes", t => t.CalculationType_Id)
                .ForeignKey("dbo.ToDoTasks", t => t.parentTaskId)
                .ForeignKey("dbo.Users", t => t.salesHeadId)
                .ForeignKey("dbo.ToDoTaskStatus", t => t.statusId)
                .ForeignKey("dbo.StatusClasses", t => t.statusClass_Id)
                .ForeignKey("dbo.Users", t => t.supervisedById)
                .ForeignKey("dbo.TargetGroups", t => t.targetGroupId)
                .ForeignKey("dbo.Users", t => t.TaskCreatorId)
                .ForeignKey("dbo.TaskGroups", t => t.taskGroupId)
                .ForeignKey("dbo.TaskTargetTypes", t => t.targetTypeId)
                .Index(t => t.TaskCreatorId)
                .Index(t => t.taskGroupId)
                .Index(t => t.parentTaskId)
                .Index(t => t.supervisedById)
                .Index(t => t.salesHeadId)
                .Index(t => t.targetGroupId)
                .Index(t => t.targetTypeId)
                .Index(t => t.statusId)
                .Index(t => t.CalculationType_Id)
                .Index(t => t.statusClass_Id);
            
            CreateTable(
                "dbo.ToDoTaskStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.String(),
                        isApproved = c.Boolean(nullable: false),
                        isActive = c.Boolean(),
                        backcolor = c.String(),
                        forecolor = c.String(),
                        HierarchicalIndex = c.Int(nullable: false),
                        forStep = c.Boolean(nullable: false),
                        forTask = c.Boolean(nullable: false),
                        MinPercentage = c.Double(nullable: false),
                        MaxPercentage = c.Double(nullable: false),
                        isDisable = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TaskTargetTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TargetTypeName = c.String(),
                        isActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.LoansAdvanceStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.String(),
                        isApproved = c.Boolean(nullable: false),
                        isActive = c.Boolean(),
                        backcolor = c.String(),
                        forecolor = c.String(),
                        HierarchicalIndex = c.Int(nullable: false),
                        isDisable = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PurchaseOrderStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.String(),
                        isApproved = c.Boolean(nullable: false),
                        isActive = c.Boolean(),
                        backcolor = c.String(),
                        forecolor = c.String(),
                        HierarchicalIndex = c.Int(nullable: false),
                        isDisable = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ModuleContractStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.String(),
                        isApproved = c.Boolean(nullable: false),
                        isActive = c.Boolean(),
                        backcolor = c.String(),
                        forecolor = c.String(),
                        isDisable = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PassOns",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        passOnName = c.String(),
                        isActive = c.Boolean(nullable: false),
                        chartofAccountId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ChartofAccounts", t => t.chartofAccountId)
                .Index(t => t.chartofAccountId);
            
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
                "dbo.LoansStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.String(),
                        isApproved = c.Boolean(nullable: false),
                        isActive = c.Boolean(),
                        backcolor = c.String(),
                        forecolor = c.String(),
                        HierarchicalIndex = c.Int(nullable: false),
                        isDisable = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.tabLoan",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreationDate = c.DateTime(nullable: false),
                        LimitDate = c.DateTime(),
                        ExpiryDate = c.DateTime(),
                        ExtensionDate = c.DateTime(),
                        CompanyId = c.Int(),
                        deptId = c.Int(),
                        bankId = c.Int(),
                        accountId = c.Int(),
                        currencyId = c.Int(),
                        MainLimitNatureId = c.Int(),
                        SubLimitNatureId = c.Int(),
                        statusId = c.Int(),
                        creatorId = c.Int(),
                        MainLimitAmount = c.Double(nullable: false),
                        SubLimitAmount = c.Double(nullable: false),
                        transactionGroupId = c.Int(nullable: false),
                        SystemRefNo = c.String(),
                        FinanceRefNo = c.String(),
                        stage = c.String(),
                        isVoid = c.Boolean(nullable: false),
                        isReviewed = c.Boolean(),
                        needReview = c.Boolean(),
                        PendingForClosing = c.Boolean(),
                        PendingForReApproval = c.Boolean(),
                        isApproved = c.Boolean(),
                        ApprovedDate = c.DateTime(),
                        isReApproved = c.Boolean(),
                        ReApprovalDate = c.DateTime(),
                        user_Id = c.Int(),
                        ClosingDate = c.DateTime(),
                        LastStatusChangeDate = c.DateTime(),
                        statusClass_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Accounts", t => t.accountId)
                .ForeignKey("dbo.Banks", t => t.bankId)
                .ForeignKey("dbo.tabCompany", t => t.CompanyId)
                .ForeignKey("dbo.Employees", t => t.creatorId)
                .ForeignKey("dbo.Currencies", t => t.currencyId)
                .ForeignKey("dbo.tabDepartment", t => t.deptId)
                .ForeignKey("dbo.FacilityNatures", t => t.MainLimitNatureId)
                .ForeignKey("dbo.LoansStatus", t => t.statusId)
                .ForeignKey("dbo.StatusClasses", t => t.statusClass_Id)
                .ForeignKey("dbo.FacilityNatures", t => t.SubLimitNatureId)
                .ForeignKey("dbo.Users", t => t.user_Id)
                .Index(t => t.CompanyId)
                .Index(t => t.deptId)
                .Index(t => t.bankId)
                .Index(t => t.accountId)
                .Index(t => t.currencyId)
                .Index(t => t.MainLimitNatureId)
                .Index(t => t.SubLimitNatureId)
                .Index(t => t.statusId)
                .Index(t => t.creatorId)
                .Index(t => t.user_Id)
                .Index(t => t.statusClass_Id);
            
            CreateTable(
                "dbo.FacilityNatures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NatureName = c.String(),
                        isActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TasksStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.String(),
                        isApproved = c.Boolean(nullable: false),
                        isActive = c.Boolean(),
                        backcolor = c.String(),
                        forecolor = c.String(),
                        HierarchicalIndex = c.Int(nullable: false),
                        isDisable = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TaskTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TypeName = c.String(),
                        isProcurementType = c.Boolean(nullable: false),
                        isSaleOrder = c.Boolean(nullable: false),
                        isPurchaseOrder = c.Boolean(nullable: false),
                        isSaleInvoice = c.Boolean(nullable: false),
                        isOffer = c.Boolean(nullable: false),
                        isInquiry = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Reconcilations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        chartofAccountId = c.Int(),
                        reconcilationDate = c.DateTime(nullable: false),
                        reconcilationAmount = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ChartofAccounts", t => t.chartofAccountId)
                .Index(t => t.chartofAccountId);
            
            CreateTable(
                "dbo.PackingStyles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TaskComments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        goodReceiveNoteId = c.Int(),
                        userId = c.Int(),
                        taskId = c.Int(),
                        Comment = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GoodReceiveNotes", t => t.goodReceiveNoteId)
                .ForeignKey("dbo.Tasks", t => t.taskId)
                .ForeignKey("dbo.Users", t => t.userId)
                .Index(t => t.goodReceiveNoteId)
                .Index(t => t.userId)
                .Index(t => t.taskId);
            
            CreateTable(
                "dbo.GoodReceiveNotes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TaskEfficiencies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        efficiencyPoints_Id = c.Int(),
                        tasksId = c.Int(),
                        TotalPoints = c.Double(nullable: false),
                        AchievedPoints = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EfficiencyPoints", t => t.efficiencyPoints_Id)
                .ForeignKey("dbo.Tasks", t => t.tasksId)
                .Index(t => t.efficiencyPoints_Id)
                .Index(t => t.tasksId);
            
            CreateTable(
                "dbo.EfficiencyPoints",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        title = c.String(),
                        isActive = c.Boolean(nullable: false),
                        Points = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TaskTrackings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        tasksId = c.Int(),
                        UpdateDateTime = c.DateTime(nullable: false),
                        UpdatedById = c.Int(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tasks", t => t.tasksId)
                .ForeignKey("dbo.Users", t => t.UpdatedById)
                .Index(t => t.tasksId)
                .Index(t => t.UpdatedById);
            
            CreateTable(
                "dbo.Warehouses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        WarehouseName = c.String(),
                        isActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Reports",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ReportName = c.String(nullable: false, maxLength: 8000, unicode: false),
                        ReportDesign = c.String(unicode: false),
                        Type = c.String(),
                        lastModified = c.DateTime(nullable: false),
                        groupId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ReportGroups", t => t.groupId)
                .Index(t => t.ReportName, unique: true)
                .Index(t => t.groupId);
            
            CreateTable(
                "dbo.ReportGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        group = c.String(),
                        parentId = c.Int(),
                        isActive = c.Boolean(nullable: false),
                        user_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ReportGroups", t => t.parentId)
                .ForeignKey("dbo.Users", t => t.user_Id)
                .Index(t => t.parentId)
                .Index(t => t.user_Id);
            
            CreateTable(
                "dbo.UserSettings",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        userId = c.Int(nullable: false),
                        settingkey = c.String(nullable: false, maxLength: 128, unicode: false),
                        settingValue = c.String(unicode: false),
                        lastModified = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.id, t.userId, t.settingkey })
                .ForeignKey("dbo.Users", t => t.userId, cascadeDelete: true)
                .Index(t => t.userId);
            
            CreateTable(
                "dbo.VehicleExpenses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        transactionGroupId = c.Int(nullable: false),
                        SystemRefNo = c.String(),
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
                        AdminBillForVehicleExpenses_Id = c.Int(),
                        AdminBillForFuelExpenses_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tabAdminBill", t => t.AdminBillForFuelExpenses_Id)
                .ForeignKey("dbo.tabAdminBill", t => t.AdminBillForVehicleExpenses_Id)
                .ForeignKey("dbo.MaintenanceHeads", t => t.maintenanceHeadId)
                .ForeignKey("dbo.Payees", t => t.payeeId)
                .Index(t => t.payeeId)
                .Index(t => t.maintenanceHeadId)
                .Index(t => t.AdminBillForVehicleExpenses_Id)
                .Index(t => t.AdminBillForFuelExpenses_Id);
            
            CreateTable(
                "dbo.MaintenanceHeads",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        HeadName = c.String(),
                        isActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ManagementSummaries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SummaryName = c.String(),
                        isActive = c.Boolean(nullable: false),
                        ParentId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ManagementSummaries", t => t.ParentId)
                .Index(t => t.ParentId);
            
            CreateTable(
                "dbo.Templates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        transactionType = c.Int(nullable: false),
                        Coa_AccountType = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BillCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Category = c.String(),
                        isActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BillItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AmountSOC = c.Double(nullable: false),
                        fieldId = c.Int(),
                        creditAccountId = c.Int(),
                        debitAccountId = c.Int(),
                        BillAmount = c.Double(nullable: false),
                        Bill_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CostSheetFields", t => t.fieldId)
                .ForeignKey("dbo.ChartofAccounts", t => t.creditAccountId)
                .ForeignKey("dbo.ChartofAccounts", t => t.debitAccountId)
                .ForeignKey("dbo.Bills", t => t.Bill_Id)
                .Index(t => t.fieldId)
                .Index(t => t.creditAccountId)
                .Index(t => t.debitAccountId)
                .Index(t => t.Bill_Id);
            
            CreateTable(
                "dbo.BillTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        billType = c.String(),
                        isActive = c.Boolean(nullable: false),
                        user_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.user_Id)
                .Index(t => t.user_Id);
            
            CreateTable(
                "dbo.VendorBillNatures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nature = c.String(),
                        isActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VendorNatures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        isApproved = c.Boolean(nullable: false),
                        isActive = c.Boolean(nullable: false),
                        user_Id = c.Int(),
                        addedDate = c.DateTime(nullable: false),
                        isVoid = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.user_Id)
                .Index(t => t.user_Id);
            
            CreateTable(
                "dbo.VendorNatureManuals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        isApproved = c.Boolean(nullable: false),
                        isActive = c.Boolean(nullable: false),
                        user_Id = c.Int(),
                        addedDate = c.DateTime(nullable: false),
                        isVoid = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.user_Id)
                .Index(t => t.user_Id);
            
            CreateTable(
                "dbo.ChartofAccountGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        referenceNo = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CashFlows",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        reportName = c.String(),
                        userId = c.Int(),
                        groupId = c.Int(),
                        saleOrderSettingKey = c.String(unicode: false),
                        purchaseOrderSettingKey = c.String(unicode: false),
                        saleinvoiceSettingKey = c.String(unicode: false),
                        vendorBillSettingKey = c.String(unicode: false),
                        adminBillSettingKey = c.String(unicode: false),
                        stlSettingKey = c.String(unicode: false),
                        paymentSettingKey = c.String(unicode: false),
                        bankSettingKey = c.String(unicode: false),
                        loanSettingKey = c.String(unicode: false),
                        advanceSettingKey = c.String(unicode: false),
                        gridReportType = c.Int(nullable: false),
                        titleId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GridReportGroups", t => t.groupId)
                .ForeignKey("dbo.ReportTitles", t => t.titleId)
                .ForeignKey("dbo.Users", t => t.userId)
                .Index(t => t.userId)
                .Index(t => t.groupId)
                .Index(t => t.titleId);
            
            CreateTable(
                "dbo.GridReportGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        groupName = c.String(),
                        isActive = c.Boolean(nullable: false),
                        gridReportType = c.Int(nullable: false),
                        parentId = c.Int(),
                        userId = c.Int(),
                        transactionType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GridReportGroups", t => t.parentId)
                .ForeignKey("dbo.Users", t => t.userId)
                .Index(t => t.parentId)
                .Index(t => t.userId);
            
            CreateTable(
                "dbo.GridReports",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        settingkey = c.String(nullable: false, maxLength: 128, unicode: false),
                        userId = c.Int(),
                        group_Id = c.Int(),
                        settingValue = c.String(unicode: false),
                        lastModified = c.DateTime(nullable: false),
                        gridReportType = c.Int(nullable: false),
                        reportName = c.String(),
                        isFavourite = c.Boolean(nullable: false),
                        from = c.DateTime(),
                        to = c.DateTime(),
                        company_Id = c.Int(),
                        titleId = c.Int(),
                    })
                .PrimaryKey(t => new { t.Id, t.settingkey })
                .ForeignKey("dbo.tabCompany", t => t.company_Id)
                .ForeignKey("dbo.Users", t => t.userId)
                .ForeignKey("dbo.GridReportGroups", t => t.group_Id)
                .ForeignKey("dbo.ReportTitles", t => t.titleId)
                .Index(t => t.userId)
                .Index(t => t.group_Id)
                .Index(t => t.company_Id)
                .Index(t => t.titleId);
            
            CreateTable(
                "dbo.ReportTitles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        titleName = c.String(),
                        isActive = c.Boolean(nullable: false),
                        gridReportType = c.Int(nullable: false),
                        userId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.userId)
                .Index(t => t.userId);
            
            CreateTable(
                "dbo.DepartmentLevels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        ParentID = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DepartmentLevels", t => t.ParentID)
                .Index(t => t.ParentID);
            
            CreateTable(
                "dbo.SharedGridGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        groupName = c.String(),
                        parentId = c.Int(),
                        isVoid = c.Boolean(nullable: false),
                        userId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SharedGridGroups", t => t.parentId)
                .ForeignKey("dbo.Users", t => t.userId)
                .Index(t => t.parentId)
                .Index(t => t.userId);
            
            CreateTable(
                "dbo.SharedReports",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        settingkey = c.String(nullable: false, maxLength: 128, unicode: false),
                        userId = c.Int(),
                        settingValue = c.String(unicode: false),
                        lastModified = c.DateTime(nullable: false),
                        reportName = c.String(),
                        sharedGroupId = c.Int(),
                    })
                .PrimaryKey(t => new { t.Id, t.settingkey })
                .ForeignKey("dbo.Users", t => t.userId)
                .ForeignKey("dbo.SharedGridGroups", t => t.sharedGroupId)
                .Index(t => t.userId)
                .Index(t => t.sharedGroupId);
            
            CreateTable(
                "dbo.Targets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        Code = c.String(),
                        Year = c.Int(nullable: false),
                        typeId = c.Int(nullable: false),
                        currencyId = c.Int(nullable: false),
                        AddedDate = c.DateTime(),
                        AchivedDate = c.DateTime(),
                        EditDate = c.DateTime(),
                        isAchived = c.Boolean(nullable: false),
                        isActive = c.Boolean(nullable: false),
                        companyId = c.Int(),
                        departmentId = c.Int(),
                        user_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tabCompany", t => t.companyId)
                .ForeignKey("dbo.Currencies", t => t.currencyId, cascadeDelete: true)
                .ForeignKey("dbo.tabDepartment", t => t.departmentId)
                .ForeignKey("dbo.TargetTypes", t => t.typeId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.user_Id)
                .Index(t => t.typeId)
                .Index(t => t.currencyId)
                .Index(t => t.companyId)
                .Index(t => t.departmentId)
                .Index(t => t.user_Id);
            
            CreateTable(
                "dbo.TargetAwards",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        target = c.Double(nullable: false),
                        IndviualAward = c.Double(nullable: false),
                        NoOfEmployees = c.Int(nullable: false),
                        TotalAward = c.Double(nullable: false),
                        Month = c.Int(nullable: false),
                        targetId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Targets", t => t.targetId)
                .Index(t => t.targetId);
            
            CreateTable(
                "dbo.TargetTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        HierarchicalIndex = c.Int(nullable: false),
                        Type = c.String(),
                        isActive = c.Boolean(nullable: false),
                        user_Id = c.Int(),
                        Frequency = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.user_Id)
                .Index(t => t.user_Id);
            
            CreateTable(
                "dbo.PurchaseInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        acquireAt = c.DateTime(nullable: false),
                        FA_Amount = c.Double(nullable: false),
                        PER = c.Double(nullable: false),
                        FA_Amount_PER = c.Double(nullable: false),
                        currecncy_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Currencies", t => t.currecncy_Id)
                .Index(t => t.currecncy_Id);
            
            CreateTable(
                "dbo.Revaluations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        assetId = c.Int(nullable: false),
                        revaluationDate = c.DateTime(nullable: false),
                        amountMR = c.Double(nullable: false),
                        MER = c.Double(nullable: false),
                        amountMER = c.Double(nullable: false),
                        transGroupID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Assets", t => t.assetId, cascadeDelete: true)
                .Index(t => t.assetId);
            
            CreateTable(
                "dbo.TenancyContracts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreationDate = c.DateTime(),
                        ContractDateFrom = c.DateTime(),
                        ContractDateTo = c.DateTime(),
                        TenancyContractDate = c.DateTime(),
                        ContractReferenceNo = c.String(),
                        RentalAmount = c.Double(nullable: false),
                        rentalBasis = c.Int(nullable: false),
                        AssetType = c.Int(nullable: false),
                        isActive = c.Boolean(nullable: false),
                        UnitNames = c.String(),
                        AssetId = c.Int(),
                        NotOwnedAssetId = c.Int(),
                        OwnerId = c.Int(),
                        TenantId = c.Int(),
                        companyId = c.Int(),
                        deptId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RentalAssets", t => t.AssetId)
                .ForeignKey("dbo.tabCompany", t => t.companyId)
                .ForeignKey("dbo.tabDepartment", t => t.deptId)
                .ForeignKey("dbo.RentalAssets", t => t.NotOwnedAssetId)
                .ForeignKey("dbo.AssetOwners", t => t.OwnerId)
                .ForeignKey("dbo.Tenants", t => t.TenantId)
                .Index(t => t.AssetId)
                .Index(t => t.NotOwnedAssetId)
                .Index(t => t.OwnerId)
                .Index(t => t.TenantId)
                .Index(t => t.companyId)
                .Index(t => t.deptId);
            
            CreateTable(
                "dbo.RentalAssets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreationDate = c.DateTime(),
                        AssetType = c.Int(nullable: false),
                        AssetNatureId = c.Int(),
                        UnitNames = c.String(),
                        NotOwnedUnitNames = c.String(),
                        assetId = c.Int(),
                        companyId = c.Int(),
                        deptId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Assets", t => t.assetId)
                .ForeignKey("dbo.AssetNatures", t => t.AssetNatureId)
                .ForeignKey("dbo.tabCompany", t => t.companyId)
                .ForeignKey("dbo.tabDepartment", t => t.deptId)
                .Index(t => t.AssetNatureId)
                .Index(t => t.assetId)
                .Index(t => t.companyId)
                .Index(t => t.deptId);
            
            CreateTable(
                "dbo.Tenants",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Profession = c.String(),
                        CreationDate = c.DateTime(),
                        isActive = c.Boolean(nullable: false),
                        companyId = c.Int(),
                        deptId = c.Int(),
                        address_Id = c.Int(),
                        contact_Id = c.Int(),
                        person_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tabAddress", t => t.address_Id)
                .ForeignKey("dbo.tabCompany", t => t.companyId)
                .ForeignKey("dbo.tabContact", t => t.contact_Id)
                .ForeignKey("dbo.tabDepartment", t => t.deptId)
                .ForeignKey("dbo.tabPerson", t => t.person_Id)
                .Index(t => t.companyId)
                .Index(t => t.deptId)
                .Index(t => t.address_Id)
                .Index(t => t.contact_Id)
                .Index(t => t.person_Id);
            
            CreateTable(
                "dbo.Emergencyontacts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Relation = c.String(),
                        contact = c.String(),
                        Address = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Functions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        IsActive = c.Boolean(),
                        functionType = c.Int(),
                        company_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tabCompany", t => t.company_Id)
                .Index(t => t.company_Id);
            
            CreateTable(
                "dbo.EmployeeApprovals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        stage = c.String(),
                        isVoid = c.Boolean(nullable: false),
                        isReviewed = c.Boolean(),
                        needReview = c.Boolean(),
                        PendingForClosing = c.Boolean(),
                        PendingForReApproval = c.Boolean(),
                        isApproved = c.Boolean(),
                        ApprovedDate = c.DateTime(),
                        isReApproved = c.Boolean(),
                        ReApprovalDate = c.DateTime(),
                        ClosingDate = c.DateTime(),
                        LastStatusChangeDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.EmployeeWorkingStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.String(),
                        isApproved = c.Boolean(nullable: false),
                        isActive = c.Boolean(),
                        backcolor = c.String(),
                        forecolor = c.String(),
                        HierarchicalIndex = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.EmployeeHRInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.LeaveApplications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ApplyDate = c.DateTime(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        leaveId = c.Int(),
                        employeeId = c.Int(),
                        currentStatus = c.Int(nullable: false),
                        isHalf = c.Boolean(nullable: false),
                        LeaveDes = c.String(),
                        stage = c.Int(nullable: false),
                        isVoid = c.Boolean(nullable: false),
                        isReviewed = c.Boolean(),
                        needReview = c.Boolean(),
                        PendingForClosing = c.Boolean(),
                        PendingForReApproval = c.Boolean(),
                        isApproved = c.Boolean(),
                        ApprovedDate = c.DateTime(),
                        isReApproved = c.Boolean(),
                        ReApprovalDate = c.DateTime(),
                        ClosingDate = c.DateTime(),
                        LastStatusChangeDate = c.DateTime(),
                        approver_EmpId = c.Int(),
                        HRinfo_Id = c.Int(),
                        initiator_EmpId = c.Int(),
                        leaveStatus_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.approver_EmpId)
                .ForeignKey("dbo.Employees", t => t.employeeId)
                .ForeignKey("dbo.EmployeeHRInfoes", t => t.HRinfo_Id)
                .ForeignKey("dbo.Employees", t => t.initiator_EmpId)
                .ForeignKey("dbo.Leaves", t => t.leaveId)
                .ForeignKey("dbo.LeaveStatus", t => t.leaveStatus_Id)
                .Index(t => t.leaveId)
                .Index(t => t.employeeId)
                .Index(t => t.approver_EmpId)
                .Index(t => t.HRinfo_Id)
                .Index(t => t.initiator_EmpId)
                .Index(t => t.leaveStatus_Id);
            
            CreateTable(
                "dbo.Leaves",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LeaveType = c.Int(nullable: false),
                        LeaveDes = c.String(),
                        LeaveDays = c.Double(nullable: false),
                        LeaveHours = c.Double(nullable: false),
                        daysCarried = c.Double(nullable: false),
                        DateFrom = c.DateTime(nullable: false),
                        DateTo = c.DateTime(nullable: false),
                        ApplyDate = c.DateTime(),
                        employeeId = c.Int(),
                        isApproved = c.Boolean(),
                        LeaveDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.employeeId)
                .Index(t => t.employeeId);
            
            CreateTable(
                "dbo.LeaveStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.String(),
                        isApproved = c.Boolean(nullable: false),
                        isActive = c.Boolean(),
                        backcolor = c.String(),
                        forecolor = c.String(),
                        HierarchicalIndex = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Qualifications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DegreeType = c.String(),
                        DegreeTitle = c.String(),
                        Specialization = c.String(),
                        MarksObtained = c.Double(nullable: false),
                        MarksTotal = c.Double(nullable: false),
                        MarksPercentage = c.Double(nullable: false),
                        Division = c.String(),
                        StartYear = c.DateTime(),
                        PassingYear = c.DateTime(),
                        Institute = c.String(),
                        Location = c.String(),
                        IsLatest = c.Boolean(nullable: false),
                        IsDistinction = c.Boolean(nullable: false),
                        DistDetails = c.String(),
                        IsValid = c.Boolean(nullable: false),
                        validTill = c.DateTime(),
                        Score = c.String(),
                        IsCompleted = c.Boolean(nullable: false),
                        employeeId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.employeeId)
                .Index(t => t.employeeId);
            
            CreateTable(
                "dbo.SalesTargets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StartingDate = c.DateTime(),
                        EndDate = c.DateTime(),
                        TargetAmount = c.Double(nullable: false),
                        CurrencyId = c.Int(),
                        isAchieved = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Currencies", t => t.CurrencyId)
                .Index(t => t.CurrencyId);
            
            CreateTable(
                "dbo.EmployeeWorkExperiences",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Company = c.String(),
                        JobDescription = c.String(),
                        JobTitle = c.String(),
                        DateFrom = c.DateTime(),
                        DateTo = c.DateTime(),
                        employerAddress = c.String(),
                        employerContact = c.String(),
                        employeeId = c.Int(),
                        isLatest = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.employeeId)
                .Index(t => t.employeeId);
            
            CreateTable(
                "dbo.MainBanks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BankName = c.String(),
                        isActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Airlines",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Allowances",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Areas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Length = c.Double(nullable: false),
                        width = c.Double(nullable: false),
                        measureUnitType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AttachmentCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ParentId = c.Int(),
                        lastChangedDate = c.DateTime(nullable: false),
                        additionDate = c.DateTime(nullable: false),
                        description = c.String(),
                        thumbnail = c.Binary(),
                        isActive = c.Boolean(nullable: false),
                        userId = c.Int(nullable: false),
                        Inquiry = c.Int(),
                        Offer = c.Int(),
                        ModuleContract = c.Int(),
                        PO = c.Int(),
                        SO = c.Int(),
                        SI = c.Int(),
                        SR = c.Int(),
                        PI = c.Int(),
                        Payment = c.Int(),
                        VBill = c.Int(),
                        ABill = c.Int(),
                        IBT = c.Int(),
                        ICBT = c.Int(),
                        MS = c.Int(),
                        LoansAdvances = c.Int(),
                        Tasks = c.Int(),
                        TargetReward = c.Int(),
                        TravelingRecord = c.Int(),
                        STL = c.Int(),
                        ProcurementProducts = c.Int(),
                        VehicleExpenses = c.Int(),
                        RentalContract = c.Int(),
                        RentalOrder = c.Int(),
                        RentalInvoice = c.Int(),
                        Memo = c.Int(),
                        Document = c.Int(),
                        AssetRental = c.Int(),
                        TenantRental = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AttachmentCategories", t => t.ParentId)
                .ForeignKey("dbo.Users", t => t.userId, cascadeDelete: true)
                .Index(t => t.ParentId)
                .Index(t => t.userId);
            
            CreateTable(
                "dbo.Attachments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        fileServerAdress = c.String(),
                        fileLocalAdress = c.String(),
                        fileName = c.String(),
                        fileType = c.Int(nullable: false),
                        lastOpendate = c.DateTime(nullable: false),
                        additionDate = c.DateTime(nullable: false),
                        comment = c.String(),
                        currentStatus = c.Int(nullable: false),
                        thumbnail = c.Binary(),
                        isactive = c.Boolean(nullable: false),
                        transactionType = c.Int(nullable: false),
                        transactionId = c.Int(nullable: false),
                        userId = c.Int(nullable: false),
                        categoryId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AttachmentCategories", t => t.categoryId)
                .ForeignKey("dbo.Users", t => t.userId, cascadeDelete: true)
                .Index(t => t.userId)
                .Index(t => t.categoryId);
            
            CreateTable(
                "dbo.AuthDocs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DocName = c.String(),
                        isMust = c.Boolean(nullable: false),
                        palcedAt = c.String(),
                        isAttached = c.Boolean(nullable: false),
                        uplaodLocation = c.String(),
                        authId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.OfficialAuths", t => t.authId, cascadeDelete: true)
                .Index(t => t.authId);
            
            CreateTable(
                "dbo.OfficialAuths",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AuthName = c.String(),
                        isActive = c.Boolean(nullable: false),
                        officialAuthtype = c.Int(nullable: false),
                        region_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Regions", t => t.region_Id)
                .Index(t => t.region_Id);
            
            CreateTable(
                "dbo.Regions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        City = c.String(),
                        Country = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.tabBackground",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Path = c.String(),
                        EmployeeIds = c.String(),
                        PopupText = c.String(),
                        isSharedAll = c.Boolean(nullable: false),
                        isUpdate = c.Boolean(nullable: false),
                        userId = c.Int(),
                        UploadedTime = c.DateTime(),
                        taskGroupId = c.Int(),
                        isGroup = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TaskGroups", t => t.taskGroupId)
                .ForeignKey("dbo.Users", t => t.userId)
                .Index(t => t.userId)
                .Index(t => t.taskGroupId);
            
            CreateTable(
                "dbo.Bonus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Buildings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Floors = c.Int(nullable: false),
                        isMortgaged = c.Boolean(nullable: false),
                        mortgagedValue = c.Double(nullable: false),
                        asset_Id = c.Int(),
                        measureUnit_Id = c.Int(),
                        mortgagee_Id = c.Int(),
                        OfficialAuths_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Assets", t => t.asset_Id)
                .ForeignKey("dbo.Areas", t => t.measureUnit_Id)
                .ForeignKey("dbo.Mortgagees", t => t.mortgagee_Id)
                .ForeignKey("dbo.OfficialAuths", t => t.OfficialAuths_Id)
                .Index(t => t.asset_Id)
                .Index(t => t.measureUnit_Id)
                .Index(t => t.mortgagee_Id)
                .Index(t => t.OfficialAuths_Id);
            
            CreateTable(
                "dbo.Mortgagees",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        isActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ConditionImages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        vehicleId = c.Int(nullable: false),
                        ImageName = c.String(),
                        location = c.String(),
                        uploadDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Vehicles", t => t.vehicleId, cascadeDelete: true)
                .Index(t => t.vehicleId);
            
            CreateTable(
                "dbo.Vehicles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        assetId = c.Int(nullable: false),
                        Model = c.Int(nullable: false),
                        Chesis = c.String(),
                        EngineNo = c.String(),
                        RegNo = c.String(),
                        isNew = c.Boolean(nullable: false),
                        EngineReading = c.Double(nullable: false),
                        LifeTimeToken = c.Boolean(nullable: false),
                        tokenValidTill = c.DateTime(nullable: false),
                        isLeased = c.Boolean(nullable: false),
                        lesseId = c.Int(),
                        leasingValue = c.Double(nullable: false),
                        BuyingStatus_Id = c.Int(),
                        currentStatus_Id = c.Int(),
                        manufacturer_Id = c.Int(),
                        OfficialAuths_Id = c.Int(),
                        vehicleType_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Assets", t => t.assetId, cascadeDelete: true)
                .ForeignKey("dbo.VehicleBuyingStatus", t => t.BuyingStatus_Id)
                .ForeignKey("dbo.VehicleCurrentStatus", t => t.currentStatus_Id)
                .ForeignKey("dbo.Lesees", t => t.lesseId)
                .ForeignKey("dbo.Manufacturers", t => t.manufacturer_Id)
                .ForeignKey("dbo.OfficialAuths", t => t.OfficialAuths_Id)
                .ForeignKey("dbo.VehicleTypes", t => t.vehicleType_Id)
                .Index(t => t.assetId)
                .Index(t => t.lesseId)
                .Index(t => t.BuyingStatus_Id)
                .Index(t => t.currentStatus_Id)
                .Index(t => t.manufacturer_Id)
                .Index(t => t.OfficialAuths_Id)
                .Index(t => t.vehicleType_Id);
            
            CreateTable(
                "dbo.VehicleBuyingStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        buyingStatus = c.String(),
                        isNew = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VehicleCurrentStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        currentStatusName = c.String(),
                        isActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Lesees",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LesseName = c.String(),
                        isActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Manufacturers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ManufacturerName = c.String(),
                        isActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VehicleTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TypeName = c.String(),
                        isActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CustomReportGeoups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreationDate = c.DateTime(),
                        LastUpdatedDate = c.DateTime(),
                        Title = c.String(),
                        creatorId = c.Int(),
                        updatorId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.creatorId)
                .ForeignKey("dbo.Users", t => t.updatorId)
                .Index(t => t.creatorId)
                .Index(t => t.updatorId);
            
            CreateTable(
                "dbo.CustomReports",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateFrom = c.DateTime(),
                        DateTo = c.DateTime(),
                        transactionItemType = c.Int(nullable: false),
                        customReportField = c.Int(nullable: false),
                        accountsType = c.Int(nullable: false),
                        companyId = c.Int(),
                        deptId = c.Int(),
                        chartOfAccountId = c.Int(),
                        CustomReportGeoup_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ChartofAccounts", t => t.chartOfAccountId)
                .ForeignKey("dbo.tabCompany", t => t.companyId)
                .ForeignKey("dbo.tabDepartment", t => t.deptId)
                .ForeignKey("dbo.CustomReportGeoups", t => t.CustomReportGeoup_Id)
                .Index(t => t.companyId)
                .Index(t => t.deptId)
                .Index(t => t.chartOfAccountId)
                .Index(t => t.CustomReportGeoup_Id);
            
            CreateTable(
                "dbo.EmployeeCoaCompanies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        compId = c.Int(),
                        EmpId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tabCompany", t => t.compId)
                .ForeignKey("dbo.Employees", t => t.EmpId)
                .Index(t => t.compId)
                .Index(t => t.EmpId);
            
            CreateTable(
                "dbo.EmployeePerformanceReviews",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreationDate = c.DateTime(),
                        PerformanceYear = c.DateTime(),
                        EmployeeId = c.Int(),
                        DeptId = c.Int(),
                        SupervisorLevelOneId = c.Int(),
                        SupervisorLevelOneReviewDate = c.DateTime(),
                        SupervisorLevelTwoId = c.Int(),
                        SupervisorLevelTwoReviewDate = c.DateTime(),
                        AdminId = c.Int(),
                        ManagementId = c.Int(),
                        AdminReviewDate = c.DateTime(),
                        MemoId = c.Int(),
                        PreviousYearGoals = c.String(),
                        NextYearGoals = c.String(),
                        performanceReviewerStage = c.Int(nullable: false),
                        performanceReviewType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.AdminId)
                .ForeignKey("dbo.tabDepartment", t => t.DeptId)
                .ForeignKey("dbo.Users", t => t.EmployeeId)
                .ForeignKey("dbo.Users", t => t.ManagementId)
                .ForeignKey("dbo.Memos", t => t.MemoId)
                .ForeignKey("dbo.Users", t => t.SupervisorLevelOneId)
                .ForeignKey("dbo.Users", t => t.SupervisorLevelTwoId)
                .Index(t => t.EmployeeId)
                .Index(t => t.DeptId)
                .Index(t => t.SupervisorLevelOneId)
                .Index(t => t.SupervisorLevelTwoId)
                .Index(t => t.AdminId)
                .Index(t => t.ManagementId)
                .Index(t => t.MemoId);
            
            CreateTable(
                "dbo.PerformanceIndicatorRatings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ReviewId = c.Int(nullable: false),
                        IndicatorDefinitionId = c.Int(nullable: false),
                        SelfRating = c.Int(nullable: false),
                        SupervisorLevelOneRating = c.Int(nullable: false),
                        SupervisorLevelTwoRating = c.Int(nullable: false),
                        AdminRating = c.Int(nullable: false),
                        ManagementRating = c.Int(nullable: false),
                        Comment = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PerformanceIndicatorDefinitions", t => t.IndicatorDefinitionId, cascadeDelete: true)
                .ForeignKey("dbo.EmployeePerformanceReviews", t => t.ReviewId, cascadeDelete: true)
                .Index(t => t.ReviewId)
                .Index(t => t.IndicatorDefinitionId);
            
            CreateTable(
                "dbo.PerformanceIndicatorDefinitions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        DisplayOrder = c.Int(nullable: false),
                        Weightage = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        IsAdminType = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.EmploymentSalaries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreationDate = c.DateTime(),
                        transactionGroupId = c.Int(nullable: false),
                        SystemRef = c.String(),
                        creatorId = c.Int(),
                        companyId = c.Int(),
                        departmentId = c.Int(),
                        employeeId = c.Int(),
                        BasicSalary = c.Double(nullable: false),
                        FromDate = c.DateTime(),
                        ToDate = c.DateTime(),
                        isActive = c.Boolean(nullable: false),
                        Description = c.String(),
                        isApproved = c.Boolean(),
                        stage = c.String(),
                        ApprovedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tabCompany", t => t.companyId)
                .ForeignKey("dbo.Users", t => t.creatorId)
                .ForeignKey("dbo.tabDepartment", t => t.departmentId)
                .ForeignKey("dbo.Employees", t => t.employeeId)
                .Index(t => t.creatorId)
                .Index(t => t.companyId)
                .Index(t => t.departmentId)
                .Index(t => t.employeeId);
            
            CreateTable(
                "dbo.Payrolls",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreationDate = c.DateTime(),
                        SalaryMonth = c.DateTime(),
                        creatorId = c.Int(),
                        companyId = c.Int(),
                        departmentId = c.Int(),
                        employeeId = c.Int(),
                        employmentSalaryId = c.Int(),
                        ProvidentFund = c.Double(nullable: false),
                        ReturnedLoan = c.Double(nullable: false),
                        loansAdvanceId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tabCompany", t => t.companyId)
                .ForeignKey("dbo.Users", t => t.creatorId)
                .ForeignKey("dbo.tabDepartment", t => t.departmentId)
                .ForeignKey("dbo.Employees", t => t.employeeId)
                .ForeignKey("dbo.LoansAdvances", t => t.loansAdvanceId)
                .ForeignKey("dbo.EmploymentSalaries", t => t.employmentSalaryId)
                .Index(t => t.creatorId)
                .Index(t => t.companyId)
                .Index(t => t.departmentId)
                .Index(t => t.employeeId)
                .Index(t => t.employmentSalaryId)
                .Index(t => t.loansAdvanceId);
            
            CreateTable(
                "dbo.SalaryAllowances",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        payrollId = c.Int(),
                        allowanceId = c.Int(),
                        Amount = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Allowances", t => t.allowanceId)
                .ForeignKey("dbo.Payrolls", t => t.payrollId)
                .Index(t => t.payrollId)
                .Index(t => t.allowanceId);
            
            CreateTable(
                "dbo.SalaryBonus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        payrollId = c.Int(),
                        bonusId = c.Int(),
                        Amount = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Bonus", t => t.bonusId)
                .ForeignKey("dbo.Payrolls", t => t.payrollId)
                .Index(t => t.payrollId)
                .Index(t => t.bonusId);
            
            CreateTable(
                "dbo.SalaryDeductions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        payrollId = c.Int(),
                        deductionId = c.Int(),
                        Amount = c.Double(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SalDeductions", t => t.deductionId)
                .ForeignKey("dbo.Payrolls", t => t.payrollId)
                .Index(t => t.payrollId)
                .Index(t => t.deductionId);
            
            CreateTable(
                "dbo.SalDeductions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ExchangeRateGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ExchangeType = c.Int(nullable: false),
                        TargetYear = c.Int(nullable: false),
                        AddedOn = c.DateTime(nullable: false),
                        transaction_currency_Id = c.Int(),
                        base_currency_Id = c.Int(),
                        Addedbyuser_Id = c.Int(),
                        Editedbyuser_Id = c.Int(),
                        LastUpdated = c.DateTime(),
                        isVoid = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Addedbyuser_Id)
                .ForeignKey("dbo.Currencies", t => t.base_currency_Id)
                .ForeignKey("dbo.Users", t => t.Editedbyuser_Id)
                .ForeignKey("dbo.Currencies", t => t.transaction_currency_Id)
                .Index(t => t.transaction_currency_Id)
                .Index(t => t.base_currency_Id)
                .Index(t => t.Addedbyuser_Id)
                .Index(t => t.Editedbyuser_Id);
            
            CreateTable(
                "dbo.ExchangeRates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        rateJan = c.Double(nullable: false),
                        rateFeb = c.Double(nullable: false),
                        rateMar = c.Double(nullable: false),
                        rateApr = c.Double(nullable: false),
                        rateMay = c.Double(nullable: false),
                        rateJun = c.Double(nullable: false),
                        rateJul = c.Double(nullable: false),
                        rateAug = c.Double(nullable: false),
                        rateSep = c.Double(nullable: false),
                        rateOct = c.Double(nullable: false),
                        rateNov = c.Double(nullable: false),
                        rateDec = c.Double(nullable: false),
                        company_Id = c.Int(),
                        ExchangeRateGroup_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tabCompany", t => t.company_Id)
                .ForeignKey("dbo.ExchangeRateGroups", t => t.ExchangeRateGroup_Id)
                .Index(t => t.company_Id)
                .Index(t => t.ExchangeRateGroup_Id);
            
            CreateTable(
                "dbo.Fields",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Identity = c.Int(nullable: false),
                        Tag = c.String(),
                        ElementName = c.String(),
                        moduleField_Id = c.Int(),
                        LastModified = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ModuleFields", t => t.moduleField_Id)
                .Index(t => t.moduleField_Id);
            
            CreateTable(
                "dbo.ModuleFields",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        transactionType = c.Int(nullable: false),
                        templateId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Templates", t => t.templateId)
                .Index(t => t.templateId);
            
            CreateTable(
                "dbo.Lands",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        isMortgaged = c.Boolean(nullable: false),
                        mortgagedValue = c.Double(nullable: false),
                        asset_Id = c.Int(),
                        measureUnit_Id = c.Int(),
                        mortgagee_Id = c.Int(),
                        OfficialAuths_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Assets", t => t.asset_Id)
                .ForeignKey("dbo.Areas", t => t.measureUnit_Id)
                .ForeignKey("dbo.Mortgagees", t => t.mortgagee_Id)
                .ForeignKey("dbo.OfficialAuths", t => t.OfficialAuths_Id)
                .Index(t => t.asset_Id)
                .Index(t => t.measureUnit_Id)
                .Index(t => t.mortgagee_Id)
                .Index(t => t.OfficialAuths_Id);
            
            CreateTable(
                "dbo.PayeeCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        isActive = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.tabPopupNotifications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        TitleColorCode = c.String(),
                        popupText = c.String(),
                        TextColorCode = c.String(),
                        EmployeeIds = c.String(),
                        FontSize = c.Double(nullable: false),
                        fontWeight = c.String(),
                        Italic = c.String(),
                        FontSizeHeading = c.Double(nullable: false),
                        fontWeightHeading = c.String(),
                        ItalicHeading = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PQDocuments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
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
                "dbo.RentalAssetMethods",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MethodName = c.String(),
                        isActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RentalAssetStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.String(),
                        isApproved = c.Boolean(nullable: false),
                        isActive = c.Boolean(),
                        backcolor = c.String(),
                        forecolor = c.String(),
                        HierarchicalIndex = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RentalReceiveAmounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        creationDate = c.DateTime(nullable: false),
                        receiveAmount = c.Double(nullable: false),
                        rentalBasis = c.Int(nullable: false),
                        AssetType = c.Int(nullable: false),
                        FinanceRefNo = c.String(),
                        SystemRefNo = c.String(),
                        transactionGroupId = c.Int(nullable: false),
                        company_Id = c.Int(),
                        dept_Id = c.Int(),
                        tenancyContractId = c.Int(),
                        Subsidary = c.String(),
                        TenantName = c.String(),
                        DateFrom = c.DateTime(),
                        DateTo = c.DateTime(),
                        RentalassetId = c.Int(),
                        tenantId = c.Int(),
                        ownerId = c.Int(),
                        rentalDate = c.DateTime(),
                        rentalAmount = c.Double(nullable: false),
                        statusId = c.Int(),
                        isVoid = c.Boolean(nullable: false),
                        isReviewed = c.Boolean(),
                        needReview = c.Boolean(),
                        PendingForClosing = c.Boolean(),
                        PendingForReApproval = c.Boolean(),
                        isApproved = c.Boolean(),
                        ApprovedDate = c.DateTime(),
                        isReApproved = c.Boolean(),
                        ReApprovalDate = c.DateTime(),
                        ClosingDate = c.DateTime(),
                        LastStatusChangeDate = c.DateTime(),
                        creatorId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tabCompany", t => t.company_Id)
                .ForeignKey("dbo.Employees", t => t.creatorId)
                .ForeignKey("dbo.tabDepartment", t => t.dept_Id)
                .ForeignKey("dbo.AssetOwners", t => t.ownerId)
                .ForeignKey("dbo.RentalAssets", t => t.RentalassetId)
                .ForeignKey("dbo.RentalAssetStatus", t => t.statusId)
                .ForeignKey("dbo.TenancyContracts", t => t.tenancyContractId)
                .ForeignKey("dbo.Tenants", t => t.tenantId)
                .Index(t => t.company_Id)
                .Index(t => t.dept_Id)
                .Index(t => t.tenancyContractId)
                .Index(t => t.RentalassetId)
                .Index(t => t.tenantId)
                .Index(t => t.ownerId)
                .Index(t => t.statusId)
                .Index(t => t.creatorId);
            
            CreateTable(
                "dbo.ResidentCountries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        travelerId = c.Int(),
                        countryId = c.Int(),
                        fromDate = c.DateTime(),
                        toDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Countries", t => t.countryId)
                .ForeignKey("dbo.Travelers", t => t.travelerId)
                .Index(t => t.travelerId)
                .Index(t => t.countryId);
            
            CreateTable(
                "dbo.Travelers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        isActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RoleFields",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Header = c.String(),
                        FieldName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SecurityDeposits",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        mainBankId = c.Int(),
                        bankId = c.Int(),
                        accountId = c.Int(),
                        Amount = c.Double(nullable: false),
                        collectionMethodId = c.Int(),
                        RefNo = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Accounts", t => t.accountId)
                .ForeignKey("dbo.Banks", t => t.bankId)
                .ForeignKey("dbo.CollectionMethods", t => t.collectionMethodId)
                .ForeignKey("dbo.MainBanks", t => t.mainBankId)
                .Index(t => t.mainBankId)
                .Index(t => t.bankId)
                .Index(t => t.accountId)
                .Index(t => t.collectionMethodId);
            
            CreateTable(
                "dbo.ShippingTerms",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ToDoTaskThemes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ThemeColorCode = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TravelingRecords",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreationDate = c.DateTime(),
                        companyId = c.Int(),
                        deptId = c.Int(),
                        travelerNameId = c.Int(),
                        transactionGroupId = c.Int(nullable: false),
                        SystemRefNo = c.String(),
                        statusId = c.Int(),
                        residentYear = c.DateTime(),
                        residentFromDate = c.DateTime(),
                        residentToDate = c.DateTime(),
                        residentCountryId = c.Int(),
                        DaysInResidentCountry = c.Double(nullable: false),
                        DaysInOtherCountries = c.Double(nullable: false),
                        TotalDays = c.Double(nullable: false),
                        RequiredResidentDays = c.Double(nullable: false),
                        stage = c.String(),
                        isVoid = c.Boolean(nullable: false),
                        isReviewed = c.Boolean(),
                        needReview = c.Boolean(),
                        PendingForClosing = c.Boolean(),
                        PendingForReApproval = c.Boolean(),
                        isApproved = c.Boolean(),
                        ApprovedDate = c.DateTime(),
                        isReApproved = c.Boolean(),
                        ReApprovalDate = c.DateTime(),
                        creatorId = c.Int(),
                        ClosingDate = c.DateTime(),
                        LastStatusChangeDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tabCompany", t => t.companyId)
                .ForeignKey("dbo.Users", t => t.creatorId)
                .ForeignKey("dbo.tabDepartment", t => t.deptId)
                .ForeignKey("dbo.ResidentCountries", t => t.residentCountryId)
                .ForeignKey("dbo.TravelingStatus", t => t.statusId)
                .ForeignKey("dbo.Travelers", t => t.travelerNameId)
                .Index(t => t.companyId)
                .Index(t => t.deptId)
                .Index(t => t.travelerNameId)
                .Index(t => t.statusId)
                .Index(t => t.residentCountryId)
                .Index(t => t.creatorId);
            
            CreateTable(
                "dbo.TravelingStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.String(),
                        isApproved = c.Boolean(nullable: false),
                        isActive = c.Boolean(),
                        backcolor = c.String(),
                        forecolor = c.String(),
                        HierarchicalIndex = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VisitingCountries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        sequence = c.Int(nullable: false),
                        transactionGroupId = c.Int(nullable: false),
                        SystemRefNo = c.String(),
                        TicketNo = c.String(),
                        TravelingRecordsId = c.Int(),
                        departingCountryId = c.Int(),
                        visitingCountryId = c.Int(),
                        airlineId = c.Int(),
                        residentYear = c.DateTime(),
                        DepartureDate = c.DateTime(),
                        ArrivalDate = c.DateTime(),
                        LeavingDate = c.DateTime(),
                        End = c.Boolean(nullable: false),
                        itineraryStatus = c.Int(),
                        currencyId = c.Int(),
                        TicketCost = c.Double(nullable: false),
                        MER = c.Double(nullable: false),
                        TicketCostMER = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Airlines", t => t.airlineId)
                .ForeignKey("dbo.Currencies", t => t.currencyId)
                .ForeignKey("dbo.Countries", t => t.departingCountryId)
                .ForeignKey("dbo.TravelingRecords", t => t.TravelingRecordsId)
                .ForeignKey("dbo.Countries", t => t.visitingCountryId)
                .Index(t => t.TravelingRecordsId)
                .Index(t => t.departingCountryId)
                .Index(t => t.visitingCountryId)
                .Index(t => t.airlineId)
                .Index(t => t.currencyId);
            
            CreateTable(
                "dbo.UnBoundReports",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        userId = c.Int(nullable: false),
                        reportName = c.String(nullable: false, maxLength: 128, unicode: false),
                        template = c.String(unicode: false),
                        reportType = c.Int(nullable: false),
                        lastModified = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.Id, t.userId, t.reportName })
                .ForeignKey("dbo.Users", t => t.userId, cascadeDelete: true)
                .Index(t => t.userId);
            
            CreateTable(
                "dbo.UniqueNumbers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UniqueName = c.String(),
                        isActive = c.Boolean(nullable: false),
                        From = c.DateTime(nullable: false),
                        To = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ViewInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        Timestamp = c.DateTime(nullable: false),
                        Info = c.String(),
                        Comment = c.String(),
                        TransactionType = c.Int(nullable: false),
                        TransactionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AdminBillTypeChartofAccounts",
                c => new
                    {
                        AdminBillType_Id = c.Int(nullable: false),
                        ChartofAccount_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.AdminBillType_Id, t.ChartofAccount_Id })
                .ForeignKey("dbo.AdminBillTypes", t => t.AdminBillType_Id, cascadeDelete: true)
                .ForeignKey("dbo.ChartofAccounts", t => t.ChartofAccount_Id, cascadeDelete: true)
                .Index(t => t.AdminBillType_Id)
                .Index(t => t.ChartofAccount_Id);
            
            CreateTable(
                "dbo.PayeeAdminBillTypes",
                c => new
                    {
                        Payee_Id = c.Int(nullable: false),
                        AdminBillType_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Payee_Id, t.AdminBillType_Id })
                .ForeignKey("dbo.Payees", t => t.Payee_Id, cascadeDelete: true)
                .ForeignKey("dbo.AdminBillTypes", t => t.AdminBillType_Id, cascadeDelete: true)
                .Index(t => t.Payee_Id)
                .Index(t => t.AdminBillType_Id);
            
            CreateTable(
                "dbo.PayeeDepartments",
                c => new
                    {
                        Payee_Id = c.Int(nullable: false),
                        Department_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Payee_Id, t.Department_Id })
                .ForeignKey("dbo.Payees", t => t.Payee_Id, cascadeDelete: true)
                .ForeignKey("dbo.tabDepartment", t => t.Department_Id, cascadeDelete: true)
                .Index(t => t.Payee_Id)
                .Index(t => t.Department_Id);
            
            CreateTable(
                "dbo.VendorAdminBillTypes",
                c => new
                    {
                        Vendor_Id = c.Int(nullable: false),
                        AdminBillType_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Vendor_Id, t.AdminBillType_Id })
                .ForeignKey("dbo.tabVendor", t => t.Vendor_Id, cascadeDelete: true)
                .ForeignKey("dbo.AdminBillTypes", t => t.AdminBillType_Id, cascadeDelete: true)
                .Index(t => t.Vendor_Id)
                .Index(t => t.AdminBillType_Id);
            
            CreateTable(
                "dbo.AdminBillNatureCompanies",
                c => new
                    {
                        AdminBillNature_Id = c.Int(nullable: false),
                        Company_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.AdminBillNature_Id, t.Company_Id })
                .ForeignKey("dbo.AdminBillNatures", t => t.AdminBillNature_Id, cascadeDelete: true)
                .ForeignKey("dbo.tabCompany", t => t.Company_Id, cascadeDelete: true)
                .Index(t => t.AdminBillNature_Id)
                .Index(t => t.Company_Id);
            
            CreateTable(
                "dbo.AssetModelAssetBrands",
                c => new
                    {
                        AssetModel_Id = c.Int(nullable: false),
                        AssetBrand_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.AssetModel_Id, t.AssetBrand_Id })
                .ForeignKey("dbo.AssetModels", t => t.AssetModel_Id, cascadeDelete: true)
                .ForeignKey("dbo.AssetBrands", t => t.AssetBrand_Id, cascadeDelete: true)
                .Index(t => t.AssetModel_Id)
                .Index(t => t.AssetBrand_Id);
            
            CreateTable(
                "dbo.AssetModelRentalAssetSubNatures",
                c => new
                    {
                        AssetModel_Id = c.Int(nullable: false),
                        RentalAssetSubNature_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.AssetModel_Id, t.RentalAssetSubNature_Id })
                .ForeignKey("dbo.AssetModels", t => t.AssetModel_Id, cascadeDelete: true)
                .ForeignKey("dbo.RentalAssetSubNatures", t => t.RentalAssetSubNature_Id, cascadeDelete: true)
                .Index(t => t.AssetModel_Id)
                .Index(t => t.RentalAssetSubNature_Id);
            
            CreateTable(
                "dbo.AssetBrandRentalAssetSubNatures",
                c => new
                    {
                        AssetBrand_Id = c.Int(nullable: false),
                        RentalAssetSubNature_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.AssetBrand_Id, t.RentalAssetSubNature_Id })
                .ForeignKey("dbo.AssetBrands", t => t.AssetBrand_Id, cascadeDelete: true)
                .ForeignKey("dbo.RentalAssetSubNatures", t => t.RentalAssetSubNature_Id, cascadeDelete: true)
                .Index(t => t.AssetBrand_Id)
                .Index(t => t.RentalAssetSubNature_Id);
            
            CreateTable(
                "dbo.CommentLogUsers",
                c => new
                    {
                        CommentLog_Id = c.Int(nullable: false),
                        User_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CommentLog_Id, t.User_id })
                .ForeignKey("dbo.CommentLogs", t => t.CommentLog_Id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_id, cascadeDelete: true)
                .Index(t => t.CommentLog_Id)
                .Index(t => t.User_id);
            
            CreateTable(
                "dbo.CommentLogUser1",
                c => new
                    {
                        CommentLog_Id = c.Int(nullable: false),
                        User_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CommentLog_Id, t.User_id })
                .ForeignKey("dbo.CommentLogs", t => t.CommentLog_Id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_id, cascadeDelete: true)
                .Index(t => t.CommentLog_Id)
                .Index(t => t.User_id);
            
            CreateTable(
                "dbo.CommentLogUser2",
                c => new
                    {
                        CommentLog_Id = c.Int(nullable: false),
                        User_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CommentLog_Id, t.User_id })
                .ForeignKey("dbo.CommentLogs", t => t.CommentLog_Id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_id, cascadeDelete: true)
                .Index(t => t.CommentLog_Id)
                .Index(t => t.User_id);
            
            CreateTable(
                "dbo.CommentLogUser3",
                c => new
                    {
                        CommentLog_Id = c.Int(nullable: false),
                        User_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CommentLog_Id, t.User_id })
                .ForeignKey("dbo.CommentLogs", t => t.CommentLog_Id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_id, cascadeDelete: true)
                .Index(t => t.CommentLog_Id)
                .Index(t => t.User_id);
            
            CreateTable(
                "dbo.MemoUsers",
                c => new
                    {
                        Memo_Id = c.Int(nullable: false),
                        User_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Memo_Id, t.User_id })
                .ForeignKey("dbo.Memos", t => t.Memo_Id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_id, cascadeDelete: true)
                .Index(t => t.Memo_Id)
                .Index(t => t.User_id);
            
            CreateTable(
                "dbo.TaskGroupsUsers",
                c => new
                    {
                        TaskGroups_Id = c.Int(nullable: false),
                        User_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.TaskGroups_Id, t.User_id })
                .ForeignKey("dbo.TaskGroups", t => t.TaskGroups_Id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_id, cascadeDelete: true)
                .Index(t => t.TaskGroups_Id)
                .Index(t => t.User_id);
            
            CreateTable(
                "dbo.TaskGroupsUser1",
                c => new
                    {
                        TaskGroups_Id = c.Int(nullable: false),
                        User_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.TaskGroups_Id, t.User_id })
                .ForeignKey("dbo.TaskGroups", t => t.TaskGroups_Id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_id, cascadeDelete: true)
                .Index(t => t.TaskGroups_Id)
                .Index(t => t.User_id);
            
            CreateTable(
                "dbo.PollUsers",
                c => new
                    {
                        Poll_Id = c.Int(nullable: false),
                        User_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Poll_Id, t.User_id })
                .ForeignKey("dbo.Polls", t => t.Poll_Id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_id, cascadeDelete: true)
                .Index(t => t.Poll_Id)
                .Index(t => t.User_id);
            
            CreateTable(
                "dbo.PollUser1",
                c => new
                    {
                        Poll_Id = c.Int(nullable: false),
                        User_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Poll_Id, t.User_id })
                .ForeignKey("dbo.Polls", t => t.Poll_Id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_id, cascadeDelete: true)
                .Index(t => t.Poll_Id)
                .Index(t => t.User_id);
            
            CreateTable(
                "dbo.PermissionRoles",
                c => new
                    {
                        Permission_Id = c.Int(nullable: false),
                        Role_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Permission_Id, t.Role_Id })
                .ForeignKey("dbo.Permissions", t => t.Permission_Id, cascadeDelete: true)
                .ForeignKey("dbo.Roles", t => t.Role_Id, cascadeDelete: true)
                .Index(t => t.Permission_Id)
                .Index(t => t.Role_Id);
            
            CreateTable(
                "dbo.RoleUsers",
                c => new
                    {
                        Role_Id = c.Int(nullable: false),
                        User_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Role_Id, t.User_id })
                .ForeignKey("dbo.Roles", t => t.Role_Id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_id, cascadeDelete: true)
                .Index(t => t.Role_Id)
                .Index(t => t.User_id);
            
            CreateTable(
                "dbo.TasksUsers",
                c => new
                    {
                        Tasks_Id = c.Int(nullable: false),
                        User_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Tasks_Id, t.User_id })
                .ForeignKey("dbo.Tasks", t => t.Tasks_Id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_id, cascadeDelete: true)
                .Index(t => t.Tasks_Id)
                .Index(t => t.User_id);
            
            CreateTable(
                "dbo.PrincipalDepartments",
                c => new
                    {
                        Principal_Id = c.Int(nullable: false),
                        Department_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Principal_Id, t.Department_Id })
                .ForeignKey("dbo.Principals", t => t.Principal_Id, cascadeDelete: true)
                .ForeignKey("dbo.tabDepartment", t => t.Department_Id, cascadeDelete: true)
                .Index(t => t.Principal_Id)
                .Index(t => t.Department_Id);
            
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
                "dbo.CustomerCompanyDepartment1",
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
                "dbo.CustomerCompanyIndustryTypes",
                c => new
                    {
                        CustomerCompany_Id = c.Int(nullable: false),
                        IndustryType_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CustomerCompany_Id, t.IndustryType_Id })
                .ForeignKey("dbo.CustomerCompanies", t => t.CustomerCompany_Id, cascadeDelete: true)
                .ForeignKey("dbo.IndustryTypes", t => t.IndustryType_Id, cascadeDelete: true)
                .Index(t => t.CustomerCompany_Id)
                .Index(t => t.IndustryType_Id);
            
            CreateTable(
                "dbo.AdminBillStatusStatusClasses",
                c => new
                    {
                        AdminBillStatus_Id = c.Int(nullable: false),
                        StatusClass_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.AdminBillStatus_Id, t.StatusClass_Id })
                .ForeignKey("dbo.AdminBillStatus", t => t.AdminBillStatus_Id, cascadeDelete: true)
                .ForeignKey("dbo.StatusClasses", t => t.StatusClass_Id, cascadeDelete: true)
                .Index(t => t.AdminBillStatus_Id)
                .Index(t => t.StatusClass_Id);
            
            CreateTable(
                "dbo.AssetRentalStatusStatusClasses",
                c => new
                    {
                        AssetRentalStatus_Id = c.Int(nullable: false),
                        StatusClass_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.AssetRentalStatus_Id, t.StatusClass_Id })
                .ForeignKey("dbo.AssetRentalStatus", t => t.AssetRentalStatus_Id, cascadeDelete: true)
                .ForeignKey("dbo.StatusClasses", t => t.StatusClass_Id, cascadeDelete: true)
                .Index(t => t.AssetRentalStatus_Id)
                .Index(t => t.StatusClass_Id);
            
            CreateTable(
                "dbo.BillStatusStatusClasses",
                c => new
                    {
                        BillStatus_Id = c.Int(nullable: false),
                        StatusClass_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.BillStatus_Id, t.StatusClass_Id })
                .ForeignKey("dbo.BillStatus", t => t.BillStatus_Id, cascadeDelete: true)
                .ForeignKey("dbo.StatusClasses", t => t.StatusClass_Id, cascadeDelete: true)
                .Index(t => t.BillStatus_Id)
                .Index(t => t.StatusClass_Id);
            
            CreateTable(
                "dbo.DocumentTemplateDocumentTypes",
                c => new
                    {
                        DocumentTemplate_Id = c.Int(nullable: false),
                        DocumentType_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.DocumentTemplate_Id, t.DocumentType_Id })
                .ForeignKey("dbo.DocumentTemplates", t => t.DocumentTemplate_Id, cascadeDelete: true)
                .ForeignKey("dbo.DocumentTypes", t => t.DocumentType_Id, cascadeDelete: true)
                .Index(t => t.DocumentTemplate_Id)
                .Index(t => t.DocumentType_Id);
            
            CreateTable(
                "dbo.DocumentStatusStatusClasses",
                c => new
                    {
                        DocumentStatus_Id = c.Int(nullable: false),
                        StatusClass_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.DocumentStatus_Id, t.StatusClass_Id })
                .ForeignKey("dbo.DocumentStatus", t => t.DocumentStatus_Id, cascadeDelete: true)
                .ForeignKey("dbo.StatusClasses", t => t.StatusClass_Id, cascadeDelete: true)
                .Index(t => t.DocumentStatus_Id)
                .Index(t => t.StatusClass_Id);
            
            CreateTable(
                "dbo.SaleInvoiceStatusStatusClasses",
                c => new
                    {
                        SaleInvoiceStatus_Id = c.Int(nullable: false),
                        StatusClass_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.SaleInvoiceStatus_Id, t.StatusClass_Id })
                .ForeignKey("dbo.SaleInvoiceStatus", t => t.SaleInvoiceStatus_Id, cascadeDelete: true)
                .ForeignKey("dbo.StatusClasses", t => t.StatusClass_Id, cascadeDelete: true)
                .Index(t => t.SaleInvoiceStatus_Id)
                .Index(t => t.StatusClass_Id);
            
            CreateTable(
                "dbo.OfferStatusStatusClasses",
                c => new
                    {
                        OfferStatus_Id = c.Int(nullable: false),
                        StatusClass_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.OfferStatus_Id, t.StatusClass_Id })
                .ForeignKey("dbo.OfferStatus", t => t.OfferStatus_Id, cascadeDelete: true)
                .ForeignKey("dbo.StatusClasses", t => t.StatusClass_Id, cascadeDelete: true)
                .Index(t => t.OfferStatus_Id)
                .Index(t => t.StatusClass_Id);
            
            CreateTable(
                "dbo.OfferVendors",
                c => new
                    {
                        Offer_Id = c.Int(nullable: false),
                        Vendor_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Offer_Id, t.Vendor_Id })
                .ForeignKey("dbo.Offers", t => t.Offer_Id, cascadeDelete: true)
                .ForeignKey("dbo.tabVendor", t => t.Vendor_Id, cascadeDelete: true)
                .Index(t => t.Offer_Id)
                .Index(t => t.Vendor_Id);
            
            CreateTable(
                "dbo.SaleOrderStatusStatusClasses",
                c => new
                    {
                        SaleOrderStatus_Id = c.Int(nullable: false),
                        StatusClass_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.SaleOrderStatus_Id, t.StatusClass_Id })
                .ForeignKey("dbo.SaleOrderStatus", t => t.SaleOrderStatus_Id, cascadeDelete: true)
                .ForeignKey("dbo.StatusClasses", t => t.StatusClass_Id, cascadeDelete: true)
                .Index(t => t.SaleOrderStatus_Id)
                .Index(t => t.StatusClass_Id);
            
            CreateTable(
                "dbo.SaleOrderVendors",
                c => new
                    {
                        SaleOrder_Id = c.Int(nullable: false),
                        Vendor_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.SaleOrder_Id, t.Vendor_Id })
                .ForeignKey("dbo.SaleOrders", t => t.SaleOrder_Id, cascadeDelete: true)
                .ForeignKey("dbo.tabVendor", t => t.Vendor_Id, cascadeDelete: true)
                .Index(t => t.SaleOrder_Id)
                .Index(t => t.Vendor_Id);
            
            CreateTable(
                "dbo.RentalContractStatusStatusClasses",
                c => new
                    {
                        RentalContractStatus_Id = c.Int(nullable: false),
                        StatusClass_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.RentalContractStatus_Id, t.StatusClass_Id })
                .ForeignKey("dbo.RentalContractStatus", t => t.RentalContractStatus_Id, cascadeDelete: true)
                .ForeignKey("dbo.StatusClasses", t => t.StatusClass_Id, cascadeDelete: true)
                .Index(t => t.RentalContractStatus_Id)
                .Index(t => t.StatusClass_Id);
            
            CreateTable(
                "dbo.RentalOrderStatusStatusClasses",
                c => new
                    {
                        RentalOrderStatus_Id = c.Int(nullable: false),
                        StatusClass_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.RentalOrderStatus_Id, t.StatusClass_Id })
                .ForeignKey("dbo.RentalOrderStatus", t => t.RentalOrderStatus_Id, cascadeDelete: true)
                .ForeignKey("dbo.StatusClasses", t => t.StatusClass_Id, cascadeDelete: true)
                .Index(t => t.RentalOrderStatus_Id)
                .Index(t => t.StatusClass_Id);
            
            CreateTable(
                "dbo.RentalInvoiceStatusStatusClasses",
                c => new
                    {
                        RentalInvoiceStatus_Id = c.Int(nullable: false),
                        StatusClass_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.RentalInvoiceStatus_Id, t.StatusClass_Id })
                .ForeignKey("dbo.RentalInvoiceStatus", t => t.RentalInvoiceStatus_Id, cascadeDelete: true)
                .ForeignKey("dbo.StatusClasses", t => t.StatusClass_Id, cascadeDelete: true)
                .Index(t => t.RentalInvoiceStatus_Id)
                .Index(t => t.StatusClass_Id);
            
            CreateTable(
                "dbo.SalesReceiptStatusStatusClasses",
                c => new
                    {
                        SalesReceiptStatus_Id = c.Int(nullable: false),
                        StatusClass_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.SalesReceiptStatus_Id, t.StatusClass_Id })
                .ForeignKey("dbo.SalesReceiptStatus", t => t.SalesReceiptStatus_Id, cascadeDelete: true)
                .ForeignKey("dbo.StatusClasses", t => t.StatusClass_Id, cascadeDelete: true)
                .Index(t => t.SalesReceiptStatus_Id)
                .Index(t => t.StatusClass_Id);
            
            CreateTable(
                "dbo.SaleInvoiceVendors",
                c => new
                    {
                        SaleInvoice_Id = c.Int(nullable: false),
                        Vendor_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.SaleInvoice_Id, t.Vendor_Id })
                .ForeignKey("dbo.SaleInvoices", t => t.SaleInvoice_Id, cascadeDelete: true)
                .ForeignKey("dbo.tabVendor", t => t.Vendor_Id, cascadeDelete: true)
                .Index(t => t.SaleInvoice_Id)
                .Index(t => t.Vendor_Id);
            
            CreateTable(
                "dbo.PurchaseInvoiceStatusStatusClasses",
                c => new
                    {
                        PurchaseInvoiceStatus_Id = c.Int(nullable: false),
                        StatusClass_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.PurchaseInvoiceStatus_Id, t.StatusClass_Id })
                .ForeignKey("dbo.PurchaseInvoiceStatus", t => t.PurchaseInvoiceStatus_Id, cascadeDelete: true)
                .ForeignKey("dbo.StatusClasses", t => t.StatusClass_Id, cascadeDelete: true)
                .Index(t => t.PurchaseInvoiceStatus_Id)
                .Index(t => t.StatusClass_Id);
            
            CreateTable(
                "dbo.PurchaseInvoiceVendors",
                c => new
                    {
                        PurchaseInvoice_Id = c.Int(nullable: false),
                        Vendor_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.PurchaseInvoice_Id, t.Vendor_Id })
                .ForeignKey("dbo.PurchaseInvoices", t => t.PurchaseInvoice_Id, cascadeDelete: true)
                .ForeignKey("dbo.tabVendor", t => t.Vendor_Id, cascadeDelete: true)
                .Index(t => t.PurchaseInvoice_Id)
                .Index(t => t.Vendor_Id);
            
            CreateTable(
                "dbo.CreditCardDepartments",
                c => new
                    {
                        CreditCard_Id = c.Int(nullable: false),
                        Department_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CreditCard_Id, t.Department_Id })
                .ForeignKey("dbo.CreditCards", t => t.CreditCard_Id, cascadeDelete: true)
                .ForeignKey("dbo.tabDepartment", t => t.Department_Id, cascadeDelete: true)
                .Index(t => t.CreditCard_Id)
                .Index(t => t.Department_Id);
            
            CreateTable(
                "dbo.PaymentStatusStatusClasses",
                c => new
                    {
                        PaymentStatus_Id = c.Int(nullable: false),
                        StatusClass_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.PaymentStatus_Id, t.StatusClass_Id })
                .ForeignKey("dbo.PaymentStatus", t => t.PaymentStatus_Id, cascadeDelete: true)
                .ForeignKey("dbo.StatusClasses", t => t.StatusClass_Id, cascadeDelete: true)
                .Index(t => t.PaymentStatus_Id)
                .Index(t => t.StatusClass_Id);
            
            CreateTable(
                "dbo.ToDoTaskUsers",
                c => new
                    {
                        ToDoTask_Id = c.Int(nullable: false),
                        User_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ToDoTask_Id, t.User_id })
                .ForeignKey("dbo.ToDoTasks", t => t.ToDoTask_Id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_id, cascadeDelete: true)
                .Index(t => t.ToDoTask_Id)
                .Index(t => t.User_id);
            
            CreateTable(
                "dbo.LoansAdvanceStatusStatusClasses",
                c => new
                    {
                        LoansAdvanceStatus_Id = c.Int(nullable: false),
                        StatusClass_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.LoansAdvanceStatus_Id, t.StatusClass_Id })
                .ForeignKey("dbo.LoansAdvanceStatus", t => t.LoansAdvanceStatus_Id, cascadeDelete: true)
                .ForeignKey("dbo.StatusClasses", t => t.StatusClass_Id, cascadeDelete: true)
                .Index(t => t.LoansAdvanceStatus_Id)
                .Index(t => t.StatusClass_Id);
            
            CreateTable(
                "dbo.PurchaseOrderStatusStatusClasses",
                c => new
                    {
                        PurchaseOrderStatus_Id = c.Int(nullable: false),
                        StatusClass_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.PurchaseOrderStatus_Id, t.StatusClass_Id })
                .ForeignKey("dbo.PurchaseOrderStatus", t => t.PurchaseOrderStatus_Id, cascadeDelete: true)
                .ForeignKey("dbo.StatusClasses", t => t.StatusClass_Id, cascadeDelete: true)
                .Index(t => t.PurchaseOrderStatus_Id)
                .Index(t => t.StatusClass_Id);
            
            CreateTable(
                "dbo.PurchaseOrderVendors",
                c => new
                    {
                        PurchaseOrder_Id = c.Int(nullable: false),
                        Vendor_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.PurchaseOrder_Id, t.Vendor_Id })
                .ForeignKey("dbo.PurchaseOrders", t => t.PurchaseOrder_Id, cascadeDelete: true)
                .ForeignKey("dbo.tabVendor", t => t.Vendor_Id, cascadeDelete: true)
                .Index(t => t.PurchaseOrder_Id)
                .Index(t => t.Vendor_Id);
            
            CreateTable(
                "dbo.ModuleContractStatusStatusClasses",
                c => new
                    {
                        ModuleContractStatus_Id = c.Int(nullable: false),
                        StatusClass_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ModuleContractStatus_Id, t.StatusClass_Id })
                .ForeignKey("dbo.ModuleContractStatus", t => t.ModuleContractStatus_Id, cascadeDelete: true)
                .ForeignKey("dbo.StatusClasses", t => t.StatusClass_Id, cascadeDelete: true)
                .Index(t => t.ModuleContractStatus_Id)
                .Index(t => t.StatusClass_Id);
            
            CreateTable(
                "dbo.ModuleContractVendors",
                c => new
                    {
                        ModuleContract_Id = c.Int(nullable: false),
                        Vendor_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ModuleContract_Id, t.Vendor_Id })
                .ForeignKey("dbo.ModuleContracts", t => t.ModuleContract_Id, cascadeDelete: true)
                .ForeignKey("dbo.tabVendor", t => t.Vendor_Id, cascadeDelete: true)
                .Index(t => t.ModuleContract_Id)
                .Index(t => t.Vendor_Id);
            
            CreateTable(
                "dbo.ProductDepartments",
                c => new
                    {
                        Product_Id = c.Int(nullable: false),
                        Department_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Product_Id, t.Department_Id })
                .ForeignKey("dbo.Products", t => t.Product_Id, cascadeDelete: true)
                .ForeignKey("dbo.tabDepartment", t => t.Department_Id, cascadeDelete: true)
                .Index(t => t.Product_Id)
                .Index(t => t.Department_Id);
            
            CreateTable(
                "dbo.InquiryStatusStatusClasses",
                c => new
                    {
                        InquiryStatus_Id = c.Int(nullable: false),
                        StatusClass_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.InquiryStatus_Id, t.StatusClass_Id })
                .ForeignKey("dbo.InquiryStatus", t => t.InquiryStatus_Id, cascadeDelete: true)
                .ForeignKey("dbo.StatusClasses", t => t.StatusClass_Id, cascadeDelete: true)
                .Index(t => t.InquiryStatus_Id)
                .Index(t => t.StatusClass_Id);
            
            CreateTable(
                "dbo.LoansStatusStatusClasses",
                c => new
                    {
                        LoansStatus_Id = c.Int(nullable: false),
                        StatusClass_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.LoansStatus_Id, t.StatusClass_Id })
                .ForeignKey("dbo.LoansStatus", t => t.LoansStatus_Id, cascadeDelete: true)
                .ForeignKey("dbo.StatusClasses", t => t.StatusClass_Id, cascadeDelete: true)
                .Index(t => t.LoansStatus_Id)
                .Index(t => t.StatusClass_Id);
            
            CreateTable(
                "dbo.StatusClassTargetRewardStatus",
                c => new
                    {
                        StatusClass_Id = c.Int(nullable: false),
                        TargetRewardStatus_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.StatusClass_Id, t.TargetRewardStatus_Id })
                .ForeignKey("dbo.StatusClasses", t => t.StatusClass_Id, cascadeDelete: true)
                .ForeignKey("dbo.TargetRewardStatus", t => t.TargetRewardStatus_Id, cascadeDelete: true)
                .Index(t => t.StatusClass_Id)
                .Index(t => t.TargetRewardStatus_Id);
            
            CreateTable(
                "dbo.TasksStatusTaskTypes",
                c => new
                    {
                        TasksStatus_Id = c.Int(nullable: false),
                        TaskType_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.TasksStatus_Id, t.TaskType_Id })
                .ForeignKey("dbo.TasksStatus", t => t.TasksStatus_Id, cascadeDelete: true)
                .ForeignKey("dbo.TaskTypes", t => t.TaskType_Id, cascadeDelete: true)
                .Index(t => t.TasksStatus_Id)
                .Index(t => t.TaskType_Id);
            
            CreateTable(
                "dbo.StatusClassTasksStatus",
                c => new
                    {
                        StatusClass_Id = c.Int(nullable: false),
                        TasksStatus_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.StatusClass_Id, t.TasksStatus_Id })
                .ForeignKey("dbo.StatusClasses", t => t.StatusClass_Id, cascadeDelete: true)
                .ForeignKey("dbo.TasksStatus", t => t.TasksStatus_Id, cascadeDelete: true)
                .Index(t => t.StatusClass_Id)
                .Index(t => t.TasksStatus_Id);
            
            CreateTable(
                "dbo.StatusClassTenantRentalStatus",
                c => new
                    {
                        StatusClass_Id = c.Int(nullable: false),
                        TenantRentalStatus_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.StatusClass_Id, t.TenantRentalStatus_Id })
                .ForeignKey("dbo.StatusClasses", t => t.StatusClass_Id, cascadeDelete: true)
                .ForeignKey("dbo.TenantRentalStatus", t => t.TenantRentalStatus_Id, cascadeDelete: true)
                .Index(t => t.StatusClass_Id)
                .Index(t => t.TenantRentalStatus_Id);
            
            CreateTable(
                "dbo.StatusClassToDoTaskStatus",
                c => new
                    {
                        StatusClass_Id = c.Int(nullable: false),
                        ToDoTaskStatus_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.StatusClass_Id, t.ToDoTaskStatus_Id })
                .ForeignKey("dbo.StatusClasses", t => t.StatusClass_Id, cascadeDelete: true)
                .ForeignKey("dbo.ToDoTaskStatus", t => t.ToDoTaskStatus_Id, cascadeDelete: true)
                .Index(t => t.StatusClass_Id)
                .Index(t => t.ToDoTaskStatus_Id);
            
            CreateTable(
                "dbo.InterBankTransferStatusStatusClasses",
                c => new
                    {
                        InterBankTransferStatus_Id = c.Int(nullable: false),
                        StatusClass_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.InterBankTransferStatus_Id, t.StatusClass_Id })
                .ForeignKey("dbo.InterBankTransferStatus", t => t.InterBankTransferStatus_Id, cascadeDelete: true)
                .ForeignKey("dbo.StatusClasses", t => t.StatusClass_Id, cascadeDelete: true)
                .Index(t => t.InterBankTransferStatus_Id)
                .Index(t => t.StatusClass_Id);
            
            CreateTable(
                "dbo.ReportUsers",
                c => new
                    {
                        Report_Id = c.Int(nullable: false),
                        User_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Report_Id, t.User_id })
                .ForeignKey("dbo.Reports", t => t.Report_Id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_id, cascadeDelete: true)
                .Index(t => t.Report_Id)
                .Index(t => t.User_id);
            
            CreateTable(
                "dbo.VendorBillNatureCompanies",
                c => new
                    {
                        VendorBillNature_Id = c.Int(nullable: false),
                        Company_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.VendorBillNature_Id, t.Company_Id })
                .ForeignKey("dbo.VendorBillNatures", t => t.VendorBillNature_Id, cascadeDelete: true)
                .ForeignKey("dbo.tabCompany", t => t.Company_Id, cascadeDelete: true)
                .Index(t => t.VendorBillNature_Id)
                .Index(t => t.Company_Id);
            
            CreateTable(
                "dbo.VendorPayees",
                c => new
                    {
                        Vendor_Id = c.Int(nullable: false),
                        Payee_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Vendor_Id, t.Payee_Id })
                .ForeignKey("dbo.tabVendor", t => t.Vendor_Id, cascadeDelete: true)
                .ForeignKey("dbo.Payees", t => t.Payee_Id, cascadeDelete: true)
                .Index(t => t.Vendor_Id)
                .Index(t => t.Payee_Id);
            
            CreateTable(
                "dbo.ChartofAccountCompanies",
                c => new
                    {
                        ChartofAccount_Id = c.Int(nullable: false),
                        Company_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ChartofAccount_Id, t.Company_Id })
                .ForeignKey("dbo.ChartofAccounts", t => t.ChartofAccount_Id, cascadeDelete: true)
                .ForeignKey("dbo.tabCompany", t => t.Company_Id, cascadeDelete: true)
                .Index(t => t.ChartofAccount_Id)
                .Index(t => t.Company_Id);
            
            CreateTable(
                "dbo.ChartofAccountGroupChartofAccounts",
                c => new
                    {
                        ChartofAccountGroup_Id = c.Int(nullable: false),
                        ChartofAccount_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ChartofAccountGroup_Id, t.ChartofAccount_Id })
                .ForeignKey("dbo.ChartofAccountGroups", t => t.ChartofAccountGroup_Id, cascadeDelete: true)
                .ForeignKey("dbo.ChartofAccounts", t => t.ChartofAccount_Id, cascadeDelete: true)
                .Index(t => t.ChartofAccountGroup_Id)
                .Index(t => t.ChartofAccount_Id);
            
            CreateTable(
                "dbo.DepartmentAccounts",
                c => new
                    {
                        Department_Id = c.Int(nullable: false),
                        Account_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Department_Id, t.Account_Id })
                .ForeignKey("dbo.tabDepartment", t => t.Department_Id, cascadeDelete: true)
                .ForeignKey("dbo.Accounts", t => t.Account_Id, cascadeDelete: true)
                .Index(t => t.Department_Id)
                .Index(t => t.Account_Id);
            
            CreateTable(
                "dbo.CashFlowCompanies",
                c => new
                    {
                        CashFlow_Id = c.Int(nullable: false),
                        Company_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CashFlow_Id, t.Company_Id })
                .ForeignKey("dbo.CashFlows", t => t.CashFlow_Id, cascadeDelete: true)
                .ForeignKey("dbo.tabCompany", t => t.Company_Id, cascadeDelete: true)
                .Index(t => t.CashFlow_Id)
                .Index(t => t.Company_Id);
            
            CreateTable(
                "dbo.CashFlowDepartments",
                c => new
                    {
                        CashFlow_Id = c.Int(nullable: false),
                        Department_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CashFlow_Id, t.Department_Id })
                .ForeignKey("dbo.CashFlows", t => t.CashFlow_Id, cascadeDelete: true)
                .ForeignKey("dbo.tabDepartment", t => t.Department_Id, cascadeDelete: true)
                .Index(t => t.CashFlow_Id)
                .Index(t => t.Department_Id);
            
            CreateTable(
                "dbo.DepartmentChartofAccountGroups",
                c => new
                    {
                        Department_Id = c.Int(nullable: false),
                        ChartofAccountGroup_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Department_Id, t.ChartofAccountGroup_Id })
                .ForeignKey("dbo.tabDepartment", t => t.Department_Id, cascadeDelete: true)
                .ForeignKey("dbo.ChartofAccountGroups", t => t.ChartofAccountGroup_Id, cascadeDelete: true)
                .Index(t => t.Department_Id)
                .Index(t => t.ChartofAccountGroup_Id);
            
            CreateTable(
                "dbo.DepartmentChartofAccounts",
                c => new
                    {
                        Department_Id = c.Int(nullable: false),
                        ChartofAccount_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Department_Id, t.ChartofAccount_Id })
                .ForeignKey("dbo.tabDepartment", t => t.Department_Id, cascadeDelete: true)
                .ForeignKey("dbo.ChartofAccounts", t => t.ChartofAccount_Id, cascadeDelete: true)
                .Index(t => t.Department_Id)
                .Index(t => t.ChartofAccount_Id);
            
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
                "dbo.DepartmentDocumentTemplates",
                c => new
                    {
                        Department_Id = c.Int(nullable: false),
                        DocumentTemplate_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Department_Id, t.DocumentTemplate_Id })
                .ForeignKey("dbo.tabDepartment", t => t.Department_Id, cascadeDelete: true)
                .ForeignKey("dbo.DocumentTemplates", t => t.DocumentTemplate_Id, cascadeDelete: true)
                .Index(t => t.Department_Id)
                .Index(t => t.DocumentTemplate_Id);
            
            CreateTable(
                "dbo.DepartmentDocumentTypes",
                c => new
                    {
                        Department_Id = c.Int(nullable: false),
                        DocumentType_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Department_Id, t.DocumentType_Id })
                .ForeignKey("dbo.tabDepartment", t => t.Department_Id, cascadeDelete: true)
                .ForeignKey("dbo.DocumentTypes", t => t.DocumentType_Id, cascadeDelete: true)
                .Index(t => t.Department_Id)
                .Index(t => t.DocumentType_Id);
            
            CreateTable(
                "dbo.DepartmentEmployees",
                c => new
                    {
                        Department_Id = c.Int(nullable: false),
                        Employee_EmpId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Department_Id, t.Employee_EmpId })
                .ForeignKey("dbo.tabDepartment", t => t.Department_Id, cascadeDelete: true)
                .ForeignKey("dbo.Employees", t => t.Employee_EmpId, cascadeDelete: true)
                .Index(t => t.Department_Id)
                .Index(t => t.Employee_EmpId);
            
            CreateTable(
                "dbo.DepartmentLoanApplicants",
                c => new
                    {
                        Department_Id = c.Int(nullable: false),
                        LoanApplicant_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Department_Id, t.LoanApplicant_Id })
                .ForeignKey("dbo.tabDepartment", t => t.Department_Id, cascadeDelete: true)
                .ForeignKey("dbo.LoanApplicants", t => t.LoanApplicant_Id, cascadeDelete: true)
                .Index(t => t.Department_Id)
                .Index(t => t.LoanApplicant_Id);
            
            CreateTable(
                "dbo.DepartmentPayments",
                c => new
                    {
                        Department_Id = c.Int(nullable: false),
                        Payment_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Department_Id, t.Payment_Id })
                .ForeignKey("dbo.tabDepartment", t => t.Department_Id, cascadeDelete: true)
                .ForeignKey("dbo.Payments", t => t.Payment_Id, cascadeDelete: true)
                .Index(t => t.Department_Id)
                .Index(t => t.Payment_Id);
            
            CreateTable(
                "dbo.DepartmentSalesReceipts",
                c => new
                    {
                        Department_Id = c.Int(nullable: false),
                        SalesReceipt_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Department_Id, t.SalesReceipt_Id })
                .ForeignKey("dbo.tabDepartment", t => t.Department_Id, cascadeDelete: true)
                .ForeignKey("dbo.SalesReceipts", t => t.SalesReceipt_Id, cascadeDelete: true)
                .Index(t => t.Department_Id)
                .Index(t => t.SalesReceipt_Id);
            
            CreateTable(
                "dbo.SharedGridGroupCompanies",
                c => new
                    {
                        SharedGridGroup_Id = c.Int(nullable: false),
                        Company_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.SharedGridGroup_Id, t.Company_Id })
                .ForeignKey("dbo.SharedGridGroups", t => t.SharedGridGroup_Id, cascadeDelete: true)
                .ForeignKey("dbo.tabCompany", t => t.Company_Id, cascadeDelete: true)
                .Index(t => t.SharedGridGroup_Id)
                .Index(t => t.Company_Id);
            
            CreateTable(
                "dbo.SharedGridGroupDepartments",
                c => new
                    {
                        SharedGridGroup_Id = c.Int(nullable: false),
                        Department_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.SharedGridGroup_Id, t.Department_Id })
                .ForeignKey("dbo.SharedGridGroups", t => t.SharedGridGroup_Id, cascadeDelete: true)
                .ForeignKey("dbo.tabDepartment", t => t.Department_Id, cascadeDelete: true)
                .Index(t => t.SharedGridGroup_Id)
                .Index(t => t.Department_Id);
            
            CreateTable(
                "dbo.SharedGridGroupEmployees",
                c => new
                    {
                        SharedGridGroup_Id = c.Int(nullable: false),
                        Employee_EmpId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.SharedGridGroup_Id, t.Employee_EmpId })
                .ForeignKey("dbo.SharedGridGroups", t => t.SharedGridGroup_Id, cascadeDelete: true)
                .ForeignKey("dbo.Employees", t => t.Employee_EmpId, cascadeDelete: true)
                .Index(t => t.SharedGridGroup_Id)
                .Index(t => t.Employee_EmpId);
            
            CreateTable(
                "dbo.DepartmentTaskGroups",
                c => new
                    {
                        Department_Id = c.Int(nullable: false),
                        TaskGroups_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Department_Id, t.TaskGroups_Id })
                .ForeignKey("dbo.tabDepartment", t => t.Department_Id, cascadeDelete: true)
                .ForeignKey("dbo.TaskGroups", t => t.TaskGroups_Id, cascadeDelete: true)
                .Index(t => t.Department_Id)
                .Index(t => t.TaskGroups_Id);
            
            CreateTable(
                "dbo.DepartmentTaskGroups1",
                c => new
                    {
                        Department_Id = c.Int(nullable: false),
                        TaskGroups_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Department_Id, t.TaskGroups_Id })
                .ForeignKey("dbo.tabDepartment", t => t.Department_Id, cascadeDelete: true)
                .ForeignKey("dbo.TaskGroups", t => t.TaskGroups_Id, cascadeDelete: true)
                .Index(t => t.Department_Id)
                .Index(t => t.TaskGroups_Id);
            
            CreateTable(
                "dbo.DepartmentTaskTypes",
                c => new
                    {
                        Department_Id = c.Int(nullable: false),
                        TaskType_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Department_Id, t.TaskType_Id })
                .ForeignKey("dbo.tabDepartment", t => t.Department_Id, cascadeDelete: true)
                .ForeignKey("dbo.TaskTypes", t => t.TaskType_Id, cascadeDelete: true)
                .Index(t => t.Department_Id)
                .Index(t => t.TaskType_Id);
            
            CreateTable(
                "dbo.DepartmentVendors",
                c => new
                    {
                        Department_Id = c.Int(nullable: false),
                        Vendor_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Department_Id, t.Vendor_Id })
                .ForeignKey("dbo.tabDepartment", t => t.Department_Id, cascadeDelete: true)
                .ForeignKey("dbo.tabVendor", t => t.Vendor_Id, cascadeDelete: true)
                .Index(t => t.Department_Id)
                .Index(t => t.Vendor_Id);
            
            CreateTable(
                "dbo.AssetTenancyContracts",
                c => new
                    {
                        Asset_Id = c.Int(nullable: false),
                        TenancyContract_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Asset_Id, t.TenancyContract_Id })
                .ForeignKey("dbo.Assets", t => t.Asset_Id, cascadeDelete: true)
                .ForeignKey("dbo.TenancyContracts", t => t.TenancyContract_Id, cascadeDelete: true)
                .Index(t => t.Asset_Id)
                .Index(t => t.TenancyContract_Id);
            
            CreateTable(
                "dbo.EmployeeChartofAccountGroups",
                c => new
                    {
                        Employee_EmpId = c.Int(nullable: false),
                        ChartofAccountGroup_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Employee_EmpId, t.ChartofAccountGroup_Id })
                .ForeignKey("dbo.Employees", t => t.Employee_EmpId, cascadeDelete: true)
                .ForeignKey("dbo.ChartofAccountGroups", t => t.ChartofAccountGroup_Id, cascadeDelete: true)
                .Index(t => t.Employee_EmpId)
                .Index(t => t.ChartofAccountGroup_Id);
            
            CreateTable(
                "dbo.EmployeeChartofAccounts",
                c => new
                    {
                        Employee_EmpId = c.Int(nullable: false),
                        ChartofAccount_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Employee_EmpId, t.ChartofAccount_Id })
                .ForeignKey("dbo.Employees", t => t.Employee_EmpId, cascadeDelete: true)
                .ForeignKey("dbo.ChartofAccounts", t => t.ChartofAccount_Id, cascadeDelete: true)
                .Index(t => t.Employee_EmpId)
                .Index(t => t.ChartofAccount_Id);
            
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
                "dbo.EmployeeCompany1",
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
                "dbo.CompanyEmployees",
                c => new
                    {
                        Company_Id = c.Int(nullable: false),
                        Employee_EmpId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Company_Id, t.Employee_EmpId })
                .ForeignKey("dbo.tabCompany", t => t.Company_Id, cascadeDelete: true)
                .ForeignKey("dbo.Employees", t => t.Employee_EmpId, cascadeDelete: true)
                .Index(t => t.Company_Id)
                .Index(t => t.Employee_EmpId);
            
            CreateTable(
                "dbo.CompanyBanks",
                c => new
                    {
                        Company_Id = c.Int(nullable: false),
                        Bank_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Company_Id, t.Bank_Id })
                .ForeignKey("dbo.tabCompany", t => t.Company_Id, cascadeDelete: true)
                .ForeignKey("dbo.Banks", t => t.Bank_Id, cascadeDelete: true)
                .Index(t => t.Company_Id)
                .Index(t => t.Bank_Id);
            
            CreateTable(
                "dbo.CompanyChartofAccountGroups",
                c => new
                    {
                        Company_Id = c.Int(nullable: false),
                        ChartofAccountGroup_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Company_Id, t.ChartofAccountGroup_Id })
                .ForeignKey("dbo.tabCompany", t => t.Company_Id, cascadeDelete: true)
                .ForeignKey("dbo.ChartofAccountGroups", t => t.ChartofAccountGroup_Id, cascadeDelete: true)
                .Index(t => t.Company_Id)
                .Index(t => t.ChartofAccountGroup_Id);
            
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
            
            CreateTable(
                "dbo.CompanyDocumentTemplates",
                c => new
                    {
                        Company_Id = c.Int(nullable: false),
                        DocumentTemplate_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Company_Id, t.DocumentTemplate_Id })
                .ForeignKey("dbo.tabCompany", t => t.Company_Id, cascadeDelete: true)
                .ForeignKey("dbo.DocumentTemplates", t => t.DocumentTemplate_Id, cascadeDelete: true)
                .Index(t => t.Company_Id)
                .Index(t => t.DocumentTemplate_Id);
            
            CreateTable(
                "dbo.CompanyDocumentTypes",
                c => new
                    {
                        Company_Id = c.Int(nullable: false),
                        DocumentType_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Company_Id, t.DocumentType_Id })
                .ForeignKey("dbo.tabCompany", t => t.Company_Id, cascadeDelete: true)
                .ForeignKey("dbo.DocumentTypes", t => t.DocumentType_Id, cascadeDelete: true)
                .Index(t => t.Company_Id)
                .Index(t => t.DocumentType_Id);
            
            CreateTable(
                "dbo.CompanyLoanApplicants",
                c => new
                    {
                        Company_Id = c.Int(nullable: false),
                        LoanApplicant_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Company_Id, t.LoanApplicant_Id })
                .ForeignKey("dbo.tabCompany", t => t.Company_Id, cascadeDelete: true)
                .ForeignKey("dbo.LoanApplicants", t => t.LoanApplicant_Id, cascadeDelete: true)
                .Index(t => t.Company_Id)
                .Index(t => t.LoanApplicant_Id);
            
            CreateTable(
                "dbo.CompanyPayees",
                c => new
                    {
                        Company_Id = c.Int(nullable: false),
                        Payee_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Company_Id, t.Payee_Id })
                .ForeignKey("dbo.tabCompany", t => t.Company_Id, cascadeDelete: true)
                .ForeignKey("dbo.Payees", t => t.Payee_Id, cascadeDelete: true)
                .Index(t => t.Company_Id)
                .Index(t => t.Payee_Id);
            
            CreateTable(
                "dbo.CompanyEmployee1",
                c => new
                    {
                        Company_Id = c.Int(nullable: false),
                        Employee_EmpId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Company_Id, t.Employee_EmpId })
                .ForeignKey("dbo.tabCompany", t => t.Company_Id, cascadeDelete: true)
                .ForeignKey("dbo.Employees", t => t.Employee_EmpId, cascadeDelete: true)
                .Index(t => t.Company_Id)
                .Index(t => t.Employee_EmpId);
            
            CreateTable(
                "dbo.CompanyTaskGroups",
                c => new
                    {
                        Company_Id = c.Int(nullable: false),
                        TaskGroups_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Company_Id, t.TaskGroups_Id })
                .ForeignKey("dbo.tabCompany", t => t.Company_Id, cascadeDelete: true)
                .ForeignKey("dbo.TaskGroups", t => t.TaskGroups_Id, cascadeDelete: true)
                .Index(t => t.Company_Id)
                .Index(t => t.TaskGroups_Id);
            
            CreateTable(
                "dbo.CompanyTaskGroups1",
                c => new
                    {
                        Company_Id = c.Int(nullable: false),
                        TaskGroups_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Company_Id, t.TaskGroups_Id })
                .ForeignKey("dbo.tabCompany", t => t.Company_Id, cascadeDelete: true)
                .ForeignKey("dbo.TaskGroups", t => t.TaskGroups_Id, cascadeDelete: true)
                .Index(t => t.Company_Id)
                .Index(t => t.TaskGroups_Id);
            
            CreateTable(
                "dbo.CompanyTaskTypes",
                c => new
                    {
                        Company_Id = c.Int(nullable: false),
                        TaskType_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Company_Id, t.TaskType_Id })
                .ForeignKey("dbo.tabCompany", t => t.Company_Id, cascadeDelete: true)
                .ForeignKey("dbo.TaskTypes", t => t.TaskType_Id, cascadeDelete: true)
                .Index(t => t.Company_Id)
                .Index(t => t.TaskType_Id);
            
            CreateTable(
                "dbo.CompanyVendors",
                c => new
                    {
                        Company_Id = c.Int(nullable: false),
                        Vendor_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Company_Id, t.Vendor_Id })
                .ForeignKey("dbo.tabCompany", t => t.Company_Id, cascadeDelete: true)
                .ForeignKey("dbo.tabVendor", t => t.Vendor_Id, cascadeDelete: true)
                .Index(t => t.Company_Id)
                .Index(t => t.Vendor_Id);
            
            CreateStoredProcedure(
                "dbo.Company_Insert",
                p => new
                    {
                        CompanyName = p.String(),
                        industryTypeId = p.Int(),
                        BizType = p.Int(),
                        EmployeerNo = p.String(),
                        CustomerVAT = p.String(),
                        SaleTaxRegistrationNumber = p.String(),
                        openingDate = p.DateTime(),
                        closingDate = p.DateTime(),
                        addressId = p.Int(),
                        contactId = p.Int(),
                        compnayType = p.Int(),
                        IsSubsidary = p.Boolean(),
                        ParentID = p.Int(),
                        CurrencyId = p.Int(),
                        isActive = p.Boolean(),
                        isLinkable = p.Boolean(),
                    },
                body:
                    @"INSERT [dbo].[tabCompany]([CompanyName], [industryTypeId], [BizType], [EmployeerNo], [CustomerVAT], [SaleTaxRegistrationNumber], [openingDate], [closingDate], [addressId], [contactId], [compnayType], [IsSubsidary], [ParentID], [CurrencyId], [isActive], [isLinkable])
                      VALUES (@CompanyName, @industryTypeId, @BizType, @EmployeerNo, @CustomerVAT, @SaleTaxRegistrationNumber, @openingDate, @closingDate, @addressId, @contactId, @compnayType, @IsSubsidary, @ParentID, @CurrencyId, @isActive, @isLinkable)
                      
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
                        CustomerVAT = p.String(),
                        SaleTaxRegistrationNumber = p.String(),
                        openingDate = p.DateTime(),
                        closingDate = p.DateTime(),
                        addressId = p.Int(),
                        contactId = p.Int(),
                        compnayType = p.Int(),
                        IsSubsidary = p.Boolean(),
                        ParentID = p.Int(),
                        CurrencyId = p.Int(),
                        isActive = p.Boolean(),
                        isLinkable = p.Boolean(),
                    },
                body:
                    @"UPDATE [dbo].[tabCompany]
                      SET [CompanyName] = @CompanyName, [industryTypeId] = @industryTypeId, [BizType] = @BizType, [EmployeerNo] = @EmployeerNo, [CustomerVAT] = @CustomerVAT, [SaleTaxRegistrationNumber] = @SaleTaxRegistrationNumber, [openingDate] = @openingDate, [closingDate] = @closingDate, [addressId] = @addressId, [contactId] = @contactId, [compnayType] = @compnayType, [IsSubsidary] = @IsSubsidary, [ParentID] = @ParentID, [CurrencyId] = @CurrencyId, [isActive] = @isActive, [isLinkable] = @isLinkable
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
            DropForeignKey("dbo.ViewInfoes", "UserId", "dbo.Users");
            DropForeignKey("dbo.UnBoundReports", "userId", "dbo.Users");
            DropForeignKey("dbo.VisitingCountries", "visitingCountryId", "dbo.Countries");
            DropForeignKey("dbo.VisitingCountries", "TravelingRecordsId", "dbo.TravelingRecords");
            DropForeignKey("dbo.VisitingCountries", "departingCountryId", "dbo.Countries");
            DropForeignKey("dbo.VisitingCountries", "currencyId", "dbo.Currencies");
            DropForeignKey("dbo.VisitingCountries", "airlineId", "dbo.Airlines");
            DropForeignKey("dbo.TravelingRecords", "travelerNameId", "dbo.Travelers");
            DropForeignKey("dbo.TravelingRecords", "statusId", "dbo.TravelingStatus");
            DropForeignKey("dbo.TravelingRecords", "residentCountryId", "dbo.ResidentCountries");
            DropForeignKey("dbo.TravelingRecords", "deptId", "dbo.tabDepartment");
            DropForeignKey("dbo.TravelingRecords", "creatorId", "dbo.Users");
            DropForeignKey("dbo.TravelingRecords", "companyId", "dbo.tabCompany");
            DropForeignKey("dbo.SecurityDeposits", "mainBankId", "dbo.MainBanks");
            DropForeignKey("dbo.SecurityDeposits", "collectionMethodId", "dbo.CollectionMethods");
            DropForeignKey("dbo.SecurityDeposits", "bankId", "dbo.Banks");
            DropForeignKey("dbo.SecurityDeposits", "accountId", "dbo.Accounts");
            DropForeignKey("dbo.ResidentCountries", "travelerId", "dbo.Travelers");
            DropForeignKey("dbo.ResidentCountries", "countryId", "dbo.Countries");
            DropForeignKey("dbo.RentalReceiveAmounts", "tenantId", "dbo.Tenants");
            DropForeignKey("dbo.RentalReceiveAmounts", "tenancyContractId", "dbo.TenancyContracts");
            DropForeignKey("dbo.RentalReceiveAmounts", "statusId", "dbo.RentalAssetStatus");
            DropForeignKey("dbo.RentalReceiveAmounts", "RentalassetId", "dbo.RentalAssets");
            DropForeignKey("dbo.RentalReceiveAmounts", "ownerId", "dbo.AssetOwners");
            DropForeignKey("dbo.RentalReceiveAmounts", "dept_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.RentalReceiveAmounts", "creatorId", "dbo.Employees");
            DropForeignKey("dbo.RentalReceiveAmounts", "company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.Lands", "OfficialAuths_Id", "dbo.OfficialAuths");
            DropForeignKey("dbo.Lands", "mortgagee_Id", "dbo.Mortgagees");
            DropForeignKey("dbo.Lands", "measureUnit_Id", "dbo.Areas");
            DropForeignKey("dbo.Lands", "asset_Id", "dbo.Assets");
            DropForeignKey("dbo.ModuleFields", "templateId", "dbo.Templates");
            DropForeignKey("dbo.Fields", "moduleField_Id", "dbo.ModuleFields");
            DropForeignKey("dbo.ExchangeRateGroups", "transaction_currency_Id", "dbo.Currencies");
            DropForeignKey("dbo.ExchangeRates", "ExchangeRateGroup_Id", "dbo.ExchangeRateGroups");
            DropForeignKey("dbo.ExchangeRates", "company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.ExchangeRateGroups", "Editedbyuser_Id", "dbo.Users");
            DropForeignKey("dbo.ExchangeRateGroups", "base_currency_Id", "dbo.Currencies");
            DropForeignKey("dbo.ExchangeRateGroups", "Addedbyuser_Id", "dbo.Users");
            DropForeignKey("dbo.Payrolls", "employmentSalaryId", "dbo.EmploymentSalaries");
            DropForeignKey("dbo.SalaryDeductions", "payrollId", "dbo.Payrolls");
            DropForeignKey("dbo.SalaryDeductions", "deductionId", "dbo.SalDeductions");
            DropForeignKey("dbo.SalaryBonus", "payrollId", "dbo.Payrolls");
            DropForeignKey("dbo.SalaryBonus", "bonusId", "dbo.Bonus");
            DropForeignKey("dbo.SalaryAllowances", "payrollId", "dbo.Payrolls");
            DropForeignKey("dbo.SalaryAllowances", "allowanceId", "dbo.Allowances");
            DropForeignKey("dbo.Payrolls", "loansAdvanceId", "dbo.LoansAdvances");
            DropForeignKey("dbo.Payrolls", "employeeId", "dbo.Employees");
            DropForeignKey("dbo.Payrolls", "departmentId", "dbo.tabDepartment");
            DropForeignKey("dbo.Payrolls", "creatorId", "dbo.Users");
            DropForeignKey("dbo.Payrolls", "companyId", "dbo.tabCompany");
            DropForeignKey("dbo.EmploymentSalaries", "employeeId", "dbo.Employees");
            DropForeignKey("dbo.EmploymentSalaries", "departmentId", "dbo.tabDepartment");
            DropForeignKey("dbo.EmploymentSalaries", "creatorId", "dbo.Users");
            DropForeignKey("dbo.EmploymentSalaries", "companyId", "dbo.tabCompany");
            DropForeignKey("dbo.EmployeePerformanceReviews", "SupervisorLevelTwoId", "dbo.Users");
            DropForeignKey("dbo.EmployeePerformanceReviews", "SupervisorLevelOneId", "dbo.Users");
            DropForeignKey("dbo.PerformanceIndicatorRatings", "ReviewId", "dbo.EmployeePerformanceReviews");
            DropForeignKey("dbo.PerformanceIndicatorRatings", "IndicatorDefinitionId", "dbo.PerformanceIndicatorDefinitions");
            DropForeignKey("dbo.EmployeePerformanceReviews", "MemoId", "dbo.Memos");
            DropForeignKey("dbo.EmployeePerformanceReviews", "ManagementId", "dbo.Users");
            DropForeignKey("dbo.Users", "EmployeePerformanceReview_Id", "dbo.EmployeePerformanceReviews");
            DropForeignKey("dbo.EmployeePerformanceReviews", "EmployeeId", "dbo.Users");
            DropForeignKey("dbo.EmployeePerformanceReviews", "DeptId", "dbo.tabDepartment");
            DropForeignKey("dbo.EmployeePerformanceReviews", "AdminId", "dbo.Users");
            DropForeignKey("dbo.EmployeeCoaCompanies", "EmpId", "dbo.Employees");
            DropForeignKey("dbo.EmployeeCoaCompanies", "compId", "dbo.tabCompany");
            DropForeignKey("dbo.CustomReportGeoups", "updatorId", "dbo.Users");
            DropForeignKey("dbo.CustomReports", "CustomReportGeoup_Id", "dbo.CustomReportGeoups");
            DropForeignKey("dbo.CustomReports", "deptId", "dbo.tabDepartment");
            DropForeignKey("dbo.CustomReports", "companyId", "dbo.tabCompany");
            DropForeignKey("dbo.CustomReports", "chartOfAccountId", "dbo.ChartofAccounts");
            DropForeignKey("dbo.CustomReportGeoups", "creatorId", "dbo.Users");
            DropForeignKey("dbo.Vehicles", "vehicleType_Id", "dbo.VehicleTypes");
            DropForeignKey("dbo.Vehicles", "OfficialAuths_Id", "dbo.OfficialAuths");
            DropForeignKey("dbo.Vehicles", "manufacturer_Id", "dbo.Manufacturers");
            DropForeignKey("dbo.Vehicles", "lesseId", "dbo.Lesees");
            DropForeignKey("dbo.Vehicles", "currentStatus_Id", "dbo.VehicleCurrentStatus");
            DropForeignKey("dbo.ConditionImages", "vehicleId", "dbo.Vehicles");
            DropForeignKey("dbo.Vehicles", "BuyingStatus_Id", "dbo.VehicleBuyingStatus");
            DropForeignKey("dbo.Vehicles", "assetId", "dbo.Assets");
            DropForeignKey("dbo.Buildings", "OfficialAuths_Id", "dbo.OfficialAuths");
            DropForeignKey("dbo.Buildings", "mortgagee_Id", "dbo.Mortgagees");
            DropForeignKey("dbo.Buildings", "measureUnit_Id", "dbo.Areas");
            DropForeignKey("dbo.Buildings", "asset_Id", "dbo.Assets");
            DropForeignKey("dbo.tabBackground", "userId", "dbo.Users");
            DropForeignKey("dbo.tabBackground", "taskGroupId", "dbo.TaskGroups");
            DropForeignKey("dbo.OfficialAuths", "region_Id", "dbo.Regions");
            DropForeignKey("dbo.AuthDocs", "authId", "dbo.OfficialAuths");
            DropForeignKey("dbo.AttachmentCategories", "userId", "dbo.Users");
            DropForeignKey("dbo.TransactionItems", "AttachmentCategory_Id", "dbo.AttachmentCategories");
            DropForeignKey("dbo.AttachmentCategories", "ParentId", "dbo.AttachmentCategories");
            DropForeignKey("dbo.Attachments", "userId", "dbo.Users");
            DropForeignKey("dbo.Attachments", "categoryId", "dbo.AttachmentCategories");
            DropForeignKey("dbo.Accounts", "vendor_Id", "dbo.tabVendor");
            DropForeignKey("dbo.Accounts", "mainBankId", "dbo.MainBanks");
            DropForeignKey("dbo.Accounts", "industryTypeId", "dbo.IndustryTypes");
            DropForeignKey("dbo.Accounts", "currency_Id", "dbo.Currencies");
            DropForeignKey("dbo.Accounts", "company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.Accounts", "COA_accountId", "dbo.ChartofAccounts");
            DropForeignKey("dbo.Accounts", "bank_Id", "dbo.Banks");
            DropForeignKey("dbo.Banks", "bankId", "dbo.MainBanks");
            DropForeignKey("dbo.Banks", "contact_Id", "dbo.tabContact");
            DropForeignKey("dbo.CompanyVendors", "Vendor_Id", "dbo.tabVendor");
            DropForeignKey("dbo.CompanyVendors", "Company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.CompanyTaskTypes", "TaskType_Id", "dbo.TaskTypes");
            DropForeignKey("dbo.CompanyTaskTypes", "Company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.CompanyTaskGroups1", "TaskGroups_Id", "dbo.TaskGroups");
            DropForeignKey("dbo.CompanyTaskGroups1", "Company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.CompanyTaskGroups", "TaskGroups_Id", "dbo.TaskGroups");
            DropForeignKey("dbo.CompanyTaskGroups", "Company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.CompanyEmployee1", "Employee_EmpId", "dbo.Employees");
            DropForeignKey("dbo.CompanyEmployee1", "Company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.SalesExchangeRates", "company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.CompanyPayees", "Payee_Id", "dbo.Payees");
            DropForeignKey("dbo.CompanyPayees", "Company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.tabCompany", "ParentID", "dbo.tabCompany");
            DropForeignKey("dbo.MarketExchangeRates", "company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.CompanyLoanApplicants", "LoanApplicant_Id", "dbo.LoanApplicants");
            DropForeignKey("dbo.CompanyLoanApplicants", "Company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.SaleOrders", "InterCompany_Id", "dbo.tabCompany");
            DropForeignKey("dbo.SaleInvoices", "InterCompany_Id", "dbo.tabCompany");
            DropForeignKey("dbo.PurchaseOrders", "InterCompany_Id", "dbo.tabCompany");
            DropForeignKey("dbo.PurchaseInvoices", "InterCompany_Id", "dbo.tabCompany");
            DropForeignKey("dbo.Payments", "InterCompany_Id", "dbo.tabCompany");
            DropForeignKey("dbo.Offers", "InterCompany_Id", "dbo.tabCompany");
            DropForeignKey("dbo.ModuleContracts", "InterCompany_Id", "dbo.tabCompany");
            DropForeignKey("dbo.MemorandumSales", "InterCompany_Id", "dbo.tabCompany");
            DropForeignKey("dbo.Inquiries", "InterCompany_Id", "dbo.tabCompany");
            DropForeignKey("dbo.tabCompany", "industryTypeId", "dbo.IndustryTypes");
            DropForeignKey("dbo.CompanyDocumentTypes", "DocumentType_Id", "dbo.DocumentTypes");
            DropForeignKey("dbo.CompanyDocumentTypes", "Company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.CompanyDocumentTemplates", "DocumentTemplate_Id", "dbo.DocumentTemplates");
            DropForeignKey("dbo.CompanyDocumentTemplates", "Company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.CompanyCustomerCompanies", "CustomerCompany_Id", "dbo.CustomerCompanies");
            DropForeignKey("dbo.CompanyCustomerCompanies", "Company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.tabCompany", "CurrencyId", "dbo.Currencies");
            DropForeignKey("dbo.Employees", "coreCompanyId", "dbo.tabCompany");
            DropForeignKey("dbo.tabCompany", "contactId", "dbo.tabContact");
            DropForeignKey("dbo.SaleOrders", "company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.SaleInvoices", "company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.PurchaseOrders", "company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.PurchaseInvoices", "company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.Offers", "company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.ModuleContracts", "company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.MemorandumSales", "company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.Inquiries", "company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.Documents", "company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.CompanyChartofAccountGroups", "ChartofAccountGroup_Id", "dbo.ChartofAccountGroups");
            DropForeignKey("dbo.CompanyChartofAccountGroups", "Company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.CompanyBanks", "Bank_Id", "dbo.Banks");
            DropForeignKey("dbo.CompanyBanks", "Company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.CompanyEmployees", "Employee_EmpId", "dbo.Employees");
            DropForeignKey("dbo.CompanyEmployees", "Company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.EmployeeWorkExperiences", "employeeId", "dbo.Employees");
            DropForeignKey("dbo.SalesReceipts", "transactionHolderId", "dbo.Employees");
            DropForeignKey("dbo.SaleOrders", "transactionHolderId", "dbo.Employees");
            DropForeignKey("dbo.SaleInvoices", "transactionHolderId", "dbo.Employees");
            DropForeignKey("dbo.PurchaseOrders", "transactionHolderId", "dbo.Employees");
            DropForeignKey("dbo.PurchaseInvoices", "transactionHolderId", "dbo.Employees");
            DropForeignKey("dbo.Payments", "transactionHolderId", "dbo.Employees");
            DropForeignKey("dbo.Offers", "transactionHolderId", "dbo.Employees");
            DropForeignKey("dbo.Inquiries", "transactionHolderId", "dbo.Employees");
            DropForeignKey("dbo.Bills", "transactionHolderId", "dbo.Employees");
            DropForeignKey("dbo.Employees", "SupervisorId", "dbo.Employees");
            DropForeignKey("dbo.CommentLogs", "employeeId", "dbo.Employees");
            DropForeignKey("dbo.Employees", "SalesTargetId", "dbo.SalesTargets");
            DropForeignKey("dbo.SalesTargets", "CurrencyId", "dbo.Currencies");
            DropForeignKey("dbo.Employees", "receivableAccountId", "dbo.ChartofAccounts");
            DropForeignKey("dbo.Qualifications", "employeeId", "dbo.Employees");
            DropForeignKey("dbo.EmployeeCompany1", "Company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.EmployeeCompany1", "Employee_EmpId", "dbo.Employees");
            DropForeignKey("dbo.Employees", "person_Id", "dbo.tabPerson");
            DropForeignKey("dbo.Employees", "HrInfoId", "dbo.EmployeeHRInfoes");
            DropForeignKey("dbo.LeaveApplications", "leaveStatus_Id", "dbo.LeaveStatus");
            DropForeignKey("dbo.LeaveApplications", "leaveId", "dbo.Leaves");
            DropForeignKey("dbo.Leaves", "employeeId", "dbo.Employees");
            DropForeignKey("dbo.LeaveApplications", "initiator_EmpId", "dbo.Employees");
            DropForeignKey("dbo.LeaveApplications", "HRinfo_Id", "dbo.EmployeeHRInfoes");
            DropForeignKey("dbo.LeaveApplications", "employeeId", "dbo.Employees");
            DropForeignKey("dbo.LeaveApplications", "approver_EmpId", "dbo.Employees");
            DropForeignKey("dbo.Employees", "employeeStatus_Id", "dbo.EmployeeWorkingStatus");
            DropForeignKey("dbo.Employees", "employeeApproval_Id", "dbo.EmployeeApprovals");
            DropForeignKey("dbo.Employees", "empFunction_Id", "dbo.Functions");
            DropForeignKey("dbo.Functions", "company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.Employees", "emergencyontact_Id", "dbo.Emergencyontacts");
            DropForeignKey("dbo.Employees", "Desig_DesigId", "dbo.Designations");
            DropForeignKey("dbo.Employees", "contact_Id", "dbo.tabContact");
            DropForeignKey("dbo.EmployeeCompanies", "Company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.EmployeeCompanies", "Employee_EmpId", "dbo.Employees");
            DropForeignKey("dbo.EmployeeChartofAccounts", "ChartofAccount_Id", "dbo.ChartofAccounts");
            DropForeignKey("dbo.EmployeeChartofAccounts", "Employee_EmpId", "dbo.Employees");
            DropForeignKey("dbo.EmployeeChartofAccountGroups", "ChartofAccountGroup_Id", "dbo.ChartofAccountGroups");
            DropForeignKey("dbo.EmployeeChartofAccountGroups", "Employee_EmpId", "dbo.Employees");
            DropForeignKey("dbo.CommentLogs", "AssigneeId", "dbo.Employees");
            DropForeignKey("dbo.Assets", "Employee_EmpId", "dbo.Employees");
            DropForeignKey("dbo.Assets", "user_Id", "dbo.Users");
            DropForeignKey("dbo.AssetTenancyContracts", "TenancyContract_Id", "dbo.TenancyContracts");
            DropForeignKey("dbo.AssetTenancyContracts", "Asset_Id", "dbo.Assets");
            DropForeignKey("dbo.TenancyContracts", "TenantId", "dbo.Tenants");
            DropForeignKey("dbo.Tenants", "person_Id", "dbo.tabPerson");
            DropForeignKey("dbo.Tenants", "deptId", "dbo.tabDepartment");
            DropForeignKey("dbo.Tenants", "contact_Id", "dbo.tabContact");
            DropForeignKey("dbo.Tenants", "companyId", "dbo.tabCompany");
            DropForeignKey("dbo.Tenants", "address_Id", "dbo.tabAddress");
            DropForeignKey("dbo.TenancyContracts", "OwnerId", "dbo.AssetOwners");
            DropForeignKey("dbo.TenancyContracts", "NotOwnedAssetId", "dbo.RentalAssets");
            DropForeignKey("dbo.TenancyContracts", "deptId", "dbo.tabDepartment");
            DropForeignKey("dbo.TenancyContracts", "companyId", "dbo.tabCompany");
            DropForeignKey("dbo.TenancyContracts", "AssetId", "dbo.RentalAssets");
            DropForeignKey("dbo.RentalAssets", "deptId", "dbo.tabDepartment");
            DropForeignKey("dbo.RentalAssets", "companyId", "dbo.tabCompany");
            DropForeignKey("dbo.Assets", "RentalAssets_Id", "dbo.RentalAssets");
            DropForeignKey("dbo.RentalAssets", "AssetNatureId", "dbo.AssetNatures");
            DropForeignKey("dbo.RentalAssets", "assetId", "dbo.Assets");
            DropForeignKey("dbo.Revaluations", "assetId", "dbo.Assets");
            DropForeignKey("dbo.Assets", "purchaseInfo_Id", "dbo.PurchaseInfoes");
            DropForeignKey("dbo.PurchaseInfoes", "currecncy_Id", "dbo.Currencies");
            DropForeignKey("dbo.Assets", "parentId", "dbo.Assets");
            DropForeignKey("dbo.Assets", "companyId", "dbo.tabCompany");
            DropForeignKey("dbo.Assets", "owner_EmpId", "dbo.Employees");
            DropForeignKey("dbo.Assets", "handler_id", "dbo.Users");
            DropForeignKey("dbo.Assets", "designation_DesigId", "dbo.Designations");
            DropForeignKey("dbo.Designations", "userId", "dbo.Users");
            DropForeignKey("dbo.Designations", "ParentId", "dbo.Designations");
            DropForeignKey("dbo.Designations", "departmentId", "dbo.tabDepartment");
            DropForeignKey("dbo.DepartmentVendors", "Vendor_Id", "dbo.tabVendor");
            DropForeignKey("dbo.DepartmentVendors", "Department_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.tabDepartment", "userID", "dbo.Users");
            DropForeignKey("dbo.DepartmentTaskTypes", "TaskType_Id", "dbo.TaskTypes");
            DropForeignKey("dbo.DepartmentTaskTypes", "Department_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.DepartmentTaskGroups1", "TaskGroups_Id", "dbo.TaskGroups");
            DropForeignKey("dbo.DepartmentTaskGroups1", "Department_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.DepartmentTaskGroups", "TaskGroups_Id", "dbo.TaskGroups");
            DropForeignKey("dbo.DepartmentTaskGroups", "Department_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.Targets", "user_Id", "dbo.Users");
            DropForeignKey("dbo.Targets", "typeId", "dbo.TargetTypes");
            DropForeignKey("dbo.TargetTypes", "user_Id", "dbo.Users");
            DropForeignKey("dbo.TargetAwards", "targetId", "dbo.Targets");
            DropForeignKey("dbo.Targets", "departmentId", "dbo.tabDepartment");
            DropForeignKey("dbo.Targets", "currencyId", "dbo.Currencies");
            DropForeignKey("dbo.Targets", "companyId", "dbo.tabCompany");
            DropForeignKey("dbo.SharedGridGroups", "userId", "dbo.Users");
            DropForeignKey("dbo.SharedReports", "sharedGroupId", "dbo.SharedGridGroups");
            DropForeignKey("dbo.SharedReports", "userId", "dbo.Users");
            DropForeignKey("dbo.SharedGridGroups", "parentId", "dbo.SharedGridGroups");
            DropForeignKey("dbo.SharedGridGroupEmployees", "Employee_EmpId", "dbo.Employees");
            DropForeignKey("dbo.SharedGridGroupEmployees", "SharedGridGroup_Id", "dbo.SharedGridGroups");
            DropForeignKey("dbo.SharedGridGroupDepartments", "Department_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.SharedGridGroupDepartments", "SharedGridGroup_Id", "dbo.SharedGridGroups");
            DropForeignKey("dbo.SharedGridGroupCompanies", "Company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.SharedGridGroupCompanies", "SharedGridGroup_Id", "dbo.SharedGridGroups");
            DropForeignKey("dbo.DepartmentSalesReceipts", "SalesReceipt_Id", "dbo.SalesReceipts");
            DropForeignKey("dbo.DepartmentSalesReceipts", "Department_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.DepartmentPayments", "Payment_Id", "dbo.Payments");
            DropForeignKey("dbo.DepartmentPayments", "Department_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.tabDepartment", "ParentID", "dbo.tabDepartment");
            DropForeignKey("dbo.DepartmentLoanApplicants", "LoanApplicant_Id", "dbo.LoanApplicants");
            DropForeignKey("dbo.DepartmentLoanApplicants", "Department_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.SaleOrders", "InterDepartment_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.SaleInvoices", "InterDepartment_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.PurchaseOrders", "InterDepartment_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.PurchaseInvoices", "InterDepartment_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.Payments", "InterDepartment_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.Offers", "InterDepartment_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.ModuleContracts", "InterDepartment_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.MemorandumSales", "InterDepartment_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.Inquiries", "InterDepartment_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.DepartmentEmployees", "Employee_EmpId", "dbo.Employees");
            DropForeignKey("dbo.DepartmentEmployees", "Department_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.DepartmentDocumentTypes", "DocumentType_Id", "dbo.DocumentTypes");
            DropForeignKey("dbo.DepartmentDocumentTypes", "Department_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.DepartmentDocumentTemplates", "DocumentTemplate_Id", "dbo.DocumentTemplates");
            DropForeignKey("dbo.DepartmentDocumentTemplates", "Department_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.SaleOrders", "dept_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.SaleInvoices", "dept_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.PurchaseOrders", "dept_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.PurchaseInvoices", "dept_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.Offers", "dept_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.ModuleContracts", "dept_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.MemorandumSales", "dept_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.tabDepartment", "LevelID", "dbo.DepartmentLevels");
            DropForeignKey("dbo.DepartmentLevels", "ParentID", "dbo.DepartmentLevels");
            DropForeignKey("dbo.Inquiries", "dept_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.Documents", "dept_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.Employees", "coreDeptId", "dbo.tabDepartment");
            DropForeignKey("dbo.DepartmentCompanies", "Company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.DepartmentCompanies", "Department_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.DepartmentChartofAccounts", "ChartofAccount_Id", "dbo.ChartofAccounts");
            DropForeignKey("dbo.DepartmentChartofAccounts", "Department_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.DepartmentChartofAccountGroups", "ChartofAccountGroup_Id", "dbo.ChartofAccountGroups");
            DropForeignKey("dbo.DepartmentChartofAccountGroups", "Department_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.tabDepartment", "chartofAccountId", "dbo.ChartofAccounts");
            DropForeignKey("dbo.CashFlows", "userId", "dbo.Users");
            DropForeignKey("dbo.CashFlows", "titleId", "dbo.ReportTitles");
            DropForeignKey("dbo.GridReportGroups", "userId", "dbo.Users");
            DropForeignKey("dbo.GridReports", "titleId", "dbo.ReportTitles");
            DropForeignKey("dbo.ReportTitles", "userId", "dbo.Users");
            DropForeignKey("dbo.GridReports", "group_Id", "dbo.GridReportGroups");
            DropForeignKey("dbo.GridReports", "userId", "dbo.Users");
            DropForeignKey("dbo.GridReports", "company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.GridReportGroups", "parentId", "dbo.GridReportGroups");
            DropForeignKey("dbo.CashFlows", "groupId", "dbo.GridReportGroups");
            DropForeignKey("dbo.CashFlowDepartments", "Department_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.CashFlowDepartments", "CashFlow_Id", "dbo.CashFlows");
            DropForeignKey("dbo.CashFlowCompanies", "Company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.CashFlowCompanies", "CashFlow_Id", "dbo.CashFlows");
            DropForeignKey("dbo.Assets", "deptId", "dbo.tabDepartment");
            DropForeignKey("dbo.DepartmentAccounts", "Account_Id", "dbo.Accounts");
            DropForeignKey("dbo.DepartmentAccounts", "Department_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.tabDepartment", "accountPayableId", "dbo.ChartofAccounts");
            DropForeignKey("dbo.ChartofAccounts", "userId", "dbo.Users");
            DropForeignKey("dbo.ChartofAccounts", "parentId", "dbo.ChartofAccounts");
            DropForeignKey("dbo.ChartofAccountGroupChartofAccounts", "ChartofAccount_Id", "dbo.ChartofAccounts");
            DropForeignKey("dbo.ChartofAccountGroupChartofAccounts", "ChartofAccountGroup_Id", "dbo.ChartofAccountGroups");
            DropForeignKey("dbo.ChartofAccounts", "currencyId", "dbo.Currencies");
            DropForeignKey("dbo.ChartofAccountCompanies", "Company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.ChartofAccountCompanies", "ChartofAccount_Id", "dbo.ChartofAccounts");
            DropForeignKey("dbo.AdminBillTypes", "user_Id", "dbo.Users");
            DropForeignKey("dbo.tabVendor", "vendorNatureManualId", "dbo.VendorNatureManuals");
            DropForeignKey("dbo.VendorNatureManuals", "user_Id", "dbo.Users");
            DropForeignKey("dbo.tabVendor", "vendorNatureId", "dbo.VendorNatures");
            DropForeignKey("dbo.VendorNatures", "user_Id", "dbo.Users");
            DropForeignKey("dbo.tabVendor", "shippingAddress_Id", "dbo.tabAddress");
            DropForeignKey("dbo.VendorPayees", "Payee_Id", "dbo.Payees");
            DropForeignKey("dbo.VendorPayees", "Vendor_Id", "dbo.tabVendor");
            DropForeignKey("dbo.tabVendor", "ParentID", "dbo.tabVendor");
            DropForeignKey("dbo.tabVendor", "contactPerson_Id", "dbo.tabPerson");
            DropForeignKey("dbo.tabVendor", "contact_Id", "dbo.tabContact");
            DropForeignKey("dbo.tabVendor", "company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.Bills", "WHT_Id", "dbo.TaxNames");
            DropForeignKey("dbo.Bills", "vendorPaymentId", "dbo.VendorPaymentStatus");
            DropForeignKey("dbo.Bills", "vendorBillNature_Id", "dbo.VendorBillNatures");
            DropForeignKey("dbo.VendorBillNatureCompanies", "Company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.VendorBillNatureCompanies", "VendorBillNature_Id", "dbo.VendorBillNatures");
            DropForeignKey("dbo.Bills", "vendor_Id", "dbo.tabVendor");
            DropForeignKey("dbo.Bills", "VATBookRefId", "dbo.VATBookRefNumbers");
            DropForeignKey("dbo.Bills", "user_Id", "dbo.Users");
            DropForeignKey("dbo.Bills", "TitleValue2Id", "dbo.Incoterms");
            DropForeignKey("dbo.Bills", "TitleValue1Id", "dbo.Incoterms");
            DropForeignKey("dbo.Bills", "tax_Id", "dbo.TaxNames");
            DropForeignKey("dbo.Bills", "statusClass_Id", "dbo.StatusClasses");
            DropForeignKey("dbo.Bills", "SOWarrantyId", "dbo.Warranties");
            DropForeignKey("dbo.Bills", "SoPaymentterm_Id", "dbo.PaymentTerms");
            DropForeignKey("dbo.Bills", "SOCurrency_Id", "dbo.Currencies");
            DropForeignKey("dbo.Bills", "purchaseOrder_Id", "dbo.PurchaseOrders");
            DropForeignKey("dbo.ProcurementProducts", "Bill_Id", "dbo.Bills");
            DropForeignKey("dbo.Bills", "POWarrantyId", "dbo.Warranties");
            DropForeignKey("dbo.Bills", "POVendor_Id", "dbo.tabVendor");
            DropForeignKey("dbo.Bills", "POPaymentterm_Id", "dbo.PaymentTerms");
            DropForeignKey("dbo.Bills", "PettyCashRefId", "dbo.BillRefNumbers");
            DropForeignKey("dbo.Payments", "Bill_Id", "dbo.Bills");
            DropForeignKey("dbo.Bills", "managementSummary_Id", "dbo.ManagementSummaries");
            DropForeignKey("dbo.Bills", "LoansAdvanceId", "dbo.LoansAdvances");
            DropForeignKey("dbo.Bills", "loanAdvanceDept_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.Bills", "loanAdvanceCompany_Id", "dbo.tabCompany");
            DropForeignKey("dbo.JournalVouchers", "bill_Id", "dbo.Bills");
            DropForeignKey("dbo.Bills", "InterDepartment_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.Bills", "InterCompany_Id", "dbo.tabCompany");
            DropForeignKey("dbo.InterBankTransfers", "vendorBillId", "dbo.Bills");
            DropForeignKey("dbo.Bills", "incoterm_Id", "dbo.Incoterms");
            DropForeignKey("dbo.Bills", "dept_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.Bills", "customerCompany_Id", "dbo.CustomerCompanies");
            DropForeignKey("dbo.Bills", "currency_Id", "dbo.Currencies");
            DropForeignKey("dbo.Bills", "CreditCardNoId", "dbo.CreditCards");
            DropForeignKey("dbo.Bills", "CostSheet_Id", "dbo.CostSheets");
            DropForeignKey("dbo.Bills", "company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.Bills", "CommissionSummarySheetId", "dbo.CommissionSummarySheets");
            DropForeignKey("dbo.Bills", "CardUserId", "dbo.CardHolders");
            DropForeignKey("dbo.BudgetSystemCostFields", "Bill_Id", "dbo.Bills");
            DropForeignKey("dbo.Bills", "billVendor_Id", "dbo.tabVendor");
            DropForeignKey("dbo.Bills", "billType_Id", "dbo.BillTypes");
            DropForeignKey("dbo.BillTypes", "user_Id", "dbo.Users");
            DropForeignKey("dbo.Bills", "BillRefNoId", "dbo.VendorBillReferences");
            DropForeignKey("dbo.BillItems", "Bill_Id", "dbo.Bills");
            DropForeignKey("dbo.BillItems", "debitAccountId", "dbo.ChartofAccounts");
            DropForeignKey("dbo.BillItems", "creditAccountId", "dbo.ChartofAccounts");
            DropForeignKey("dbo.BillItems", "fieldId", "dbo.CostSheetFields");
            DropForeignKey("dbo.Bills", "billCategoryId", "dbo.BillCategories");
            DropForeignKey("dbo.Bills", "bid_Id", "dbo.Bids");
            DropForeignKey("dbo.Bills", "allocation_Id", "dbo.Employees");
            DropForeignKey("dbo.Adjustments", "vendorBill_Id", "dbo.Bills");
            DropForeignKey("dbo.Adjustments", "adminBillId", "dbo.tabAdminBill");
            DropForeignKey("dbo.tabAdminBill", "vendor_Id", "dbo.tabVendor");
            DropForeignKey("dbo.tabAdminBill", "VATBookRefId", "dbo.VATBookRefNumbers");
            DropForeignKey("dbo.tabAdminBill", "user_Id", "dbo.Users");
            DropForeignKey("dbo.tabAdminBill", "template_Id", "dbo.Templates");
            DropForeignKey("dbo.tabAdminBill", "tax_Id", "dbo.TaxNames");
            DropForeignKey("dbo.tabAdminBill", "statusClass_Id", "dbo.StatusClasses");
            DropForeignKey("dbo.tabAdminBill", "SecondaryCreditCardNoId", "dbo.CreditCards");
            DropForeignKey("dbo.tabAdminBill", "PrimaryCreditCardNoId", "dbo.CreditCards");
            DropForeignKey("dbo.Payments", "AdminBill_Id", "dbo.tabAdminBill");
            DropForeignKey("dbo.tabAdminBill", "payee_Id", "dbo.Payees");
            DropForeignKey("dbo.tabAdminBill", "managementSummary_Id", "dbo.ManagementSummaries");
            DropForeignKey("dbo.ManagementSummaries", "ParentId", "dbo.ManagementSummaries");
            DropForeignKey("dbo.tabAdminBill", "LoansAdvanceId", "dbo.LoansAdvances");
            DropForeignKey("dbo.VehicleExpenses", "payeeId", "dbo.Payees");
            DropForeignKey("dbo.VehicleExpenses", "maintenanceHeadId", "dbo.MaintenanceHeads");
            DropForeignKey("dbo.VehicleExpenses", "AdminBillForVehicleExpenses_Id", "dbo.tabAdminBill");
            DropForeignKey("dbo.VehicleExpenses", "AdminBillForFuelExpenses_Id", "dbo.tabAdminBill");
            DropForeignKey("dbo.tabAdminBill", "employeeForBill_Id", "dbo.Employees");
            DropForeignKey("dbo.tabAdminBill", "emp_Id", "dbo.Employees");
            DropForeignKey("dbo.tabAdminBill", "dept_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.tabAdminBill", "currency_Id", "dbo.Currencies");
            DropForeignKey("dbo.tabAdminBill", "creatorId", "dbo.Employees");
            DropForeignKey("dbo.tabAdminBill", "company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.tabAdminBill", "CoaCredit_Id", "dbo.ChartofAccounts");
            DropForeignKey("dbo.tabAdminBill", "COA_Id", "dbo.ChartofAccounts");
            DropForeignKey("dbo.tabAdminBill", "CardUserId", "dbo.CardHolders");
            DropForeignKey("dbo.tabAdminBill", "BillRefNoId", "dbo.BillRefNumbers");
            DropForeignKey("dbo.tabAdminBill", "BankId", "dbo.Banks");
            DropForeignKey("dbo.AssetRentals", "vendorId", "dbo.tabVendor");
            DropForeignKey("dbo.RentalContracts", "assetRentalId", "dbo.AssetRentals");
            DropForeignKey("dbo.AssetRentals", "parentId", "dbo.AssetRentals");
            DropForeignKey("dbo.AssetRentals", "deptId", "dbo.tabDepartment");
            DropForeignKey("dbo.AssetRentals", "creatorId", "dbo.Users");
            DropForeignKey("dbo.UserSettings", "userId", "dbo.Users");
            DropForeignKey("dbo.ReportUsers", "User_id", "dbo.Users");
            DropForeignKey("dbo.ReportUsers", "Report_Id", "dbo.Reports");
            DropForeignKey("dbo.ReportGroups", "user_Id", "dbo.Users");
            DropForeignKey("dbo.Reports", "groupId", "dbo.ReportGroups");
            DropForeignKey("dbo.ReportGroups", "parentId", "dbo.ReportGroups");
            DropForeignKey("dbo.Tasks", "warehouseId", "dbo.Warehouses");
            DropForeignKey("dbo.Tasks", "vendorId", "dbo.tabVendor");
            DropForeignKey("dbo.Tasks", "taskTypeId", "dbo.TaskTypes");
            DropForeignKey("dbo.TaskTrackings", "UpdatedById", "dbo.Users");
            DropForeignKey("dbo.TaskTrackings", "tasksId", "dbo.Tasks");
            DropForeignKey("dbo.TaskEfficiencies", "tasksId", "dbo.Tasks");
            DropForeignKey("dbo.TaskEfficiencies", "efficiencyPoints_Id", "dbo.EfficiencyPoints");
            DropForeignKey("dbo.TaskComments", "userId", "dbo.Users");
            DropForeignKey("dbo.TaskComments", "taskId", "dbo.Tasks");
            DropForeignKey("dbo.TaskComments", "goodReceiveNoteId", "dbo.GoodReceiveNotes");
            DropForeignKey("dbo.Tasks", "supervisedById", "dbo.Users");
            DropForeignKey("dbo.Tasks", "statusClass_Id", "dbo.StatusClasses");
            DropForeignKey("dbo.Tasks", "saleOrderId", "dbo.SaleOrders");
            DropForeignKey("dbo.Tasks", "saleInvoiceId", "dbo.SaleInvoices");
            DropForeignKey("dbo.Tasks", "purchaseOrderId", "dbo.PurchaseOrders");
            DropForeignKey("dbo.Tasks", "offerId", "dbo.Offers");
            DropForeignKey("dbo.Tasks", "moduleContractId", "dbo.ModuleContracts");
            DropForeignKey("dbo.Tasks", "lotNumberId", "dbo.LotNumbers");
            DropForeignKey("dbo.Tasks", "inquiryId", "dbo.Inquiries");
            DropForeignKey("dbo.Tasks", "employeeId", "dbo.Employees");
            DropForeignKey("dbo.Tasks", "deptId", "dbo.tabDepartment");
            DropForeignKey("dbo.Tasks", "customerId", "dbo.CustomerCompanies");
            DropForeignKey("dbo.Tasks", "currencyId", "dbo.Currencies");
            DropForeignKey("dbo.Tasks", "creatorId", "dbo.Users");
            DropForeignKey("dbo.Tasks", "companyId", "dbo.tabCompany");
            DropForeignKey("dbo.Tasks", "checklistId", "dbo.Checklists");
            DropForeignKey("dbo.ProcurementProducts", "Checklist_Id", "dbo.Checklists");
            DropForeignKey("dbo.ProcurementProducts", "packingStyleId", "dbo.PackingStyles");
            DropForeignKey("dbo.ProcurementProducts", "product_Id", "dbo.InquiryProducts");
            DropForeignKey("dbo.ProcurementProducts", "debitAccountId", "dbo.ChartofAccounts");
            DropForeignKey("dbo.ProcurementProducts", "creditAccountId", "dbo.ChartofAccounts");
            DropForeignKey("dbo.ProcurementProducts", "fieldId", "dbo.CostSheetFields");
            DropForeignKey("dbo.JournalTransactions", "userId", "dbo.Users");
            DropForeignKey("dbo.JournalTransactions", "ReconcilationId", "dbo.Reconcilations");
            DropForeignKey("dbo.Reconcilations", "chartofAccountId", "dbo.ChartofAccounts");
            DropForeignKey("dbo.InterBankTransfers", "vendor_Id", "dbo.tabVendor");
            DropForeignKey("dbo.InterBankTransfers", "VATBookRefId", "dbo.VATBookRefNumbers");
            DropForeignKey("dbo.InterBankTransfers", "user_Id", "dbo.Users");
            DropForeignKey("dbo.InterBankTransfers", "transferMethod_Id", "dbo.TranferMethods");
            DropForeignKey("dbo.InterBankTransfers", "taxTypeId", "dbo.TaxTypes");
            DropForeignKey("dbo.InterBankTransfers", "taxNameId", "dbo.TaxNames");
            DropForeignKey("dbo.InterBankTransfers", "statusClass_Id", "dbo.StatusClasses");
            DropForeignKey("dbo.ProcurementProducts", "interBankTransferId", "dbo.InterBankTransfers");
            DropForeignKey("dbo.InterBankTransfers", "PettyCashRefId", "dbo.BillRefNumbers");
            DropForeignKey("dbo.JournalTransactions", "InterBankId", "dbo.InterBankTransfers");
            DropForeignKey("dbo.InterBankTransfers", "statusId", "dbo.InterBankTransferStatus");
            DropForeignKey("dbo.InterBankTransferStatusStatusClasses", "StatusClass_Id", "dbo.StatusClasses");
            DropForeignKey("dbo.InterBankTransferStatusStatusClasses", "InterBankTransferStatus_Id", "dbo.InterBankTransferStatus");
            DropForeignKey("dbo.StatusClassToDoTaskStatus", "ToDoTaskStatus_Id", "dbo.ToDoTaskStatus");
            DropForeignKey("dbo.StatusClassToDoTaskStatus", "StatusClass_Id", "dbo.StatusClasses");
            DropForeignKey("dbo.StatusClassTenantRentalStatus", "TenantRentalStatus_Id", "dbo.TenantRentalStatus");
            DropForeignKey("dbo.StatusClassTenantRentalStatus", "StatusClass_Id", "dbo.StatusClasses");
            DropForeignKey("dbo.StatusClassTasksStatus", "TasksStatus_Id", "dbo.TasksStatus");
            DropForeignKey("dbo.StatusClassTasksStatus", "StatusClass_Id", "dbo.StatusClasses");
            DropForeignKey("dbo.TasksStatusTaskTypes", "TaskType_Id", "dbo.TaskTypes");
            DropForeignKey("dbo.TasksStatusTaskTypes", "TasksStatus_Id", "dbo.TasksStatus");
            DropForeignKey("dbo.Tasks", "statusId", "dbo.TasksStatus");
            DropForeignKey("dbo.StatusClassTargetRewardStatus", "TargetRewardStatus_Id", "dbo.TargetRewardStatus");
            DropForeignKey("dbo.StatusClassTargetRewardStatus", "StatusClass_Id", "dbo.StatusClasses");
            DropForeignKey("dbo.LoansStatusStatusClasses", "StatusClass_Id", "dbo.StatusClasses");
            DropForeignKey("dbo.LoansStatusStatusClasses", "LoansStatus_Id", "dbo.LoansStatus");
            DropForeignKey("dbo.tabLoan", "user_Id", "dbo.Users");
            DropForeignKey("dbo.tabLoan", "SubLimitNatureId", "dbo.FacilityNatures");
            DropForeignKey("dbo.tabLoan", "statusClass_Id", "dbo.StatusClasses");
            DropForeignKey("dbo.tabLoan", "statusId", "dbo.LoansStatus");
            DropForeignKey("dbo.tabLoan", "MainLimitNatureId", "dbo.FacilityNatures");
            DropForeignKey("dbo.tabLoan", "deptId", "dbo.tabDepartment");
            DropForeignKey("dbo.tabLoan", "currencyId", "dbo.Currencies");
            DropForeignKey("dbo.tabLoan", "creatorId", "dbo.Employees");
            DropForeignKey("dbo.tabLoan", "CompanyId", "dbo.tabCompany");
            DropForeignKey("dbo.tabLoan", "bankId", "dbo.Banks");
            DropForeignKey("dbo.tabLoan", "accountId", "dbo.Accounts");
            DropForeignKey("dbo.InquiryStatusStatusClasses", "StatusClass_Id", "dbo.StatusClasses");
            DropForeignKey("dbo.InquiryStatusStatusClasses", "InquiryStatus_Id", "dbo.InquiryStatus");
            DropForeignKey("dbo.Inquiries", "user_Id", "dbo.Users");
            DropForeignKey("dbo.Inquiries", "statusClass_Id", "dbo.StatusClasses");
            DropForeignKey("dbo.InquiryProducts", "Inquiry_Id", "dbo.Inquiries");
            DropForeignKey("dbo.InquiryProducts", "product_Id", "dbo.Products");
            DropForeignKey("dbo.Products", "user_Id", "dbo.Users");
            DropForeignKey("dbo.Products", "unitOfMeasureId", "dbo.UnitOfMeasures");
            DropForeignKey("dbo.UnitOfMeasures", "user_Id", "dbo.Users");
            DropForeignKey("dbo.Products", "parentId", "dbo.Products");
            DropForeignKey("dbo.Products", "nature_Id", "dbo.ProductNatures");
            DropForeignKey("dbo.ProductNatures", "user_Id", "dbo.Users");
            DropForeignKey("dbo.JournalTransactions", "prodId", "dbo.Products");
            DropForeignKey("dbo.Products", "incomeAccount_id", "dbo.ChartofAccounts");
            DropForeignKey("dbo.ProductDepartments", "Department_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.ProductDepartments", "Product_Id", "dbo.Products");
            DropForeignKey("dbo.Products", "company_id", "dbo.tabCompany");
            DropForeignKey("dbo.Products", "cgsInvenAccount_id", "dbo.ChartofAccounts");
            DropForeignKey("dbo.Products", "cgsAccount_id", "dbo.ChartofAccounts");
            DropForeignKey("dbo.ProductCategories", "user_Id", "dbo.Users");
            DropForeignKey("dbo.Products", "categoryId", "dbo.ProductCategories");
            DropForeignKey("dbo.ProductCategories", "parentId", "dbo.ProductCategories");
            DropForeignKey("dbo.Products", "cAssetAccount_id", "dbo.ChartofAccounts");
            DropForeignKey("dbo.BookerStatementItems", "taxNameId", "dbo.TaxNames");
            DropForeignKey("dbo.BookerStatementItems", "product_Id", "dbo.Products");
            DropForeignKey("dbo.BookerStatementItems", "passOnId", "dbo.PassOns");
            DropForeignKey("dbo.PassOns", "chartofAccountId", "dbo.ChartofAccounts");
            DropForeignKey("dbo.ModuleContractVendors", "Vendor_Id", "dbo.tabVendor");
            DropForeignKey("dbo.ModuleContractVendors", "ModuleContract_Id", "dbo.ModuleContracts");
            DropForeignKey("dbo.ModuleContracts", "user_Id", "dbo.Users");
            DropForeignKey("dbo.ModuleContracts", "transactionHolderId", "dbo.Employees");
            DropForeignKey("dbo.ModuleContracts", "TitleValue2Id", "dbo.Incoterms");
            DropForeignKey("dbo.ModuleContracts", "TitleValue1Id", "dbo.Incoterms");
            DropForeignKey("dbo.ModuleContracts", "statusClass_Id", "dbo.StatusClasses");
            DropForeignKey("dbo.ProcurementProducts", "ModuleContract_Id", "dbo.ModuleContracts");
            DropForeignKey("dbo.ModuleContracts", "principal_Id", "dbo.Principals");
            DropForeignKey("dbo.ModuleContracts", "paymentterm_Id", "dbo.PaymentTerms");
            DropForeignKey("dbo.ModuleContracts", "offer_Id", "dbo.Offers");
            DropForeignKey("dbo.ModuleContractStatusStatusClasses", "StatusClass_Id", "dbo.StatusClasses");
            DropForeignKey("dbo.ModuleContractStatusStatusClasses", "ModuleContractStatus_Id", "dbo.ModuleContractStatus");
            DropForeignKey("dbo.ModuleContracts", "ModuleContractStatus_Id", "dbo.ModuleContractStatus");
            DropForeignKey("dbo.MemorandumSales", "ModuleContract_Id", "dbo.ModuleContracts");
            DropForeignKey("dbo.ModuleContracts", "incoterm_Id", "dbo.Incoterms");
            DropForeignKey("dbo.ModuleContracts", "allocation_Id", "dbo.Employees");
            DropForeignKey("dbo.ModuleContracts", "customerCompany_Id", "dbo.CustomerCompanies");
            DropForeignKey("dbo.ModuleContracts", "currency_Id", "dbo.Currencies");
            DropForeignKey("dbo.ComparativeStatements", "ModuleContract_Id", "dbo.ModuleContracts");
            DropForeignKey("dbo.ComparativeStatements", "VendorId", "dbo.tabVendor");
            DropForeignKey("dbo.ComparativeStatements", "productId", "dbo.ProcurementProducts");
            DropForeignKey("dbo.ComparativeStatements", "offerCurrencyId", "dbo.Currencies");
            DropForeignKey("dbo.ComparativeStatements", "vendorIncoTermId", "dbo.Incoterms");
            DropForeignKey("dbo.ComparativeStatements", "incoTerm_Id", "dbo.Incoterms");
            DropForeignKey("dbo.Incoterms", "user_Id", "dbo.Users");
            DropForeignKey("dbo.PurchaseOrders", "TitleValue2Id", "dbo.Incoterms");
            DropForeignKey("dbo.PurchaseOrders", "TitleValue1Id", "dbo.Incoterms");
            DropForeignKey("dbo.PurchaseOrders", "incoterm_Id", "dbo.Incoterms");
            DropForeignKey("dbo.PurchaseOrders", "WHT_Id", "dbo.TaxNames");
            DropForeignKey("dbo.PurchaseOrderVendors", "Vendor_Id", "dbo.tabVendor");
            DropForeignKey("dbo.PurchaseOrderVendors", "PurchaseOrder_Id", "dbo.PurchaseOrders");
            DropForeignKey("dbo.PurchaseOrders", "vendorPaymentId", "dbo.VendorPaymentStatus");
            DropForeignKey("dbo.PurchaseOrders", "user_Id", "dbo.Users");
            DropForeignKey("dbo.PurchaseOrders", "tax_Id", "dbo.TaxNames");
            DropForeignKey("dbo.PurchaseOrders", "statusClass_Id", "dbo.StatusClasses");
            DropForeignKey("dbo.PurchaseOrders", "SOCurrency_Id", "dbo.Currencies");
            DropForeignKey("dbo.PurchaseOrders", "PurchaseOrderStatus_Id", "dbo.PurchaseOrderStatus");
            DropForeignKey("dbo.PurchaseOrderStatusStatusClasses", "StatusClass_Id", "dbo.StatusClasses");
            DropForeignKey("dbo.PurchaseOrderStatusStatusClasses", "PurchaseOrderStatus_Id", "dbo.PurchaseOrderStatus");
            DropForeignKey("dbo.ProcurementProducts", "PurchaseOrder_Id", "dbo.PurchaseOrders");
            DropForeignKey("dbo.PurchaseOrders", "lotNumberId", "dbo.LotNumbers");
            DropForeignKey("dbo.LoansAdvances", "vendorId", "dbo.tabVendor");
            DropForeignKey("dbo.LoansAdvances", "VATBookRefId", "dbo.VATBookRefNumbers");
            DropForeignKey("dbo.LoansAdvances", "statusClass_Id", "dbo.StatusClasses");
            DropForeignKey("dbo.LoansAdvances", "statusId", "dbo.LoansAdvanceStatus");
            DropForeignKey("dbo.LoansAdvanceStatusStatusClasses", "StatusClass_Id", "dbo.StatusClasses");
            DropForeignKey("dbo.LoansAdvanceStatusStatusClasses", "LoansAdvanceStatus_Id", "dbo.LoansAdvanceStatus");
            DropForeignKey("dbo.SalesReceipts", "LoansAdvanceId", "dbo.LoansAdvances");
            DropForeignKey("dbo.LoansAdvances", "saleOrderId", "dbo.SaleOrders");
            DropForeignKey("dbo.LoansAdvances", "SaleInvoiceId", "dbo.SaleInvoices");
            DropForeignKey("dbo.LoansAdvances", "purchaseOrderId", "dbo.PurchaseOrders");
            DropForeignKey("dbo.LoansAdvances", "PettyCashRefId", "dbo.BillRefNumbers");
            DropForeignKey("dbo.Payments", "LoansAdvanceId", "dbo.LoansAdvances");
            DropForeignKey("dbo.Payments", "vendor_Id", "dbo.tabVendor");
            DropForeignKey("dbo.Payments", "VATBookRefId", "dbo.VATBookRefNumbers");
            DropForeignKey("dbo.Payments", "user_Id", "dbo.Users");
            DropForeignKey("dbo.Payments", "taskGroups_Id", "dbo.TaskGroups");
            DropForeignKey("dbo.Payments", "TargetReward_Id", "dbo.TargetRewards");
            DropForeignKey("dbo.TargetRewards", "user_Id", "dbo.Users");
            DropForeignKey("dbo.TargetRewards", "toDoTask_Id", "dbo.ToDoTasks");
            DropForeignKey("dbo.ToDoTasks", "targetTypeId", "dbo.TaskTargetTypes");
            DropForeignKey("dbo.ToDoTasks", "taskGroupId", "dbo.TaskGroups");
            DropForeignKey("dbo.ToDoTasks", "TaskCreatorId", "dbo.Users");
            DropForeignKey("dbo.ToDoTasks", "targetGroupId", "dbo.TargetGroups");
            DropForeignKey("dbo.ToDoTasks", "supervisedById", "dbo.Users");
            DropForeignKey("dbo.ToDoTasks", "statusClass_Id", "dbo.StatusClasses");
            DropForeignKey("dbo.ToDoTasks", "statusId", "dbo.ToDoTaskStatus");
            DropForeignKey("dbo.ToDoTasks", "salesHeadId", "dbo.Users");
            DropForeignKey("dbo.ToDoTasks", "parentTaskId", "dbo.ToDoTasks");
            DropForeignKey("dbo.ToDoTasks", "CalculationType_Id", "dbo.StatusCalculationTypes");
            DropForeignKey("dbo.ToDoTaskUsers", "User_id", "dbo.Users");
            DropForeignKey("dbo.ToDoTaskUsers", "ToDoTask_Id", "dbo.ToDoTasks");
            DropForeignKey("dbo.TargetRewards", "targetRewardNatureId", "dbo.TargetRewardNatures");
            DropForeignKey("dbo.TargetRewards", "statusClass_Id", "dbo.StatusClasses");
            DropForeignKey("dbo.TargetRewards", "statusId", "dbo.TargetRewardStatus");
            DropForeignKey("dbo.JournalTransactions", "TargetRewardId", "dbo.TargetRewards");
            DropForeignKey("dbo.TargetRewards", "currencyId", "dbo.Currencies");
            DropForeignKey("dbo.TargetRewards", "creator_Id", "dbo.Users");
            DropForeignKey("dbo.Payments", "statusClass_Id", "dbo.StatusClasses");
            DropForeignKey("dbo.PaymentStatusStatusClasses", "StatusClass_Id", "dbo.StatusClasses");
            DropForeignKey("dbo.PaymentStatusStatusClasses", "PaymentStatus_Id", "dbo.PaymentStatus");
            DropForeignKey("dbo.Payments", "statusId", "dbo.PaymentStatus");
            DropForeignKey("dbo.SalesReceipts", "paymentId", "dbo.Payments");
            DropForeignKey("dbo.Payments", "PInvoice_Id", "dbo.PurchaseInvoices");
            DropForeignKey("dbo.Payments", "PrimaryCreditCardNoId", "dbo.CreditCards");
            DropForeignKey("dbo.CreditCards", "SecondaryCardHolderId", "dbo.CardHolders");
            DropForeignKey("dbo.CreditCards", "PrimaryCardNoId", "dbo.CreditCards");
            DropForeignKey("dbo.CreditCards", "PrimaryCardHolderId", "dbo.CardHolders");
            DropForeignKey("dbo.CreditCardDepartments", "Department_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.CreditCardDepartments", "CreditCard_Id", "dbo.CreditCards");
            DropForeignKey("dbo.CreditCards", "currencyId", "dbo.Currencies");
            DropForeignKey("dbo.CreditCards", "creditCardTypeId", "dbo.CreditCardTypes");
            DropForeignKey("dbo.CreditCards", "CompanyId", "dbo.tabCompany");
            DropForeignKey("dbo.CreditCards", "CardUserID", "dbo.CardHolders");
            DropForeignKey("dbo.CreditCards", "BankID", "dbo.Banks");
            DropForeignKey("dbo.PettyCashes", "PettyCashRefId", "dbo.BillRefNumbers");
            DropForeignKey("dbo.PettyCashes", "PaymentId", "dbo.Payments");
            DropForeignKey("dbo.PettyCashes", "currencyId", "dbo.Currencies");
            DropForeignKey("dbo.PettyCashes", "LoansAdvanceId", "dbo.LoansAdvances");
            DropForeignKey("dbo.PettyCashes", "InterCompanyId", "dbo.InterCompanyBankTransfers");
            DropForeignKey("dbo.VATBooks", "vendorBillId", "dbo.Bills");
            DropForeignKey("dbo.VATBooks", "vendorId", "dbo.tabVendor");
            DropForeignKey("dbo.VATBooks", "VATBookRefNumberRefId", "dbo.VATBookRefNumbers");
            DropForeignKey("dbo.PurchaseInvoiceVendors", "Vendor_Id", "dbo.tabVendor");
            DropForeignKey("dbo.PurchaseInvoiceVendors", "PurchaseInvoice_Id", "dbo.PurchaseInvoices");
            DropForeignKey("dbo.PurchaseInvoices", "vendorPaymentId", "dbo.VendorPaymentStatus");
            DropForeignKey("dbo.PurchaseInvoices", "vendor_Id", "dbo.tabVendor");
            DropForeignKey("dbo.VATBooks", "purchaseInvoiceId", "dbo.PurchaseInvoices");
            DropForeignKey("dbo.PurchaseInvoices", "VATBookRefId", "dbo.VATBookRefNumbers");
            DropForeignKey("dbo.PurchaseInvoices", "user_Id", "dbo.Users");
            DropForeignKey("dbo.PurchaseInvoices", "TitleValue2Id", "dbo.Incoterms");
            DropForeignKey("dbo.PurchaseInvoices", "TitleValue1Id", "dbo.Incoterms");
            DropForeignKey("dbo.PurchaseInvoices", "tax_Id", "dbo.TaxNames");
            DropForeignKey("dbo.PurchaseInvoices", "statusClass_Id", "dbo.StatusClasses");
            DropForeignKey("dbo.PurchaseInvoices", "purchaseOrder_Id", "dbo.PurchaseOrders");
            DropForeignKey("dbo.PurchaseInvoices", "PurchaseInvoiceStatus_Id", "dbo.PurchaseInvoiceStatus");
            DropForeignKey("dbo.PurchaseInvoiceStatusStatusClasses", "StatusClass_Id", "dbo.StatusClasses");
            DropForeignKey("dbo.PurchaseInvoiceStatusStatusClasses", "PurchaseInvoiceStatus_Id", "dbo.PurchaseInvoiceStatus");
            DropForeignKey("dbo.ProcurementProducts", "PurchaseInvoice_Id", "dbo.PurchaseInvoices");
            DropForeignKey("dbo.PurchaseInvoices", "POPaymentterm_Id", "dbo.PaymentTerms");
            DropForeignKey("dbo.PurchaseInvoices", "incoterm_Id", "dbo.PaymentTerms");
            DropForeignKey("dbo.JournalTransactions", "PurchaseInvoiceId", "dbo.PurchaseInvoices");
            DropForeignKey("dbo.SaleInvoiceVendors", "Vendor_Id", "dbo.tabVendor");
            DropForeignKey("dbo.SaleInvoiceVendors", "SaleInvoice_Id", "dbo.SaleInvoices");
            DropForeignKey("dbo.VATBooks", "saleInvoiceId", "dbo.SaleInvoices");
            DropForeignKey("dbo.SaleInvoices", "VATBookRefId", "dbo.VATBookRefNumbers");
            DropForeignKey("dbo.SaleInvoices", "user_Id", "dbo.Users");
            DropForeignKey("dbo.SaleInvoices", "TitleValue2Id", "dbo.Incoterms");
            DropForeignKey("dbo.SaleInvoices", "TitleValue1Id", "dbo.Incoterms");
            DropForeignKey("dbo.SaleInvoices", "stlDiscountCurrency_Id", "dbo.Currencies");
            DropForeignKey("dbo.SaleInvoices", "stlSTLCurrency_Id", "dbo.Currencies");
            DropForeignKey("dbo.SaleInvoices", "statusClass_Id", "dbo.StatusClasses");
            DropForeignKey("dbo.SalesReceipts", "saleInvoiceId", "dbo.SaleInvoices");
            DropForeignKey("dbo.VATBooks", "saleReceiptId", "dbo.SalesReceipts");
            DropForeignKey("dbo.SalesReceipts", "VATBookRefId", "dbo.VATBookRefNumbers");
            DropForeignKey("dbo.SalesReceipts", "user_Id", "dbo.Users");
            DropForeignKey("dbo.SalesReceipts", "statusClass_Id", "dbo.StatusClasses");
            DropForeignKey("dbo.SalesReceipts", "receiptId", "dbo.SalesReceipts");
            DropForeignKey("dbo.SalesReceiptStatusStatusClasses", "StatusClass_Id", "dbo.StatusClasses");
            DropForeignKey("dbo.SalesReceiptStatusStatusClasses", "SalesReceiptStatus_Id", "dbo.SalesReceiptStatus");
            DropForeignKey("dbo.SalesReceipts", "StatusId", "dbo.SalesReceiptStatus");
            DropForeignKey("dbo.RentalInvoices", "TenantRentalId", "dbo.TenantRentals");
            DropForeignKey("dbo.RentalInvoiceStatusStatusClasses", "StatusClass_Id", "dbo.StatusClasses");
            DropForeignKey("dbo.RentalInvoiceStatusStatusClasses", "RentalInvoiceStatus_Id", "dbo.RentalInvoiceStatus");
            DropForeignKey("dbo.RentalInvoices", "statusId", "dbo.RentalInvoiceStatus");
            DropForeignKey("dbo.SalesReceipts", "rentalInvoiceId", "dbo.RentalInvoices");
            DropForeignKey("dbo.RentalInvoices", "rentalOrderId", "dbo.RentalOrders");
            DropForeignKey("dbo.RentalOrders", "TenantRentalId", "dbo.TenantRentals");
            DropForeignKey("dbo.RentalOrderStatusStatusClasses", "StatusClass_Id", "dbo.StatusClasses");
            DropForeignKey("dbo.RentalOrderStatusStatusClasses", "RentalOrderStatus_Id", "dbo.RentalOrderStatus");
            DropForeignKey("dbo.RentalOrders", "statusId", "dbo.RentalOrderStatus");
            DropForeignKey("dbo.TenantRentals", "statusId", "dbo.TenantRentalStatus");
            DropForeignKey("dbo.RentalContracts", "tenantRentalId", "dbo.TenantRentals");
            DropForeignKey("dbo.TenantRentals", "personId", "dbo.tabPerson");
            DropForeignKey("dbo.TenantRentals", "deptId", "dbo.tabDepartment");
            DropForeignKey("dbo.TenantRentals", "creatorId", "dbo.Users");
            DropForeignKey("dbo.TenantRentals", "contactId", "dbo.tabContact");
            DropForeignKey("dbo.TenantRentals", "companyId", "dbo.tabCompany");
            DropForeignKey("dbo.TenantRentals", "assetRentalId", "dbo.AssetRentals");
            DropForeignKey("dbo.TenantRentals", "addressId", "dbo.tabAddress");
            DropForeignKey("dbo.RentalContractStatusStatusClasses", "StatusClass_Id", "dbo.StatusClasses");
            DropForeignKey("dbo.RentalContractStatusStatusClasses", "RentalContractStatus_Id", "dbo.RentalContractStatus");
            DropForeignKey("dbo.RentalContracts", "statusId", "dbo.RentalContractStatus");
            DropForeignKey("dbo.RentalOrders", "rentalContractId", "dbo.RentalContracts");
            DropForeignKey("dbo.RentalContracts", "deptId", "dbo.tabDepartment");
            DropForeignKey("dbo.RentalContracts", "currencyId", "dbo.Currencies");
            DropForeignKey("dbo.RentalContracts", "creatorId", "dbo.Users");
            DropForeignKey("dbo.RentalContracts", "companyId", "dbo.tabCompany");
            DropForeignKey("dbo.RentalOrders", "deptId", "dbo.tabDepartment");
            DropForeignKey("dbo.RentalOrders", "currencyId", "dbo.Currencies");
            DropForeignKey("dbo.RentalOrders", "creatorId", "dbo.Users");
            DropForeignKey("dbo.RentalOrders", "companyId", "dbo.tabCompany");
            DropForeignKey("dbo.RentalOrders", "assetRentalId", "dbo.AssetRentals");
            DropForeignKey("dbo.RentalInvoices", "deptId", "dbo.tabDepartment");
            DropForeignKey("dbo.RentalInvoices", "currencyId", "dbo.Currencies");
            DropForeignKey("dbo.RentalInvoices", "creatorId", "dbo.Users");
            DropForeignKey("dbo.RentalInvoices", "companyId", "dbo.tabCompany");
            DropForeignKey("dbo.RentalInvoices", "assetRentalId", "dbo.AssetRentals");
            DropForeignKey("dbo.ReceiptTaxes", "taxNameId", "dbo.TaxNames");
            DropForeignKey("dbo.ReceiptTaxes", "SalesReceiptForDeductionTaxId", "dbo.SalesReceipts");
            DropForeignKey("dbo.ReceiptTaxes", "SalesReceiptForBankChargesTaxId", "dbo.SalesReceipts");
            DropForeignKey("dbo.SalesReceipts", "principal_Id", "dbo.Principals");
            DropForeignKey("dbo.SalesReceipts", "PettyCashRefId", "dbo.BillRefNumbers");
            DropForeignKey("dbo.PettyCashes", "SaleReceiptId", "dbo.SalesReceipts");
            DropForeignKey("dbo.JournalTransactions", "SaleReceiptId", "dbo.SalesReceipts");
            DropForeignKey("dbo.SalesReceipts", "deptId", "dbo.tabDepartment");
            DropForeignKey("dbo.SalesReceipts", "CustomerId", "dbo.CustomerCompanies");
            DropForeignKey("dbo.SalesReceipts", "CurrencyId", "dbo.Currencies");
            DropForeignKey("dbo.SalesReceipts", "CostSheet_Id", "dbo.CostSheets");
            DropForeignKey("dbo.SalesReceipts", "companyId", "dbo.tabCompany");
            DropForeignKey("dbo.SalesReceipts", "collectionMethodId", "dbo.CollectionMethods");
            DropForeignKey("dbo.SalesReceipts", "COAcredit_Id", "dbo.ChartofAccounts");
            DropForeignKey("dbo.SalesReceipts", "coaAccountId", "dbo.ChartofAccounts");
            DropForeignKey("dbo.BudgetSystemCostFields", "SalesReceipt_Id", "dbo.SalesReceipts");
            DropForeignKey("dbo.ReceiptDeductions", "SalesReceiptForDeductionId", "dbo.SalesReceipts");
            DropForeignKey("dbo.ReceiptDeductions", "SalesReceiptForBankChargesId", "dbo.SalesReceipts");
            DropForeignKey("dbo.ReceiptDeductions", "deduction_Id", "dbo.Deductions");
            DropForeignKey("dbo.SalesReceipts", "BankId", "dbo.Banks");
            DropForeignKey("dbo.SalesReceipts", "AccountId", "dbo.Accounts");
            DropForeignKey("dbo.SaleOrders", "WarrantyId", "dbo.Warranties");
            DropForeignKey("dbo.SaleOrderVendors", "Vendor_Id", "dbo.tabVendor");
            DropForeignKey("dbo.SaleOrderVendors", "SaleOrder_Id", "dbo.SaleOrders");
            DropForeignKey("dbo.SaleOrders", "vendorPaymentId", "dbo.VendorPaymentStatus");
            DropForeignKey("dbo.SaleOrders", "user_Id", "dbo.Users");
            DropForeignKey("dbo.SaleOrders", "TitleValue2Id", "dbo.Incoterms");
            DropForeignKey("dbo.SaleOrders", "TitleValue1Id", "dbo.Incoterms");
            DropForeignKey("dbo.SaleOrders", "taxNameId", "dbo.TaxNames");
            DropForeignKey("dbo.SaleOrders", "statusClass_Id", "dbo.StatusClasses");
            DropForeignKey("dbo.SplitPERs", "saleOrderId", "dbo.SaleOrders");
            DropForeignKey("dbo.SaleOrderStatusStatusClasses", "StatusClass_Id", "dbo.StatusClasses");
            DropForeignKey("dbo.SaleOrderStatusStatusClasses", "SaleOrderStatus_Id", "dbo.SaleOrderStatus");
            DropForeignKey("dbo.SaleOrders", "saleOrderStatus_Id", "dbo.SaleOrderStatus");
            DropForeignKey("dbo.SaleInvoices", "SaleOrderId", "dbo.SaleOrders");
            DropForeignKey("dbo.SaleOrders", "saleExchangerateId", "dbo.SalesExchangeRates");
            DropForeignKey("dbo.SaleOrders", "SaleOrderKey_Id", "dbo.SaleOrdeRrefKeys");
            DropForeignKey("dbo.SaleOrdeRrefKeys", "dept_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.SaleOrdeRrefKeys", "comp_Id", "dbo.tabCompany");
            DropForeignKey("dbo.PurchaseOrders", "saleOrder_Id", "dbo.SaleOrders");
            DropForeignKey("dbo.ProcurementProducts", "SaleOrder_Id", "dbo.SaleOrders");
            DropForeignKey("dbo.SaleOrders", "principal_Id", "dbo.Principals");
            DropForeignKey("dbo.SaleOrders", "PerformanceSheet_Id", "dbo.PerformanceSheets");
            DropForeignKey("dbo.PerformanceSheets", "supervoisedId", "dbo.Employees");
            DropForeignKey("dbo.PerformanceSheets", "staffLevelTwoId", "dbo.Employees");
            DropForeignKey("dbo.PerformanceSheets", "staffLevelOneId", "dbo.Employees");
            DropForeignKey("dbo.PerfomarmanceSheetFields", "PerformanceSheet_Id", "dbo.PerformanceSheets");
            DropForeignKey("dbo.PerfomarmanceSheetFields", "Head_Id", "dbo.PerformanceSheetHeads");
            DropForeignKey("dbo.PerformanceSheetHeads", "creatorId", "dbo.Users");
            DropForeignKey("dbo.PerformanceSheets", "deptId", "dbo.tabDepartment");
            DropForeignKey("dbo.PerformanceSheets", "customerId", "dbo.CustomerCompanies");
            DropForeignKey("dbo.SaleOrders", "paymentterm_Id", "dbo.PaymentTerms");
            DropForeignKey("dbo.SaleOrders", "ParentSO_Id", "dbo.SaleOrders");
            DropForeignKey("dbo.SaleOrders", "offer_Id", "dbo.Offers");
            DropForeignKey("dbo.SaleOrders", "moduleContract_Id", "dbo.ModuleContracts");
            DropForeignKey("dbo.MemorandumSales", "user_Id", "dbo.Users");
            DropForeignKey("dbo.MemorandumSales", "TitleValue2Id", "dbo.Incoterms");
            DropForeignKey("dbo.MemorandumSales", "TitleValue1Id", "dbo.Incoterms");
            DropForeignKey("dbo.MemorandumSales", "SaleOrder_Id", "dbo.SaleOrders");
            DropForeignKey("dbo.ProcurementProducts", "MemorandumSale_Id", "dbo.MemorandumSales");
            DropForeignKey("dbo.MemorandumSales", "principal_Id", "dbo.Principals");
            DropForeignKey("dbo.OfferVendors", "Vendor_Id", "dbo.tabVendor");
            DropForeignKey("dbo.OfferVendors", "Offer_Id", "dbo.Offers");
            DropForeignKey("dbo.Offers", "user_Id", "dbo.Users");
            DropForeignKey("dbo.Offers", "TitleValue2Id", "dbo.Incoterms");
            DropForeignKey("dbo.Offers", "TitleValue1Id", "dbo.Incoterms");
            DropForeignKey("dbo.Offers", "statusClass_Id", "dbo.StatusClasses");
            DropForeignKey("dbo.ProcurementProducts", "Offer_Id", "dbo.Offers");
            DropForeignKey("dbo.Offers", "principal_Id", "dbo.Principals");
            DropForeignKey("dbo.Offers", "paymentterm_Id", "dbo.PaymentTerms");
            DropForeignKey("dbo.OfferStatusStatusClasses", "StatusClass_Id", "dbo.StatusClasses");
            DropForeignKey("dbo.OfferStatusStatusClasses", "OfferStatus_Id", "dbo.OfferStatus");
            DropForeignKey("dbo.Offers", "offerStatus_Id", "dbo.OfferStatus");
            DropForeignKey("dbo.MemorandumSales", "Offer_Id", "dbo.Offers");
            DropForeignKey("dbo.Offers", "inquiry_Id", "dbo.Inquiries");
            DropForeignKey("dbo.Offers", "incoterm_Id", "dbo.Incoterms");
            DropForeignKey("dbo.Offers", "allocation_Id", "dbo.Employees");
            DropForeignKey("dbo.Offers", "customerCompany_Id", "dbo.CustomerCompanies");
            DropForeignKey("dbo.Offers", "currency_Id", "dbo.Currencies");
            DropForeignKey("dbo.Offers", "CostSheet_Id", "dbo.CostSheets");
            DropForeignKey("dbo.Offers", "costCenterCurrencyId", "dbo.Currencies");
            DropForeignKey("dbo.ComparativeStatements", "offerId", "dbo.Offers");
            DropForeignKey("dbo.BookerStatementItems", "offerId", "dbo.Offers");
            DropForeignKey("dbo.Offers", "bid_Id", "dbo.Bids");
            DropForeignKey("dbo.MemorandumSales", "memorandumSaleStatus_Id", "dbo.MemorandumSaleStatus");
            DropForeignKey("dbo.MemorandumSales", "customerCompany_Id", "dbo.CustomerCompanies");
            DropForeignKey("dbo.SaleOrders", "marketExchangerateId", "dbo.MarketExchangeRates");
            DropForeignKey("dbo.SaleOrders", "interBankTransfer_Id", "dbo.InterBankTransfers");
            DropForeignKey("dbo.SaleOrders", "incoterm_Id", "dbo.Incoterms");
            DropForeignKey("dbo.SaleOrders", "allocation_Id", "dbo.Employees");
            DropForeignKey("dbo.SaleOrders", "customerCompany_Id", "dbo.CustomerCompanies");
            DropForeignKey("dbo.SaleOrders", "currency_Id", "dbo.Currencies");
            DropForeignKey("dbo.SaleOrders", "CostSheet_Id", "dbo.CostSheets");
            DropForeignKey("dbo.SaleOrders", "costCentercurrency_Id", "dbo.Currencies");
            DropForeignKey("dbo.SaleOrders", "CommissionSummarySheetId", "dbo.CommissionSummarySheets");
            DropForeignKey("dbo.SaleOrders", "Budget_Id", "dbo.BudgetCostSheets");
            DropForeignKey("dbo.BookerStatementItems", "saleOrderId", "dbo.SaleOrders");
            DropForeignKey("dbo.Bills", "saleOrder_Id", "dbo.SaleOrders");
            DropForeignKey("dbo.SaleOrders", "BillRefNoId", "dbo.VendorBillReferences");
            DropForeignKey("dbo.VendorBillReferences", "companyId", "dbo.tabCompany");
            DropForeignKey("dbo.SaleOrders", "bid_Id", "dbo.Bids");
            DropForeignKey("dbo.SaleOrders", "auditYearAdjustment_Id", "dbo.AuditYearAdjustments");
            DropForeignKey("dbo.SaleInvoiceStatusStatusClasses", "StatusClass_Id", "dbo.StatusClasses");
            DropForeignKey("dbo.SaleInvoiceStatusStatusClasses", "SaleInvoiceStatus_Id", "dbo.SaleInvoiceStatus");
            DropForeignKey("dbo.SaleInvoices", "saleInvoiceStatus_Id", "dbo.SaleInvoiceStatus");
            DropForeignKey("dbo.ProcurementProducts", "SaleInvoice_Id", "dbo.SaleInvoices");
            DropForeignKey("dbo.SaleInvoices", "principal_Id", "dbo.Principals");
            DropForeignKey("dbo.SaleInvoices", "paymentterm_Id", "dbo.PaymentTerms");
            DropForeignKey("dbo.SaleInvoices", "lotNumberId", "dbo.LotNumbers");
            DropForeignKey("dbo.LotNumbers", "deptId", "dbo.tabDepartment");
            DropForeignKey("dbo.LotNumbers", "companyId", "dbo.tabCompany");
            DropForeignKey("dbo.JournalTransactions", "SaleInvoiceId", "dbo.SaleInvoices");
            DropForeignKey("dbo.Inventories", "SaleInviceId", "dbo.SaleInvoices");
            DropForeignKey("dbo.SaleInvoices", "insuranceAppliedBy_Id", "dbo.Employees");
            DropForeignKey("dbo.SaleInvoices", "incoterm_Id", "dbo.Incoterms");
            DropForeignKey("dbo.SaleInvoices", "allocation_Id", "dbo.Employees");
            DropForeignKey("dbo.CustomerCredits", "SaleInvoiceId", "dbo.SaleInvoices");
            DropForeignKey("dbo.CustomerCredits", "CustomerCompanyId", "dbo.CustomerCompanies");
            DropForeignKey("dbo.SaleInvoices", "customerCompany_Id", "dbo.CustomerCompanies");
            DropForeignKey("dbo.SaleInvoices", "currency_Id", "dbo.Currencies");
            DropForeignKey("dbo.SaleInvoices", "CostSheet_Id", "dbo.CostSheets");
            DropForeignKey("dbo.SaleInvoices", "CommissionSummarySheetId", "dbo.CommissionSummarySheets");
            DropForeignKey("dbo.BudgetSystemCostFields", "SaleInvoice_Id", "dbo.SaleInvoices");
            DropForeignKey("dbo.BookerStatementItems", "saleInvoiceId", "dbo.SaleInvoices");
            DropForeignKey("dbo.SaleInvoices", "bank_Id", "dbo.Banks");
            DropForeignKey("dbo.SaleInvoices", "account_Id", "dbo.Accounts");
            DropForeignKey("dbo.Inventories", "PurchaseInvoiceId", "dbo.PurchaseInvoices");
            DropForeignKey("dbo.Inventories", "prodId", "dbo.Products");
            DropForeignKey("dbo.Inventories", "adjustment_Id", "dbo.InventoryAdjustments");
            DropForeignKey("dbo.InventoryAdjustments", "depId_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.InventoryAdjustments", "currency_Id", "dbo.Currencies");
            DropForeignKey("dbo.InventoryAdjustments", "creator_Id", "dbo.Users");
            DropForeignKey("dbo.InventoryAdjustments", "company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.InventoryAdjustments", "chartofAccount_Id", "dbo.ChartofAccounts");
            DropForeignKey("dbo.InventoryAdjustments", "AdjustmentStatus_Id", "dbo.InventoryAdjustmentStatus");
            DropForeignKey("dbo.Inventories", "deptId", "dbo.tabDepartment");
            DropForeignKey("dbo.Inventories", "currencyId", "dbo.Currencies");
            DropForeignKey("dbo.Inventories", "userId", "dbo.Users");
            DropForeignKey("dbo.Inventories", "companyId", "dbo.tabCompany");
            DropForeignKey("dbo.Inventories", "bookerItemId", "dbo.BookerStatementItems");
            DropForeignKey("dbo.PurchaseInvoices", "allocation_Id", "dbo.Employees");
            DropForeignKey("dbo.PurchaseInvoices", "customerCompany_Id", "dbo.CustomerCompanies");
            DropForeignKey("dbo.PurchaseInvoices", "currency_Id", "dbo.Currencies");
            DropForeignKey("dbo.PurchaseInvoices", "CostSheet_Id", "dbo.CostSheets");
            DropForeignKey("dbo.BudgetSystemCostFields", "PurchaseInvoice_Id", "dbo.PurchaseInvoices");
            DropForeignKey("dbo.BookerStatementItems", "purchaseInvoiceId", "dbo.PurchaseInvoices");
            DropForeignKey("dbo.VATBooks", "paymentId", "dbo.Payments");
            DropForeignKey("dbo.VATBooks", "currencyId", "dbo.Currencies");
            DropForeignKey("dbo.VATBooks", "loansAdvanceId", "dbo.LoansAdvances");
            DropForeignKey("dbo.VATBooks", "interCompanyId", "dbo.InterCompanyBankTransfers");
            DropForeignKey("dbo.VATBooks", "interBankTransferId", "dbo.InterBankTransfers");
            DropForeignKey("dbo.VATBooks", "deptId", "dbo.tabDepartment");
            DropForeignKey("dbo.VATBooks", "customerId", "dbo.CustomerCompanies");
            DropForeignKey("dbo.VATBooks", "companyId", "dbo.tabCompany");
            DropForeignKey("dbo.VATBooks", "adminBillId", "dbo.tabAdminBill");
            DropForeignKey("dbo.InterCompanyBankTransfers", "VATBookRefId", "dbo.VATBookRefNumbers");
            DropForeignKey("dbo.VATBookRefNumbers", "companyId", "dbo.tabCompany");
            DropForeignKey("dbo.InterCompanyBankTransfers", "user_Id", "dbo.Users");
            DropForeignKey("dbo.InterCompanyBankTransfers", "transferMethod_Id", "dbo.TranferMethods");
            DropForeignKey("dbo.STLs", "vendor_Id", "dbo.tabVendor");
            DropForeignKey("dbo.STLs", "user_Id", "dbo.Users");
            DropForeignKey("dbo.STLs", "statusId", "dbo.STLStatus");
            DropForeignKey("dbo.STLs", "stlCurrency_Id", "dbo.Currencies");
            DropForeignKey("dbo.STLs", "stlBank_Id", "dbo.Banks");
            DropForeignKey("dbo.STLs", "stlAccount_Id", "dbo.Accounts");
            DropForeignKey("dbo.STLSettlements", "stl_Id", "dbo.STLs");
            DropForeignKey("dbo.ReversalSettlements", "stl_Id", "dbo.STLs");
            DropForeignKey("dbo.STLs", "paymentCurrency_Id", "dbo.Currencies");
            DropForeignKey("dbo.STLs", "paymentBank_Id", "dbo.Banks");
            DropForeignKey("dbo.STLs", "paymentAccount_Id", "dbo.Accounts");
            DropForeignKey("dbo.JournalTransactions", "STLId", "dbo.STLs");
            DropForeignKey("dbo.STLs", "interestAmountCurrency_Id", "dbo.Currencies");
            DropForeignKey("dbo.STLs", "interest_Id", "dbo.STLInterests");
            DropForeignKey("dbo.STLInterests", "interestTypeId", "dbo.STLInterestTypes");
            DropForeignKey("dbo.STLInterests", "COA_Id", "dbo.ChartofAccounts");
            DropForeignKey("dbo.InterCompanyBankTransfers", "stlId", "dbo.STLs");
            DropForeignKey("dbo.InterBankTransfers", "stlId", "dbo.STLs");
            DropForeignKey("dbo.STLs", "dept_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.STLs", "customer_Id", "dbo.CustomerCompanies");
            DropForeignKey("dbo.STLs", "company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.STLs", "cashMarginPerc_Id", "dbo.MarginPercentages");
            DropForeignKey("dbo.MarginPercentages", "marginpercentageTypeId", "dbo.MarginPercentageTypes");
            DropForeignKey("dbo.MarginPercentages", "COA_Id", "dbo.ChartofAccounts");
            DropForeignKey("dbo.STLs", "cashMarginCurrency_Id", "dbo.Currencies");
            DropForeignKey("dbo.STLs", "cashMarginDrAccount_Id", "dbo.Accounts");
            DropForeignKey("dbo.STLs", "cashMarginBank_Id", "dbo.Banks");
            DropForeignKey("dbo.InterCompanyBankTransfers", "statusClass_Id", "dbo.StatusClasses");
            DropForeignKey("dbo.ProcurementProducts", "InterCompanyBankTransfer_Id", "dbo.InterCompanyBankTransfers");
            DropForeignKey("dbo.InterCompanyBankTransfers", "PettyCashRefToId", "dbo.BillRefNumbers");
            DropForeignKey("dbo.InterCompanyBankTransfers", "PettyCashRefFromId", "dbo.BillRefNumbers");
            DropForeignKey("dbo.PettyCashes", "InterCompanyBankTransfer_Id1", "dbo.InterCompanyBankTransfers");
            DropForeignKey("dbo.PettyCashes", "InterCompanyBankTransfer_Id", "dbo.InterCompanyBankTransfers");
            DropForeignKey("dbo.JournalTransactions", "InterCompanyId", "dbo.InterCompanyBankTransfers");
            DropForeignKey("dbo.InterCompBankTransferVATs", "taxNameId", "dbo.TaxNames");
            DropForeignKey("dbo.InterCompBankTransferVATs", "InterCompanyBankTransferToId", "dbo.InterCompanyBankTransfers");
            DropForeignKey("dbo.InterCompBankTransferVATs", "InterCompanyBankTransferFromId", "dbo.InterCompanyBankTransfers");
            DropForeignKey("dbo.InterCompanyBankTransfers", "StatusId", "dbo.InterBankTransferStatus");
            DropForeignKey("dbo.InterCompanyBankTransfers", "emp_Id", "dbo.Employees");
            DropForeignKey("dbo.InterCompanyBankTransfers", "deptTo_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.InterCompanyBankTransfers", "deptFrom_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.InterCompanyBankTransfers", "currencyToId", "dbo.Currencies");
            DropForeignKey("dbo.InterCompanyBankTransfers", "currencyFromId", "dbo.Currencies");
            DropForeignKey("dbo.InterCompanyBankTransfers", "currency_Id", "dbo.Currencies");
            DropForeignKey("dbo.InterCompanyBankTransfers", "companyTo_Id", "dbo.tabCompany");
            DropForeignKey("dbo.InterCompanyBankTransfers", "companyFrom_Id", "dbo.tabCompany");
            DropForeignKey("dbo.InterCompanyBankTransfers", "COAdebit_Id", "dbo.ChartofAccounts");
            DropForeignKey("dbo.InterCompanyBankTransfers", "COAcredit_Id", "dbo.ChartofAccounts");
            DropForeignKey("dbo.InterCompanyBankTransfers", "vendorBillId", "dbo.Bills");
            DropForeignKey("dbo.InterCompanyBankTransfers", "bankTo_Id", "dbo.Banks");
            DropForeignKey("dbo.InterCompanyBankTransfers", "bankFrom_Id", "dbo.Banks");
            DropForeignKey("dbo.InterCompBankTransferCharges", "InterCompanyBankTransferToId", "dbo.InterCompanyBankTransfers");
            DropForeignKey("dbo.InterCompBankTransferCharges", "InterCompanyBankTransferFromId", "dbo.InterCompanyBankTransfers");
            DropForeignKey("dbo.InterCompBankTransferCharges", "deduction_Id", "dbo.Deductions");
            DropForeignKey("dbo.InterCompanyBankTransfers", "adminBillId", "dbo.tabAdminBill");
            DropForeignKey("dbo.InterCompanyBankTransfers", "accountTo_Id", "dbo.Accounts");
            DropForeignKey("dbo.InterCompanyBankTransfers", "accountFrom_Id", "dbo.Accounts");
            DropForeignKey("dbo.PettyCashes", "interBankTransferId", "dbo.InterBankTransfers");
            DropForeignKey("dbo.PettyCashes", "deptId", "dbo.tabDepartment");
            DropForeignKey("dbo.PettyCashes", "companyId", "dbo.tabCompany");
            DropForeignKey("dbo.PettyCashes", "billId", "dbo.Bills");
            DropForeignKey("dbo.PettyCashes", "AdminBillId", "dbo.tabAdminBill");
            DropForeignKey("dbo.Payments", "paymentterm_Id", "dbo.PaymentTerms");
            DropForeignKey("dbo.PaymentTaxes", "Payment_Id", "dbo.Payments");
            DropForeignKey("dbo.PaymentTaxes", "taxNameId", "dbo.TaxNames");
            DropForeignKey("dbo.Payments", "PaymentRefNoId", "dbo.BillRefNumbers");
            DropForeignKey("dbo.BillRefNumbers", "companyId", "dbo.tabCompany");
            DropForeignKey("dbo.Payments", "paymentMethodId", "dbo.PaymentMethods");
            DropForeignKey("dbo.PaymentDeductions", "Payment_Id", "dbo.Payments");
            DropForeignKey("dbo.PaymentDeductions", "deduction_id", "dbo.Deductions");
            DropForeignKey("dbo.JournalTransactions", "PaymentId", "dbo.Payments");
            DropForeignKey("dbo.Payments", "insuranceAppliedBy_Id", "dbo.Employees");
            DropForeignKey("dbo.Payments", "currency_Id", "dbo.Currencies");
            DropForeignKey("dbo.Payments", "creditCardBankId", "dbo.Banks");
            DropForeignKey("dbo.Payments", "CostSheetId", "dbo.CostSheets");
            DropForeignKey("dbo.Payments", "company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.Payments", "coaAccountId", "dbo.ChartofAccounts");
            DropForeignKey("dbo.BudgetSystemCostFields", "Payment_Id", "dbo.Payments");
            DropForeignKey("dbo.BudgetSystemCostFields", "Head_Id", "dbo.BudgetSheetHeads");
            DropForeignKey("dbo.Payments", "bankId", "dbo.Banks");
            DropForeignKey("dbo.Payments", "accountId", "dbo.Accounts");
            DropForeignKey("dbo.LoansAdvances", "deptId", "dbo.tabDepartment");
            DropForeignKey("dbo.LoansAdvances", "currencyId", "dbo.Currencies");
            DropForeignKey("dbo.LoansAdvances", "creatorId", "dbo.Users");
            DropForeignKey("dbo.LoansAdvances", "companyId", "dbo.tabCompany");
            DropForeignKey("dbo.LoansAdvances", "applicantTypeId", "dbo.LoanApplicantTypes");
            DropForeignKey("dbo.LoansAdvances", "employeeId", "dbo.Employees");
            DropForeignKey("dbo.LoansAdvances", "applicantId", "dbo.LoanApplicants");
            DropForeignKey("dbo.LoanApplicants", "applicantTypeId", "dbo.LoanApplicantTypes");
            DropForeignKey("dbo.LoanApplicantTypes", "accountId", "dbo.ChartofAccounts");
            DropForeignKey("dbo.JournalVouchers", "purchaseOrder_Id", "dbo.PurchaseOrders");
            DropForeignKey("dbo.JournalVouchers", "userId", "dbo.Users");
            DropForeignKey("dbo.JournalVouchers", "statusId", "dbo.JournalVoucherStatus");
            DropForeignKey("dbo.JournalTransactions", "journalVoucher_id", "dbo.JournalVouchers");
            DropForeignKey("dbo.JournalVouchers", "emp_Id", "dbo.Employees");
            DropForeignKey("dbo.JournalVouchers", "dept_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.JournalVouchers", "currencyId", "dbo.Currencies");
            DropForeignKey("dbo.JournalVouchers", "company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.PurchaseOrders", "customerCompany_Id", "dbo.CustomerCompanies");
            DropForeignKey("dbo.PurchaseOrders", "currency_Id", "dbo.Currencies");
            DropForeignKey("dbo.PurchaseOrders", "CostSheet_Id", "dbo.CostSheets");
            DropForeignKey("dbo.CostSheets", "SupplierWarrantyId", "dbo.Warranties");
            DropForeignKey("dbo.Warranties", "user_Id", "dbo.Users");
            DropForeignKey("dbo.PurchaseOrders", "SOWarrantyId", "dbo.Warranties");
            DropForeignKey("dbo.PurchaseOrders", "POWarrantyId", "dbo.Warranties");
            DropForeignKey("dbo.CostSheets", "paymentterm_Id", "dbo.PaymentTerms");
            DropForeignKey("dbo.PaymentTerms", "user_Id", "dbo.Users");
            DropForeignKey("dbo.PurchaseOrders", "SoPaymentterm_Id", "dbo.PaymentTerms");
            DropForeignKey("dbo.PurchaseOrders", "POPaymentterm_Id", "dbo.PaymentTerms");
            DropForeignKey("dbo.PaymentTerms", "ParentId", "dbo.PaymentTerms");
            DropForeignKey("dbo.CostSheets", "incoterm_Id", "dbo.Incoterms");
            DropForeignKey("dbo.FieldValues", "CostSheet_Id", "dbo.CostSheets");
            DropForeignKey("dbo.FieldValues", "FieldId", "dbo.CostSheetFields");
            DropForeignKey("dbo.CostSheetSOFields", "FieldId", "dbo.CostSheetFields");
            DropForeignKey("dbo.CostSheetSOFields", "CostSheetId", "dbo.CostSheets");
            DropForeignKey("dbo.CostSheetSIFields", "FieldId", "dbo.CostSheetFields");
            DropForeignKey("dbo.CostSheetSIFields", "CostSheetId", "dbo.CostSheets");
            DropForeignKey("dbo.CostSheetSaleReceiptFields", "FieldId", "dbo.CostSheetFields");
            DropForeignKey("dbo.CostSheetSaleReceiptFields", "CostSheetId", "dbo.CostSheets");
            DropForeignKey("dbo.CostSheetPOFields", "FieldId", "dbo.CostSheetFields");
            DropForeignKey("dbo.CostSheetPOFields", "CostSheetId", "dbo.CostSheets");
            DropForeignKey("dbo.CostSheetPaymentFields", "FieldId", "dbo.CostSheetFields");
            DropForeignKey("dbo.CostSheetPaymentFields", "CostSheetId", "dbo.CostSheets");
            DropForeignKey("dbo.CostSheetOfferFields", "FieldId", "dbo.CostSheetFields");
            DropForeignKey("dbo.CostSheetOfferFields", "CostSheetId", "dbo.CostSheets");
            DropForeignKey("dbo.CostSheetBillFields", "FieldId", "dbo.CostSheetFields");
            DropForeignKey("dbo.CostSheetBillFields", "CostSheetId", "dbo.CostSheets");
            DropForeignKey("dbo.CostFieldHistories", "FieldId", "dbo.CostSheetFields");
            DropForeignKey("dbo.CostFieldHistories", "CostSheetId", "dbo.CostSheets");
            DropForeignKey("dbo.PurchaseOrders", "CommissionSummarySheetId", "dbo.CommissionSummarySheets");
            DropForeignKey("dbo.CommissionSummarySheets", "OfferCurrencyId", "dbo.Currencies");
            DropForeignKey("dbo.SummaryFieldValues", "CommissionSummarySheet_Id", "dbo.CommissionSummarySheets");
            DropForeignKey("dbo.SummaryFieldValues", "FieldId", "dbo.SummarySheetFields");
            DropForeignKey("dbo.SummarySheetFields", "AddedbyUserId", "dbo.Users");
            DropForeignKey("dbo.PurchaseOrders", "Budget_Id", "dbo.BudgetCostSheets");
            DropForeignKey("dbo.BudgetCostSheets", "empId", "dbo.Employees");
            DropForeignKey("dbo.BudgetCostSheets", "deptId", "dbo.tabDepartment");
            DropForeignKey("dbo.BudgetCostSheets", "currencyId", "dbo.Currencies");
            DropForeignKey("dbo.BudgetCostSheets", "companyId", "dbo.tabCompany");
            DropForeignKey("dbo.BudgetCostSheets", "budgetCostSheetStatus_Id", "dbo.BudgetCostSheetStatus");
            DropForeignKey("dbo.BudgetCostFields", "BudgetCostSheet_Id", "dbo.BudgetCostSheets");
            DropForeignKey("dbo.BudgetCostFields", "Head_Id", "dbo.BudgetSheetHeads");
            DropForeignKey("dbo.BudgetSheetHeads", "creatorId", "dbo.Users");
            DropForeignKey("dbo.BookerStatementItems", "purchaseOrderId", "dbo.PurchaseOrders");
            DropForeignKey("dbo.PurchaseOrders", "bid_Id", "dbo.Bids");
            DropForeignKey("dbo.PurchaseOrders", "allocation_Id", "dbo.Employees");
            DropForeignKey("dbo.ComparativeStatements", "creatorId", "dbo.Users");
            DropForeignKey("dbo.ComparativeStatementItems", "itemCurrencyId", "dbo.Currencies");
            DropForeignKey("dbo.ComparativeStatementItems", "costSheetFieldId", "dbo.CostSheetFields");
            DropForeignKey("dbo.ComparativeStatementItems", "convertedCurrencyId", "dbo.Currencies");
            DropForeignKey("dbo.ComparativeStatementItems", "comparativeStatementId", "dbo.ComparativeStatements");
            DropForeignKey("dbo.BookerStatementItems", "ModuleContractId", "dbo.ModuleContracts");
            DropForeignKey("dbo.ModuleContracts", "bid_Id", "dbo.Bids");
            DropForeignKey("dbo.BookerStatementItems", "focSamplingId", "dbo.FOCSamplings");
            DropForeignKey("dbo.FOCSamplings", "chartofAccountId", "dbo.ChartofAccounts");
            DropForeignKey("dbo.BookerStatementItems", "claimDiscountId", "dbo.ClaimDiscounts");
            DropForeignKey("dbo.ClaimDiscounts", "chartofAccountId", "dbo.ChartofAccounts");
            DropForeignKey("dbo.Inquiries", "inquiryStatus_Id", "dbo.InquiryStatus");
            DropForeignKey("dbo.Inquiries", "allocation_Id", "dbo.Employees");
            DropForeignKey("dbo.Inquiries", "customerCompany_Id", "dbo.CustomerCompanies");
            DropForeignKey("dbo.DocumentStatusStatusClasses", "StatusClass_Id", "dbo.StatusClasses");
            DropForeignKey("dbo.DocumentStatusStatusClasses", "DocumentStatus_Id", "dbo.DocumentStatus");
            DropForeignKey("dbo.Documents", "statusClass_Id", "dbo.StatusClasses");
            DropForeignKey("dbo.Documents", "status_Id", "dbo.DocumentStatus");
            DropForeignKey("dbo.Documents", "employee_Id", "dbo.Employees");
            DropForeignKey("dbo.Documents", "documentType_Id", "dbo.DocumentTypes");
            DropForeignKey("dbo.Documents", "documentTemplate_Id", "dbo.DocumentTemplates");
            DropForeignKey("dbo.Documents", "documentAuthority_Id", "dbo.DocumentAuthorities");
            DropForeignKey("dbo.DocumentAuthorities", "documentTemplate_Id", "dbo.DocumentTemplates");
            DropForeignKey("dbo.DocumentTemplateDocumentTypes", "DocumentType_Id", "dbo.DocumentTypes");
            DropForeignKey("dbo.DocumentTemplateDocumentTypes", "DocumentTemplate_Id", "dbo.DocumentTemplates");
            DropForeignKey("dbo.Documents", "creator_Id", "dbo.Users");
            DropForeignKey("dbo.Documents", "country_Id", "dbo.Countries");
            DropForeignKey("dbo.BillStatusStatusClasses", "StatusClass_Id", "dbo.StatusClasses");
            DropForeignKey("dbo.BillStatusStatusClasses", "BillStatus_Id", "dbo.BillStatus");
            DropForeignKey("dbo.Bills", "BillStatus_Id", "dbo.BillStatus");
            DropForeignKey("dbo.AssetRentalStatusStatusClasses", "StatusClass_Id", "dbo.StatusClasses");
            DropForeignKey("dbo.AssetRentalStatusStatusClasses", "AssetRentalStatus_Id", "dbo.AssetRentalStatus");
            DropForeignKey("dbo.AssetRentals", "statusId", "dbo.AssetRentalStatus");
            DropForeignKey("dbo.tabAdminBill", "statusId", "dbo.AdminBillStatus");
            DropForeignKey("dbo.AdminBillStatusStatusClasses", "StatusClass_Id", "dbo.StatusClasses");
            DropForeignKey("dbo.AdminBillStatusStatusClasses", "AdminBillStatus_Id", "dbo.AdminBillStatus");
            DropForeignKey("dbo.InterBankTransferVATs", "taxNameId", "dbo.TaxNames");
            DropForeignKey("dbo.TaxNames", "taxTypeId", "dbo.TaxTypes");
            DropForeignKey("dbo.TaxNames", "COA_Id", "dbo.ChartofAccounts");
            DropForeignKey("dbo.InterBankTransferVATs", "InterBankTransferToId", "dbo.InterBankTransfers");
            DropForeignKey("dbo.InterBankTransferVATs", "InterBankTransferFromId", "dbo.InterBankTransfers");
            DropForeignKey("dbo.InterBankTransfers", "industryTypeId", "dbo.IndustryTypes");
            DropForeignKey("dbo.IndustryTypes", "user_Id", "dbo.Users");
            DropForeignKey("dbo.CustomerCompanies", "shippingAddress_Id", "dbo.tabAddress");
            DropForeignKey("dbo.CustomerCompanies", "ParentID", "dbo.CustomerCompanies");
            DropForeignKey("dbo.CustomerCompanyIndustryTypes", "IndustryType_Id", "dbo.IndustryTypes");
            DropForeignKey("dbo.CustomerCompanyIndustryTypes", "CustomerCompany_Id", "dbo.CustomerCompanies");
            DropForeignKey("dbo.CustomerCompanyDepartment1", "Department_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.CustomerCompanyDepartment1", "CustomerCompany_Id", "dbo.CustomerCompanies");
            DropForeignKey("dbo.CustomerCompanyDepartments", "Department_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.CustomerCompanyDepartments", "CustomerCompany_Id", "dbo.CustomerCompanies");
            DropForeignKey("dbo.AuditYearAdjustments", "user_Id", "dbo.Users");
            DropForeignKey("dbo.AuditYearAdjustments", "principal_Id", "dbo.Principals");
            DropForeignKey("dbo.Principals", "shippingAddress_Id", "dbo.tabAddress");
            DropForeignKey("dbo.Principals", "ParentID", "dbo.Principals");
            DropForeignKey("dbo.PrincipalDepartments", "Department_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.PrincipalDepartments", "Principal_Id", "dbo.Principals");
            DropForeignKey("dbo.Principals", "contactPerson_Id", "dbo.tabPerson");
            DropForeignKey("dbo.Principals", "contact_Id", "dbo.tabContact");
            DropForeignKey("dbo.Principals", "company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.Principals", "billingAddres_Id", "dbo.tabAddress");
            DropForeignKey("dbo.AuditYearAdjustments", "allocation_Id", "dbo.Employees");
            DropForeignKey("dbo.AuditYearAdjustments", "dept_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.AuditYearAdjustments", "customerCompany_Id", "dbo.CustomerCompanies");
            DropForeignKey("dbo.AuditYearAdjustments", "company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.AuditYearAdjustments", "auditCurrency_Id", "dbo.Currencies");
            DropForeignKey("dbo.ContactPersons", "ReligionId", "dbo.Religions");
            DropForeignKey("dbo.ContactPersons", "person_Id", "dbo.tabPerson");
            DropForeignKey("dbo.ContactPersons", "customerCompanyId", "dbo.CustomerCompanies");
            DropForeignKey("dbo.ContactPersons", "contact_Id", "dbo.tabContact");
            DropForeignKey("dbo.ContactPersons", "bank_Id", "dbo.Banks");
            DropForeignKey("dbo.CustomerCompanies", "contactPerson_Id", "dbo.tabPerson");
            DropForeignKey("dbo.CustomerCompanies", "contact_Id", "dbo.tabContact");
            DropForeignKey("dbo.CustomerCompanies", "company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.CustomerCompanies", "billingAddres_Id", "dbo.tabAddress");
            DropForeignKey("dbo.InterBankTransfers", "emp_Id", "dbo.Employees");
            DropForeignKey("dbo.InterBankTransfers", "dept_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.InterBankTransfers", "currencyToId", "dbo.Currencies");
            DropForeignKey("dbo.InterBankTransfers", "currencyFromId", "dbo.Currencies");
            DropForeignKey("dbo.InterBankTransfers", "currency_Id", "dbo.Currencies");
            DropForeignKey("dbo.InterBankTransfers", "company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.InterBankTransfers", "bankTo_Id", "dbo.Banks");
            DropForeignKey("dbo.InterBankTransfers", "bankFrom_Id", "dbo.Banks");
            DropForeignKey("dbo.IBTbankCharges", "InterBankTransferToId", "dbo.InterBankTransfers");
            DropForeignKey("dbo.IBTbankCharges", "InterBankTransferFromId", "dbo.InterBankTransfers");
            DropForeignKey("dbo.IBTbankCharges", "deduction_Id", "dbo.Deductions");
            DropForeignKey("dbo.Deductions", "chartofAccountId", "dbo.ChartofAccounts");
            DropForeignKey("dbo.InterBankTransfers", "adminBillId", "dbo.tabAdminBill");
            DropForeignKey("dbo.InterBankTransfers", "accountTo_Id", "dbo.Accounts");
            DropForeignKey("dbo.InterBankTransfers", "accountFrom_Id", "dbo.Accounts");
            DropForeignKey("dbo.JournalTransactions", "deptId", "dbo.tabDepartment");
            DropForeignKey("dbo.JournalTransactions", "currencyId", "dbo.Currencies");
            DropForeignKey("dbo.JournalTransactions", "costSheetFieldId", "dbo.CostSheetFields");
            DropForeignKey("dbo.JournalTransactions", "companyId", "dbo.tabCompany");
            DropForeignKey("dbo.JournalTransactions", "Bill_Id", "dbo.Bills");
            DropForeignKey("dbo.JournalTransactions", "AdminBillId", "dbo.tabAdminBill");
            DropForeignKey("dbo.JournalTransactions", "accountId", "dbo.ChartofAccounts");
            DropForeignKey("dbo.CostSheetFields", "AddedbyUserId", "dbo.Users");
            DropForeignKey("dbo.Checklists", "creatorId", "dbo.Users");
            DropForeignKey("dbo.Tasks", "assignedToId", "dbo.Users");
            DropForeignKey("dbo.Tasks", "assignedById", "dbo.Users");
            DropForeignKey("dbo.TasksUsers", "User_id", "dbo.Users");
            DropForeignKey("dbo.TasksUsers", "Tasks_Id", "dbo.Tasks");
            DropForeignKey("dbo.RoleUsers", "User_id", "dbo.Users");
            DropForeignKey("dbo.RoleUsers", "Role_Id", "dbo.Roles");
            DropForeignKey("dbo.PermissionRoles", "Role_Id", "dbo.Roles");
            DropForeignKey("dbo.PermissionRoles", "Permission_Id", "dbo.Permissions");
            DropForeignKey("dbo.Permissions", "ParentId", "dbo.Permissions");
            DropForeignKey("dbo.Roles", "parentId", "dbo.Roles");
            DropForeignKey("dbo.PollUser1", "User_id", "dbo.Users");
            DropForeignKey("dbo.PollUser1", "Poll_Id", "dbo.Polls");
            DropForeignKey("dbo.PollUsers", "User_id", "dbo.Users");
            DropForeignKey("dbo.PollUsers", "Poll_Id", "dbo.Polls");
            DropForeignKey("dbo.Polls", "taskGroupId", "dbo.TaskGroups");
            DropForeignKey("dbo.Polls", "initiatedById", "dbo.Users");
            DropForeignKey("dbo.Memos", "createdForId", "dbo.Users");
            DropForeignKey("dbo.LoginUserDetails", "userId", "dbo.Users");
            DropForeignKey("dbo.Users", "employeeId", "dbo.Employees");
            DropForeignKey("dbo.Memos", "taskGroupId", "dbo.TaskGroups");
            DropForeignKey("dbo.TaskGroupsUser1", "User_id", "dbo.Users");
            DropForeignKey("dbo.TaskGroupsUser1", "TaskGroups_Id", "dbo.TaskGroups");
            DropForeignKey("dbo.TaskGroupsUsers", "User_id", "dbo.Users");
            DropForeignKey("dbo.TaskGroupsUsers", "TaskGroups_Id", "dbo.TaskGroups");
            DropForeignKey("dbo.TaskGroups", "targetGroup_Id", "dbo.TargetGroups");
            DropForeignKey("dbo.TargetGroups", "parentId", "dbo.TargetGroups");
            DropForeignKey("dbo.TaskGroups", "payableAccount_Id", "dbo.ChartofAccounts");
            DropForeignKey("dbo.TaskGroups", "parentId", "dbo.TaskGroups");
            DropForeignKey("dbo.TaskGroups", "GroupCreatorId", "dbo.Users");
            DropForeignKey("dbo.TaskGroups", "currency_Id", "dbo.Currencies");
            DropForeignKey("dbo.SalesExchangeRates", "target_currency_Id", "dbo.Currencies");
            DropForeignKey("dbo.MarketExchangeRates", "target_currency_Id", "dbo.Currencies");
            DropForeignKey("dbo.SalesExchangeRates", "base_currency_Id", "dbo.Currencies");
            DropForeignKey("dbo.SalesExchangeRates", "Editedbyuser_Id", "dbo.Users");
            DropForeignKey("dbo.SalesExchangeRates", "Addedbyuser_Id", "dbo.Users");
            DropForeignKey("dbo.MarketExchangeRates", "base_currency_Id", "dbo.Currencies");
            DropForeignKey("dbo.MarketExchangeRates", "Editedbyuser_Id", "dbo.Users");
            DropForeignKey("dbo.MarketExchangeRates", "Addedbyuser_Id", "dbo.Users");
            DropForeignKey("dbo.TaskGroups", "cgsAccount_Id", "dbo.ChartofAccounts");
            DropForeignKey("dbo.TaskGroups", "CalculationType_Id", "dbo.StatusCalculationTypes");
            DropForeignKey("dbo.StatusCalculationTypes", "TotalField_Id", "dbo.SoCalculationFields");
            DropForeignKey("dbo.StatusCalculationTypes", "AchievedField_Id", "dbo.SoCalculationFields");
            DropForeignKey("dbo.Memos", "createdById", "dbo.Users");
            DropForeignKey("dbo.MemoUsers", "User_id", "dbo.Users");
            DropForeignKey("dbo.MemoUsers", "Memo_Id", "dbo.Memos");
            DropForeignKey("dbo.CommentLogUser3", "User_id", "dbo.Users");
            DropForeignKey("dbo.CommentLogUser3", "CommentLog_Id", "dbo.CommentLogs");
            DropForeignKey("dbo.CommentLogUser2", "User_id", "dbo.Users");
            DropForeignKey("dbo.CommentLogUser2", "CommentLog_Id", "dbo.CommentLogs");
            DropForeignKey("dbo.CommentLogs", "salesPersonId", "dbo.Employees");
            DropForeignKey("dbo.CommentLogs", "ReplyCommentId", "dbo.CommentLogs");
            DropForeignKey("dbo.CommentLogs", "FlagId", "dbo.NotificationFlags");
            DropForeignKey("dbo.Notifications", "UserId", "dbo.Users");
            DropForeignKey("dbo.Notifications", "SendingUserId", "dbo.Users");
            DropForeignKey("dbo.Notifications", "FlagId", "dbo.NotificationFlags");
            DropForeignKey("dbo.Notifications", "CcUserId", "dbo.Users");
            DropForeignKey("dbo.CommentLogs", "managerId", "dbo.Employees");
            DropForeignKey("dbo.CommentLogs", "financePersonId", "dbo.Employees");
            DropForeignKey("dbo.CommentLogUser1", "User_id", "dbo.Users");
            DropForeignKey("dbo.CommentLogUser1", "CommentLog_Id", "dbo.CommentLogs");
            DropForeignKey("dbo.CommentLogUsers", "User_id", "dbo.Users");
            DropForeignKey("dbo.CommentLogUsers", "CommentLog_Id", "dbo.CommentLogs");
            DropForeignKey("dbo.CommentLogs", "CategoryId", "dbo.CommentCategories");
            DropForeignKey("dbo.CommentCategories", "user_Id", "dbo.Users");
            DropForeignKey("dbo.TransactionItems", "CommentCategory_Id", "dbo.CommentCategories");
            DropForeignKey("dbo.AssetRentals", "countryId", "dbo.Countries");
            DropForeignKey("dbo.AssetRentals", "companyId", "dbo.tabCompany");
            DropForeignKey("dbo.AssetRentals", "cityId", "dbo.Cities");
            DropForeignKey("dbo.AssetRentals", "assetTypeId", "dbo.AssetTypes");
            DropForeignKey("dbo.AssetRentals", "assetSubNatureId", "dbo.RentalAssetSubNatures");
            DropForeignKey("dbo.AssetRentals", "assetRentalUnitId", "dbo.AssetRentalUnits");
            DropForeignKey("dbo.AssetRentalUnits", "countryId", "dbo.Countries");
            DropForeignKey("dbo.AssetRentalUnits", "cityId", "dbo.Cities");
            DropForeignKey("dbo.AssetRentalUnits", "assetRentalLocationId", "dbo.AssetRentalLocations");
            DropForeignKey("dbo.AssetRentals", "assetRentalLocationId", "dbo.AssetRentalLocations");
            DropForeignKey("dbo.AssetRentalLocations", "countryId", "dbo.Countries");
            DropForeignKey("dbo.AssetRentalLocations", "cityId", "dbo.Cities");
            DropForeignKey("dbo.Cities", "countryId", "dbo.Countries");
            DropForeignKey("dbo.AssetRentals", "assetNumberId", "dbo.AssetNumbers");
            DropForeignKey("dbo.AssetRentals", "AssetNatureId", "dbo.RentalAssetNatures");
            DropForeignKey("dbo.AssetRentals", "assetModelId", "dbo.AssetModels");
            DropForeignKey("dbo.AssetRentals", "assetHolderEmployeeId", "dbo.Employees");
            DropForeignKey("dbo.AssetRentals", "assetBrandId", "dbo.AssetBrands");
            DropForeignKey("dbo.AssetBrandRentalAssetSubNatures", "RentalAssetSubNature_Id", "dbo.RentalAssetSubNatures");
            DropForeignKey("dbo.AssetBrandRentalAssetSubNatures", "AssetBrand_Id", "dbo.AssetBrands");
            DropForeignKey("dbo.AssetBrands", "assetNatureId", "dbo.RentalAssetNatures");
            DropForeignKey("dbo.AssetModelRentalAssetSubNatures", "RentalAssetSubNature_Id", "dbo.RentalAssetSubNatures");
            DropForeignKey("dbo.AssetModelRentalAssetSubNatures", "AssetModel_Id", "dbo.AssetModels");
            DropForeignKey("dbo.RentalAssetSubNatures", "assetNatureId", "dbo.RentalAssetNatures");
            DropForeignKey("dbo.AssetModels", "assetNatureId", "dbo.RentalAssetNatures");
            DropForeignKey("dbo.AssetModelAssetBrands", "AssetBrand_Id", "dbo.AssetBrands");
            DropForeignKey("dbo.AssetModelAssetBrands", "AssetModel_Id", "dbo.AssetModels");
            DropForeignKey("dbo.tabAdminBill", "assetRentalId", "dbo.AssetRentals");
            DropForeignKey("dbo.AssetRentals", "addressId", "dbo.tabAddress");
            DropForeignKey("dbo.tabAdminBill", "adminBillType_Id", "dbo.AdminBillTypes");
            DropForeignKey("dbo.tabAdminBill", "adminBillNature_Id", "dbo.AdminBillNatures");
            DropForeignKey("dbo.AdminBillNatureCompanies", "Company_Id", "dbo.tabCompany");
            DropForeignKey("dbo.AdminBillNatureCompanies", "AdminBillNature_Id", "dbo.AdminBillNatures");
            DropForeignKey("dbo.VendorBillAdjustments", "bill_Id", "dbo.Bills");
            DropForeignKey("dbo.tabVendor", "billingAddres_Id", "dbo.tabAddress");
            DropForeignKey("dbo.VendorAdminBillTypes", "AdminBillType_Id", "dbo.AdminBillTypes");
            DropForeignKey("dbo.VendorAdminBillTypes", "Vendor_Id", "dbo.tabVendor");
            DropForeignKey("dbo.Payees", "vehicleOwnerId", "dbo.RentedVehicleOwners");
            DropForeignKey("dbo.Payees", "ParentId", "dbo.Payees");
            DropForeignKey("dbo.PayeeDepartments", "Department_Id", "dbo.tabDepartment");
            DropForeignKey("dbo.PayeeDepartments", "Payee_Id", "dbo.Payees");
            DropForeignKey("dbo.Payees", "companyId", "dbo.tabCompany");
            DropForeignKey("dbo.PayeeAdminBillTypes", "AdminBillType_Id", "dbo.AdminBillTypes");
            DropForeignKey("dbo.PayeeAdminBillTypes", "Payee_Id", "dbo.Payees");
            DropForeignKey("dbo.AdminBillTypeChartofAccounts", "ChartofAccount_Id", "dbo.ChartofAccounts");
            DropForeignKey("dbo.AdminBillTypeChartofAccounts", "AdminBillType_Id", "dbo.AdminBillTypes");
            DropForeignKey("dbo.Designations", "companyId", "dbo.tabCompany");
            DropForeignKey("dbo.Assets", "coOwnerID", "dbo.Employees");
            DropForeignKey("dbo.Assets", "CoOwnerAssetId", "dbo.AssetOwners");
            DropForeignKey("dbo.Assets", "assetStatus_Id", "dbo.AssetStatus");
            DropForeignKey("dbo.Assets", "OwnerId", "dbo.AssetOwners");
            DropForeignKey("dbo.AssetOwners", "owner_Id", "dbo.tabPerson");
            DropForeignKey("dbo.PersonPhotoes", "Person_Id", "dbo.tabPerson");
            DropForeignKey("dbo.AssetOwners", "contact_Id", "dbo.tabContact");
            DropForeignKey("dbo.AssetOwners", "address_Id", "dbo.tabAddress");
            DropForeignKey("dbo.Assets", "AssetNatureId", "dbo.AssetNatures");
            DropForeignKey("dbo.AssetNatures", "parentId", "dbo.AssetNatures");
            DropForeignKey("dbo.Assets", "address_Id", "dbo.tabAddress");
            DropForeignKey("dbo.Employees", "address2_Id", "dbo.tabAddress");
            DropForeignKey("dbo.Employees", "address_Id", "dbo.tabAddress");
            DropForeignKey("dbo.tabCompany", "addressId", "dbo.tabAddress");
            DropForeignKey("dbo.Banks", "address_Id", "dbo.tabAddress");
            DropIndex("dbo.CompanyVendors", new[] { "Vendor_Id" });
            DropIndex("dbo.CompanyVendors", new[] { "Company_Id" });
            DropIndex("dbo.CompanyTaskTypes", new[] { "TaskType_Id" });
            DropIndex("dbo.CompanyTaskTypes", new[] { "Company_Id" });
            DropIndex("dbo.CompanyTaskGroups1", new[] { "TaskGroups_Id" });
            DropIndex("dbo.CompanyTaskGroups1", new[] { "Company_Id" });
            DropIndex("dbo.CompanyTaskGroups", new[] { "TaskGroups_Id" });
            DropIndex("dbo.CompanyTaskGroups", new[] { "Company_Id" });
            DropIndex("dbo.CompanyEmployee1", new[] { "Employee_EmpId" });
            DropIndex("dbo.CompanyEmployee1", new[] { "Company_Id" });
            DropIndex("dbo.CompanyPayees", new[] { "Payee_Id" });
            DropIndex("dbo.CompanyPayees", new[] { "Company_Id" });
            DropIndex("dbo.CompanyLoanApplicants", new[] { "LoanApplicant_Id" });
            DropIndex("dbo.CompanyLoanApplicants", new[] { "Company_Id" });
            DropIndex("dbo.CompanyDocumentTypes", new[] { "DocumentType_Id" });
            DropIndex("dbo.CompanyDocumentTypes", new[] { "Company_Id" });
            DropIndex("dbo.CompanyDocumentTemplates", new[] { "DocumentTemplate_Id" });
            DropIndex("dbo.CompanyDocumentTemplates", new[] { "Company_Id" });
            DropIndex("dbo.CompanyCustomerCompanies", new[] { "CustomerCompany_Id" });
            DropIndex("dbo.CompanyCustomerCompanies", new[] { "Company_Id" });
            DropIndex("dbo.CompanyChartofAccountGroups", new[] { "ChartofAccountGroup_Id" });
            DropIndex("dbo.CompanyChartofAccountGroups", new[] { "Company_Id" });
            DropIndex("dbo.CompanyBanks", new[] { "Bank_Id" });
            DropIndex("dbo.CompanyBanks", new[] { "Company_Id" });
            DropIndex("dbo.CompanyEmployees", new[] { "Employee_EmpId" });
            DropIndex("dbo.CompanyEmployees", new[] { "Company_Id" });
            DropIndex("dbo.EmployeeCompany1", new[] { "Company_Id" });
            DropIndex("dbo.EmployeeCompany1", new[] { "Employee_EmpId" });
            DropIndex("dbo.EmployeeCompanies", new[] { "Company_Id" });
            DropIndex("dbo.EmployeeCompanies", new[] { "Employee_EmpId" });
            DropIndex("dbo.EmployeeChartofAccounts", new[] { "ChartofAccount_Id" });
            DropIndex("dbo.EmployeeChartofAccounts", new[] { "Employee_EmpId" });
            DropIndex("dbo.EmployeeChartofAccountGroups", new[] { "ChartofAccountGroup_Id" });
            DropIndex("dbo.EmployeeChartofAccountGroups", new[] { "Employee_EmpId" });
            DropIndex("dbo.AssetTenancyContracts", new[] { "TenancyContract_Id" });
            DropIndex("dbo.AssetTenancyContracts", new[] { "Asset_Id" });
            DropIndex("dbo.DepartmentVendors", new[] { "Vendor_Id" });
            DropIndex("dbo.DepartmentVendors", new[] { "Department_Id" });
            DropIndex("dbo.DepartmentTaskTypes", new[] { "TaskType_Id" });
            DropIndex("dbo.DepartmentTaskTypes", new[] { "Department_Id" });
            DropIndex("dbo.DepartmentTaskGroups1", new[] { "TaskGroups_Id" });
            DropIndex("dbo.DepartmentTaskGroups1", new[] { "Department_Id" });
            DropIndex("dbo.DepartmentTaskGroups", new[] { "TaskGroups_Id" });
            DropIndex("dbo.DepartmentTaskGroups", new[] { "Department_Id" });
            DropIndex("dbo.SharedGridGroupEmployees", new[] { "Employee_EmpId" });
            DropIndex("dbo.SharedGridGroupEmployees", new[] { "SharedGridGroup_Id" });
            DropIndex("dbo.SharedGridGroupDepartments", new[] { "Department_Id" });
            DropIndex("dbo.SharedGridGroupDepartments", new[] { "SharedGridGroup_Id" });
            DropIndex("dbo.SharedGridGroupCompanies", new[] { "Company_Id" });
            DropIndex("dbo.SharedGridGroupCompanies", new[] { "SharedGridGroup_Id" });
            DropIndex("dbo.DepartmentSalesReceipts", new[] { "SalesReceipt_Id" });
            DropIndex("dbo.DepartmentSalesReceipts", new[] { "Department_Id" });
            DropIndex("dbo.DepartmentPayments", new[] { "Payment_Id" });
            DropIndex("dbo.DepartmentPayments", new[] { "Department_Id" });
            DropIndex("dbo.DepartmentLoanApplicants", new[] { "LoanApplicant_Id" });
            DropIndex("dbo.DepartmentLoanApplicants", new[] { "Department_Id" });
            DropIndex("dbo.DepartmentEmployees", new[] { "Employee_EmpId" });
            DropIndex("dbo.DepartmentEmployees", new[] { "Department_Id" });
            DropIndex("dbo.DepartmentDocumentTypes", new[] { "DocumentType_Id" });
            DropIndex("dbo.DepartmentDocumentTypes", new[] { "Department_Id" });
            DropIndex("dbo.DepartmentDocumentTemplates", new[] { "DocumentTemplate_Id" });
            DropIndex("dbo.DepartmentDocumentTemplates", new[] { "Department_Id" });
            DropIndex("dbo.DepartmentCompanies", new[] { "Company_Id" });
            DropIndex("dbo.DepartmentCompanies", new[] { "Department_Id" });
            DropIndex("dbo.DepartmentChartofAccounts", new[] { "ChartofAccount_Id" });
            DropIndex("dbo.DepartmentChartofAccounts", new[] { "Department_Id" });
            DropIndex("dbo.DepartmentChartofAccountGroups", new[] { "ChartofAccountGroup_Id" });
            DropIndex("dbo.DepartmentChartofAccountGroups", new[] { "Department_Id" });
            DropIndex("dbo.CashFlowDepartments", new[] { "Department_Id" });
            DropIndex("dbo.CashFlowDepartments", new[] { "CashFlow_Id" });
            DropIndex("dbo.CashFlowCompanies", new[] { "Company_Id" });
            DropIndex("dbo.CashFlowCompanies", new[] { "CashFlow_Id" });
            DropIndex("dbo.DepartmentAccounts", new[] { "Account_Id" });
            DropIndex("dbo.DepartmentAccounts", new[] { "Department_Id" });
            DropIndex("dbo.ChartofAccountGroupChartofAccounts", new[] { "ChartofAccount_Id" });
            DropIndex("dbo.ChartofAccountGroupChartofAccounts", new[] { "ChartofAccountGroup_Id" });
            DropIndex("dbo.ChartofAccountCompanies", new[] { "Company_Id" });
            DropIndex("dbo.ChartofAccountCompanies", new[] { "ChartofAccount_Id" });
            DropIndex("dbo.VendorPayees", new[] { "Payee_Id" });
            DropIndex("dbo.VendorPayees", new[] { "Vendor_Id" });
            DropIndex("dbo.VendorBillNatureCompanies", new[] { "Company_Id" });
            DropIndex("dbo.VendorBillNatureCompanies", new[] { "VendorBillNature_Id" });
            DropIndex("dbo.ReportUsers", new[] { "User_id" });
            DropIndex("dbo.ReportUsers", new[] { "Report_Id" });
            DropIndex("dbo.InterBankTransferStatusStatusClasses", new[] { "StatusClass_Id" });
            DropIndex("dbo.InterBankTransferStatusStatusClasses", new[] { "InterBankTransferStatus_Id" });
            DropIndex("dbo.StatusClassToDoTaskStatus", new[] { "ToDoTaskStatus_Id" });
            DropIndex("dbo.StatusClassToDoTaskStatus", new[] { "StatusClass_Id" });
            DropIndex("dbo.StatusClassTenantRentalStatus", new[] { "TenantRentalStatus_Id" });
            DropIndex("dbo.StatusClassTenantRentalStatus", new[] { "StatusClass_Id" });
            DropIndex("dbo.StatusClassTasksStatus", new[] { "TasksStatus_Id" });
            DropIndex("dbo.StatusClassTasksStatus", new[] { "StatusClass_Id" });
            DropIndex("dbo.TasksStatusTaskTypes", new[] { "TaskType_Id" });
            DropIndex("dbo.TasksStatusTaskTypes", new[] { "TasksStatus_Id" });
            DropIndex("dbo.StatusClassTargetRewardStatus", new[] { "TargetRewardStatus_Id" });
            DropIndex("dbo.StatusClassTargetRewardStatus", new[] { "StatusClass_Id" });
            DropIndex("dbo.LoansStatusStatusClasses", new[] { "StatusClass_Id" });
            DropIndex("dbo.LoansStatusStatusClasses", new[] { "LoansStatus_Id" });
            DropIndex("dbo.InquiryStatusStatusClasses", new[] { "StatusClass_Id" });
            DropIndex("dbo.InquiryStatusStatusClasses", new[] { "InquiryStatus_Id" });
            DropIndex("dbo.ProductDepartments", new[] { "Department_Id" });
            DropIndex("dbo.ProductDepartments", new[] { "Product_Id" });
            DropIndex("dbo.ModuleContractVendors", new[] { "Vendor_Id" });
            DropIndex("dbo.ModuleContractVendors", new[] { "ModuleContract_Id" });
            DropIndex("dbo.ModuleContractStatusStatusClasses", new[] { "StatusClass_Id" });
            DropIndex("dbo.ModuleContractStatusStatusClasses", new[] { "ModuleContractStatus_Id" });
            DropIndex("dbo.PurchaseOrderVendors", new[] { "Vendor_Id" });
            DropIndex("dbo.PurchaseOrderVendors", new[] { "PurchaseOrder_Id" });
            DropIndex("dbo.PurchaseOrderStatusStatusClasses", new[] { "StatusClass_Id" });
            DropIndex("dbo.PurchaseOrderStatusStatusClasses", new[] { "PurchaseOrderStatus_Id" });
            DropIndex("dbo.LoansAdvanceStatusStatusClasses", new[] { "StatusClass_Id" });
            DropIndex("dbo.LoansAdvanceStatusStatusClasses", new[] { "LoansAdvanceStatus_Id" });
            DropIndex("dbo.ToDoTaskUsers", new[] { "User_id" });
            DropIndex("dbo.ToDoTaskUsers", new[] { "ToDoTask_Id" });
            DropIndex("dbo.PaymentStatusStatusClasses", new[] { "StatusClass_Id" });
            DropIndex("dbo.PaymentStatusStatusClasses", new[] { "PaymentStatus_Id" });
            DropIndex("dbo.CreditCardDepartments", new[] { "Department_Id" });
            DropIndex("dbo.CreditCardDepartments", new[] { "CreditCard_Id" });
            DropIndex("dbo.PurchaseInvoiceVendors", new[] { "Vendor_Id" });
            DropIndex("dbo.PurchaseInvoiceVendors", new[] { "PurchaseInvoice_Id" });
            DropIndex("dbo.PurchaseInvoiceStatusStatusClasses", new[] { "StatusClass_Id" });
            DropIndex("dbo.PurchaseInvoiceStatusStatusClasses", new[] { "PurchaseInvoiceStatus_Id" });
            DropIndex("dbo.SaleInvoiceVendors", new[] { "Vendor_Id" });
            DropIndex("dbo.SaleInvoiceVendors", new[] { "SaleInvoice_Id" });
            DropIndex("dbo.SalesReceiptStatusStatusClasses", new[] { "StatusClass_Id" });
            DropIndex("dbo.SalesReceiptStatusStatusClasses", new[] { "SalesReceiptStatus_Id" });
            DropIndex("dbo.RentalInvoiceStatusStatusClasses", new[] { "StatusClass_Id" });
            DropIndex("dbo.RentalInvoiceStatusStatusClasses", new[] { "RentalInvoiceStatus_Id" });
            DropIndex("dbo.RentalOrderStatusStatusClasses", new[] { "StatusClass_Id" });
            DropIndex("dbo.RentalOrderStatusStatusClasses", new[] { "RentalOrderStatus_Id" });
            DropIndex("dbo.RentalContractStatusStatusClasses", new[] { "StatusClass_Id" });
            DropIndex("dbo.RentalContractStatusStatusClasses", new[] { "RentalContractStatus_Id" });
            DropIndex("dbo.SaleOrderVendors", new[] { "Vendor_Id" });
            DropIndex("dbo.SaleOrderVendors", new[] { "SaleOrder_Id" });
            DropIndex("dbo.SaleOrderStatusStatusClasses", new[] { "StatusClass_Id" });
            DropIndex("dbo.SaleOrderStatusStatusClasses", new[] { "SaleOrderStatus_Id" });
            DropIndex("dbo.OfferVendors", new[] { "Vendor_Id" });
            DropIndex("dbo.OfferVendors", new[] { "Offer_Id" });
            DropIndex("dbo.OfferStatusStatusClasses", new[] { "StatusClass_Id" });
            DropIndex("dbo.OfferStatusStatusClasses", new[] { "OfferStatus_Id" });
            DropIndex("dbo.SaleInvoiceStatusStatusClasses", new[] { "StatusClass_Id" });
            DropIndex("dbo.SaleInvoiceStatusStatusClasses", new[] { "SaleInvoiceStatus_Id" });
            DropIndex("dbo.DocumentStatusStatusClasses", new[] { "StatusClass_Id" });
            DropIndex("dbo.DocumentStatusStatusClasses", new[] { "DocumentStatus_Id" });
            DropIndex("dbo.DocumentTemplateDocumentTypes", new[] { "DocumentType_Id" });
            DropIndex("dbo.DocumentTemplateDocumentTypes", new[] { "DocumentTemplate_Id" });
            DropIndex("dbo.BillStatusStatusClasses", new[] { "StatusClass_Id" });
            DropIndex("dbo.BillStatusStatusClasses", new[] { "BillStatus_Id" });
            DropIndex("dbo.AssetRentalStatusStatusClasses", new[] { "StatusClass_Id" });
            DropIndex("dbo.AssetRentalStatusStatusClasses", new[] { "AssetRentalStatus_Id" });
            DropIndex("dbo.AdminBillStatusStatusClasses", new[] { "StatusClass_Id" });
            DropIndex("dbo.AdminBillStatusStatusClasses", new[] { "AdminBillStatus_Id" });
            DropIndex("dbo.CustomerCompanyIndustryTypes", new[] { "IndustryType_Id" });
            DropIndex("dbo.CustomerCompanyIndustryTypes", new[] { "CustomerCompany_Id" });
            DropIndex("dbo.CustomerCompanyDepartment1", new[] { "Department_Id" });
            DropIndex("dbo.CustomerCompanyDepartment1", new[] { "CustomerCompany_Id" });
            DropIndex("dbo.CustomerCompanyDepartments", new[] { "Department_Id" });
            DropIndex("dbo.CustomerCompanyDepartments", new[] { "CustomerCompany_Id" });
            DropIndex("dbo.PrincipalDepartments", new[] { "Department_Id" });
            DropIndex("dbo.PrincipalDepartments", new[] { "Principal_Id" });
            DropIndex("dbo.TasksUsers", new[] { "User_id" });
            DropIndex("dbo.TasksUsers", new[] { "Tasks_Id" });
            DropIndex("dbo.RoleUsers", new[] { "User_id" });
            DropIndex("dbo.RoleUsers", new[] { "Role_Id" });
            DropIndex("dbo.PermissionRoles", new[] { "Role_Id" });
            DropIndex("dbo.PermissionRoles", new[] { "Permission_Id" });
            DropIndex("dbo.PollUser1", new[] { "User_id" });
            DropIndex("dbo.PollUser1", new[] { "Poll_Id" });
            DropIndex("dbo.PollUsers", new[] { "User_id" });
            DropIndex("dbo.PollUsers", new[] { "Poll_Id" });
            DropIndex("dbo.TaskGroupsUser1", new[] { "User_id" });
            DropIndex("dbo.TaskGroupsUser1", new[] { "TaskGroups_Id" });
            DropIndex("dbo.TaskGroupsUsers", new[] { "User_id" });
            DropIndex("dbo.TaskGroupsUsers", new[] { "TaskGroups_Id" });
            DropIndex("dbo.MemoUsers", new[] { "User_id" });
            DropIndex("dbo.MemoUsers", new[] { "Memo_Id" });
            DropIndex("dbo.CommentLogUser3", new[] { "User_id" });
            DropIndex("dbo.CommentLogUser3", new[] { "CommentLog_Id" });
            DropIndex("dbo.CommentLogUser2", new[] { "User_id" });
            DropIndex("dbo.CommentLogUser2", new[] { "CommentLog_Id" });
            DropIndex("dbo.CommentLogUser1", new[] { "User_id" });
            DropIndex("dbo.CommentLogUser1", new[] { "CommentLog_Id" });
            DropIndex("dbo.CommentLogUsers", new[] { "User_id" });
            DropIndex("dbo.CommentLogUsers", new[] { "CommentLog_Id" });
            DropIndex("dbo.AssetBrandRentalAssetSubNatures", new[] { "RentalAssetSubNature_Id" });
            DropIndex("dbo.AssetBrandRentalAssetSubNatures", new[] { "AssetBrand_Id" });
            DropIndex("dbo.AssetModelRentalAssetSubNatures", new[] { "RentalAssetSubNature_Id" });
            DropIndex("dbo.AssetModelRentalAssetSubNatures", new[] { "AssetModel_Id" });
            DropIndex("dbo.AssetModelAssetBrands", new[] { "AssetBrand_Id" });
            DropIndex("dbo.AssetModelAssetBrands", new[] { "AssetModel_Id" });
            DropIndex("dbo.AdminBillNatureCompanies", new[] { "Company_Id" });
            DropIndex("dbo.AdminBillNatureCompanies", new[] { "AdminBillNature_Id" });
            DropIndex("dbo.VendorAdminBillTypes", new[] { "AdminBillType_Id" });
            DropIndex("dbo.VendorAdminBillTypes", new[] { "Vendor_Id" });
            DropIndex("dbo.PayeeDepartments", new[] { "Department_Id" });
            DropIndex("dbo.PayeeDepartments", new[] { "Payee_Id" });
            DropIndex("dbo.PayeeAdminBillTypes", new[] { "AdminBillType_Id" });
            DropIndex("dbo.PayeeAdminBillTypes", new[] { "Payee_Id" });
            DropIndex("dbo.AdminBillTypeChartofAccounts", new[] { "ChartofAccount_Id" });
            DropIndex("dbo.AdminBillTypeChartofAccounts", new[] { "AdminBillType_Id" });
            DropIndex("dbo.ViewInfoes", new[] { "UserId" });
            DropIndex("dbo.UnBoundReports", new[] { "userId" });
            DropIndex("dbo.VisitingCountries", new[] { "currencyId" });
            DropIndex("dbo.VisitingCountries", new[] { "airlineId" });
            DropIndex("dbo.VisitingCountries", new[] { "visitingCountryId" });
            DropIndex("dbo.VisitingCountries", new[] { "departingCountryId" });
            DropIndex("dbo.VisitingCountries", new[] { "TravelingRecordsId" });
            DropIndex("dbo.TravelingRecords", new[] { "creatorId" });
            DropIndex("dbo.TravelingRecords", new[] { "residentCountryId" });
            DropIndex("dbo.TravelingRecords", new[] { "statusId" });
            DropIndex("dbo.TravelingRecords", new[] { "travelerNameId" });
            DropIndex("dbo.TravelingRecords", new[] { "deptId" });
            DropIndex("dbo.TravelingRecords", new[] { "companyId" });
            DropIndex("dbo.SecurityDeposits", new[] { "collectionMethodId" });
            DropIndex("dbo.SecurityDeposits", new[] { "accountId" });
            DropIndex("dbo.SecurityDeposits", new[] { "bankId" });
            DropIndex("dbo.SecurityDeposits", new[] { "mainBankId" });
            DropIndex("dbo.ResidentCountries", new[] { "countryId" });
            DropIndex("dbo.ResidentCountries", new[] { "travelerId" });
            DropIndex("dbo.RentalReceiveAmounts", new[] { "creatorId" });
            DropIndex("dbo.RentalReceiveAmounts", new[] { "statusId" });
            DropIndex("dbo.RentalReceiveAmounts", new[] { "ownerId" });
            DropIndex("dbo.RentalReceiveAmounts", new[] { "tenantId" });
            DropIndex("dbo.RentalReceiveAmounts", new[] { "RentalassetId" });
            DropIndex("dbo.RentalReceiveAmounts", new[] { "tenancyContractId" });
            DropIndex("dbo.RentalReceiveAmounts", new[] { "dept_Id" });
            DropIndex("dbo.RentalReceiveAmounts", new[] { "company_Id" });
            DropIndex("dbo.Lands", new[] { "OfficialAuths_Id" });
            DropIndex("dbo.Lands", new[] { "mortgagee_Id" });
            DropIndex("dbo.Lands", new[] { "measureUnit_Id" });
            DropIndex("dbo.Lands", new[] { "asset_Id" });
            DropIndex("dbo.ModuleFields", new[] { "templateId" });
            DropIndex("dbo.Fields", new[] { "moduleField_Id" });
            DropIndex("dbo.ExchangeRates", new[] { "ExchangeRateGroup_Id" });
            DropIndex("dbo.ExchangeRates", new[] { "company_Id" });
            DropIndex("dbo.ExchangeRateGroups", new[] { "Editedbyuser_Id" });
            DropIndex("dbo.ExchangeRateGroups", new[] { "Addedbyuser_Id" });
            DropIndex("dbo.ExchangeRateGroups", new[] { "base_currency_Id" });
            DropIndex("dbo.ExchangeRateGroups", new[] { "transaction_currency_Id" });
            DropIndex("dbo.SalaryDeductions", new[] { "deductionId" });
            DropIndex("dbo.SalaryDeductions", new[] { "payrollId" });
            DropIndex("dbo.SalaryBonus", new[] { "bonusId" });
            DropIndex("dbo.SalaryBonus", new[] { "payrollId" });
            DropIndex("dbo.SalaryAllowances", new[] { "allowanceId" });
            DropIndex("dbo.SalaryAllowances", new[] { "payrollId" });
            DropIndex("dbo.Payrolls", new[] { "loansAdvanceId" });
            DropIndex("dbo.Payrolls", new[] { "employmentSalaryId" });
            DropIndex("dbo.Payrolls", new[] { "employeeId" });
            DropIndex("dbo.Payrolls", new[] { "departmentId" });
            DropIndex("dbo.Payrolls", new[] { "companyId" });
            DropIndex("dbo.Payrolls", new[] { "creatorId" });
            DropIndex("dbo.EmploymentSalaries", new[] { "employeeId" });
            DropIndex("dbo.EmploymentSalaries", new[] { "departmentId" });
            DropIndex("dbo.EmploymentSalaries", new[] { "companyId" });
            DropIndex("dbo.EmploymentSalaries", new[] { "creatorId" });
            DropIndex("dbo.PerformanceIndicatorRatings", new[] { "IndicatorDefinitionId" });
            DropIndex("dbo.PerformanceIndicatorRatings", new[] { "ReviewId" });
            DropIndex("dbo.EmployeePerformanceReviews", new[] { "MemoId" });
            DropIndex("dbo.EmployeePerformanceReviews", new[] { "ManagementId" });
            DropIndex("dbo.EmployeePerformanceReviews", new[] { "AdminId" });
            DropIndex("dbo.EmployeePerformanceReviews", new[] { "SupervisorLevelTwoId" });
            DropIndex("dbo.EmployeePerformanceReviews", new[] { "SupervisorLevelOneId" });
            DropIndex("dbo.EmployeePerformanceReviews", new[] { "DeptId" });
            DropIndex("dbo.EmployeePerformanceReviews", new[] { "EmployeeId" });
            DropIndex("dbo.EmployeeCoaCompanies", new[] { "EmpId" });
            DropIndex("dbo.EmployeeCoaCompanies", new[] { "compId" });
            DropIndex("dbo.CustomReports", new[] { "CustomReportGeoup_Id" });
            DropIndex("dbo.CustomReports", new[] { "chartOfAccountId" });
            DropIndex("dbo.CustomReports", new[] { "deptId" });
            DropIndex("dbo.CustomReports", new[] { "companyId" });
            DropIndex("dbo.CustomReportGeoups", new[] { "updatorId" });
            DropIndex("dbo.CustomReportGeoups", new[] { "creatorId" });
            DropIndex("dbo.Vehicles", new[] { "vehicleType_Id" });
            DropIndex("dbo.Vehicles", new[] { "OfficialAuths_Id" });
            DropIndex("dbo.Vehicles", new[] { "manufacturer_Id" });
            DropIndex("dbo.Vehicles", new[] { "currentStatus_Id" });
            DropIndex("dbo.Vehicles", new[] { "BuyingStatus_Id" });
            DropIndex("dbo.Vehicles", new[] { "lesseId" });
            DropIndex("dbo.Vehicles", new[] { "assetId" });
            DropIndex("dbo.ConditionImages", new[] { "vehicleId" });
            DropIndex("dbo.Buildings", new[] { "OfficialAuths_Id" });
            DropIndex("dbo.Buildings", new[] { "mortgagee_Id" });
            DropIndex("dbo.Buildings", new[] { "measureUnit_Id" });
            DropIndex("dbo.Buildings", new[] { "asset_Id" });
            DropIndex("dbo.tabBackground", new[] { "taskGroupId" });
            DropIndex("dbo.tabBackground", new[] { "userId" });
            DropIndex("dbo.OfficialAuths", new[] { "region_Id" });
            DropIndex("dbo.AuthDocs", new[] { "authId" });
            DropIndex("dbo.Attachments", new[] { "categoryId" });
            DropIndex("dbo.Attachments", new[] { "userId" });
            DropIndex("dbo.AttachmentCategories", new[] { "userId" });
            DropIndex("dbo.AttachmentCategories", new[] { "ParentId" });
            DropIndex("dbo.EmployeeWorkExperiences", new[] { "employeeId" });
            DropIndex("dbo.SalesTargets", new[] { "CurrencyId" });
            DropIndex("dbo.Qualifications", new[] { "employeeId" });
            DropIndex("dbo.Leaves", new[] { "employeeId" });
            DropIndex("dbo.LeaveApplications", new[] { "leaveStatus_Id" });
            DropIndex("dbo.LeaveApplications", new[] { "initiator_EmpId" });
            DropIndex("dbo.LeaveApplications", new[] { "HRinfo_Id" });
            DropIndex("dbo.LeaveApplications", new[] { "approver_EmpId" });
            DropIndex("dbo.LeaveApplications", new[] { "employeeId" });
            DropIndex("dbo.LeaveApplications", new[] { "leaveId" });
            DropIndex("dbo.Functions", new[] { "company_Id" });
            DropIndex("dbo.Tenants", new[] { "person_Id" });
            DropIndex("dbo.Tenants", new[] { "contact_Id" });
            DropIndex("dbo.Tenants", new[] { "address_Id" });
            DropIndex("dbo.Tenants", new[] { "deptId" });
            DropIndex("dbo.Tenants", new[] { "companyId" });
            DropIndex("dbo.RentalAssets", new[] { "deptId" });
            DropIndex("dbo.RentalAssets", new[] { "companyId" });
            DropIndex("dbo.RentalAssets", new[] { "assetId" });
            DropIndex("dbo.RentalAssets", new[] { "AssetNatureId" });
            DropIndex("dbo.TenancyContracts", new[] { "deptId" });
            DropIndex("dbo.TenancyContracts", new[] { "companyId" });
            DropIndex("dbo.TenancyContracts", new[] { "TenantId" });
            DropIndex("dbo.TenancyContracts", new[] { "OwnerId" });
            DropIndex("dbo.TenancyContracts", new[] { "NotOwnedAssetId" });
            DropIndex("dbo.TenancyContracts", new[] { "AssetId" });
            DropIndex("dbo.Revaluations", new[] { "assetId" });
            DropIndex("dbo.PurchaseInfoes", new[] { "currecncy_Id" });
            DropIndex("dbo.TargetTypes", new[] { "user_Id" });
            DropIndex("dbo.TargetAwards", new[] { "targetId" });
            DropIndex("dbo.Targets", new[] { "user_Id" });
            DropIndex("dbo.Targets", new[] { "departmentId" });
            DropIndex("dbo.Targets", new[] { "companyId" });
            DropIndex("dbo.Targets", new[] { "currencyId" });
            DropIndex("dbo.Targets", new[] { "typeId" });
            DropIndex("dbo.SharedReports", new[] { "sharedGroupId" });
            DropIndex("dbo.SharedReports", new[] { "userId" });
            DropIndex("dbo.SharedGridGroups", new[] { "userId" });
            DropIndex("dbo.SharedGridGroups", new[] { "parentId" });
            DropIndex("dbo.DepartmentLevels", new[] { "ParentID" });
            DropIndex("dbo.ReportTitles", new[] { "userId" });
            DropIndex("dbo.GridReports", new[] { "titleId" });
            DropIndex("dbo.GridReports", new[] { "company_Id" });
            DropIndex("dbo.GridReports", new[] { "group_Id" });
            DropIndex("dbo.GridReports", new[] { "userId" });
            DropIndex("dbo.GridReportGroups", new[] { "userId" });
            DropIndex("dbo.GridReportGroups", new[] { "parentId" });
            DropIndex("dbo.CashFlows", new[] { "titleId" });
            DropIndex("dbo.CashFlows", new[] { "groupId" });
            DropIndex("dbo.CashFlows", new[] { "userId" });
            DropIndex("dbo.VendorNatureManuals", new[] { "user_Id" });
            DropIndex("dbo.VendorNatures", new[] { "user_Id" });
            DropIndex("dbo.BillTypes", new[] { "user_Id" });
            DropIndex("dbo.BillItems", new[] { "Bill_Id" });
            DropIndex("dbo.BillItems", new[] { "debitAccountId" });
            DropIndex("dbo.BillItems", new[] { "creditAccountId" });
            DropIndex("dbo.BillItems", new[] { "fieldId" });
            DropIndex("dbo.ManagementSummaries", new[] { "ParentId" });
            DropIndex("dbo.VehicleExpenses", new[] { "AdminBillForFuelExpenses_Id" });
            DropIndex("dbo.VehicleExpenses", new[] { "AdminBillForVehicleExpenses_Id" });
            DropIndex("dbo.VehicleExpenses", new[] { "maintenanceHeadId" });
            DropIndex("dbo.VehicleExpenses", new[] { "payeeId" });
            DropIndex("dbo.UserSettings", new[] { "userId" });
            DropIndex("dbo.ReportGroups", new[] { "user_Id" });
            DropIndex("dbo.ReportGroups", new[] { "parentId" });
            DropIndex("dbo.Reports", new[] { "groupId" });
            DropIndex("dbo.Reports", new[] { "ReportName" });
            DropIndex("dbo.TaskTrackings", new[] { "UpdatedById" });
            DropIndex("dbo.TaskTrackings", new[] { "tasksId" });
            DropIndex("dbo.TaskEfficiencies", new[] { "tasksId" });
            DropIndex("dbo.TaskEfficiencies", new[] { "efficiencyPoints_Id" });
            DropIndex("dbo.TaskComments", new[] { "taskId" });
            DropIndex("dbo.TaskComments", new[] { "userId" });
            DropIndex("dbo.TaskComments", new[] { "goodReceiveNoteId" });
            DropIndex("dbo.Reconcilations", new[] { "chartofAccountId" });
            DropIndex("dbo.tabLoan", new[] { "statusClass_Id" });
            DropIndex("dbo.tabLoan", new[] { "user_Id" });
            DropIndex("dbo.tabLoan", new[] { "creatorId" });
            DropIndex("dbo.tabLoan", new[] { "statusId" });
            DropIndex("dbo.tabLoan", new[] { "SubLimitNatureId" });
            DropIndex("dbo.tabLoan", new[] { "MainLimitNatureId" });
            DropIndex("dbo.tabLoan", new[] { "currencyId" });
            DropIndex("dbo.tabLoan", new[] { "accountId" });
            DropIndex("dbo.tabLoan", new[] { "bankId" });
            DropIndex("dbo.tabLoan", new[] { "deptId" });
            DropIndex("dbo.tabLoan", new[] { "CompanyId" });
            DropIndex("dbo.UnitOfMeasures", new[] { "user_Id" });
            DropIndex("dbo.ProductNatures", new[] { "user_Id" });
            DropIndex("dbo.ProductCategories", new[] { "user_Id" });
            DropIndex("dbo.ProductCategories", new[] { "parentId" });
            DropIndex("dbo.PassOns", new[] { "chartofAccountId" });
            DropIndex("dbo.ToDoTasks", new[] { "statusClass_Id" });
            DropIndex("dbo.ToDoTasks", new[] { "CalculationType_Id" });
            DropIndex("dbo.ToDoTasks", new[] { "statusId" });
            DropIndex("dbo.ToDoTasks", new[] { "targetTypeId" });
            DropIndex("dbo.ToDoTasks", new[] { "targetGroupId" });
            DropIndex("dbo.ToDoTasks", new[] { "salesHeadId" });
            DropIndex("dbo.ToDoTasks", new[] { "supervisedById" });
            DropIndex("dbo.ToDoTasks", new[] { "parentTaskId" });
            DropIndex("dbo.ToDoTasks", new[] { "taskGroupId" });
            DropIndex("dbo.ToDoTasks", new[] { "TaskCreatorId" });
            DropIndex("dbo.TargetRewards", new[] { "statusClass_Id" });
            DropIndex("dbo.TargetRewards", new[] { "creator_Id" });
            DropIndex("dbo.TargetRewards", new[] { "targetRewardNatureId" });
            DropIndex("dbo.TargetRewards", new[] { "currencyId" });
            DropIndex("dbo.TargetRewards", new[] { "toDoTask_Id" });
            DropIndex("dbo.TargetRewards", new[] { "user_Id" });
            DropIndex("dbo.TargetRewards", new[] { "statusId" });
            DropIndex("dbo.CreditCards", new[] { "currencyId" });
            DropIndex("dbo.CreditCards", new[] { "creditCardTypeId" });
            DropIndex("dbo.CreditCards", new[] { "PrimaryCardNoId" });
            DropIndex("dbo.CreditCards", new[] { "CardUserID" });
            DropIndex("dbo.CreditCards", new[] { "SecondaryCardHolderId" });
            DropIndex("dbo.CreditCards", new[] { "PrimaryCardHolderId" });
            DropIndex("dbo.CreditCards", new[] { "BankID" });
            DropIndex("dbo.CreditCards", new[] { "CompanyId" });
            DropIndex("dbo.TenantRentals", new[] { "creatorId" });
            DropIndex("dbo.TenantRentals", new[] { "statusId" });
            DropIndex("dbo.TenantRentals", new[] { "addressId" });
            DropIndex("dbo.TenantRentals", new[] { "personId" });
            DropIndex("dbo.TenantRentals", new[] { "contactId" });
            DropIndex("dbo.TenantRentals", new[] { "assetRentalId" });
            DropIndex("dbo.TenantRentals", new[] { "deptId" });
            DropIndex("dbo.TenantRentals", new[] { "companyId" });
            DropIndex("dbo.RentalContracts", new[] { "currencyId" });
            DropIndex("dbo.RentalContracts", new[] { "statusId" });
            DropIndex("dbo.RentalContracts", new[] { "creatorId" });
            DropIndex("dbo.RentalContracts", new[] { "tenantRentalId" });
            DropIndex("dbo.RentalContracts", new[] { "assetRentalId" });
            DropIndex("dbo.RentalContracts", new[] { "deptId" });
            DropIndex("dbo.RentalContracts", new[] { "companyId" });
            DropIndex("dbo.RentalOrders", new[] { "currencyId" });
            DropIndex("dbo.RentalOrders", new[] { "statusId" });
            DropIndex("dbo.RentalOrders", new[] { "creatorId" });
            DropIndex("dbo.RentalOrders", new[] { "TenantRentalId" });
            DropIndex("dbo.RentalOrders", new[] { "assetRentalId" });
            DropIndex("dbo.RentalOrders", new[] { "deptId" });
            DropIndex("dbo.RentalOrders", new[] { "companyId" });
            DropIndex("dbo.RentalOrders", new[] { "rentalContractId" });
            DropIndex("dbo.RentalInvoices", new[] { "currencyId" });
            DropIndex("dbo.RentalInvoices", new[] { "statusId" });
            DropIndex("dbo.RentalInvoices", new[] { "creatorId" });
            DropIndex("dbo.RentalInvoices", new[] { "TenantRentalId" });
            DropIndex("dbo.RentalInvoices", new[] { "assetRentalId" });
            DropIndex("dbo.RentalInvoices", new[] { "deptId" });
            DropIndex("dbo.RentalInvoices", new[] { "companyId" });
            DropIndex("dbo.RentalInvoices", new[] { "rentalOrderId" });
            DropIndex("dbo.ReceiptTaxes", new[] { "SalesReceiptForBankChargesTaxId" });
            DropIndex("dbo.ReceiptTaxes", new[] { "SalesReceiptForDeductionTaxId" });
            DropIndex("dbo.ReceiptTaxes", new[] { "taxNameId" });
            DropIndex("dbo.ReceiptDeductions", new[] { "SalesReceiptForBankChargesId" });
            DropIndex("dbo.ReceiptDeductions", new[] { "SalesReceiptForDeductionId" });
            DropIndex("dbo.ReceiptDeductions", new[] { "deduction_Id" });
            DropIndex("dbo.SalesReceipts", new[] { "VATBookRefId" });
            DropIndex("dbo.SalesReceipts", new[] { "statusClass_Id" });
            DropIndex("dbo.SalesReceipts", new[] { "transactionHolderId" });
            DropIndex("dbo.SalesReceipts", new[] { "COAcredit_Id" });
            DropIndex("dbo.SalesReceipts", new[] { "LoansAdvanceId" });
            DropIndex("dbo.SalesReceipts", new[] { "PettyCashRefId" });
            DropIndex("dbo.SalesReceipts", new[] { "coaAccountId" });
            DropIndex("dbo.SalesReceipts", new[] { "CostSheet_Id" });
            DropIndex("dbo.SalesReceipts", new[] { "principal_Id" });
            DropIndex("dbo.SalesReceipts", new[] { "user_Id" });
            DropIndex("dbo.SalesReceipts", new[] { "StatusId" });
            DropIndex("dbo.SalesReceipts", new[] { "paymentId" });
            DropIndex("dbo.SalesReceipts", new[] { "receiptId" });
            DropIndex("dbo.SalesReceipts", new[] { "saleInvoiceId" });
            DropIndex("dbo.SalesReceipts", new[] { "AccountId" });
            DropIndex("dbo.SalesReceipts", new[] { "BankId" });
            DropIndex("dbo.SalesReceipts", new[] { "collectionMethodId" });
            DropIndex("dbo.SalesReceipts", new[] { "rentalInvoiceId" });
            DropIndex("dbo.SalesReceipts", new[] { "CurrencyId" });
            DropIndex("dbo.SalesReceipts", new[] { "CustomerId" });
            DropIndex("dbo.SalesReceipts", new[] { "deptId" });
            DropIndex("dbo.SalesReceipts", new[] { "companyId" });
            DropIndex("dbo.SplitPERs", new[] { "saleOrderId" });
            DropIndex("dbo.SaleOrdeRrefKeys", new[] { "comp_Id" });
            DropIndex("dbo.SaleOrdeRrefKeys", new[] { "dept_Id" });
            DropIndex("dbo.PerformanceSheetHeads", new[] { "creatorId" });
            DropIndex("dbo.PerfomarmanceSheetFields", new[] { "PerformanceSheet_Id" });
            DropIndex("dbo.PerfomarmanceSheetFields", new[] { "Head_Id" });
            DropIndex("dbo.PerformanceSheets", new[] { "staffLevelTwoId" });
            DropIndex("dbo.PerformanceSheets", new[] { "staffLevelOneId" });
            DropIndex("dbo.PerformanceSheets", new[] { "supervoisedId" });
            DropIndex("dbo.PerformanceSheets", new[] { "deptId" });
            DropIndex("dbo.PerformanceSheets", new[] { "customerId" });
            DropIndex("dbo.Offers", new[] { "offerStatus_Id" });
            DropIndex("dbo.Offers", new[] { "costCenterCurrencyId" });
            DropIndex("dbo.Offers", new[] { "CostSheet_Id" });
            DropIndex("dbo.Offers", new[] { "statusClass_Id" });
            DropIndex("dbo.Offers", new[] { "transactionHolderId" });
            DropIndex("dbo.Offers", new[] { "TitleValue2Id" });
            DropIndex("dbo.Offers", new[] { "TitleValue1Id" });
            DropIndex("dbo.Offers", new[] { "incoterm_Id" });
            DropIndex("dbo.Offers", new[] { "paymentterm_Id" });
            DropIndex("dbo.Offers", new[] { "currency_Id" });
            DropIndex("dbo.Offers", new[] { "bid_Id" });
            DropIndex("dbo.Offers", new[] { "inquiry_Id" });
            DropIndex("dbo.Offers", new[] { "principal_Id" });
            DropIndex("dbo.Offers", new[] { "InterCompany_Id" });
            DropIndex("dbo.Offers", new[] { "InterDepartment_Id" });
            DropIndex("dbo.Offers", new[] { "company_Id" });
            DropIndex("dbo.Offers", new[] { "allocation_Id" });
            DropIndex("dbo.Offers", new[] { "user_Id" });
            DropIndex("dbo.Offers", new[] { "dept_Id" });
            DropIndex("dbo.Offers", new[] { "customerCompany_Id" });
            DropIndex("dbo.MemorandumSales", new[] { "ModuleContract_Id" });
            DropIndex("dbo.MemorandumSales", new[] { "memorandumSaleStatus_Id" });
            DropIndex("dbo.MemorandumSales", new[] { "TitleValue2Id" });
            DropIndex("dbo.MemorandumSales", new[] { "TitleValue1Id" });
            DropIndex("dbo.MemorandumSales", new[] { "Offer_Id" });
            DropIndex("dbo.MemorandumSales", new[] { "SaleOrder_Id" });
            DropIndex("dbo.MemorandumSales", new[] { "InterCompany_Id" });
            DropIndex("dbo.MemorandumSales", new[] { "InterDepartment_Id" });
            DropIndex("dbo.MemorandumSales", new[] { "company_Id" });
            DropIndex("dbo.MemorandumSales", new[] { "principal_Id" });
            DropIndex("dbo.MemorandumSales", new[] { "user_Id" });
            DropIndex("dbo.MemorandumSales", new[] { "dept_Id" });
            DropIndex("dbo.MemorandumSales", new[] { "customerCompany_Id" });
            DropIndex("dbo.VendorBillReferences", new[] { "companyId" });
            DropIndex("dbo.SaleOrders", new[] { "saleOrderStatus_Id" });
            DropIndex("dbo.SaleOrders", new[] { "moduleContract_Id" });
            DropIndex("dbo.SaleOrders", new[] { "auditYearAdjustment_Id" });
            DropIndex("dbo.SaleOrders", new[] { "statusClass_Id" });
            DropIndex("dbo.SaleOrders", new[] { "transactionHolderId" });
            DropIndex("dbo.SaleOrders", new[] { "SaleOrderKey_Id" });
            DropIndex("dbo.SaleOrders", new[] { "PerformanceSheet_Id" });
            DropIndex("dbo.SaleOrders", new[] { "Budget_Id" });
            DropIndex("dbo.SaleOrders", new[] { "ParentSO_Id" });
            DropIndex("dbo.SaleOrders", new[] { "interBankTransfer_Id" });
            DropIndex("dbo.SaleOrders", new[] { "taxNameId" });
            DropIndex("dbo.SaleOrders", new[] { "marketExchangerateId" });
            DropIndex("dbo.SaleOrders", new[] { "saleExchangerateId" });
            DropIndex("dbo.SaleOrders", new[] { "TitleValue2Id" });
            DropIndex("dbo.SaleOrders", new[] { "TitleValue1Id" });
            DropIndex("dbo.SaleOrders", new[] { "BillRefNoId" });
            DropIndex("dbo.SaleOrders", new[] { "incoterm_Id" });
            DropIndex("dbo.SaleOrders", new[] { "paymentterm_Id" });
            DropIndex("dbo.SaleOrders", new[] { "costCentercurrency_Id" });
            DropIndex("dbo.SaleOrders", new[] { "currency_Id" });
            DropIndex("dbo.SaleOrders", new[] { "bid_Id" });
            DropIndex("dbo.SaleOrders", new[] { "offer_Id" });
            DropIndex("dbo.SaleOrders", new[] { "vendorPaymentId" });
            DropIndex("dbo.SaleOrders", new[] { "InterCompany_Id" });
            DropIndex("dbo.SaleOrders", new[] { "InterDepartment_Id" });
            DropIndex("dbo.SaleOrders", new[] { "company_Id" });
            DropIndex("dbo.SaleOrders", new[] { "principal_Id" });
            DropIndex("dbo.SaleOrders", new[] { "allocation_Id" });
            DropIndex("dbo.SaleOrders", new[] { "user_Id" });
            DropIndex("dbo.SaleOrders", new[] { "CommissionSummarySheetId" });
            DropIndex("dbo.SaleOrders", new[] { "CostSheet_Id" });
            DropIndex("dbo.SaleOrders", new[] { "dept_Id" });
            DropIndex("dbo.SaleOrders", new[] { "WarrantyId" });
            DropIndex("dbo.SaleOrders", new[] { "customerCompany_Id" });
            DropIndex("dbo.LotNumbers", new[] { "deptId" });
            DropIndex("dbo.LotNumbers", new[] { "companyId" });
            DropIndex("dbo.CustomerCredits", new[] { "CustomerCompanyId" });
            DropIndex("dbo.CustomerCredits", new[] { "SaleInvoiceId" });
            DropIndex("dbo.SaleInvoices", new[] { "saleInvoiceStatus_Id" });
            DropIndex("dbo.SaleInvoices", new[] { "VATBookRefId" });
            DropIndex("dbo.SaleInvoices", new[] { "insuranceAppliedBy_Id" });
            DropIndex("dbo.SaleInvoices", new[] { "statusClass_Id" });
            DropIndex("dbo.SaleInvoices", new[] { "lotNumberId" });
            DropIndex("dbo.SaleInvoices", new[] { "stlSTLCurrency_Id" });
            DropIndex("dbo.SaleInvoices", new[] { "stlDiscountCurrency_Id" });
            DropIndex("dbo.SaleInvoices", new[] { "transactionHolderId" });
            DropIndex("dbo.SaleInvoices", new[] { "CostSheet_Id" });
            DropIndex("dbo.SaleInvoices", new[] { "account_Id" });
            DropIndex("dbo.SaleInvoices", new[] { "bank_Id" });
            DropIndex("dbo.SaleInvoices", new[] { "TitleValue2Id" });
            DropIndex("dbo.SaleInvoices", new[] { "TitleValue1Id" });
            DropIndex("dbo.SaleInvoices", new[] { "incoterm_Id" });
            DropIndex("dbo.SaleInvoices", new[] { "paymentterm_Id" });
            DropIndex("dbo.SaleInvoices", new[] { "currency_Id" });
            DropIndex("dbo.SaleInvoices", new[] { "SaleOrderId" });
            DropIndex("dbo.SaleInvoices", new[] { "InterCompany_Id" });
            DropIndex("dbo.SaleInvoices", new[] { "InterDepartment_Id" });
            DropIndex("dbo.SaleInvoices", new[] { "company_Id" });
            DropIndex("dbo.SaleInvoices", new[] { "principal_Id" });
            DropIndex("dbo.SaleInvoices", new[] { "allocation_Id" });
            DropIndex("dbo.SaleInvoices", new[] { "user_Id" });
            DropIndex("dbo.SaleInvoices", new[] { "CommissionSummarySheetId" });
            DropIndex("dbo.SaleInvoices", new[] { "dept_Id" });
            DropIndex("dbo.SaleInvoices", new[] { "customerCompany_Id" });
            DropIndex("dbo.InventoryAdjustments", new[] { "AdjustmentStatus_Id" });
            DropIndex("dbo.InventoryAdjustments", new[] { "chartofAccount_Id" });
            DropIndex("dbo.InventoryAdjustments", new[] { "currency_Id" });
            DropIndex("dbo.InventoryAdjustments", new[] { "depId_Id" });
            DropIndex("dbo.InventoryAdjustments", new[] { "company_Id" });
            DropIndex("dbo.InventoryAdjustments", new[] { "creator_Id" });
            DropIndex("dbo.Inventories", new[] { "adjustment_Id" });
            DropIndex("dbo.Inventories", new[] { "bookerItemId" });
            DropIndex("dbo.Inventories", new[] { "SaleInviceId" });
            DropIndex("dbo.Inventories", new[] { "PurchaseInvoiceId" });
            DropIndex("dbo.Inventories", new[] { "deptId" });
            DropIndex("dbo.Inventories", new[] { "companyId" });
            DropIndex("dbo.Inventories", new[] { "userId" });
            DropIndex("dbo.Inventories", new[] { "currencyId" });
            DropIndex("dbo.Inventories", new[] { "prodId" });
            DropIndex("dbo.PurchaseInvoices", new[] { "PurchaseInvoiceStatus_Id" });
            DropIndex("dbo.PurchaseInvoices", new[] { "VATBookRefId" });
            DropIndex("dbo.PurchaseInvoices", new[] { "statusClass_Id" });
            DropIndex("dbo.PurchaseInvoices", new[] { "transactionHolderId" });
            DropIndex("dbo.PurchaseInvoices", new[] { "CostSheet_Id" });
            DropIndex("dbo.PurchaseInvoices", new[] { "TitleValue2Id" });
            DropIndex("dbo.PurchaseInvoices", new[] { "TitleValue1Id" });
            DropIndex("dbo.PurchaseInvoices", new[] { "incoterm_Id" });
            DropIndex("dbo.PurchaseInvoices", new[] { "POPaymentterm_Id" });
            DropIndex("dbo.PurchaseInvoices", new[] { "tax_Id" });
            DropIndex("dbo.PurchaseInvoices", new[] { "vendorPaymentId" });
            DropIndex("dbo.PurchaseInvoices", new[] { "vendor_Id" });
            DropIndex("dbo.PurchaseInvoices", new[] { "allocation_Id" });
            DropIndex("dbo.PurchaseInvoices", new[] { "user_Id" });
            DropIndex("dbo.PurchaseInvoices", new[] { "customerCompany_Id" });
            DropIndex("dbo.PurchaseInvoices", new[] { "InterCompany_Id" });
            DropIndex("dbo.PurchaseInvoices", new[] { "InterDepartment_Id" });
            DropIndex("dbo.PurchaseInvoices", new[] { "company_Id" });
            DropIndex("dbo.PurchaseInvoices", new[] { "dept_Id" });
            DropIndex("dbo.PurchaseInvoices", new[] { "purchaseOrder_Id" });
            DropIndex("dbo.PurchaseInvoices", new[] { "currency_Id" });
            DropIndex("dbo.VATBooks", new[] { "VATBookRefNumberRefId" });
            DropIndex("dbo.VATBooks", new[] { "loansAdvanceId" });
            DropIndex("dbo.VATBooks", new[] { "interCompanyId" });
            DropIndex("dbo.VATBooks", new[] { "adminBillId" });
            DropIndex("dbo.VATBooks", new[] { "paymentId" });
            DropIndex("dbo.VATBooks", new[] { "interBankTransferId" });
            DropIndex("dbo.VATBooks", new[] { "saleReceiptId" });
            DropIndex("dbo.VATBooks", new[] { "vendorBillId" });
            DropIndex("dbo.VATBooks", new[] { "purchaseInvoiceId" });
            DropIndex("dbo.VATBooks", new[] { "saleInvoiceId" });
            DropIndex("dbo.VATBooks", new[] { "currencyId" });
            DropIndex("dbo.VATBooks", new[] { "deptId" });
            DropIndex("dbo.VATBooks", new[] { "vendorId" });
            DropIndex("dbo.VATBooks", new[] { "customerId" });
            DropIndex("dbo.VATBooks", new[] { "companyId" });
            DropIndex("dbo.VATBookRefNumbers", new[] { "companyId" });
            DropIndex("dbo.STLSettlements", new[] { "stl_Id" });
            DropIndex("dbo.ReversalSettlements", new[] { "stl_Id" });
            DropIndex("dbo.STLInterests", new[] { "COA_Id" });
            DropIndex("dbo.STLInterests", new[] { "interestTypeId" });
            DropIndex("dbo.MarginPercentages", new[] { "COA_Id" });
            DropIndex("dbo.MarginPercentages", new[] { "marginpercentageTypeId" });
            DropIndex("dbo.STLs", new[] { "vendor_Id" });
            DropIndex("dbo.STLs", new[] { "customer_Id" });
            DropIndex("dbo.STLs", new[] { "cashMarginPerc_Id" });
            DropIndex("dbo.STLs", new[] { "interest_Id" });
            DropIndex("dbo.STLs", new[] { "cashMarginDrAccount_Id" });
            DropIndex("dbo.STLs", new[] { "cashMarginCurrency_Id" });
            DropIndex("dbo.STLs", new[] { "cashMarginBank_Id" });
            DropIndex("dbo.STLs", new[] { "stlAccount_Id" });
            DropIndex("dbo.STLs", new[] { "stlCurrency_Id" });
            DropIndex("dbo.STLs", new[] { "stlBank_Id" });
            DropIndex("dbo.STLs", new[] { "paymentAccount_Id" });
            DropIndex("dbo.STLs", new[] { "paymentCurrency_Id" });
            DropIndex("dbo.STLs", new[] { "paymentBank_Id" });
            DropIndex("dbo.STLs", new[] { "interestAmountCurrency_Id" });
            DropIndex("dbo.STLs", new[] { "statusId" });
            DropIndex("dbo.STLs", new[] { "user_Id" });
            DropIndex("dbo.STLs", new[] { "dept_Id" });
            DropIndex("dbo.STLs", new[] { "company_Id" });
            DropIndex("dbo.InterCompBankTransferVATs", new[] { "InterCompanyBankTransferToId" });
            DropIndex("dbo.InterCompBankTransferVATs", new[] { "InterCompanyBankTransferFromId" });
            DropIndex("dbo.InterCompBankTransferVATs", new[] { "taxNameId" });
            DropIndex("dbo.InterCompBankTransferCharges", new[] { "InterCompanyBankTransferToId" });
            DropIndex("dbo.InterCompBankTransferCharges", new[] { "InterCompanyBankTransferFromId" });
            DropIndex("dbo.InterCompBankTransferCharges", new[] { "deduction_Id" });
            DropIndex("dbo.InterCompanyBankTransfers", new[] { "VATBookRefId" });
            DropIndex("dbo.InterCompanyBankTransfers", new[] { "statusClass_Id" });
            DropIndex("dbo.InterCompanyBankTransfers", new[] { "stlId" });
            DropIndex("dbo.InterCompanyBankTransfers", new[] { "adminBillId" });
            DropIndex("dbo.InterCompanyBankTransfers", new[] { "vendorBillId" });
            DropIndex("dbo.InterCompanyBankTransfers", new[] { "PettyCashRefToId" });
            DropIndex("dbo.InterCompanyBankTransfers", new[] { "PettyCashRefFromId" });
            DropIndex("dbo.InterCompanyBankTransfers", new[] { "currencyToId" });
            DropIndex("dbo.InterCompanyBankTransfers", new[] { "currencyFromId" });
            DropIndex("dbo.InterCompanyBankTransfers", new[] { "COAcredit_Id" });
            DropIndex("dbo.InterCompanyBankTransfers", new[] { "COAdebit_Id" });
            DropIndex("dbo.InterCompanyBankTransfers", new[] { "user_Id" });
            DropIndex("dbo.InterCompanyBankTransfers", new[] { "emp_Id" });
            DropIndex("dbo.InterCompanyBankTransfers", new[] { "accountTo_Id" });
            DropIndex("dbo.InterCompanyBankTransfers", new[] { "bankTo_Id" });
            DropIndex("dbo.InterCompanyBankTransfers", new[] { "deptTo_Id" });
            DropIndex("dbo.InterCompanyBankTransfers", new[] { "companyTo_Id" });
            DropIndex("dbo.InterCompanyBankTransfers", new[] { "accountFrom_Id" });
            DropIndex("dbo.InterCompanyBankTransfers", new[] { "bankFrom_Id" });
            DropIndex("dbo.InterCompanyBankTransfers", new[] { "deptFrom_Id" });
            DropIndex("dbo.InterCompanyBankTransfers", new[] { "companyFrom_Id" });
            DropIndex("dbo.InterCompanyBankTransfers", new[] { "currency_Id" });
            DropIndex("dbo.InterCompanyBankTransfers", new[] { "transferMethod_Id" });
            DropIndex("dbo.InterCompanyBankTransfers", new[] { "StatusId" });
            DropIndex("dbo.PettyCashes", new[] { "InterCompanyBankTransfer_Id1" });
            DropIndex("dbo.PettyCashes", new[] { "InterCompanyBankTransfer_Id" });
            DropIndex("dbo.PettyCashes", new[] { "PettyCashRefId" });
            DropIndex("dbo.PettyCashes", new[] { "billId" });
            DropIndex("dbo.PettyCashes", new[] { "LoansAdvanceId" });
            DropIndex("dbo.PettyCashes", new[] { "InterCompanyId" });
            DropIndex("dbo.PettyCashes", new[] { "AdminBillId" });
            DropIndex("dbo.PettyCashes", new[] { "PaymentId" });
            DropIndex("dbo.PettyCashes", new[] { "interBankTransferId" });
            DropIndex("dbo.PettyCashes", new[] { "SaleReceiptId" });
            DropIndex("dbo.PettyCashes", new[] { "currencyId" });
            DropIndex("dbo.PettyCashes", new[] { "deptId" });
            DropIndex("dbo.PettyCashes", new[] { "companyId" });
            DropIndex("dbo.PaymentTaxes", new[] { "Payment_Id" });
            DropIndex("dbo.PaymentTaxes", new[] { "taxNameId" });
            DropIndex("dbo.BillRefNumbers", new[] { "companyId" });
            DropIndex("dbo.PaymentDeductions", new[] { "Payment_Id" });
            DropIndex("dbo.PaymentDeductions", new[] { "deduction_id" });
            DropIndex("dbo.BudgetSystemCostFields", new[] { "Bill_Id" });
            DropIndex("dbo.BudgetSystemCostFields", new[] { "SalesReceipt_Id" });
            DropIndex("dbo.BudgetSystemCostFields", new[] { "SaleInvoice_Id" });
            DropIndex("dbo.BudgetSystemCostFields", new[] { "PurchaseInvoice_Id" });
            DropIndex("dbo.BudgetSystemCostFields", new[] { "Payment_Id" });
            DropIndex("dbo.BudgetSystemCostFields", new[] { "Head_Id" });
            DropIndex("dbo.Payments", new[] { "insuranceAppliedBy_Id" });
            DropIndex("dbo.Payments", new[] { "VATBookRefId" });
            DropIndex("dbo.Payments", new[] { "statusClass_Id" });
            DropIndex("dbo.Payments", new[] { "transactionHolderId" });
            DropIndex("dbo.Payments", new[] { "paymentterm_Id" });
            DropIndex("dbo.Payments", new[] { "LoansAdvanceId" });
            DropIndex("dbo.Payments", new[] { "coaAccountId" });
            DropIndex("dbo.Payments", new[] { "CostSheetId" });
            DropIndex("dbo.Payments", new[] { "InterDepartment_Id" });
            DropIndex("dbo.Payments", new[] { "InterCompany_Id" });
            DropIndex("dbo.Payments", new[] { "user_Id" });
            DropIndex("dbo.Payments", new[] { "PaymentRefNoId" });
            DropIndex("dbo.Payments", new[] { "accountId" });
            DropIndex("dbo.Payments", new[] { "bankId" });
            DropIndex("dbo.Payments", new[] { "paymentMethodId" });
            DropIndex("dbo.Payments", new[] { "statusId" });
            DropIndex("dbo.Payments", new[] { "currency_Id" });
            DropIndex("dbo.Payments", new[] { "vendor_Id" });
            DropIndex("dbo.Payments", new[] { "PrimaryCreditCardNoId" });
            DropIndex("dbo.Payments", new[] { "creditCardBankId" });
            DropIndex("dbo.Payments", new[] { "company_Id" });
            DropIndex("dbo.Payments", new[] { "taskGroups_Id" });
            DropIndex("dbo.Payments", new[] { "TargetReward_Id" });
            DropIndex("dbo.Payments", new[] { "PInvoice_Id" });
            DropIndex("dbo.Payments", new[] { "Bill_Id" });
            DropIndex("dbo.Payments", new[] { "AdminBill_Id" });
            DropIndex("dbo.LoanApplicantTypes", new[] { "accountId" });
            DropIndex("dbo.LoanApplicants", new[] { "applicantTypeId" });
            DropIndex("dbo.LoansAdvances", new[] { "VATBookRefId" });
            DropIndex("dbo.LoansAdvances", new[] { "statusClass_Id" });
            DropIndex("dbo.LoansAdvances", new[] { "PettyCashRefId" });
            DropIndex("dbo.LoansAdvances", new[] { "currencyId" });
            DropIndex("dbo.LoansAdvances", new[] { "purchaseOrderId" });
            DropIndex("dbo.LoansAdvances", new[] { "saleOrderId" });
            DropIndex("dbo.LoansAdvances", new[] { "SaleInvoiceId" });
            DropIndex("dbo.LoansAdvances", new[] { "statusId" });
            DropIndex("dbo.LoansAdvances", new[] { "vendorId" });
            DropIndex("dbo.LoansAdvances", new[] { "employeeId" });
            DropIndex("dbo.LoansAdvances", new[] { "applicantId" });
            DropIndex("dbo.LoansAdvances", new[] { "applicantTypeId" });
            DropIndex("dbo.LoansAdvances", new[] { "creatorId" });
            DropIndex("dbo.LoansAdvances", new[] { "deptId" });
            DropIndex("dbo.LoansAdvances", new[] { "companyId" });
            DropIndex("dbo.JournalVouchers", new[] { "purchaseOrder_Id" });
            DropIndex("dbo.JournalVouchers", new[] { "bill_Id" });
            DropIndex("dbo.JournalVouchers", new[] { "emp_Id" });
            DropIndex("dbo.JournalVouchers", new[] { "dept_Id" });
            DropIndex("dbo.JournalVouchers", new[] { "company_Id" });
            DropIndex("dbo.JournalVouchers", new[] { "currencyId" });
            DropIndex("dbo.JournalVouchers", new[] { "statusId" });
            DropIndex("dbo.JournalVouchers", new[] { "userId" });
            DropIndex("dbo.Warranties", new[] { "user_Id" });
            DropIndex("dbo.PaymentTerms", new[] { "ParentId" });
            DropIndex("dbo.PaymentTerms", new[] { "user_Id" });
            DropIndex("dbo.FieldValues", new[] { "CostSheet_Id" });
            DropIndex("dbo.FieldValues", new[] { "FieldId" });
            DropIndex("dbo.CostSheetSOFields", new[] { "CostSheetId" });
            DropIndex("dbo.CostSheetSOFields", new[] { "FieldId" });
            DropIndex("dbo.CostSheetSIFields", new[] { "CostSheetId" });
            DropIndex("dbo.CostSheetSIFields", new[] { "FieldId" });
            DropIndex("dbo.CostSheetSaleReceiptFields", new[] { "CostSheetId" });
            DropIndex("dbo.CostSheetSaleReceiptFields", new[] { "FieldId" });
            DropIndex("dbo.CostSheetPOFields", new[] { "CostSheetId" });
            DropIndex("dbo.CostSheetPOFields", new[] { "FieldId" });
            DropIndex("dbo.CostSheetPaymentFields", new[] { "CostSheetId" });
            DropIndex("dbo.CostSheetPaymentFields", new[] { "FieldId" });
            DropIndex("dbo.CostSheetOfferFields", new[] { "CostSheetId" });
            DropIndex("dbo.CostSheetOfferFields", new[] { "FieldId" });
            DropIndex("dbo.CostSheetBillFields", new[] { "CostSheetId" });
            DropIndex("dbo.CostSheetBillFields", new[] { "FieldId" });
            DropIndex("dbo.CostFieldHistories", new[] { "CostSheetId" });
            DropIndex("dbo.CostFieldHistories", new[] { "FieldId" });
            DropIndex("dbo.CostSheets", new[] { "incoterm_Id" });
            DropIndex("dbo.CostSheets", new[] { "paymentterm_Id" });
            DropIndex("dbo.CostSheets", new[] { "SupplierWarrantyId" });
            DropIndex("dbo.SummarySheetFields", new[] { "AddedbyUserId" });
            DropIndex("dbo.SummaryFieldValues", new[] { "CommissionSummarySheet_Id" });
            DropIndex("dbo.SummaryFieldValues", new[] { "FieldId" });
            DropIndex("dbo.CommissionSummarySheets", new[] { "OfferCurrencyId" });
            DropIndex("dbo.BudgetSheetHeads", new[] { "creatorId" });
            DropIndex("dbo.BudgetCostFields", new[] { "BudgetCostSheet_Id" });
            DropIndex("dbo.BudgetCostFields", new[] { "Head_Id" });
            DropIndex("dbo.BudgetCostSheets", new[] { "budgetCostSheetStatus_Id" });
            DropIndex("dbo.BudgetCostSheets", new[] { "currencyId" });
            DropIndex("dbo.BudgetCostSheets", new[] { "empId" });
            DropIndex("dbo.BudgetCostSheets", new[] { "deptId" });
            DropIndex("dbo.BudgetCostSheets", new[] { "companyId" });
            DropIndex("dbo.PurchaseOrders", new[] { "PurchaseOrderStatus_Id" });
            DropIndex("dbo.PurchaseOrders", new[] { "lotNumberId" });
            DropIndex("dbo.PurchaseOrders", new[] { "statusClass_Id" });
            DropIndex("dbo.PurchaseOrders", new[] { "transactionHolderId" });
            DropIndex("dbo.PurchaseOrders", new[] { "Budget_Id" });
            DropIndex("dbo.PurchaseOrders", new[] { "TitleValue2Id" });
            DropIndex("dbo.PurchaseOrders", new[] { "TitleValue1Id" });
            DropIndex("dbo.PurchaseOrders", new[] { "incoterm_Id" });
            DropIndex("dbo.PurchaseOrders", new[] { "SOCurrency_Id" });
            DropIndex("dbo.PurchaseOrders", new[] { "currency_Id" });
            DropIndex("dbo.PurchaseOrders", new[] { "bid_Id" });
            DropIndex("dbo.PurchaseOrders", new[] { "POPaymentterm_Id" });
            DropIndex("dbo.PurchaseOrders", new[] { "SoPaymentterm_Id" });
            DropIndex("dbo.PurchaseOrders", new[] { "vendorPaymentId" });
            DropIndex("dbo.PurchaseOrders", new[] { "allocation_Id" });
            DropIndex("dbo.PurchaseOrders", new[] { "user_Id" });
            DropIndex("dbo.PurchaseOrders", new[] { "POWarrantyId" });
            DropIndex("dbo.PurchaseOrders", new[] { "SOWarrantyId" });
            DropIndex("dbo.PurchaseOrders", new[] { "customerCompany_Id" });
            DropIndex("dbo.PurchaseOrders", new[] { "InterCompany_Id" });
            DropIndex("dbo.PurchaseOrders", new[] { "InterDepartment_Id" });
            DropIndex("dbo.PurchaseOrders", new[] { "company_Id" });
            DropIndex("dbo.PurchaseOrders", new[] { "dept_Id" });
            DropIndex("dbo.PurchaseOrders", new[] { "CommissionSummarySheetId" });
            DropIndex("dbo.PurchaseOrders", new[] { "CostSheet_Id" });
            DropIndex("dbo.PurchaseOrders", new[] { "saleOrder_Id" });
            DropIndex("dbo.PurchaseOrders", new[] { "WHT_Id" });
            DropIndex("dbo.PurchaseOrders", new[] { "tax_Id" });
            DropIndex("dbo.Incoterms", new[] { "user_Id" });
            DropIndex("dbo.ComparativeStatementItems", new[] { "comparativeStatementId" });
            DropIndex("dbo.ComparativeStatementItems", new[] { "itemCurrencyId" });
            DropIndex("dbo.ComparativeStatementItems", new[] { "convertedCurrencyId" });
            DropIndex("dbo.ComparativeStatementItems", new[] { "costSheetFieldId" });
            DropIndex("dbo.ComparativeStatements", new[] { "ModuleContract_Id" });
            DropIndex("dbo.ComparativeStatements", new[] { "productId" });
            DropIndex("dbo.ComparativeStatements", new[] { "vendorIncoTermId" });
            DropIndex("dbo.ComparativeStatements", new[] { "creatorId" });
            DropIndex("dbo.ComparativeStatements", new[] { "offerCurrencyId" });
            DropIndex("dbo.ComparativeStatements", new[] { "incoTerm_Id" });
            DropIndex("dbo.ComparativeStatements", new[] { "VendorId" });
            DropIndex("dbo.ComparativeStatements", new[] { "offerId" });
            DropIndex("dbo.ModuleContracts", new[] { "ModuleContractStatus_Id" });
            DropIndex("dbo.ModuleContracts", new[] { "InterCompany_Id" });
            DropIndex("dbo.ModuleContracts", new[] { "InterDepartment_Id" });
            DropIndex("dbo.ModuleContracts", new[] { "statusClass_Id" });
            DropIndex("dbo.ModuleContracts", new[] { "transactionHolderId" });
            DropIndex("dbo.ModuleContracts", new[] { "TitleValue2Id" });
            DropIndex("dbo.ModuleContracts", new[] { "TitleValue1Id" });
            DropIndex("dbo.ModuleContracts", new[] { "incoterm_Id" });
            DropIndex("dbo.ModuleContracts", new[] { "paymentterm_Id" });
            DropIndex("dbo.ModuleContracts", new[] { "currency_Id" });
            DropIndex("dbo.ModuleContracts", new[] { "bid_Id" });
            DropIndex("dbo.ModuleContracts", new[] { "offer_Id" });
            DropIndex("dbo.ModuleContracts", new[] { "principal_Id" });
            DropIndex("dbo.ModuleContracts", new[] { "company_Id" });
            DropIndex("dbo.ModuleContracts", new[] { "allocation_Id" });
            DropIndex("dbo.ModuleContracts", new[] { "user_Id" });
            DropIndex("dbo.ModuleContracts", new[] { "dept_Id" });
            DropIndex("dbo.ModuleContracts", new[] { "customerCompany_Id" });
            DropIndex("dbo.FOCSamplings", new[] { "chartofAccountId" });
            DropIndex("dbo.ClaimDiscounts", new[] { "chartofAccountId" });
            DropIndex("dbo.BookerStatementItems", new[] { "purchaseInvoiceId" });
            DropIndex("dbo.BookerStatementItems", new[] { "purchaseOrderId" });
            DropIndex("dbo.BookerStatementItems", new[] { "saleInvoiceId" });
            DropIndex("dbo.BookerStatementItems", new[] { "passOnId" });
            DropIndex("dbo.BookerStatementItems", new[] { "saleOrderId" });
            DropIndex("dbo.BookerStatementItems", new[] { "claimDiscountId" });
            DropIndex("dbo.BookerStatementItems", new[] { "focSamplingId" });
            DropIndex("dbo.BookerStatementItems", new[] { "taxNameId" });
            DropIndex("dbo.BookerStatementItems", new[] { "product_Id" });
            DropIndex("dbo.BookerStatementItems", new[] { "ModuleContractId" });
            DropIndex("dbo.BookerStatementItems", new[] { "offerId" });
            DropIndex("dbo.Products", new[] { "cAssetAccount_id" });
            DropIndex("dbo.Products", new[] { "cgsInvenAccount_id" });
            DropIndex("dbo.Products", new[] { "company_id" });
            DropIndex("dbo.Products", new[] { "cgsAccount_id" });
            DropIndex("dbo.Products", new[] { "parentId" });
            DropIndex("dbo.Products", new[] { "incomeAccount_id" });
            DropIndex("dbo.Products", new[] { "user_Id" });
            DropIndex("dbo.Products", new[] { "categoryId" });
            DropIndex("dbo.Products", new[] { "unitOfMeasureId" });
            DropIndex("dbo.Products", new[] { "nature_Id" });
            DropIndex("dbo.Products", new[] { "code" });
            DropIndex("dbo.InquiryProducts", new[] { "Inquiry_Id" });
            DropIndex("dbo.InquiryProducts", new[] { "product_Id" });
            DropIndex("dbo.Inquiries", new[] { "inquiryStatus_Id" });
            DropIndex("dbo.Inquiries", new[] { "statusClass_Id" });
            DropIndex("dbo.Inquiries", new[] { "transactionHolderId" });
            DropIndex("dbo.Inquiries", new[] { "allocation_Id" });
            DropIndex("dbo.Inquiries", new[] { "user_Id" });
            DropIndex("dbo.Inquiries", new[] { "InterCompany_Id" });
            DropIndex("dbo.Inquiries", new[] { "InterDepartment_Id" });
            DropIndex("dbo.Inquiries", new[] { "company_Id" });
            DropIndex("dbo.Inquiries", new[] { "dept_Id" });
            DropIndex("dbo.Inquiries", new[] { "customerCompany_Id" });
            DropIndex("dbo.DocumentAuthorities", new[] { "documentTemplate_Id" });
            DropIndex("dbo.Documents", new[] { "employee_Id" });
            DropIndex("dbo.Documents", new[] { "statusClass_Id" });
            DropIndex("dbo.Documents", new[] { "status_Id" });
            DropIndex("dbo.Documents", new[] { "creator_Id" });
            DropIndex("dbo.Documents", new[] { "country_Id" });
            DropIndex("dbo.Documents", new[] { "company_Id" });
            DropIndex("dbo.Documents", new[] { "dept_Id" });
            DropIndex("dbo.Documents", new[] { "documentAuthority_Id" });
            DropIndex("dbo.Documents", new[] { "documentTemplate_Id" });
            DropIndex("dbo.Documents", new[] { "documentType_Id" });
            DropIndex("dbo.TaxNames", new[] { "COA_Id" });
            DropIndex("dbo.TaxNames", new[] { "taxTypeId" });
            DropIndex("dbo.InterBankTransferVATs", new[] { "InterBankTransferToId" });
            DropIndex("dbo.InterBankTransferVATs", new[] { "InterBankTransferFromId" });
            DropIndex("dbo.InterBankTransferVATs", new[] { "taxNameId" });
            DropIndex("dbo.Principals", new[] { "shippingAddress_Id" });
            DropIndex("dbo.Principals", new[] { "contactPerson_Id" });
            DropIndex("dbo.Principals", new[] { "contact_Id" });
            DropIndex("dbo.Principals", new[] { "company_Id" });
            DropIndex("dbo.Principals", new[] { "billingAddres_Id" });
            DropIndex("dbo.Principals", new[] { "ParentID" });
            DropIndex("dbo.AuditYearAdjustments", new[] { "auditCurrency_Id" });
            DropIndex("dbo.AuditYearAdjustments", new[] { "principal_Id" });
            DropIndex("dbo.AuditYearAdjustments", new[] { "allocation_Id" });
            DropIndex("dbo.AuditYearAdjustments", new[] { "customerCompany_Id" });
            DropIndex("dbo.AuditYearAdjustments", new[] { "dept_Id" });
            DropIndex("dbo.AuditYearAdjustments", new[] { "company_Id" });
            DropIndex("dbo.AuditYearAdjustments", new[] { "user_Id" });
            DropIndex("dbo.ContactPersons", new[] { "person_Id" });
            DropIndex("dbo.ContactPersons", new[] { "contact_Id" });
            DropIndex("dbo.ContactPersons", new[] { "bank_Id" });
            DropIndex("dbo.ContactPersons", new[] { "ReligionId" });
            DropIndex("dbo.ContactPersons", new[] { "customerCompanyId" });
            DropIndex("dbo.CustomerCompanies", new[] { "shippingAddress_Id" });
            DropIndex("dbo.CustomerCompanies", new[] { "contactPerson_Id" });
            DropIndex("dbo.CustomerCompanies", new[] { "contact_Id" });
            DropIndex("dbo.CustomerCompanies", new[] { "company_Id" });
            DropIndex("dbo.CustomerCompanies", new[] { "billingAddres_Id" });
            DropIndex("dbo.CustomerCompanies", new[] { "ParentID" });
            DropIndex("dbo.IndustryTypes", new[] { "user_Id" });
            DropIndex("dbo.Deductions", new[] { "chartofAccountId" });
            DropIndex("dbo.IBTbankCharges", new[] { "InterBankTransferToId" });
            DropIndex("dbo.IBTbankCharges", new[] { "InterBankTransferFromId" });
            DropIndex("dbo.IBTbankCharges", new[] { "deduction_Id" });
            DropIndex("dbo.InterBankTransfers", new[] { "VATBookRefId" });
            DropIndex("dbo.InterBankTransfers", new[] { "statusClass_Id" });
            DropIndex("dbo.InterBankTransfers", new[] { "stlId" });
            DropIndex("dbo.InterBankTransfers", new[] { "adminBillId" });
            DropIndex("dbo.InterBankTransfers", new[] { "vendorBillId" });
            DropIndex("dbo.InterBankTransfers", new[] { "PettyCashRefId" });
            DropIndex("dbo.InterBankTransfers", new[] { "currencyToId" });
            DropIndex("dbo.InterBankTransfers", new[] { "currencyFromId" });
            DropIndex("dbo.InterBankTransfers", new[] { "vendor_Id" });
            DropIndex("dbo.InterBankTransfers", new[] { "industryTypeId" });
            DropIndex("dbo.InterBankTransfers", new[] { "taxNameId" });
            DropIndex("dbo.InterBankTransfers", new[] { "taxTypeId" });
            DropIndex("dbo.InterBankTransfers", new[] { "user_Id" });
            DropIndex("dbo.InterBankTransfers", new[] { "transferMethod_Id" });
            DropIndex("dbo.InterBankTransfers", new[] { "statusId" });
            DropIndex("dbo.InterBankTransfers", new[] { "accountTo_Id" });
            DropIndex("dbo.InterBankTransfers", new[] { "bankTo_Id" });
            DropIndex("dbo.InterBankTransfers", new[] { "accountFrom_Id" });
            DropIndex("dbo.InterBankTransfers", new[] { "bankFrom_Id" });
            DropIndex("dbo.InterBankTransfers", new[] { "currency_Id" });
            DropIndex("dbo.InterBankTransfers", new[] { "emp_Id" });
            DropIndex("dbo.InterBankTransfers", new[] { "dept_Id" });
            DropIndex("dbo.InterBankTransfers", new[] { "company_Id" });
            DropIndex("dbo.JournalTransactions", new[] { "STLId" });
            DropIndex("dbo.JournalTransactions", new[] { "TargetRewardId" });
            DropIndex("dbo.JournalTransactions", new[] { "InterCompanyId" });
            DropIndex("dbo.JournalTransactions", new[] { "currencyId" });
            DropIndex("dbo.JournalTransactions", new[] { "companyId" });
            DropIndex("dbo.JournalTransactions", new[] { "PurchaseInvoiceId" });
            DropIndex("dbo.JournalTransactions", new[] { "PaymentId" });
            DropIndex("dbo.JournalTransactions", new[] { "costSheetFieldId" });
            DropIndex("dbo.JournalTransactions", new[] { "Bill_Id" });
            DropIndex("dbo.JournalTransactions", new[] { "ReconcilationId" });
            DropIndex("dbo.JournalTransactions", new[] { "AdminBillId" });
            DropIndex("dbo.JournalTransactions", new[] { "deptId" });
            DropIndex("dbo.JournalTransactions", new[] { "SaleReceiptId" });
            DropIndex("dbo.JournalTransactions", new[] { "prodId" });
            DropIndex("dbo.JournalTransactions", new[] { "SaleInvoiceId" });
            DropIndex("dbo.JournalTransactions", new[] { "InterBankId" });
            DropIndex("dbo.JournalTransactions", new[] { "journalVoucher_id" });
            DropIndex("dbo.JournalTransactions", new[] { "userId" });
            DropIndex("dbo.JournalTransactions", new[] { "accountId" });
            DropIndex("dbo.CostSheetFields", new[] { "AddedbyUserId" });
            DropIndex("dbo.ProcurementProducts", new[] { "Bill_Id" });
            DropIndex("dbo.ProcurementProducts", new[] { "Checklist_Id" });
            DropIndex("dbo.ProcurementProducts", new[] { "ModuleContract_Id" });
            DropIndex("dbo.ProcurementProducts", new[] { "PurchaseOrder_Id" });
            DropIndex("dbo.ProcurementProducts", new[] { "PurchaseInvoice_Id" });
            DropIndex("dbo.ProcurementProducts", new[] { "SaleOrder_Id" });
            DropIndex("dbo.ProcurementProducts", new[] { "MemorandumSale_Id" });
            DropIndex("dbo.ProcurementProducts", new[] { "Offer_Id" });
            DropIndex("dbo.ProcurementProducts", new[] { "SaleInvoice_Id" });
            DropIndex("dbo.ProcurementProducts", new[] { "InterCompanyBankTransfer_Id" });
            DropIndex("dbo.ProcurementProducts", new[] { "interBankTransferId" });
            DropIndex("dbo.ProcurementProducts", new[] { "debitAccountId" });
            DropIndex("dbo.ProcurementProducts", new[] { "creditAccountId" });
            DropIndex("dbo.ProcurementProducts", new[] { "fieldId" });
            DropIndex("dbo.ProcurementProducts", new[] { "product_Id" });
            DropIndex("dbo.ProcurementProducts", new[] { "packingStyleId" });
            DropIndex("dbo.Checklists", new[] { "creatorId" });
            DropIndex("dbo.Tasks", new[] { "statusClass_Id" });
            DropIndex("dbo.Tasks", new[] { "checklistId" });
            DropIndex("dbo.Tasks", new[] { "lotNumberId" });
            DropIndex("dbo.Tasks", new[] { "warehouseId" });
            DropIndex("dbo.Tasks", new[] { "employeeId" });
            DropIndex("dbo.Tasks", new[] { "vendorId" });
            DropIndex("dbo.Tasks", new[] { "taskTypeId" });
            DropIndex("dbo.Tasks", new[] { "moduleContractId" });
            DropIndex("dbo.Tasks", new[] { "offerId" });
            DropIndex("dbo.Tasks", new[] { "inquiryId" });
            DropIndex("dbo.Tasks", new[] { "saleInvoiceId" });
            DropIndex("dbo.Tasks", new[] { "purchaseOrderId" });
            DropIndex("dbo.Tasks", new[] { "saleOrderId" });
            DropIndex("dbo.Tasks", new[] { "currencyId" });
            DropIndex("dbo.Tasks", new[] { "creatorId" });
            DropIndex("dbo.Tasks", new[] { "assignedById" });
            DropIndex("dbo.Tasks", new[] { "assignedToId" });
            DropIndex("dbo.Tasks", new[] { "supervisedById" });
            DropIndex("dbo.Tasks", new[] { "statusId" });
            DropIndex("dbo.Tasks", new[] { "customerId" });
            DropIndex("dbo.Tasks", new[] { "deptId" });
            DropIndex("dbo.Tasks", new[] { "companyId" });
            DropIndex("dbo.Permissions", new[] { "ParentId" });
            DropIndex("dbo.Roles", new[] { "parentId" });
            DropIndex("dbo.Polls", new[] { "initiatedById" });
            DropIndex("dbo.Polls", new[] { "taskGroupId" });
            DropIndex("dbo.LoginUserDetails", new[] { "userId" });
            DropIndex("dbo.TargetGroups", new[] { "parentId" });
            DropIndex("dbo.SalesExchangeRates", new[] { "company_Id" });
            DropIndex("dbo.SalesExchangeRates", new[] { "Editedbyuser_Id" });
            DropIndex("dbo.SalesExchangeRates", new[] { "Addedbyuser_Id" });
            DropIndex("dbo.SalesExchangeRates", new[] { "base_currency_Id" });
            DropIndex("dbo.SalesExchangeRates", new[] { "target_currency_Id" });
            DropIndex("dbo.MarketExchangeRates", new[] { "company_Id" });
            DropIndex("dbo.MarketExchangeRates", new[] { "Editedbyuser_Id" });
            DropIndex("dbo.MarketExchangeRates", new[] { "Addedbyuser_Id" });
            DropIndex("dbo.MarketExchangeRates", new[] { "base_currency_Id" });
            DropIndex("dbo.MarketExchangeRates", new[] { "target_currency_Id" });
            DropIndex("dbo.StatusCalculationTypes", new[] { "TotalField_Id" });
            DropIndex("dbo.StatusCalculationTypes", new[] { "AchievedField_Id" });
            DropIndex("dbo.TaskGroups", new[] { "targetGroup_Id" });
            DropIndex("dbo.TaskGroups", new[] { "CalculationType_Id" });
            DropIndex("dbo.TaskGroups", new[] { "currency_Id" });
            DropIndex("dbo.TaskGroups", new[] { "cgsAccount_Id" });
            DropIndex("dbo.TaskGroups", new[] { "payableAccount_Id" });
            DropIndex("dbo.TaskGroups", new[] { "GroupCreatorId" });
            DropIndex("dbo.TaskGroups", new[] { "parentId" });
            DropIndex("dbo.Memos", new[] { "taskGroupId" });
            DropIndex("dbo.Memos", new[] { "createdForId" });
            DropIndex("dbo.Memos", new[] { "createdById" });
            DropIndex("dbo.Notifications", new[] { "FlagId" });
            DropIndex("dbo.Notifications", new[] { "SendingUserId" });
            DropIndex("dbo.Notifications", new[] { "CcUserId" });
            DropIndex("dbo.Notifications", new[] { "UserId" });
            DropIndex("dbo.TransactionItems", new[] { "AttachmentCategory_Id" });
            DropIndex("dbo.TransactionItems", new[] { "CommentCategory_Id" });
            DropIndex("dbo.CommentCategories", new[] { "user_Id" });
            DropIndex("dbo.CommentLogs", new[] { "financePersonId" });
            DropIndex("dbo.CommentLogs", new[] { "salesPersonId" });
            DropIndex("dbo.CommentLogs", new[] { "managerId" });
            DropIndex("dbo.CommentLogs", new[] { "FlagId" });
            DropIndex("dbo.CommentLogs", new[] { "CategoryId" });
            DropIndex("dbo.CommentLogs", new[] { "ReplyCommentId" });
            DropIndex("dbo.CommentLogs", new[] { "AssigneeId" });
            DropIndex("dbo.CommentLogs", new[] { "employeeId" });
            DropIndex("dbo.Users", new[] { "EmployeePerformanceReview_Id" });
            DropIndex("dbo.Users", new[] { "userName" });
            DropIndex("dbo.Users", new[] { "employeeId" });
            DropIndex("dbo.AssetRentalUnits", new[] { "assetRentalLocationId" });
            DropIndex("dbo.AssetRentalUnits", new[] { "countryId" });
            DropIndex("dbo.AssetRentalUnits", new[] { "cityId" });
            DropIndex("dbo.Cities", new[] { "countryId" });
            DropIndex("dbo.AssetRentalLocations", new[] { "countryId" });
            DropIndex("dbo.AssetRentalLocations", new[] { "cityId" });
            DropIndex("dbo.RentalAssetSubNatures", new[] { "assetNatureId" });
            DropIndex("dbo.AssetModels", new[] { "assetNatureId" });
            DropIndex("dbo.AssetBrands", new[] { "assetNatureId" });
            DropIndex("dbo.AssetRentals", new[] { "statusId" });
            DropIndex("dbo.AssetRentals", new[] { "addressId" });
            DropIndex("dbo.AssetRentals", new[] { "assetRentalUnitId" });
            DropIndex("dbo.AssetRentals", new[] { "assetRentalLocationId" });
            DropIndex("dbo.AssetRentals", new[] { "cityId" });
            DropIndex("dbo.AssetRentals", new[] { "countryId" });
            DropIndex("dbo.AssetRentals", new[] { "parentId" });
            DropIndex("dbo.AssetRentals", new[] { "creatorId" });
            DropIndex("dbo.AssetRentals", new[] { "assetHolderEmployeeId" });
            DropIndex("dbo.AssetRentals", new[] { "vendorId" });
            DropIndex("dbo.AssetRentals", new[] { "assetTypeId" });
            DropIndex("dbo.AssetRentals", new[] { "assetNumberId" });
            DropIndex("dbo.AssetRentals", new[] { "assetModelId" });
            DropIndex("dbo.AssetRentals", new[] { "assetBrandId" });
            DropIndex("dbo.AssetRentals", new[] { "assetSubNatureId" });
            DropIndex("dbo.AssetRentals", new[] { "AssetNatureId" });
            DropIndex("dbo.AssetRentals", new[] { "deptId" });
            DropIndex("dbo.AssetRentals", new[] { "companyId" });
            DropIndex("dbo.tabAdminBill", new[] { "VATBookRefId" });
            DropIndex("dbo.tabAdminBill", new[] { "statusClass_Id" });
            DropIndex("dbo.tabAdminBill", new[] { "LoansAdvanceId" });
            DropIndex("dbo.tabAdminBill", new[] { "tax_Id" });
            DropIndex("dbo.tabAdminBill", new[] { "adminBillNature_Id" });
            DropIndex("dbo.tabAdminBill", new[] { "managementSummary_Id" });
            DropIndex("dbo.tabAdminBill", new[] { "user_Id" });
            DropIndex("dbo.tabAdminBill", new[] { "BillRefNoId" });
            DropIndex("dbo.tabAdminBill", new[] { "SecondaryCreditCardNoId" });
            DropIndex("dbo.tabAdminBill", new[] { "PrimaryCreditCardNoId" });
            DropIndex("dbo.tabAdminBill", new[] { "BankId" });
            DropIndex("dbo.tabAdminBill", new[] { "CardUserId" });
            DropIndex("dbo.tabAdminBill", new[] { "payee_Id" });
            DropIndex("dbo.tabAdminBill", new[] { "employeeForBill_Id" });
            DropIndex("dbo.tabAdminBill", new[] { "vendor_Id" });
            DropIndex("dbo.tabAdminBill", new[] { "adminBillType_Id" });
            DropIndex("dbo.tabAdminBill", new[] { "CoaCredit_Id" });
            DropIndex("dbo.tabAdminBill", new[] { "COA_Id" });
            DropIndex("dbo.tabAdminBill", new[] { "creatorId" });
            DropIndex("dbo.tabAdminBill", new[] { "statusId" });
            DropIndex("dbo.tabAdminBill", new[] { "currency_Id" });
            DropIndex("dbo.tabAdminBill", new[] { "assetRentalId" });
            DropIndex("dbo.tabAdminBill", new[] { "emp_Id" });
            DropIndex("dbo.tabAdminBill", new[] { "dept_Id" });
            DropIndex("dbo.tabAdminBill", new[] { "company_Id" });
            DropIndex("dbo.tabAdminBill", new[] { "template_Id" });
            DropIndex("dbo.Adjustments", new[] { "vendorBill_Id" });
            DropIndex("dbo.Adjustments", new[] { "adminBillId" });
            DropIndex("dbo.VendorBillAdjustments", new[] { "bill_Id" });
            DropIndex("dbo.Bills", new[] { "BillStatus_Id" });
            DropIndex("dbo.Bills", new[] { "VATBookRefId" });
            DropIndex("dbo.Bills", new[] { "statusClass_Id" });
            DropIndex("dbo.Bills", new[] { "LoansAdvanceId" });
            DropIndex("dbo.Bills", new[] { "transactionHolderId" });
            DropIndex("dbo.Bills", new[] { "vendorBillNature_Id" });
            DropIndex("dbo.Bills", new[] { "BillRefNoId" });
            DropIndex("dbo.Bills", new[] { "managementSummary_Id" });
            DropIndex("dbo.Bills", new[] { "billCategoryId" });
            DropIndex("dbo.Bills", new[] { "PettyCashRefId" });
            DropIndex("dbo.Bills", new[] { "CreditCardNoId" });
            DropIndex("dbo.Bills", new[] { "CardUserId" });
            DropIndex("dbo.Bills", new[] { "TitleValue2Id" });
            DropIndex("dbo.Bills", new[] { "TitleValue1Id" });
            DropIndex("dbo.Bills", new[] { "incoterm_Id" });
            DropIndex("dbo.Bills", new[] { "SOCurrency_Id" });
            DropIndex("dbo.Bills", new[] { "currency_Id" });
            DropIndex("dbo.Bills", new[] { "bid_Id" });
            DropIndex("dbo.Bills", new[] { "POPaymentterm_Id" });
            DropIndex("dbo.Bills", new[] { "SoPaymentterm_Id" });
            DropIndex("dbo.Bills", new[] { "POVendor_Id" });
            DropIndex("dbo.Bills", new[] { "billVendor_Id" });
            DropIndex("dbo.Bills", new[] { "vendor_Id" });
            DropIndex("dbo.Bills", new[] { "vendorPaymentId" });
            DropIndex("dbo.Bills", new[] { "allocation_Id" });
            DropIndex("dbo.Bills", new[] { "user_Id" });
            DropIndex("dbo.Bills", new[] { "POWarrantyId" });
            DropIndex("dbo.Bills", new[] { "SOWarrantyId" });
            DropIndex("dbo.Bills", new[] { "customerCompany_Id" });
            DropIndex("dbo.Bills", new[] { "loanAdvanceDept_Id" });
            DropIndex("dbo.Bills", new[] { "loanAdvanceCompany_Id" });
            DropIndex("dbo.Bills", new[] { "InterCompany_Id" });
            DropIndex("dbo.Bills", new[] { "InterDepartment_Id" });
            DropIndex("dbo.Bills", new[] { "company_Id" });
            DropIndex("dbo.Bills", new[] { "dept_Id" });
            DropIndex("dbo.Bills", new[] { "CommissionSummarySheetId" });
            DropIndex("dbo.Bills", new[] { "CostSheet_Id" });
            DropIndex("dbo.Bills", new[] { "purchaseOrder_Id" });
            DropIndex("dbo.Bills", new[] { "saleOrder_Id" });
            DropIndex("dbo.Bills", new[] { "billType_Id" });
            DropIndex("dbo.Bills", new[] { "WHT_Id" });
            DropIndex("dbo.Bills", new[] { "tax_Id" });
            DropIndex("dbo.tabVendor", new[] { "shippingAddress_Id" });
            DropIndex("dbo.tabVendor", new[] { "contactPerson_Id" });
            DropIndex("dbo.tabVendor", new[] { "contact_Id" });
            DropIndex("dbo.tabVendor", new[] { "company_Id" });
            DropIndex("dbo.tabVendor", new[] { "billingAddres_Id" });
            DropIndex("dbo.tabVendor", new[] { "vendorNatureManualId" });
            DropIndex("dbo.tabVendor", new[] { "vendorNatureId" });
            DropIndex("dbo.tabVendor", new[] { "ParentID" });
            DropIndex("dbo.Payees", new[] { "vehicleOwnerId" });
            DropIndex("dbo.Payees", new[] { "companyId" });
            DropIndex("dbo.Payees", new[] { "ParentId" });
            DropIndex("dbo.AdminBillTypes", new[] { "user_Id" });
            DropIndex("dbo.ChartofAccounts", new[] { "currencyId" });
            DropIndex("dbo.ChartofAccounts", new[] { "userId" });
            DropIndex("dbo.ChartofAccounts", new[] { "parentId" });
            DropIndex("dbo.tabDepartment", new[] { "accountPayableId" });
            DropIndex("dbo.tabDepartment", new[] { "chartofAccountId" });
            DropIndex("dbo.tabDepartment", new[] { "LevelID" });
            DropIndex("dbo.tabDepartment", new[] { "ParentID" });
            DropIndex("dbo.tabDepartment", new[] { "userID" });
            DropIndex("dbo.Designations", new[] { "userId" });
            DropIndex("dbo.Designations", new[] { "departmentId" });
            DropIndex("dbo.Designations", new[] { "companyId" });
            DropIndex("dbo.Designations", new[] { "ParentId" });
            DropIndex("dbo.PersonPhotoes", new[] { "Person_Id" });
            DropIndex("dbo.AssetOwners", new[] { "owner_Id" });
            DropIndex("dbo.AssetOwners", new[] { "contact_Id" });
            DropIndex("dbo.AssetOwners", new[] { "address_Id" });
            DropIndex("dbo.AssetNatures", new[] { "parentId" });
            DropIndex("dbo.Assets", new[] { "Employee_EmpId" });
            DropIndex("dbo.Assets", new[] { "RentalAssets_Id" });
            DropIndex("dbo.Assets", new[] { "purchaseInfo_Id" });
            DropIndex("dbo.Assets", new[] { "owner_EmpId" });
            DropIndex("dbo.Assets", new[] { "handler_id" });
            DropIndex("dbo.Assets", new[] { "designation_DesigId" });
            DropIndex("dbo.Assets", new[] { "assetStatus_Id" });
            DropIndex("dbo.Assets", new[] { "address_Id" });
            DropIndex("dbo.Assets", new[] { "user_Id" });
            DropIndex("dbo.Assets", new[] { "CoOwnerAssetId" });
            DropIndex("dbo.Assets", new[] { "OwnerId" });
            DropIndex("dbo.Assets", new[] { "coOwnerID" });
            DropIndex("dbo.Assets", new[] { "deptId" });
            DropIndex("dbo.Assets", new[] { "companyId" });
            DropIndex("dbo.Assets", new[] { "parentId" });
            DropIndex("dbo.Assets", new[] { "AssetNatureId" });
            DropIndex("dbo.Employees", new[] { "person_Id" });
            DropIndex("dbo.Employees", new[] { "employeeStatus_Id" });
            DropIndex("dbo.Employees", new[] { "employeeApproval_Id" });
            DropIndex("dbo.Employees", new[] { "empFunction_Id" });
            DropIndex("dbo.Employees", new[] { "emergencyontact_Id" });
            DropIndex("dbo.Employees", new[] { "Desig_DesigId" });
            DropIndex("dbo.Employees", new[] { "contact_Id" });
            DropIndex("dbo.Employees", new[] { "address2_Id" });
            DropIndex("dbo.Employees", new[] { "address_Id" });
            DropIndex("dbo.Employees", new[] { "receivableAccountId" });
            DropIndex("dbo.Employees", new[] { "HrInfoId" });
            DropIndex("dbo.Employees", new[] { "SalesTargetId" });
            DropIndex("dbo.Employees", new[] { "SupervisorId" });
            DropIndex("dbo.Employees", new[] { "coreDeptId" });
            DropIndex("dbo.Employees", new[] { "coreCompanyId" });
            DropIndex("dbo.tabCompany", new[] { "CurrencyId" });
            DropIndex("dbo.tabCompany", new[] { "ParentID" });
            DropIndex("dbo.tabCompany", new[] { "contactId" });
            DropIndex("dbo.tabCompany", new[] { "addressId" });
            DropIndex("dbo.tabCompany", new[] { "industryTypeId" });
            DropIndex("dbo.Banks", new[] { "contact_Id" });
            DropIndex("dbo.Banks", new[] { "address_Id" });
            DropIndex("dbo.Banks", new[] { "bankId" });
            DropIndex("dbo.Accounts", new[] { "currency_Id" });
            DropIndex("dbo.Accounts", new[] { "company_Id" });
            DropIndex("dbo.Accounts", new[] { "bank_Id" });
            DropIndex("dbo.Accounts", new[] { "vendor_Id" });
            DropIndex("dbo.Accounts", new[] { "industryTypeId" });
            DropIndex("dbo.Accounts", new[] { "COA_accountId" });
            DropIndex("dbo.Accounts", new[] { "mainBankId" });
            DropTable("dbo.CompanyVendors");
            DropTable("dbo.CompanyTaskTypes");
            DropTable("dbo.CompanyTaskGroups1");
            DropTable("dbo.CompanyTaskGroups");
            DropTable("dbo.CompanyEmployee1");
            DropTable("dbo.CompanyPayees");
            DropTable("dbo.CompanyLoanApplicants");
            DropTable("dbo.CompanyDocumentTypes");
            DropTable("dbo.CompanyDocumentTemplates");
            DropTable("dbo.CompanyCustomerCompanies");
            DropTable("dbo.CompanyChartofAccountGroups");
            DropTable("dbo.CompanyBanks");
            DropTable("dbo.CompanyEmployees");
            DropTable("dbo.EmployeeCompany1");
            DropTable("dbo.EmployeeCompanies");
            DropTable("dbo.EmployeeChartofAccounts");
            DropTable("dbo.EmployeeChartofAccountGroups");
            DropTable("dbo.AssetTenancyContracts");
            DropTable("dbo.DepartmentVendors");
            DropTable("dbo.DepartmentTaskTypes");
            DropTable("dbo.DepartmentTaskGroups1");
            DropTable("dbo.DepartmentTaskGroups");
            DropTable("dbo.SharedGridGroupEmployees");
            DropTable("dbo.SharedGridGroupDepartments");
            DropTable("dbo.SharedGridGroupCompanies");
            DropTable("dbo.DepartmentSalesReceipts");
            DropTable("dbo.DepartmentPayments");
            DropTable("dbo.DepartmentLoanApplicants");
            DropTable("dbo.DepartmentEmployees");
            DropTable("dbo.DepartmentDocumentTypes");
            DropTable("dbo.DepartmentDocumentTemplates");
            DropTable("dbo.DepartmentCompanies");
            DropTable("dbo.DepartmentChartofAccounts");
            DropTable("dbo.DepartmentChartofAccountGroups");
            DropTable("dbo.CashFlowDepartments");
            DropTable("dbo.CashFlowCompanies");
            DropTable("dbo.DepartmentAccounts");
            DropTable("dbo.ChartofAccountGroupChartofAccounts");
            DropTable("dbo.ChartofAccountCompanies");
            DropTable("dbo.VendorPayees");
            DropTable("dbo.VendorBillNatureCompanies");
            DropTable("dbo.ReportUsers");
            DropTable("dbo.InterBankTransferStatusStatusClasses");
            DropTable("dbo.StatusClassToDoTaskStatus");
            DropTable("dbo.StatusClassTenantRentalStatus");
            DropTable("dbo.StatusClassTasksStatus");
            DropTable("dbo.TasksStatusTaskTypes");
            DropTable("dbo.StatusClassTargetRewardStatus");
            DropTable("dbo.LoansStatusStatusClasses");
            DropTable("dbo.InquiryStatusStatusClasses");
            DropTable("dbo.ProductDepartments");
            DropTable("dbo.ModuleContractVendors");
            DropTable("dbo.ModuleContractStatusStatusClasses");
            DropTable("dbo.PurchaseOrderVendors");
            DropTable("dbo.PurchaseOrderStatusStatusClasses");
            DropTable("dbo.LoansAdvanceStatusStatusClasses");
            DropTable("dbo.ToDoTaskUsers");
            DropTable("dbo.PaymentStatusStatusClasses");
            DropTable("dbo.CreditCardDepartments");
            DropTable("dbo.PurchaseInvoiceVendors");
            DropTable("dbo.PurchaseInvoiceStatusStatusClasses");
            DropTable("dbo.SaleInvoiceVendors");
            DropTable("dbo.SalesReceiptStatusStatusClasses");
            DropTable("dbo.RentalInvoiceStatusStatusClasses");
            DropTable("dbo.RentalOrderStatusStatusClasses");
            DropTable("dbo.RentalContractStatusStatusClasses");
            DropTable("dbo.SaleOrderVendors");
            DropTable("dbo.SaleOrderStatusStatusClasses");
            DropTable("dbo.OfferVendors");
            DropTable("dbo.OfferStatusStatusClasses");
            DropTable("dbo.SaleInvoiceStatusStatusClasses");
            DropTable("dbo.DocumentStatusStatusClasses");
            DropTable("dbo.DocumentTemplateDocumentTypes");
            DropTable("dbo.BillStatusStatusClasses");
            DropTable("dbo.AssetRentalStatusStatusClasses");
            DropTable("dbo.AdminBillStatusStatusClasses");
            DropTable("dbo.CustomerCompanyIndustryTypes");
            DropTable("dbo.CustomerCompanyDepartment1");
            DropTable("dbo.CustomerCompanyDepartments");
            DropTable("dbo.PrincipalDepartments");
            DropTable("dbo.TasksUsers");
            DropTable("dbo.RoleUsers");
            DropTable("dbo.PermissionRoles");
            DropTable("dbo.PollUser1");
            DropTable("dbo.PollUsers");
            DropTable("dbo.TaskGroupsUser1");
            DropTable("dbo.TaskGroupsUsers");
            DropTable("dbo.MemoUsers");
            DropTable("dbo.CommentLogUser3");
            DropTable("dbo.CommentLogUser2");
            DropTable("dbo.CommentLogUser1");
            DropTable("dbo.CommentLogUsers");
            DropTable("dbo.AssetBrandRentalAssetSubNatures");
            DropTable("dbo.AssetModelRentalAssetSubNatures");
            DropTable("dbo.AssetModelAssetBrands");
            DropTable("dbo.AdminBillNatureCompanies");
            DropTable("dbo.VendorAdminBillTypes");
            DropTable("dbo.PayeeDepartments");
            DropTable("dbo.PayeeAdminBillTypes");
            DropTable("dbo.AdminBillTypeChartofAccounts");
            DropTable("dbo.ViewInfoes");
            DropTable("dbo.UniqueNumbers");
            DropTable("dbo.UnBoundReports");
            DropTable("dbo.VisitingCountries");
            DropTable("dbo.TravelingStatus");
            DropTable("dbo.TravelingRecords");
            DropTable("dbo.ToDoTaskThemes");
            DropTable("dbo.ShippingTerms");
            DropTable("dbo.SecurityDeposits");
            DropTable("dbo.RoleFields");
            DropTable("dbo.Travelers");
            DropTable("dbo.ResidentCountries");
            DropTable("dbo.RentalReceiveAmounts");
            DropTable("dbo.RentalAssetStatus");
            DropTable("dbo.RentalAssetMethods");
            DropTable("dbo.tabpUser");
            DropTable("dbo.PQDocuments");
            DropTable("dbo.tabPopupNotifications");
            DropTable("dbo.PayeeCategories");
            DropTable("dbo.Lands");
            DropTable("dbo.ModuleFields");
            DropTable("dbo.Fields");
            DropTable("dbo.ExchangeRates");
            DropTable("dbo.ExchangeRateGroups");
            DropTable("dbo.SalDeductions");
            DropTable("dbo.SalaryDeductions");
            DropTable("dbo.SalaryBonus");
            DropTable("dbo.SalaryAllowances");
            DropTable("dbo.Payrolls");
            DropTable("dbo.EmploymentSalaries");
            DropTable("dbo.PerformanceIndicatorDefinitions");
            DropTable("dbo.PerformanceIndicatorRatings");
            DropTable("dbo.EmployeePerformanceReviews");
            DropTable("dbo.EmployeeCoaCompanies");
            DropTable("dbo.CustomReports");
            DropTable("dbo.CustomReportGeoups");
            DropTable("dbo.VehicleTypes");
            DropTable("dbo.Manufacturers");
            DropTable("dbo.Lesees");
            DropTable("dbo.VehicleCurrentStatus");
            DropTable("dbo.VehicleBuyingStatus");
            DropTable("dbo.Vehicles");
            DropTable("dbo.ConditionImages");
            DropTable("dbo.Mortgagees");
            DropTable("dbo.Buildings");
            DropTable("dbo.Bonus");
            DropTable("dbo.tabBackground");
            DropTable("dbo.Regions");
            DropTable("dbo.OfficialAuths");
            DropTable("dbo.AuthDocs");
            DropTable("dbo.Attachments");
            DropTable("dbo.AttachmentCategories");
            DropTable("dbo.Areas");
            DropTable("dbo.Allowances");
            DropTable("dbo.Airlines");
            DropTable("dbo.MainBanks");
            DropTable("dbo.EmployeeWorkExperiences");
            DropTable("dbo.SalesTargets");
            DropTable("dbo.Qualifications");
            DropTable("dbo.LeaveStatus");
            DropTable("dbo.Leaves");
            DropTable("dbo.LeaveApplications");
            DropTable("dbo.EmployeeHRInfoes");
            DropTable("dbo.EmployeeWorkingStatus");
            DropTable("dbo.EmployeeApprovals");
            DropTable("dbo.Functions");
            DropTable("dbo.Emergencyontacts");
            DropTable("dbo.Tenants");
            DropTable("dbo.RentalAssets");
            DropTable("dbo.TenancyContracts");
            DropTable("dbo.Revaluations");
            DropTable("dbo.PurchaseInfoes");
            DropTable("dbo.TargetTypes");
            DropTable("dbo.TargetAwards");
            DropTable("dbo.Targets");
            DropTable("dbo.SharedReports");
            DropTable("dbo.SharedGridGroups");
            DropTable("dbo.DepartmentLevels");
            DropTable("dbo.ReportTitles");
            DropTable("dbo.GridReports");
            DropTable("dbo.GridReportGroups");
            DropTable("dbo.CashFlows");
            DropTable("dbo.ChartofAccountGroups");
            DropTable("dbo.VendorNatureManuals");
            DropTable("dbo.VendorNatures");
            DropTable("dbo.VendorBillNatures");
            DropTable("dbo.BillTypes");
            DropTable("dbo.BillItems");
            DropTable("dbo.BillCategories");
            DropTable("dbo.Templates");
            DropTable("dbo.ManagementSummaries");
            DropTable("dbo.MaintenanceHeads");
            DropTable("dbo.VehicleExpenses");
            DropTable("dbo.UserSettings");
            DropTable("dbo.ReportGroups");
            DropTable("dbo.Reports");
            DropTable("dbo.Warehouses");
            DropTable("dbo.TaskTrackings");
            DropTable("dbo.EfficiencyPoints");
            DropTable("dbo.TaskEfficiencies");
            DropTable("dbo.GoodReceiveNotes");
            DropTable("dbo.TaskComments");
            DropTable("dbo.PackingStyles");
            DropTable("dbo.Reconcilations");
            DropTable("dbo.TaskTypes");
            DropTable("dbo.TasksStatus");
            DropTable("dbo.FacilityNatures");
            DropTable("dbo.tabLoan");
            DropTable("dbo.LoansStatus");
            DropTable("dbo.UnitOfMeasures");
            DropTable("dbo.ProductNatures");
            DropTable("dbo.ProductCategories");
            DropTable("dbo.PassOns");
            DropTable("dbo.ModuleContractStatus");
            DropTable("dbo.PurchaseOrderStatus");
            DropTable("dbo.LoansAdvanceStatus");
            DropTable("dbo.TaskTargetTypes");
            DropTable("dbo.ToDoTaskStatus");
            DropTable("dbo.ToDoTasks");
            DropTable("dbo.TargetRewardNatures");
            DropTable("dbo.TargetRewardStatus");
            DropTable("dbo.TargetRewards");
            DropTable("dbo.PaymentStatus");
            DropTable("dbo.CreditCardTypes");
            DropTable("dbo.CardHolders");
            DropTable("dbo.CreditCards");
            DropTable("dbo.PurchaseInvoiceStatus");
            DropTable("dbo.SalesReceiptStatus");
            DropTable("dbo.RentalInvoiceStatus");
            DropTable("dbo.RentalOrderStatus");
            DropTable("dbo.TenantRentalStatus");
            DropTable("dbo.TenantRentals");
            DropTable("dbo.RentalContractStatus");
            DropTable("dbo.RentalContracts");
            DropTable("dbo.RentalOrders");
            DropTable("dbo.RentalInvoices");
            DropTable("dbo.ReceiptTaxes");
            DropTable("dbo.CollectionMethods");
            DropTable("dbo.ReceiptDeductions");
            DropTable("dbo.SalesReceipts");
            DropTable("dbo.VendorPaymentStatus");
            DropTable("dbo.SplitPERs");
            DropTable("dbo.SaleOrderStatus");
            DropTable("dbo.SaleOrdeRrefKeys");
            DropTable("dbo.PerformanceSheetHeads");
            DropTable("dbo.PerfomarmanceSheetFields");
            DropTable("dbo.PerformanceSheets");
            DropTable("dbo.OfferStatus");
            DropTable("dbo.Offers");
            DropTable("dbo.MemorandumSaleStatus");
            DropTable("dbo.MemorandumSales");
            DropTable("dbo.VendorBillReferences");
            DropTable("dbo.SaleOrders");
            DropTable("dbo.SaleInvoiceStatus");
            DropTable("dbo.LotNumbers");
            DropTable("dbo.CustomerCredits");
            DropTable("dbo.SaleInvoices");
            DropTable("dbo.InventoryAdjustmentStatus");
            DropTable("dbo.InventoryAdjustments");
            DropTable("dbo.Inventories");
            DropTable("dbo.PurchaseInvoices");
            DropTable("dbo.VATBooks");
            DropTable("dbo.VATBookRefNumbers");
            DropTable("dbo.TranferMethods");
            DropTable("dbo.STLStatus");
            DropTable("dbo.STLSettlements");
            DropTable("dbo.ReversalSettlements");
            DropTable("dbo.STLInterestTypes");
            DropTable("dbo.STLInterests");
            DropTable("dbo.MarginPercentageTypes");
            DropTable("dbo.MarginPercentages");
            DropTable("dbo.STLs");
            DropTable("dbo.InterCompBankTransferVATs");
            DropTable("dbo.InterCompBankTransferCharges");
            DropTable("dbo.InterCompanyBankTransfers");
            DropTable("dbo.PettyCashes");
            DropTable("dbo.PaymentTaxes");
            DropTable("dbo.BillRefNumbers");
            DropTable("dbo.PaymentMethods");
            DropTable("dbo.PaymentDeductions");
            DropTable("dbo.BudgetSystemCostFields");
            DropTable("dbo.Payments");
            DropTable("dbo.LoanApplicantTypes");
            DropTable("dbo.LoanApplicants");
            DropTable("dbo.LoansAdvances");
            DropTable("dbo.JournalVoucherStatus");
            DropTable("dbo.JournalVouchers");
            DropTable("dbo.Warranties");
            DropTable("dbo.PaymentTerms");
            DropTable("dbo.FieldValues");
            DropTable("dbo.CostSheetSOFields");
            DropTable("dbo.CostSheetSIFields");
            DropTable("dbo.CostSheetSaleReceiptFields");
            DropTable("dbo.CostSheetPOFields");
            DropTable("dbo.CostSheetPaymentFields");
            DropTable("dbo.CostSheetOfferFields");
            DropTable("dbo.CostSheetBillFields");
            DropTable("dbo.CostFieldHistories");
            DropTable("dbo.CostSheets");
            DropTable("dbo.SummarySheetFields");
            DropTable("dbo.SummaryFieldValues");
            DropTable("dbo.CommissionSummarySheets");
            DropTable("dbo.BudgetCostSheetStatus");
            DropTable("dbo.BudgetSheetHeads");
            DropTable("dbo.BudgetCostFields");
            DropTable("dbo.BudgetCostSheets");
            DropTable("dbo.PurchaseOrders");
            DropTable("dbo.Incoterms");
            DropTable("dbo.ComparativeStatementItems");
            DropTable("dbo.ComparativeStatements");
            DropTable("dbo.Bids");
            DropTable("dbo.ModuleContracts");
            DropTable("dbo.FOCSamplings");
            DropTable("dbo.ClaimDiscounts");
            DropTable("dbo.BookerStatementItems");
            DropTable("dbo.Products");
            DropTable("dbo.InquiryProducts");
            DropTable("dbo.Inquiries");
            DropTable("dbo.InquiryStatus");
            DropTable("dbo.DocumentTypes");
            DropTable("dbo.DocumentTemplates");
            DropTable("dbo.DocumentAuthorities");
            DropTable("dbo.Documents");
            DropTable("dbo.DocumentStatus");
            DropTable("dbo.BillStatus");
            DropTable("dbo.AssetRentalStatus");
            DropTable("dbo.AdminBillStatus");
            DropTable("dbo.StatusClasses");
            DropTable("dbo.InterBankTransferStatus");
            DropTable("dbo.TaxTypes");
            DropTable("dbo.TaxNames");
            DropTable("dbo.InterBankTransferVATs");
            DropTable("dbo.Principals");
            DropTable("dbo.AuditYearAdjustments");
            DropTable("dbo.Religions");
            DropTable("dbo.ContactPersons");
            DropTable("dbo.CustomerCompanies");
            DropTable("dbo.IndustryTypes");
            DropTable("dbo.Deductions");
            DropTable("dbo.IBTbankCharges");
            DropTable("dbo.InterBankTransfers");
            DropTable("dbo.JournalTransactions");
            DropTable("dbo.CostSheetFields");
            DropTable("dbo.ProcurementProducts");
            DropTable("dbo.Checklists");
            DropTable("dbo.Tasks");
            DropTable("dbo.Permissions");
            DropTable("dbo.Roles");
            DropTable("dbo.Polls");
            DropTable("dbo.LoginUserDetails");
            DropTable("dbo.TargetGroups");
            DropTable("dbo.SalesExchangeRates");
            DropTable("dbo.MarketExchangeRates");
            DropTable("dbo.Currencies");
            DropTable("dbo.SoCalculationFields");
            DropTable("dbo.StatusCalculationTypes");
            DropTable("dbo.TaskGroups");
            DropTable("dbo.Memos");
            DropTable("dbo.Notifications");
            DropTable("dbo.NotificationFlags");
            DropTable("dbo.TransactionItems");
            DropTable("dbo.CommentCategories");
            DropTable("dbo.CommentLogs");
            DropTable("dbo.Users");
            DropTable("dbo.AssetTypes");
            DropTable("dbo.AssetRentalUnits");
            DropTable("dbo.Countries");
            DropTable("dbo.Cities");
            DropTable("dbo.AssetRentalLocations");
            DropTable("dbo.AssetNumbers");
            DropTable("dbo.RentalAssetSubNatures");
            DropTable("dbo.RentalAssetNatures");
            DropTable("dbo.AssetModels");
            DropTable("dbo.AssetBrands");
            DropTable("dbo.AssetRentals");
            DropTable("dbo.AdminBillNatures");
            DropTable("dbo.tabAdminBill");
            DropTable("dbo.Adjustments");
            DropTable("dbo.VendorBillAdjustments");
            DropTable("dbo.Bills");
            DropTable("dbo.tabVendor");
            DropTable("dbo.RentedVehicleOwners");
            DropTable("dbo.Payees");
            DropTable("dbo.AdminBillTypes");
            DropTable("dbo.ChartofAccounts");
            DropTable("dbo.tabDepartment");
            DropTable("dbo.Designations");
            DropTable("dbo.AssetStatus");
            DropTable("dbo.PersonPhotoes");
            DropTable("dbo.tabPerson");
            DropTable("dbo.tabContact");
            DropTable("dbo.AssetOwners");
            DropTable("dbo.AssetNatures");
            DropTable("dbo.Assets");
            DropTable("dbo.Employees");
            DropTable("dbo.tabCompany");
            DropTable("dbo.tabAddress");
            DropTable("dbo.Banks");
            DropTable("dbo.Accounts");
        }
    }
}
