using ERP_BL.ChartofAccounts;
using ERP_BL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZAS_ERP.ChartofAccounts.ViewModels
{
   public class ReconcilationViewModel
    {
        public int Id { get; set; }
        public coaTransactionsType coaTransactionsType { get; set; }
        public DateTime? creationDate { get; set; }
        public ChartofAccount chartofAccount { get; set; }
        public string memo { get; set; }
        public double amount { get; set; }
        public double balance { get; set; }
        public ReconcilationType transactionType { get; set; }
        public ReconcilationTransaction reconcilationTransaction { get; set; }

    }
}
