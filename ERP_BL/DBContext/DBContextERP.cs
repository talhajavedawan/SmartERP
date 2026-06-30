
using ERP_BL.AssetsRentals;
using ERP_BL.AssetsRentals.RentalContracts;
using ERP_BL.AssetsRentals.RentalInvoices;
using ERP_BL.AssetsRentals.RentalOrders;
using ERP_BL.AssetsRentals.SecurityDeposits;
using ERP_BL.AssetsRentals.TenantRentals;
using ERP_BL.Bankings;
using ERP_BL.CashBook;
using ERP_BL.ChartofAccounts;
using ERP_BL.Countryy;
using ERP_BL.CreditCards;
using ERP_BL.CustomReports;
using ERP_BL.DBContext;
using ERP_BL.Documents;
using ERP_BL.ExchangeRates;
using ERP_BL.Fields;
using ERP_BL.FilesAndDocs;
using ERP_BL.HR;
using ERP_BL.Payments;
using ERP_BL.Procurements;
using ERP_BL.Procurements.AdminBills;
using ERP_BL.Procurements.Bill;
using ERP_BL.Procurements.Budget;
using ERP_BL.Procurements.InterBankTransfers;
using ERP_BL.Procurements.Inventories;
using ERP_BL.Procurements.LoansAdvances;
using ERP_BL.Procurements.Memos;
using ERP_BL.Procurements.StatusClass;
using ERP_BL.Rentals;
//using ERP_BL.Procurements.NewBills;
using ERP_BL.Reports;
using ERP_BL.Tax;
using ERP_BL.ToDoTasks;
using ERP_BL.ToDoTasks.Taskss;
using ERP_BL.User;
using System;
using System.Data.Common;
using System.Data.Entity;

namespace ERP_BL.Databases
{
    public class DBContextERP : DbContext
    {
        public static DbContext dbContext;


        public DBContextERP() /*: base("DBContextERP")*/


        //public DBContextERP() : base(new Connections., false)
        {
            //Database.SetInitializer<DBContextERP>(new DropCreateDatabaseIfModelChanges<DBContextERP>());
            Database.SetInitializer<DBContextERP>(null);

        }
        public DBContextERP(string con) : base("DBContextERP")


        //public DBContextERP() : base(new Connections., false)
        {
            try
            {
                Database.SetInitializer<DBContextERP>(null);
                this.Configuration.LazyLoadingEnabled = false;
                //this.Database.CommandTimeout = 180;
                //Database.SetInitializer<DBContextERP>(new DropCreateDatabaseIfModelChanges<DBContextERP>());
                // Database.SetInitializer<DBContextERP>(con);
            }
            catch (Exception exception)
            {
                Console.Write(exception.Message);
            }
        }
        public DBContextERP(DbConnection existingConnection, bool contextOwnsConnection) : base(Connections.connection, false)

