using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.Payments;
using ERP_BL.Procurements.AdminBills;
using ERP_BL.Procurements.InterBankTransfers;
using ERP_BL.Procurements.LoansAdvances;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.VATBook
{

    public class VATBook : INotifyPropertyChanged
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string FinanceRefNo { get; set; }
        public string SystemRefNo { get; set; }
        public string VATBookRef { get; set; }
        public double debit { get; set; }
        public double credit { get; set; }
        public double MER { get; set; }
        public double total { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? GLPostingDate { get; set; }
        public TransactionItemType TransactionType { get; set; }
        public int? companyId { get; set; }
        [ForeignKey("companyId")]
        public virtual Company company { get; set; }
        public int? customerId { get; set; }
        [ForeignKey("customerId")]
        public virtual CustomerCompany customerCompany { get; set; }
        public int? vendorId { get; set; }
        [ForeignKey("vendorId")]
        public virtual Vendor vendor { get; set; }
        public int? deptId { get; set; }
        [ForeignKey("deptId")]
        public virtual Department department { get; set; }
        public string Description { get; set; }
        public int? currencyId { get; set; }
        [ForeignKey("currencyId")]
        public virtual Currency OriginalCurrency { get; set; }
        //public int? SaleOrderId { get; set; }
        //[ForeignKey("SaleOrderId")]
        //public virtual SaleOrder saleOrder { get; set; }
        public int? saleInvoiceId { get; set; }
        [ForeignKey("saleInvoiceId")]
        public virtual SaleInvoice saleInvoice { get; set; }
        //public int? PurchaseOrderId { get; set; }
        //[ForeignKey("PurchaseOrderId")]
        //public virtual PurchaseOrder purchaseOrder { get; set; }
        public int? purchaseInvoiceId { get; set; }
        [ForeignKey("purchaseInvoiceId")]
        public virtual PurchaseInvoice purchaseInvoice { get; set; }
        public int? vendorBillId { get; set; }
        [ForeignKey("vendorBillId")]
        public virtual Bill vendorBill { get; set; }
        public int? saleReceiptId { get; set; }
        [ForeignKey("saleReceiptId")]
        public virtual SalesReceipt salesReceipt { get; set; }
        public int? interBankTransferId { get; set; }
        [ForeignKey("interBankTransferId")]
        public virtual InterBankTransfer interBankTransfer { get; set; }
        public int? paymentId { get; set; }
        [ForeignKey("paymentId")]
        public virtual Payment payment { get; set; }
        public int? adminBillId { get; set; }
        [ForeignKey("adminBillId")]
        public virtual AdminBill adminBill { get; set; }
        public int? interCompanyId { get; set; }
        [ForeignKey("interCompanyId")]
        public virtual InterCompanyBankTransfer interCompanyTransfer { get; set; }
        public int? loansAdvanceId { get; set; }
        [ForeignKey("loansAdvanceId")]
        public virtual LoansAdvance loansAdvance { get; set; }
        public int? VATBookRefNumberRefId { get; set; }
        [ForeignKey("VATBookRefNumberRefId")]
        public virtual VATBookRefNumber vatBookRefNumber { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public object GetPropertyValue(string propertyName)
        {
            return this.GetType().GetProperty(propertyName).GetValue(this, null);
        }
    }
    public class VATBookRefNumber
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int? companyId { get; set; }
        [ForeignKey("companyId")]
        public virtual Company company { get; set; }

        public string VATBookReferenceNo { get; set; }
        public bool isActive { get; set; }
    }
}
