using System.Collections.Generic;

namespace ERP_BL.Enums
{
    public enum BizTypes
    {
        Corporation,
        SoleProprietor,
        Partnership,
        NGO
    }
    public enum IndustryTypes
    {
        ServiceProvider,
        InformationTechnology,
        Construction
    }


    public enum EmployeeStatus
    {
        Active,
        Resigned,
        Terminated,
        Retired
    }
    public enum actionRequired
    {
        AddNew,
        Edit,
        Load,
        Delete,
        Mark_Void
    }
    public enum CompnayTypes
    {
        Group,
        Company,
        CustomerCompany,
        VendorCompany,
        PrincipalCompany
    }
    public enum InquiryType
    {
        Tender,
        Supply,
        Principal,
        Bill,
        Standard,
        Inventory,
        DistributionBiz,
        SupplyCCC,
        DistributionBiz_CustomerCredit,
        Audit_Year_Adjustment

    }

    public enum ContactTypes
    {
        Primary,
        Secodary,
        Other,
    }

    public enum Gender
    {
        Male,
        Female,
        Shemale,
        Not_Disclosed
    }
    public enum AddressTypes
    {
        companyAddress,
        personAddress,
        shippingAddress,
        billingAddress,
        assetAddress
    }
    public enum ItemNature
    {
        Inventory,
        Services
    }
    public enum InvoiceStage
    {
        None,
        Partialy,
        Fully
    }
    public enum AccessLevel
    {
        Full,
        Edit,
        None
    }
    public enum TransactionItemType
    {
        UnDefined=0,
        Inquiry=1,
        Offer=2,
        Sale_Order=3,
        Memorandum_Sale=4,
        Sale_Invoice=5,
        Purchase_Order=6,
        Purchase_Invoice=7,
        CostCenter=8,
        SummarySheet=9,
        Bill = 10,
        Sale_Receipt = 11,
        FixedAssets = 12,
        InterBank_Transfer = 13,
        Employee = 14,
        Leave = 15,
        JV,
        Admin_Bill,
        InterCompanyBank_Transfer,
        PurchaseInvoice,
        Payments,
        BackgroundImages,
        RentalContract,
        Loans, 
        ToDo_Task,
        LoansAdvances,
        EmploymentSalary,
        Payroll,
        Tasks,
        Budget,
        TargetReward,
        TargetStep,
        STL,
        TravelingRecord,
        InventoryAdjustment,
        ProcurementProducts,
        VehicleExpenses,
        RentalOrder,
        RentalInvoice,
        AssetRental,
        TenantRental,
        ModuleContract,
        Memo,
        Document,
        PerformanceReview
    }
    

    public enum ChatFileType
    {
        One2One,
        CompanyGroup,
        CustomGroup,
        Notification
    }

    public enum TransactionInfo
    {
        Initialized,
        viewed,
        Status_Changed,
        Reviewer_Rejected,
        Approver_Rejected,
        Rejected,
        Reviewed,
        Edited,
        Closed,
        Approved_Closing,
        Approved_Adding,
        Created_Invoice,
        Attachment_Uploaded,
        Attachment_Downloaded,
        Status_Class_Changed


    }
    public enum TransactionStage
    {
        Initialized,
        Added,
        AwaitingFirstReview,
        AwaitingSecondReview,
        AwaitingApproval,
        //ReviewedOnce,
        //ReviewedTwice,
        Approved,
        Rejected,
        Closed,
        
    }
    public enum fileType
    {
        bmp,
        png,
        jpg,
        jpeg,
        gif,
        txt,
        doc,
        docx,
        xls,
        xlxs,
        all
    }
    public enum UploadFlag
    {
        Failed,
        Uploaded,
        PendingUploading,
        SavedLocaly,
        ErrorUploading,
        Error
    }
    public enum CostFieldType
    {
        Budgeted_Margin,
        Revised_Margin,
        Actual_Margin,
        System_Cost,
        vendor,
        System_Payment,
        Added_System_Cost,
        Bill_Cost,
        PO_Cost,
        addedSRBCValue,
        soAmountSRBC,
        SRBC,
        Receipt_Cost, 
        SI_Cost,
       
    }
    public enum TargetFrequency
    {
        Monthly,
        Yearly
        
    }
    public enum DataType
    {
        All,
        Active,
        Inactive,
        Open,
        Closed,
        PendingForApproval,
        PendingForReApproval,
        PendingForClosing
    }
    public enum MeasureUnitType
    {
        Millimeter,
        Centimeter,
        Meter,
        Kilometer,
        Inches,
        Feet,
        Yards,
        Miles,
        Nautical_Miles
    }

