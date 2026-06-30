using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZAS_ERP.Payments.ModelViews
{
    class CompanyLoanPaymentModelView
    {
        public int Id { get; set; }
        public int LoansAdvanceId { get; set; }
        public DateTime LACreationDate { get; set; }
        public string Employee { get; set; }
        public string ApplicantType { get; set; }
        public string ApplicantName { get; set; }
        public string currency { get; set; }
        public string SystemReferenceNo { get; set; }
        public double LoanAmount { get; set; }
        public double AmountDue { get; set; }
        public double DebitedAmount { get; set; }
        public double Total { get; set; }
        public DateTime LoanReturnDate { get; set; }
        public string LoansAdvanceStage { get; set; }
        public int? transactionHolderId { get; set; }
        public DateTime holderChangeDate { get; set; }
    }
}
