using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZAS_ERP.Payments.ModelViews
{
    class PaymentRegisterModelView
    {
        public int Id { get; set; }

        public int transactionGroupId { get; set; }

        public string Module { get; set; }

        public string Template { get; set; }

        public DateTime? CreationDate { get; set; }

        //public int? AdminBill_Id { get; set; }
        //    [ForeignKey("AdminBill_Id")]
        //    [InverseProperty("Payments")]
        //    public virtual AdminBill adminBill { get; set; }

        //    public int? Bill_Id { get; set; }
        //    [ForeignKey("Bill_Id")]
        //    [InverseProperty("Payments")]
        //    public virtual Bill Bill { get; set; }

        //    public int? PInvoice_Id { get; set; }
        //    [ForeignKey("PInvoice_Id")]
        //    [InverseProperty("Payments")]
        //    public PurchaseInvoice purchaseInvoice { get; set; }


        public string Company { get; set; }

        public string Departments { get; set; }
        
        public string CreditCardBank { get; set; }

        public string PrimaryCreditCardNo { get; set; }

        public string Vendor { get; set; }

        public DateTime? DebitedDate { get; set; }

        public DateTime? PaymentDate { get; set; }

        public string SystemRefNo { get; set; }
        public string PaymentRefNo { get; set; }
        public string Currency { get; set; }
            
        public double PaymentAmount { get; set; }

        public string Status { get; set; }

        public string PaymentMethod { get; set; }

        public string Bank { get; set; }

        public string Account { get; set; }

        public string PaymentRefNo1 { get; set; } //It is used as a Payment Ref No but previously created for Admin Bill on requirement

        public string InstrumentNo { get; set; }

        public DateTime? InstrumentDate { get; set; }

        public DateTime? BillCreationDate { get; set; }

        public string BillFinanceRefNo { get; set; }

        public string BillNumber { get; set; }

        public DateTime? BillingMonth { get; set; }

        public DateTime? BillDueDate { get; set; }

        public double BillAmount { get; set; }
            //public double AmountDue { get; set; }
        public double DebitedAmount { get; set; }
        public double Deductions { get; set; }



        public string stage { get; set; }

        public bool isVoid { get; set; }
        public bool? isReviewed { get; set; }
        public bool? needReview { get; set; }

        public bool? PendingForClosing { get; set; }
        public bool? PendingForReApproval { get; set; }

        public bool? isApproved { get; set; }
        public DateTime? ApprovedDate { get; set; }

        public bool? isReApproved { get; set; }
        public DateTime? ReApprovalDate { get; set; }
        public string user { get; set; }

        public DateTime? ClosingDate { get; set; }

        public DateTime? LastStatusChangeDate { get; set; }        
    }
}
