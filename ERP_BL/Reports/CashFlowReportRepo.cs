using ERP_BL.Databases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.Reports
{
  public  class CashFlowReportRepo
    {
        DBContextERP context = new DBContextERP();
        public List< ERP_BL.Databases.Company> GetEmployeeForCashFlow(int empID)
        {
            //var employee = context.Employees.FirstOrDefault(x => x.EmpId == empID);
            return context.Companies.Where(x=>x.employees.FirstOrDefault(y=>y.EmpId==empID)!=null).ToList();
        }
        public void AddCashFlow(ERP_BL.CashFlow.CashFlow cashFlow)
        {
            context.cashFlows.Add(cashFlow);
            context.SaveChanges();
        }
        public void Update(ERP_BL.CashFlow.CashFlow cashFlow)
        {
            var dbReport = context.cashFlows.FirstOrDefault(x => x.Id == cashFlow.Id);
            dbReport = cashFlow;
            context.SaveChanges();
        }
    }
}
