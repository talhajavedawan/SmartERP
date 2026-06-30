using ERP_BL.CashBook;
using ERP_BL.ChartofAccounts;
using ERP_BL.CreditCards;
using ERP_BL.Enums;
using ERP_BL.Payments;
using ERP_BL.Procurements.AdminBills;
using ERP_BL.Procurements.Bill;
using ERP_BL.Procurements.Budget;
using ERP_BL.Procurements.InterBankTransfers;
using ERP_BL.Procurements.LoansAdvances;
using ERP_BL.Procurements.StatusClass;
using ERP_BL.Tax;
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

    public class Bill : INotifyPropertyChanged
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
        public DateTime? BillDate { get; set; }

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

        public int? CreditDays { get; set; }

        public int? TargetYear { get; set; }
        public int? TargetMonth { get; set; }
        public string LCnumber { get; set; }
        public float? ExchangeRate { get; set; }
        public double? NetCommision { get; set; } // Commision Amount

        public double? Commision { get; set; } // Commision Amount
        public double? commisioninBase { get; set; } // Commision Amount
        public double? SOC_ER { get; set; }
        public double? SoAmountSOC_ER { get; set; }
        //public double PERValue { get; set; }

        public double? marginExchangeRate { get; set; } // Commision Exchange Rate Amount
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

        public double? POAmountSER { get; set; }
        public bool? isPercentTax { get; set; }
        public double? salesTax { get; set; }

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

        public string SupplierReferenceNo { get; set; }
        public DateTime? SupplyDate { get; set; }

        public string stage { get; set; }
        public string InvoiceStage { get; set; }
        public int? billType_Id { get; set; }
        [ForeignKey("billType_Id ")]
        public virtual BillType Billtype { get; set; }
        public virtual ICollection<ProcurementProduct> products { get; set; }
        public virtual BillStatus BillStatus { get; set; }
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
        public int? purchaseOrder_Id { get; set; }
        [ForeignKey("purchaseOrder_Id ")]
        [InverseProperty("Bills")]
        public virtual PurchaseOrder PurchaseOrder{ get; set; }
        public int? CostSheet_Id { get; set; }
        [ForeignKey("CostSheet_Id ")]
        public virtual CostSheet CostSheet { get; set; }
        public int? CommissionSummarySheetId { get; set; }
        [ForeignKey("CommissionSummarySheetId ")]
        public virtual CommissionSummarySheet CommissionSummarySheet { get; set; }
        public int dept_Id { get; set; }
        [InverseProperty("DepartmentBills")]
        [ForeignKey("dept_Id ")]
        public virtual Department department { get; set; }
        public int? company_Id { get; set; }
        [InverseProperty("CompanyBills")]
        [ForeignKey("company_Id ")]
        public virtual Company company { get; set; }
        public int? InterDepartment_Id { get; set; }
        [InverseProperty("InterDepartmentBills")]
        [ForeignKey("InterDepartment_Id ")]
        public virtual Department InterDepartment { get; set; }
        public int? InterCompany_Id { get; set; }
        [InverseProperty("InterCompanyBills")]
        [ForeignKey("InterCompany_Id ")]
        public virtual Company InterCompany { get; set; }
        public bool? isInterCompany { get; set; }

        public int? loanAdvanceCompany_Id { get; set; }
        [InverseProperty("LoanAdvanceCompanyBills")]
        [ForeignKey("loanAdvanceCompany_Id")]
        public virtual Company loanAdvanceCompany { get; set; }

        public int? loanAdvanceDept_Id { get; set; }
        [InverseProperty("loanAdvanceDepartmentBills")]
        [ForeignKey("loanAdvanceDept_Id")]
        public virtual Department loanAdvanceDepartment { get; set; }

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
        public int? vendor_Id { get; set; }
        [ForeignKey("vendor_Id")]
        [InverseProperty("Bills")]
        public virtual Vendor vendor { get; set; }

        public int? billVendor_Id { get; set; }
        [ForeignKey("billVendor_Id")]
        [InverseProperty("billVendorBills")]
        public virtual Vendor billVendor { get; set; }

        public string billVendorName { get; set; }

        public int? POVendor_Id { get; set; }
        [ForeignKey("POVendor_Id")]
        [InverseProperty("POVendorBills")]
        public virtual Vendor POVendor { get; set; }

        public string POVendorName { get; set; }
        //public virtual List<PurchaseInvoice> PurchaseInvoices { get; set; }

        //public int principal_Id { get; set; }
        //[ForeignKey("principal_Id")]
        //public virtual Principal principal { get; set; }

        public int? SoPaymentterm_Id { get; set; }
        [ForeignKey("SoPaymentterm_Id")]
        public virtual PaymentTerm SoPaymentTerm { get; set; }
        public int? POPaymentterm_Id { get; set; }
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
        public int? incoterm_Id { get; set; }
        [ForeignKey("incoterm_Id")]
        public virtual Incoterm incoterm { get; set; }

        public int? TitleValue1Id { get; set; }
        [ForeignKey("TitleValue1Id")]
        public virtual Incoterm TitleValue1 { get; set; }
        public int? TitleValue2Id { get; set; }
        [ForeignKey("TitleValue2Id")]
        public virtual Incoterm TitleValue2 { get; set; }
        public virtual ICollection<JournalTransaction> journalTransactions { get; set; }



        public event PropertyChangedEventHandler PropertyChanged;
        public object GetPropertyValue(string propertyName)
        {
            //returns value of property Name
            return this.GetType().GetProperty(propertyName).GetValue(this, null);
        }

        public int? CardUserId { get; set; }
        [ForeignKey("CardUserId")]
        public virtual CardHolder cardUser { get; set; }

        public int? CreditCardNoId { get; set; }
        [ForeignKey("CreditCardNoId")]
        public virtual CreditCard creditCardNo { get; set; }
        [InverseProperty("Bill")]
        public virtual List<JournalVoucher> journalVouchers { get; set; }


        public int? PettyCashRefId { get; set; }
        [ForeignKey("PettyCashRefId")]
        public virtual BillRefNumber PettyCashRef { get; set; } //It is used as a Petty Cash Ref No but previously created for Admin Bill on requirement

        public bool? isDeposit { get; set; }
        public virtual List<PettyCash> pettyCashes { get; set; }


        [InverseProperty("Bill")]
        public virtual List<Payment> Payments { get; set; }

        public int? billCategoryId { get; set; }
        [ForeignKey("billCategoryId")]
        public BillCategory billCategory { get; set; }

        public bool hasSummary { get; set; }

        public int? managementSummary_Id { get; set; }
        [ForeignKey("managementSummary_Id")]
        public virtual ManagementSummary managementSummary { get; set; }

        public string SummaryMemo { get; set; }
        public virtual ICollection<BillItem> billItems { get; set; }

        public int? BillRefNoId { get; set; }
        [ForeignKey("BillRefNoId")]
        public virtual VendorBillReference BillRefNo { get; set; }

        public int? vendorBillNature_Id { get; set; }
        [ForeignKey("vendorBillNature_Id")]
        public virtual VendorBillNature vendorBillNature { get; set; }
        public DateTime? GLPostingDate { get; set; }
        public double? taxAmount { get; set; }
        [InverseProperty("Bill")]
        public virtual List<InterBankTransfer> interBankTransfers { get; set; }
        public virtual List<BudgetSystemCostField> BudgetSystemCostFields { get; set; }
        public int? transactionHolderId { get; set; }
        [ForeignKey("transactionHolderId")]
        public Employee TransactionHolder { get; set; }
        public DateTime holderChangeDate { get; set; }

        // Navigation property
        [InverseProperty("bill")]
        public virtual ICollection<VendorBillAdjustment> Adjustments { get; set; }

        // Navigation property
        [InverseProperty("vendorBill")]
        public virtual ICollection<Adjustment> Adjustmentss { get; set; }

        public int? LoansAdvanceId { get; set; }
        [ForeignKey("LoansAdvanceId")]
        [InverseProperty("bills")]
        public virtual LoansAdvance loansAdvance { get; set; }

        public int? statusClass_Id { get; set; }
        [ForeignKey("statusClass_Id")]
        public virtual StatusClass StatusClass { get; set; }
        public virtual List<ERP_BL.VATBook.VATBook> VATBooks { get; set; }

        public int? VATBookRefId { get; set; }
        [ForeignKey("VATBookRefId")]
        public virtual ERP_BL.VATBook.VATBookRefNumber VATBookRefNumber { get; set; }
    }

    public class VendorBillAdjustment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public LoansAdvanceType loansAdvanceType { get; set; }

        public int transactionGroupId { get; set; }

        public DateTime? AdjustmentDate { get; set; }

        public string ReferenceNo { get; set; }

        public double AdjustmentAmount { get; set; }

        // Foreign key and navigation property
        public int? bill_Id { get; set; }
        [ForeignKey("bill_Id")]
        [InverseProperty("Adjustments")]
        public virtual ERP_BL.Databases.Bill bill { get; set; }

        public bool? isApproved { get; set; }
        public DateTime? ApprovedDate { get; set; }
    }

    public class BillStatus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Status { get; set; }
        public bool isApproved { get; set; }
        public bool? isActive { get; set; }
        public virtual List<Bill> Bills { get; set; }
        public string backcolor { get; set; }
        public string forecolor { get; set; }
        public int HierarchicalIndex { get; set; }
        public bool isDisable { get; set; }
        [InverseProperty("billStatuses")]
        public virtual List<StatusClass> billStatusSubClasses { get; set; }


    }

    public class BillCategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Category { get; set; }
        public bool isActive { get; set; }
    }

    public class VendorBillReference
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Reference { get; set; }

        public int? companyId { get; set; }
        [ForeignKey("companyId")]
        public virtual Company company { get; set; }

        public bool isActive { get; set; }
    }


    public class VendorBillNature
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Nature { get; set; }
        public virtual List<Company> Companies { get; set; }

        public bool isActive { get; set; }
    }

}
