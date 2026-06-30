using ERP_BL.ChartofAccounts;
using ERP_BL.Enums;
using ERP_BL.Procurements.Budget;
using ERP_BL.Procurements.Inventories;
using ERP_BL.Procurements.LoansAdvances;
using ERP_BL.Procurements.StatusClass;
using ERP_BL.ToDoTasks.Taskss;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.Databases
{
    public class SaleInvoice : INotifyPropertyChanged
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string referenceNo { get; set; } // customer side number
        public string SalesReferenceNo { get; set; }  // internal number
        public string FinanceRefrenceNo { get; set; }  // internal number
        public string BatchRefrenceNo { get; set; }  // system reference
        public string commisionRefrenceNo { get; set; }  // internal number
        public string offerReferenceNo { get; set; }
        public DateTime? saleInvoiceDate { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? deliveryDate { get; set; }
        public DateTime? ETDDate { get; set; }
        public DateTime? ETADate { get; set; }
        public DateTime? ClosingDate { get; set; }
        public DateTime? LastStatusChangeDate { get; set; }
        public DateTime? LastStatusClassChangeDate { get; set; }
        //public DateTime? shipmentDate { get; set; }
        //public DateTime? orderConfirmationDate { get; set; }
        public DateTime? BLAWBDate { get; set; }
        public DateTime? lCDate { get; set; }
        public DateTime? materialReciptDate { get; set; }
        //public DateTime? revisedShipmentDate { get; set; }
        public DateTime? PaymentDueStartDate { get; set; }
        public DateTime? ExpectedPayment { get; set; }
        public DateTime? PaymentDueAgeing { get; set; }

        public DateTime? paymentOnDate { get; set; }
        public bool isPaid { get; set; }
        public bool isRedInvoice { get; set; }
        public double deliveryDays { get; set; }
        public string BLdeliveryRefNo { get; set; }
        public int CreditDays { get; set; }
        public bool isVoid { get; set; } = false;
        public int targetYear { get; set; }
        public int targetMonth { get; set; }
        public string lCnumber { get; set; }
        public float exchangeRate { get; set; }
        public decimal marginExchangeRate { get; set; } // Commision Exchange Rate Amount
        public string maker { get; set; }
        public string lotNo { get; set; }
        public string invoiceNo { get; set; }
        public DateTime? invoiceDate { get; set; }

        public string origin { get; set; }
        public string OwnDescription { get; set; }
        public string comments { get; set; }
        public string deliveryTime { get; set; } // Valid time for Delivery
        public double totalInvoiceAmount { get; set; }
        [Range(1900, 3000)]
        public int? PaymentYear { get; set; }
        [Range(0, 4)]
        public int? PaymentQuarter { get; set; }
        public double SOCFRValue { get; set; }
        //public double totalBaseFOBValue { get; set; }
        public double totalBaseAmount { get; set; }
        //public double RemainingBaseFOBValue { get; set; }
        public double RemainingBaseAmount { get; set; }
        public decimal? UnInvoicedTotalWeight { get; set; }
        public decimal? UnInvoicedTotalQuantity { get; set; }
        public double ReceivedAmount { get; set; }
        public decimal? TotalWeight { get; set; }
        public decimal? TotalQuantity { get; set; }

        public double SoAmountSER { get; set; }

        public bool isPercentTax { get; set; }
        public double salesTax { get; set; }
        //public DateTime lastSubmissionDate { get; set; }
        public int customerCompany_Id { get; set; }
        [ForeignKey("customerCompany_Id ")]
        public virtual CustomerCompany customerCompany { get; set; }
        public bool? isReviewed { get; set; }
        public bool? needReview { get; set; }
        public string stage { get; set; }
        public int dept_Id { get; set; }
        [ForeignKey("dept_Id ")]
        [InverseProperty("DepartmentSaleInvoices")]
        public virtual Department department { get; set; }

        public int? CommissionSummarySheetId { get; set; }
        [ForeignKey("CommissionSummarySheetId ")]
        public virtual CommissionSummarySheet CommissionSummarySheet { get; set; }
        public int? user_Id { get; set; }
        [ForeignKey("user_Id")]
        public virtual User user { get; set; }
        public int allocation_Id { get; set; }
        [ForeignKey("allocation_Id")]
        public virtual Employee employee { get; set; }
        //public int vendor_Id { get; set; }
        //[ForeignKey("vendor_Id")]
        [InverseProperty("SaleInvoices")]
        public virtual List<Vendor> vendors { get; set; }
        public int principal_Id { get; set; }
        [ForeignKey("principal_Id")]
        public virtual Principal principal { get; set; }
        public int? company_Id { get; set; }
        [ForeignKey("company_Id ")]
        [InverseProperty("CompanySaleInvoices")]
        public virtual Company company { get; set; }
        public int? InterDepartment_Id { get; set; }
        [InverseProperty("InterDepartmentSaleInvoices")]
        [ForeignKey("InterDepartment_Id ")]
        public virtual Department InterDepartment { get; set; }
        public int? InterCompany_Id { get; set; }
        [ForeignKey("InterCompany_Id ")]
        [InverseProperty("InterCompanySaleInvoices")]
        public virtual Company InterCompany { get; set; }
        public bool? isInterCompany { get; set; }
        public InquiryType saleInvoicetype { get; set; }
        //public int? vendorPaymentId { get; set; }
        //[ForeignKey("vendorPaymentId")]
        //public virtual VendorPaymentStatus vendorPaymentStatus { get; set; }
        public int? SaleOrderId { get; set; }
        [ForeignKey("SaleOrderId ")]
        public virtual SaleOrder SaleOrder { get; set; }

        //public int? bid_Id { get; set; }
        //[ForeignKey("bid_Id ")]
        //public virtual Bid bid { get; set; }
        public int currency_Id { get; set; }
        [ForeignKey("currency_Id")]
        public virtual Currency currency { get; set; }

        public int paymentterm_Id { get; set; }
        [ForeignKey("paymentterm_Id")]
        public virtual PaymentTerm paymentTerm { get; set; }
        public int incoterm_Id { get; set; }
        [ForeignKey("incoterm_Id")]
        public virtual Incoterm incoterm { get; set; }
        public virtual ICollection<ProcurementProduct> products { get; set; }
        public virtual SaleInvoiceStatus saleInvoiceStatus { get; set; }
        public bool? PendingForClosing { get; set; }
        public bool? isApproved { get; set; } = true;
        public DateTime? ApprovedDate { get; set; }
        //public bool? isReviewed { get; set; }
        //public bool? needReview { get; set; }
        public int? TitleValue1Id { get; set; }
        [ForeignKey("TitleValue1Id")]
        public virtual Incoterm TitleValue1 { get; set; }
        public int? TitleValue2Id { get; set; }
        [ForeignKey("TitleValue2Id")]
        public virtual Incoterm TitleValue2 { get; set; }
        [InverseProperty("saleInvoice")]
        public virtual List<SalesReceipt> salesReceipts { get; set; }
        public virtual ICollection<JournalTransaction> journalTransactions { get; set; }
        public virtual ICollection<Inventory> Inventories { get; set; }
        public string SItaxSubject { get; set; }
        public string CISubject { get; set; }
        public string DNSubject { get; set; }
        public string Commission { get; set; } 

        public DateTime? GLPostingDate { get; set; }

        public int? bank_Id { get; set; }
        [ForeignKey("bank_Id")]
        public virtual Bank bank { get; set; }

        public int? account_Id { get; set; }
        [ForeignKey("account_Id")]
        public virtual Account account { get; set; }
        public double totaltaxAmount { get; set; }

        public int? CostSheet_Id { get; set; }
        [ForeignKey("CostSheet_Id ")]
        public virtual CostSheet CostSheet { get; set; }
        public double amountSOC { get; set; }
        public virtual List<BookerStatementItem> BookerStatementItems { get; set; }

        public double totalDistributionAmount { get; set; }

        public double totalGSTAmount { get; set; }

        public double totalAmountAfterGST { get; set; }

        public double totalClaimDiscount { get; set; }
        public double totalPassOn { get; set; }
        public double totalFocSampling { get; set; }
        public double totalNetAmount { get; set; }
        public bool isInterCompanyReceivable { get; set; }
        public bool isAdvancePayment { get; set; }
        public virtual List<BudgetSystemCostField> BudgetSystemCostFields { get; set; }

        public int? transactionHolderId { get; set; }
        [ForeignKey("transactionHolderId")]
        public Employee TransactionHolder { get; set; }
        public DateTime holderChangeDate { get; set; }
        public bool isSTLGenerated { get; set; }
        public bool isSTLDiscount { get; set; }
        public int? stlDiscountCurrency_Id { get; set; }
        [ForeignKey("stlDiscountCurrency_Id")]
        public virtual Currency stlDiscountCurrency { get; set; }
        public double stlDiscountAmount { get; set; }
        public int? stlSTLCurrency_Id { get; set; }
        [ForeignKey("stlSTLCurrency_Id")]
        public virtual Currency stlCurrency { get; set; }
        public double stlAmount { get; set; }

        public int? lotNumberId { get; set; }
        [ForeignKey("lotNumberId")]
        public virtual LotNumber lotNumber { get; set; }

        public DateTime? ExpectedDiscountDate{ get; set; }
        public int? statusClass_Id { get; set; }
        [ForeignKey("statusClass_Id")]
        public virtual StatusClass StatusClass { get; set; }
        [InverseProperty("SaleInvoice")]
        public virtual List<LoansAdvance> LoansAdvances { get; set; }
        [InverseProperty("SaleInvoice")]
        public virtual List<CustomerCredit> CustomerCredits { get; set; }
        public bool insuranceRequired { get; set; }
        public bool insuranceApplied { get; set; }
        public bool insuranceNotApplicable { get; set; }

        public int? insuranceAppliedBy_Id { get; set; }
        [ForeignKey("insuranceAppliedBy_Id")]
        public virtual Employee insuranceAppliedBy { get; set; }
        public virtual List<ERP_BL.VATBook.VATBook> VATBooks { get; set; }

        public int? VATBookRefId { get; set; }
        [ForeignKey("VATBookRefId")]
        public virtual ERP_BL.VATBook.VATBookRefNumber VATBookRefNumber { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

      
    }

    public class SaleInvoiceStatus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Status { get; set; }
        public bool isApproved { get; set; }
        public bool? isActive { get; set; }
        public virtual List<SaleInvoice> SaleInvoices { get; set; }
        public string backcolor { get; set; }
        public string forecolor { get; set; }
        public bool isDisable { get; set; }
        [InverseProperty("siStatuses")]
        public virtual List<StatusClass> siStatusSubClasses { get; set; }

    }
    public class CustomerCredit  
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int SerialNo { get; set; }

        public int? SaleInvoiceId { get; set; }
        [ForeignKey("SaleInvoiceId")]
        [InverseProperty("CustomerCredits")]
        public virtual SaleInvoice SaleInvoice { get; set; }
        public int? CustomerCompanyId { get; set; }
        [ForeignKey("CustomerCompanyId")]
        public virtual CustomerCompany CustomerCompany { get; set; }
        public double creditAmount { get; set; }
    }
}