    public enum RentalBasis
    {
        Hourly, 
        Daily, 
        Weekly, 
        Monthly, 
        Yearly
    }
    public enum OfficialAuthType
    {
        Land,
        Biolding,
        Vehicles
    }
    public enum ReceiptType
    {
        Sales_Customer,
        Sales_Department,
        Loans_Advances,
        Direct_Receipt,
        Customer_Credits,
        Rental_Receipt
    }
    public enum AccountsType
    {
        Savings,
        Current, 
        Fixed_Term
    }
    public enum AccountsNature
    {
        Bank
    }

    public enum AccountsCategory
    {
        Company,
        Personal,
        Vendor,
        Customer
    }
    public enum GridReportType
    {
        StandardReport,
        MemorizedReport
            
    }
    public enum DegreeType
    {
        Nill,
        Primary,
        Middle,
        Matriculation,
        Intermediate,
        Bachlors_2yrs,
        Bachlors_4yrs,
        Masters_2yrs,
        MS_Mphil,
        Phd,
        Course,
        Certification
    }
    public enum TransferType
    {
        IBT_Single_Currency,
        Advances,
        Inter_Company,
        IBT_Multiple_Currency,
        Inter_Company_Multiple_Currency,
        Inter_Company_Multiple_Currency_Account_Convertor
    }
    public enum CardHolderType
    {
        Primary,
        Secondary
    }
    public enum COA_AccountType
    {
        Income,
        Expense,
        Fixed_Asset,
        Bank,
        Loan,
        Credit_Card,
        Equity,
        Account_Receivable,
        Other_Current_Asset,
        Other_Asset,
        Accounts_Payable,
        Other_Current_Liability,
        Longterm_Liability,
        Cost_of_Goods_Sold,
        Other_Income,
        Other_Expense
    }
    public enum LeaveType
    {
        AnnualLeave,
        CasualLeave,
        HalfDay,
        Absent,
        Adjustment
        
        
    }
    public enum LeaveCurrentStatus
    {
        Applied,
        UnderApproval,
        //UnderApprovalAdmin,
        Approved,
        Rejected 
    }
    public enum LeaveAdjustmentType
    {
        ExtraDay,
        Leave,
        Allocation
    }
    public enum coaTransactionsType
    {
        JV,
        PurchaseOrder,
        SaleOrder,
        SaleInvoice,
        SaleReceipt,
        Offer,
        Inquiry,
        Bill,
        InterBankTransfer,
        AdminBill,
        Payment,
        PurchaseInvoice,
        InterCompanyTransfer,
        TragetReward,
        STL
    }
   
    public enum BillTemplate
    {
        Default,
        Credit_Cards
    }
    public enum ProductType
    {
        Inventory,
        Non_Inventory,
        Services
    }
public enum TaxFlag
    {
        Tax,
        No_Tax
    }
    public enum CoaReportModuleType
    {
        Profit_and_Loss,
        BalanceSheet
    }
    public enum ReconcilationType
    {
        Uncleared_Transactions,
        Cleared_Transactions,
        New_Transactions,

    }
    public enum ReconcilationTransaction
    {
        Checks_and_Payments,
        Deposit_and_other_Credits,
    }

    public enum PaymentTransactionType
    {
        Admin_Bills,
        Vendor_Bills,
        Purchase_Invoice,
        Loans_Advances,
        Target_Reward
    }

