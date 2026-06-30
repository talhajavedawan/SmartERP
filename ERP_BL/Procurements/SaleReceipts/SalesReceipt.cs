using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP_BL.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ERP_BL.ChartofAccounts;
using ERP_BL.Tax;
using ERP_BL.CashBook;
using ERP_BL.Procurements.AdminBills;
using ERP_BL.Procurements.LoansAdvances;
using ERP_BL.Procurements.Budget;
using ERP_BL.Procurements.StatusClass;
using ERP_BL.Payments;
using ERP_BL.AssetsRentals.RentalInvoices;

namespace ERP_BL.Databases
{
    public class SalesReceipt
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public ReceiptType receiptType { get; set; }
        public DateTime CreationDate { get; set; }

        public int? companyId { get; set; }
        [ForeignKey("companyId")]
        public virtual Company company { get; set; }
        public int? deptId { get; set; }
        [ForeignKey("deptId")]
        public virtual Department department { get; set; }

        public int? CustomerId { get; set; }
        [ForeignKey("CustomerId")] 
        public virtual CustomerCompany Customer { get; set; }

        public int CustomerCreditSerialNo { get; set; } //comment

        public virtual List<Department> Departments { get; set; }

        public string SystemRefNo { get; set; }
        public string ReceiptRefNo { get; set; }
        public int? CurrencyId { get; set; }
        [ForeignKey("CurrencyId")]
        public virtual Currency Currency { get; set; }

        public int? rentalInvoiceId { get; set; }
        [ForeignKey("rentalInvoiceId ")]
        [InverseProperty("salesReceipts")]
        public virtual RentalInvoice rentalInvoice { get; set; }

        public double CollectionAmount { get; set; }
        public double TotalCollectionAmount { get; set; }

        public virtual List<ReceiptDeduction> receiptDeductions { get; set; }

        public virtual List<ReceiptDeduction> bankCharges { get; set; }
        public int? collectionMethodId { get; set; }
        [ForeignKey("collectionMethodId")]
        public virtual CollectionMethod collectionMethod { get; set; }
        public int BankId { get; set; }
        [ForeignKey("BankId")]
        public virtual Bank bank { get; set; }
        public int AccountId { get; set; }
        [ForeignKey("AccountId")]
        public virtual Account account { get; set; }
        public bool AppliesToSales { get; set; }
        public int transactionGroupId { get; set; }

        public int? saleInvoiceId { get; set; }
        [ForeignKey("saleInvoiceId")]
        [InverseProperty("salesReceipts")]
        public virtual SaleInvoice saleInvoice { get; set; }

        public int? receiptId { get; set; }
        [ForeignKey("receiptId")]
        public virtual SalesReceipt salesReceipt { get; set; }

        public int? paymentId { get; set; }
        [ForeignKey("paymentId")]
        [InverseProperty("salesReceipts")]
        public virtual Payment payment { get; set; }


        public int? StatusId { get; set; }
        [ForeignKey("StatusId")]
        public virtual SalesReceiptStatus saleReceiptStatus { get; set; }


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
        public virtual User user { get; set; }

        public DateTime? ClosingDate { get; set; }
        public DateTime? LastStatusChangeDate { get; set; }

        public DateTime? CreditedDate { get; set; } //New Migration
        public DateTime? DepositedDate { get; set; } //New Migration
        public string InstrumentNo { get; set; } //New Migration
        public DateTime? InstrumentDate { get; set; } //New Migration

        public int? principal_Id { get; set; }
        [ForeignKey("principal_Id")]
        public virtual Principal principal { get; set; } //New Migration
        public virtual List<JournalTransaction> journalTransactions { get; set; }

        public int? CostSheet_Id { get; set; }
        [ForeignKey("CostSheet_Id ")]
        public virtual CostSheet CostSheet { get; set; }

        public int? costSheetFieldId { get; set; }

        public bool isPostToGL { get; set; }
        public DateTime? GLPostingDate { get; set; }
        public bool isBypassBank { get; set; }
        public int? coaAccountId { get; set; }
        [ForeignKey("coaAccountId")]
        public virtual ChartofAccount ChartofAccount { get; set; }
        public SalesReceipt()
        {
            this.receiptDeductions = new List<ReceiptDeduction>();
        }

