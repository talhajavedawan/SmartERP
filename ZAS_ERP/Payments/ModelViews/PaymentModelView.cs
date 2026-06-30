using ERP_BL.Databases;
using ERP_BL.Payments;
using ERP_BL.Tax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ZAS_ERP.Procurementss.Payments.ModelViews
{
    public class PaymentModelView
    {
        public int id { get; set; }
        public int BillId { get; set; }
        public int GroupId { get; set; }
        public DateTime BillCreationDate { get; set; }
        public string vendor { get; set; }
        public string CoaDebit { get; set; }
        public string CoaCredit { get; set; }
        public string AdminBillLink { get; set; }
        public string AdminBillNature { get; set; }
        public string Currency { get; set; }
        public string PrimaryCardNo { get; set; }
        public string SecondaryCardNo { get; set; }
        public string BillFinanceRefNo { get; set; }
        public string BillNumber { get; set; }
        public DateTime BillingMonth { get; set; }
        public DateTime BillDueDate { get; set; }
        public double OriginalAmount { get; set; }
        public double AmountDue { get; set; }
        public double DebitedAmount { get; set; }
        public double Deductions { get; set; }
        public double VAT { get; set; }
        public double Total { get; set; }
        public Button SyetemCost { get; set; }
        public int CostSheetId { get; set; }
        public Button CostSheet { get; set; }
        public List<PaymentDeduction> paymentDeductions { get; set; }
        public List<PaymentTax> paymentTaxes { get; set; }
        public bool? IsAdjusted { get; set; }
        public string AdminBillStage { get; set; }

        public string Memo { get; set; }
        public string SystemRefNo { get; set; }
        public int? transactionHolderId { get; set; }
        public DateTime holderChangeDate { get; set; }
    }
}
