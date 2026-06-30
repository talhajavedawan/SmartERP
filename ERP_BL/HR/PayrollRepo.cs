using ERP_BL.Databases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.HR
{
    public class PayrollRepo
    {
        DBContextERP context = new DBContextERP();

        /// <summary>
        /// Add Employment Salary
        /// </summary>
        /// <param name="employmentSalary"></param>
        public void AddEmploymentSalary(EmploymentSalary employmentSalary)
        {
            context.employmentSalaries.Add(employmentSalary);
            context.SaveChanges();
        }

        /// <summary>
        /// Update Employment Salary
        /// </summary>
        /// <param name="employmentSalary"></param>
        public void UpdateEmploymentSalary(EmploymentSalary employmentSalary)
        {
            var _employmentSalary = context.employmentSalaries.FirstOrDefault(x=>x.Id == employmentSalary.Id);
            _employmentSalary = employmentSalary;
            context.SaveChanges();
        }

        /// <summary>
        /// Get Employment Salary By Id
        /// </summary>
        /// <param name="empSalaryId"></param>
        /// <returns></returns>
        public EmploymentSalary GetEmploymentSalary(int empSalaryId)
        {
            return context.employmentSalaries

                .FirstOrDefault(x=>x.Id == empSalaryId);
        }

        /// <summary>
        /// Get All Employment Salaries
        /// </summary>
        /// <param name="empSalaryId"></param>
        /// <returns></returns>
        public List<EmploymentSalary> GetAllEmploymentSalaries()
        {
            return context.employmentSalaries
    
                .ToList();
        }

        /// <summary>
        /// Get All Active Employment Salaries
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public List<EmploymentSalary> GetAllActiveEmploymentSalaries()
        {
            return context.employmentSalaries
    
                .Where(x=>x.isActive == true)
                .ToList();
        }

        /// <summary>
        /// Get last EmploymentSalary
        /// </summary>
        /// <returns></returns>
        public int GetLastTransactionId()
        {
            var payment = context.employmentSalaries.OrderByDescending(q => q.Id).FirstOrDefault();
            if (payment == null)
                return 0;
            return payment.transactionGroupId;
        }
    }
}
