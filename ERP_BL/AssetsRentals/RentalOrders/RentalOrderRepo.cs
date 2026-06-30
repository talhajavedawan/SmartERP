using ERP_BL.AssetsRentals.RentalOrders;
using ERP_BL.Databases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.AssetsRentals.RentalOrders
{
    public class RentalOrderRepo
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
            var rentalOrder = context.rentalOrders.OrderByDescending(q => q.Id).FirstOrDefault();

            if (rentalOrder == null)
                return 0;
            return rentalOrder.transactionGroupId;
        }

        /// <summary>
        /// Add new Contract
        /// </summary>
        /// <param name="rentalOrder"></param>
        public void AddRentalOrder(RentalOrder rentalOrder)
        {
            context.rentalOrders.Add(rentalOrder);
            context.SaveChanges();
        }

        /// <summary>
        /// Update Contract
        /// </summary>
        /// <param name="rentalOrder"></param>
        public void UpdateRentalOrder(RentalOrder rentalOrder)
        {
            var _rentalOrder = context.rentalOrders.FirstOrDefault(x => x.Id == rentalOrder.Id);
            _rentalOrder = rentalOrder;
            context.SaveChanges();
        }

        /// <summary>
        /// Get Contract by Id
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public RentalOrder GetRentalOrder(int orderId)
        {
            return context.rentalOrders.FirstOrDefault(x => x.Id == orderId);
        }

        /// <summary>
        /// Get All Rental Contracts
        /// </summary>
        /// <returns></returns>
        public List<RentalOrder> GetAllRentalOrders()
        {
            return context.rentalOrders.ToList();
        }


        /// <summary>
        /// Get All Rental Contracts
        /// </summary>
        /// <returns></returns>
        public List<RentalOrder> GetAllActiveRentalOrders(int uid)
        {

            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.rentalOrders

           .Where(x => (deptIds.Contains(x.department.Id) || x.creatorId == uid) && companyIds.Contains(x.company.Id) && x.Status.isActive == true && x.isReApproved != false && x.isApproved == true && x.PendingForClosing != true && x.isVoid != true)
           .ToList();


        }



        /// <summary>
        /// Add New Status
        /// </summary>
        /// <param name="rentalOrderStatus"></param>
        public void AddRentalOrderStatus(RentalOrderStatus rentalOrderStatus)
        {
            context.rentalOrderStatuses.Add(rentalOrderStatus);
            context.SaveChanges();
        }

        /// <summary>
        /// Update Existing Status
        /// </summary>
        /// <param name="rentalOrderStatus"></param>
        public void UpdateRentalOrderStatus(RentalOrderStatus rentalOrderStatus)
        {
            RentalOrderStatus _rentalOrderStatus = context.rentalOrderStatuses.FirstOrDefault(x => x.Id == rentalOrderStatus.Id);
            _rentalOrderStatus.Status = rentalOrderStatus.Status;
            _rentalOrderStatus.isActive = rentalOrderStatus.isActive;
            _rentalOrderStatus.forecolor = rentalOrderStatus.forecolor;
            _rentalOrderStatus.backcolor = rentalOrderStatus.backcolor;
            context.SaveChanges();
        }

        /// <summary>
        /// Get A Status
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns></returns>
        public RentalOrderStatus GetRentalOrderStatus(int statusId)
        {
            return context.rentalOrderStatuses
                //.Include("payments")
                .FirstOrDefault(x => x.Id == statusId);
        }


        /// <summary>
        /// Get All Statuses
        /// </summary>
        /// <returns></returns>
        public List<RentalOrderStatus> GetAllRentalOrderStatuses()
        {
            return context.rentalOrderStatuses
                //.Include("payments")
                .ToList();
        }

        public List<RentalOrderStatus> GetAllClosedStatus()
        {
            var statusList = context.rentalOrderStatuses.Where(x => x.isActive == false)
                .ToList();
            return statusList;

        }

        public List<RentalOrderStatus> GetAllOpenStatus()
        {
            var statusList = context.rentalOrderStatuses.Where(x => x.isActive == true)
                .ToList();
            return statusList;

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

            return context.rentalOrders

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

            return context.rentalOrders

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

            return context.rentalOrders

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
            return context.rentalOrders

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
            return context.rentalOrders

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
            return context.rentalOrders

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
            return context.rentalOrders
                .Where(x => (deptIds.Contains(x.department.Id) || x.creatorId == uid) && companyIds.Contains((int)x.companyId) && x.isVoid == true)
                .Count();
        }


        /// <summary>
        /// Get all Void Inter-Bank Transfers.
        /// </summary>
        /// <returns></returns>
        public int getVoidRegisterAdministratorCount()
        {
            return context.rentalOrders
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

            return context.rentalOrders
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
            return context.rentalOrders
                .Where(x => (deptIds.Contains(x.department.Id) || x.creatorId == uid) && companyIds.Contains((int)x.companyId) && (x.Creator.employee.EmpId == user.employee.EmpId) && x.isVoid != true)
                .Count();
        }

        /// Get Count Inter-Bank Transfer.
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForAdministratorCount()
        {
            return context.rentalOrders

                .Where(x => x.isApproved == false && x.isVoid != true)
                .Count();
        }


        /// <summary>
        /// Get all Inter-Bank Transfer .
        /// </summary>
        /// <returns></returns>
        public int getInterBankTransferAdministratorCount()
        {
            return context.rentalOrders
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

            return context.rentalOrders

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

            return context.rentalOrders

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

            return context.rentalOrders

                .Where(x => (deptIds.Contains(x.department.Id) || x.creatorId == uid) && companyIds.Contains((int)x.companyId) && (x.Creator.employee.EmpId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .Count();
        }

        /// <summary>
        /// Get Count Inter-Bank Transfers.
        /// </summary>
        /// <returns></returns>
        public int getAllPendingForClosingAdministratorCount()
        {
            return context.rentalOrders

                .Where(x => x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .Count();
        }

        /// <summary>
        /// Get All Pending For Approval Rental Contracts
        /// </summary>
        /// <returns></returns>
        public List<RentalOrder> GetAllPendingForApproval(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            return context.rentalOrders

        .Where(x => (deptIds.Contains(x.department.Id) || x.creatorId == uid) && companyIds.Contains(x.company.Id) && x.isApproved == false && x.isVoid != true)
         .ToList();
        }


        /// <summary>
        /// Rental Contracts Pending for re-approval
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public List<RentalOrder> getAllPendingForReApprovalDepartmental(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            return context.rentalOrders

         .Where(x => (deptIds.Contains(x.department.Id) || x.creatorId == uid) && companyIds.Contains(x.company.Id) && x.isReApproved == false && x.isApproved == true && x.isVoid != true)
             .ToList();

        }


        /// <summary>
        /// Get all pending for closing Rental Contracts by Departmental
        /// </summary>
        /// <returns></returns>
        public List<RentalOrder> getAllPendingForClosingDepartmental(int uid)
        {
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.rentalOrders
            .Where(x => (deptIds.Contains(x.department.Id) || x.creatorId == uid) && companyIds.Contains(x.company.Id) && x.PendingForClosing == true && x.isVoid != true)
            .ToList();
        }


        /// <summary>
        /// Get All Rental Contracts
        /// </summary>
        /// <returns></returns>
        public List<RentalOrder> GetAllRentalOrders(int uid)
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
            return context.rentalOrders

           .Where(x => (deptIds.Contains(x.department.Id) || x.creatorId == uid) && companyIds.Contains(x.company.Id) && x.isVoid != true)
           .ToList();


        }

        /// <summary>
        /// Get all Void Rental Contract own.
        /// </summary>
        /// <returns></returns>
        public List<RentalOrder> getVoidRegisterOwn(int uid)
        {

            var user = context.Users.FirstOrDefault(x => x.id == uid);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();

            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);

            return context.rentalOrders
         .Where(x => (deptIds.Contains(x.department.Id) || x.creatorId == uid) && companyIds.Contains((int)x.company.Id) && x.isVoid == true)
             .ToList();


        }
    }
}