        public virtual List<PettyCash> pettyCashes { get; set; }
        public bool? isDeposit { get; set; }
        public bool? isAmountOC { get; set; }

        public int? PettyCashRefId { get; set; }
        [ForeignKey("PettyCashRefId")]
        public virtual BillRefNumber PettyCashRef { get; set; }

        public int? LoansAdvanceId { get; set; }
        [ForeignKey("LoansAdvanceId")]
        [InverseProperty("SalesReceipts")]
        public virtual LoansAdvance loansAdvance { get; set; }

        public bool? IsAdjustedDedVAT { get; set; }
        public virtual List<ReceiptTax> ReceiptDeductionTaxes { get; set; }

        public bool? IsAdjustedBankVAT { get; set; }
        public virtual List<ReceiptTax> ReceiptBankTaxes { get; set; }

        //public double Deductions { get; set; }
        public double DeductionExchangeRate { get; set; }
        public double DeductionSOC { get; set; }

       
        //public int? receiptCOA_Id { get; set; }
        //[ForeignKey("receiptCOA_Id")]
        //public virtual ReceiptCOA receiptCOA { get; set; }

        public int? COAcredit_Id { get; set; }
        [ForeignKey("COAcredit_Id")]
        public virtual ChartofAccount COAcredit { get; set; }


        //public int? COAdebit_Id { get; set; }
        //[ForeignKey("COAdebit_Id")]
        //public virtual ChartofAccount COAdebit { get; set; }

        //public int? SalesReceipt_Id { get; set; }
        //[ForeignKey("SalesReceipt_Id")]
        //public virtual SalesReceipt SalesReceipt { get; set; }

        public string Description { get; set; }
        

        public virtual List<BudgetSystemCostField> BudgetSystemCostFields { get; set; }
        public int? transactionHolderId { get; set; }
        [ForeignKey("transactionHolderId")]
        public Employee TransactionHolder { get; set; }
        public DateTime holderChangeDate { get; set; }
        public double GLMER { get; set; }

        public int? statusClass_Id { get; set; }
        [ForeignKey("statusClass_Id")]
        public virtual StatusClass StatusClass { get; set; }
        public virtual List<ERP_BL.VATBook.VATBook> VATBooks { get; set; }