        //public DBContextERP() : base(new Connections., false)
        {
            //Database.SetInitializer<DBContextERP>(new DropCreateDatabaseIfModelChanges<DBContextERP>());
            Database.SetInitializer<DBContextERP>(new DBContextERPDBInitializer());

        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>().MapToStoredProcedures();
            //modelBuilder.Entity<Inquiry>().MapToStoredProcedures();
            //modelBuilder.Entity<Offer>().MapToStoredProcedures();
            //modelBuilder.Entity<SaleOrder>().MapToStoredProcedures();
            //modelBuilder.Entity<CustomerCompany>().MapToStoredProcedures();
            //modelBuilder.Entity<Employee>().MapToStoredProcedures();
            //modelBuilder.Entity<Vendor>().MapToStoredProcedures();
            //modelBuilder.Entity<Department>().MapToStoredProcedures();
            //modelBuilder.Entity<InquiryProduct>().MapToStoredProcedures();
            //modelBuilder.Entity<Product>().MapToStoredProcedures();
            //modelBuilder.Entity<SaleOrderStatus>().MapToStoredProcedures();
            //modelBuilder.Entity<OfferStatus>().MapToStoredProcedures();
            //modelBuilder.Entity<InquiryStatus>().MapToStoredProcedures();
            //modelBuilder.Entity<Currency>().MapToStoredProcedures();
            //modelBuilder.Entity<Address>().MapToStoredProcedures();
            //modelBuilder.Entity<Contact>().MapToStoredProcedures();
            //modelBuilder.Entity<User>().MapToStoredProcedures();
            //modelBuilder.Entity<ProcurementProduct>().MapToStoredProcedures();
            //modelBuilder.Entity<Product>().MapToStoredProcedures();

            //base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<Address>()
            //.HasIndex(p => p.Id)
            //.IsUnique();

            //base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<Contact>()
            //.HasIndex(p => p.Id)
            //.IsUnique();

            //base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<Department>()
            //.HasIndex(p => p.Id)
            //.IsUnique();

            //base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<Company>()
            //.HasIndex(p => p.Id)
            //.IsUnique();

            //base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<CustomerCompany>()
            //.HasIndex(p => p.Id)
            //.IsUnique();

            //base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<Vendor>()
            //.HasIndex(p => p.Id)
            //.IsUnique();

            //base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<Principal>()
            //.HasIndex(p => p.Id)
            //.IsUnique();

            //base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<Person>()
            //.HasIndex(p => p.Id)
            //.IsUnique();

            //base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<Employee>()
            //.HasIndex(p => p.EmpId)
            //.IsUnique();

            //base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<User>()
            //.HasIndex(p => p.id)
            //.IsUnique();

            //base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<InquiryStatus>()
            //.HasIndex(p => p.Id)
            //.IsUnique();

            //base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<Inquiry>()
            //.HasIndex(p => p.Id)
            //.IsUnique();

            //base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<Product>()
            //.HasIndex(p => p.Id)
            //.IsUnique();

            //base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<Currency>()
            //.HasIndex(p => p.Id)
            //.IsUnique();

            //base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<Offer>()
            //.HasIndex(p => p.Id)
            //.IsUnique();

            //base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<PaymentTerm>()
            //.HasIndex(p => p.Id)
            //.IsUnique();

            //base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<Incoterm>()
            //.HasIndex(p => p.Id)
            //.IsUnique();

            //base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<MemorandumSale>()
            //.HasIndex(p => p.Id)
            //.IsUnique();

            //base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<MemorandumSaleStatus>()
            //.HasIndex(p => p.Id)
            //.IsUnique();

            //base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<SaleOrder>()
            //.HasIndex(p => p.Id)
            //.IsUnique();

            //base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<SaleOrderStatus>()
            //.HasIndex(p => p.Id)
            //.IsUnique();

            //base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<PurchaseOrder>()
            //.HasIndex(p => p.Id)
            //.IsUnique();

            //base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<PurchaseOrderStatus>()
            //.HasIndex(p => p.Id)
            //.IsUnique();

            //base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<Bid>()
            //.HasIndex(p => p.Id)
            //.IsUnique();

            //base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<UserSettings>()
            //.HasIndex(p => p.id)
            //.IsUnique();

            //base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<Role>()
            //.HasIndex(p => p.Id)
            //.IsUnique();

            //base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<Permission>()
            //.HasIndex(p => p.Id)
            //.IsUnique();

            //base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<SalesTarget>()
            //.HasIndex(p => p.Id)
            //.IsUnique();

            //base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<Report>()
            //.HasIndex(p => p.Id)
            //.IsUnique();

            //base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<UnBoundReport>()
            //.HasIndex(p => p.Id)
            //.IsUnique();

            //base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<ReportGroup>()
            //.HasIndex(p => p.Id)
            //.IsUnique();

            //base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<ProcurementProduct>()
            //.HasIndex(p => p.Id)
            //.IsUnique();

            //base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<InquiryProduct>()
            //.HasIndex(p => p.Id)
            //.IsUnique();

            //base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<ExchangeRate>()
            //.HasIndex(p => p.Id)
            //.IsUnique();

            //base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<SalesExchangeRate>()
            //.HasIndex(p => p.Id)
            //.IsUnique();

            //base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<ViewInfo>()
            //.HasIndex(p => p.Id)
            //.IsUnique();

            //base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<CostSheet>()
            //.HasIndex(p => p.Id)
            //.IsUnique();

            //base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<CostSheetField>()
            //.HasIndex(p => p.Id)
            //.IsUnique();

            //base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<FieldValue>()
            //.HasIndex(p => p.Id)
            //.IsUnique();

            //base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<Notification>()
            //.HasIndex(p => p.Id)
            //.IsUnique();

            //base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<CommentLog>()
            //.HasIndex(p => p.Id)
            //.IsUnique();


            //modelBuilder.Entity<Offer>()
            //    .HasKey(o => o.Id)
            //    .HasRequired(u => u.user).WithRequiredPrincipal(u=>u.id)
            //    .WithRequiredDependent()
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Offer>()
            //    .HasRequired(u => u.user)
            //    .WithRequiredDependent()
            //    .WillCascadeOnDelete(false);

        }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<AttachmentCategory> AttachmentCategories { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<Contact> Contacts { get; set; }

        public DbSet<Department> Departments { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<IndustryType> IndustryTypes { get; set; }
        public DbSet<VendorNature> vendorNatures { get; set; }
        public DbSet<VendorNatureManual> vendorNaturesManual { get; set; }
        public DbSet<CustomerCompany> customerCompanies { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<Principal> Principals { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Designation> Designations { get; set; }

        public DbSet<ContactPerson> contactPersons { get; set; } // relationships class only

        public DbSet<Employee> Employees { get; set; }
        public DbSet<pUser> pUsers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<InquiryStatus> inquiryStatuses { get; set; }
        public DbSet<Inquiry> inquiries { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductNature> productNatures { get; set; }
        public DbSet<UnitOfMeasure> unitOfMeasures { get; set; }
        public DbSet<ProductCategory> productCategories { get; set; }

        public DbSet<Currency> currencies { get; set; }
        public DbSet<Offer> offers { get; set; }
        public DbSet<OfferStatus> offerStatuses { get; set; }
        public DbSet<PaymentTerm> paymentTerms { get; set; }
        public DbSet<Incoterm> incoterms { get; set; }
        public DbSet<VendorPaymentStatus> vendorPaymentStatuses { get; set; }
        public DbSet<MemorandumSale> memorandumSales { get; set; }
        public DbSet<MemorandumSaleStatus> memorandumSaleStatuses { get; set; }
        public DbSet<SaleOrder> saleOrders { get; set; }
        public DbSet<SaleOrderStatus> saleOrderStatuses { get; set; }
        public DbSet<SaleInvoice> saleInvoices { get; set; }
        public DbSet<SaleInvoiceStatus> saleInvoiceStatuses { get; set; }
        public DbSet<PurchaseOrder> purchaseOrders { get; set; }
        public DbSet<PurchaseOrderStatus> purchaseOrderStatuses { get; set; }
        public DbSet<Bill> bills { get; set; }
        public DbSet<BillStatus> billStatuses { get; set; }
        public DbSet<BillType> billTypes { get; set; }
        public DbSet<Bid> bids { get; set; }
        public DbSet<UserSettings> userSettings { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<SalesTarget> SalesTargets { get; set; }
        public DbSet<Warranty> Warranties { get; set; }

        public DbSet<Report> Reports { get; set; }
        public DbSet<UnBoundReport> UnBoundReport { get; set; }
        public DbSet<ReportGroup> ReportGroups { get; set; }
        public DbSet<ProcurementProduct> procurementProducts { get; set; }
        public DbSet<InquiryProduct> inquiryProducts { get; set; }
        public DbSet<MarketExchangeRate> exchangeRates { get; set; }
        public DbSet<SalesExchangeRate> salesExchangeRates { get; set; }
        public DbSet<ViewInfo> viewInfos { get; set; }
        public DbSet<CostSheet> costSheets { get; set; }
        public DbSet<CostSheetField> costSheetFields { get; set; }
        public DbSet<FieldValue> fieldValues { get; set; }
        public DbSet<SummaryFieldValue> SummaryFieldValues { get; set; }
        public DbSet<SummarySheetField> summarySheetFields { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<CommentLog> CommentLogs { get; set; }
        public DbSet<CommentCategory> commentCategories { get; set; }
        public DbSet<Target> Targets { get; set; }
        public DbSet<TargetType> TargetTypes { get; set; }
        public DbSet<TargetAward> TargetAwards { get; set; }
        public DbSet<TransactionItem> TransactionItems { get; set; }
        public DbSet<Asset> Assets { get; set; }
        public DbSet<AssetNature> AssetNatures { get; set; }
        public DbSet<Land> Lands { get; set; }
        public DbSet<Building> Buildings { get; set; }
        public DbSet<Mortgagee> Mortgagees { get; set; }
        public DbSet<Area> Areas { get; set; }
        public DbSet<OfficialAuth> OfficialAuths { get; set; }
        public DbSet<AuthDoc> AuthDocs { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<PurchaseInfo> PurchaseInfos { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<VehicleType> VehicleTypes { get; set; }
        public DbSet<VehicleCurrentStatus> VehicleCurrentStatuses { get; set; }  // of vehicle
        public DbSet<VehicleBuyingStatus> VehicleBuyingStatuses { get; set; }

        public DbSet<Revaluation> Revaluations { get; set; }
        public DbSet<ConditionImage> ConditionImages { get; set; }
        public DbSet<Lesee> Lesees { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }

        // following Sales receipt Entities are not in well formated way. Please avoid any migration to live Databases. As it will create issue later. 
        public DbSet<SalesReceipt> salesReceipts { get; set; }
        public DbSet<CollectionMethod> collectionMethods { get; set; }
        public DbSet<Deduction> deductions { get; set; }
        public DbSet<Account> accounts { get; set; }
        public DbSet<Bank> banks { get; set; }

        public DbSet<GridReport> GridReports { get; set; }
        public DbSet<GridReportGroup> GridReportGroups { get; set; }

        public DbSet<AssetStatus> assetStatuses { get; set; }
        public DbSet<ReceiptDeduction> receiptDeductions { get; set; }
        public DbSet<SalesReceiptStatus> salesReceiptStatuses { get; set; }

        public DbSet<Qualification> qualifications { get; set; }
     
        // public DbSet<HrInfo> HrInfos { get; set; }
        public DbSet<Function> functions { get; set; }
        public DbSet<EmployeeApproval> employeeApprovals { get; set; }
        public DbSet<EmployeeWorkingStatus> employeeStatuses { get; set; }

        public DbSet<InterBankTransfer> interBankTransfers { get; set; }
        public DbSet<InterCompanyBankTransfer> interCompanyBankTransfers { get; set; }
        public DbSet<InterBankTransferStatus> interBankTransferStatuses { get; set; }
        public DbSet<TranferMethod> tranferMethods { get; set; }
        public DbSet<CreditCard> creditCards { get; set; }
        public DbSet<CardHolder> cardHolders { get; set; }
        public DbSet<CreditCardType> creditCardTypes { get; set; }

        public DbSet<ChartofAccount> ChartofAccounts { get; set; }

        public DbSet<Leave> leaves { get; set; }
  
        public DbSet<LeaveStatus> leaveStatuses { get; set; }
        public DbSet<EmployeeHRInfo> employeeHRInfos { get; set; }
        public DbSet<LeaveApplication> leaveApplications { get; set; }
        public DbSet<EmployeeWorkExperience> employeeWorkExperiences { get; set; }
        public DbSet<Emergencyontact> emergencyontacts { get; set; }
        public DbSet<ERP_BL.ChartofAccounts.JournalTransaction> journalTransactions { get; set; }

        //public DbSet<JVStatus> jVStatuses { get; set; }

        public DbSet<JournalVoucher> journalVouchers { get; set; }
        public DbSet<JournalVoucherStatus> journalVoucherStatuses { get; set; }

        public DbSet<TaxType> taxTypes { get; set; }
        public DbSet<TaxName> taxNames { get; set; }
   
        public DbSet<Field> fields { get; set; }
        public DbSet<ModuleFields> moduleFields { get; set; }
        public DbSet<Template> templates { get; set; }
        //public DbSet<TemplateFields> templateFields { get; set; }

        public DbSet<AdminBill> adminBills { get; set; }
        public DbSet<Payee> payees { get; set; }
        public DbSet<PayeeCategory> payeeCategories { get; set; }

        public DbSet<AdminBillStatus> adminBillStatuses { get; set; }

        public DbSet<BillRefNumber> billRefNumbers { get; set; }
        public DbSet<AdminBillType> adminBillTypes { get; set; }
        public DbSet<Reconcilation> reconcilations { get; set; }
        public DbSet<CostSheetBillField> costSheetBillFields { get; set; }
        public DbSet<CostSheetPOField> costSheetPOFields { get; set; }
        public DbSet<CostSheetSOField> costSheetSOFields { get; set; }



        public DbSet<Payment> payments { get; set; }
        public DbSet<PaymentStatus> paymentStatuses { get; set; }
        public DbSet<PaymentMethod> paymentMethods { get; set; }
        public DbSet<PurchaseInvoice> purchaseInvoices { get; set; }
        public DbSet<PurchaseInvoiceStatus> purchaseInvoiceStatuses { get; set; }

        public DbSet<ERP_BL.BackgroundImages.BackgroundImages> BackgroundImages { get; set; }
        public DbSet<ERP_BL.PopupNotificatios.popupNotifications> popupNotifications { get; set; }
        public DbSet<PQDocument> PQDocuments { get; set; }


        public DbSet<RentalAssets> RentalAssets { get; set; }
       
        public DbSet<RentalAssetStatus> RentalAssetStatuses { get; set; }
        
        public DbSet<RentalAssetMethod> RentalAssetMethods { get; set; }
        public DbSet<Tenant> tenants { get; set; }
        public DbSet<ShippingTerm> ShippingTerms { get; set; }
        public DbSet<TenancyContract> tenancyContracts { get; set; }
        //public DbSet<RentalAgreement> rentalAgreements { get; set; }

        public DbSet<NotificationFlag> notificationFlags { get; set; }

        public DbSet<AssetOwner> assetOwners { get; set; }
  

        public DbSet<Loans> loans { get; set; }
        public DbSet<LoansStatus> loansStatuses { get; set; }
        public DbSet<FacilityNature> facilityNatures { get; set; }

        public DbSet<RoleField> roleFields { get; set; }
        public DbSet<CostSheetSaleReceiptField> costSheetSaleReceiptFields { get; set; }

        // public DbSet<NotOwnedRentalAsset> notOwnedRentalAssets { get; set; }
        public DbSet<RentalReceiveAmount> rentalReceiveAmounts { get; set; }
        public DbSet<BillCategory> billCategories { get; set; }
        public DbSet<CostSheetPaymentField> costSheetPaymentFields { get; set; }

        public DbSet<PaymentDeduction> paymentDeductions { get; set; }

        public DbSet<ManagementSummary> managementSummaries { get; set; }
        public DbSet<AdminBillNature> adminBillNatures { get; set; }
        public DbSet<Inventory> inventories { get; set; }
        public DbSet<SplitPER> splitPERs { get; set; }
        public DbSet<Religion> religions { get; set; }
        public DbSet<BillItem> billItems { get; set; }

        public DbSet<TaskGroups> taskGroups { get; set; }
        public DbSet<ToDoTask> toDoTasks { get; set; }
        public DbSet<TaskTargetType> taskTargetTypes { get; set; }
        public DbSet<TargetRewardNature> targetRewardNatures { get; set; }
        public DbSet<ToDoTaskTheme> toDoTaskThemes { get; set; }

        public DbSet<VendorBillReference> vendorBillReferences { get; set; }
        public DbSet<ExchangeRateGroup> exchangeRateGroups { get; set; }
        public DbSet<ExchangeRate> excRates { get; set; }
        public DbSet<VendorBillNature> vendorBillNatures { get; set; }
        public DbSet<ToDoTaskStatus> toDoTaskStatuses { get; set; }
        public DbSet<SharedGridGroup> sharedGridGroups { get; set; }
        public DbSet<SharedReport> sharedReports { get; set; }

        public DbSet<PettyCash> pettyCashes { get; set; }
        public DbSet<Country> countries { get; set; }
        public DbSet<City> cities { get; set; }
        public DbSet<CustomReport> customReports { get; set; }

        public DbSet<CustomReportGeoup> customReportGeoups { get; set; }

        public DbSet<MainBank> mainBanks { get; set; }

        public DbSet<LoansAdvance> loansAdvances { get; set; }
        public DbSet<LoanApplicantType> loanApplicantTypes { get; set; }
        public DbSet<LoanApplicant> loanApplicants { get; set; }
        public DbSet<LoansAdvanceStatus> loansAdvanceStatuses { get; set; }
        public DbSet<PersonPhoto> personPhotos { get; set; }
        public DbSet<CostSheetSIField> costSheetSIFields { get; set; }

        public DbSet<LoginUserDetails> loginUserDetails { get; set; }
        public DbSet<Adjustment> adjustments { get; set; }
        public DbSet<VendorBillAdjustment> vendorBillAdjustments { get; set; }

        public DbSet<EmploymentSalary> employmentSalaries { get; set; }
        public DbSet<Payroll> payrolls { get; set; }
        public DbSet<SalaryAllowance> salaryAllowances { get; set; }
        public DbSet<Allowance> allowances { get; set; }
        public DbSet<SalaryBonus> salaryBonuses { get; set; }
        public DbSet<Bonus> bonuses { get; set; }
        public DbSet<SalaryDeduction> salaryDeductions { get; set; }
        public DbSet<SalDeduction> salDeductions { get; set; }

        public DbSet<Tasks> tasks { get; set; }
        public DbSet<TasksStatus> taskStatuses { get; set; }
        public DbSet<TaskTracking> taskTrackings { get; set; }
        public DbSet<ComparativeStatement> comparativeStatements { get; set; }
        public DbSet<ComparativeStatementItem> comparativeStatementItems { get; set; }
        public DbSet<FOCSampling> fOCSamplings { get; set; }
        public DbSet<ClaimDiscount> claimDiscounts { get; set; }
        public DbSet<BookerStatementItem> bookerStatementItems { get; set; }
        public DbSet<PassOn> passOns { get; set; }
        public DbSet<ReceiptTax> receiptTaxes { get; set; }
        public DbSet<UniqueNumber> uniqueNumbers { get; set; }
        public DbSet<TaskEfficiency> taskEfficiencies { get; set; }
        public DbSet<EfficiencyPoints> efficiencyPoints { get; set; }

        public DbSet<BudgetCostSheet> budgetCostSheets { get; set; }

        public DbSet<BudgetCostField> budgetCostFields { get; set; }
        public DbSet<BudgetSheetHead> budgetCostHeads { get; set; }
      
        public DbSet<BudgetCostSheetStatus>  budgetCostSheetStatuses { get; set; }
        public DbSet<BudgetSystemCostField> budgetSystemCostFields { get; set; }
        public DbSet<TaskType> taskTypes { get; set; }
        public DbSet<TargetGroup> targetGroups { get; set; }
  		public DbSet<PerformanceSheet> performanceSheets { get; set; }
        public DbSet<PerformanceSheetHead> performanceSheetHeads { get; set; }
        public DbSet<TargetRewards> targetRewards { get; set; }
        public DbSet<StatusCalculationType> statusCalculationTypes { get; set; }
        public DbSet<SoCalculationFields> SoCalculationFields { get; set; }
        public DbSet<SaleOrdeRrefKey> saleOrdeRrefKeys { get; set; }
        public DbSet<STL> STLs { get; set; }
        public DbSet<TargetRewardStatus> targetRewardStatuses { get; set; }
        public DbSet<STLStatus> STLStatuses { get; set; }

        public DbSet<InterBankTransferVAT> interBankTransferVATs { get; set; }
        public DbSet<IBTbankCharges> IBTbankCharges { get; set; }

        public DbSet<TravelingRecords> travelingRecords { get; set; }
        public DbSet<Traveler> travelers { get; set; }
        public DbSet<VisitingCountry> visitingCountries { get; set; }
        public DbSet<TravelingStatus> travelingStatuses { get; set; }
        public DbSet<EmployeeCoaCompanies> employeeCoaCompanies { get; set; }
        public DbSet<ResidentCountry> residentCountries { get; set; }
        public DbSet<Airline> airlines { get; set; }
        public DbSet<STLInterestType> interestTypes { get; set; }
        public DbSet<STLInterest> interests { get; set; }
        public DbSet<MarginPercentage> marginPercentages { get; set; }
        public DbSet<MarginPercentageType> marginPercentageTypes { get; set; }
        public DbSet<STLSettlement> sTLSettlements { get; set; }
        public DbSet<ReversalSettlement> reversalSettlements { get; set; }

        public DbSet<Poll> polls { get; set; }
        public DbSet<InventoryAdjustment> inventoryAdjustments { get; set; }
        public DbSet<InventoryAdjustmentStatus> adjustmentStatuses { get; set; }

        public DbSet<RentedVehicleOwner> rentedVehicleOwners { get; set; }
        public DbSet<MaintenanceHead> maintenanceHeads { get; set; }
        public DbSet<VehicleExpenses> vehicleExpenses { get; set; }

        public DbSet<Warehouse> warehouses { get; set; }
        public DbSet<LotNumber> lotNumbers { get; set; }
        public DbSet<ReportTitle> reportTitles { get; set; }

        public DbSet<Checklist> checklists { get; set; }
        public DbSet<PackingStyle> packingStyles { get; set; }
       
        public DbSet<StatusClass> statusClasses { get; set; }
        public DbSet<GoodReceiveNote> goodReceiveNotes {  get; set; }
        public DbSet<TaskComment> taskComments { get; set; }
        public DbSet<CustomerCredit> customerCredits { get; set; }

        public DbSet<RentalAssetNature> rentalAssetNatures { get; set; }
        public DbSet<RentalAssetSubNature> assetSubNatures { get; set; }
        public DbSet<AssetRental> assetRentals { get; set; }
        public DbSet<AssetType> assetTypes { get; set; }
        public DbSet<AssetBrand> assetBrands { get; set; }
        public DbSet<AssetModel> assetModels { get; set; }
        public DbSet<AssetNumber> assetNumbers { get; set; }
        public DbSet<AssetRentalLocation> assetRentalLocations { get; set; }
        public DbSet<AssetRentalUnit> assetRentalUnits { get; set; }
        public DbSet<RentalContract> rentalContracts { get; set; }
        public DbSet<RentalContractStatus> rentalContractStatuses { get; set; }
        public DbSet<RentalOrder> rentalOrders { get; set; }
        public DbSet<RentalOrderStatus> rentalOrderStatuses { get; set; }
        public DbSet<RentalInvoice> rentalInvoices { get; set; }
        public DbSet<RentalInvoiceStatus> rentalInvoiceStatuses { get; set; }
        public DbSet<AssetRentalStatus> assetRentalStatuses { get; set; }
        public DbSet<TenantRental> tenantRentals { get; set; }
        public DbSet<SecurityDeposit> securityDeposits { get; set; }
        public DbSet<AuditYearAdjustment> auditYearAdjustments { get; set; }
        public DbSet<ChartofAccountGroup> chartofAccountGroups { get; set; }
        public DbSet<TenantRentalStatus> tenantRentalStatuses { get; set; }
        public DbSet<ModuleContract> ModuleContracts { get; set; }
        public DbSet<ModuleContractStatus> ModuleContractStatuses { get; set; }
        public DbSet<ERP_BL.Databases.DepartmentLevel> DepartmentLevels { get; set; }
        public DbSet<Memo> memos { get; set; }
        public DbSet<EmployeePerformanceReview> employeePerformanceReviews { get; set; }
        public DbSet<PerformanceIndicatorDefinition> performanceIndicatorDefinitions { get; set; }
        public DbSet<PerformanceIndicatorRating> performanceIndicatorRatings { get; set; }
        public DbSet<ERP_BL.VATBook.VATBook> VATBooks { get; set; }
        public DbSet<ERP_BL.VATBook.VATBookRefNumber> VATBookRefNumbers { get; set; }
        //public DbSet<ERP_BL.CashFlow.CashFlow> CashFlows { get; set; }
        //public DbSet<ERP_BL.CashFlow.CashFlowRefNumber> CashFlowRefNumbers { get; set; }

        public DbSet<Document> documents { get; set; }
        public DbSet<DocumentStatus> documentStatuses { get; set; }
        public DbSet<DocumentType> documentTypes { get; set; }
        public DbSet<DocumentTemplate> documentTemplates { get; set; }
        public DbSet<DocumentAuthority> documentAuthorities { get; set; }
        public DbSet<ERP_BL.CashFlow.CashFlow> cashFlows { get; set; }
    }

    public class DBContextERPDBInitializer : DropCreateDatabaseAlways<DBContextERP>
    {
    }
}
