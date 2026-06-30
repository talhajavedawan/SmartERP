using ERP_BL.CashBook;
using ERP_BL.ChartofAccounts;
using ERP_BL.CreditCards;
using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.Procurements.AdminBills;
using ERP_BL.Procurements.Budget;
using ERP_BL.Procurements.LoansAdvances;
using ERP_BL.Procurements.StatusClass;
using ERP_BL.Tax;
using ERP_BL.ToDoTasks;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.Payments
{
    public class Payment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public PaymentTransactionType transactionType { get; set; }

        public PaymentAdminBillTemplate? paymentAdminBillTemplate { get; set; }
        public PaymentVendorBillTemplate? paymentVendorBillTemplate { get; set; }
        public PaymentPurchaseInvoiceTemplate? paymentPurchaseInvoiceTemplate { get; set; }
        public PaymentLoansAdvancesTemplate? paymentLoansAdvancesTemplate { get; set; }
        public PaymentTargetRewardsTemplate? paymentTargetRewardsTemplate { get; set; }

        public DateTime? CreationDate { get; set; }

        public int transactionGroupId { get; set; }

        public int? AdminBill_Id { get; set; }
        [ForeignKey("AdminBill_Id")]
        [InverseProperty("Payments")]
        public virtual AdminBill adminBill { get; set; }

        public int? Bill_Id { get; set; }
        [ForeignKey("Bill_Id")]
        [InverseProperty("Payments")]
        public virtual Bill Bill { get; set; }

        public int? PInvoice_Id { get; set; }
        [ForeignKey("PInvoice_Id")]
        [InverseProperty("Payments")]
        public virtual PurchaseInvoice purchaseInvoice { get; set; }

        public int? TargetReward_Id { get; set; }
        [ForeignKey("TargetReward_Id")]
        [InverseProperty("Payments")]
        public virtual TargetRewards targetRewards { get; set; }

        public int? taskGroups_Id { get; set; }
        [ForeignKey("taskGroups_Id")]
        public virtual TaskGroups taskGroups { get; set; }

        [InverseProperty("payment")]
        public virtual List<SalesReceipt> salesReceipts { get; set; }

        public int? company_Id { get; set; }
        [ForeignKey("company_Id")]
        public virtual Company company { get; set; }

        //public int? dept_Id { get; set; }
        //[ForeignKey("dept_Id")]
        public virtual List<Department> departments { get; set; }

        public int? creditCardBankId { get; set; }
        [ForeignKey("creditCardBankId")]
        public virtual Bank creditCardBank { get; set; }

        public int? PrimaryCreditCardNoId { get; set; }
        [ForeignKey("PrimaryCreditCardNoId")]
        public virtual CreditCard primaryCreditCardNo { get; set; }

        public int? vendor_Id { get; set; }
        [ForeignKey("vendor_Id")]
        public virtual Vendor vendor { get; set; }

        public DateTime? DebitedDate { get; set; }

        public DateTime? PaymentDate { get; set; }

        public string SystemRefNo { get; set; }
        public string PaymentRefNo { get; set; }

        public int? currency_Id { get; set; }
        [ForeignKey("currency_Id")]
        public virtual Currency currency { get; set; }

        public double PaymentAmount { get; set; }

        public int? statusId { get; set; }
        [ForeignKey("statusId")]
        public virtual PaymentStatus Status { get; set; }

        public int? paymentMethodId { get; set; }
        [ForeignKey("paymentMethodId")]
        public virtual PaymentMethod paymentMethod { get; set; }

        public int? bankId { get; set; }
        [ForeignKey("bankId")]
        public virtual Bank bank { get; set; }

        public int? accountId { get; set; }
        [ForeignKey("accountId")]
        public virtual Account account { get; set; }

        public int? PaymentRefNoId { get; set; }
        [ForeignKey("PaymentRefNoId")]
        public virtual BillRefNumber PaymentRefNo1 { get; set; } //It is used as a Petty Cash Ref No but previously created for Admin Bill on requirement

        public string InstrumentNo { get; set; }

        public DateTime? InstrumentDate { get; set; }

        public DateTime? BillCreationDate { get; set; }

        public string BillFinanceRefNo { get; set; }

        public string BillNumber { get; set; }

        public DateTime? BillingMonth { get; set; }

        public DateTime? BillDueDate { get; set; }        

        public double BillAmount { get; set; }
        //public double AmountDue { get; set; }
        public double DebitedAmount { get; set; }
        public double Deductions { get; set; }
        public double DeductionExchangeRate { get; set; }
        public double DeductionSOC { get; set; }


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

        public bool createdFromBill { get; set; }
        public virtual List<JournalTransaction> journalTransactions { get; set; }
        public virtual List<PaymentDeduction> paymentDeductions { get; set; }
    
        public Payment()
        { this.paymentDeductions = new List<PaymentDeduction>(); }

        public int? InterCompany_Id { get; set; }
        [InverseProperty("InterCompanyPayments")]
        [ForeignKey("InterCompany_Id ")]
        public virtual Company InterCompany { get; set; }
        public bool? isInterCompany { get; set; }

        public int? InterDepartment_Id { get; set; }
        [InverseProperty("InterDepartmentPayments")]
        [ForeignKey("InterDepartment_Id ")]
        public virtual Department InterDepartment { get; set; }
        public int? CostSheetId { get; set; }
        [ForeignKey("CostSheetId")]
        public virtual CostSheet CostSheet { get; set; }
        public bool isPostToGL { get; set; }
        public DateTime? GLPostingDate { get; set; }

        public virtual List<PettyCash> pettyCashes { get; set; }
        public bool? isDeposit { get; set; }
        public bool isBypassBank { get; set; }
        public int? coaAccountId { get; set; }
        [ForeignKey("coaAccountId")]
        public virtual ChartofAccount ChartofAccount { get; set; }

        public int? LoansAdvanceId { get; set; }
        [ForeignKey("LoansAdvanceId")]
        [InverseProperty("Payments")]
        public virtual LoansAdvance loansAdvance { get; set; }

        public bool? IsAdjusted { get; set; }
        public virtual List<PaymentTax> PaymentTaxes { get; set; }
        public double totalVATamount { get; set; }

        public int? paymentterm_Id { get; set; }
        [ForeignKey("paymentterm_Id")]
        public virtual PaymentTerm paymentTerm { get; set; }
        public virtual List<BudgetSystemCostField> BudgetSystemCostFields { get; set; }
        public int? transactionHolderId { get; set; }
        [ForeignKey("transactionHolderId")]
        public ERP_BL.Databases.Employee TransactionHolder { get; set; }
        public DateTime holderChangeDate { get; set; }
        public int? statusClass_Id { get; set; }
        [ForeignKey("statusClass_Id")]
        public virtual ERP_BL.Procurements.StatusClass.StatusClass StatusClass { get; set; }
        public virtual List<ERP_BL.VATBook.VATBook> VATBooks { get; set; }

        public int? VATBookRefId { get; set; }
        [ForeignKey("VATBookRefId")]
        public virtual ERP_BL.VATBook.VATBookRefNumber VATBookRefNumber { get; set; }

        public bool isVATBookPosted { get; set; }

        public bool insuranceRequired { get; set; }
        public bool insuranceApplied { get; set; }
        public bool insuranceNotApplicable { get; set; }

        public int? insuranceAppliedBy_Id { get; set; }
        [ForeignKey("insuranceAppliedBy_Id")]
        public virtual ERP_BL.Databases.Employee insuranceAppliedBy { get; set; }
    }

    public class PaymentTax
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? taxNameId { get; set; }
        [ForeignKey("taxNameId")]
        public virtual TaxName taxName { get; set; }
        public double Amount { get; set; }

    }

    public class PaymentStatus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Status { get; set; }
        public bool isApproved { get; set; }
        public bool? isActive { get; set; }
        public virtual List<Payment> payments { get; set; }
        public string backcolor { get; set; }
        public string forecolor { get; set; }
        public int HierarchicalIndex { get; set; }
        public bool isDisable { get; set; }
        public bool isPaid { get; set; }
        [InverseProperty("paymentStatuses")]
        public virtual List<StatusClass> paymentStatusSubClasses { get; set; }
    }

    public class PaymentMethod
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string MethodName { get; set; }
        public bool isActive { get; set; }
    }
}
