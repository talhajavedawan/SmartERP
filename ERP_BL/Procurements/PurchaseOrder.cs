using ERP_BL.ChartofAccounts;
using ERP_BL.Enums;
using ERP_BL.Procurements.Budget;
using ERP_BL.Procurements.LoansAdvances;
using ERP_BL.Procurements.StatusClass;
using ERP_BL.Tax;
using ERP_BL.ToDoTasks.Taskss;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.Databases
{

    public class PurchaseOrder : INotifyPropertyChanged
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string POReferenceNo { get; set; }
        public string SOReferenceNo { get; set; }
        public string SalesReferenceNo { get; set; }  // internal number
        public string FinanceRefrenceNo { get; set; }  // internal number
        public string VendorName { get; set; }  // internal number
        public string OfferReferenceNo { get; set; }
        public string SyetmReferenceNo { get; set; }


        public DateTime? saleOrderDate { get; set; }
        public DateTime? PurchaseOrderDate { get; set; }

        public DateTime? CreationDate { get; set; }
        public DateTime? ClosingDate { get; set; }
        public DateTime? LastStatusChangeDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public DateTime? ShipmentDate { get; set; }
        public DateTime? OrderConfirmationDate { get; set; }
        public DateTime? BillOfLaddingDate { get; set; }
        public DateTime? lCDate { get; set; }
        public DateTime? MaterialReciptDate { get; set; }
        public DateTime? RevisedShipmentDate { get; set; }
        public DateTime? PaymentDueStartDate { get; set; }
        public DateTime? ExpectedPayment { get; set; }
        public DateTime? PaymentDueAgeing { get; set; }
        public int CreditDays { get; set; }

        public int TargetYear { get; set; }
        public int TargetMonth { get; set; }
        public string LCnumber { get; set; }
        public float ExchangeRate { get; set; }
        public double? NetCommision { get; set; } // Commision Amount

        public double? Commision { get; set; } // Commision Amount
        public double? commisioninBase { get; set; } // Commision Amount
        public double SOC_ER { get; set; }
        public double SoAmountSOC_ER { get; set; }
        //public double PERValue { get; set; }

        public double marginExchangeRate { get; set; } // Commision Exchange Rate Amount
        public double? margin { get; set; }
        public double? BudgetedMargininBase { get; set; }
        public double? SalesBudgetedMargin { get; set; }
        public double? BudgetedMarginPercent { get; set; }

        public double? RevisedMargin { get; set; }
        public double? RevisedMargininBase { get; set; }
        public double? SalesRevisedMargin { get; set; }
        public double? RevisedMarginPercent { get; set; }
        public double? ActualMarginPercent { get; set; }
        public double? ActualMargin { get; set; }
        public double? ActualMargininBase { get; set; }
        public double? SalesActualMargin { get; set; }

        public bool? transshipment { get; set; }
        public string packing { get; set; }
        //LC Shipment Date
        public DateTime? LCShipmentDate { get; set; }
        //LC Expiry Date
        public DateTime? LCExpiryDate { get; set; }
        public string deliveryTerm { get; set; }
        //LC Amendment No
        public string LCAmedmentNo { get; set; }
        //LC Shpment date(After Amendment)
        public DateTime? LCShipmentAmendmentDate { get; set; }
        //LC Expiry date(After Amendement)
        public DateTime? LCExpiryAmedmentDate { get; set; }
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
        //public double RemainingBaseFOBValue { get; set; }
        //public double RemainingBaseCFRValue { get; set; }
        public decimal? UnInvoicedTotalWeight { get; set; }
        public decimal? UnInvoicedTotalQuantity { get; set; }
        public double ReceivedAmount { get; set; }
        public decimal? TotalWeight { get; set; }
        public decimal? TotalQuantity { get; set; }

        public virtual List<LoansAdvance> LoansAdvances { get; set; }

        //New Tax Fields Added
        public bool? hasTax { get; set; }
        public int? tax_Id { get; set; }
        [ForeignKey("tax_Id")]
        public virtual TaxName tax { get; set; }

        public double? billWithTax { get; set; }

        public bool? hasWHT { get; set; }
        public int? WHT_Id { get; set; }
        [ForeignKey("WHT_Id")]
        public virtual TaxName WHT { get; set; }

        public double? billAfterTax { get; set; }

        public double POAmountSER { get; set; }
        public bool isPercentTax { get; set; }
        public double salesTax { get; set; }
        public string stage { get; set; }
        public string InvoiceStage { get; set; }
        public InquiryType PurchaseOrdertype { get; set; }
        public virtual ICollection<ProcurementProduct> products { get; set; }
        public virtual PurchaseOrderStatus PurchaseOrderStatus { get; set; }
        public bool isVoid { get; set; } = false;

        public bool? isReviewed { get; set; }
        public bool? needReview { get; set; }
        
        public bool? PendingForClosing { get; set; }
        public bool? PendingForReApproval { get; set; }
        public bool? isApproved { get; set; }
        public DateTime? ApprovedDate { get; set; }

        public DateTime? ReApprovalDate { get; set; }
        public bool? isReApproved { get; set; }
        public int? saleOrder_Id { get; set; }
        [ForeignKey("saleOrder_Id ")]
        public virtual SaleOrder SaleOrder { get; set; }
        public int? CostSheet_Id { get; set; }
        [ForeignKey("CostSheet_Id ")]
        public virtual CostSheet CostSheet { get; set; }
        public int? CommissionSummarySheetId { get; set; }
        [ForeignKey("CommissionSummarySheetId ")]
        public virtual CommissionSummarySheet CommissionSummarySheet { get; set; }
        public int dept_Id { get; set; }
        [ForeignKey("dept_Id ")]
        [InverseProperty("DepartmentPurchaseOrders")]
        public virtual Department department { get; set; }
        public int? company_Id { get; set; }
        [ForeignKey("company_Id ")]
        [InverseProperty("CompanyPurchaseOrders")]
        public virtual Company company { get; set; }
        public int? InterDepartment_Id { get; set; }
        [InverseProperty("InterDepartmentPurchaseOrders")]
        [ForeignKey("InterDepartment_Id ")]
        public virtual Department InterDepartment { get; set; }
        public int? InterCompany_Id { get; set; }
        [ForeignKey("InterCompany_Id ")]
        [InverseProperty("InterCompanyPurchaseOrders")]
        public virtual Company InterCompany { get; set; }
        public bool? isInterCompany { get; set; }
        public int customerCompany_Id { get; set; }
        [ForeignKey("customerCompany_Id ")]
        public virtual CustomerCompany customerCompany { get; set; }

        public int? SOWarrantyId { get; set; }
        [ForeignKey("SOWarrantyId ")]
        public virtual Warranty SOWarranty { get; set; }
        public int? POWarrantyId { get; set; }
        [ForeignKey("POWarrantyId ")]
        public virtual Warranty POWarranty { get; set; }
        public int? user_Id { get; set; }
        [ForeignKey("user_Id")]
        public virtual User user { get; set; }
        public int allocation_Id { get; set; }
        [ForeignKey("allocation_Id")]
        public virtual Employee AllocateTo { get; set; }
        
        public int? vendorPaymentId { get; set; }
        [ForeignKey("vendorPaymentId")]
        public virtual VendorPaymentStatus vendorPaymentStatus { get; set; }
        [InverseProperty("PurchaseOrders")]
        public virtual List<Vendor> vendors { get; set; }
        [InverseProperty("PurchaseOrder")]
        public virtual List<Bill> Bills { get; set; }

        //public int principal_Id { get; set; }
        //[ForeignKey("principal_Id")]
        //public virtual Principal principal { get; set; }

        public int? SoPaymentterm_Id { get; set; }
        [ForeignKey("SoPaymentterm_Id")]
        public virtual PaymentTerm SoPaymentTerm { get; set; }
        public int POPaymentterm_Id { get; set; }
        [ForeignKey("POPaymentterm_Id")]
        public virtual PaymentTerm POPaymentTerm { get; set; }
        public int? bid_Id { get; set; }
        [ForeignKey("bid_Id ")]
        public virtual Bid bid { get; set; }
        public int currency_Id { get; set; }
        [ForeignKey("currency_Id")]
        public virtual Currency currency { get; set; }
        public int? SOCurrency_Id { get; set; }
        [ForeignKey("SOCurrency_Id")]
        public virtual Currency SOCurrency { get; set; }
        public int incoterm_Id { get; set; }
        [ForeignKey("incoterm_Id")]
        public virtual Incoterm incoterm { get; set; }
       
        public int? TitleValue1Id { get; set; }
        [ForeignKey("TitleValue1Id")]
        public virtual Incoterm TitleValue1 { get; set; }
        public int? TitleValue2Id { get; set; }
        [ForeignKey("TitleValue2Id")]
        public virtual Incoterm TitleValue2 { get; set; }
       
        public string proforma { get; set; }
        public string stlRefNo{ get; set; }
        public double totaltaxAmount { get; set; }
        public virtual List<PurchaseInvoice> PurchaseInvoices { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
       
        public object GetPropertyValue(string propertyName)
        {
            //returns value of property Name
            return this.GetType().GetProperty(propertyName).GetValue(this, null);
        }
        [InverseProperty("PurchaseOrder")]
        public List<JournalVoucher> journalVouchers { get; set; }

        public int CostSheetFieldId { get; set; }


        public bool isAdjustedTax { get; set; }

        public bool isDiscount { get; set; }
        public double Discount { get; set; }

        public bool isFreight { get; set; }
        public double Freight { get; set; }
        public bool isCOO { get; set; }
        public double COO { get; set; }



        public double totalAmount { get; set; }
        public double totalAmountGST { get; set; }
        public double totalClaimDisount { get; set; }
        public double totalPassOn { get; set; }
        public double totalFocSampling { get; set; }
        public double totalPOAdvance { get; set; }
        public double expectedPaymentAmount { get; set; }
        public double totalPOSattled { get; set; }


        public virtual List<BookerStatementItem> BookerStatementItems { get; set; }
        public int? Budget_Id { get; set; }
        [ForeignKey("Budget_Id")]
        public virtual BudgetCostSheet BudgetCostSheet { get; set; }

        public int? transactionHolderId { get; set; }
        [ForeignKey("transactionHolderId")]
        public Employee TransactionHolder { get; set; }
        public DateTime holderChangeDate { get; set; }
        public int? statusClass_Id { get; set; }
        [ForeignKey("statusClass_Id")]
        public virtual StatusClass StatusClass { get; set; }

        public string lotNo { get; set; }

        public int? lotNumberId { get; set; }
        [ForeignKey("lotNumberId")]
        public virtual LotNumber lotNumber { get; set; }
    }

    public class PurchaseOrderStatus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Status { get; set; }
        public bool isApproved { get; set; }
        public bool? isActive { get; set; }
        public virtual List<PurchaseOrder> PurchaseOrders { get; set; }
        public string backcolor { get; set; }
        public string forecolor { get; set; }
        public int HierarchicalIndex { get; set; }
        public bool isDisable { get; set; }
        [InverseProperty("poStatuses")]
        public virtual List<StatusClass> poStatusSubClasses { get; set; }

    }

}
