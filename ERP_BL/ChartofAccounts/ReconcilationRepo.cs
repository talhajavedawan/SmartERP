using ERP_BL.Databases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.ChartofAccounts
{
   public class ReconcilationRepo
    {
        DBContextERP context = new DBContextERP();

        public void AddReconcilation(Reconcilation reconcilation)
        {
            context.reconcilations.Add(reconcilation);
            context.SaveChanges();
        }
        public double GetReconcilationAmount(int _chartofAccountId, DateTime datePeriod)
        {
            var Reconcilation = context.reconcilations.FirstOrDefault(x => x.chartofAccountId == _chartofAccountId && x.reconcilationDate == datePeriod);
            return Reconcilation.reconcilationAmount;
        }
    }
}
