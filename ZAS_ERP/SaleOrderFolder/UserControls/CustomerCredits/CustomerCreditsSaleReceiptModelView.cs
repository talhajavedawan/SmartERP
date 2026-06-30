using ERP_BL.Databases;
using ERP_BL.Procurements.Budget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace ZAS_ERP.SaleOrderFolder.UserControls.CustomerCredits
{
    public class CustomerCreditsSaleReceiptModelView
    {
        public int Id { get; set; }
        public DateTime? Date { get; set; }
        public int serialNo { get; set; }
        public string Customer { get; set; }
        public string SalesReferenceNo { get; set; }
        public int InvoiceId { get; set; }
        public string FinanceRefNo { get; set; }
        public string Currency { get; set; }
        public double OriginalAmount { get; set; }
        public double AmountDue { get; set; }
        public double CreditedAmount { get; set; }
        public double Deductions { get; set; }
        public double VAT { get; set; }
        public double BankCharges { get; set; }

        public double ExchangeRate { get; set; }
        public double DeductionSOC { get; set; }
        public double BankChargesSOC { get; set; }
        public double TotalDeductionSOC { get; set; }
        public double TotalAmount { get; set; }
        public Button SyetemCost { get; set; }
        public int CostSheetId { get; set; }
        public Button CostSheet { get; set; }
        public string InvoiceStage { get; set; }
        public List<ReceiptDeduction> receiptDeductions = new List<ReceiptDeduction>();
        public List<ReceiptDeduction> bankChargess = new List<ReceiptDeduction>();
        public List<ReceiptTax> receiptDedTaxes { get; set; }
        public List<ReceiptTax> receiptBankTaxes { get; set; }

        public double dedVAT { get; set; }
        public double bankVAT { get; set; }
        public bool? IsBankAdjusted { get; set; }
        public bool? IsDedAdjusted { get; set; }
        public List<BudgetSystemCostField> BudgetSystemCostFields { get; set; }
        public int? transactionHolderId { get; set; }
        public DateTime holderChangeDate { get; set; }
    }
}
