using ERP_BL.ChartofAccounts;
using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.Payments;
using ERP_BL.Procurements.Budget;
using ERP_BL.Procurements.Inventories;
using ERP_BL.Procurements.StatusClass;
using ERP_BL.Tax;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.Databases
{
   public class PurchaseInvoice
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string PIReferenceNo { get; set; }
        public string SOReferenceNo { get; set; }
        public string SalesReferenceNo { get; set; }  // internal number
        public string FinanceRefrenceNo { get; set; }  // internal number
        public string VendorName { get; set; }  // internal number
        public string OfferReferenceNo { get; set; }
        public double totalInvoiceAmount { get; set; }
        public double POCFRValue { get; set; }
        public double totalBaseAmount { get; set; }
        public float exchangeRate { get; set; }
        public decimal marginExchangeRate { get; set; }
        public string stage { get; set; }
        public DateTime? ClosingDate { get; set; }
        public string POReferenceNo { get; set; }
        public DateTime? PODate { get; set; }
        public DateTime? PODeliveryDate { get; set; }
        public DateTime? SODate { get; set; }
        public DateTime? SODeliveryDate { get; set; }
        public DateTime? LastStatusChangeDate { get; set; }
        public DateTime? CreationDate { get; set; }
        public InquiryType PurchaseInvoicetype { get; set; }
        public virtual PurchaseInvoiceStatus PurchaseInvoiceStatus { get; set; }
        public bool isVoid { get; set; } = false;

        public bool? isReviewed { get; set; }
        public bool? needReview { get; set; }

        public bool? PendingForClosing { get; set; }
        public bool? PendingForReApproval { get; set; }
        public bool? isApproved { get; set; }
        public DateTime? ApprovedDate { get; set; }

        public DateTime? ReApprovalDate { get; set; }
        public bool? isReApproved { get; set; }


        public int currency_Id { get; set; }
        [ForeignKey("currency_Id")]
        public virtual Currency currency { get; set; }

        public int? purchaseOrder_Id { get; set; }
        [ForeignKey("purchaseOrder_Id ")]
        public virtual PurchaseOrder PurchaseOrder { get; set; }
        public int dept_Id { get; set; }
        [ForeignKey("dept_Id ")]
        [InverseProperty("DepartmentPurchaseInvoices")]
        public virtual Department department { get; set; }
        public int? company_Id { get; set; }
        [ForeignKey("company_Id ")]
        [InverseProperty("CompanyPurchaseInvoices")]
        public virtual Company company { get; set; }
        public int? InterDepartment_Id { get; set; }
        [InverseProperty("InterDepartmentPurchaseInvoices")]
        [ForeignKey("InterDepartment_Id ")]
        public virtual Department InterDepartment { get; set; }
        public int? InterCompany_Id { get; set; }
        [ForeignKey("InterCompany_Id ")]
        [InverseProperty("InterCompanyPurchaseInvoices")]
        public virtual Company InterCompany { get; set; }
        public bool? isInterCompany { get; set; }
        public int customerCompany_Id { get; set; }
        [ForeignKey("customerCompany_Id ")]
        public virtual CustomerCompany customerCompany { get; set; }

        public int? user_Id { get; set; }
        [ForeignKey("user_Id")]
        public virtual User user { get; set; }
        public int allocation_Id { get; set; }
        [ForeignKey("allocation_Id")]
        public virtual ERP_BL.Databases.Employee employee { get; set; }
        public decimal? TotalWeight { get; set; }
        public decimal? TotalQuantity { get; set; }

        public int? vendor_Id { get; set; }
        [ForeignKey("vendor_Id ")]
        public virtual Vendor Vendor { get; set; }
        public virtual ICollection<ProcurementProduct> products { get; set; }

        [InverseProperty("PurchaseInvoices")]
        public virtual List<Vendor> vendors { get; set; }
        public int? vendorPaymentId { get; set; }
        [ForeignKey("vendorPaymentId")]
        public virtual VendorPaymentStatus vendorPaymentStatus { get; set; }virtual
        public int? tax_Id { get; set; }
        [ForeignKey("tax_Id")]
        public virtual TaxName tax { get; set; }

        [InverseProperty("purchaseInvoice")]
        public virtual List<Payment> Payments { get; set; }
        public virtual ICollection<JournalTransaction> journalTransactions { get; set; }
        public decimal POCER { get; set; }
        public decimal PIAmuontSOC { get; set; }
        public int? POPaymentterm_Id { get; set; }
        [ForeignKey("POPaymentterm_Id")]
        public virtual PaymentTerm POPaymentTerm { get; set; }
        public int? incoterm_Id { get; set; }
        [ForeignKey("incoterm_Id")]
        public virtual PaymentTerm POIncoTerm { get; set; }
        public int? TitleValue1Id { get; set; }
        [ForeignKey("TitleValue1Id")]
        public virtual Incoterm TitleValue1 { get; set; }
        public int? TitleValue2Id { get; set; }
        [ForeignKey("TitleValue2Id")]
        public virtual Incoterm TitleValue2 { get; set; }
        public int? CostSheet_Id { get; set; }
        [ForeignKey("CostSheet_Id ")]
        public virtual CostSheet CostSheet { get; set; }


        public virtual ICollection<Inventory> Inventories { get; set; }
        public DateTime? GLPostingDate { get; set; }
        public double totaltaxAmount { get; set; }
        public bool isAdjustedTax { get; set; }
        public virtual List<BookerStatementItem> BookerStatementItems { get; set; }
        public virtual List<BudgetSystemCostField> BudgetSystemCostFields { get; set; }
        public int? transactionHolderId { get; set; }
        [ForeignKey("transactionHolderId")]
        public Employee TransactionHolder { get; set; }
        public DateTime holderChangeDate { get; set; }
        public int? statusClass_Id { get; set; }
        [ForeignKey("statusClass_Id")]
        public virtual StatusClass StatusClass { get; set; }
        public virtual List<ERP_BL.VATBook.VATBook> VATBooks { get; set; }

        public int? VATBookRefId { get; set; }
        [ForeignKey("VATBookRefId")]
        public virtual ERP_BL.VATBook.VATBookRefNumber VATBookRefNumber { get; set; }
    }
    public class PurchaseInvoiceStatus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Status { get; set; }
        public bool isApproved { get; set; }
        public bool? isActive { get; set; }
        public virtual List<PurchaseInvoice> PurchaseInvoices { get; set; }
        public string backcolor { get; set; }
        public string forecolor { get; set; }
        public int HierarchicalIndex { get; set; }
        public bool isDisable { get; set; }

        [InverseProperty("piStatuses")]
        public virtual List<StatusClass> piStatusSubClasses { get; set; }

    }
}
