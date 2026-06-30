using ERP_BL.ChartofAccounts;
using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZAS_ERP.SaleOrderFolder.UserControls.DirectReceiptPayment
{
    public class DirectReceiptForPaymentModelView
    {
        public int Id { get; set; }
        public PaymentTransactionType transactionType { get; set; }
        public virtual ChartofAccount COAcredit { get; set; }
        public Payment payment { get; set; }
        public int PaymentId { get; set; }
        public DateTime PaymentCreationDate { get; set; }
        public string Description { get; set; }
        public string vendor { get; set; }
        public string SystemRefNo { get; set; }
        public string PaymentRefNo { get; set; }
        public string currency { get; set; }
        public double PaymentAmount { get; set; }
        public double RemainingAmount { get; set; }
        public double CreditedAmount { get; set; }
        public double Deductions { get; set; }
        public double VAT { get; set; }
        public double BankCharges { get; set; }
        public double ExchangeRate { get; set; }
        public double DeductionSOC { get; set; }
        public double BankChargesSOC { get; set; }
        public double TotalDeductionSOC { get; set; }
        public double Total { get; set; }
        public string PaymentStage { get; set; }
        public List<ReceiptDeduction> receiptDeductions = new List<ReceiptDeduction>();
        public List<ReceiptDeduction> bankChargess = new List<ReceiptDeduction>();
        public List<ReceiptTax> receiptDedTaxes { get; set; }
        public List<ReceiptTax> receiptBankTaxes { get; set; }
        public double dedVAT { get; set; }
        public double bankVAT { get; set; }
        public bool? IsBankAdjusted { get; set; }
        public bool? IsDedAdjusted { get; set; }
    }
}
