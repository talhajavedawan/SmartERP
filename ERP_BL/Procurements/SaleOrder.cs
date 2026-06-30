using ERP_BL.Enums;
using ERP_BL.Procurements;
using ERP_BL.Procurements.Budget;
using ERP_BL.Procurements.InterBankTransfers;
using ERP_BL.Procurements.LoansAdvances;
using ERP_BL.Procurements.StatusClass;

//using ERP_BL.Procurements.StatusClass;
using ERP_BL.Tax;
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
    public class SaleOrder : INotifyPropertyChanged
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string referenceNo { get; set; } // customer side number
        public string SalesReferenceNo { get; set; }  // internal number
        public string FinanceRefrenceNo { get; set; }  // internal number
        public string commisionRefrenceNo { get; set; }  // internal number
        public string offerReferenceNo { get; set; }
        public DateTime? saleOrderDate { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? ClosingDate { get; set; }
        public DateTime? LastStatusChangeDate { get; set; }
        public DateTime? deliveryDate { get; set; }
        public DateTime? shipmentDate { get; set; }
        public DateTime? orderConfirmationDate { get; set; }
        public DateTime? billLaddingDate { get; set; }
        public DateTime? lCDate { get; set; }
        public DateTime? materialReciptDate { get; set; }
        public DateTime? revisedShipmentDate { get; set; }
        public DateTime? PaymentDueStartDate { get; set; }
        public DateTime? ExpectedPayment { get; set; }
        public DateTime? PaymentDueAgeing { get; set; }

        public int CreditDays { get; set; }

        public int targetYear { get; set; }
        public int targetMonth { get; set; }
        public string lCnumber { get; set; }
        public float exchangeRate { get; set; }
        public decimal? netCommision { get; set; } // Commision Amount

        public decimal? commision { get; set; } // Commision Amount
        public decimal? commisioninBase { get; set; } // Commision Amount
        public decimal PER { get; set; }
        public double SoAmountPER { get; set; }
        public double PERValue { get; set; }

        public decimal marginExchangeRate { get; set; } // Commision Exchange Rate Amount
        public decimal? margin { get; set; }
        public decimal? BudgetedMargininBase { get; set; }
        public decimal? SalesBudgetedMargin { get; set; }
        public decimal? BudgetedMarginPercent { get; set; }

        public decimal? RevisedMargin { get; set; }
        public decimal? RevisedMargininBase { get; set; }
        public decimal? SalesRevisedMargin { get; set; }
        public decimal? RevisedMarginPercent { get; set; }
        public decimal? ActualMarginPercent { get; set; }
        public decimal? ActualMargin { get; set; }
        public decimal? ActualMargininBase { get; set; }
        public decimal? SalesActualMargin { get; set; }
        public decimal? SystemMargin { get; set; }
        public decimal? SalesSystemMargin { get; set; }
        public decimal? SalesMarketMargin { get; set; }

        public bool? transshipment { get; set; }
        public string packing { get; set; }
        //LC Shipment Date
        public DateTime? LCShipmentDate { get; set; }
        //LC Expiry Date
        public DateTime? LCExpiryDate { get; set; }
        public string deliveryTerm { get; set; }
        //LC Amendment No
        public string LCAmedmentNo
        {
            get; set;
        }
        //LC Shpment date(After Amendment)
        public DateTime? LCShipmentAmendmentDate { get; set; }
        //LC Expiry date(After Amendement)
        public DateTime? LCExpiryAmedmentDate { get; set; }


        //public string deliveryTerm { get; set; }    // Terms Decided for Delivery
        //public string paymentTerm { get; set; }
        public string maker { get; set; }
        public string origin { get; set; }
        public string OwnDescription { get; set; }


        public string comments { get; set; }
        public string deliveryTime { get; set; } // Valid time for Delivery
        public double totalFOBValue { get; set; }

        public double totalCFRValue { get; set; }
        public double RemainingFOBValue { get; set; }

        public double RemainingCFRValue { get; set; }
        public double totalBaseFOBValue { get; set; }
        public double totalBaseCFRValue { get; set; }
        public double RemainingBaseFOBValue { get; set; }
        public double RemainingBaseCFRValue { get; set; }
        public decimal? UnInvoicedTotalWeight { get; set; }
        public decimal? UnInvoicedTotalQuantity { get; set; }
        public double ReceivedAmount { get; set; }
        public decimal? TotalWeight { get; set; }
        public decimal? TotalQuantity { get; set; }

        public double SoAmountSER { get; set; }
        public bool isVoid { get; set; } = false;
        public bool isPercentTax { get; set; }
        public double salesTax { get; set; }
        //public DateTime lastSubmissionDate { get; set; }
        public int customerCompany_Id { get; set; }
        [ForeignKey("customerCompany_Id ")]
        public virtual CustomerCompany customerCompany { get; set; }
        public bool? isReviewed { get; set; }
        public bool? needReview { get; set; }
        public string stage { get; set; }
        public string InvoiceStage { get; set; }

        public int? WarrantyId { get; set; }
        [ForeignKey("WarrantyId ")]
        public virtual Warranty Warranty { get; set; }
        public int dept_Id { get; set; }
        [ForeignKey("dept_Id ")]
        [InverseProperty("DepartmentSaleOrders")]
        public virtual Department department { get; set; }
        public int? CostSheet_Id { get; set; }
        [ForeignKey("CostSheet_Id ")]
        public virtual CostSheet CostSheet { get; set; }
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
        [InverseProperty("SaleOrders")]
        public virtual List<Vendor> vendors { get; set; }
        public virtual List<SaleInvoice> SaleInvoices { get; set; }

        public virtual List<LoansAdvance> LoansAdvances { get; set; }

        public virtual List<PurchaseOrder> PurchaseOrders { get; set; }
        [InverseProperty("SaleOrder")]
        public virtual List<Bill> Bills { get; set; }
        [InverseProperty("SaleOrder")]
        public virtual List<MemorandumSale> MemorandumSales { get; set; }

        public int principal_Id { get; set; }
        [ForeignKey("principal_Id")]
        public virtual Principal principal { get; set; }
        public int? company_Id { get; set; }
        [ForeignKey("company_Id ")]
        [InverseProperty("CompanySaleOrders")]
        public virtual Company company { get; set; }
        public int? InterDepartment_Id { get; set; }
        [InverseProperty("InterDepartmentSaleOrders")]
        [ForeignKey("InterDepartment_Id ")]
        public virtual Department InterDepartment { get; set; }
        public int? InterCompany_Id { get; set; }
        [ForeignKey("InterCompany_Id ")]
        [InverseProperty("InterCompanySaleOrders")]
        public virtual Company InterCompany { get; set; }
        public bool? isInterCompany { get; set; }

        public InquiryType saleOrdertype { get; set; }
        public int? vendorPaymentId { get; set; }
        [ForeignKey("vendorPaymentId")]
        public virtual VendorPaymentStatus vendorPaymentStatus { get; set; }
        public int? offer_Id { get; set; }
        [ForeignKey("offer_Id ")]
        public virtual Offer offer { get; set; }

        public int? bid_Id { get; set; }
        [ForeignKey("bid_Id ")]
        public virtual Bid bid { get; set; }
        public int currency_Id { get; set; }
        [ForeignKey("currency_Id")]
        public virtual Currency currency { get; set; }

        public int? costCentercurrency_Id { get; set; }
        [ForeignKey("costCentercurrency_Id")]
        public virtual Currency costcenterCurrency { get; set; }

        public double costCenterExchangeRate { get; set; }
        public double costCenterAmount { get; set; }
        public int paymentterm_Id { get; set; }
        [ForeignKey("paymentterm_Id")]
        public virtual PaymentTerm paymentTerm { get; set; }
        public int incoterm_Id { get; set; }
        [ForeignKey("incoterm_Id")]
        public virtual Incoterm incoterm { get; set; }

        public int? BillRefNoId { get; set; }
        [ForeignKey("BillRefNoId")]
        public virtual VendorBillReference BillRefNo { get; set; }

        public virtual ICollection<ProcurementProduct> products { get; set; }
        public virtual SaleOrderStatus saleOrderStatus { get; set; }
        public bool? PendingForClosing { get; set; }
        public bool? PendingForReApproval { get; set; }
        public bool? isApproved { get; set; } = true;
        public DateTime? ReApprovalDate { get; set; }
        public bool? isReApproved { get; set; } = true;
        public DateTime? ApprovedDate { get; set; }
        public int? TitleValue1Id { get; set; }
        [ForeignKey("TitleValue1Id")]
        public virtual Incoterm TitleValue1 { get; set; }
        public int? TitleValue2Id { get; set; }
        [ForeignKey("TitleValue2Id")]
        public virtual Incoterm TitleValue2 { get; set; }
        public int? saleExchangerateId { get; set; }
        [ForeignKey("saleExchangerateId")]
        public SalesExchangeRate saleExchangerate { get; set; }
        public int? marketExchangerateId { get; set; }
        [ForeignKey("marketExchangerateId")]
        public MarketExchangeRate marketExchangerate { get; set; }
        public int? taxNameId { get; set; }
        [ForeignKey("taxNameId")]
        public virtual TaxName taxName { get; set; }
        public DateTime? deliveryDateFinal { get; set; }
        public virtual List<SplitPER> SplitPERs{ get; set; }
        public double totaltaxAmount { get; set; }
        public int? interBankTransfer_Id { get; set; }
        [ForeignKey("interBankTransfer_Id ")]
        public virtual InterBankTransfer InterBankTransfer { get; set; }
        public double totalComissionSER { get; set; }
        public double totalComissionMER { get; set; }
        public double totalNetComissionSER { get; set; }
        public double totalNetComissionMER { get; set; }
        public int? ParentSO_Id { get; set; }
        [ForeignKey("ParentSO_Id ")]
        public virtual SaleOrder ParentSaleOrder { get; set; }
        public virtual List<BookerStatementItem> BookerStatementItems { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public double totalDistributionAmount { get; set; }

        public double totalGSTAmount { get; set; }

        public double totalAmountAfterGST { get; set; }

        public double totalClaimDiscount { get; set; }
        public double totalPassOn { get; set; }
        public double totalFocSampling { get; set; }
        public double totalNetAmount { get; set; }
        public double revisedBudgetAmount { get; set; }
        public double ccSER { get; set; }
        public double ccMER { get; set; }

        public int? Budget_Id { get; set; }
        [ForeignKey("Budget_Id")]
        public virtual BudgetCostSheet BudgetCostSheet { get; set; }
        public int? PerformanceSheet_Id { get; set; }
        [ForeignKey("PerformanceSheet_Id")]
        public virtual PerformanceSheet PerformanceSheet { get; set; }
        public DateTime? auditYear { get; set; }

        public int? SaleOrderKey_Id { get; set; }
        [ForeignKey("SaleOrderKey_Id")]
        public virtual SaleOrdeRrefKey referenceKey { get; set; }
        public int? transactionHolderId { get; set; }
        [ForeignKey("transactionHolderId")]
        public virtual Employee TransactionHolder { get; set; }
        public DateTime holderChangeDate { get; set; }
        public object GetPropertyValue(string propertyName)
        {
            //returns value of property Name
            return this.GetType().GetProperty(propertyName).GetValue(this, null);
        }
        //// This method is called by the Set accessor of each property.  
        //// The CallerMemberName attribute that is applied to the optional propertyName  
        //// parameter causes the property name of the caller to be substituted as an argument.  
        //private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}
        public int? statusClass_Id { get; set; }
        [ForeignKey("statusClass_Id")]
        public virtual StatusClass StatusClass { get; set; }
        public bool isGeneratedBySOLink { get; set; }

        public int? auditYearAdjustment_Id { get; set; }
        [ForeignKey("auditYearAdjustment_Id")]
        public virtual AuditYearAdjustment AuditYearAdjustment { get; set; }
        public int? moduleContract_Id { get; set; }
        [ForeignKey("moduleContract_Id ")]
        public virtual ModuleContract moduleContract { get; set; }

    }

    public class SaleOrderStatus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Status { get; set; }
        public bool isApproved { get; set; }
        public bool? isActive { get; set; }
        public virtual List<SaleOrder> SaleOrders { get; set; }
        public string backcolor { get; set; }
        public string forecolor { get; set; }
        public bool isDisable { get; set; }

        [InverseProperty("soStatuses")]
        public virtual List<StatusClass> soStatusSubClasses { get; set; }
    }

    public class SplitPER
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime? Year { get; set; }
        public DateTime? Month { get; set; }
        public double Amount { get; set; }

        public int? saleOrderId { get; set; }
        [ForeignKey("saleOrderId")]
        [InverseProperty("SplitPERs")]
        public virtual SaleOrder saleOrder { get; set; }
    }
    public class PerformanceSheet
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string soNumber { get; set; }
        public int? customerId { get; set; }
        [ForeignKey("customerId")]
        public CustomerCompany customer { get; set; }
        public int? deptId { get; set; }
        [ForeignKey("deptId")]
        public Department department { get; set; }
        public int? supervoisedId { get; set; }
        [ForeignKey("supervoisedId")]
        public Employee supervoisedBy { get; set; }
        public int? staffLevelOneId { get; set; }
        [ForeignKey("staffLevelOneId")]
        public Employee staffLevelOne { get; set; }
        public int? staffLevelTwoId { get; set; }
        [ForeignKey("staffLevelTwoId")]
        public Employee staffLevelTwo { get; set; }
        public double totalPoints { get; set; }
        public double totalPointsPerc { get; set; }
        public double totalAveragePoints { get; set; }
        public virtual List<PerfomarmanceSheetField> performanceSheetFields{ get; set; }

  

    }
    public class PerfomarmanceSheetField
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? Head_Id { get; set; }
        [ForeignKey("Head_Id")]
        public virtual PerformanceSheetHead PerformanceSheetHead { get; set; }
        public double point { get; set; }
        public double revisedPoint { get; set; }
    }
    public class PerformanceSheetHead
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string HeadName { get; set; }
        public bool isActive { get; set; }
        public int SortId { get; set; }
        public int? creatorId { get; set; }
        [ForeignKey("creatorId")]
        public virtual User Creator { get; set; }
        public double totalPoints { get; set; }
    }

    public class SaleOrdeRrefKey
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string key { get; set; }
        [Required]
        public DateTime keyDate { get; set; }
        [Required]
        public string Creator { get; set; }


        [Required]
        public int dept_Id { get; set; }
        [ForeignKey("dept_Id ")]
        public virtual Department department { get; set; }
        [Required]
        public int comp_Id { get; set; }
        [ForeignKey("comp_Id ")]
        public virtual Company Company { get; set; }
        public  string salesRefNo { get; set; }
        public  int SaleOrderNumber { get; set; }
        public virtual double amountOC { get; set; }
        

    }
    public class AllOrdersView
    {
        public int Id { get; set; }
        public int? GroupId { get; set; }
        public int? ParentId { get; set; }

        public string Company { get; set; }
        public string Vendor { get; set; }
        public string Department { get; set; }
        public string Stage { get; set; }
        public TransactionItemType transactionType { get; set; }
        public double AmountOC { get; set; }
        public string SalesReference { get; set; }
        public string Currency { get; set; }
        public DateTime CreationDate { get; set; }

        // Add this Composite Key property
        public string CompositeKey => $"{Id}-{transactionType}";

        private string _Status { get; set; }
        private string _BackColor { get; set; }
        private string _StatusClass { get; set; }
        private string _BackColorStatusClass { get; set; }

        public string Status
        {
            get { return _Status; }
            set
            {
                _Status = value;
                NotifyPropertyChanged("Status");
            }
        }

        public string StatusClass
        {
            get { return _StatusClass; }
            set
            {
                _StatusClass = value;
                NotifyPropertyChanged("StatusClass");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string BackColor
        {
            get { return _BackColor; }
            set
            {
                _BackColor = value;
                NotifyPropertyChanged("BackColor");
            }
        }

        public string BackColorStatusClass
        {
            get { return _BackColorStatusClass; }
            set
            {
                _BackColorStatusClass = value;
                NotifyPropertyChanged("BackColorStatusClass");
            }
        }
    }

}
