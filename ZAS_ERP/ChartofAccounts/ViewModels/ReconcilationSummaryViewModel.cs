using ERP_BL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZAS_ERP.ChartofAccounts.ViewModels
{
   public class ReconcilationSummaryViewModel
    {
        public ReconcilationType transactionType { get; set; }
        public ReconcilationTransaction reconcilationTransaction { get; set; }
        public double amount { get; set; }
        public double balance { get; set; }
    }
}
