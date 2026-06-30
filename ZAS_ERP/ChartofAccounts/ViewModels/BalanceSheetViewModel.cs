using ERP_BL.ChartofAccounts;
using ERP_BL.Databases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZAS_ERP.ChartofAccounts.ViewModels
{
  public  class BalanceSheetViewModel
    {
        public int Id { get; set; }
        public ChartofAccount Account { get; set; }
        public string Companies { get; set; }
        public string Departments { get; set; }
        public double Balance { get; set; }
        public double BalanceOC { get; set; }
    }
}
