using ERP_BL.Databases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.Procurements.LoansAdvances
{
    public class CompanyLoansRepo
    {
        DBContextERP context = new DBContextERP();


        /// <summary>
        /// Add Loans Advances
        /// </summary>
        /// <param name="loansAdvance"></param>
        public void AddLoansAdvance(LoansAdvance loansAdvance)
        {
            context.loansAdvances.Add(loansAdvance);
            context.SaveChanges();
        }

        /// <summary>
        /// Update Existing Loans Advance
        /// </summary>
        /// <param name="loansAdvance"></param>
        public void UpdateLoansAdvance(LoansAdvance loansAdvance)
        {
            var _loansAdvance = context.loansAdvances.FirstOrDefault(x => x.Id == loansAdvance.Id);
            _loansAdvance = loansAdvance;
            if (_loansAdvance.pettyCashes != null)
            {
                var alltransactions = context.pettyCashes.Where(x => x.LoansAdvanceId == _loansAdvance.Id).ToList();
                context.pettyCashes.RemoveRange(alltransactions);
            }
            _loansAdvance.pettyCashes = loansAdvance.pettyCashes;
            context.SaveChanges();
        }

        /// <summary>
        /// Get Loans Advance
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        public LoansAdvance GetLoansAdvance(int loansAdvanceId)
        {
            return context.loansAdvances
                .Include("company")
                .Include("department")
                .FirstOrDefault(x => x.Id == loansAdvanceId);
        }

        /// <summary>
        /// Get All Loans and Advances
        /// </summary>
        /// <returns></returns>
        public List<LoansAdvance> GetAllLoansAdvances(int uid)
        {

            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            List<int> adminCompanyIds = new List<int>();
            foreach (var comp in user.employee.AdminBillCompanies)
                adminCompanyIds.Add(comp.Id);

            return context.loansAdvances

           .Where(x => (deptIds.Contains(x.department.Id) || x.creatorId == uid) && ((companyIds.Contains(x.company.Id) && x.loansAdvanceType == Enums.LoansAdvanceType.Vendor_Bill) || (adminCompanyIds.Contains(x.company.Id) && x.loansAdvanceType == Enums.LoansAdvanceType.Admin_Bill)) && x.isVoid != true && x.advanceTemplate == Enums.LoansAdvanceTemplate.Loan)
           .ToList();


        }
        public List<LoansAdvance> getCashFlowAllActive(int uid)
        {

            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);

            List<int> adminCompanyIds = new List<int>();
            foreach (var comp in user.employee.AdminBillCompanies)
                adminCompanyIds.Add(comp.Id);

            return context.loansAdvances

           .Where(x => (deptIds.Contains(x.department.Id) 
           || x.creatorId == uid) && ((companyIds.Contains(x.company.Id) && x.loansAdvanceType == Enums.LoansAdvanceType.Vendor_Bill)
           || (adminCompanyIds.Contains(x.company.Id) && deptIds.Contains(x.department.Id) &&
           x.loansAdvanceType == Enums.LoansAdvanceType.Admin_Bill)) &&
           x.Status.isActive == true && x.isReApproved != false && x.isApproved == true &&
           x.PendingForClosing != true && x.isVoid != true &&
           x.advanceTemplate == Enums.LoansAdvanceTemplate.Loan)
           .ToList();


        }

        /// <summary>
        /// Get All Loans and Advances
        /// </summary>
        /// <returns></returns>
        public List<LoansAdvance> GetAllActiveLoansAdvances(int uid)
        {

            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            List<int> adminCompanyIds = new List<int>();
            foreach (var comp in user.employee.AdminBillCompanies)
                adminCompanyIds.Add(comp.Id);

            return context.loansAdvances

           .Where(x => (deptIds.Contains(x.department.Id) || x.creatorId == uid) && ((companyIds.Contains(x.company.Id) && x.loansAdvanceType == Enums.LoansAdvanceType.Vendor_Bill) || (adminCompanyIds.Contains(x.company.Id) && x.loansAdvanceType == Enums.LoansAdvanceType.Admin_Bill)) && x.Status.isActive == true && x.isReApproved != false && x.isApproved == true && x.PendingForClosing != true && x.isVoid != true && x.advanceTemplate == Enums.LoansAdvanceTemplate.Loan)
           .ToList();


        }


        /// <summary>
        /// Get All Loans and Advances
        /// </summary>
        /// <returns></returns>
        public List<LoansAdvance> GetAllClosedLoansAdvances(int uid)
        {

            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            List<int> adminCompanyIds = new List<int>();
            foreach (var comp in user.employee.AdminBillCompanies)
                adminCompanyIds.Add(comp.Id);


            return context.loansAdvances

           .Where(x => (deptIds.Contains(x.department.Id) || x.creatorId == uid) && ((companyIds.Contains(x.company.Id) && x.loansAdvanceType == Enums.LoansAdvanceType.Vendor_Bill) || (adminCompanyIds.Contains(x.company.Id) && x.loansAdvanceType == Enums.LoansAdvanceType.Admin_Bill)) && x.Status.isActive == true && x.isReApproved != false && x.isApproved == true && x.PendingForClosing != true && x.isVoid != true && x.advanceTemplate == Enums.LoansAdvanceTemplate.Loan)
           .ToList();


        }


        /// <summary>
        /// Get All Loans and Advances
        /// </summary>
        /// <returns></returns>
        public List<LoansAdvance> GetAllLoansAdvancesByStatusId(int uid, int statusId)
        {

            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            List<int> adminCompanyIds = new List<int>();
            foreach (var comp in user.employee.AdminBillCompanies)
                adminCompanyIds.Add(comp.Id);

            return context.loansAdvances

           .Where(x => (deptIds.Contains(x.department.Id) || x.creatorId == uid) && ((companyIds.Contains(x.company.Id) && x.loansAdvanceType == Enums.LoansAdvanceType.Vendor_Bill) || (adminCompanyIds.Contains(x.company.Id) && x.loansAdvanceType == Enums.LoansAdvanceType.Admin_Bill)) && x.Status.Id == statusId && x.isReApproved != false && x.isApproved == true && x.PendingForClosing != true && x.isVoid != true && x.advanceTemplate == Enums.LoansAdvanceTemplate.Loan)
           .ToList();


        }


        /// <summary>
        /// Get All Pending For Approval Loans Advances
        /// </summary>
        /// <returns></returns>
        public List<LoansAdvance> GetAllPendingForApproval(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> adminCompanyIds = new List<int>();
            foreach (var comp in user.employee.AdminBillCompanies)
                adminCompanyIds.Add(comp.Id);

            return context.loansAdvances

        .Where(x => (deptIds.Contains(x.department.Id) || x.creatorId == uid) && ((companyIds.Contains(x.company.Id) && x.loansAdvanceType == Enums.LoansAdvanceType.Vendor_Bill) || (adminCompanyIds.Contains(x.company.Id) && x.loansAdvanceType == Enums.LoansAdvanceType.Admin_Bill)) && x.isApproved == false && x.isVoid != true && x.advanceTemplate == Enums.LoansAdvanceTemplate.Loan)
         .ToList();
        }

        /// <summary>
        /// Admin Bills Pending for re-approval
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public List<LoansAdvance> getAllPendingForReApprovalDepartmental(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            List<int> adminCompanyIds = new List<int>();
            foreach (var comp in user.employee.AdminBillCompanies)
                adminCompanyIds.Add(comp.Id);

            return context.loansAdvances

         .Where(x => (deptIds.Contains(x.department.Id) || x.creatorId == uid) && ((companyIds.Contains(x.company.Id) && x.loansAdvanceType == Enums.LoansAdvanceType.Vendor_Bill) || (adminCompanyIds.Contains(x.company.Id) && x.loansAdvanceType == Enums.LoansAdvanceType.Admin_Bill)) && x.isReApproved == false && x.isApproved == true && x.isVoid != true && x.advanceTemplate == Enums.LoansAdvanceTemplate.Loan)
             .ToList();

        }


        /// <summary>
        /// Get all pending for closing Loans and Advances by Departmental
        /// </summary>
        /// <returns></returns>
        public List<LoansAdvance> getAllPendingForClosingDepartmental(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> adminCompanyIds = new List<int>();
            foreach (var comp in user.employee.AdminBillCompanies)
                adminCompanyIds.Add(comp.Id);

            return context.loansAdvances
            .Where(x => (deptIds.Contains(x.department.Id) || x.creatorId == uid) && ((companyIds.Contains(x.company.Id) && x.loansAdvanceType == Enums.LoansAdvanceType.Vendor_Bill) || (adminCompanyIds.Contains(x.company.Id) && x.loansAdvanceType == Enums.LoansAdvanceType.Admin_Bill)) && x.PendingForClosing == true && x.isVoid != true && x.advanceTemplate == Enums.LoansAdvanceTemplate.Loan)
            .ToList();
        }


        /// <summary>
        /// Get all Void Inter-Bank Transfer own.
        /// </summary>
        /// <returns></returns>
        public List<LoansAdvance> getVoidRegisterOwn(int uid)
        {

            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();

            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            List<int> adminCompanyIds = new List<int>();
            foreach (var comp in user.employee.AdminBillCompanies)
                adminCompanyIds.Add(comp.Id);

            return context.loansAdvances
         .Where(x => (deptIds.Contains(x.department.Id) || x.creatorId == uid) && ((companyIds.Contains(x.company.Id) && x.loansAdvanceType == Enums.LoansAdvanceType.Vendor_Bill) || (adminCompanyIds.Contains(x.company.Id) && x.loansAdvanceType == Enums.LoansAdvanceType.Admin_Bill)) && x.isVoid == true && x.advanceTemplate == Enums.LoansAdvanceTemplate.Loan)
             .ToList();


        }


        /// <summary>
        /// Get Purchase Invoices by Company, Dept and Currency
        /// </summary>
        /// <returns></returns>
        public List<LoansAdvance> getLoansAdvancebyCompanyDept(int compId, List<Department> departments, int currId)
        {
            List<int> dept_Ids = new List<int>();
            foreach (var _dept in departments)
            {
                dept_Ids.Add(_dept.Id);
            }

            var loansAdvances = context.loansAdvances
           .Where(x => x.companyId == compId && x.currencyId == currId && x.Status.isActive == true && x.isApproved == true && x.isVoid != true && dept_Ids.Contains((int)x.deptId) && x.advanceTemplate == Enums.LoansAdvanceTemplate.Loan)
           .ToList();

            //List<LoansAdvance> LAToReturn = new List<LoansAdvance>();
            //foreach (var _LA in loansAdvances)
            //{
            //    if (dept_Ids.Contains((int)_LA.deptId))
            //    {
            //        LAToReturn.Add(_LA);
            //    }
            //}

            return loansAdvances;
        }


        /// <summary>
        /// Get Purchase Invoices by Company, Dept and Currency
        /// </summary>
        /// <returns></returns>
        public List<LoansAdvance> getLoansAdvanceForSR(int compId, int deptId, int currId)
        {
            return context.loansAdvances
           .Where(x => x.companyId == compId && x.currencyId == currId && x.Status.isActive == true && x.isApproved == true && x.isVoid != true && x.deptId == deptId && x.advanceTemplate == Enums.LoansAdvanceTemplate.Loan)
           .ToList();
        }


        /// <summary>
        /// Get Count pending Inter-Bank Transfers by Departments
        /// <returns></returns>
        public int getAllPendingForApprovalDepartmentalCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> adminCompanyIds = new List<int>();
            foreach (var comp in user.employee.AdminBillCompanies)
                adminCompanyIds.Add(comp.Id);

            return context.loansAdvances

                .Where(x => (deptIds.Contains(x.department.Id) || x.creatorId == uid) && ((companyIds.Contains(x.company.Id) && x.loansAdvanceType == Enums.LoansAdvanceType.Vendor_Bill) || (adminCompanyIds.Contains(x.company.Id) && x.loansAdvanceType == Enums.LoansAdvanceType.Admin_Bill)) && x.isApproved == false && x.isVoid != true && x.advanceTemplate == Enums.LoansAdvanceTemplate.Loan)
                .Count();
        }


        /// <summary>
        /// Get Count pending Inter-Bank Transfers by supervisor Id and <paramref name="StatusId"/>.
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForApprovalCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> adminCompanyIds = new List<int>();
            foreach (var comp in user.employee.AdminBillCompanies)
                adminCompanyIds.Add(comp.Id);

            return context.loansAdvances

                .Where(x => (deptIds.Contains(x.department.Id) || x.creatorId == uid) && ((companyIds.Contains(x.company.Id) && x.loansAdvanceType == Enums.LoansAdvanceType.Vendor_Bill) || (adminCompanyIds.Contains(x.company.Id) && x.loansAdvanceType == Enums.LoansAdvanceType.Admin_Bill)) && (x.Creator.employee.SupervisorId == user.employee.EmpId || x.Creator.employee.EmpId == user.employee.EmpId || x.Creator.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId || x.Creator.employee.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true && x.advanceTemplate == Enums.LoansAdvanceTemplate.Loan)
                .Count();
        }

        /// <summary>
        /// Get Own Count pending Inter-Bank Transfers
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForApprovalCountOwn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> adminCompanyIds = new List<int>();
            foreach (var comp in user.employee.AdminBillCompanies)
                adminCompanyIds.Add(comp.Id);

            return context.loansAdvances

                .Where(x => (deptIds.Contains(x.department.Id) || x.creatorId == uid) && ((companyIds.Contains(x.company.Id) && x.loansAdvanceType == Enums.LoansAdvanceType.Vendor_Bill) || (adminCompanyIds.Contains(x.company.Id) && x.loansAdvanceType == Enums.LoansAdvanceType.Admin_Bill)) && (x.Creator.employee.EmpId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true && x.advanceTemplate == Enums.LoansAdvanceTemplate.Loan)
                .Count();
        }



        /// <summary>
        /// Get Count pending Inter-Bank Transfers by Departments
        /// <returns></returns>
        public int getAllPendingForReApprovalDepartmentalCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            List<int> adminCompanyIds = new List<int>();
            foreach (var comp in user.employee.AdminBillCompanies)
                adminCompanyIds.Add(comp.Id);

            return context.loansAdvances

                .Where(x => (deptIds.Contains(x.department.Id) || x.creatorId == uid) && ((companyIds.Contains(x.company.Id) && x.loansAdvanceType == Enums.LoansAdvanceType.Vendor_Bill) || (adminCompanyIds.Contains(x.company.Id) && x.loansAdvanceType == Enums.LoansAdvanceType.Admin_Bill)) && x.isApproved == true && x.isReApproved == false && x.isVoid != true && x.advanceTemplate == Enums.LoansAdvanceTemplate.Loan)
                .Count();
        }


        /// <summary>
        /// Get Count pending Inter-Bank Transfers by user Id and <paramref name="UserID"/>.
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForReApprovalCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            List<int> adminCompanyIds = new List<int>();
            foreach (var comp in user.employee.AdminBillCompanies)
                adminCompanyIds.Add(comp.Id);

            return context.loansAdvances

                .Where(x => (deptIds.Contains(x.department.Id) || x.creatorId == uid) && ((companyIds.Contains(x.company.Id) && x.loansAdvanceType == Enums.LoansAdvanceType.Vendor_Bill) || (adminCompanyIds.Contains(x.company.Id) && x.loansAdvanceType == Enums.LoansAdvanceType.Admin_Bill)) && (x.Creator.employee.SupervisorId == user.employee.EmpId || x.Creator.employee.EmpId == user.employee.EmpId || x.Creator.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId || x.Creator.employee.Supervisor.SupervisorId == user.employee.EmpId) && x.isReApproved == false && x.isApproved == true && x.isVoid != true && x.advanceTemplate == Enums.LoansAdvanceTemplate.Loan)
                .Count();
        }


        /// <summary>
        /// Get Own Count pending Inter-Bank Transfers user Id and <paramref name="UserID"/>.
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForReApprovalCountOwn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            List<int> adminCompanyIds = new List<int>();
            foreach (var comp in user.employee.AdminBillCompanies)
                adminCompanyIds.Add(comp.Id);

            return context.loansAdvances

                .Where(x => (deptIds.Contains(x.department.Id) || x.creatorId == uid) && ((companyIds.Contains(x.company.Id) && x.loansAdvanceType == Enums.LoansAdvanceType.Vendor_Bill) || (adminCompanyIds.Contains(x.company.Id) && x.loansAdvanceType == Enums.LoansAdvanceType.Admin_Bill)) && (x.Creator.employee.EmpId == user.employee.EmpId) && x.isReApproved == false && x.isVoid != true && x.isApproved == true && x.advanceTemplate == Enums.LoansAdvanceTemplate.Loan)
                .Count();
        }

        /// <summary>
        /// Get all Void Inter-Bank Transfer for user count. 
        /// </summary>
        /// <returns></returns>
        public int getVoidRegisterCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();

            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            List<int> adminCompanyIds = new List<int>();
            foreach (var comp in user.employee.AdminBillCompanies)
                adminCompanyIds.Add(comp.Id);


            return context.loansAdvances
                .Where(x => (deptIds.Contains(x.department.Id) || x.creatorId == uid) && ((companyIds.Contains(x.company.Id) && x.loansAdvanceType == Enums.LoansAdvanceType.Vendor_Bill) || (adminCompanyIds.Contains(x.company.Id) && x.loansAdvanceType == Enums.LoansAdvanceType.Admin_Bill)) && x.isVoid == true && x.advanceTemplate == Enums.LoansAdvanceTemplate.Loan)
                .Count();
        }


        /// <summary>
        /// Get all Void Inter-Bank Transfers.
        /// </summary>
        /// <returns></returns>
        public int getVoidRegisterAdministratorCount()
        {
            return context.loansAdvances
    .Where(x => x.isVoid == true && x.advanceTemplate == Enums.LoansAdvanceTemplate.Loan)
    .Count();
        }


        /// <summary>
        /// Get all Inter-Bank Transfers for user count. 
        /// </summary>
        /// <returns></returns>
        public int getBankTransferRegisterCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> adminCompanyIds = new List<int>();
            foreach (var comp in user.employee.AdminBillCompanies)
                adminCompanyIds.Add(comp.Id);

            return context.loansAdvances
                .Where(x => (deptIds.Contains(x.department.Id) || x.creatorId == uid) && ((companyIds.Contains(x.company.Id) && x.loansAdvanceType == Enums.LoansAdvanceType.Vendor_Bill) || (adminCompanyIds.Contains(x.company.Id) && x.loansAdvanceType == Enums.LoansAdvanceType.Admin_Bill)) && x.isVoid != true && x.advanceTemplate == Enums.LoansAdvanceTemplate.Loan)
                .Count();
        }

        /// <summary>
        /// Get all Inter-Bank Transfer  own count. 
        /// </summary>
        /// <returns></returns>
        public int getBankTransferRegisterCountOWn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            List<int> adminCompanyIds = new List<int>();
            foreach (var comp in user.employee.AdminBillCompanies)
                adminCompanyIds.Add(comp.Id);

            return context.loansAdvances
                .Where(x => (deptIds.Contains(x.department.Id) || x.creatorId == uid) && ((companyIds.Contains(x.company.Id) && x.loansAdvanceType == Enums.LoansAdvanceType.Vendor_Bill) || (adminCompanyIds.Contains(x.company.Id) && x.loansAdvanceType == Enums.LoansAdvanceType.Admin_Bill)) && (x.Creator.employee.EmpId == user.employee.EmpId) && x.isVoid != true && x.advanceTemplate == Enums.LoansAdvanceTemplate.Loan)
                .Count();
        }

        /// Get Count Inter-Bank Transfer.
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForAdministratorCount()
        {
            return context.loansAdvances

                .Where(x => x.isApproved == false && x.isVoid != true && x.advanceTemplate == Enums.LoansAdvanceTemplate.Loan)
                .Count();
        }


        /// <summary>
        /// Get all Inter-Bank Transfer .
        /// </summary>
        /// <returns></returns>
        public int getInterBankTransferAdministratorCount()
        {
            return context.loansAdvances
                .Where(x => x.isVoid != true && x.advanceTemplate == Enums.LoansAdvanceTemplate.Loan)
                .Count();
        }

        /// <summary>
        /// Get all pending Inter-Bank Transfer by Departmental Count
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForClosingDepartmentalCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> adminCompanyIds = new List<int>();
            foreach (var comp in user.employee.AdminBillCompanies)
                adminCompanyIds.Add(comp.Id);

            return context.loansAdvances

                .Where(x => (deptIds.Contains(x.department.Id) || x.creatorId == uid) && ((companyIds.Contains(x.company.Id) && x.loansAdvanceType == Enums.LoansAdvanceType.Vendor_Bill) || (adminCompanyIds.Contains(x.company.Id) && x.loansAdvanceType == Enums.LoansAdvanceType.Admin_Bill)) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true && x.advanceTemplate == Enums.LoansAdvanceTemplate.Loan)
                .Count();
        }


        /// <summary>
        /// Get Count pending for closing Inter-Bank Transfer by supervisor Id and <paramref name="StatusId"/>.
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForClosingCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> adminCompanyIds = new List<int>();
            foreach (var comp in user.employee.AdminBillCompanies)
                adminCompanyIds.Add(comp.Id);

            return context.loansAdvances

                .Where(x => (deptIds.Contains(x.department.Id) || x.creatorId == uid) && ((companyIds.Contains(x.company.Id) && x.loansAdvanceType == Enums.LoansAdvanceType.Vendor_Bill) || (adminCompanyIds.Contains(x.company.Id) && x.loansAdvanceType == Enums.LoansAdvanceType.Admin_Bill)) && (x.Creator.employee.SupervisorId == user.employee.EmpId || x.Creator.employee.EmpId == user.employee.EmpId || x.Creator.employee.Supervisor.SupervisorId == user.employee.EmpId || x.Creator.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true && x.advanceTemplate == Enums.LoansAdvanceTemplate.Loan)
                .Count();
        }


        /// <summary>
        /// Get Count pending for closing Inter-Bank Transfers own <paramref name="StatusId"/>.
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForClosingCountOwn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> adminCompanyIds = new List<int>();
            foreach (var comp in user.employee.AdminBillCompanies)
                adminCompanyIds.Add(comp.Id);

            return context.loansAdvances

                .Where(x => (deptIds.Contains(x.department.Id) || x.creatorId == uid) && ((companyIds.Contains(x.company.Id) && x.loansAdvanceType == Enums.LoansAdvanceType.Vendor_Bill) || (adminCompanyIds.Contains(x.company.Id) && x.loansAdvanceType == Enums.LoansAdvanceType.Admin_Bill)) && (x.Creator.employee.EmpId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true && x.advanceTemplate == Enums.LoansAdvanceTemplate.Loan)
                .Count();
        }

        /// <summary>
        /// Get Count Inter-Bank Transfers.
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForClosingAdministratorCount()
        {
            return context.loansAdvances

                .Where(x => x.isApproved == true && x.PendingForClosing == true && x.isVoid != true && x.advanceTemplate == Enums.LoansAdvanceTemplate.Loan)
                .Count();
        }

        /// <summary>
        /// Get last Payment
        /// </summary>
        /// <returns></returns>
        public int GetLastTransactionId()
        {
            var loansAdvance = context.loansAdvances.OrderByDescending(q => q.Id).FirstOrDefault();
            return loansAdvance.transactionGroupId;
        }




        /// <summary>
        /// Add Applicant
        /// </summary>
        /// <param name="applicantType"></param>
        public void AddApplicant(LoanApplicant applicant)
        {
            var compIds = applicant.companies.Select(x => x.Id);
            var deptIds = applicant.departments.Select(x => x.Id);

            applicant.companies = new List<Company>();
            foreach (var _id in compIds)
            {
                applicant.companies.Add(context.Companies.FirstOrDefault(x => x.Id == _id));
            }

            applicant.departments = new List<Department>();
            foreach (var _id in deptIds)
            {
                applicant.departments.Add(context.Departments.FirstOrDefault(x => x.Id == _id));
            }

            context.loanApplicants.Add(applicant);
            context.SaveChanges();
        }

        /// <summary>
        /// Update Existing Applicant
        /// </summary>
        /// <param name="applicant"></param>
        public void UpdateApplicant(LoanApplicant applicant)
        {
            var compIds = applicant.companies.Select(x => x.Id);
            var deptIds = applicant.departments.Select(x => x.Id);


            var _applicantType = context.loanApplicants.FirstOrDefault(x => x.Id == applicant.Id);
            _applicantType = applicant;

            _applicantType.companies = new List<Company>();
            foreach (var _id in compIds)
            {
                _applicantType.companies.Add(context.Companies.FirstOrDefault(x => x.Id == _id));
            }

            _applicantType.departments = new List<Department>();
            foreach (var _id in deptIds)
            {
                _applicantType.departments.Add(context.Departments.FirstOrDefault(x => x.Id == _id));
            }
            context.SaveChanges();
        }

        /// <summary>
        /// Get Applicant
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        public LoanApplicant GetApplicant(int appId)
        {
            return context.loanApplicants
                .FirstOrDefault(x => x.Id == appId);
        }

        /// <summary>
        /// Get All Applicant
        /// </summary>
        /// <returns></returns>
        public List<LoanApplicant> GetAllApplicants()
        {
            return context.loanApplicants.Where(x => x.LenderType == false)
                .ToList();
        }

        /// <summary>
        /// Get All Applicant
        /// </summary>
        /// <returns></returns>
        public List<LoanApplicant> GetAllLenders()
        {
            return context.loanApplicants.Where(x => x.LenderType == true)
                .ToList();
        }

        /// <summary>
        /// Get All Applicants By Type, Company and Department
        /// </summary>
        /// <returns></returns>
        public List<LoanApplicant> GetApplicantsByTypeCompDept(int typeId, int compId, int deptId)
        {
            return context.loanApplicants
                .Where(x => x.applicantTypeId == typeId && x.companies.FirstOrDefault(y => y.Id == compId) != null && x.departments.FirstOrDefault(z => z.Id == deptId) != null && x.LenderType == false)
                .ToList();
        }

        /// <summary>
        /// Get All Applicants By Type, Company and Department
        /// </summary>
        /// <returns></returns>
        public List<LoanApplicant> GetLendersByTypeCompDept(int typeId, int compId, int deptId)
        {
            return context.loanApplicants
                .Where(x => x.applicantTypeId == typeId && x.companies.FirstOrDefault(y => y.Id == compId) != null && x.departments.FirstOrDefault(z => z.Id == deptId) != null && x.LenderType == true)
                .ToList();
        }

        /// <summary>
        /// Add Applicant Type
        /// </summary>
        /// <param name="applicantType"></param>
        public void AddApplicantType(LoanApplicantType applicantType)
        {
            context.loanApplicantTypes.Add(applicantType);
            context.SaveChanges();
        }

        /// <summary>
        /// Update Existing Applicant Type
        /// </summary>
        /// <param name="applicantType"></param>
        public void UpdateApplicantType(LoanApplicantType applicantType)
        {
            var _applicantType = context.loanApplicantTypes.FirstOrDefault(x => x.Id == applicantType.Id);
            _applicantType = applicantType;
            context.SaveChanges();
        }

        /// <summary>
        /// Get Applicant Type
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public LoanApplicantType GetApplicantType(int typeId)
        {
            return context.loanApplicantTypes.FirstOrDefault(x => x.Id == typeId);
        }

        /// <summary>
        /// Get All Applicant Types
        /// </summary>
        /// <returns></returns>
        public List<LoanApplicantType> GetAllApplicantTypes()
        {
            return context.loanApplicantTypes.Where(x => x.LenderType == false).ToList();
        }

        /// <summary>
        /// Get All Applicant Types
        /// </summary>
        /// <returns></returns>
        public List<LoanApplicantType> GetAllLenderTypes()
        {
            return context.loanApplicantTypes.Where(x => x.LenderType == true).ToList();
        }

        /// <summary>
        /// Add New Loans Advance Status
        /// </summary>
        /// <param name="loansAdvanceStatus"></param>
        public void AddLoansAdvanceStatus(LoansAdvanceStatus loansAdvanceStatus)
        {
            context.loansAdvanceStatuses.Add(loansAdvanceStatus);
            context.SaveChanges();
        }

        /// <summary>
        /// Update Existing Loans Advance Status
        /// </summary>
        /// <param name="loansAdvanceStatus"></param>
        public void UpdateLoansAdvanceStatus(LoansAdvanceStatus loansAdvanceStatus)
        {
            LoansAdvanceStatus _paymentStatus = context.loansAdvanceStatuses.FirstOrDefault(x => x.Id == loansAdvanceStatus.Id);
            _paymentStatus.Status = loansAdvanceStatus.Status;
            _paymentStatus.isActive = loansAdvanceStatus.isActive;
            _paymentStatus.forecolor = loansAdvanceStatus.forecolor;
            _paymentStatus.backcolor = loansAdvanceStatus.backcolor;
            context.SaveChanges();
        }

        /// <summary>
        /// Get A Payment Status
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns></returns>
        public LoansAdvanceStatus GetLoansAdvanceStatus(int statusId)
        {
            return context.loansAdvanceStatuses
                //.Include("payments")
                .FirstOrDefault(x => x.Id == statusId);
        }


        /// <summary>
        /// Get All Payment Statuses
        /// </summary>
        /// <returns></returns>
        public List<LoansAdvanceStatus> GetAllloansAdvanceStatuses()
        {
            return context.loansAdvanceStatuses
                //.Include("payments")
                .ToList();
        }

        public List<LoansAdvanceStatus> GetAllClosedStatus()
        {
            var statusList = context.loansAdvanceStatuses.Where(x => x.isActive == false)
                .ToList();
            return statusList;

        }

        public List<LoansAdvanceStatus> GetAllOpenStatus()
        {
            var statusList = context.loansAdvanceStatuses.Where(x => x.isActive == true)
                .ToList();
            return statusList;

        }


        /// <summary>
        /// get user based on employeeid
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public ERP_BL.Databases.User getuser(int userid)
        {
            return context.Users
                .Include("employee.Companies")
                .Include("employee.Companies.departments")
                .Include("employee.Companies.departments.parentDepartment")
                .Include("employee.departments")
                .FirstOrDefault(x => x.id == userid);

        }

        /// <summary>
        /// Get all Companies
        /// </summary>
        /// <returns>List of Companies Objects</returns>
        public List<ERP_BL.Databases.Company> GetUserCompanies(int id)
        {
            var user = context.Users
                .FirstOrDefault(x => x.id == id);
            return user.employee.Companies;
        }


    }
}
