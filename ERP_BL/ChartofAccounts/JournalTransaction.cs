using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.Payments;
using ERP_BL.Procurements.AdminBills;
using ERP_BL.Procurements.InterBankTransfers;
using ERP_BL.ToDoTasks;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.ChartofAccounts
{
    public class JournalTransaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public double debit { get; set; }
        public double credit { get; set; }
        public string memo { get; set; }
        public DateTime? creationDate { get; set; }
        public int? accountId { get; set; }
        [ForeignKey("accountId")]
        public virtual ChartofAccount account { get; set; }
        public int? userId { get; set; }
        [ForeignKey("userId")]
        public virtual ERP_BL.Databases.User user { get; set; }
        public bool isAdjustment { get; set; }
        public string transactionRefno { get; set; }
        public coaTransactionsType coaTransactionsType { get; set; }
        public int? journalVoucher_id { get; set; }
        [ForeignKey("journalVoucher_id")]
        public virtual JournalVoucher journalVoucher { get; set; }

        public int? InterBankId { get; set; }
        [ForeignKey("InterBankId")]
        public virtual InterBankTransfer InterBank { get; set; }
        public int? SaleInvoiceId { get; set; }
        [ForeignKey("SaleInvoiceId")]
        public virtual SaleInvoice SaleInvoice { get; set; }

        public int? prodId { get; set; }
        [ForeignKey("prodId")]
        public virtual Product Product { get; set; }
      
        public int? SaleReceiptId { get; set; }
        [ForeignKey("SaleReceiptId")]
        public virtual SalesReceipt SalesReceipt { get; set; }
        public int? deptId { get; set; }
        [ForeignKey("deptId ")]
        public virtual Department department { get; set; }
        public bool taxFlag { get; set; }
        public int? AdminBillId { get; set; }
        [ForeignKey("AdminBillId")]
        public virtual AdminBill AdminBill { get; set; }
        public int? ReconcilationId { get; set; }
        [ForeignKey("ReconcilationId")]
        public virtual Reconcilation Reconcilation { get; set; }

        public bool isReconciled { get; set; }
        public DateTime? reconcilationDate { get; set; }
        public ReconcilationType reconcilationType { get; set; }
        public int? Bill_Id { get; set; }
        [ForeignKey("Bill_Id")]
        public virtual Bill Bill { get; set; }
        public int? costSheetFieldId { get; set; }
        [ForeignKey("costSheetFieldId")]
        public virtual CostSheetField CostSheetField { get; set; }
        public int? PaymentId { get; set; }
        [ForeignKey("PaymentId")]
        public virtual Payment Payment { get; set; }
        public int? PurchaseInvoiceId { get; set; }
        [ForeignKey("PurchaseInvoiceId")]
        public virtual PurchaseInvoice PurchaseInvoice { get; set; }
        public double  MER { get; set; }
        public double  total { get; set; }
        public int? companyId { get; set; }
        [ForeignKey("companyId ")]
        public virtual Company company { get; set; }
        public int? currencyId { get; set; }
        [ForeignKey("currencyId ")]
        public virtual Currency currency { get; set; }

        public int? InterCompanyId { get; set; }
        [ForeignKey("InterCompanyId")]
        public virtual InterCompanyBankTransfer InterCompanyTransfer { get; set; }

        public int? TargetRewardId { get; set; }
        [ForeignKey("TargetRewardId")]
        public virtual TargetRewards TargetReward { get; set; }

        public int? STLId { get; set; }
        [ForeignKey("STLId")]
        public virtual STL STL { get; set; }


    }
}
