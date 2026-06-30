using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZAS_ERP.SaleOrderFolder.UserControls.RentalReceipts
{
    public class RentalReceiptModelView
    {
        public int Id { get; set; }
        public int RentalInvoiceId { get; set; }
        public DateTime InvoiceCreationDate { get; set; }
        public string RentalBasis { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public DateTime Month { get; set; }
        public string currency { get; set; }
        public string SystemReferenceNo { get; set; }
        //public string FinanceRefrenceNo { get; set; }
        public double InvoiceAmount { get; set; }
        public double ReceiivedAmount { get; set; }
        public double RemainingAmount { get; set; }
        public double CreditedAmount { get; set; }
        //public double Deductions { get; set; }
        //public double VAT { get; set; }
        //public double ExchangeRate { get; set; }
        //public double DeductionSOC { get; set; }
        public double Total { get; set; }
        //public List<PaymentDeduction> paymentDeductions { get; set; }
        //public List<PaymentTax> paymentTaxes { get; set; }
        //public bool? IsAdjusted { get; set; }
        public string RentalInvoiceStage { get; set; }
        public int? transactionHolderId { get; set; }
        public DateTime holderChangeDate { get; set; }
    }
}
