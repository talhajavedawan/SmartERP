using ERP_BL.AssetsRentals;
using ERP_BL.CashBook;
using ERP_BL.ChartofAccounts;
using ERP_BL.CreditCards;
using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.Payments;
using ERP_BL.Procurements.InterBankTransfers;
using ERP_BL.Procurements.LoansAdvances;
using ERP_BL.Tax;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.Procurements.AdminBills
{
    [Table("tabAdminBill")]
    public class AdminBill
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public AdminBillTypes billTypes { get; set; }

        public bool? isProgressiveCost { get; set; }

        public int? template_Id { get; set; }
        [ForeignKey("template_Id")]
        public virtual ERP_BL.Fields.Template template { get; set; }

        public DateTime? CreationDate { get; set; }

        public int? company_Id { get; set; }
        [ForeignKey("company_Id")]
        public virtual Company company { get; set; }

        public int? dept_Id { get; set; }
        [ForeignKey("dept_Id")]
        public virtual Department department { get; set; }

        public int? emp_Id { get; set; }
        [ForeignKey("emp_Id")]
        public virtual ERP_BL.Databases.Employee employee { get; set; }

        public int? assetRentalId { get; set; }
        [ForeignKey("assetRentalId")]
        public virtual AssetRental assetRental { get; set; }

        

        //Bill Status [Class]

        public string SystemRefNo { get; set; }
        public string FinanceRefNo { get; set; }
        public string FinanceRefNo2 { get; set; }
        public DateTime? TransactionDate { get; set; }

        public int? currency_Id { get; set; }
        [ForeignKey("currency_Id")]
        public virtual Currency currency { get; set; }

        public int? statusId { get; set; }
        [ForeignKey("statusId")]
        public virtual AdminBillStatus BillStatus { get; set; }

        public int? creatorId { get; set; }
        [ForeignKey("creatorId")]
        public virtual ERP_BL.Databases.Employee Creator { get; set; }

        public int? COA_Id { get; set; }
        [ForeignKey("COA_Id")]
        public virtual ChartofAccount chartofAccount { get; set; }

        public int? CoaCredit_Id { get; set; }
        [ForeignKey("CoaCredit_Id")]
        public virtual ChartofAccount chartofAccountCredit { get; set; }

        public int? adminBillType_Id { get; set; }
        [ForeignKey("adminBillType_Id")]
        public virtual AdminBillType AdminBillType { get; set; }

        public int? vendor_Id { get; set; }
        [ForeignKey("vendor_Id")]
        public virtual Vendor vendor{ get; set; }

        public bool EmployeeForEveryBill { get; set; }

        public int? employeeForBill_Id { get; set; }
        [ForeignKey("employeeForBill_Id")]
        public virtual ERP_BL.Databases.Employee employeeForBill { get; set; }

        public int? payee_Id { get; set; }
        [ForeignKey("payee_Id")]
        public virtual Payee payee { get; set; }

        public string Bill_Number { get; set; }

        public DateTime? BillingMonthFrom { get; set; }
        public DateTime? BillingMonthTo { get; set; }
        public DateTime? DueDate { get; set; }



        public double AmountOC { get; set; }
        public double MER { get; set; }
        public double AmountMER { get; set; }

        public double TotalAmountOC { get; set; }

        public double RemainingAmountOC { get; set; }

        //These Fields will show on selection of Credit Card Template
        public int? CardUserId { get; set; }
        [ForeignKey("CardUserId")]
        public CardHolder cardUser { get; set; }

        public int? BankId { get; set; }
        [ForeignKey("BankId")]
        public virtual Bank bank { get; set; }

        public CardHolderType? CardHolderType { get; set; }

        public int? PrimaryCreditCardNoId { get; set; }
        [ForeignKey("PrimaryCreditCardNoId")]
        public virtual CreditCard primaryCreditCardNo { get; set; }

        public int? SecondaryCreditCardNoId { get; set; }
        [ForeignKey("SecondaryCreditCardNoId")]
        public virtual CreditCard secondaryCreditCardNo { get; set; }

        public int transactionGroupId { get; set; }

        public string Memo { get; set; }

        public int? BillRefNoId { get; set; }
        [ForeignKey("BillRefNoId")]
        public virtual BillRefNumber BillRefNo { get; set; }

        public string stage { get; set; }

        public bool isVoid { get; set; }
        public bool? isReviewed { get; set; }
        public bool? needReview { get; set; }

        public bool? PendingForClosing { get; set; }
        public bool? PendingForReApproval { get; set; }

        public bool? isApproved { get; set; }
        public DateTime? ApprovedDate { get; set; }

        public bool? isReApproved { get; set; }
        public DateTime? ReApprovalDate { get; set; }


        public int? user_Id { get; set; }
        [ForeignKey("user_Id")]
        public virtual ERP_BL.Databases.User user { get; set; }

        public DateTime? ClosingDate { get; set; }
        public DateTime? LastStatusChangeDate { get; set; }
        public virtual ICollection<JournalTransaction> journalTransactions { get; set; }

        [InverseProperty("adminBill")]
        public virtual List<Payment> Payments { get; set; }

        public bool isVehicleType { get; set; }
        //[InverseProperty("adminBill")]
        public virtual List<VehicleExpenses> vehicleExpenses { get; set; }
        public virtual List<VehicleExpenses> fuelExpenses { get; set; }

        public bool hasSummary { get; set; }

        public int? managementSummary_Id { get; set; }
        [ForeignKey("managementSummary_Id")]
        public virtual ManagementSummary managementSummary { get; set; }

        public string SummaryMemo { get; set; }

        public int? adminBillNature_Id { get; set; }
        [ForeignKey("adminBillNature_Id")]
        public virtual AdminBillNature adminBillNature { get; set; }

        public bool? hasWAT { get; set; }

        public int? tax_Id { get; set; }
        [ForeignKey("tax_Id")]
        public virtual TaxName tax { get; set; }

        public double TaxAmount { get; set; }
        public double AmountWithTax { get; set; }
        public bool isPostToGL { get; set; }
        public DateTime? GLPostingDate { get; set; }


        public virtual List<PettyCash> pettyCashes { get; set; }
        public bool? isDeposit { get; set; }
        public bool? isVATBookPosted { get; set; }
        public bool? isAmountOC { get; set; }

        public int? LoansAdvanceId { get; set; }
        [ForeignKey("LoansAdvanceId")]
        [InverseProperty("AdminBills")]
        public virtual LoansAdvance loansAdvance { get; set; }

        [InverseProperty("adminBill")]
        public virtual List<Adjustment> Adjustments { get; set; }
        public virtual List<InterBankTransfer> InterBankTransfers { get; set; }

        public bool? isAdjustedTax { get; set; }
        public int? statusClass_Id { get; set; }
        [ForeignKey("statusClass_Id")]
        public virtual ERP_BL.Procurements.StatusClass.StatusClass StatusClass { get; set; }
        public virtual List<ERP_BL.VATBook.VATBook> VATBooks { get; set; }

        public int? VATBookRefId { get; set; }
        [ForeignKey("VATBookRefId")]
        public virtual ERP_BL.VATBook.VATBookRefNumber VATBookRefNumber { get; set; }
        public AdminBill()
        {
            this.vehicleExpenses = new List<VehicleExpenses>();
            this.fuelExpenses = new List<VehicleExpenses>();
        }
    }

    public class Adjustment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public LoansAdvanceType loansAdvanceType { get; set; }

        public int transactionGroupId { get; set; }

        public DateTime? AdjustmentDate { get; set; }

        public string ReferenceNo { get; set; }

        public double AdjustmentAmount { get; set; }

        public int? adminBillId { get; set; }
        [ForeignKey("adminBillId")]
        [InverseProperty("Adjustments")]
        public virtual AdminBill adminBill { get; set; }

        public int? vendorBill_Id { get; set; }
        [ForeignKey("vendorBill_Id")]
        [InverseProperty("Adjustmentss")]
        public virtual ERP_BL.Databases.Bill vendorBill { get; set; }

        public bool? isApproved { get; set; }
        public DateTime? ApprovedDate { get; set; }
    }


    public class PayeeCategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }
        public bool? isActive { get; set; }

    }

    public class Payee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string PayeeName { get; set; }

        public int? ParentId { get; set; }
        [ForeignKey("ParentId")]
        public virtual Payee parentPayee { get; set; }

        public virtual List<Company> Companies { get; set; }

        public virtual List<Department> departments { get; set; }

        public virtual List<AdminBillType> AdminBillTypes { get; set; }

        public virtual List<Vendor> vendors { get; set; }

        public bool isActive { get; set; }
        public bool isSubsidiary { get; set; }

        public bool isVehicleType { get; set; }
        public bool isOwned { get; set; }

        public int? companyId { get; set; }
        [ForeignKey("companyId")]
        public virtual Company company { get; set; }

        public int? vehicleOwnerId { get; set; }
        [ForeignKey("vehicleOwnerId")]
        public virtual RentedVehicleOwner VehicleOwner { get; set; }

        public Payee()
        {
            AdminBillTypes = new List<AdminBillType>();
            vendors = new List<Vendor>();
            Companies = new List<Company>();
            departments = new List<Department>();
        }
    }

    public class RentedVehicleOwner
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string OwnerName { get; set; }
        public bool isActive { get; set; }
    }

    public class VehicleExpenses
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int transactionGroupId { get; set; }

        public string SystemRefNo { get; set; }

        public DateTime? CreationDate { get; set; }

        public DateTime? FuelDate { get; set; }

        public VehicleExpenseType expenseType { get; set; }

        public double CurrentMeterReading { get; set; }
        public double LastMeterReading { get; set; }
        public double MeterReadingDifference { get; set; }

        public double Litres { get; set; }

        public double ExpenseAmount { get; set; }

        public int? payeeId { get; set; }
        [ForeignKey("payeeId")]
        public virtual Payee payee { get; set; }

        public int? maintenanceHeadId { get; set; }
        [ForeignKey("maintenanceHeadId")]
        public virtual MaintenanceHead maintenanceHead { get; set; }

        public int? AdminBillForVehicleExpenses_Id { get; set; }
        [ForeignKey("AdminBillForVehicleExpenses_Id")]
        [InverseProperty("vehicleExpenses")]
        public virtual AdminBill AdminBillForVehicleExpenses { get; set; }

        public int? AdminBillForFuelExpenses_Id { get; set; }
        [ForeignKey("AdminBillForFuelExpenses_Id")]
        [InverseProperty("fuelExpenses")]
        public virtual AdminBill AdminBillForFuelExpenses { get; set; }
    }

    public class MaintenanceHead
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string HeadName { get; set; }
        public bool isActive { get; set; }
    }

    public class AdminBillStatus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Status { get; set; }
        public bool isApproved { get; set; }
        public bool? isActive { get; set; }
        public virtual List<AdminBill> Bills { get; set; }
        public string backcolor { get; set; }
        public string forecolor { get; set; }
        public int HierarchicalIndex { get; set; }
        public bool isDisable { get; set; }
        [InverseProperty("adminBillStatuses")]
        public virtual List<ERP_BL.Procurements.StatusClass.StatusClass> adminBillStatusSubClasses { get; set; }

    }

    //Used in multiple modules
    public class BillRefNumber
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int? companyId { get; set; }
        [ForeignKey("companyId")]
        public virtual Company company { get; set; }

        public string BillReferenceNo { get; set; }

        public bool isActive { get; set; }
    }

    public class AdminBillType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string name { get; set; }

        public bool isApproved { get; set; }

        public bool isActive { get; set; }

        public int? user_Id { get; set; }
        [ForeignKey("user_Id")]
        public virtual ERP_BL.Databases.User user { get; set; }

        public DateTime addedDate { get; set; }

        public virtual List<Vendor> vendors { get; set; }
        public virtual List<ChartofAccount> ChartofAccounts { get; set; }

        public virtual ICollection<Payee> payees { get; set; }

        public AdminBillType()
        { }
    }

    public class ManagementSummary
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string SummaryName { get; set; }
        public bool isActive { get; set; }

        public int? ParentId { get; set; }
        [ForeignKey("ParentId")]
        public virtual ManagementSummary parentSummary { get; set; }
    }

    public class AdminBillNature
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Nature { get; set; }
        public virtual List<Company> Companies { get; set; }

        public bool isActive { get; set; }
    }
}
