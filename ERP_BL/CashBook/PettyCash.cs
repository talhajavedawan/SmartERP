using ERP_BL.Databases;
using ERP_BL.Enums;
using ERP_BL.Payments;
using ERP_BL.Procurements.AdminBills;
using ERP_BL.Procurements.InterBankTransfers;
using ERP_BL.Procurements.LoansAdvances;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.CashBook
{
    public class PettyCash : INotifyPropertyChanged
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime? CreationDate { get; set; }

        public TransactionItemType TransactionType { get; set; }
        //public int TransactionId { get; set; }

        public int? companyId { get; set; }
        [ForeignKey("companyId")]
        public virtual Company company { get; set; }

        public int? deptId { get; set; }
        [ForeignKey("deptId")]
        public virtual Department department { get; set; }

        public string Description { get; set; }

        public int? currencyId { get; set; }
        [ForeignKey("currencyId")]
        public virtual Currency OriginalCurrency { get; set; }

        public string FinanceRefNo { get; set; }
        public string SystemRefNo { get; set; }

        public double debit { get; set; }
        public double credit { get; set; }

        public int? SaleReceiptId { get; set; }
        [ForeignKey("SaleReceiptId")]
        public virtual SalesReceipt SalesReceipt { get; set; }

        //public bool taxFlag { get; set; }
        public int? interBankTransferId { get; set; }
        [ForeignKey("interBankTransferId")]
        public virtual InterBankTransfer interBankTransfer { get; set; }

        public int? PaymentId { get; set; }
        [ForeignKey("PaymentId")]
        public virtual Payment Payment { get; set; }

        public int? AdminBillId { get; set; }
        [ForeignKey("AdminBillId")]
        public virtual AdminBill adminBill { get; set; }

        public double MER { get; set; }
        public double total { get; set; }

        public int? InterCompanyId { get; set; }
        [ForeignKey("InterCompanyId")]
        public virtual InterCompanyBankTransfer InterCompanyTransfer { get; set; }

        public int? LoansAdvanceId { get; set; }
        [ForeignKey("LoansAdvanceId")]
        public virtual LoansAdvance loansAdvance { get; set; }

        public int? billId { get; set; }
        [ForeignKey("billId")]
        public virtual Bill bill { get; set; }

        public int? PettyCashRefId { get; set; }
        [ForeignKey("PettyCashRefId")]
        public virtual BillRefNumber PettyCashRef { get; set; }

        //public bool? isVoid { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public object GetPropertyValue(string propertyName)
        {
            //returns value of property Name
            return this.GetType().GetProperty(propertyName).GetValue(this, null);
        }
    }

    
}
