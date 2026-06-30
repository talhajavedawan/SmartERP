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
   public class VendorBillPaymentModelView
    {
        public int Id { get; set; }
        public int BillId { get; set; }
        public DateTime BillCreationDate { get; set; }
        public string vendor { get; set; }
        public string currency { get; set; }
        public string customer { get; set; }
        public string BillFinanceRefNo { get; set; }
        public string SystemRefNo { get; set; }
        public DateTime BillDate { get; set; }
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
        public string VendorBillStage { get; set; }
        public List<CostSheetPaymentField> costSheetPaymentFields = new List<CostSheetPaymentField>();
        public List<CostSheetSOField> costSheetSOFields = new List<CostSheetSOField>();
        public List<CostFieldValues> costfieldValues = new List<CostFieldValues>();
        public List<CostFieldValues> costFieldValue = new List<CostFieldValues>();
        List<CostFieldValues> costfieldCheckedValues = new List<CostFieldValues>();
        public int? transactionHolderId { get; set; }
        public DateTime holderChangeDate { get; set; }
    }
}
