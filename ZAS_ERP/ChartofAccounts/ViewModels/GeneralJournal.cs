using ERP_BL.ChartofAccounts;
using ERP_BL.Databases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZAS_ERP.ChartofAccounts.ViewModels
{
   public class GeneralJournal
    {
        public DateTime ?PostingDate { get; set; }
        public Company company { get; set; }
        public Department department { get; set; }
        public string transactionRefno { get; set; }
        public ChartofAccount chartofAccount { get; set; }
        public string memo { get; set; }
        public double debitPKR { get; set; }
        public double MER { get; set; }
        public double creditPKR { get; set; }
        public double balancePKR { get; set; }
        public Currency currency { get; set; }
        public double debit { get; set; }
        public double credit { get; set; }
        public double balanceOC { get; set; }
    }
}
