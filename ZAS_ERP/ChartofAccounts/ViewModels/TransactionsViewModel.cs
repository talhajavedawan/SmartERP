using ERP_BL.ChartofAccounts;
using ERP_BL.Databases;
using ERP_BL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZAS_ERP.ChartofAccounts.ViewModels
{
   public class TransactionsViewModel
    {
        public int Id { get; set; }
        public double Debit { get; set; }
        public double Credit { get; set; }
        public string Memo { get; set; }
        public ChartofAccount Account { get; set; }
        public User Creator { get; set; }
        public bool isAdjustment { get; set; }
        public string transactionRef { get; set; }
        public DateTime? creationDate { get; set; }
        public coaTransactionsType coaTransactionsType { get; set; }
        public virtual JournalVoucher journalVoucher { get; set; }

    }
}
