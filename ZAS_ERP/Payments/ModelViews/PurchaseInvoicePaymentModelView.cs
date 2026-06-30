using ERP_BL.Databases;
using ERP_BL.Payments;
using ERP_BL.Tax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZAS_ERP.Procurementss.SaleOrderss;

namespace ZAS_ERP.Payments.ModelViews
{
    public class PurchaseInvoicePaymentModelView
    {
        public int Id { get; set; }
        public int PInvoiceId { get; set; }
        public DateTime PICreationDate { get; set; }
        public string vendor { get; set; }
        public string currency { get; set; }
        public string customer { get; set; }
        public string PIReferenceNo { get; set; }
        public string FinanceRefrenceNo { get; set; }
        public double OriginalAmount { get; set; }
        public double AmountDue { get; set; }
        public double DebitedAmount { get; set; }
        public double Deductions { get; set; }
        public double VAT { get; set; }
        public double ExchangeRate { get; set; }
        public double DeductionSOC { get; set; }
        public double Total { get; set; }
        public List<PaymentDeduction> paymentDeductions { get; set; }
        public List<PaymentTax> paymentTaxes { get; set; }
        public bool? IsAdjusted { get; set; }
        public string PurchaseInvoiceStage { get; set; }
        public string PurchaseOrderStage { get; set; }
        public List<CostFieldValues> costfieldValues = new List<CostFieldValues>();
        public int? transactionHolderId { get; set; }
        public DateTime holderChangeDate { get; set; }
    }
}
