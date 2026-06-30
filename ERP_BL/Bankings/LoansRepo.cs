using ERP_BL.Databases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.Bankings
{
    //comment
    public class LoansRepo
    {
        DBContextERP context = new DBContextERP();


        public void AddLoans(List<Loans> loans)
        {
            context.loans.AddRange(loans);
            context.SaveChanges();
        }

        public void Update(List<Loans> loans, List<Loans> removedLoans)
        {
            if (removedLoans != null)
            {
                foreach (var _loan in removedLoans)
                {
                    var loan = context.loans.FirstOrDefault(x => x.Id == _loan.Id);
                    context.loans.Remove(loan);
                }
            }
            foreach (var _loan in loans)
            {
                if (_loan.Id > 0)
                {
                    var loansToUpdate = context.loans.FirstOrDefault(x => x.Id == _loan.Id);
                    loansToUpdate = _loan;
                }
                else
                    context.loans.Add(_loan);

            }

            context.SaveChanges();
        }

        public void UpdateLoans(List<Loans> loans)
        {
            foreach(var _loan in loans)
            {
                if (_loan.Id > 0)
                {
                    var loansToUpdate = context.loans.FirstOrDefault(x => x.Id == _loan.Id);
                    loansToUpdate = _loan;
                }
                //else
                //    context.loans.Add(_loan);
                
            }
            
            context.SaveChanges();
        }

        public void SetLoansToVoid(List<Loans> loans, bool isVoid)
        {
            foreach (var _loan in loans)
            {
                if (_loan.Id > 0)
                {
                    var loansToUpdate = context.loans.FirstOrDefault(x => x.Id == _loan.Id);
                    loansToUpdate = _loan;
                    loansToUpdate.isVoid = isVoid;
                }
            }

            context.SaveChanges();
        }

        public List<Loans> GetAllLoansByGroupId(int groupId)
        {
            return context.loans

                .Where(x=>x.transactionGroupId == groupId)
                .ToList();
        }

        
        public List<Loans> GetAllLoansOpenAndClosed(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);

            return context.loans

                .Where(x => deptIds.Contains(x.department.Id) && companyIds.Contains(x.company.Id) && x.isApproved == true && x.isReApproved != false && x.PendingForClosing != true && x.isVoid != true)
                .ToList();
        }

        /// <summary>
        /// Get all active Loans Transfer except pending for closing.
        /// </summary>
        /// <returns></returns>
        public List<Loans> getAllActiveandUnapprovedLoans(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);

            return context.loans

                .Where(x => deptIds.Contains(x.department.Id) && companyIds.Contains(x.company.Id) && x.Status.isActive == true && x.isApproved == true && x.isReApproved != false && x.PendingForClosing != true && x.isVoid != true)
                .ToList();
        }

        /// <summary>
        /// Get all Inactive Loans except pending for closing.
        /// </summary>
        /// <returns></returns>
        public List<Loans> getAllInActiveandUnapprovedLoans(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);

            return context.loans

                .Where(x => deptIds.Contains(x.department.Id) && companyIds.Contains(x.company.Id) && x.Status.isActive != true && x.isReApproved != false && x.isApproved == true && x.PendingForClosing != true && x.isVoid != true)
                //.Where(x => x.PendingForClosing == false)
                .ToList();
        }

        /// <summary>
        /// Get all Loans  by <paramref name="StatusId"/>.
        /// </summary>
        /// <returns></returns>
        public List<Loans> getAllLoansbyStatusId(int uid, int StatusId)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.loans
   
                .Where(x => deptIds.Contains(x.department.Id) && companyIds.Contains(x.company.Id) && x.Status.Id == StatusId && x.isApproved == true && x.isReApproved != false && x.isVoid != true && x.PendingForClosing != true)
                .ToList();
        }


        /// <summary>
        /// Get all pending for closing Loans by Departmental
        /// </summary>
        /// <returns></returns>
        public List<Loans> getAllPendingForClosingDepartmental(int uid)
        {
            var user = context.Users.Include("Employee").FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.loans

                .Where(x => deptIds.Contains(x.department.Id) && companyIds.Contains(x.company.Id) && x.PendingForClosing == true && x.isVoid != true)
                .ToList();
        }

        /// <summary>
        /// Loans Pending for approval
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public List<Loans> getAllPendingForApprovalDepartmental(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.loans

                .Where(x => deptIds.Contains(x.department.Id) && companyIds.Contains(x.company.Id) && x.isApproved == false && x.isVoid != true)
                .ToList();
        }

        /// <summary>
        /// Get all active Loans except pending for closing.
        /// </summary>
        /// <returns></returns>
        public List<Loans> getAllActiveandUnapprovedTransactions(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);

            return context.loans
 
                .Where(x => deptIds.Contains(x.department.Id) && companyIds.Contains(x.company.Id) && x.Status.isActive == true && x.isApproved == true && x.isReApproved != false && x.PendingForClosing != true && x.isVoid != true)
                .ToList();
        }


        /// <summary>
        /// Get all Loans
        /// </summary>
        /// <returns></returns>
        public List<Loans> getInterBankTransferRegister(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.loans

                .Where(x => deptIds.Contains(x.department.Id) && companyIds.Contains(x.company.Id) && x.isVoid != true)
                .ToList();
        }

        /// <summary>
        /// Get all Loans.
        /// </summary>
        /// <returns></returns>
        public List<Loans> getInterBankTransferRegisterAdministrator()
        {
            return context.loans    

                .Where(x => x.isVoid != true)
                .ToList();
        }


        //#Void
        /// <summary>
        /// Get all Void Loans for user.
        /// </summary>
        /// <returns></returns>
        public List<Loans> getVoidRegister(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.loans

                .Where(x => deptIds.Contains(x.department.Id) && companyIds.Contains((int)x.company.Id) && x.isVoid == true)
                .ToList();
        }


        /// <summary>
        /// Get all Void Inter-Bank Transfer own.
        /// </summary>
        /// <returns></returns>
        public List<Loans> getVoidRegisterOwn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();

            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);

            return context.loans

                .Where(x => deptIds.Contains(x.department.Id) && companyIds.Contains((int)x.company.Id) && x.isVoid == true && (x.user.employee.EmpId == user.employee.EmpId))
                .ToList();
        }


        /// <summary>
        /// Get all Void Loans
        /// </summary>
        /// <returns></returns>
        public List<Loans> getVoidRegisterAdministrator()
        {
            return context.loans

                .Where(x => x.isVoid == true)
                .ToList();
        }



        public List<Loans> GetAllLoans(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);

            return context.loans

            .Where(x => deptIds.Contains(x.department.Id) && companyIds.Contains(x.company.Id) && x.Status.isActive == true && x.isApproved == true && x.isReApproved != false && x.PendingForClosing != true && x.isVoid != true)

                .ToList();
        }


        /// <summary>
        /// Loans Pending for re-approval
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public List<Loans> getAllPendingForReApprovalDepartmental(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.loans

                .Where(x => deptIds.Contains(x.department.Id) && companyIds.Contains(x.company.Id) && x.isReApproved == false && x.isApproved == true && x.isVoid != true)
                .ToList();
        }


        /// <summary>
        /// Get Count pending Loans by Departments
        /// <returns></returns>
        public int getAllPendingForApprovalDepartmentalCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.AdminBillCompanies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            
            return context.loans
            .Where(x => (deptIds.Contains(x.department.Id) || x.user_Id == uid) && companyIds.Contains((int)x.CompanyId) && x.isApproved == false && x.isVoid != true)
            .Count();
        }


        /// <summary>
        /// Get Count pending Loans by supervisor Id and <paramref name="StatusId"/>.
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForApprovalCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.AdminBillCompanies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            
            return context.loans
            .Where(x => (deptIds.Contains(x.department.Id) || x.user_Id == uid) && companyIds.Contains((int)x.CompanyId) && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true)
            .Count();            
        }


        /// <summary>
        /// Get Own Count pending Loans
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForApprovalCountOwn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.AdminBillCompanies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.loans
           .Where(x => (deptIds.Contains(x.department.Id) || x.user_Id == uid) && companyIds.Contains((int)x.CompanyId) && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true)
           .Count();
        }


        /// <summary>
        /// Get Count pending Loans by Departments
        /// <returns></returns>
        public int getAllPendingForReApprovalDepartmentalCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.AdminBillCompanies)
                companyIds.Add(comp.Id);
            return context.loans
            .Where(x => (deptIds.Contains(x.department.Id) || x.user_Id == uid) && companyIds.Contains((int)x.CompanyId) && x.isApproved == true && x.isReApproved == false && x.isVoid != true)
            .Count();
        }


        /// <summary>
        /// Get Count pending Loans by user Id and <paramref name="UserID"/>.
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForReApprovalCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.AdminBillCompanies)
                companyIds.Add(comp.Id);
            return context.loans
            .Where(x => (deptIds.Contains(x.department.Id) || x.user_Id == uid) && companyIds.Contains((int)x.CompanyId) && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId) && x.isReApproved == false && x.isApproved == true && x.isVoid != true)
            .Count();
        }


        /// <summary>
        /// Get Own Count pending Loans user Id and <paramref name="UserID"/>.
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForReApprovalCountOwn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.AdminBillCompanies)
                companyIds.Add(comp.Id);

            return context.loans
            .Where(x => (deptIds.Contains(x.department.Id) || x.user_Id == uid) && companyIds.Contains((int)x.CompanyId) && (x.user.employee.EmpId == user.employee.EmpId) && x.isReApproved == false && x.isVoid != true && x.isApproved == true)
            .Count();
        }

        /// <summary>
        /// Get all Void Loans for user count. 
        /// </summary>
        /// <returns></returns>
        public int getVoidRegisterCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();

            foreach (var comp in user.employee.AdminBillCompanies)
                companyIds.Add(comp.Id);

            return context.loans
            .Where(x => (deptIds.Contains(x.department.Id) || x.user_Id == uid) && companyIds.Contains((int)x.CompanyId) && x.isVoid == true)
            .Count();
        }

        /// <summary>
        /// Get all Void Loans
        /// </summary>
        /// <returns></returns>
        public int getVoidRegisterAdministratorCount()
        {
            return context.loans
    .Where(x => x.isVoid == true)
    .Count();
        }

        /// <summary>
        /// Get all Loans for user count. 
        /// </summary>
        /// <returns></returns>
        public int getBankTransferRegisterCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.AdminBillCompanies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.loans
            .Where(x => (deptIds.Contains(x.department.Id) || x.user_Id == uid) && companyIds.Contains((int)x.CompanyId) && x.isVoid != true)
            .Count();
        }

        /// Get Count Loans
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForAdministratorCount()
        {
            return context.loans

                .Where(x => x.isApproved == false && x.isVoid != true)
                .Count();
        }

        /// <summary>
        /// Get all Loans
        /// </summary>
        /// <returns></returns>
        public int getInterBankTransferAdministratorCount()
        {
            return context.loans
                .Where(x => x.isVoid != true)
                .Count();
        }

        /// <summary>
        /// Get all pending Loans by Departmental Count
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForClosingDepartmentalCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.AdminBillCompanies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.loans

           .Where(x => (deptIds.Contains(x.department.Id) || x.user_Id == uid) && companyIds.Contains((int)x.CompanyId) && x.isApproved != false && x.PendingForClosing == true && x.isVoid != true)
           .Count();
        }

        /// <summary>
        /// Get Count pending for closing Loans by supervisor Id and <paramref name="StatusId"/>.
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForClosingCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.AdminBillCompanies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.loans
            .Where(x => (deptIds.Contains(x.department.Id) || x.user_Id == uid) && companyIds.Contains((int)x.CompanyId) && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
            .Count();
            
        }


        /// <summary>
        /// Get Count pending for closing Loans own <paramref name="StatusId"/>.
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForClosingCountOwn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.AdminBillCompanies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

           
            return context.loans
           
            .Where(x => (deptIds.Contains(x.department.Id) || x.user_Id == uid) && companyIds.Contains((int)x.CompanyId) && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
            .Count();
        }

        /// <summary>
        /// Get Count Loans
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForClosingAdministratorCount()
        {
            return context.loans

                .Where(x => x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .Count();
        }

        /// <summary>
        /// Get last Loan
        /// </summary>
        /// <returns></returns>
        public int GetLastPaymentId()
        {
            var month = DateTime.Now.Month.ToString();
            if (month.Length == 1)
            {
                month = "0" + month;
            }
            var currentMonthYear = DateTime.Now.Year.ToString() + month.ToString();

            //var lastId = context.adminBills.Max(x => x.transactionGroupId);
            var listOfLoans = context.loans.Where(x => x.SystemRefNo.Contains(currentMonthYear)).ToList();
            if (listOfLoans.Count > 0)
            {
                var lastId = listOfLoans.Max(x => x.transactionGroupId);
                return lastId;
            }
            else
                return 0;
        }

        public void AddFacilityNature(FacilityNature facilityNature)
        {
            context.facilityNatures.Add(facilityNature);
            context.SaveChanges();
        }

        public void UpdateFacilityNature(FacilityNature facilityNature)
        {
            var _facilityNature = context.facilityNatures.FirstOrDefault(x => x.Id == facilityNature.Id);
            _facilityNature.NatureName = facilityNature.NatureName;
            _facilityNature.isActive = facilityNature.isActive;
            //context.facilityNatures.Add(_facilityNature);
            context.SaveChanges();
        }

        public FacilityNature GetFacilityNature(int natureId)
        {
            return context.facilityNatures
                .FirstOrDefault(x=>x.Id == natureId);
        }

        public List< FacilityNature> GetAllFacilityNatures()
        {
            return context.facilityNatures.ToList();
        }

        /// <summary>
        /// Get All Loans Statuses
        /// </summary>
        /// <returns></returns>
        public List<LoansStatus> GetAllLoansStatuses()
        {
            return context.loansStatuses
                .ToList();
        }

        /// <summary>
        /// Get A Loans Status
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns></returns>
        public LoansStatus GetLoansStatus(int statusId)
        {
            return context.loansStatuses
                .FirstOrDefault(x => x.Id == statusId);
        }

        /// <summary>
        /// Add New Loans Status
        /// </summary>
        /// <param name="loansStatus"></param>
        public void AddLoansStatus(LoansStatus loansStatus)
        {
            context.loansStatuses.Add(loansStatus);
            context.SaveChanges();
        }

        /// <summary>
        /// Update Existing Loans Status
        /// </summary>
        /// <param name="loansStatus"></param>
        public void UpdateLoansStatus(LoansStatus loansStatus)
        {
            LoansStatus _loansStatus = context.loansStatuses.FirstOrDefault(x => x.Id == loansStatus.Id);
            _loansStatus = loansStatus;
            context.SaveChanges();
        }


        public List<LoansStatus> GetAllOpenLoansStatus()
        {
            var statusList = context.loansStatuses.Where(x => x.isActive == true)
                .ToList();
            return statusList;

        }

        public List<LoansStatus> GetAllCloseLoansStatus()
        {
            var statusList = context.loansStatuses.Where(x => x.isActive == false)
                .ToList();
            return statusList;

        }
    }
}