    public enum PaymentAdminBillTemplate
    {
        Admin_Bills,
        Admin_Bill_Credit_Card
    }

    public enum PaymentVendorBillTemplate
    {
        Vendor_Bills,
    }

    public enum PaymentPurchaseInvoiceTemplate
    {
        Purchase_Invoice,
    }

    public enum PaymentLoansAdvancesTemplate
    {
        Loans_Advance,
    }

    public enum PaymentTargetRewardsTemplate
    {
        Target_Reward,
    }

    public enum RentalType
    {
        Vehicle,
        Land
    }

    public enum TenancyType
    {
        Dummy1,
        Dummy2
    }
    public enum RentalAssetType 
    {
        Owned,
        NotOwned 
    }
    public enum InventoryTransactionsType
    {
        PurchaseInvoice,
        SaleInvoice,
        Adjustment
    }
    public enum ExchangeRateType
    {
        SER,
        MER
    }

    public enum TaskGroupTemplate
    {
        Standard,
        Optional
    }
    public enum CustomReportFields
    {
        AmountOC,
        AmountMER
    }
    public enum TransactionCounterType 
    {
        PendingForApprovalDepartmental,
        PendingForApproval,
        PendingForApprovalOwn,
        PendingForReapprovalDepartmental,
        PendingForReapproval,
        PendingForReapprovalOwn,
        PendingForClosingDepartmental,
        PendingForClosing,
        PendingForClosingOwn,
        Open,
        Close,
        Total
    }
    public enum DepartmentLevels
    {
        DepartmentLevel1,
        DepartmentLevel2,
        DepartmentLevel3,
        DepartmentLevel4,
        DepartmentLevel5
    }

    public enum CustomerLevels
    {
        CustomerLevel1,
        CustomerLevel2,
        CustomerLevel3,
        CustomerLevel4,
        CustomerLevel5
    }

    public enum TargetsTransactionType
    {
        SaleOrder,
        SaleInvoice,
        PurchaseOrder,
        PurchaseInvoice,
        SaleReceipt,
        Offer,
        Inquiry,
        VendorBill
    }

    public enum FunctionType
    {
        Sales,
        Finance,
        Other
    }
    public enum LoansAdvanceTemplate
    {
        Advance,
        Loan
    }
    public enum LoansAdvanceType
    {
        Admin_Bill,
        Vendor_Bill
    }

    public enum TaskTemplate
    {
        Tax_Record
    }

    public enum ItineraryStatus
    {
        Closed,
        Planned
    }
    public enum ReportTransactionType
    {
        Inquiry=0, 
        Offer=1,
        Sale_Order=2,        
        Sale_Invoice=3,
        Purchase_Order=4,
        Purchase_Invoice=5,
        Bill=6,
        Sale_Receipt=7,
        InterBank_Transfer=8,
        Admin_Bill=9,
        InterCompanyBank_Transfer=10,
        Payments=11,
        Targets=12,
        Trial_Balance=13,
        TargetReward=14,
        Chart_of_Account=15,
        STL,
        Tasks,
        SaleOrderPRIT,
        Advances,
        Loans,
        Assets,
        CashFlow
    }

    public enum AdminBillTypes
    {
        Admin_Bill,
        Asset
    }

    public enum PollingType
    {
        Hidden_Polling,
        Open_Polling
    }
    public enum AdjustmentType
    {
        Quantity,
        Amount,
        Quantity_and_Amount
        
    }

    public enum AssetCategory
    {
        Building,
        Land,
        Vehicle
    }
    public enum Themes
    {
        LightBlue,
        Silver,
        DeepBlue
    }

    public enum VehicleExpenseType
    {
        Fuel,
        Maintenance
    }

    public enum MemoType
    {
        Linked,
        Non_Linked,
        Group
    }

    public enum PerformanceReviewerStage
    {
        Self,
        SupervisorLevel1,
        SupervisorLevel2,
        Admin,
        Management
    }

    public enum PerformanceReviewType
    {
        Self,
        LevelOne,
        LevelTwo
    }
}