        public int? VATBookRefId { get; set; }
        [ForeignKey("VATBookRefId")]
        public virtual ERP_BL.VATBook.VATBookRefNumber VATBookRefNumber { get; set; }
    }


    //public class ReceiptCOA
    //{
    //    [Key]
    //    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    //    public int Id { get; set; }
     
    //    //public int? COAcredit_Id { get; set; }
    //    //[ForeignKey("COAcredit_Id")]
    //    //public virtual ChartofAccount COAcredit { get; set; }


  
    //    //public int? COAdebit_Id { get; set; }
    //    //[ForeignKey("COAdebit_Id")]
    //    //public virtual ChartofAccount COAdebit { get; set; }

      
    //    ////public int? SalesReceipt_Id { get; set; }
    //    ////[ForeignKey("SalesReceipt_Id")]
    //    ////public virtual SalesReceipt SalesReceipt { get; set; }

       
    //    //public string Description { get; set; }
    //    //public double CreditedAmount { get; set; }

    //}


    public class CollectionMethod
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public String MethodName { get; set; }
        public bool isActive { get; set; }
    }

    public class ReceiptTax
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? taxNameId { get; set; }
        [ForeignKey("taxNameId")]
        public virtual TaxName taxName { get; set; }

        public int? SalesReceiptForDeductionTaxId { get; set; }
        [ForeignKey("SalesReceiptForDeductionTaxId")]
        [InverseProperty("ReceiptDeductionTaxes")]
        public virtual SalesReceipt SalesReceiptForDeductionTax { get; set; }

        public int? SalesReceiptForBankChargesTaxId { get; set; }
        [ForeignKey("SalesReceiptForBankChargesTaxId")]
        [InverseProperty("ReceiptBankTaxes")]
        public virtual SalesReceipt SalesReceiptForBankChargesTax { get; set; }

        public double Amount { get; set; }

    }

    public class ReceiptDeduction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? deduction_Id { get; set; }
        [ForeignKey("deduction_Id")]
        public virtual Deduction deduction { get; set; }

        public int? SalesReceiptForDeductionId { get; set; }
        [ForeignKey("SalesReceiptForDeductionId")]
        [InverseProperty("receiptDeductions")]
        public virtual SalesReceipt SalesReceiptForDeduction { get; set; }

        public int? SalesReceiptForBankChargesId { get; set; }
        [ForeignKey("SalesReceiptForBankChargesId")]
        [InverseProperty("bankCharges")]
        public virtual SalesReceipt SalesReceiptForBankCharges { get; set; }

        public double Amount { get; set; }

    }

    public class PaymentDeduction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? deduction_id { get; set; }
        [ForeignKey("deduction_id")]
        public virtual Deduction deduction { get; set; }
        public double Amount { get; set; }

    }

   
    public class Deduction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string title { get; set; }
        public bool isActive { get; set; }
        public ReceiptType receiptType { get; set; }

        public int? chartofAccountId { get; set; }
        [ForeignKey("chartofAccountId")]
        public virtual ChartofAccount COA { get; set; }
        public double paymentAmount { get; set; }
    }

    [Table("Accounts")]
    public class Account
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public AccountsType accountType { get; set; }
        public virtual Currency currency { get; set; }
        public virtual Company company { get; set; }
        public virtual List<Department> departments { get; set; }
        public string AccountNo { get; set; }
        public string AccountNick { get; set; }
        public AccountsNature nature { get; set; }
        public string COANo { get; set; }
        // when COA is competed we will add COA FK as reference.
        public int? mainBankId { get; set; }
        [ForeignKey("mainBankId")]
        public virtual MainBank mainBank { get; set; }

        public virtual Bank bank { get; set; }//It is used as Bank branch
        public string IBAN { get; set; }

        public COA_AccountType? COA_Type { get; set; }

        public int? COA_accountId { get; set; }
        [ForeignKey("COA_accountId")]
        public virtual ChartofAccount COAaccount { get; set; }

        //public bool isPersonal { get; set; }
        public AccountsCategory accountsCategory { get; set; }

        public int? industryTypeId { get; set; }
        [ForeignKey("industryTypeId")]
        public virtual IndustryType industryType { get; set; }

        public int? vendor_Id { get; set; }
        [ForeignKey("vendor_Id")]
        public virtual Vendor vendor { get; set; }

        public bool isActive { get; set; }
        public bool isAdjustmentAccount { get; set; }

        public Account()
        {
            departments = new List<Department>();
        }
    }

    public class MainBank
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string BankName { get; set; }
        public bool isActive { get; set; }
    }

    //This is used for Branch
    public class Bank
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int? bankId { get; set; }
        [ForeignKey("bankId")]
        public virtual MainBank mainBank { get; set; }

        public string BankName { get; set; } //This is used for Branch
        public string BranchCode { get; set; }
        public string Location { get; set; }
        public string SwiftCode { get; set; }
        public virtual Address address { get; set; }
        public virtual Contact contact { get; set; }
        public virtual List<ContactPerson> contactPersons { get; set; }
        public virtual List<Company> companies { get; set; }
        public Bank()
        {
            companies = new List<Company>();
        }
    }

    public class SalesReceiptStatus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Status { get; set; }
        public bool isApproved { get; set; }
        public bool? isActive { get; set; }
        public virtual List<SalesReceipt> SaleReceipts { get; set; }
        public string backcolor { get; set; }
        public string forecolor { get; set; }
        public int HierarchicalIndex { get; set; }
        public bool isDisable { get; set; }
        [InverseProperty("srStatuses")]
        public virtual List<ERP_BL.Procurements.StatusClass.StatusClass> srStatusSubClasses { get; set; }

    }
}