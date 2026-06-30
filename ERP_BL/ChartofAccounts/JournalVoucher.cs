using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.Procurements.InterBankTransfers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.ChartofAccounts
{
    public class JournalVoucher
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string entryNo { get; set; }
        public string voucherRefno { get; set; }
        public DateTime postingDate { get; set; }
        [Required]
        public virtual ICollection<JournalTransaction> journalTransactions { get; set; }
        public int? userId { get; set; }
        [ForeignKey("userId")]
        public virtual ERP_BL.Databases.User user { get; set; }
        public bool isVoid { get; set; } = false;
        public coaTransactionsType coaTransactionsType { get; set; }
        public int? statusId { get; set; }
        [ForeignKey("statusId")]
        public virtual JournalVoucherStatus JournalVoucherStatus { get; set; }
        public DateTime? LastStatusChangeDate { get; set; }
        public bool? isReviewed { get; set; }
        public bool? needReview { get; set; }
        public bool? PendingForClosing { get; set; }
        public bool? PendingForReApproval { get; set; }
        public bool? isApproved { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public DateTime? ReApprovalDate { get; set; }
        public bool? isReApproved { get; set; }
        public string stage { get; set; }
        public DateTime? ClosingDate { get; set; }
        public int? currencyId { get; set; }
        [ForeignKey("currencyId")]
        public virtual Currency Currency { get; set; }
        public int? company_Id { get; set; }
        [ForeignKey("company_Id ")]
        public virtual Company company { get; set; }
        public int? dept_Id { get; set; }
        [ForeignKey("dept_Id")]
        public virtual Department department { get; set; }
        public int? emp_Id { get; set; }
        [ForeignKey("emp_Id")]
        public virtual ERP_BL.Databases.Employee employee { get; set; }
        public double MER { get; set; }
      
        public int? bill_Id { get; set; }
        [ForeignKey("bill_Id")]
        public virtual Bill Bill { get; set; }

        public int? purchaseOrder_Id { get; set; }
        [ForeignKey("purchaseOrder_Id")]
        public virtual PurchaseOrder PurchaseOrder { get; set; }
        public int paymentGroupId { get; set; }
        public int receiptGroupId { get; set; }
      
      
    }
    public class JournalVoucherStatus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Status { get; set; }
        public bool isApproved { get; set; }
        public bool? isActive { get; set; }
        public virtual List<JournalVoucher> journalVouchers { get; set; }
        public string backcolor { get; set; }
        public string forecolor { get; set; }
        public int HierarchicalIndex { get; set; }
    }
}
