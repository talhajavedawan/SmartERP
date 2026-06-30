using ERP_BL.Databases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.AssetsRentals.RentalInvoices
{
    public class RentalInvoiceRepo
    {
        DBContextERP context = new DBContextERP();

        public ERP_BL.Databases.Employee GetEmployee(int empID)
        {
            return context.Employees
                .FirstOrDefault(x => x.EmpId == empID);
        }

        /// <summary>
        /// Get last Rental Contract
        /// </summary>
        /// <returns></returns>
        public int GetLastTransactionId()
        {
            var rentalInvoice = context.rentalInvoices.OrderByDescending(q => q.Id).FirstOrDefault();

            if (rentalInvoice == null)
                return 0;
            return rentalInvoice.transactionGroupId;
        }

        /// <summary>
        /// Add new Contract
        /// </summary>
        /// <param name="rentalInvoice"></param>
        public void AddRentalInvoice(RentalInvoice rentalInvoice)
        {
            context.rentalInvoices.Add(rentalInvoice);
            context.SaveChanges();
        }

        /// <summary>
        /// Update Contract
        /// </summary>
        /// <param name="rentalInvoice"></param>
        public void UpdateRentalInvoice(RentalInvoice rentalInvoice)
        {
            var _rentalInvoice = context.rentalInvoices.FirstOrDefault(x => x.Id == rentalInvoice.Id);
            _rentalInvoice = rentalInvoice;
            context.SaveChanges();
        }

        /// <summary>
        /// Get Contract by Id
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <returns></returns>
        public RentalInvoice GetRentalInvoice(int invoiceId)
        {
            return context.rentalInvoices.FirstOrDefault(x => x.Id == invoiceId);
        }

        /// <summary>
        /// Get All Rental Contracts
        /// </summary>
        /// <returns></returns>
        public List<RentalInvoice> GetAllRentalInvoices()
        {
            return context.rentalInvoices.ToList();
        }


        /// <summary>
        /// Get All Rental Contracts
        /// </summary>
        /// <returns></returns>
        public List<RentalInvoice> GetAllActiveRentalInvoices(int uid)
        {

            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.rentalInvoices

           .Where(x => (deptIds.Contains(x.department.Id) || x.creatorId == uid) && companyIds.Contains(x.company.Id) && x.Status.isActive == true && x.isReApproved != false && x.isApproved == true && x.PendingForClosing != true && x.isVoid != true)
           .ToList();


        }



        /// <summary>
        /// Add New Status
        /// </summary>
        /// <param name="rentalInvoiceStatus"></param>
        public void AddRentalInvoiceStatus(RentalInvoiceStatus rentalInvoiceStatus)
        {
            context.rentalInvoiceStatuses.Add(rentalInvoiceStatus);
            context.SaveChanges();
        }

        /// <summary>
        /// Update Existing Status
        /// </summary>
        /// <param name="rentalInvoiceStatus"></param>
        public void UpdateRentalInvoiceStatus(RentalInvoiceStatus rentalInvoiceStatus)
        {
            RentalInvoiceStatus _rentalInvoiceStatus = context.rentalInvoiceStatuses.FirstOrDefault(x => x.Id == rentalInvoiceStatus.Id);
            _rentalInvoiceStatus.Status = rentalInvoiceStatus.Status;
            _rentalInvoiceStatus.isActive = rentalInvoiceStatus.isActive;
            _rentalInvoiceStatus.forecolor = rentalInvoiceStatus.forecolor;
            _rentalInvoiceStatus.backcolor = rentalInvoiceStatus.backcolor;
            context.SaveChanges();
        }

        /// <summary>
        /// Get A Status
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns></returns>
        public RentalInvoiceStatus GetRentalInvoiceStatus(int statusId)
        {
            return context.rentalInvoiceStatuses
                //.Include("payments")
                .FirstOrDefault(x => x.Id == statusId);
        }


        /// <summary>
        /// Get All Statuses
        /// </summary>
        /// <returns></returns>
        public List<RentalInvoiceStatus> GetAllRentalInvoiceStatuses()
        {
            return context.rentalInvoiceStatuses
                //.Include("payments")
                .ToList();
        }

        public List<RentalInvoiceStatus> GetAllClosedStatus()
        {
            var statusList = context.rentalInvoiceStatuses.Where(x => x.isActive == false)
                .ToList();
            return statusList;

        }

        public List<RentalInvoiceStatus> GetAllOpenStatus()
        {
            var statusList = context.rentalInvoiceStatuses.Where(x => x.isActive == true)
                .ToList();
            return statusList;

        }


        /// <summary>
        /// Get Purchase Invoices by Company, Dept and Currency
        /// </summary>
        /// <returns></returns>
        public List<RentalInvoice> GetRentalInvoiceForSR(int compId, List<Department> departments, int currId)
        {
            List<int> dept_Ids = new List<int>();
            foreach (var _dept in departments)
            {
                dept_Ids.Add(_dept.Id);
            }

            return context.rentalInvoices
           .Where(x => x.companyId == compId && x.currencyId == currId && x.Status.isActive == true && x.isApproved == true && x.isVoid != true && dept_Ids.Contains(x.deptId.Value) )
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

            return context.rentalInvoices

                .Where(x => (deptIds.Contains(x.department.Id) || x.creatorId == uid) && companyIds.Contains((int)x.companyId) && x.isApproved == false && x.isVoid != true)
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

            return context.rentalInvoices

                .Where(x => (deptIds.Contains(x.department.Id) || x.creatorId == uid) && companyIds.Contains((int)x.companyId) && (x.Creator.employee.SupervisorId == user.employee.EmpId || x.Creator.employee.EmpId == user.employee.EmpId || x.Creator.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId || x.Creator.employee.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true)
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

            return context.rentalInvoices

                .Where(x => (deptIds.Contains(x.department.Id) || x.creatorId == uid) && companyIds.Contains((int)x.companyId) && (x.Creator.employee.EmpId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true)
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
            return context.rentalInvoices

                .Where(x => (deptIds.Contains(x.department.Id) || x.creatorId == uid) && companyIds.Contains((int)x.companyId) && x.isApproved == true && x.isReApproved == false && x.isVoid != true)
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
            return context.rentalInvoices

                .Where(x => (deptIds.Contains(x.department.Id) || x.creatorId == uid) && companyIds.Contains((int)x.companyId) && (x.Creator.employee.SupervisorId == user.employee.EmpId || x.Creator.employee.EmpId == user.employee.EmpId || x.Creator.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId || x.Creator.employee.Supervisor.SupervisorId == user.employee.EmpId) && x.isReApproved == false && x.isApproved == true && x.isVoid != true)
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
            return context.rentalInvoices

                .Where(x => (deptIds.Contains(x.department.Id) || x.creatorId == uid) && companyIds.Contains((int)x.companyId) && (x.Creator.employee.EmpId == user.employee.EmpId) && x.isReApproved == false && x.isVoid != true && x.isApproved == true)
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
            return context.rentalInvoices
                .Where(x => (deptIds.Contains(x.department.Id) || x.creatorId == uid) && companyIds.Contains((int)x.companyId) && x.isVoid == true)
                .Count();
        }


        /// <summary>
        /// Get all Void Inter-Bank Transfers.
        /// </summary>
        /// <returns></returns>
        public int getVoidRegisterAdministratorCount()
        {
            return context.rentalInvoices
    .Where(x => x.isVoid == true)
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

            return context.rentalInvoices
                .Where(x => (deptIds.Contains(x.department.Id) || x.creatorId == uid) && companyIds.Contains((int)x.companyId) && x.isVoid != true)
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
            return context.rentalInvoices
                .Where(x => (deptIds.Contains(x.department.Id) || x.creatorId == uid) && companyIds.Contains((int)x.companyId) && (x.Creator.employee.EmpId == user.employee.EmpId) && x.isVoid != true)
                .Count();
        }

        /// Get Count Inter-Bank Transfer.
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForAdministratorCount()
        {
            return context.rentalInvoices

                .Where(x => x.isApproved == false && x.isVoid != true)
                .Count();
        }


        /// <summary>
        /// Get all Inter-Bank Transfer .
        /// </summary>
        /// <returns></returns>
        public int getInterBankTransferAdministratorCount()
        {
            return context.rentalInvoices
                .Where(x => x.isVoid != true)
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

            return context.rentalInvoices

                .Where(x => (deptIds.Contains(x.department.Id) || x.creatorId == uid) && companyIds.Contains((int)x.companyId) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
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

            return context.rentalInvoices

                .Where(x => (deptIds.Contains(x.department.Id) || x.creatorId == uid) && companyIds.Contains((int)x.companyId) && (x.Creator.employee.SupervisorId == user.employee.EmpId || x.Creator.employee.EmpId == user.employee.EmpId || x.Creator.employee.Supervisor.SupervisorId == user.employee.EmpId || x.Creator.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
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

            return context.rentalInvoices

                .Where(x => (deptIds.Contains(x.department.Id) || x.creatorId == uid) && companyIds.Contains((int)x.companyId) && (x.Creator.employee.EmpId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .Count();
        }

        /// <summary>
        /// Get Count Inter-Bank Transfers.
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForClosingAdministratorCount()
        {
            return context.rentalInvoices

                .Where(x => x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .Count();
        }

        /// <summary>
        /// Get All Pending For Approval Rental Contracts
        /// </summary>
        /// <returns></returns>
        public List<RentalInvoice> GetAllPendingForApproval(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            return context.rentalInvoices

        .Where(x => (deptIds.Contains(x.department.Id) || x.creatorId == uid) && companyIds.Contains(x.company.Id) && x.isApproved == false && x.isVoid != true)
         .ToList();
        }


        /// <summary>
        /// Rental Contracts Pending for re-approval
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public List<RentalInvoice> getAllPendingForReApprovalDepartmental(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            return context.rentalInvoices

         .Where(x => (deptIds.Contains(x.department.Id) || x.creatorId == uid) && companyIds.Contains(x.company.Id) && x.isReApproved == false && x.isApproved == true && x.isVoid != true)
             .ToList();

        }


        /// <summary>
        /// Get all pending for closing Rental Contracts by Departmental
        /// </summary>
        /// <returns></returns>
        public List<RentalInvoice> getAllPendingForClosingDepartmental(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.rentalInvoices
            .Where(x => (deptIds.Contains(x.department.Id) || x.creatorId == uid) && companyIds.Contains(x.company.Id) && x.PendingForClosing == true && x.isVoid != true)
            .ToList();
        }


        /// <summary>
        /// Get All Rental Contracts
        /// </summary>
        /// <returns></returns>
        public List<RentalInvoice> GetAllRentalInvoices(int uid)
        {

            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
            {
                companyIds.Add(comp.Id);

            }
            return context.rentalInvoices

           .Where(x => (deptIds.Contains(x.department.Id) || x.creatorId == uid) && companyIds.Contains(x.company.Id) && x.isVoid != true)
           .ToList();


        }

        /// <summary>
        /// Get all Void Rental Contract own.
        /// </summary>
        /// <returns></returns>
        public List<RentalInvoice> getVoidRegisterOwn(int uid)
        {

            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();

            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);

            return context.rentalInvoices
         .Where(x => (deptIds.Contains(x.department.Id) || x.creatorId == uid) && companyIds.Contains((int)x.company.Id) && x.isVoid == true)
             .ToList();


        }
    }
}
