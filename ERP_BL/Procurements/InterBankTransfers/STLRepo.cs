using ERP_BL.Databases;
using ERP_BL.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.Procurements.InterBankTransfers
{
   public class STLRepo
    {
        DBContextERP context = new DBContextERP();
        public void Add(STL _stl)
        {
            context.STLs.Add(_stl);
            context.SaveChanges();
        }
        public void update(STL _stl)
        {
            var dbSTL = context.STLs.FirstOrDefault(x => x.Id == _stl.Id);
            context.journalTransactions.RemoveRange(context.journalTransactions.Where(x => x.STLId == _stl.Id));
            dbSTL = _stl;
            context.SaveChanges();
        }
        public STLStatus GetLoansStatus(int statusId)
        {
            return context.STLStatuses
                .FirstOrDefault(x => x.Id == statusId);
        }
        public void AddSTLStatus(STLStatus loansStatus)
        {
            context.STLStatuses.Add(loansStatus);
            context.SaveChanges();
        }
        public void UpdateLoansStatus(STLStatus stlStatus)
        {
            STLStatus _stlStatus = context.STLStatuses.FirstOrDefault(x => x.Id == stlStatus.Id);
            _stlStatus = stlStatus;
            context.SaveChanges();
        }
        public List<STLStatus> GetAllSTLStatuses()
        {
            return context.STLStatuses
                .ToList();
        }
        public List<STLStatus> getAllActiveSTLStatus()
        {
            List<STLStatus> statuses = new List<STLStatus>();
            using (var _DbContext = new DBContextERP())
            {
                //return context.saleOrderStatuses.Include("SaleOrders").Where(x => x.isActive == true).ToList();
                statuses = _DbContext.STLStatuses.Where(x => x.isActive == true).ToList();
            }
            return statuses;
        }
        public List<STLStatus> getAllSTLStatus()
        {
            return context.STLStatuses.ToList();
        }
        public List<STLStatus> GetAllCloseSTLStatus()
        {
            var statusList = context.STLStatuses.Where(x => x.isActive == false)
                .ToList();
            return statusList;

        }
        public List<STLStatus> GetAllOpenSTLStatus()
        {
            var statusList = context.STLStatuses.Where(x => x.isActive == true)
                .ToList();
            return statusList;

        }
        public List<STL> GetAllTransactionsOpenAndClosed(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);

            var stls = context.STLs
                .Where(x => companyIds.Contains(x.company.Id) &&
                 x.isApproved == true && x.isReApproved != false
                 && x.PendingForClosing != true && x.isVoid != true)
                .ToList();

            //List<STL> paymentsToReturn = new List<STL>();
            //foreach (var _payment in stls)
            //{
            //    if (_payment.departments != null && _payment.departments.Count > 0)
            //        foreach (var _dept in _payment.departments)
            //        {
            //            if ((deptIds.Contains(_dept.Id) && companyIds.Contains(_payment.company_Id.Value)) || (_payment.InterDepartment_Id != null ? deptIds.Contains(_payment.InterDepartment_Id.Value) : false && _payment.company_Id != null ? companyIds.Contains(_payment.InterCompany_Id.Value) : false))
            //            {
            //                paymentsToReturn.Add(_payment);
            //                break;
            //            }
            //        }
            //}

            return stls;
        }
        public List<STL> getAllActiveandUnapprovedTransactions(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);

            var stls = context.STLs
                .Where(x => companyIds.Contains(x.company.Id) &&

                 x.stlStatus.isActive == true && x.isApproved == true 
                 && x.isReApproved != false 
                 && x.PendingForClosing != true 
                 && x.isVoid != true)
                .ToList();
            return stls;
        }
        public List<STL> getAllInActiveandUnapprovedReceipts(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);

            var stls = context.STLs

                .Where(x => companyIds.Contains(x.company.Id) 
                && x.stlStatus.isActive != true 
                && x.isReApproved != false 
                && x.isApproved == true 
                && x.PendingForClosing != true 
                && x.isVoid != true)
                .ToList();

         

            return stls;
        }
        public List<STL> getAllSaleReceiptsbyStatusId(int uid, int StatusId)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            var stls = context.STLs
                .Where(x =>companyIds.Contains(x.company.Id) 
                
                && x.stlStatus.Id == StatusId 
                && x.isApproved == true 
                && x.isReApproved != false 
                && x.isVoid != true 
                && x.PendingForClosing != true)
                .ToList();

            

            return stls;
        }
        public int getAllPendingForApprovalDepartmentalCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.STLs

                .Where(x => deptIds.Contains((int)x.dept_Id) && companyIds.Contains((int)x.company_Id) 
                &&
                 x.isApproved == false && x.isVoid != true)
                .Count();
        }
        public int getAllPendingForApprovalCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.STLs

                .Where(x => (deptIds.Contains((int)x.dept_Id) && companyIds.Contains((int)x.company_Id) 
                || x.user.employee.EmpId == user.employee.EmpId
                || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId 
                || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId) 
                && x.isApproved == false && x.isVoid != true)
                .Count();
        }
        public int getAllPendingForReApprovalDepartmentalCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.STLs

                .Where(x => deptIds.Contains((int)x.dept_Id) && companyIds.Contains((int)x.company_Id) 
                &&
                x.isApproved == true && 
                x.isReApproved == false && 
                x.isVoid != true)
                .Count();
        }
        public int getAllPendingForReApprovalCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.STLs

                .Where(x => (deptIds.Contains((int)x.dept_Id) && companyIds.Contains((int)x.company_Id) && 
                (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId) && x.isReApproved == false && x.isApproved == true && x.isVoid != true)
                ).Count();
        }
        public int getAllPendingForApprovalCountOwn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.STLs

                .Where(x => (deptIds.Contains((int)x.dept_Id) && companyIds.Contains((int)x.company_Id) 
              
                && x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true)
                .Count();
        }
        public int getAllPendingForReApprovalCountOwnByCompany(List<Company> compList, int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = user.employee.departments.Select(x => x.Id).ToList();
            List<int> companyId = new List<int>();
            foreach (var v in compList)
            {
                companyId.Add(v.Id);
            }

            return context.STLs
                .Where(x => (deptIds.Contains((int)x.dept_Id) && companyId.Contains((int)x.company_Id) && 
                x.user.employee.EmpId == user.employee.EmpId) && x.isReApproved == false && x.isVoid != true && x.isApproved == true)
                .Count();
        }
        public int getAllPendingForReApprovalCountOwn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.STLs
                .Where(x => (deptIds.Contains((int)x.dept_Id) && companyIds.Contains((int)x.company_Id) &&  (x.user.employee.EmpId == user.employee.EmpId) && x.isReApproved == false && x.isVoid != true && x.isApproved == true))
                .Count();
        }
        public int getVoidRegisterCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();

            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.STLs
                .Where(x => deptIds.Contains((int)x.dept_Id) && companyIds.Contains((int)x.company_Id) 
                && x.isVoid == true)
                .Count();
        }
        public int getVoidRegisterAdministratorCount()
        {
            return context.STLs

    .Where(x => x.isVoid == true)
    .Count();
        }
        public int getSaleRegisterCountOWn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.STLs
                .Where(x => (deptIds.Contains((int)x.dept_Id) && companyIds.Contains((int)x.company_Id) 
                && (x.user.employee.EmpId == user.employee.EmpId) && x.isVoid != true))
                .Count();
        }
        public int getSaleRegisterCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.STLs

                .Where(x => deptIds.Contains((int)x.dept_Id) && companyIds.Contains((int)x.company_Id) 
                 && x.isVoid != true)
                .Count();
        }
        public int getAllPendingForAdministratorCount()
        {
            return context.STLs

                .Where(x => x.isApproved == false && x.isVoid != true)
                .Count();
        }
        public int getSaleRegisterAdministratorCount()
        {
            return context.STLs

    .Where(x => x.isVoid != true)
    .Count();
        }
        public int getAllPendingForClosingDepartmentalCount(int uid)
        {
            var user = context.Users.Include("Employee").FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.STLs

                .Where(x => (deptIds.Contains((int)x.dept_Id) && companyIds.Contains((int)x.company_Id)
                && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true))
                .Count();
        }
        public int getAllPendingForClosingCount(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.STLs

                .Where(x => (deptIds.Contains((int)x.dept_Id) && companyIds.Contains((int)x.company_Id)
                && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                ).Count();
        }
        public int getAllPendingForClosingCountOwn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.STLs

                .Where(x => (deptIds.Contains((int)x.dept_Id) && companyIds.Contains((int)x.company_Id) 
                && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                ).Count();
        }
        public int getAllPendingForClosingAdministratorCount()
        {
            return context.STLs

                .Where(x => x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .Count();
        }
        public List<STL> getAll()
        {
            return context.STLs

            .Where(x => x.isVoid != true)
                .ToList();
        }
        public List<STL> getAll(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            companyIds.AddRange(user.employee.Companies.Select(x => x.Id).Distinct().ToList());
            deptIds.AddRange(user.employee.departments.Select(x => x.Id).Distinct().ToList());
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.STLs


                .Where(x => (deptIds.Contains((int)x.dept_Id) && companyIds.Contains((int)x.company_Id) 
                
                && x.isApproved == true && x.isVoid != true))
                .ToList();
        }
        public List<STL> getAllActive(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.STLs

                .Where(x => (deptIds.Contains((int)x.dept_Id) && companyIds.Contains((int)x.company_Id) 
                 && x.stlStatus.isActive == true && x.isApproved == true && x.isVoid != true))
                .ToList();
        }
        public List<STL> getAllInActive(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.STLs

                .Where(x => (deptIds.Contains((int)x.dept_Id) && companyIds.Contains((int)x.company_Id)
                 && x.stlStatus.isActive == false && x.isApproved == true && x.isVoid != true))
                .ToList();
        }
        public List<STL> getAllPendingForApprovalDepartmental(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            return context.STLs
                .Where(x => deptIds.Contains((int)x.dept_Id) && companyIds.Contains((int)x.company_Id)
                 && x.isApproved == false && x.isVoid != true)
                .ToList();
        }
        public List<STL> getAllPendingForApproval(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.STLs

                .Where(x => deptIds.Contains((int)x.dept_Id) && companyIds.Contains((int)x.company_Id) 
                 && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true)
                .ToList();
        }
        public List<STL> getAllPendingForApprovalOwn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.STLs

                .Where(x => (deptIds.Contains((int)x.dept_Id) && companyIds.Contains((int)x.company_Id) 
                 && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true))
                .ToList();
        }
        public List<STL> getAllPendingForAdministrator()
        {
            return context.STLs

                .Where(x => x.isApproved == false && x.isVoid != true)
                .ToList();
        }
        public List<STL> getAllPendingForClosingDepartmental(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            return context.STLs
                .Where(x => (deptIds.Contains((int)x.dept_Id) && companyIds.Contains((int)x.company_Id) 
                && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true))
                .ToList();
        }
        public List<STL> getAllPendingForClosing(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            return context.STLs

                .Where(x => (deptIds.Contains((int)x.dept_Id) 
                && companyIds.Contains((int)x.company_Id) 
                && x.user.employee.SupervisorId == user.employee.EmpId 
                && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true))
                .ToList();
        }
        public List<STL> getAllPendingForClosingOwn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            return context.STLs
                .Where(x => (deptIds.Contains((int)x.dept_Id) && companyIds.Contains((int)x.company_Id) 
                && (x.user.employee.EmpId == user.employee.EmpId) && x.PendingForClosing == true && x.isVoid != true))
                .ToList();
        }
        public List<STL> getAllPendingForClosingAdministrator()
        {
            return context.STLs

                .Where(x => x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .ToList();
        }
        public List<STL> getAllbyStatusId(int uid, int StatusId)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.STLs

                .Where(x => (deptIds.Contains((int)x.dept_Id) && companyIds.Contains((int)x.company_Id) 
                && x.stlStatus.Id == StatusId && x.isApproved == true && x.isVoid != true))
                .ToList();
        }
        public STL Get(int stlId)
        {
            return context.STLs.FirstOrDefault(x => x.Id == stlId);
        }
        public STL GetbyGroupId(int stlGroupId)
        {
            return context.STLs.FirstOrDefault(x => x.paymentGroupId == stlGroupId);
        }
        public void Approve(STL _stl)
        {
            STL stltoUpdate = context.STLs.FirstOrDefault(x => x.Id == _stl.Id);
            stltoUpdate.isApproved = _stl.isApproved;
            stltoUpdate.stage = _stl.stage;
            context.SaveChanges();
        }
        public void updateStatusById(int stlId, STLStatus status)
        {
            STL stltoUpdate = context.STLs.FirstOrDefault(x => x.Id == stlId);
            var STLStatus = context.STLStatuses.FirstOrDefault(x => x.Id == status.Id);
            stltoUpdate.stlStatus = STLStatus;
            context.SaveChanges();
        }
        public void addStatus(STLStatus status)
        {
            context.STLStatuses.Add(status);
            context.SaveChanges();
        }
        public List<STLStatus> getAllInActiveSTLStatus()
        {
            List<STLStatus> statuses = new List<STLStatus>();
            using (var _DbContext = new DBContextERP())
            {
                var st = _DbContext.STLStatuses.Where(x => x.isActive == false);
                if (st != null)
                    statuses = st.ToList();
            }
            return statuses;
        }
        public STLStatus getstatus(int stlstatusid)
        {
            return context.STLStatuses.FirstOrDefault(x => x.Id == stlstatusid);
        }
        public List<STL> getSTLRegister(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.STLs

                .Where(x => deptIds.Contains((int)x.dept_Id) 
                && companyIds.Contains((int)x.company_Id)  && x.isVoid != true
                )
                .ToList();
        }
        public List<STL> getSTLAdministrator()
        {
            return context.STLs

            .Where(x =>  x.isVoid != true)
            .ToList();
        }
        public List<STL> getAllPendingForReApprovalDepartmental(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.STLs

                .Where(x => deptIds.Contains((int)x.dept_Id) && companyIds.Contains((int)x.company_Id)  && x.isReApproved == false && x.isApproved == true && x.isVoid != true)
                .ToList();
        }
        public List<STL> getAllPendingForReApproval(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.STLs

                .Where(x => deptIds.Contains((int)x.dept_Id) && companyIds.Contains((int)x.company_Id)  && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isReApproved == false && x.isApproved == true && x.isVoid != true)
                .ToList();
        }
        public List<STL> getAllPendingForReApprovalOwn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.STLs

                .Where(x => deptIds.Contains((int)x.dept_Id) && companyIds.Contains((int)x.company_Id) &&(x.user.employee.EmpId == user.employee.EmpId) && x.isReApproved == false && x.isVoid != true && x.isApproved == true)
                .ToList();
        }
        public List<STL> getVoidRegister(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.STLs

                .Where(x => deptIds.Contains((int)x.dept_Id) && companyIds.Contains((int)x.company_Id)  && x.isVoid == true)
                .ToList();
        }
        public List<STL> getVoidRegisterOwn(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();

            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);

            return context.STLs

                .Where(x => deptIds.Contains((int)x.dept_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid == true && (x.user.employee.EmpId == user.employee.EmpId))
                .ToList();
        }
        public List<STL> getVoidRegisterAdministrator()
        {
            return context.STLs.Where(x => x.isVoid == true).ToList();
        }
        public List<STL> getCashFlowAllActive(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.STLs

                .Where(x => deptIds.Contains((int)x.dept_Id) 
                && companyIds.Contains((int)x.company_Id)
                && x.stlStatus.isActive == true
                && x.isVoid != true
                && x.settlmentBalance!=0
               
               )
                .ToList();
        }
    }
}
